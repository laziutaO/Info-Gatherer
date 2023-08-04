using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string oldNameTopic = String.Empty; 
        public MainWindow()
        {
            InitializeComponent();
            DisplayTopics();
        }       

        private void OnCreate_click(object sender, RoutedEventArgs e)
        {
            CreatePanel.Visibility = Visibility.Hidden;
            CreateButton();
            noteName.Text = String.Empty;
        }

        private void OnAdd_click(object sender, RoutedEventArgs e)
        {
            CreatePanel.Visibility = Visibility.Visible;
            noteName.MaxLength = 26;
        }

        private void OnRename_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("edit button opened!");
            Border renamePanel = new();
            renamePanel.Visibility = Visibility.Visible;

            Canvas renameCanvas = new();
            renameCanvas.Background = new SolidColorBrush(Colors.Bisque);
            renamePanel.Child = renameCanvas;

            Label renameLabel = new();
            renameLabel.Content = "NEW NAME:";
            renameCanvas.Children.Add(renameLabel);

            TextBox renameTextBox = new();
            
        }

        private void CreateButton()
        {
            string content = noteName.Text;
            string _add_topic_query = $"INSERT INTO Topic VALUES('{content}', Null)";
            DbExecution.ExecuteQuery(_add_topic_query);
            DisplayTopics();
        }

        private void DisplayTopics()
        {
            menuPanel.Children.Clear();
            try
            {
                foreach (string name in DbExecution.ReadRows(DbExecution.read_names))
                {
                    Canvas button_canvas = new Canvas();
                    button_canvas.Height = 170;
                    button_canvas.Width = 220;
                    button_canvas.Background = new SolidColorBrush(Colors.Transparent);
                    menuPanel.Children.Add(button_canvas);

                    Button butt = new();
                    butt.Height = 150;
                    butt.Width = 200;
                    butt.Content = name;
                    AdaptContentSize(name, ref butt);
                    butt.Background = new SolidColorBrush(Colors.Crimson);
                    butt.Foreground = new SolidColorBrush(Colors.Black);
                    button_canvas.Children.Add(butt);
                    butt.Margin = new Thickness(0, 10, 0, 20);

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
            StackPanel stackpanel = new();

            Button renameButton = new Button();
            renameButton.Content = "Rename";
            renameButton.Padding = new Thickness(1); // Adjust the padding value to reduce the gap
            renameButton.Margin = new Thickness(5, 5, 5, 5);
            renameButton.FontWeight = FontWeights.Light;
            renameButton.Width = 150;
            renameButton.Click += OnRename_click;

            Button deleteButton = new Button();
            deleteButton.Content = "Delete";
            deleteButton.Padding = new Thickness(1); // Adjust the padding value to reduce the gap
            deleteButton.Margin = new Thickness(5, 0, 5, 5);
            deleteButton.FontWeight = FontWeights.Light;

            stackpanel.Children.Add(renameButton);
            stackpanel.Children.Add(deleteButton);

            dropdown.Content = stackpanel;
        }

        private void AdaptContentSize(string content, ref Button butt)
        {
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
        private void Back_to_menu(object sender, RoutedEventArgs e)
        {
            CreatePanel.Visibility = Visibility.Hidden;
        }
    }
}
