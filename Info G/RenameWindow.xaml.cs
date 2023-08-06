using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Info_G
{
    /// <summary>
    /// Interaction logic for RenameWindow.xaml
    /// </summary>
    public partial class RenameWindow : Window
    {
        MainWindow mainWindow;
        public RenameWindow(MainWindow _mainWindow)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
        }

        private void OnSave_Rename_click(object sender, RoutedEventArgs e)
        {
            string new_name = Rename_textbox.Text;
            string _rename_topic_query = $"UPDATE Topic SET Name = '{new_name}' WHERE Name = '{mainWindow.oldNameTopic}';";
            DbExecution.ExecuteQuery(_rename_topic_query);
            mainWindow.DisplayTopics();
            this.Close();

        }

        private void OnBack_Window_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
