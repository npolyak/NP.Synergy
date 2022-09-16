using Avalonia.Controls;
using NP.Synergy;
using NP.Utilities;
using System.Collections.Generic;

namespace TestIntMethodConverter
{
    public partial class MainWindow : Window
    {
        private SynergyAssembly _synergyAssembly = new SynergyAssembly();

        public MainWindow()
        {
            InitializeComponent();

            _synergyAssembly.SetCell(ObjectIds.IntStr, typeof(string), DataPointDirection.Source, true);
            _synergyAssembly.SetCell(ObjectIds.Int, typeof(int), DataPointDirection.Target, true);

            _synergyAssembly.AddStaticMethodAction
            (
                typeof(IntConverterUtils),
                nameof(IntConverterUtils.StrToInt),
                new Dictionary<string, object>
                {
                    { "str", ObjectIds.IntStr }
                },
                ObjectIds.Int
            );

            RootPanel.DataContext = _synergyAssembly;
        }
    }
}
