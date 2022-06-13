using System.Collections.ObjectModel;

namespace TestTransformAndFilteringWithSingleSelection
{
    public class WritersTestCollection : ObservableCollection<Person>
    {
        private void Add(string firstName, string lastName, int born, int died)
        {
            this.Add(new Person(firstName, lastName, born, died));
        }

        public WritersTestCollection()
        {
            Add("William", "Shakespear", 1564, 1616);
            Add("Robert", "Stevenson", 1850, 1894);
            Add("Rudyard", "Kipling", 1865, 1936);
            Add("Arthur", "Conan Doyle", 1859, 1930);
        }
    }
}
