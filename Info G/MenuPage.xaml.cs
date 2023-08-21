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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DropdownTopicControl;

namespace Info_G
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        
        public string oldNameTopic = String.Empty;

        private RenameWindow renameWindow;

        private DeleteWindow deleteWindow;

        private AddTopicWindow addTopicWindow;

        public MenuPage()
        {
            InitializeComponent();
            DisplayTopics();
        }


        private void OnAdd_click(object sender, RoutedEventArgs e)
        {
            addTopicWindow = new AddTopicWindow(this);
            addTopicWindow.Show();
        }

        private void OnRename_click(object sender, RoutedEventArgs e)
        {
            renameWindow = new RenameWindow(this);
            renameWindow.Show();
        }

        private void OnDelete_click(object sender, RoutedEventArgs e)
        {
            int topic_id = Get_TopicId();
            deleteWindow = new DeleteWindow(this, topic_id);
            deleteWindow.Show();
        }
        private void CanvasMouseEnter(object sender, MouseEventArgs e)
        {
            // Get old name of topic 
            if (sender is Canvas canvas)
            {
                foreach (UIElement element in canvas.Children)
                {
                    if (element is Button button)
                    {
                        oldNameTopic = (string)button.Content;
                    }
                }
            }
        }

        private void Topic_open_click(object sender, RoutedEventArgs e)
        {
            // Get the topic ID first
            int topicId = Get_TopicId();
            // Set the topic ID
            InformationPage informationPage = new InformationPage(topicId);
            informationPage.topicId = topicId; 
            NavigationService.Navigate(informationPage);
        }

        private int Get_TopicId()
        {
            string _get_topic_id = $"SELECT Id FROM Topic WHERE Name = '{oldNameTopic}';";
            return (int)DbExecution.ExecuteReturnQuery(_get_topic_id);
        }
        public void DisplayTopics()
        {
            menuPanel.Children.Clear();
            try
            {
                //Get names of topics from database and create button for each topic
                foreach (string name in DbExecution.ReadNames(DbExecution.read_names))
                {
                    //set canvas for containing button
                    Canvas button_canvas = new Canvas();
                    button_canvas.Height = 170;
                    button_canvas.Width = 220;
                    button_canvas.Margin = new Thickness(20, 10, 10, 10);
                    button_canvas.Background = new SolidColorBrush(Colors.Transparent);
                    button_canvas.MouseEnter += CanvasMouseEnter;
                    menuPanel.Children.Add(button_canvas);

                    //set actual button
                    Button butt = new();
                    butt.Height = 150;
                    butt.Width = 200;
                    butt.Content = name;
                    
                    AdaptContentSize(name, ref butt);
                    butt.Background = new SolidColorBrush(Colors.Crimson);
                    butt.Foreground = new SolidColorBrush(Colors.Black);
                    button_canvas.Children.Add(butt);
                    butt.Margin = new Thickness(0, 10, 0, 20);
                    butt.Click += Topic_open_click;
                    //set dropdown options fore each topic
                    DropdownTopic dropdownTopic = new DropdownTopic();
                    dropdownTopic.Height = 20;
                    dropdownTopic.Width = 14;
                    button_canvas.Children.Add(dropdownTopic);
                    dropdownTopic.Margin = new Thickness(175, 30, 0, 20);
                    SetEditButton(ref dropdownTopic, name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }



        private void SetEditButton(ref DropdownTopic dropdown, string curNameTopic)
        {
            //settting dropdown options
            StackPanel stackpanel = new();
            //button for renaming topic
            Button renameButton = new Button();
            renameButton.Content = "Rename";
            renameButton.Padding = new Thickness(1); // Adjust the padding value to reduce the gap
            renameButton.Margin = new Thickness(5, 5, 5, 5);
            renameButton.FontWeight = FontWeights.Light;
            renameButton.Width = 150;
            renameButton.Click += OnRename_click;
            //button for deleting topic
            Button deleteButton = new Button();
            deleteButton.Content = "Delete";
            deleteButton.Padding = new Thickness(1); // Adjust the padding value to reduce the gap
            deleteButton.Margin = new Thickness(5, 0, 5, 5);
            deleteButton.FontWeight = FontWeights.Light;
            deleteButton.Click += OnDelete_click;

            stackpanel.Children.Add(renameButton);
            stackpanel.Children.Add(deleteButton);

            dropdown.Content = stackpanel;
        }

        private void AdaptContentSize(string content, ref Button butt)
        {
            //method for adjusting font size according to text length
            if (content.Length > 22)
                butt.FontSize = 12;
            else if (content.Length > 16)
                butt.FontSize = 14;
            else if (content.Length > 12)
                butt.FontSize = 16;
            else if (content.Length > 7)
                butt.FontSize = 18;
            else
                butt.FontSize = 24;
        }

    }
}

