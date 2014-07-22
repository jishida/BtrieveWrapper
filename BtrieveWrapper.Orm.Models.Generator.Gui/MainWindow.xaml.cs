using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace BtrieveWrapper.Orm.Models.Generator.Gui
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        string _filePath = null;

        public MainWindow() {
            InitializeComponent();
            App.ConverterTypes = this.Resources["ConverterTypes"] as TypeList;
            App.Load();
        }

        private void MenuItemNew_Click(object sender, RoutedEventArgs e) {
            this.DataContext = new Model();
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.DefaultExt = ".xml";
            dialog.Filter = "Xml documents (.xml)|*.xml|All files|*.*";
            dialog.Multiselect = false;
            var result = dialog.ShowDialog(this);
            try {
                if (result ?? false) {
                    this.DataContext = Model.FromXml(dialog.FileName);
                    _filePath = dialog.FileName;
                }
            } catch {
                MessageBox.Show("Failed");
            }
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e) {
            if (_filePath == null) {
                this.MenuItemSaveAs_Click(sender, e);
            } else {
                var model = this.DataContext as Model;
                if (model == null) {
                    MessageBox.Show("Open or create model definition file.");
                    return;
                }
                try {
                    Model.ToXml(model, _filePath);
                    MessageBox.Show("Saved");
                } catch {
                    MessageBox.Show("Failed");
                }
            }
        }

        private void MenuItemSaveAs_Click(object sender, RoutedEventArgs e) {
            var model = this.DataContext as Model;
            if (model == null) {
                MessageBox.Show("Open or create model definition file.");
                return;
            }
            var dialog = new SaveFileDialog();
            dialog.FileName = _filePath == null ? "Model.xml" : System.IO.Path.GetFileName(_filePath);
            dialog.DefaultExt = ".xml";
            dialog.Filter = "Xml documents (.xml)|*.xml|All files|*.*";
            var result = dialog.ShowDialog(this);
            if (result ?? false) {
                var exec = true;
                if (System.IO.File.Exists(dialog.FileName)) {
                    var messageResult = MessageBox.Show("This file is already exists. Do you want to overwrite it?", "", MessageBoxButton.YesNo);
                    if (messageResult == MessageBoxResult.No) {
                        exec = false;
                    }
                }
                if (exec) {
                    try {
                        Model.ToXml(model, dialog.FileName);
                        _filePath = dialog.FileName;
                        MessageBox.Show("Saved");
                    } catch {
                        MessageBox.Show("Failed");
                    }
                }
            }
        }

        private void RecordUpButton_Click(object sender, RoutedEventArgs e) {
            this.RecordListDataGrid.Up();
        }

        private void RecordDownButton_Click(object sender, RoutedEventArgs e) {
            this.RecordListDataGrid.Down();
        }

        private void FieldUpButtion_Click(object sender, RoutedEventArgs e) {
            this.FieldListDataGrid.Up();
        }

        private void FieldDownButtion_Click(object sender, RoutedEventArgs e) {
            this.FieldListDataGrid.Down();
        }

        private void KeyUpButtion_Click(object sender, RoutedEventArgs e) {
            this.KeyListDataGrid.Up();
        }

        private void KeyDownButtion_Click(object sender, RoutedEventArgs e) {
            this.KeyListDataGrid.Down();
        }

        private void KeySegmentUpButton_Click(object sender, RoutedEventArgs e) {
            this.KeySegmentListDataGrid.Up();
        }

        private void KeySegmentDownButton_Click(object sender, RoutedEventArgs e) {
            this.KeySegmentListDataGrid.Down();
        }

        private void AdvancedButton_Click(object sender, RoutedEventArgs e) {
            new AdvancedWindow(this.DataContext).ShowDialog();
        }

        private void MenuItemGenerator_Click(object sender, RoutedEventArgs e) {
            new GeneratorWindow().ShowDialog();
        }
    }
}
