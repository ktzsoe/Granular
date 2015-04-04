﻿extern alias wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Granular.Extensions;
using System.Windows.Media.Imaging;

namespace Granular.Host.Wpf.Render
{
    public interface IWpfValueConverter
    {
        wpf::System.Windows.Media.Brush Convert(Brush brush);
        wpf::System.Windows.Media.Color Convert(Color color);
        wpf::System.Windows.CornerRadius Convert(CornerRadius cornerRadius);
        wpf::System.Windows.Point Convert(Point point);
        wpf::System.Windows.Rect Convert(Rect rect);
        wpf::System.Windows.Size Convert(Size size);
        wpf::System.Windows.Thickness Convert(Thickness thickness);
        wpf::System.Windows.Media.Typeface Convert(Typeface typeface);
        wpf::System.Windows.FontStretch Convert(FontStretch fontStretch);
        wpf::System.Windows.FontWeight Convert(FontWeight fontWeight);
        wpf::System.Windows.FontStyle Convert(FontStyle fontStyle);
        wpf::System.Windows.Media.FontFamily Convert(FontFamily fontFamily);
        wpf::System.Windows.TextAlignment Convert(TextAlignment textAlignment);
        wpf::System.Windows.TextTrimming Convert(TextTrimming textTrimming);
        wpf::System.Windows.TextWrapping Convert(TextWrapping textWrapping);
        wpf::System.Windows.Controls.ScrollBarVisibility Convert(ScrollBarVisibility scrollBarVisibility);
        wpf::System.Windows.Media.ImageSource Convert(ImageSource source);

        MouseButton ConvertBack(wpf::System.Windows.Input.MouseButton mouseButton);
        MouseButtonState ConvertBack(wpf::System.Windows.Input.MouseButtonState mouseButtonState);
        Point ConvertBack(wpf::System.Windows.Point point);
        Key ConvertBack(wpf::System.Windows.Input.Key key);
        KeyStates ConvertBack(wpf::System.Windows.Input.KeyStates keyStates);
    }

    public class WpfValueConverter : IWpfValueConverter
    {
        public static readonly WpfValueConverter Default = new WpfValueConverter();

        private WpfValueConverter()
        {
            //
        }

        public wpf::System.Windows.Media.Brush Convert(Brush brush)
        {
            if (brush == null)
            {
                return null;
            }

            if (brush is SolidColorBrush)
            {
                return new wpf::System.Windows.Media.SolidColorBrush(Convert(((SolidColorBrush)brush).Color)) { Opacity = brush.Opacity };
            }

            if (brush is LinearGradientBrush)
            {
                return new wpf::System.Windows.Media.LinearGradientBrush(Convert(((LinearGradientBrush)brush).GradientStops))
                {
                    Opacity = brush.Opacity,
                    SpreadMethod = Convert(((LinearGradientBrush)brush).SpreadMethod),
                    StartPoint = Convert(((LinearGradientBrush)brush).StartPoint),
                    EndPoint = Convert(((LinearGradientBrush)brush).EndPoint),
                };
            }

            if (brush is RadialGradientBrush)
            {
                return new wpf::System.Windows.Media.RadialGradientBrush(Convert(((RadialGradientBrush)brush).GradientStops))
                {
                    Opacity = brush.Opacity,
                    SpreadMethod = Convert(((RadialGradientBrush)brush).SpreadMethod),
                    Center = Convert(((RadialGradientBrush)brush).Center),
                    GradientOrigin = Convert(((RadialGradientBrush)brush).GradientOrigin),
                    RadiusX = ((RadialGradientBrush)brush).RadiusX,
                    RadiusY = ((RadialGradientBrush)brush).RadiusY,
                };
            }

            throw new Granular.Exception("Conversion is not implemented");
        }

        private wpf::System.Windows.Media.GradientStopCollection Convert(GradientStopCollection gradientStopCollection)
        {
            return new wpf::System.Windows.Media.GradientStopCollection(gradientStopCollection.Select(gradientStop => Convert(gradientStop)));
        }

        private wpf::System.Windows.Media.GradientStop Convert(GradientStop gradientStop)
        {
            return new wpf::System.Windows.Media.GradientStop(Convert(gradientStop.Color), gradientStop.Offset);
        }

