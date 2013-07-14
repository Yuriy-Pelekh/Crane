using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using Crane.Core;

namespace Crane
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private Solvers _solverType;
        private string _title;
        private List<CranePosition> _result;

        public MainWindow()
        {
            InitializeComponent();

            _worker.DoWork += Worker_DoWork;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            RulesView.TextBlock.Text = TapTask.GetRules();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Execute();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Title = _title ?? string.Empty;

            DataGrid.ItemsSource = _result;

            if (_result == null)
            {
                MessageBox.Show("Something went wrong. Result is empty.", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                CraneView.TopPoints = _result.Select(c => c.Distance).ToArray();
                CraneView.BottomPoints = _result.Select(c => c.Angle).ToArray();
            }

            Spinner.Visibility = Visibility.Hidden;
        }

        private void ButtonExecuteClick(object sender, RoutedEventArgs e)
        {
            if (!_worker.IsBusy)
            {
                _solverType = RadioButtonEuler.IsChecked.HasValue && RadioButtonEuler.IsChecked.Value
                                  ? Solvers.Euler
                                  : Solvers.RungeKutta;

                Spinner.Visibility = Visibility.Visible;
                _worker.RunWorkerAsync();
            }
        }

        private void Execute()
        {
            var sw = new Stopwatch();
            sw.Start();

            try
            {
                _result = TapTask.Execute(_solverType);
            }
            catch (Exception ex)
            {
                sw.Stop();
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (sw.IsRunning)
                {
                    sw.Stop();
                }
            }

            _title = string.Format(CultureInfo.InvariantCulture, "Time elapsed: {0} ms. (version: {1})",
                                  sw.ElapsedMilliseconds, GetAssemblyVersion());
        }

        private static string GetAssemblyVersion()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var assemblyVersion = executingAssembly.FullName.Split(',')[1].Split('=')[1];
            return assemblyVersion;
        }
    }
}
