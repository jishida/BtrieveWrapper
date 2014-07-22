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
    /// AdvancedWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AdvancedWindow : Window
    {
        public AdvancedWindow(object context) {
            InitializeComponent();
            this.DataContext = context;
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
