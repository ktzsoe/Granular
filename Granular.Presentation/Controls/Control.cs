﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace System.Windows.Controls
{
    public class Control : FrameworkElement
    {
        public static readonly RoutedEvent PreviewMouseDoubleClickEvent = EventManager.RegisterRoutedEvent("PreviewMouseDoubleClick", RoutingStrategy.Tunnel, typeof(MouseButtonEventHandler), typeof(Control));
        public event MouseButtonEventHandler PreviewMouseDoubleClick
        {
            add { AddHandler(PreviewMouseDoubleClickEvent, value); }
            remove { RemoveHandler(PreviewMouseDoubleClickEvent, value); }
        }

        public static readonly RoutedEvent MouseDoubleClickEvent = EventManager.RegisterRoutedEvent("MouseDoubleClick", RoutingStrategy.Bubble, typeof(MouseButtonEventHandler), typeof(Control));
        public event MouseButtonEventHandler MouseDoubleClick
        {
            add { AddHandler(MouseDoubleClickEvent, value); }
            remove { RemoveHandler(MouseDoubleClickEvent, value); }
        }

        public static readonly DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(ControlTemplate), typeof(Control), new FrameworkPropertyMetadata(propertyChangedCallback: (sender, e) => ((Control)sender).ApplyTemplate()));
        public ControlTemplate Template
        {
            get { return (ControlTemplate)GetValue(TemplateProperty); }
            set { SetValue(TemplateProperty, value); }
        }

        public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(Control));
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static readonly DependencyProperty ForegroundProperty = TextBlock.ForegroundProperty.AddOwner(typeof(Control));
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public static readonly DependencyProperty FontFamilyProperty = TextBlock.FontFamilyProperty.AddOwner(typeof(Control));
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly DependencyProperty FontSizeProperty = TextBlock.FontSizeProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(inherits: true, affectsMeasure: true));
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly DependencyProperty FontStyleProperty = TextBlock.FontStyleProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(inherits: true, affectsMeasure: true));
        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public static readonly DependencyProperty FontWeightProperty = TextBlock.FontWeightProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(inherits: true, affectsMeasure: true));
        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public static readonly DependencyProperty FontStretchProperty = TextBlock.FontStretchProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(inherits: true, affectsMeasure: true));
        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public static readonly DependencyProperty BorderBrushProperty = Border.BorderBrushProperty.AddOwner(typeof(Control));
        public Brush BorderBrush
        {
            get { return (Brush)GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public static readonly DependencyProperty BorderThicknessProperty = Border.BorderThicknessProperty.AddOwner(typeof(Control));
        public Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(Control), new FrameworkPropertyMetadata(HorizontalAlignment.Left));
        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }

        public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", typeof(VerticalAlignment), typeof(Control), new FrameworkPropertyMetadata(VerticalAlignment.Top));
        public VerticalAlignment VerticalContentAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalContentAlignmentProperty); }
            set { SetValue(VerticalContentAlignmentProperty, value); }
        }

        public static readonly DependencyProperty PaddingProperty = Border.PaddingProperty.AddOwner(typeof(Control));
        public Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        static Control()
        {
            FocusableProperty.OverrideMetadata(typeof(Control), new FrameworkPropertyMetadata(true));

            EventManager.RegisterClassHandler(typeof(Control), UIElement.PreviewMouseDownEvent, (MouseButtonEventHandler)OnPreviewMouseDown, true);
            EventManager.RegisterClassHandler(typeof(Control), UIElement.MouseDownEvent, (MouseButtonEventHandler)OnMouseDown, true);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            ControlPropertyMetadata metadata = e.Property.GetMetadata(GetType()) as ControlPropertyMetadata;

            if (metadata != null && metadata.AffectsVisualState)
            {
                UpdateVisualState(true);
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (TemplateChild == null)
            {
                return Size.Zero;
            }

            TemplateChild.Measure(availableSize);
            return TemplateChild.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (TemplateChild != null)
            {
                TemplateChild.Arrange(new Rect(finalSize));
            }

            return finalSize;
        }

        protected override void OnResourcesChanged(ResourcesChangedEventArgs e)
        {
            base.OnResourcesChanged(e);
            SetValue(TemplateProperty, GetDefaultControlTemplate(), BaseValueSource.Default);
        }

        protected virtual void UpdateVisualState(bool useTransitions)
        {
            //
        }

        protected override void OnApplyTemplate()
        {
            UpdateVisualState(false);
        }

        protected override IFrameworkTemplate GetTemplate()
        {
            return Template;
        }

        private ControlTemplate GetDefaultControlTemplate()
        {
            Type type = GetType();

            while (type != typeof(FrameworkElement))
            {
                object value;
                if (TryGetResource(new TemplateKey(type), out value))
                {
                    return value as ControlTemplate;
                }

                type = type.BaseType;
            }

            return null;
        }

        private bool RaiseMouseButtonEvent(MouseButtonEventArgs e, RoutedEvent routedEvent)
        {
            MouseButtonEventArgs eventArgs = new MouseButtonEventArgs(routedEvent, e.OriginalSource, e.MouseDevice, e.Timestamp, e.AbsolutePosition, e.ChangedButton, e.ButtonState, e.ClickCount);
            RaiseEvent(eventArgs);
            return eventArgs.Handled;
        }

        private static void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && ((Control)sender).RaiseMouseButtonEvent(e, PreviewMouseDoubleClickEvent))
            {
                e.Handled = true;
            }
        }

        private static void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && ((Control)sender).RaiseMouseButtonEvent(e, MouseDoubleClickEvent))
            {
                e.Handled = true;
            }
        }
    }
}