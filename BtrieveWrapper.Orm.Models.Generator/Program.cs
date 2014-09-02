using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper.Orm.Models.Generator
{
    class Program
    {
        static int Main(string[] args) {
#if DEBUG
#else
            try {
#endif
                Arguments arguments;
                try {
                    arguments = Arguments.Parse(args);
                } catch {
                    throw new ArgumentException();
                }
                foreach (var assemly in arguments.Assemblies) {
                    foreach (var type in assemly.GetTypes()) {
                        if (type.GetInterfaces().Any(i => i == typeof(IModelInitializable))) {
                            var initializable = (IModelInitializable)Activator.CreateInstance(type);
                            initializable.Initialize();
                        }
                    }
                }
                switch (arguments.Mode) {
                    case OperationMode.GenerateModelFromDdf: {
                            Console.WriteLine("Mode {0}", arguments.Mode);
                            var uri = arguments.Uri;
                            if (uri == null) {
                                throw new ArgumentException();
                            }
                            var model = Model.FromDdf(uri.UriHost, uri.UriDbName, uri.UriUser, uri.UriPassword, uri.UriPrompt, null, arguments.BtrieveLibrary, arguments.DependencyLibraries);
                            if (arguments.Namespace != null) {
                                model.Namespace = arguments.Namespace;
                            }
                            if (System.IO.File.Exists(arguments.Output) && !arguments.Silent) {
                                for (; ; ) {
                                    Console.WriteLine(@"""{0}"" already exists. Overwrite it? (y/n)", arguments.Output);
                                    var line = Console.ReadLine();
                                    if (line == "y") {
                                        break;
                                    } else if (line == "n") {
                                        Console.WriteLine("Operation was canceled.");
                                        return 1;
                                    }
                                    Console.WriteLine("Invalid input text.");
                                }
                            }
                            using (var stream = new System.IO.FileStream(arguments.Output, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                                Model.ToXml(model, stream);
                            }
                        }
                        break;
                    case OperationMode.GenerateModelFromDirectory: {
                            Console.WriteLine("Mode {0}", arguments.Mode);
                            var inputDirectory = new System.IO.DirectoryInfo(arguments.Input);
                            var model = Model.FromDirectory(arguments.Name, inputDirectory.FullName, arguments.SearchPattern, System.IO.SearchOption.AllDirectories, null, arguments.BtrieveLibrary, arguments.DependencyLibraries);
                            if (arguments.Namespace != null) {
                                model.Namespace = arguments.Namespace;
                            }
                            if (System.IO.File.Exists(arguments.Output) && !arguments.Silent) {
                                for (; ; ) {
                                    Console.WriteLine(@"""{0}"" already exists. Overwrite it? (y/n)", arguments.Output);
                                    var line = Console.ReadLine();
                                    if (line == "y") {
                                        break;
                                    } else if (line == "n") {
                                        Console.WriteLine("Operation was canceled.");
                                        return 1;
                                    }
                                    Console.WriteLine("Invalid input text.");
                                }
                            }
                            using (var stream = new System.IO.FileStream(arguments.Output, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read)) {
                                Model.ToXml(model, stream);
                            }
                        }
                        break;
                    case OperationMode.GenerateCodeFromModel: {
                            Console.WriteLine("Mode {0}", arguments.Mode);
                            Model model;
                            try {
                                model = Model.FromXml(arguments.Input);
                            } catch (InvalidModelException) {
                                Console.WriteLine("Model is Invalid.");
                                return 1;
                            } catch {
                                Console.WriteLine("Deserializing failed.");
                                return 1;
                            }
                            model.Namespace = model.Namespace ?? "BtrieveWrapper.Orm.CustomModels";
                            string clientTemplate, recordTemplate;
                            using (var stream = new System.IO.FileStream(arguments.ClientTemplate, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                            using (var reader = new System.IO.StreamReader(stream, Encoding.UTF8)) {
                                clientTemplate = reader.ReadToEnd();
                            }
                            using (var stream = new System.IO.FileStream(arguments.RecordTemplate, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                            using (var reader = new System.IO.StreamReader(stream, Encoding.UTF8)) {
                                recordTemplate = reader.ReadToEnd();
                            }
                            var output = System.IO.Path.Combine(arguments.Output, model.DisplayName+"DbClient" + arguments.CodeExtension);
                            var exec = true;
                            if (System.IO.File.Exists(output) && !arguments.Silent) {
                                for (; ; ) {
                                    Console.WriteLine(@"""{0}"" already exists. Overwrite it? (y/n)", output);
                                    var line = Console.ReadLine();
                                    if (line == "y") {
                                        break;
                                    } else if (line == "n") {
                                        exec = false;
                                        break;
                                    }
                                    Console.WriteLine("Invalid input text.");
                                }
                            }
                            if (exec) {
                                using (var stream = new System.IO.FileStream(output, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read))
                                using (var writer = new System.IO.StreamWriter(stream, Encoding.UTF8)) {
                                    writer.Write(Template.TemplateParser.Parse(clientTemplate, model, "Model"));
                                }
                            } else {
                                Console.WriteLine(@"""{0}"" was ignored.", output);
                            }
                            foreach (var record in model.Records) {
                                output = System.IO.Path.Combine(arguments.Output, record.DisplayName + arguments.CodeExtension);
                                exec = true;
                                if (System.IO.File.Exists(output) && !arguments.Silent) {
                                    for (; ; ) {
                                        Console.WriteLine(@"""{0}"" already exists. Overwrite it? (y/n)", output);
                                        var line = Console.ReadLine();
                                        if (line == "y") {
                                            break;
                                        } else if (line == "n") {
                                            exec = false;
                                            break;
                                        }
                                        Console.WriteLine("Invalid input text.");
                                    }
                                }
                                if (exec) {
                                    using (var stream = new System.IO.FileStream(output, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read))
                                    using (var writer = new System.IO.StreamWriter(stream, Encoding.UTF8)) {
                                        writer.Write(Template.TemplateParser.Parse(recordTemplate, record, "Record"));
                                    }
                                } else {
                                    Console.WriteLine(@"""{0}"" was ignored.", output);
                                }
                            }
                        }
                        break;
                    default:
                        throw new ArgumentException();
                }
                return 0;
#if DEBUG
#else
            } catch (ArgumentException) {
                Console.WriteLine("Invalid Arguments.");
                Console.WriteLine();
                Arguments.WriteHelpMessage();
            }
            return 1;
#endif
        }
    }
}
