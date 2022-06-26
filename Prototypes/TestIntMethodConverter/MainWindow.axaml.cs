using Avalonia.Controls;
using NP.Synergy;
using NP.Utilities;
using System.Collections.Generic;

namespace TestIntMethodConverter
{
    public partial class MainWindow : Window
    {
        private Container _container = new Container();

        public MainWindow()
        {
            InitializeComponent();

            _container.SetCell(ObjectIds.IntStr, typeof(string), DataPointDirection.Source, true);
            _container.SetCell(ObjectIds.Int, typeof(int), DataPointDirection.Target, true);

            _container.AddStaticMethodAction
            (
                typeof(IntConverterUtils),
                nameof(IntConverterUtils.StrToInt),
                new Dictionary<string, object>
                {
                    { "str", ObjectIds.IntStr }
                },
                ObjectIds.Int
            );

            RootPanel.DataContext = _container;
        }
    }
}
