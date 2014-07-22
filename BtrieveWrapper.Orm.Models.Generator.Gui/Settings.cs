using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace BtrieveWrapper.Orm.Models.Generator.Gui
{
    [Serializable]
    public class Settings : DependencyObject
    {
        public Settings() {
            this.ImportFilePathCollection = new ObservableCollection<string>();
        }

        public static readonly DependencyProperty ImportFilePathsProperty = DependencyProperty.Register(
            "ImportFilePaths", typeof(string[]), typeof(Settings));

        [XmlArrayItem]
        public string[] ImportFilePaths {
            get { return this.ImportFilePathCollection.ToArray(); }
            set {
                this.ImportFilePathCollection.Clear();
                foreach (var item in value) {
                    this.ImportFilePathCollection.Add(item);
                }
            }
        }

        [XmlIgnore]
        public ObservableCollection<string> ImportFilePathCollection { get; set; }
    }
}
