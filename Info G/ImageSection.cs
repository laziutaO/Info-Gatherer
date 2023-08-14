using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Info_G
{
    public class ImageSection
    {
        public Grid grid { get; set; }

        public Image imageControl { get; set; }

        public RichTextBox captionTextBox { get; set; }

        public ImageSection()
        {
            SettingUpGrid();
            SettingUpImage();
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

       
    }
}
