using DropdownTopicControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Info_G
{
    /// <summary>
    /// Interaction logic for InformationPage.xaml
    /// </summary>
    public partial class InformationPage : Page
    {
        private RichTextBox activeTextBox { get; set; }

        private TextBlock activeTextBlock { get; set; }

        public int topicId { get; set; }

        private string textToChange { get; set; } = string.Empty;

        private ImageSection _activeImageSection;

        private ImageSection ActiveImageSection
        {
            get
            {
                if (_activeImageSection == null)
                {
                    _activeImageSection = new ImageSection(_topicId: topicId, _infoPage: this);
                }
                return _activeImageSection;
            }
            set
            {
                _activeImageSection = value;
            }
        }

        public DropdownTopic activeDropdown { get; set; }

        Grid grid { get; set; }

        Grid activeGrid { get; set; }
        public InformationPage(int topic_id)
        {
            InitializeComponent();
            topicId = topic_id;
            DisplayText();
        }
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Grid grid)
            {
                this.grid = grid;
                activeGrid = grid;
                foreach (UIElement element in grid.Children)
                {
                    if (element is RichTextBox textBox)
                    {
                        activeTextBox = textBox;
                        
                    }
                    else if (element is TextBlock textBlock)
                    {
                        activeTextBlock = textBlock;
                    }

                }
            }
        }
        private DropdownTopic SetDropdown(int height, int width)
        {
            DropdownTopic dropdownTopic = new DropdownTopic();

            dropdownTopic.Height = height;
            dropdownTopic.Width = width;
            dropdownTopic.VerticalAlignment = VerticalAlignment.Top;
            return dropdownTopic;
        }
        public void DisplayText()
        {
            try
            {
                DataTable dt = DbExecution.ReadRows(topicId);

                foreach (DataRow row in dt.Rows)
                {
                    DropdownTopic dropdownTopic = SetDropdown(40, 20);
                    
                    string text = row["Text"].ToString();
                    if (!row.IsNull("Image"))
                    {
                        int imageId = (int)row["Id"];
                        ImageSection imSection = new(_topicId: topicId, _infoPage: this);
                        imSection.sectionId = imageId;
                        imSection.SetSavedImageShowing();

                        byte[] imageByteArray = (byte[])row["Image"];
                        imSection.DisplayImageFromByteArray(imageByteArray);

                        if (!row.IsNull("Text")) 
                            imSection.SetCaptionBlock(text);

                        infoPanel.Children.Add(imSection.grid);
                        imSection.SetEditButton(ref dropdownTopic);
                    }

                    else
                    {
                        grid = new();
                        grid.Width = 1100;
                        grid.Background = new SolidColorBrush(Colors.Wheat);
                        grid.Margin = new Thickness(0, 50, 0, 0);
                        grid.MouseEnter += Grid_MouseEnter;
                        RowDefinition rowDef = new RowDefinition();
                        rowDef.Height = GridLength.Auto;

                        ColumnDefinition columnDef1 = new ColumnDefinition();
                        columnDef1.Width = new GridLength(800);

                        ColumnDefinition columnDef2 = new ColumnDefinition();

                        grid.ColumnDefinitions.Add(columnDef1);
                        grid.ColumnDefinitions.Add(columnDef2);
                        infoPanel.Children.Add(grid);

                        TextBlock textBlock = new();
                        textBlock.Width = 750;
                        textBlock.Margin = new Thickness(20, 10, 10, 20);
                        textBlock.FontSize = 20;
                        textBlock.Text = row["Text"].ToString();

                        grid.Margin = new Thickness(20, 50, 0, 0);
                        grid.Children.Add(textBlock);

                        dropdownTopic = SetDropdown(40, 20);

                        grid.Children.Add(dropdownTopic); 

                        Grid.SetColumn(dropdownTopic, 1);

                        dropdownTopic.Margin = new Thickness(0, 0, 0, 0);

                        SetEditButton(ref dropdownTopic);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }
        private void SetEditButton(ref DropdownTopic dropdown)
        {
            
            StackPanel stackpanel = new();

            Button editButton = new Button();
            editButton.Content = "Edit text";
            editButton.Padding = new Thickness(1); // Adjust the padding value to reduce the gap
            editButton.Margin = new Thickness(5, 5, 5, 5);
            editButton.FontWeight = FontWeights.Bold;
            editButton.Background = new SolidColorBrush(Colors.Crimson);
            editButton.FontSize = 20;
            editButton.Width = 120;
            editButton.Height = 50;
            editButton.Click += OnEdit_click;

            Button deleteButton = new Button();
            deleteButton.Content = "Delete";
            deleteButton.Padding = new Thickness(1); // Adjust the padding value to reduce the gap
            deleteButton.Margin = new Thickness(5, 0, 5, 5);
            deleteButton.Background = new SolidColorBrush(Colors.Crimson);
            deleteButton.FontSize = 20;
            deleteButton.FontWeight= FontWeights.Bold;
            deleteButton.Width = 120;
            deleteButton.Height = 50;
            deleteButton.Click += OnDelete_click;

            stackpanel.Children.Add(editButton);
            stackpanel.Children.Add(deleteButton);
            dropdown.Content = stackpanel;
            activeDropdown = dropdown;
        }

        private void OnDelete_click(object sender, RoutedEventArgs e)
        {
            activeDropdown.IsOpen = false;
            string activeText = activeTextBlock.Text;
            string _delete_text_query = $"DELETE FROM Information WHERE Text = '{activeText}' AND Topic_Id = {topicId};";
            DbExecution.ExecuteQuery(_delete_text_query);
            infoPanel.Children.Clear();
            DisplayText();
        }

        private void OnEdit_click(object sender, RoutedEventArgs e)
        {
            //editing logic
            activeDropdown.IsOpen = false;

            foreach (UIElement element in grid.Children)
            {
                if (element is Grid textGrid)
                {
                    foreach (UIElement element2 in textGrid.Children)
                    {
                        if (element2 is TextBlock textBlock)
                        {
                            textToChange = textBlock.Text;
                            activeGrid.Children.Remove(textBlock);
                            break;
                        }
                    }
                    
                }
            }
            RichTextBox newTextBox = new();
            newTextBox.Document.Blocks.Add(new Paragraph(new Run(textToChange)));
            newTextBox.FontSize = 20;
            grid.Children.Add(newTextBox);
            Grid.SetColumn(newTextBox, 0);
            activeTextBox = newTextBox;

            Button save = new Button();
            save.Width = 120;
            save.Height = 50;
            save.Content = "Update";
            save.FontWeight = FontWeights.Bold;
            save.Background = new SolidColorBrush(Colors.Crimson);
            save.FontSize = 20;
            save.Click += OnUpdate_text_click;
            //adding buttons
            Button delete_text = new Button();
            delete_text.Width = 120;
            delete_text.Height = 50;
            delete_text.FontWeight = FontWeights.Bold;
            delete_text.Content = "Delete";
            delete_text.Background = new SolidColorBrush(Colors.Crimson);
            delete_text.FontSize = 20;
            delete_text.Click += OnDelete_text_click;

            grid.Children.Add(save);
            grid.Children.Add(delete_text);

            Grid.SetColumn(save, 1);
            Grid.SetColumn(delete_text, 1);

            save.Margin = new Thickness(50, 0, 0, 150);
            delete_text.Margin = new Thickness(50, 0, 0, 0);

        }
        private void OnBack_to_menu_click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuPage());
        }

        private void OnAddPhoto_click(object sender, RoutedEventArgs e)
        {
            ImageSection imageSection = new ImageSection(_topicId:topicId, _infoPage:this, _panelLabel:new Label());
            imageSection.SetUpImageSection();
            ActiveImageSection = imageSection;      
            infoPanel.Children.Add(imageSection.grid);
        }

        private void OnSave_text_click(object sender, RoutedEventArgs e)
        {
            string active_text = new TextRange(activeTextBox.Document.ContentStart, activeTextBox.Document.ContentEnd).Text;
            string _save_text_query = $"INSERT INTO Information VALUES('{active_text}', NULL, {topicId});";
            DbExecution.ExecuteQuery(_save_text_query);
            infoPanel.Children.Clear();
            DisplayText();
        }

        private void OnUpdate_text_click(object sender, RoutedEventArgs e)
        {
            string active_text = new TextRange(activeTextBox.Document.ContentStart, activeTextBox.Document.ContentEnd).Text;
            string _update_text_query = $"UPDATE Information SET Text = '{active_text}' WHERE Text = '{textToChange}' AND Topic_Id = {topicId};";
            DbExecution.ExecuteQuery(_update_text_query);
            textToChange = String.Empty;
            infoPanel.Children.Clear();
            DisplayText();
        }

        private void OnAddText_click(object sender, RoutedEventArgs e)
        {
            //defining grid and textbox
            grid = new();
            grid.Width = 1100;
            grid.Height = 500;
            grid.Margin = new Thickness(20, 50, 0, 0);
            grid.Background = new SolidColorBrush(Colors.Beige);
            grid.Margin = new Thickness(0, 50, 0, 0);
            grid.MouseEnter += Grid_MouseEnter;

            ColumnDefinition columnDef1 = new ColumnDefinition();
            columnDef1.Width = new GridLength(800);

            ColumnDefinition columnDef2 = new ColumnDefinition();

            grid.ColumnDefinitions.Add(columnDef1);
            grid.ColumnDefinitions.Add(columnDef2);
            infoPanel.Children.Add(grid);

            RichTextBox textBox = new RichTextBox();
            textBox.FontSize = 20;
            grid.Children.Add(textBox);
            Grid.SetColumn(textBox, 0);

            Button save = new Button();
            save.Width = 120;
            save.Height = 50;
            save.Content = "Save";
            save.FontWeight = FontWeights.Bold;
            save.FontSize = 20;
            save.Background = new SolidColorBrush(Colors.Crimson);
            save.Click += OnSave_text_click;
            //adding buttons
            Button delete_text = new Button();
            delete_text.Width = 120;
            delete_text.Height = 50;
            delete_text.Content = "Delete";
            delete_text.FontWeight = FontWeights.Bold;
            delete_text.FontSize = 20;
            delete_text.Background = new SolidColorBrush(Colors.Crimson);
            delete_text.Click += OnDelete_text_click;

            grid.Children.Add(save);
            grid.Children.Add(delete_text);
            
            Grid.SetColumn(save, 1);
            Grid.SetColumn(delete_text, 1);

            save.Margin = new Thickness(50, 0, 0, 150);
            delete_text.Margin = new Thickness(50, 0, 0, 0);
        }

        private void OnDelete_text_click(object sender, RoutedEventArgs e)
        {
            infoPanel.Children.Remove(activeGrid);
        }

        private void InformationPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (ActiveImageSection.imageControl != null && Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.V)
                {
                    if (Clipboard.ContainsImage())
                    {
                        BitmapSource imageSource = Clipboard.GetImage();
                        ActiveImageSection.imageControl.Source = imageSource;
                        
                        ActiveImageSection.SpawnSaveAndCaptionButtons();
                    }
                }
            }

        }

        
    }
}
