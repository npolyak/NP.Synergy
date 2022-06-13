using Avalonia.Controls;
using NP.Concepts.Behaviors;
using NP.Synergy;
using NP.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TestTransformAndFilteringWithSingleSelection
{
    public partial class MainWindow : Window
    {
        private Container _container = new Container();

        public MainWindow()
        {
            InitializeComponent();

            _container.SetCell
            (
                ObjectIds.WritersCollection, 
                typeof(ObservableCollection<Person>), 
                DataPointDirection.Source, 
                true);

            _container[ObjectIds.WritersCollection] = new WritersTestCollection();

            _container.SetCell
            (
                ObjectIds.BornFromYear,
                typeof(int?),
                DataPointDirection.Source,
                true
            );

            _container.SetCell
            (
                ObjectIds.BornToYear,
                typeof(int?),
                DataPointDirection.Source,
                true
            );


            _container.SetCell
            (
                ObjectIds.FilteredWritersCollection, 
                typeof(IEnumerable<Person>), 
                DataPointDirection.SourceAndTarget, 
                true);

            _container.SetCell
            (
                ObjectIds.SelectableFilteredWritersCollection,
                typeof(IEnumerable<Person>),
                DataPointDirection.SourceAndTarget,
                true);

            _container.SetCell
            (
                ObjectIds.SelectedWriter,
                typeof(SelectablePerson),
                DataPointDirection.Target,
                true);

            _container.AddBehavior
            (
                new FilterPersonCollectionByBornDateBehavior(),
                new Dictionary<string, object>
                {
                    { nameof(FilterPersonCollectionByBornDateBehavior.InputCollection), ObjectIds.WritersCollection },
                    { nameof(FilterPersonCollectionByBornDateBehavior.OutputCollection), ObjectIds.FilteredWritersCollection },
                    { nameof(FilterPersonCollectionByBornDateBehavior.FromYear), ObjectIds.BornFromYear },
                    { nameof(FilterPersonCollectionByBornDateBehavior.ToYear), ObjectIds.BornToYear }
                }
            );

            _container.AddBehavior
            (
                new PersonCollectionToSelectablePersonCollectionBehavior(),
                new Dictionary<string, object>
                {
                    { nameof(PersonCollectionToSelectablePersonCollectionBehavior.InputCollection), ObjectIds.FilteredWritersCollection },
                    { nameof(PersonCollectionToSelectablePersonCollectionBehavior.OutputCollection), ObjectIds.SelectableFilteredWritersCollection }
                }
            );

            _container.AddBehavior
            (
                new SingleSelectionBehavior<SelectablePerson>(),
                new Dictionary<string, object>
                {
                    {nameof(SingleSelectionBehavior<SelectablePerson>.TheCollection), ObjectIds.SelectableFilteredWritersCollection },
                    {nameof(SingleSelectionBehavior<SelectablePerson>.TheSelectedItem), ObjectIds.SelectedWriter }
                }
            );

            RootPanel.DataContext = _container;
        }
    }
}
