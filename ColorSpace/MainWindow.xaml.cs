using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace ColorSpace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private int width = 500;
        private int height = 500;

        List<Color> colorList = new List<Color>();
        private byte[] buffer;
        private WriteableBitmap bitmap;

        public MainWindow()
        {
            InitializeComponent();

            width = (int)image.Width;
            height = (int)image.Height;

            buffer = new byte[3 * width * height];
            bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Rgb24, null);
        }       

        //private void UpdateBuffer()
        //{
        //    int ofs = 0;
        //    Color c;
        //    Random rnd = new Random();

        //    for (var i = 0; i < width * height; i++)
        //    {
        //        c = Color.FromArgb((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255),
        //                            (byte)rnd.Next(0, 255));

        //        buffer[ofs++] = c.R;
        //        buffer[ofs++] = c.G;
        //        buffer[ofs++] = c.B;

        //        colorList.Add(c);
        //    }
        //}

        private void FillRandomColorButton_Click(object sender, RoutedEventArgs e)
        {
            colorList.Clear();
            image.Source = bitmap;

            int offset = 0;
            Color c;
            Random rnd = new Random();
            for (var i = 0; i < width * height; i++)
            {
                c = Color.FromArgb((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255),
                                    (byte)rnd.Next(0, 255));

                //buffer[offset++] = c.A;
                buffer[offset++] = c.R;
                buffer[offset++] = c.G;
                buffer[offset++] = c.B;

                colorList.Add(c);
            }

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), buffer, 3 * width, 0);
        }

        private void SortColorButton_Click(object sender, RoutedEventArgs e)
        {
            var orderedColorList = colorList
                  .OrderBy(col => System.Drawing.Color.FromArgb(col.R, col.G, col.B).GetHue())
                  .ToArray();

            int offset = 0;
            for (var i = 0; i < orderedColorList.Length; i++)
            {
                Color c = orderedColorList[i];

                buffer[offset++] = c.R;
                buffer[offset++] = c.G;
                buffer[offset++] = c.B;
            }


            bitmap.WritePixels(new Int32Rect(0, 0, width, height), buffer, 3 * width, 0);
            var transformedBitmap = new TransformedBitmap(bitmap, new RotateTransform(270));
            image.Source = transformedBitmap;
        }
    }
}