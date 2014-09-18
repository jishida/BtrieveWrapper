using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace BtrieveWrapper.Orm.Models.Template
{
    class BlockParser
    {
        class ParserContext
        {
            public ParserContext(object context) {
                this.Context = context;
                this.Dictionary = new Dictionary<string, object>();
            }

            public object Context { get; private set; }
            public Dictionary<string, object> Dictionary { get; private set; }

            public override string ToString() {
                return this.Context.ToString();
            }
        }

        class IfParameter
        {
            public object Member { get; set; }
            public string Value { get; set; }
            public string Block { get; set; }
            public bool IsElse { get; set; }
        }

        enum BlockState
        {
            If,
            For
        }

        
        public BlockParser(object context, string name) : this(context, name, null) { }

        protected BlockParser(object context, string name, IEnumerable<BlockParser> parsers) {
            if (context == null || name == null) {
                throw new ArgumentNullException();
            }
            if (!Regex.IsMatch(name, Configurations.RootMemberRegexPattern)) {
                throw new ArgumentException();
            }
            this.Context = context;
            this.Name = name;
            this._parsers = parsers ?? new BlockParser[0];
            if (this._parsers.Any(p => p.Name == this.Name)) {
                throw new ArgumentException();
            }
        }

        string Name { get; set; }
        object Context { get; set; }
        IEnumerable<BlockParser> _parsers;
        IEnumerable<BlockParser> Parsers {
            get {
                foreach (var parser in this._parsers) {
                    yield return parser;
                }
                yield return this;
            }
        }

        object GetMemberValue(string member) {
            var members = member.Split('.');
            object context=null;

            for (var i = 0; i < members.Length; i++) {
                if (i == 0) {
                   var parser = this.Parsers.SingleOrDefault(p => p.Name == members[i]);
                    if (parser == null) {
                        context = BlockParser.GetValue(this.Parsers.First().Context, members[i]);
                    } else {
                        context = parser.Context;
                    }
                } else {
                    context = BlockParser.GetValue(context, members[i]);
                }
            }
            return context;
        }

        static object GetValue(object obj, string name) {
            var context = obj as ParserContext;
            if (context != null) {
                obj = context.Context;
                if (context.Dictionary.ContainsKey(name)) {
                    return context.Dictionary[name];
                }
            }
            var type = obj.GetType();
            var property = type.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.MemberType == MemberTypes.Property)
                .Select(m => (PropertyInfo)m)
                .SingleOrDefault(p => p.Name == name);
            if (property != null) {
                return property.GetValue(obj, null);
            }
           var field = type.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.MemberType == MemberTypes.Field)
                .Select(m => (System.Reflection.FieldInfo)m)
                .SingleOrDefault(f => f.Name == name);
           if (field != null) {
               return field.GetValue(obj);
           }
           var method = type.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.MemberType == MemberTypes.Method)
                .Select(m => (MethodInfo)m)
                .SingleOrDefault(m => m.Name == name && m.GetParameters().Length == 0);
           if (method != null) {
               return method.Invoke(obj, null);
           }
           throw new ConstructionException();
        }

        public string Parse(string block) {
            var resultBuilder = new StringBuilder();
            var blockBuilder = new StringBuilder();
            string forItem = null;
            System.Collections.IEnumerable forMembers = null;
            var ifParameters = new List<IfParameter>();
            var blockStates = new Stack<BlockState>();

            for (; ; ) {
                var match = BlockMatcher.GetMatch(block);

                if (match == null) {
                    if (blockStates.Count != 0) {
                        throw new ConstructionException();
                    }
                    resultBuilder.Append(block);
                    return resultBuilder.ToString().UnescapeTemplate();
                }
                if (blockStates.Count == 0) {
                    resultBuilder.Append(block.Substring(0, match.Match.Index));
                } else {
                    blockBuilder.Append(block.Substring(0, match.Match.Index));
                }
                block = block.Substring(match.Match.Index + match.Match.Length, block.Length - match.Match.Index - match.Match.Length);
                switch (match.Type) {
                    case BlockType.Tag:
                        if (blockStates.Count == 0) {
                            var member=match.Match.Groups["member"];
                            if (member.Success) {
                                resultBuilder.Append(this.GetMemberValue(member.Value));
                            }else{
                                throw new InvalidOperationException();
                            }
                        } else {
                            blockBuilder.Append(match.Match.Value);
                        }
                        break;
                    case BlockType.If:
                        if (blockStates.Count == 0) {
                            ifParameters.Clear();
                            blockBuilder = new StringBuilder();

                            var member = match.Match.Groups["member"];
                            var value = match.Match.Groups["value"];
                            var parameter = new IfParameter();
                            parameter.Member = this.GetMemberValue(member.Value);
                            parameter.Value = value.Success ? value.Value.UnescapeValue() : null;
                            parameter.IsElse = false;
                            ifParameters.Add(parameter);
                        } else {
                            blockBuilder.Append(match.Match.Value);
                        }
                        blockStates.Push(BlockState.If);
                        break;
                    case BlockType.ElseIf:
                        if (blockStates.Count == 0 || blockStates.Peek() != BlockState.If) {
                            throw new ConstructionException();
                        } else if (blockStates.Count == 1) {
                            ifParameters.Last().Block = blockBuilder.ToString();
                            blockBuilder = new StringBuilder();

                            var member = match.Match.Groups["member"];
                            var value = match.Match.Groups["value"];
                            var parameter = new IfParameter();
                            parameter.Member = this.GetMemberValue(member.Value);
                            parameter.Value = value.Success ? value.Value.UnescapeValue() : null;
                            parameter.IsElse = false;
                            ifParameters.Add(parameter);
                        } else {
                            blockBuilder.Append(match.Match.Value);
                        }
                        break;
                    case BlockType.Else:
                        if (blockStates.Count == 0 || blockStates.Peek() != BlockState.If) {
                            throw new ConstructionException();
                        } else if (blockStates.Count == 1) {
                            ifParameters.Last().Block = blockBuilder.ToString();
                            blockBuilder = new StringBuilder();

                            var parameter = new IfParameter();
                            parameter.Member = null;
                            parameter.Value = null;
                            parameter.IsElse = true;
                            ifParameters.Add(parameter);
                        } else {
                            blockBuilder.Append(match.Match.Value);
                        }
                        break;
                    case BlockType.EndIf:
                        if (blockStates.Count == 0 || blockStates.Peek() != BlockState.If) {
                            throw new ConstructionException();
                        } else if (blockStates.Count == 1) {
                            ifParameters.Last().Block = blockBuilder.ToString();
                            blockBuilder = null;

                            foreach (var ifParameter in ifParameters) {
                                var matched = false;
                                if (ifParameter.IsElse) {
                                    matched = true;
                                } else if (ifParameter.Value == null) {
                                    if (ifParameter.Member != null &&
                                        !ifParameter.Member.Equals(ifParameter.Member.GetType().GetDefault())) {
                                        matched = true;
                                    }
                                } else {
                                    if (ifParameter.Member != null &&
                                        ifParameter.Member.ToString() == ifParameter.Value) {
                                        matched = true;
                                    }
                                }
                                if (matched) {
                                    var parser = new BlockParser(this.Context, this.Name, this._parsers);
                                    resultBuilder.Append(parser.Parse(ifParameter.Block));
                                    break;
                                }
                            }
                        } else {
                            blockBuilder.Append(match.Match.Value);
                        }
                        blockStates.Pop();
                        break;
                    case BlockType.For:
                        if (blockStates.Count == 0) {
                            blockBuilder = new StringBuilder();

                            var item = match.Match.Groups["item"];
                            var list = match.Match.Groups["list"];
                            forItem = item.Value;
                            forMembers =this.GetMemberValue( list.Value) as System.Collections.IEnumerable;
                        } else {
                            blockBuilder.Append(match.Match.Value);
                        }
                        blockStates.Push(BlockState.For);
                        break;
                    case BlockType.EndFor:
                        if (blockStates.Count == 0 || blockStates.Peek() != BlockState.For) {
                            throw new ConstructionException();
                        } else if (blockStates.Count == 1) {
                            if (forMembers != null) {
                                var forBlock = blockBuilder.ToString();
                                var members=new List<object>();
                                foreach(var forMember in forMembers){
                                    members.Add(forMember);
                                }
                                for (var i = 0;i<members.Count;i++) {
                                    var parsers=this._parsers.ToList();
                                    parsers.Add(this);
                                    var context = new ParserContext(members[i]);
                                    context.Dictionary["IsFirst"] = i == 0;
                                    context.Dictionary["IsLast"] = i == members.Count - 1;
                                    context.Dictionary["ForIndex"] = i;
                                    context.Dictionary["ForCount"] = members.Count;
                                    var parser = new BlockParser(context, forItem, parsers);
                                    resultBuilder.Append(parser.Parse(forBlock));
                                }
                            }
                        } else {
                            blockBuilder.Append(match.Match.Value);
                        }
                        blockStates.Pop();
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

        }
    }
}
