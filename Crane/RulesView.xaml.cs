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
            Initialize(NumberOfParts, 3, 25);

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
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("{");
            stringBuilder.AppendLine("    \"inputs\" : [");
            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            \"name\" : \"Distance\",");
            stringBuilder.AppendLine("            \"terms\" : [");

            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine("                    \"function\" : \"(0,1) (0.14,0)\",");
            stringBuilder.AppendLine("                    \"term\" : \"T1\"");
            stringBuilder.AppendLine("                },");

            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine("                    \"function\" : \"(0,0) (0.14,1) (0.28,0)\",");
            stringBuilder.AppendLine("                    \"term\" : \"T\"");
            stringBuilder.AppendLine("                },");

            stringBuilder.AppendLine("                {");
            stringBuilder.AppendLine("                    \"function\" : \"(0.84,0) (1,1)\",");
            stringBuilder.AppendLine("                    \"term\" : \"T0\"");
            stringBuilder.AppendLine("                }]");

            stringBuilder.AppendLine("        }],");
            stringBuilder.AppendLine("    \"name\" : \"Power\",");
            stringBuilder.AppendLine("    \"rules\" : [");

            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            \"function\" : \"(0.1,1) (0.5,0)\",");
            stringBuilder.AppendLine("            \"term\" : \"T0\",");
            stringBuilder.AppendLine("            \"condition\" : \"distance is T0\"");
            stringBuilder.AppendLine("        },");

            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            \"function\" : \"(0,0) (0.25,1) (0.5,0)\",");
            stringBuilder.AppendLine("            \"term\" : \"T\",");
            stringBuilder.AppendLine("            \"condition\" : \"distance is T\"");
            stringBuilder.AppendLine("        },");

            stringBuilder.AppendLine("        {");
            stringBuilder.AppendLine("            \"function\" : \"(1.25,0) (1.5,1)\",");
            stringBuilder.AppendLine("            \"term\" : \"T1\",");
            stringBuilder.AppendLine("            \"condition\" : \"distance is T\"");
            stringBuilder.AppendLine("        }]");
            
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine(string.Empty);

            TapTask.SetRules(stringBuilder.ToString());

            TextBlock.Text = TapTask.GetRules();

        }
    }
}
