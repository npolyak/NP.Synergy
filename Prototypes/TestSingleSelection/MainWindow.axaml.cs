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
                ObjectIds.SelectablePeopleCollection, 
                typeof(ObservableCollection<SelectablePerson>), 
                DataPointDirection.Source, 
                true);

            _container[ObjectIds.SelectablePeopleCollection] = new SelectablePeopleTestCollection();
            _container.SetCell
            (
                ObjectIds.SelectedPerson, 
                typeof(SelectablePerson), 
                DataPointDirection.SourceAndTarget, 
                true);

            _container
                .AddBehavior
                (
                    new SingleSelectionBehavior<SelectablePerson>(),
                    new Dictionary<string, object>
                    {
                        { nameof(SingleSelectionBehavior<SelectablePerson>.TheCollection), ObjectIds.SelectablePeopleCollection },
                        { nameof(SingleSelectionBehavior<SelectablePerson>.TheSelectedItem), ObjectIds.SelectedPerson }
                    }
                );

            RootPanel.DataContext = _container;
        }
    }
}
