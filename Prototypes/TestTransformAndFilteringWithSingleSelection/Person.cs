using NP.Utilities;

namespace TestTransformAndFilteringWithSingleSelection
{
    public class Person
    {
        public string FirstName { get; }
        public string LastName { get; }

        public int Born { get; }

        public int Died { get; }

        public Person(string firstName, string lastName, int born, int died)
        {
            FirstName = firstName;
            LastName = lastName;

            Born = born;
            Died = died;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Person p)
            {
                return FirstName == p.FirstName && LastName == p.LastName;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return FirstName.GetHashCodeExtension() ^ LastName.GetHashCodeExtension();
        }
    }
}
