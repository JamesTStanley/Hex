using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
using Hex;

namespace HexControlWpf
{
    public class HexControl : Control
    {
        static HexControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HexControl), new FrameworkPropertyMetadata(typeof(HexControl)));
        }

        #region Dependency Properties

        public HexOrientation Orientation
        {
            get { return (HexOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof (HexOrientation), typeof (HexControl),
                                        new FrameworkPropertyMetadata(HexOrientation.FlatTopped,
                                                                      FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                      FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                      FrameworkPropertyMetadataOptions.AffectsRender,
                                                                      OnHexOrientationPropertyChanged));

        private static void OnHexOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        public double VertexRadius
        {
            get { return (double)GetValue(VertexRadiusProperty); }
            set { SetValue(VertexRadiusProperty, value); }
        }

        public static readonly DependencyProperty VertexRadiusProperty =
            DependencyProperty.Register("VertexRadius", typeof (double), typeof (HexControl),
                                        new FrameworkPropertyMetadata(double.NaN,
                                                                      FrameworkPropertyMetadataOptions.AffectsArrange |
                                                                      FrameworkPropertyMetadataOptions.AffectsMeasure |
                                                                      FrameworkPropertyMetadataOptions.AffectsRender,
                                                                      OnVertexRadiusPropertyChanged),
                                        OnVertexRadiusPropertyValidate);

        private static bool OnVertexRadiusPropertyValidate(object value)
        {
            return ((double) value > 0) || double.IsNaN((double)value);
        }

        private static void OnVertexRadiusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((HexControl)d).CalculateVerticies();
            //((HexControl) d).InvalidateVisual();
        }

        #endregion

        private Polygon _hexBackgroundElement;
        public Polygon HexBackgroundElement
        {
            get { return _hexBackgroundElement; }
            set 
            {
                _hexBackgroundElement = value; 
                if (_hexBackgroundElement != null)
                    CalculateVerticies();
            }
        }

        public override void OnApplyTemplate()
        {
            HexBackgroundElement = GetTemplateChild("HexBackground") as Polygon;
            base.OnApplyTemplate();
        }

        internal void CalculateVerticies()
        {
            var verticies = new PointCollection(6);

            for (int i = 0; i < 6; i++)
            {
                double angle;
                if (Orientation == HexOrientation.FlatTopped)
                    angle = 2 * Math.PI / 6 * i;
                else
                    angle = 2 * Math.PI / 6 * (i + 0.5);

                var vertexX = VertexRadius * Math.Cos(angle);
                var vertexY = VertexRadius * Math.Sin(angle);

                verticies.Add(new Point(vertexX, vertexY));
            }

            HexBackgroundElement.Points = verticies;
        }
    }
}
