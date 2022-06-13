using NP.Concepts.Behaviors;

namespace TestTransformAndFilteringWithSingleSelection
{
    public class SelectablePerson : SelectableItem<SelectablePerson>
    {
        public Person Person { get; }

        public SelectablePerson(Person person)
        {
            Person = person;
        }
    }

    public class PersonCollectionToSelectablePersonCollectionBehavior : TransformCollectionItemsBehavior<Person, SelectablePerson>
    {
        public PersonCollectionToSelectablePersonCollectionBehavior() : base( person => new SelectablePerson(person), (person, selectablePerson) => person.Equals(selectablePerson.Person))
        {

        }
    }
}
