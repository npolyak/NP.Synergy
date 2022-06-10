using Avalonia.Controls;
using NP.Concepts.Behaviors;
using NP.Synergy;
using System.Collections.ObjectModel;

namespace TestSingleSelection
{
    public partial class MainWindow : Window
    {
        private Container _container = new Container();

        public MainWindow()
        {
            InitializeComponent();

            _container.SetCell(ObjectIds.SelectablePeopleCollection, typeof(ObservableCollection<SelectablePerson>), true);
            _container[ObjectIds.SelectablePeopleCollection] = new SelectablePeopleTestCollection();
            _container.SetCell(ObjectIds.SelectedPerson, typeof(SelectablePerson), true);
            _container
                .AddBehavior
                (
                    new SingleSelectionBehavior<SelectablePerson>(),
                    new (string, object)[]
                    {
                        ( nameof(SingleSelectionBehavior<SelectablePerson>.TheCollection), ObjectIds.SelectablePeopleCollection),
                        ( nameof(SingleSelectionBehavior<SelectablePerson>.TheSelectedItem), ObjectIds.SelectedPerson)
                    }
                );


            RootPanel.DataContext = _container;
        }
    }
}
