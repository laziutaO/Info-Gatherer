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
    /// Interaction logic for DeleteWindow.xaml
    /// </summary>
    public partial class DeleteWindow : Window
    {
        MenuPage mainWindow;

        int topic_Id;
        public DeleteWindow(MenuPage _mainWindow, int _topic_Id)
        {
            InitializeComponent();
            mainWindow = _mainWindow;
            topic_Id = _topic_Id;
        }

        private void OnDelete_click(object sender, RoutedEventArgs e)
        {
            string _delete_topic_query = $"DELETE FROM Information WHERE Topic_Id = {topic_Id};" +
                $"DELETE FROM Topic WHERE Id = {topic_Id};";
            DbExecution.ExecuteQuery(_delete_topic_query);
            mainWindow.DisplayTopics();
            this.Close();

        }

        private void OnBack_Window_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
