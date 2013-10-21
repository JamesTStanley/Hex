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
using System.Windows.Media.Animation;
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
        public ContentControl[] SextantContents { get; set; }
        public ContentPresenter[] SextantContentPresenters { get; set; }

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

            SextantContents = new ContentControl[6];
            SextantContents[0] = GetTemplateChild("Sextant0Content") as ContentControl;
            SextantContents[1] = GetTemplateChild("Sextant1Content") as ContentControl;
            SextantContents[2] = GetTemplateChild("Sextant2Content") as ContentControl;
            SextantContents[3] = GetTemplateChild("Sextant3Content") as ContentControl;
            SextantContents[4] = GetTemplateChild("Sextant4Content") as ContentControl;
            SextantContents[5] = GetTemplateChild("Sextant5Content") as ContentControl;

            SextantContentPresenters = new ContentPresenter[6];
            SextantContentPresenters[0] = GetTemplateChild("Sextant0ContentPresenter") as ContentPresenter;
            SextantContentPresenters[1] = GetTemplateChild("Sextant1ContentPresenter") as ContentPresenter;
            SextantContentPresenters[2] = GetTemplateChild("Sextant2ContentPresenter") as ContentPresenter;
            SextantContentPresenters[3] = GetTemplateChild("Sextant3ContentPresenter") as ContentPresenter;
            SextantContentPresenters[4] = GetTemplateChild("Sextant4ContentPresenter") as ContentPresenter;
            SextantContentPresenters[5] = GetTemplateChild("Sextant5ContentPresenter") as ContentPresenter;

            base.OnApplyTemplate();
        }

        // TODO: Figure out if this should be exposed as a public property (probably)
        // and if the rotation should be handled by its assignment, inferring the
        // rotation direction based on + or - in change?
        private int HexRotationIndex = 0;

        public void RotateClockwise()
        {
            HexRotationIndex = HexRotationIndex == 0 ? 5 : HexRotationIndex - 1;
            var storyBoard = HexCanvasElement.FindResource("RotateHex") as Storyboard;
            ((DoubleAnimation)storyBoard.Children[0]).To -= 60;
            storyBoard.Begin();
        }

        public void RotateCounterClockwise()
        {
            HexRotationIndex = HexRotationIndex == 5 ? 0 : HexRotationIndex + 1;
            var storyBoard = HexCanvasElement.FindResource("RotateHex") as Storyboard;
            ((DoubleAnimation)storyBoard.Children[0]).To += 60;
            storyBoard.Begin();
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

                var size = _verticies.AsSextantBoundingBox(i);
                SextantContents[i].Width = VertexRadius;
                SextantContents[i].Height = size.Height;
                SextantContents[i].SetValue(Canvas.LeftProperty, size.Left);
                SextantContents[i].SetValue(Canvas.TopProperty, size.Top);
                SextantContents[i].Clip = _verticies.AsSextantClipGeometry(i);
            }

            double translateX;
            double translateY;
            double rotateAngle;

            for (int i = 0; i <= 5; i++)
            {
                CalculateContentPresenterTransforms(Orientation, i, out rotateAngle, out translateX, out translateY);

                var stGroup = new TransformGroup();
                stGroup.Children.Add(new RotateTransform(rotateAngle));
                stGroup.Children.Add(new TranslateTransform(translateX, translateY));
                SextantContentPresenters[i].RenderTransformOrigin = new Point(0.5, 0.5);
                SextantContentPresenters[i].RenderTransform = stGroup;
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

        private void CalculateContentPresenterTransforms(HexOrientation orientation, int verticeIndex, out double angle, 
            out double translateX, out double translateY)
        {
            if (Orientation == HexOrientation.FlatTopped)
            {
                switch (verticeIndex)
                {
                    case 0:
                        translateX = -0.125 * VertexRadius;
                        translateY = -0.22 * VertexRadius;
                        angle = -60d;
                        break;
                    case 1:
                        translateX = 0;
                        translateY = 0;
                        angle = 0d;
                        break;
                    case 2:
                        translateX = 0.125 * VertexRadius;
                        translateY = -0.22 * VertexRadius;
                        angle = 60d;
                        break;
                    case 3:
                        translateX = 0.125 * VertexRadius;
                        translateY = 0.22 * VertexRadius;
                        angle = 120;
                        break;
                    case 4:
                        translateX = 0;
                        translateY = 0;
                        angle = 180d;
                        break;
                    case 5:
                    default:
                        translateX = -0.125 * VertexRadius;
                        translateY = 0.22 * VertexRadius;
                        angle = -120d;
                        break;
                } 
            }
            else
            {
                switch (verticeIndex)
                {
                    case 0:
                        translateX = -0.32 * VertexRadius;
                        translateY = -0.18 * VertexRadius;
                        angle = -30d;
                        break;
                    case 1:
                        translateX = 0.18 * VertexRadius;
                        translateY = -0.18 * VertexRadius;
                        angle = 30d;
                        break;
                    case 2:
                        translateX = 0;
                        translateY = 0;
                        angle = 90;
                        break;
                    case 3:
                        translateX = 0.18 * VertexRadius;
                        translateY = 0.18 * VertexRadius;
                        angle = 150d;
                        break;
                    case 4:
                        translateX = -0.32 * VertexRadius;
                        translateY = 0.18 * VertexRadius;
                        angle = -150d;
                        break;
                    case 5:
                    default:
                        translateX = -0.13 * VertexRadius;
                        translateY = 0;
                        angle = -90;
                        break;
                }
            }
        }
    }
}
