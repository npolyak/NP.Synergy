using System.Collections.ObjectModel;

namespace TestSingleSelection
{
    public class SelectablePeopleTestCollection : ObservableCollection<SelectablePerson>
    {
        private void Add(string firstName, string lastName)
        {
            this.Add(new SelectablePerson(firstName, lastName));
        }

        public SelectablePeopleTestCollection()
        {
            Add("Joe", "Doe");
            Add("Jack", "Matcham");
            Add("Jane", "Dane");
        }
    }
}
