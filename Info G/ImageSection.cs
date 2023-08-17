using DropdownTopicControl;
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

        public int sectionId { get; set; }  
        public Grid grid { get; set; }

        public Image imageControl { get; set; }

        public RichTextBox captionBox { get; set; }

        public TextBlock captionBlock { get; set; }

        private InformationPage infoPage;

        private string textToChange { get; set; } = String.Empty;

        public ImageSection(int topicId, InformationPage infoPage)
        {
            this.topicId = topicId;
            this.infoPage = infoPage;
        }

        public void SetUpImageSection()
        {
            SettingUpImage();
            SettingUpGrid();
        }
        public void SetSavedImageShowing()
        {
            SettingUpImage();
            SettingUpGrid(creatingImage: false);
            
        }
        private void SettingUpGrid(bool creatingImage = true, int width = 1100, int height = 500)
        {
            grid = new();
            grid.Width = width;
            if(creatingImage)
                grid.Height = height;
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

        public void SetCaptionBlock(string text)
        {
            captionBlock = new();
            captionBlock.Text = text;
            captionBlock.FontSize = 14;
            grid.Children.Add(captionBlock);
            grid.Margin = new Thickness(0, 50, 0, 0);

            Grid.SetRow(captionBlock, 2);
            Grid.SetColumn(captionBlock, 0);
            
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

        public void SetEditButton(ref DropdownTopic dropdown)
        {
            StackPanel stackpanel = new();

            Button editButton = new Button();
            editButton.Content = "Edit caption";
            // Adjust the padding value to reduce the gap
            editButton.Padding = new Thickness(1); 
            editButton.Margin = new Thickness(5, 5, 5, 5);
            editButton.FontWeight = FontWeights.Light;
            editButton.Width = 150;
            editButton.Click += OnEdit_click;

            Button deleteIMageButton = new Button();
            deleteIMageButton.Content = "Delete image";
            // Adjust the padding value to reduce the gap
            deleteIMageButton.Padding = new Thickness(1); 
            deleteIMageButton.Margin = new Thickness(5, 0, 5, 5);
            deleteIMageButton.FontWeight = FontWeights.Light;
            deleteIMageButton.Click += OnDeleteImage_click;

            Button deleteSectionButton = new Button();
            deleteIMageButton.Content = "Delete section";
            // Adjust the padding value to reduce the gap
            deleteIMageButton.Padding = new Thickness(1); 
            deleteIMageButton.Margin = new Thickness(5, 0, 5, 5);
            deleteIMageButton.FontWeight = FontWeights.Light;
            //deleteIMageButton.Click += OnDeleteSection_click;

            stackpanel.Children.Add(editButton);
            stackpanel.Children.Add(deleteIMageButton);
            stackpanel.Children.Add(deleteSectionButton);
            dropdown.Content = stackpanel;

            grid.Children.Add(dropdown);

            Grid.SetColumn(dropdown, 1);

            dropdown.Margin = new Thickness(0, 0, 0, 0);
            infoPage.activeDropdown = dropdown;
        }

        private void OnEdit_click(object sender, RoutedEventArgs e)
        {
            //editing logic
            infoPage.activeDropdown.IsOpen = false;

            foreach (UIElement element in grid.Children)
            {
                if (element is TextBlock textBlock)
                {
                    textToChange = textBlock.Text;
                    grid.Children.Remove(textBlock);
                    break;
                }
            }

            captionBox = new();
            captionBox.Document.Blocks.Add(new Paragraph(new Run(textToChange)));
            captionBox.FontSize = 14;
            grid.Children.Add(captionBox);
            Grid.SetColumn(captionBox, 0);
            Grid.SetRow(captionBox, 1);

            Button save = new Button();
            save.Width = 120;
            save.Height = 50;
            save.Content = "Update";
            save.Background = new SolidColorBrush(Colors.Crimson);
            save.Click += OnUpdate_text_click;

            //adding button
            grid.Children.Add(save);

            Grid.SetColumn(save, 1);

            save.Margin = new Thickness(50, 0, 0, 150);

        }
        private void OnUpdate_text_click(object sender, RoutedEventArgs e)
        {
            string active_text = new TextRange(captionBox.Document.ContentStart, captionBox.Document.ContentEnd).Text;
            string _update_text_query = $"UPDATE Information SET Text = '{active_text}' WHERE Id = {sectionId};";
            DbExecution.ExecuteQuery(_update_text_query);
            textToChange = String.Empty;
            infoPage.infoPanel.Children.Clear();
            infoPage.DisplayText();
        }

        private void OnDeleteImage_click(object sender, RoutedEventArgs e)
        {
            string _delete_text_query = $"DELETE FROM Information WHERE Id = {sectionId} AND Topic_Id = {topicId};";
            DbExecution.ExecuteQuery(_delete_text_query);
            infoPage.infoPanel.Children.Clear();
            infoPage.DisplayText();
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

            RemoveGrid();
            infoPage.infoPanel.Children.Clear();
            infoPage.DisplayText();
        }

        private void RemoveGrid()
        {
            // get parent
            WrapPanel panel = VisualTreeHelper.GetParent(grid) as WrapPanel;

            // If the parent is not null and it's a child of the infoPanel, remove it
            if (panel != null && panel.Children.Contains(grid))
            {
                panel.Children.Remove(grid);
            }
        }

        private void OnRemoveImage_click(object sender, RoutedEventArgs e)
        {
            RemoveGrid();
        }

        private BitmapImage ConvertByteArrayToBitmapImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
        }
        public void DisplayImageFromByteArray(byte[] imageBytes)
        {
            BitmapImage image = ConvertByteArrayToBitmapImage(imageBytes);
            if (image != null)
            {
                // Set the BitmapImage as the Source of Image control
                imageControl.Source = image;
            }
        }

    }
}
