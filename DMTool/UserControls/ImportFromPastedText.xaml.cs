using DMTool.Source;
using System.Windows;

namespace DMTool.UserControls
{
    public partial class ImportFromPastedText : Window
    {
        private Monster _monster;

        public ImportFromPastedText()
        {
            InitializeComponent();

            _monster = new Monster();
            MonsterUserControl.Monster = _monster;
        }

        private Monster ImportDndBeyondCharacter(string text, bool monster = true)
        {
            string name = "";
            string descr = "";
            string hitDice = "";
            string hitPoints = "";

            var lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim().Replace("\r", "");

                if (line.Length == 0)
                {
                    continue;
                }

                if (name == "")
                {
                    name = line;
                    continue;
                }

                if (descr == "")
                {
                    descr = line;
                    continue;
                }
            }

            var m = new Monster();
            m.Name = name;
            m.Description = descr;
            m.HitDice = hitDice;
            m.HitPoints = int.Parse(hitPoints);

            return m;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var saveType = (SaveAsNPC.IsChecked.HasValue && SaveAsNPC.IsChecked.Value) ? SaveType.Character : SaveType.Monster;
            _monster.Save(saveType);
        }

        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            _monster = ImportDndBeyondCharacter(ImportText.Text);
            MonsterUserControl.Monster = _monster;
        }
    }
}
