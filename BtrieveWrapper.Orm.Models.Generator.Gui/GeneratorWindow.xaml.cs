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
using System.Windows.Shapes;

namespace BtrieveWrapper.Orm.Models.Generator.Gui
{
    /// <summary>
    /// GeneratorWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class GeneratorWindow : Window, System.Windows.Forms.IWin32Window
    {
        public GeneratorWindow() {
            InitializeComponent();
        }

        private void DdfOutputBrowseButton_Click(object sender, RoutedEventArgs e) {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".xml";
            try {
                dialog.FileName = System.IO.Path.GetFileName(this.DdfOutputTextBox.Text);
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(this.DdfOutputTextBox.Text);
            } catch { }
            dialog.Filter = "Model Definition (*.xml)|*.xml|All Files (*.*)|*.*";
            var dialogResult = dialog.ShowDialog(this) ?? false;
            if (dialogResult) {
                this.DdfOutputTextBox.Text = dialog.FileName;
            }
        }

        public IntPtr Handle { get { return new System.Windows.Interop.WindowInteropHelper(this).Handle; } } 

        private void DirOutputBrowseButton_Click(object sender, RoutedEventArgs e) {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.DefaultExt = ".xml";
            try {
                dialog.FileName = System.IO.Path.GetFileName(this.DirOutputTextBox.Text);
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(this.DirOutputTextBox.Text);
            } catch { }
            dialog.Filter = "Model Definition (*.xml)|*.xml|All Files (*.*)|*.*";
            var dialogResult = dialog.ShowDialog(this) ?? false;
            if (dialogResult) {
                this.DirOutputTextBox.Text = dialog.FileName;
            }
        }

        private void DirInputBrowseButton_Click(object sender, RoutedEventArgs e) {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = this.DirInputTextBox.Text;
            var dialogResult = dialog.ShowDialog(this);
            if (dialogResult == System.Windows.Forms.DialogResult.OK) {
                this.DirInputTextBox.Text = dialog.SelectedPath;
            }
        }

        private void CodeOutputBrowseButton_Click(object sender, RoutedEventArgs e) {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.SelectedPath = this.CodeOutputTextBox.Text;
            var dialogResult = dialog.ShowDialog(this);
            if (dialogResult == System.Windows.Forms.DialogResult.OK) {
                this.CodeOutputTextBox.Text = dialog.SelectedPath;
            }
        }

        private void CodeInputBrowseButton_Click(object sender, RoutedEventArgs e) {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".xml";
            try {
                dialog.FileName = System.IO.Path.GetFileName(this.CodeInputTextBox.Text);
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(this.CodeInputTextBox.Text);
            } catch { }
            dialog.Filter = "Model Definition (*.xml)|*.xml|All Files (*.*)|*.*";
            var dialogResult = dialog.ShowDialog(this) ?? false;
            if (dialogResult) {
                this.CodeInputTextBox.Text = dialog.FileName;
            }
        }

        private void CodeDbClientTemplateBrowseButton_Click(object sender, RoutedEventArgs e) {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            try {
                dialog.FileName = System.IO.Path.GetFileName(this.CodeDbClientTemplateTextBox.Text);
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(this.CodeDbClientTemplateTextBox.Text);
            } catch { }
            dialog.Filter = "Template|*";
            var dialogResult = dialog.ShowDialog(this) ?? false;
            if (dialogResult) {
                this.CodeDbClientTemplateTextBox.Text = dialog.FileName;
            }
        }

        private void CodeRecordTemplateBrowseButton_Click(object sender, RoutedEventArgs e) {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            try {
                dialog.FileName = System.IO.Path.GetFileName(this.CodeRecordTemplateTextBox.Text);
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(this.CodeRecordTemplateTextBox.Text);
            } catch { }
            dialog.Filter = "Template|*";
            var dialogResult = dialog.ShowDialog(this) ?? false;
            if (dialogResult) {
                this.CodeRecordTemplateTextBox.Text = dialog.FileName;
            }
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e) {
            try {
                switch (this.SelectionTab.SelectedIndex) {
                    case 0:
                        var ddfArgs = new List<string>();
                        var ddfOutput = this.DdfOutputTextBox.Text;
                        var ddfHost = this.DdfHostTextBox.Text;
                        var ddfDbName = this.DdfDbNameTextBox.Text;
                        var ddfUser = this.DdfUserTextBox.Text;
                        var ddfPassword = this.DdfPasswordTextBox.Text;
                        var ddfOwnerName = this.DdfOwnerNameTextBox.Text;
                        var ddfNamespace = this.DdfNamespaceTextBox.Text;
                        var ddfDllPath = this.DdfDllTextBox.Text;
                        ddfArgs.Add("--mode=0");
                        if (String.IsNullOrWhiteSpace(ddfOutput)) {
                            throw new Exception("Input \"Output File\"");
                        } else {
                            ddfArgs.Add("--output=" + ddfOutput);
                        }
                        if (String.IsNullOrWhiteSpace(ddfHost)) {
                            throw new Exception("Input \"Host\"");
                        }
                        if (String.IsNullOrWhiteSpace(ddfDbName)) {
                            throw new Exception("Input \"DbName\"");
                        }
                        var uri = BtrieveWrapper.Orm.Path.Uri(ddfHost, ddfDbName);
                        if (!String.IsNullOrWhiteSpace(ddfUser)) {
                            uri.UriUser = ddfUser;
                        }
                        if (!String.IsNullOrWhiteSpace(ddfPassword)) {
                            uri.UriPassword = ddfPassword;
                        }
                        ddfArgs.Add("--input=" + uri.GetFilePath());
                        if (!String.IsNullOrWhiteSpace(ddfOwnerName)) {
                            ddfArgs.Add("--owner-name=" + ddfOwnerName);
                        }
                        if (!String.IsNullOrWhiteSpace(ddfNamespace)) {
                            ddfArgs.Add("--namespace=" + ddfNamespace);
                        }
                        if (!String.IsNullOrWhiteSpace(ddfDllPath)) {
                            ddfArgs.Add("--btrvlib=" + ddfDllPath);
                        }
                        ddfArgs.Add("--silent");
                        var ddfStartInfo = new System.Diagnostics.ProcessStartInfo("BtrieveWrapper.Orm.Models.Generator.exe", GetArguments(ddfArgs.ToArray()));
                        var ddfProcess = System.Diagnostics.Process.Start(ddfStartInfo);
                        ddfProcess.WaitForExit();
                        if (ddfProcess.ExitCode == 0) {
                            MessageBox.Show("Done");
                        } else {
                            MessageBox.Show("Failed");
                        }
                        break;
                    case 1:
                        var dirArgs = new List<string>();
                        var dirOutput = this.DirOutputTextBox.Text;
                        var dirInput = this.DirInputTextBox.Text;
                        var dirModelName = this.DirModelNameTextBox.Text;
                        var dirNamespace = this.DirNamespaceTextBox.Text;
                        var dirWildcard = this.DirWildcardTextBox.Text;
                        var dirDllPath = this.DirDllTextBox.Text;
                        dirArgs.Add("--mode=1");
                        if (String.IsNullOrWhiteSpace(dirOutput)) {
                            throw new Exception("Input \"Output File\"");
                        } else {
                            dirArgs.Add("--output=" + dirOutput);
                        }
                        if (String.IsNullOrWhiteSpace(dirInput)) {
                            throw new Exception("Input \"Source Directory\"");
                        } else {
                            dirArgs.Add("--input=" + dirInput);
                        }
                        if (!String.IsNullOrWhiteSpace(dirModelName)) {
                            dirArgs.Add("--name=" + dirModelName);
                        }
                        if (!String.IsNullOrWhiteSpace(dirNamespace)) {
                            dirArgs.Add("--namespace=" + dirNamespace);
                        }
                        if (!String.IsNullOrWhiteSpace(dirWildcard)) {
                            dirArgs.Add("--search-pattern=" + dirWildcard);
                        }
                        if (!String.IsNullOrWhiteSpace(dirDllPath)) {
                            dirArgs.Add("--btrvlib=" + dirDllPath);
                        }
                        dirArgs.Add("--silent");
                        var dirStartInfo = new System.Diagnostics.ProcessStartInfo("BtrieveWrapper.Orm.Models.Generator.exe", GetArguments(dirArgs.ToArray()));
                        var dirProcess = System.Diagnostics.Process.Start(dirStartInfo);
                        dirProcess.WaitForExit();
                        if (dirProcess.ExitCode == 0) {
                            MessageBox.Show("Done");
                        } else {
                            MessageBox.Show("Failed");
                        }
                        break;
                    case 2:
                        var codeArgs = new List<string>();
                        var codeOutput = this.CodeOutputTextBox.Text;
                        var codeInput = this.CodeInputTextBox.Text;
                        var codeExt = this.CodeExtTextBox.Text;
                        var codeDbClientTemplate = this.CodeDbClientTemplateTextBox.Text;
                        var codeRecordTemplate = this.CodeRecordTemplateTextBox.Text;
                        codeArgs.Add("--mode=2");
                        if (String.IsNullOrWhiteSpace(codeOutput)) {
                            throw new Exception("Input \"Output Directory\"");
                        } else {
                            codeArgs.Add("--output=" + codeOutput);
                        }
                        if (String.IsNullOrWhiteSpace(codeInput)) {
                            throw new Exception("Input \"Source Directory\"");
                        } else {
                            codeArgs.Add("--input=" + codeInput);
                        }
                        if (!String.IsNullOrWhiteSpace(codeExt)) {
                            codeArgs.Add("--ext-code=" + codeExt);
                        }
                        if (!String.IsNullOrWhiteSpace(codeDbClientTemplate)) {
                            codeArgs.Add("--template-client=" + codeDbClientTemplate);
                        }
                        if (!String.IsNullOrWhiteSpace(codeRecordTemplate)) {
                            codeArgs.Add("--template-record=" + codeRecordTemplate);
                        }
                        codeArgs.Add("--silent");
                        var codeStartInfo = new System.Diagnostics.ProcessStartInfo("BtrieveWrapper.Orm.Models.Generator.exe", GetArguments(codeArgs.ToArray()));
                        var codeProcess = System.Diagnostics.Process.Start(codeStartInfo);
                        codeProcess.WaitForExit();
                        if (codeProcess.ExitCode == 0) {
                            MessageBox.Show("Done");
                        } else {
                            MessageBox.Show("Failed");
                        }
                        break;
                }
            } catch(Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        static string GetArguments(string[] args) {
            if (args == null || args.Length == 0) {
                return null;
            }
            var result = new StringBuilder();
            for (var i = 0; i < args.Length; i++) {
                var arg = args[i].Replace("\\", "\\\\").Replace("\"", "\\\"");
                if (arg.Contains(' ')) {
                    result.Append('"');
                    result.Append(arg);
                    result.Append('"');
                } else {
                    result.Append(arg);
                }
                if (i != args.Length - 1) {
                    result.Append(' ');
                }
            }
            return result.ToString();
        }
    }
}
