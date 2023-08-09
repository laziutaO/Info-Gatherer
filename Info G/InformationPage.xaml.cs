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
        Image imageControl { get; set; }    

        Grid grid { get; set; }
        public InformationPage()
        {
            InitializeComponent();
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

            RowDefinition rowDef1 = new RowDefinition();
            rowDef1.Height = new GridLength(400); // Set the height to 120

            RowDefinition rowDef2 = new RowDefinition();
            // By not setting the Height property of the second row definition, it will take the remaining available space

            ColumnDefinition columnDef1 = new ColumnDefinition();
            columnDef1.Width = new GridLength(800);

            ColumnDefinition columnDef2 = new ColumnDefinition();

            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);

            grid.ColumnDefinitions.Add(columnDef1);
            grid.ColumnDefinitions.Add(columnDef2);

            grid.Background = new SolidColorBrush(Colors.Wheat);
            grid.Children.Add(imageControl);
            Grid.SetColumn(imageControl, 0);
            Grid.SetRow(imageControl, 0);
            infoPanel.Children.Add(grid);
        }

        private void OnAddText_click(object sender, RoutedEventArgs e)
        {

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