        private wpf::System.Windows.Media.GradientSpreadMethod Convert(GradientSpreadMethod gradientSpreadMethod)
        {
            return (wpf::System.Windows.Media.GradientSpreadMethod)((int)gradientSpreadMethod);
        }

        public wpf::System.Windows.Media.Color Convert(Color color)
        {
            return wpf::System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public wpf::System.Windows.CornerRadius Convert(CornerRadius cornerRadius)
        {
            return new wpf::System.Windows.CornerRadius(cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomRight, cornerRadius.BottomLeft);
        }

        public wpf::System.Windows.Point Convert(Point point)
        {
            return new wpf::System.Windows.Point(point.X, point.Y);
        }

        public wpf::System.Windows.Rect Convert(Rect rect)
        {
            return new wpf::System.Windows.Rect(Convert(rect.Location), Convert(rect.Size));
        }

        public wpf::System.Windows.Size Convert(Size size)
        {
            return new wpf::System.Windows.Size(size.Width, size.Height);
        }

        public wpf::System.Windows.Thickness Convert(Thickness thickness)
        {
            return new wpf::System.Windows.Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
        }

        public wpf::System.Windows.Media.Typeface Convert(Typeface typeface)
        {
            return new wpf::System.Windows.Media.Typeface(Convert(typeface.FontFamily), Convert(typeface.Style), Convert(typeface.Weight), Convert(typeface.Stretch));
        }

        public wpf::System.Windows.FontStretch Convert(FontStretch fontStretch)
        {
            switch (fontStretch)
            {
                case FontStretch.Normal: return wpf::System.Windows.FontStretches.Normal;
                case FontStretch.Condensed: return wpf::System.Windows.FontStretches.Condensed;
                case FontStretch.Expanded: return wpf::System.Windows.FontStretches.Expanded;
                case FontStretch.ExtraCondensed: return wpf::System.Windows.FontStretches.ExtraCondensed;
                case FontStretch.ExtraExpanded: return wpf::System.Windows.FontStretches.ExtraExpanded;
                case FontStretch.Medium: return wpf::System.Windows.FontStretches.Medium;
                case FontStretch.SemiCondensed: return wpf::System.Windows.FontStretches.SemiCondensed;
                case FontStretch.SemiExpanded: return wpf::System.Windows.FontStretches.SemiExpanded;
                case FontStretch.UltraCondensed: return wpf::System.Windows.FontStretches.UltraCondensed;
                case FontStretch.UltraExpanded: return wpf::System.Windows.FontStretches.UltraExpanded;
            }

            throw new Granular.Exception("Unexpected FontStretch value \"{0}\"", fontStretch);
        }

        public wpf::System.Windows.FontWeight Convert(FontWeight fontWeight)
        {
            switch (fontWeight)
            {
                case FontWeight.Normal: return wpf::System.Windows.FontWeights.Normal;
                case FontWeight.Black: return wpf::System.Windows.FontWeights.Black;
                case FontWeight.Bold: return wpf::System.Windows.FontWeights.Bold;
                case FontWeight.DemiBold: return wpf::System.Windows.FontWeights.DemiBold;
                case FontWeight.ExtraBlack: return wpf::System.Windows.FontWeights.ExtraBlack;
                case FontWeight.ExtraBold: return wpf::System.Windows.FontWeights.ExtraBold;
                case FontWeight.ExtraLight: return wpf::System.Windows.FontWeights.ExtraLight;
                case FontWeight.Heavy: return wpf::System.Windows.FontWeights.Heavy;
                case FontWeight.Light: return wpf::System.Windows.FontWeights.Light;
                case FontWeight.Medium: return wpf::System.Windows.FontWeights.Medium;
                case FontWeight.Regular: return wpf::System.Windows.FontWeights.Regular;
                case FontWeight.SemiBold: return wpf::System.Windows.FontWeights.SemiBold;
                case FontWeight.Thin: return wpf::System.Windows.FontWeights.Thin;
                case FontWeight.UltraBlack: return wpf::System.Windows.FontWeights.UltraBlack;
                case FontWeight.UltraBold: return wpf::System.Windows.FontWeights.UltraBold;
                case FontWeight.UltraLight: return wpf::System.Windows.FontWeights.UltraLight;
            }

            throw new Granular.Exception("Unexpected FontWeight value \"{0}\"", fontWeight);
        }

        public wpf::System.Windows.FontStyle Convert(FontStyle fontStyle)
        {
            switch (fontStyle)
            {
                case FontStyle.Normal: return wpf::System.Windows.FontStyles.Normal;
                case FontStyle.Italic: return wpf::System.Windows.FontStyles.Italic;
                case FontStyle.Oblique: return wpf::System.Windows.FontStyles.Oblique;
            }

            throw new Granular.Exception("Unexpected FontStyle value \"{0}\"", fontStyle);
        }

        public wpf::System.Windows.Media.FontFamily Convert(FontFamily fontFamily)
        {
            string familyName = fontFamily.FamilyNames.FirstOrDefault(name => wpf::System.Windows.Media.Fonts.SystemFontFamilies.Any(font => font.Source.Equals(name)));
            return !familyName.IsNullOrEmpty() ? new wpf::System.Windows.Media.FontFamily(familyName) : new wpf::System.Windows.Media.FontFamily();
        }

        public wpf::System.Windows.TextAlignment Convert(TextAlignment textAlignment)
        {
            switch (textAlignment)
            {
                case TextAlignment.Left: return wpf::System.Windows.TextAlignment.Left;
                case TextAlignment.Right: return wpf::System.Windows.TextAlignment.Right;
                case TextAlignment.Center: return wpf::System.Windows.TextAlignment.Center;
                case TextAlignment.Justify: return wpf::System.Windows.TextAlignment.Justify;
            }

            throw new Granular.Exception("Unexpected TextAlignment value \"{0}\"", textAlignment);
        }

        public wpf::System.Windows.TextTrimming Convert(TextTrimming textTrimming)
        {
            switch (textTrimming)
            {
                case TextTrimming.None: return wpf::System.Windows.TextTrimming.None;
                case TextTrimming.CharacterEllipsis: return wpf::System.Windows.TextTrimming.CharacterEllipsis;
            }

            throw new Granular.Exception("Unexpected TextTrimming value \"{0}\"", textTrimming);
        }

        public wpf::System.Windows.TextWrapping Convert(TextWrapping textWrapping)
        {
            switch (textWrapping)
            {
                case TextWrapping.Wrap: return wpf::System.Windows.TextWrapping.Wrap;
                case TextWrapping.NoWrap: return wpf::System.Windows.TextWrapping.NoWrap;
            }

            throw new Granular.Exception("Unexpected TextWrapping value \"{0}\"", textWrapping);
        }

        public wpf::System.Windows.Controls.ScrollBarVisibility Convert(ScrollBarVisibility scrollBarVisibility)
        {
            switch (scrollBarVisibility)
            {
                case ScrollBarVisibility.Disabled: return wpf::System.Windows.Controls.ScrollBarVisibility.Disabled;
                case ScrollBarVisibility.Auto: return wpf::System.Windows.Controls.ScrollBarVisibility.Auto;
                case ScrollBarVisibility.Hidden: return wpf::System.Windows.Controls.ScrollBarVisibility.Hidden;
                case ScrollBarVisibility.Visible: return wpf::System.Windows.Controls.ScrollBarVisibility.Visible;
            }

            throw new Granular.Exception("Unexpected ScrollBarVisibility value \"{0}\"", scrollBarVisibility);
        }

        public wpf::System.Windows.Media.ImageSource Convert(ImageSource source)
        {
            return ((WpfRenderImageSource)source.RenderImageSource).BitmapImage;
        }

        public MouseButton ConvertBack(wpf::System.Windows.Input.MouseButton mouseButton)
        {
            return (MouseButton)((int)mouseButton);
        }

        public MouseButtonState ConvertBack(wpf::System.Windows.Input.MouseButtonState mouseButtonState)
        {
            return (MouseButtonState)((int)mouseButtonState);
        }

        public Point ConvertBack(wpf::System.Windows.Point point)
        {
            return new Point(point.X, point.Y);
        }

        public Key ConvertBack(wpf::System.Windows.Input.Key key)
        {
            return (Key)((int)key);
        }

        public KeyStates ConvertBack(wpf::System.Windows.Input.KeyStates keyStates)
        {
            return (KeyStates)((int)keyStates & 1);
        }
    }
}