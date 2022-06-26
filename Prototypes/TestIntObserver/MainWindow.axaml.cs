using Avalonia.Controls;
using NP.Synergy;
using NP.Utilities;
using System.Collections.Generic;

namespace TestIntObserver
{
    public partial class MainWindow : Window
    {
        private Container _container = new Container();

        public MainWindow()
        {
            InitializeComponent();

            _container.SetCell(ObjectIds.IntStr, typeof(string), DataPointDirection.Source, true);
            _container.SetCell(ObjectIds.Int, typeof(int), DataPointDirection.Target, true);

            _container.AddAction
            (
                new StrToIntConverter(),
                new Dictionary<string, object>
                {
                    { nameof(StrToIntConverter.TheStr), ObjectIds.IntStr },
                    { nameof(StrToIntConverter.TheVal), ObjectIds.Int }
                }
            );

            RootPanel.DataContext = _container;
        }
    }
}
