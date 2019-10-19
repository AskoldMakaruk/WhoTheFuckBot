using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using Image = BotApi.DB.Model.Image;

namespace BotApi
{
    public static class ImageHelper
    {
        private static GraphicsPath GetStringPath(string s, float dpi, RectangleF rect, Font font, StringFormat format)
        {
            var path = new GraphicsPath();
            // Convert font size into appropriate coordinates
            var emSize = dpi * font.SizeInPoints / 72;
            path.AddString(s, font.FontFamily, (int) font.Style, emSize, rect, format);

            return path;
        }

        public static byte[] DrawText(Image image, string text)
        {
            var stream = new MemoryStream(image.Value);

            var source = new Bitmap(stream);
            var g = Graphics.FromImage(source);

            var c = (int) (source.Height / 5);
            var area = new Rectangle(0, source.Height - c, source.Width, source.Height - c - 20);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var stringFont = GetAdjustedFont(g, text, new Font("Arial", 24), area.Width, 50, 10, true);

            var format = StringFormat.GenericTypographic;
            var dpi = g.DpiY;

            var size = g.MeasureString(text, stringFont);

            var newarea = new RectangleF(
                area.X + (area.Width - size.Width) / 2, area.Y, size.Width, size.Height);
            using(var graphicsPath = GetStringPath(text, dpi, newarea, stringFont, format))
            {
                g.FillPath(Brushes.White, graphicsPath);
                g.DrawPath(Pens.Black, graphicsPath);

                //g.DrawString(text, stringFont, Brushes.Black, area);
            }

            g.Flush();

            stream.Dispose();
            stream = new MemoryStream();

            source.Save(stream, ImageFormat.Jpeg);
            var res = stream.ToArray();
            return res;
        }

        public static Font GetAdjustedFont(Graphics g, string graphicString, Font originalFont,
            int containerWidth,
            int maxFontSize, int minFontSize, bool smallestOnFail)
        {
            Font testFont = null;
            // We utilize MeasureString which we get via a control instance           
            for (var adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
            {
                testFont = new Font(originalFont.Name, adjustedSize, originalFont.Style);

                // Test the string with the new size
                var adjustedSizeNew = g.MeasureString(graphicString, testFont);

                if (containerWidth > Convert.ToInt32(adjustedSizeNew.Width))
                {
                    // Good font, return it
                    return testFont;
                }
            }

            // If you get here there was no fontsize that worked
            // return minimumSize or original?
            if (smallestOnFail)
            {
                return testFont;
            }
            else
            {
                return originalFont;
            }
        }

        //static List<string> WrapText(string text, double pixels, string fontFamily,
        //                             float  emSize)
        //{
        //    string[] originalLines = text.Split(new string[] { " " },
        //        StringSplitOptions.None);

        //    List<string> wrappedLines = new List<string>();

        //    StringBuilder actualLine  = new StringBuilder();
        //    double        actualWidth = 0;

        //    foreach (var item in originalLines)
        //    {
        //        FormattedText formatted = new FormattedText(item,
        //            CultureInfo.CurrentCulture,
        //            System.Windows.FlowDirection.LeftToRight,
        //            new Typeface(fontFamily), emSize, Brushes.Black);

        //        actualLine.Append(item + " ");
        //        actualWidth += formatted.Width;

        //        if (actualWidth > pixels)
        //        {
        //            wrappedLines.Add(actualLine.ToString());
        //            actualLine.Clear();
        //            actualWidth = 0;
        //        }
        //    }

        //    if (actualLine.Length > 0)
        //        wrappedLines.Add(actualLine.ToString());

        //    return wrappedLines;
        //}
    }
}