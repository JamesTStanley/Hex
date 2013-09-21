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

        public object Content0
        {
            get { return GetValue(Content0Property); }
            set { SetValue(Content0Property, value); }
        }

        public static readonly DependencyProperty Content0Property =
            DependencyProperty.Register("Content0", typeof(object), typeof(HexControl),
                                        new FrameworkPropertyMetadata(null,
                                        FrameworkPropertyMetadataOptions.AffectsRender));

        public object Content1
        {
            get { return GetValue(Content1Property); }
            set { SetValue(Content1Property, value); }
        }

        public static readonly DependencyProperty Content1Property =
            DependencyProperty.Register("Content1", typeof(object), typeof(HexControl),
                                        new FrameworkPropertyMetadata(null,
                                        FrameworkPropertyMetadataOptions.AffectsRender));

        public object Content2
        {
            get { return GetValue(Content2Property); }
            set { SetValue(Content2Property, value); }
        }

        public static readonly DependencyProperty Content2Property =
            DependencyProperty.Register("Content2", typeof(object), typeof(HexControl),
                                        new FrameworkPropertyMetadata(null,
                                        FrameworkPropertyMetadataOptions.AffectsRender));

        public object Content3
        {
            get { return GetValue(Content3Property); }
            set { SetValue(Content3Property, value); }
        }

        public static readonly DependencyProperty Content3Property =
            DependencyProperty.Register("Content3", typeof(object), typeof(HexControl),
                                        new FrameworkPropertyMetadata(null,
                                        FrameworkPropertyMetadataOptions.AffectsRender));

        public object Content4
        {
            get { return GetValue(Content4Property); }
            set { SetValue(Content4Property, value); }
        }

        public static readonly DependencyProperty Content4Property =
            DependencyProperty.Register("Content4", typeof(object), typeof(HexControl),
                                        new FrameworkPropertyMetadata(null,
                                        FrameworkPropertyMetadataOptions.AffectsRender));

        public object Content5
        {
            get { return GetValue(Content5Property); }
            set { SetValue(Content5Property, value); }
        }

        public static readonly DependencyProperty Content5Property =
            DependencyProperty.Register("Content5", typeof(object), typeof(HexControl),
                                        new FrameworkPropertyMetadata(null,
                                        FrameworkPropertyMetadataOptions.AffectsRender));
        #endregion

        // TODO: Not sure these need to be pulic, or even properties
        public Canvas HexCanvasElement { get; set; }
        public Polygon HexBackgroundElement { get; set; }
        public Polyline[] HexFaces { get; set; }
        public Polygon[] Sextants { get; set; }
        public ContentPresenter ContentPresenter0 { get; set; }
        public ContentPresenter ContentPresenter1 { get; set; }
        public ContentPresenter ContentPresenter2 { get; set; }
        public ContentPresenter ContentPresenter3 { get; set; }
        public ContentPresenter ContentPresenter4 { get; set; }
        public ContentPresenter ContentPresenter5 { get; set; }


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

            ContentPresenter0 = GetTemplateChild("Sextant0Content") as ContentPresenter;
            ContentPresenter1 = GetTemplateChild("Sextant1Content") as ContentPresenter;
            ContentPresenter2 = GetTemplateChild("Sextant2Content") as ContentPresenter;
            ContentPresenter3 = GetTemplateChild("Sextant3Content") as ContentPresenter;
            ContentPresenter4 = GetTemplateChild("Sextant4Content") as ContentPresenter;
            ContentPresenter5 = GetTemplateChild("Sextant5Content") as ContentPresenter;

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

            var cp0Size = _verticies.AsSextantBoundingBox(0);
            ContentPresenter0.Width = cp0Size.Width;
            ContentPresenter0.Height = cp0Size.Height;
            ContentPresenter0.SetValue(Canvas.LeftProperty, cp0Size.Left);
            ContentPresenter0.SetValue(Canvas.TopProperty, cp0Size.Top);
            ContentPresenter0.Clip = _verticies.AsSextantClipGeometry(0);

            var cp1Size = _verticies.AsSextantBoundingBox(1);
            ContentPresenter1.Width = cp1Size.Width;
            ContentPresenter1.Height = cp1Size.Height;
            ContentPresenter1.SetValue(Canvas.LeftProperty, cp1Size.Left);
            ContentPresenter1.SetValue(Canvas.TopProperty, cp1Size.Top);
            ContentPresenter1.Clip = _verticies.AsSextantClipGeometry(1);

            var cp2Size = _verticies.AsSextantBoundingBox(2);
            ContentPresenter2.Width = cp2Size.Width;
            ContentPresenter2.Height = cp2Size.Height;
            ContentPresenter2.SetValue(Canvas.LeftProperty, cp2Size.Left);
            ContentPresenter2.SetValue(Canvas.TopProperty, cp2Size.Top);
            ContentPresenter2.Clip = _verticies.AsSextantClipGeometry(2);

            var cp3Size = _verticies.AsSextantBoundingBox(3);
            ContentPresenter3.Width = cp3Size.Width;
            ContentPresenter3.Height = cp3Size.Height;
            ContentPresenter3.SetValue(Canvas.LeftProperty, cp3Size.Left);
            ContentPresenter3.SetValue(Canvas.TopProperty, cp3Size.Top);
            ContentPresenter3.Clip = _verticies.AsSextantClipGeometry(3);

            var cp4Size = _verticies.AsSextantBoundingBox(4);
            ContentPresenter4.Width = cp4Size.Width;
            ContentPresenter4.Height = cp4Size.Height;
            ContentPresenter4.SetValue(Canvas.LeftProperty, cp4Size.Left);
            ContentPresenter4.SetValue(Canvas.TopProperty, cp4Size.Top);
            ContentPresenter4.Clip = _verticies.AsSextantClipGeometry(4);

            var cp5Size = _verticies.AsSextantBoundingBox(5);
            ContentPresenter5.Width = cp5Size.Width;
            ContentPresenter5.Height = cp5Size.Height;
            ContentPresenter5.SetValue(Canvas.LeftProperty, cp5Size.Left);
            ContentPresenter5.SetValue(Canvas.TopProperty, cp5Size.Top);
            ContentPresenter5.Clip = _verticies.AsSextantClipGeometry(5);

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
