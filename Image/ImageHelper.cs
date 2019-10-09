using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WhoTheFuckBot.Images
{
    public static class ImageHelper
    {
        public static Bitmap DrawText(Bitmap source, Rectangle area, string text, Font font)
        {
            Graphics g = Graphics.FromImage(source);

            g.FillRectangle(Brushes.White, area);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var stringFont = GetAdjustedFont(g, text, font, area.Width, 50, 10, true);

            g.DrawString(text, stringFont, Brushes.Black, area);

            g.Flush();
            return source;
        }
        public static Font GetAdjustedFont(Graphics g, string graphicString, Font originalFont, int containerWidth, int maxFontSize, int minFontSize, bool smallestOnFail)
        {
            Font testFont = null;
            // We utilize MeasureString which we get via a control instance           
            for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
            {
                testFont = new Font(originalFont.Name, adjustedSize, originalFont.Style);

                // Test the string with the new size
                SizeF adjustedSizeNew = g.MeasureString(graphicString, testFont);

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

    }
}