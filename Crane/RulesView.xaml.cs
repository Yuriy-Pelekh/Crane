using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Crane.Core;

namespace Crane
{
    /// <summary>
    /// Interaction logic for RulesView.xaml
    /// </summary>
    public partial class RulesView : UserControl
    {
        private const int Min = 0;
        private const int Delta = 5;
        private const int Step = 1;

        public RulesView()
        {
            InitializeComponent();

            Initialize(MinPower, Min);
            Initialize(MaxPower, Min + Step);
            Initialize(NumberOfParts, 3, 97);

            TextBlock.Text = TapTask.GetRules();
        }

        private static void Initialize(Selector selector, int min, int delta = Delta)
        {
            selector.Items.Clear();

            for (var i = min; i <= min + delta; i += Step)
            {
                selector.Items.Add(i);
            }

            selector.SelectedIndex = 0;
        }

        private void MinPower_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Initialize(MaxPower, (int) e.AddedItems[0] + Step);
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            double min = (int) MinPower.SelectedItem;
            double max = (int) MaxPower.SelectedItem;
            double count = (int) NumberOfParts.SelectedItem;

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("{\"inputs\" : [{");
            stringBuilder.AppendLine("\t\"name\" : \"Distance\",");
            stringBuilder.AppendLine("\t\"terms\" : [");

            var distanceStep = 1d/count;
            var f = new Function(-distanceStep, 0, distanceStep);

            for (var i = distanceStep; f.Index <= count; i += distanceStep)
            {
                if (f.Index > 0)
                {
                    stringBuilder.AppendLine(",");
                }
                stringBuilder.AppendLine("\t{");
                stringBuilder.AppendLine(string.Format("\t\t\"function\" : \"({0},0) ({1},1) ({2},0)\",", f.Previouse, f.Current, f.Next));
                stringBuilder.AppendLine(string.Format("\t\t\"term\" : \"T{0}\"", f.Index));
                stringBuilder.Append("\t}");
                f.SetNewValue(i + distanceStep);
            }
            
            stringBuilder.AppendLine("]");
            stringBuilder.AppendLine("}],");
            stringBuilder.AppendLine("\"name\" : \"Power\",");
            stringBuilder.AppendLine("\"rules\" : [");

            var powerStep = (max - min)/count;
            f = new Function(-powerStep, 0, powerStep);

            for (var i = powerStep; f.Index <= count; i += powerStep)
            {
                if (f.Index > 0)
                {
                    stringBuilder.AppendLine(",");
                }
                stringBuilder.AppendLine("{");
                stringBuilder.AppendLine(string.Format("\t\t\"function\" : \"({0},0) ({1},1) ({2},0)\",", f.Previouse, f.Current, f.Next));
                stringBuilder.AppendLine(string.Format("\t\t\"term\" : \"T{0}\",", f.Index));
                stringBuilder.AppendLine(string.Format("\t\t\"condition\" : \"distance is T{0}\"", count - f.Index));
                stringBuilder.Append("}");
                f.SetNewValue(i + powerStep);
            }

            stringBuilder.AppendLine("]}");

            TapTask.SetRules(stringBuilder.ToString());
            TextBlock.Text = TapTask.GetRules();
        }
    }

    public class Function
    {
        public Function(double previouse, double current, double next)
        {
            Previouse = previouse;
            Current = current;
            Next = next;
        }

        public double Previouse { get; private set; }
        public double Current { get; private set; }
        public double Next { get; private set; }
        public int Index { get; private set; }

        public void SetNewValue(double value)
        {
            Previouse = Current;
            Current = Next;
            Next = value;
            Index++;
        }
    }
}
