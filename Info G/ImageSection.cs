using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Info_G
{
    public class ImageSection
    {
        public int topicId { get; set; }
        public Grid grid { get; set; }

        public Image imageControl { get; set; }

        public RichTextBox captionTextBox { get; set; }

        public RichTextBox captionBox { get; set; }

        public ImageSection(int topicId)
        {
            this.topicId = topicId;
            SettingUpImage();
            SettingUpGrid();
        }

        private void SettingUpGrid()
        {
            grid = new();
            grid.Width = 1100;
            grid.Height = 500;
            grid.Margin = new Thickness(0, 50, 0, 0);

            RowDefinition rowDef1 = new RowDefinition();
            rowDef1.Height = new GridLength(400); // Set the height to 120

            RowDefinition rowDef2 = new RowDefinition();
            rowDef2.Height = new GridLength(50);

            RowDefinition rowDef3 = new RowDefinition();

            ColumnDefinition columnDef1 = new ColumnDefinition();
            columnDef1.Width = new GridLength(800);

            ColumnDefinition columnDef2 = new ColumnDefinition();

            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);
            grid.RowDefinitions.Add(rowDef3);

            grid.ColumnDefinitions.Add(columnDef1);
            grid.ColumnDefinitions.Add(columnDef2);

            grid.Background = new SolidColorBrush(Colors.Wheat);
            grid.Children.Add(imageControl);
            Grid.SetColumn(imageControl, 0);
            Grid.SetRow(imageControl, 0);
        }

        private void SettingUpImage()
        {
            imageControl = new();
            imageControl.Width = 800; 
            imageControl.Height = 400; 
        }

        public void SetCaptionBox()
        {
            captionBox = new RichTextBox();
            captionBox.FontSize = 14;
            grid.Children.Add(captionBox);
            grid.Margin = new Thickness(0, 50, 0, 0);

            Grid.SetRow(captionBox, 2);
            Grid.SetColumn(captionBox, 0);
        }

        public void SpawnSaveAndCaptionButtons()
        {
            if (grid == null) return;

            Button add_caption = new Button();
            add_caption.Width = 120;
            add_caption.Height = 50;
            add_caption.Content = "Add Caption";
            add_caption.Background = new SolidColorBrush(Colors.Crimson);
            add_caption.Click += OnAddCaption_click;

            Button save = new Button();
            save.Width = 120;
            save.Height = 50;
            save.Content = "Save";
            save.Background = new SolidColorBrush(Colors.Crimson);
            save.Click += OnSave_click;

            Button remove_image = new Button();
            remove_image.Width = 120;
            remove_image.Height = 50;
            remove_image.Content = "Remove image";
            remove_image.Background = new SolidColorBrush(Colors.Crimson);
            remove_image.Click += OnRemoveImage_click;

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

        private void OnAddCaption_click(object sender, RoutedEventArgs e)
        {
            SetCaptionBox();
        }


        private byte[] ConvertImageSourceToByteArray(ImageSource imageSource)
        {
            if (imageSource == null)
                return null;

            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageSource));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }

        private string GetCaptionText()
        {
            return captionBox == null ? null : new TextRange(captionBox.Document.ContentStart, captionBox.Document.ContentEnd).Text;
        }

        private void OnSave_click(object sender, RoutedEventArgs e)
        {
            string captionText = GetCaptionText();
            string query;

            // Convert the Image control's Source (BitmapImage) to a byte array
            byte[] imageBytes = ConvertImageSourceToByteArray(imageControl.Source);

            if (captionBox != null)
                query = $"INSERT INTO Information VALUES ('{captionText}', @ImageData, {topicId})";
            else
                query = $"INSERT INTO Information VALUES (NULL, @ImageData, {topicId})";

            DbExecution.SaveImageToDatabase(imageBytes, query);
        }

       

        private void OnRemoveImage_click(object sender, RoutedEventArgs e)
        {
            // Get the parent of the button, which is the Grid
            WrapPanel panel = VisualTreeHelper.GetParent(grid) as WrapPanel;

            // If the parent is not null and it's a child of the infoPanel, remove it
            if (panel != null && panel.Children.Contains(grid))
            {
                panel.Children.Remove(grid);
            }
        }

    }
}
