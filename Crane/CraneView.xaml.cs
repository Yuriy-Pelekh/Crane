using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Crane
{
    /// <summary>
    /// Interaction logic for CraneView.xaml
    /// </summary>
    public partial class CraneView : UserControl
    {
        public CraneView()
        {
            InitializeComponent();
        }

        public double[] TopPoints { get; set; }
        public double[] BottomPoints { get; set; }

        private void ButtonRunClick(object sender, RoutedEventArgs e)
        {
            var duration = new Duration(TimeSpan.FromSeconds(TopPoints.Length/100.0));
            var maxWidth = Canvas.ActualWidth - 10;
            var maxHeight = Canvas.ActualHeight - 10;

            Line.Y1 = maxHeight - 25;

            var sb = InitializeAnimation(maxWidth, duration);
            sb.Begin();
        }

        private Storyboard InitializeAnimation(double to, Duration duration)
        {
            var storyboard = new Storyboard {Duration = duration};

            var myDoubleAnimationTop = InitializeBottomAnimation(to, duration);
            var myDoubleAnimationBottom = InitializeTopAnimation(to, duration);

            storyboard.Children.Add(myDoubleAnimationTop);
            storyboard.Children.Add(myDoubleAnimationBottom);

            Storyboard.SetTarget(myDoubleAnimationTop, Line);
            Storyboard.SetTarget(myDoubleAnimationBottom, Line);

            Storyboard.SetTargetProperty(myDoubleAnimationTop, new PropertyPath("(X1)"));
            Storyboard.SetTargetProperty(myDoubleAnimationBottom, new PropertyPath("(X2)"));

            return storyboard;
        }

        private DoubleAnimationUsingKeyFrames InitializeTopAnimation(double to, Duration duration)
        {
            var myDoubleAnimationTop = new DoubleAnimationUsingKeyFrames {Duration = duration};

            for (var i = 0; i < TopPoints.Length; i++)
            {
                var topPoint = TopPoints[i];

                if (topPoint > 1.0)
                {
                    topPoint = 1.0;
                }

                myDoubleAnimationTop.KeyFrames.Add(new LinearDoubleKeyFrame
                    {
                        KeyTime = KeyTime.FromPercent((double) i/TopPoints.Length),
                        Value = to*topPoint
                    });
            }

            //myDoubleAnimationTop.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = KeyTime.FromPercent(0.4), Value = to / 5 });
            //myDoubleAnimationTop.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = KeyTime.FromPercent(0.8), Value = to / 2 });
            //myDoubleAnimationTop.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = KeyTime.FromPercent(1.0), Value = to });

            return myDoubleAnimationTop;
        }

        private DoubleAnimationUsingKeyFrames InitializeBottomAnimation(double to, Duration duration)
        {
            var myDoubleAnimationBottom = new DoubleAnimationUsingKeyFrames {Duration = duration};

            for (var i = 0; i < TopPoints.Length; i++)
            {
                var topPoint = TopPoints[i];

                if (topPoint > 1.0)
                {
                    topPoint = 1.0;
                }
                myDoubleAnimationBottom.KeyFrames.Add(new LinearDoubleKeyFrame
                    {
                        KeyTime = KeyTime.FromPercent((double) i/TopPoints.Length),
                        Value = to*topPoint + BottomPoints[i]*100
                    });
            }

            //myDoubleAnimationBottom.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = KeyTime.FromPercent(0.25), Value = to / 3 });
            //myDoubleAnimationBottom.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = KeyTime.FromPercent(1.0), Value = to });

            return myDoubleAnimationBottom;
        }
    }
}
