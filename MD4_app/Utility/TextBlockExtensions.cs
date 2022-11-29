using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace MD4_app.Utility
{
    internal static class TextBlockExtensions
    {
        public static bool IsTextTrimmed(this TextBlock textBlock)
        {
            Typeface typeface = new Typeface(textBlock.FontFamily,
                textBlock.FontStyle,
                textBlock.FontWeight,
                textBlock.FontStretch
            );

            // FormattedText is used to measure the whole width of the text held up by TextBlock container.
            FormattedText formmatedText = new(
                textBlock.Text,
                Thread.CurrentThread.CurrentCulture,
                textBlock.FlowDirection,
                typeface,
                textBlock.FontSize,
                textBlock.Foreground
            );

            return formmatedText.Width > textBlock.Width;
        }
    }
}
