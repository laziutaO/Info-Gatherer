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
    /// Interaction logic for AddTopicWindow.xaml
    /// </summary>
    public partial class AddTopicWindow : Window
    {
        public int topic_length { get; set; } = 26;

        MenuPage menuPage;
        public AddTopicWindow(MenuPage menuPage)
        {
            InitializeComponent();
            this.menuPage = menuPage;
            DataContext = this;
        }

        private void Back_to_menu(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnCreate_click(object sender, RoutedEventArgs e)
        {
            noteName.Text = String.Empty;
            CreateButton();
            
        }

        private void CreateButton()
        {
            string content = noteName.Text;
            string _add_topic_query = $"INSERT INTO Topic VALUES('{content}', Null);";
            DbExecution.ExecuteQuery(_add_topic_query);
            menuPage.DisplayTopics();
            this.Close();
        }
    }
}
