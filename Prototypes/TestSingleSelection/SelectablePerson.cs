using NP.Concepts.Behaviors;
using NP.Utilities;
using System;

namespace TestSingleSelection
{
    public class SelectablePerson : VMBase, ISelectableItem<SelectablePerson>
    {
        public string FirstName { get; }
        public string LastName { get; }

        public int Born { get; }

        public int Died { get; }

        public SelectablePerson(string firstName, string lastName, int born, int died)
        {
            FirstName = firstName;
            LastName = lastName;

            Born = born;
            Died = died;
        }

        private bool _selected;
        public bool IsSelected 
        { 
            get => _selected;
            set 
            {
                if (_selected == value)
                    return;

                _selected = value;

                IsSelectedChanged?.Invoke(this);

                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event Action<SelectablePerson>? IsSelectedChanged;

        public void Select()
        {
            IsSelected = true;
        }

        public void ToggleSelection()
        {
            IsSelected = !IsSelected;
        }
    }
}
