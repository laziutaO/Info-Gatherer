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

        private Grid imageGrid { get; set; }

        public Image imageControl { get; set; }

        public RichTextBox captionBox { get; set; }

        public TextBlock captionBlock { get; set; }

        private InformationPage infoPage;

        private Border imageBorder { get; set; }

        private Label pasteLabel { get; set; }

        private string textToChange { get; set; } = String.Empty;

        public ImageSection(int _topicId, InformationPage _infoPage, Label _panelLabel = null)
        {
            topicId = _topicId;
            infoPage = _infoPage;
            pasteLabel = _panelLabel;
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

            grid.Margin = new Thickness(20, 50, 0, 0);

            RowDefinition rowDef1 = new RowDefinition();
            rowDef1.Height = new GridLength(400); 

            RowDefinition rowDef2 = new RowDefinition();
            rowDef2.Height = new GridLength(50);

            RowDefinition rowDef3 = new RowDefinition();
            rowDef3.Height = GridLength.Auto;

            ColumnDefinition columnDef1 = new ColumnDefinition();
            columnDef1.Width = new GridLength(800);

            ColumnDefinition columnDef2 = new ColumnDefinition();

            grid.RowDefinitions.Add(rowDef1);
            grid.RowDefinitions.Add(rowDef2);
            grid.RowDefinitions.Add(rowDef3);

            grid.ColumnDefinitions.Add(columnDef1);
            grid.ColumnDefinitions.Add(columnDef2);

            grid.Background = new SolidColorBrush(Colors.Wheat);

            SetImageGrid();
            imageGrid.Children.Add(imageControl);

            if(!creatingImage )
                imageGrid.Children.Remove(pasteLabel);
            
            Grid.SetColumn(imageControl, 0);
            Grid.SetRow(imageControl, 0);
            
        }

        private void SetImageGrid()
        {
            //set up image grid
            imageBorder = new();
            imageBorder.Visibility = Visibility.Visible;
            imageBorder.BorderBrush = new SolidColorBrush(Colors.Black);
            imageGrid = new Grid();
            
            imageGrid.Margin = new Thickness(10, 10, 10, 10);
            imageGrid.Height = 380;
            imageGrid.Width = 720;
            imageGrid.Background = new SolidColorBrush(Colors.White);
            imageBorder.Child = imageGrid;
            grid.Children.Add(imageBorder);
            Grid.SetColumn(imageControl, 0);
            Grid.SetRow(imageControl, 0);

            //set up message
            if(pasteLabel != null)
            {
                pasteLabel.FontSize = 16;
                pasteLabel.Content = "Paste image here";
                pasteLabel.Margin = new Thickness(300, 200, 0, 0);
                imageGrid.Children.Add(pasteLabel);
            }
            
        }

        private void SettingUpImage()
        {
            imageControl = new();
            imageControl.Width = 700; 
            imageControl.Height = 380; 
        }

        public void SetCaptionBox(bool edit = false)
        {
            captionBox = new();
            if(edit) 
                captionBox.Document.Blocks.Add(new Paragraph(new Run(textToChange)));

            captionBox.FontSize = 20;
            captionBox.Height = 400;
            grid.Children.Add(captionBox);
            Grid.SetColumn(captionBox, 0);
            Grid.SetRow(captionBox, 2);
        }

        public void SetCaptionBlock(string text)
        {
            captionBlock = new();
            captionBlock.Width = 750;
            captionBlock.Text = text;
            captionBlock.FontSize = 20;
            captionBlock.Margin = new Thickness(20, 10, 10, 20);
            grid.Children.Add(captionBlock);
            Grid.SetRow(captionBlock, 2);
            Grid.SetColumn(captionBlock, 0);
            grid.Margin = new Thickness(20, 50, 0, 0);

        }

        public void SpawnSaveAndCaptionButtons()
        {
            if (grid == null) return;

            Button add_caption = new ();
            add_caption.Width = 150;
            add_caption.Height = 50;
            add_caption.Content = "Add Caption";
            add_caption.FontSize = 20;
            add_caption.FontWeight = FontWeights.Bold;
            add_caption.Background = new SolidColorBrush(Colors.Crimson);
            add_caption.Click += OnAddCaption_click;

            Button save = new ();
            save.Width = 150;
            save.Height = 50;
            save.Content = "Save";
            save.FontSize = 20;
            save.FontWeight = FontWeights.Bold;
            save.Background = new SolidColorBrush(Colors.Crimson);
            save.Click += OnSave_click;

            Button remove_image = new ();
            remove_image.Width = 150;
            remove_image.Height = 50;
            remove_image.FontWeight = FontWeights.Bold;
            remove_image.FontSize = 20;
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

            Button editButton = new ();
            editButton.Content = "Edit caption";
            // Adjust the padding value to reduce the gap
            editButton.Padding = new Thickness(1); 
            editButton.Margin = new Thickness(5, 5, 5, 5);
            editButton.FontWeight = FontWeights.Bold;
            editButton.Background = new SolidColorBrush(Colors.Crimson);
            editButton.FontSize = 20;
            editButton.Width = 200;
            editButton.Height = 50;
            editButton.Click += OnEdit_click;

            Button deleteSectionButton = new ();
            deleteSectionButton.Content = "Delete section";
            // Adjust the padding value to reduce the gap
            deleteSectionButton.Padding = new Thickness(1); 
            deleteSectionButton.Margin = new Thickness(5, 0, 5, 5);
            deleteSectionButton.FontWeight = FontWeights.Bold;
            deleteSectionButton.Background = new SolidColorBrush(Colors.Crimson);
            deleteSectionButton.FontSize = 20;
            deleteSectionButton.Height = 50;
            deleteSectionButton.Click += OnDeleteSection_click;

            stackpanel.Children.Add(editButton);
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
            bool removeGrid = false;
            foreach (UIElement element in grid.Children)
            {
                if(element is Grid captionGrid)
                {
                    foreach (UIElement element2 in captionGrid.Children)
                    {
                        if (element2 is TextBlock textBlock)
                        {
                            textToChange = textBlock.Text;
                            removeGrid = true;  
                            //grid.Children.Remove(captionBlock);
                            break;
                        }
                    }
                }
            }
            SetCaptionBox(edit: true);

            Button save = new Button();
            save.Width = 120;
            save.Height = 50;
            save.FontSize = 20;
            save.FontWeight = FontWeights.Bold;
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

        private void OnDeleteSection_click(object sender, RoutedEventArgs e)
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
