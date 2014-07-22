using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace BtrieveWrapper.Orm.Models.Generator.Gui
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public static TypeList ConverterTypes { get; internal set; }

        public static Settings Settings { get; private set; }

        static App() {
            App.Settings = new Settings();
        }

        public static void Load() {
            var types = new List<Type>();
            var assembly = Assembly.GetEntryAssembly();
            foreach (var type in assembly.GetReferencedAssemblies().SelectMany(a => Assembly.Load(a).GetTypes()).Where(t => t.GetInterfaces().Any(i => i == typeof(IFieldConverter)))) {
                types.Add(type);
            }
            if (App.Settings.ImportFilePaths != null) {
                foreach (var path in App.Settings.ImportFilePaths) {
                    try {
                        assembly = Assembly.LoadFrom(path);
                        foreach (var type in assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i == typeof(IFieldConverter)))) {
                            types.Add(type);
                        }
                    } catch { }
                }
            }
            App.ConverterTypes.Clear();
            foreach (var type in types.OrderBy(t => t.FullName)) {
                App.ConverterTypes.Add(type);
            }
        }
    }
}
