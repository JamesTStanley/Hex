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
            ((HexControl)d).UpdateElementLayout();
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
            ((HexControl)d).UpdateElementLayout();
        }

        public double FaceStrokeThickness
        {
            get { return (double)GetValue(FaceStrokeThicknessProperty); }
            set { SetValue(FaceStrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty FaceStrokeThicknessProperty =
            DependencyProperty.Register("FaceStrokeThickness", typeof(double), typeof(HexControl),
                                        new FrameworkPropertyMetadata(1d, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush FaceStroke
        {
            get { return GetValue(FaceStrokeProperty) as Brush; }
            set { SetValue(FaceStrokeProperty, value); }
        }

        public static readonly DependencyProperty FaceStrokeProperty =
            DependencyProperty.Register("FaceStroke", typeof(Brush), typeof(HexControl),
                                        new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), 
                                        FrameworkPropertyMetadataOptions.AffectsRender));
        
        #endregion

        // TODO: Not sure these need to be pulic, or even properties
        public Canvas HexCanvasElement { get; set; }
        public Polygon HexBackgroundElement { get; set; }
        public Polyline[] HexFaces { get; set; }
        public Polygon[] Sextants { get; set; }

        private List<Tuple<double, double>> _verticies;
        

        public override void OnApplyTemplate()
        {
            HexBackgroundElement = GetTemplateChild("HexBackground") as Polygon;
            HexCanvasElement = GetTemplateChild("HexCanvas") as Canvas;
            HexFaces = new Polyline[6];
            HexFaces[0] = GetTemplateChild("HexFace0") as Polyline;
            HexFaces[1] = GetTemplateChild("HexFace1") as Polyline;
            HexFaces[2] = GetTemplateChild("HexFace2") as Polyline;
            HexFaces[3] = GetTemplateChild("HexFace3") as Polyline;
            HexFaces[4] = GetTemplateChild("HexFace4") as Polyline;
            HexFaces[5] = GetTemplateChild("HexFace5") as Polyline;
            Sextants = new Polygon[6];
            Sextants[0] = GetTemplateChild("Sextant0") as Polygon;
            Sextants[1] = GetTemplateChild("Sextant1") as Polygon;
            Sextants[2] = GetTemplateChild("Sextant2") as Polygon;
            Sextants[3] = GetTemplateChild("Sextant3") as Polygon;
            Sextants[4] = GetTemplateChild("Sextant4") as Polygon;
            Sextants[5] = GetTemplateChild("Sextant5") as Polygon;

            base.OnApplyTemplate();
        }

        internal void UpdateElementLayout()
        {
            if (HexBackgroundElement == null)
                return;

            switch (Orientation)
            {
                case HexOrientation.FlatTopped:
                    HexCanvasElement.Width = 2 * VertexRadius;
                    HexCanvasElement.Height = Math.Sqrt(3) * VertexRadius;
                    break;
                case HexOrientation.PointyTopped:
                    HexCanvasElement.Width = Math.Sqrt(3)*VertexRadius;
                    HexCanvasElement.Height = 2 * VertexRadius;
                    break;
            }

            CalculateVerticies();
            HexBackgroundElement.Points = _verticies.AsBoundary();
            for (int i = 0; i <= 5; i++)
            {
                HexFaces[i].Points = _verticies.AsFace(i);
                Sextants[i].Points = _verticies.AsSextant(i);
            }

            HexBackgroundElement.Width = HexCanvasElement.Width;
            HexBackgroundElement.Height = HexCanvasElement.Height;
        }

        private void CalculateVerticies()
        {
            _verticies = new List<Tuple<double, double>>(6);

            for (int i = 0; i < 6; i++)
            {
                double angle;
                if (Orientation == HexOrientation.FlatTopped)
                    angle = 2 * Math.PI / 6 * i;
                else
                    angle = 2 * Math.PI / 6 * (i + 0.5);

                var vertexX = VertexRadius * Math.Cos(angle);
                var vertexY = VertexRadius * Math.Sin(angle);

                // The plus-dimension-divide-by-2 is to get the polygon centered within its canvas which has 0,0 in
                // the UL corner and not the center, as the calculation will yield many negative coordinate values
                _verticies.Add(new Tuple<double, double>(
                                vertexX + HexCanvasElement.Width / 2, 
                                vertexY + HexCanvasElement.Height / 2));
            }
        }
    }
}
