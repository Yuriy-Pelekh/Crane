using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Crane
{
    /// <summary>
    /// Interaction logic for Spinner.xaml
    /// </summary>
    public partial class Spinner : UserControl
    {
        private Storyboard _storyboard;

        public Spinner()
        {
            InitializeComponent();
            IsVisibleChanged += OnVisibleChanged;
        }

        private void OnVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                StartAnimation();
            }
            else
            {
                StopAnimation();
            }
        }

        private void StartAnimation()
        {
            _storyboard = (Storyboard)FindResource("CanvasAnimation");
            _storyboard.Begin(Canvas, true);
        }

        private void StopAnimation()
        {
            _storyboard.Remove(Canvas);
        }
    }
}
