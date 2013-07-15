using System;
using System.Threading;
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
        private Timer _timer;

        public CraneView()
        {
            InitializeComponent();

            TopPoints = new double[0];
            BottomPoints = new double[0];
        }

        public double[] TopPoints { get; set; }
        public double[] BottomPoints { get; set; }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var duration = new Duration(TimeSpan.FromSeconds(TopPoints.Length/100.0));
            var maxWidth = Canvas.ActualWidth;
            var maxHeight = Canvas.ActualHeight;

            Line.Y1 = maxHeight - 25;

            var button = sender as Button;
            button.IsEnabled = false;

            var isFinished = false;
            var storyboard = InitializeAnimation(maxWidth, duration);
            storyboard.Completed += (o, args) =>
                {
                    button.IsEnabled = true;
                    isFinished = true;
                };
            storyboard.Begin();

            _timer = new Timer(state => Dispatcher.BeginInvoke(new Action(() =>
                {
                    InfoBlock.Text = string.Format("Progress: {0:P0} Timeline: {1}",
                                                   storyboard.GetCurrentProgress(),
                                                   storyboard.GetCurrentTime());

                    if (isFinished)
                    {
                        _timer.Change(int.MaxValue, int.MaxValue);
                    }
                })));
            _timer.Change(0, 10);
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

                if (topPoint > 1d)
                {
                    topPoint = 1d;
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

                if (topPoint > 1d)
                {
                    topPoint = 1d;
                }

                myDoubleAnimationBottom.KeyFrames.Add(new LinearDoubleKeyFrame
                    {
                        KeyTime = KeyTime.FromPercent((double) i/TopPoints.Length),
                        Value = to*topPoint + BottomPoints[i]*500
                    });
            }

            //myDoubleAnimationBottom.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = KeyTime.FromPercent(0.25), Value = to / 3 });
            //myDoubleAnimationBottom.KeyFrames.Add(new LinearDoubleKeyFrame { KeyTime = KeyTime.FromPercent(1.0), Value = to });

            return myDoubleAnimationBottom;
        }
    }
}
