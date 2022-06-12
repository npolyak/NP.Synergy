using Avalonia.Controls;
using NP.Concepts.Behaviors;
using NP.Synergy;
using NP.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestSingleSelection
{
    public partial class MainWindow : Window
    {
        private Container _container = new Container();

        public MainWindow()
        {
            InitializeComponent();

            _container.SetCell
            (
                ObjectIds.SelectableWritersCollection, 
                typeof(ObservableCollection<SelectablePerson>), 
                DataPointDirection.Source, 
                true);

            _container[ObjectIds.SelectableWritersCollection] = new SelectableWritersTestCollection();
            _container.SetCell
            (
                ObjectIds.SelectedWriter, 
                typeof(SelectablePerson), 
                DataPointDirection.SourceAndTarget, 
                true);

            _container
                .AddBehavior
                (
                    new SingleSelectionBehavior<SelectablePerson>(),
                    new Dictionary<string, object>
                    {
                        { nameof(SingleSelectionBehavior<SelectablePerson>.TheCollection), ObjectIds.SelectableWritersCollection },
                        { nameof(SingleSelectionBehavior<SelectablePerson>.TheSelectedItem), ObjectIds.SelectedWriter }
                    }
                );

            RootPanel.DataContext = _container;
        }
    }
}
