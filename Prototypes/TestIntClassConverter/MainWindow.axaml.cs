using Avalonia.Controls;
using NP.Synergy;
using NP.Utilities;
using System.Collections.Generic;

namespace TestIntClassConverter
{
    public partial class MainWindow : Window
    {
        private SynergyAssembly _assembly = new SynergyAssembly();

        public MainWindow()
        {
            InitializeComponent();

            _assembly.SetCell(ObjectIds.IntStr, typeof(string), DataPointDirection.Source, true);
            _assembly.SetCell(ObjectIds.Int, typeof(int), DataPointDirection.Target, true);

            _assembly.AddAction
            (
                new StrToIntConverter(),
                new Dictionary<string, object>
                {
                    { nameof(StrToIntConverter.TheStr), ObjectIds.IntStr },
                    { nameof(StrToIntConverter.TheVal), ObjectIds.Int }
                }
            );

            RootPanel.DataContext = _assembly;
        }
    }
}
