using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BtrieveWrapper.Orm.Models.Generator
{
    class Arguments
    {
        public const string HelpMessage =
@"[usage]
BtrieveWrapper.Orm.Models.Generator
    --input=<source path>
    --output=<destination path>
    [--mode=<mode>]
    [--btrvlib=<btrieve library path>]
    [--owner-name=<owner name>]
    [--import=<path1>[;<path2>;...;<pathn>]]
    [--template-client=<code template path>]
    [--template-record=<code template path>]
    [--name=<model name>]
    [--namespace=<namespace>]
    [--ext-code=<extension>]
    [--search-pattern=<wild card>]
    [--silent]
[parameter]
    --input, -i
        Set input file, directory or URI.

        such as
        --input=btrv://127.0.0.1/Demodata
        --input=""C:\ProgramData\Pervasive Software\PSQL\Demodata""
        --input=model.xml
        
    --output, -o
        Set output file path.

        such as
        --output=demo_model_from_dir.xml
        --output=""C:\ProgramData\Pervasive Software\PSQL\Demodata\model.xml""
        --output=.\

[optional]
    --mode
        Set operation name or number.Default is 0.
        
        operation list:
        0. GenerateModelFromDdf
        1. GenerateModelFromDirectory
        2. GenerateCodeFromModel

    --btrvlib
        Set Btrieve API library path. Default is ""w3btrv7.dll""

    --owner-name
        Set owner name to open btrieve files. Default is empty.

    --import
        Set necessary .Net libraries paths.
        
        such as
        --import=Demo.Converters.dll;""C:\Demo\bin\Demo.dll""

    --template-client
        Set a path of DbClient code template. Default is ""CSharpDbClientTemplate""

    --template-record
        Set a path of Record and RecordManager code template. Default is ""CSharpRecordTemplate""

    --name
        Set a name of model. Default is ""Model""

    --namespace
        Set a namespace of output code. Default is ""BtrieveWrapper.Orm.CustomModels""

    --ext-code=<extension>
        Set extention string of output code file in mode 2. Default is "".cs""

    --silent, -s
        If this option is valid, this program overwrites output files without confirming.

    --search-pattern
        This option is used, when this program searches files in mode 1. Default is ""*""

    --help, -h, -?
        Display help message.
";

        static readonly Regex RegexInput = new Regex(@"^(--input|-i)=(?<p>.*)$", RegexOptions.Compiled);
        static readonly Regex RegexOutput = new Regex(@"^(--output|-o)=(?<p>.*)$", RegexOptions.Compiled);
        static readonly Regex RegexMode = new Regex(@"^--mode=(?<p>.*)$", RegexOptions.Compiled);
        static readonly Regex RegexBtrieveLibrary = new Regex(@"^--btrvlib=(?<p>.*)$", RegexOptions.Compiled);
        static readonly Regex RegexOwnerName = new Regex(@"^--owner-name=(?<p>.*)$", RegexOptions.Compiled);
        static readonly Regex RegexImport = new Regex(@"^--import=(?<p>.*)$", RegexOptions.Compiled);
        static readonly Regex RegexTemplateClient = new Regex(@"^--template-client=(?<p>.*)$", RegexOptions.Compiled);
        static readonly Regex RegexTemplateRecord = new Regex(@"^--template-record=(?<p>.*)$", RegexOptions.Compiled);
        static readonly Regex RegexName = new Regex(@"^--name=(?<p>.*)$", RegexOptions.Compiled);
        static readonly Regex RegexNamespace = new Regex(@"^--namespace=(?<p>.*)$", RegexOptions.Compiled);
        static readonly Regex RegexExtCode = new Regex(@"^--ext-code=(?<p>.*)$", RegexOptions.Compiled);
        static readonly Regex RegexSilent = new Regex(@"^(--silent|-s)$", RegexOptions.Compiled);
        static readonly Regex RegexSearchPattern = new Regex(@"^--search-pattern=(?<p>.*)$", RegexOptions.Compiled);
        static readonly Regex RegexHelp = new Regex(@"^(--help|-h|-\?)$", RegexOptions.Compiled);

        Arguments() { }

        public static Arguments Parse(string[] args) {
            if (args.Any(a => RegexHelp.IsMatch(a))) {
                WriteHelpMessage();
                return null;
            }
            var result = new Arguments();
            result.Assemblies=new List<System.Reflection.Assembly>();
            var arg = args.SingleOrDefault(a => RegexMode.IsMatch(a));
            if (arg == null) {
                result.Mode = OperationMode.GenerateModelFromDdf;
            } else {
                var match = RegexMode.Match(arg);
                var parameter = match.Groups["p"].Value;
                result.Mode = (OperationMode)Enum.Parse(typeof(OperationMode), parameter, true);
            }
            arg = args.Single(a => RegexInput.IsMatch(a));
            {
                var match = RegexInput.Match(arg);
                result.Input = match.Groups["p"].Value;
            }
            arg = args.SingleOrDefault(a => RegexOutput.IsMatch(a));
            if (arg != null) {
                var match = RegexOutput.Match(arg);
                result.Output = match.Groups["p"].Value;
            } else {
                switch (result.Mode) {
                    case OperationMode.GenerateModelFromDdf:
                    case OperationMode.GenerateModelFromDirectory:
                        result.Output = "model.xml";
                        break;
                    case OperationMode.GenerateCodeFromModel:
                        result.Output = Environment.CurrentDirectory;
                        break;
                }
            }
            arg = args.SingleOrDefault(a => RegexBtrieveLibrary.IsMatch(a));
            if (arg == null) {
                result.BtrieveLibrary = null;
            } else {
                var match = RegexBtrieveLibrary.Match(arg);
                result.BtrieveLibrary = match.Groups["p"].Value;
            }
            arg = args.SingleOrDefault(a => RegexOwnerName.IsMatch(a));
            if (arg == null) {
                result.OwnerName = null;
            } else {
                var match = RegexOwnerName.Match(arg);
                result.OwnerName = match.Groups["p"].Value;
            }
            arg = args.SingleOrDefault(a => RegexImport.IsMatch(a));
            if (arg == null) {
                result.ImportLibraries = new string[0];
            } else {
                var match = RegexImport.Match(arg);
                result.ImportLibraries = match.Groups["p"].Value.Split(';');
                foreach (var assembly in result.ImportLibraries) {
                        result.Assemblies.Add(System.Reflection.Assembly.LoadFile(assembly));
                }
            }
            arg = args.SingleOrDefault(a => RegexTemplateClient.IsMatch(a));
            if (arg == null) {
                result.ClientTemplate = "CSharpDbClientTemplate";
            } else {
                var match = RegexTemplateClient.Match(arg);
                result.ClientTemplate = match.Groups["p"].Value;
            }
            arg = args.SingleOrDefault(a => RegexTemplateRecord.IsMatch(a));
            if (arg == null) {
                result.RecordTemplate = "CSharpRecordTemplate";
            } else {
                var match = RegexTemplateRecord.Match(arg);
                result.RecordTemplate = match.Groups["p"].Value;
            }
            arg = args.SingleOrDefault(a => RegexName.IsMatch(a));
            if (arg == null) {
                result.Name = "Model";
            } else {
                var match = RegexName.Match(arg);
                result.Name = match.Groups["p"].Value;
            }
            arg = args.SingleOrDefault(a => RegexNamespace.IsMatch(a));
            if (arg != null) {
                var match = RegexNamespace.Match(arg);
                result.Namespace = match.Groups["p"].Value;
            }
            arg = args.SingleOrDefault(a => RegexExtCode.IsMatch(a));
            if (arg == null) {
                result.CodeExtension = ".cs";
            } else {
                var match = RegexExtCode.Match(arg);
                result.CodeExtension = match.Groups["p"].Value;
            }
            result.Silent = args.Any(a => RegexSilent.IsMatch(a));
            arg = args.SingleOrDefault(a => RegexSearchPattern.IsMatch(a));
            if (arg == null) {
                result.SearchPattern = "*";
            } else {
                var match = RegexSearchPattern.Match(arg);
                result.SearchPattern = match.Groups["p"].Value;
            }
            return result;
        }

        public OperationMode Mode { get; private set; }
        public string Input { get; private set; }
        public string Output { get; private set; }
        public string BtrieveLibrary { get; private set; }
        public string OwnerName { get; private set; }
        public string[] ImportLibraries { get; private set; }
        public string ClientTemplate { get; private set; }
        public string RecordTemplate { get; private set; }
        public string Name { get; private set; }
        public string Namespace { get; private set; }
        public string CodeExtension { get; private set; }
        public bool Silent { get; private set; }
        public string SearchPattern { get; private set; }
        public List<System.Reflection.Assembly> Assemblies { get; private set; }
        public Path Uri {
            get {
                var match = Regex.Match(this.Input, @"^btrv://((?<user>[a-zA-Z0-9\-_]+)@)?(?<host>([a-zA-Z0-9-]+|[0-9.]+))/(?<dbname>[a-zA-Z0-9]+)(\?(pwd=(?<pwd>[a-zA-Z0-9-_]+)|prompt=(?<prompt>yes|no))+)?$");
                if (!match.Success) {
                    return null;
                }
                var host = match.Groups["host"].Value;
                var dbName = match.Groups["dbname"].Value;
                var user = match.Groups["user"].Success ? match.Groups["user"].Value : null;
                var pwd = match.Groups["pwd"].Success ? match.Groups["pwd"].Value : null;
                bool? prompt =null;
                if (match.Groups["prompt"].Success) {
                    prompt = match.Groups["pwd"].Value == "yes" ? true : false;
                }
                return Path.Uri(host, dbName, uriUser: user, uriPassword: pwd, uriPrompt: prompt);
            }
        }

        public static void WriteHelpMessage(){
            Console.WriteLine(HelpMessage);
        }
    }
}
