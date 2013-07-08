using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using Crane.Core;

namespace Crane
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void ButtonExecuteClick(object sender, RoutedEventArgs e)
    {
      var solverType = radioButtonEuler.IsChecked.Value
                         ? Solvers.Euler
                         : Solvers.RungeKutta;

      var sw = new Stopwatch();
      sw.Start();
      List<CranePosition> result;
      try
      {
        result = TapTask.Execute(solverType);
      }
      catch(Exception ex)
      {
        sw.Stop();

        MessageBox.Show(ex.Message);
        return;
      }

      sw.Stop();

      Title = string.Format(CultureInfo.InvariantCulture, "Time elapsed: {0} ms.", sw.ElapsedMilliseconds);

      dataGrid.ItemsSource = result;

      craneView.TopPoints = result.Select(c => c.Distance).ToArray();
      craneView.BottomPoints = result.Select(c => c.Angle).ToArray();
    }
  }
}
