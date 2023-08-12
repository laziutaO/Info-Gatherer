using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace Info_G
{
    /// <summary>
    /// Interaction logic for InformationPage.xaml
    /// </summary>
    public partial class InformationPage : Page
    {
        private RichTextBox activeTextBox { get; set; }

        public int topicId { get; set; }

        private Image activeImage { get; set; }
        private Image imageControl { get; set; }    

        Grid grid { get; set; }
        public InformationPage()
        {
            InitializeComponent();
        }
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Grid grid)
            {
                foreach (UIElement element in grid.Children)
                {
                    if (element is RichTextBox textBox)
                    {
                        activeTextBox = textBox;
                        
                        
                    }

                    else if (element is Image image) 
                    { 
                        //
                    }
                }
            }
        }
        private void Back_to_menu_click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MenuPage());
        }

        private void OnAddPhoto_click(object sender, RoutedEventArgs e)
        {
            //creating panel for pasting image
            imageControl = new();
            imageControl.Width = 800; // Set your desired width value
            imageControl.Height = 400; // Set your desired height value
            
            //creating canvas panel
            grid = new();
            grid.Width = 1100;
            grid.Height = 500;
            grid.Margin = new Thickness(0, 50, 0, 0);

            RowDefinition rowDef1 = new RowDefinition();
            rowDef1.Height = new GridLength(400); // Set the height to 120

            RowDefinition rowDef2 = new RowDefinition();
            rowDef2.Height = new GridLength (50);

            RowDefinition rowDef3 = new RowDefinition();

            ColumnDefinition columnDef1 = new ColumnDefinition();
            columnDef1.Width = new GridLength(800);

            ColumnDefinition columnDef2 = new ColumnDefinition();

            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);
            grid.RowDefinitions.Add (rowDef3);

            grid.ColumnDefinitions.Add(columnDef1);
            grid.ColumnDefinitions.Add(columnDef2);

            grid.Background = new SolidColorBrush(Colors.Wheat);
            grid.Children.Add(imageControl);
            Grid.SetColumn(imageControl, 0);
            Grid.SetRow(imageControl, 0);
            infoPanel.Children.Add(grid);
        }

        private void AddCaption_click(object sender, RoutedEventArgs e)
        {
            RichTextBox captionBox = new RichTextBox();
            captionBox.FontSize = 14;
            grid.Children.Add(captionBox);
            grid.Margin = new Thickness(0, 50, 0, 0);

            Grid.SetRow(captionBox, 2);
            Grid.SetColumn(captionBox, 0);
        }

        private void Save_text_click(object sender, RoutedEventArgs e)
        {
            string active_text = new TextRange(activeTextBox.Document.ContentStart, activeTextBox.Document.ContentEnd).Text;
            MessageBox.Show(active_text);
            string _save_text_query = $"INSERT INTO Information VALUES('{activeTextBox}', NULL, {topicId});";
            DbExecution.ExecuteQuery(_save_text_query);
        }

        private void OnAddText_click(object sender, RoutedEventArgs e)
        {
            //defining grid and textbox
            grid = new();
            grid.Width = 1100;
            grid.Height = 500;
            grid.Background = new SolidColorBrush(Colors.Beige);
            grid.Margin = new Thickness(0, 50,0,0);
            grid.MouseEnter += Grid_MouseEnter;

            ColumnDefinition columnDef1 = new ColumnDefinition();
            columnDef1.Width = new GridLength(800);

            ColumnDefinition columnDef2 = new ColumnDefinition();

            grid.ColumnDefinitions.Add(columnDef1);
            grid.ColumnDefinitions.Add(columnDef2);
            infoPanel.Children.Add(grid);

            RichTextBox textBox = new RichTextBox();
            textBox.FontSize = 14;
            grid.Children.Add(textBox);
            Grid.SetColumn(textBox, 0);

            Button save = new Button();
            save.Width = 120;
            save.Height = 50;
            save.Content = "Save";
            save.Background = new SolidColorBrush(Colors.Crimson);
            save.Click += Save_text_click;
            //adding buttons
            Button delete_text = new Button();
            delete_text.Width = 120;
            delete_text.Height = 50;
            delete_text.Content = "Delete";
            delete_text.Background = new SolidColorBrush(Colors.Crimson);
            delete_text.Click += Delete_text_click;

            grid.Children.Add(save);
            grid.Children.Add(delete_text);
            
            Grid.SetColumn(save, 1);
            Grid.SetColumn(delete_text, 1);

            save.Margin = new Thickness(50, 0, 0, 150);
            delete_text.Margin = new Thickness(50, 0, 0, 0);
        }

        private void Delete_text_click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void InformationPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (imageControl != null && Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Key == Key.V)
                {
                    if (Clipboard.ContainsImage())
                    {
                        BitmapSource imageSource = Clipboard.GetImage();
                        imageControl.Source = imageSource;
                        
                        SpawnSaveAndCaptionButtons();
                    }

                }
            }

        }

        private void SpawnSaveAndCaptionButtons()
        {
            if (grid == null) return;

            Button add_caption = new Button();
            add_caption.Width = 120;
            add_caption.Height = 50;
            add_caption.Content = "Add Caption";
            add_caption.Background = new SolidColorBrush(Colors.Crimson);
            add_caption.Click += AddCaption_click;

            Button save = new Button();
            save.Width = 120;
            save.Height = 50;
            save.Content = "Save";
            save.Background = new SolidColorBrush(Colors.Crimson);

            Button remove_image = new Button();
            remove_image.Width = 120;
            remove_image.Height = 50;
            remove_image.Content = "Remove image";
            remove_image.Background = new SolidColorBrush(Colors.Crimson);

            grid.Children.Add(add_caption);
            grid.Children.Add(save);
            grid.Children.Add(remove_image);

            Grid.SetRow(add_caption, 0);
            Grid.SetRow(save, 0);
            Grid.SetRow(remove_image, 0);
            Grid.SetColumn(add_caption, 1);
            Grid.SetColumn(save, 1);
            Grid.SetColumn(remove_image, 1);

            add_caption.Margin = new Thickness(50, 0, 0, 300);
            save.Margin = new Thickness(50, 0, 0, 150);
            remove_image.Margin = new Thickness(50, 0, 0, 0);
        }
    }
}
