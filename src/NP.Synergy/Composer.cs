using Avalonia.Data;
using NP.Utilities;

namespace NP.Synergy
{
    public class Composer
    {
        private HashSet<string> _stringKeys = new HashSet<string>();
        private Dictionary<object, Cell> _props = new Dictionary<object, Cell>();

        private void CellExistsError(object key) 
        {
            throw new InvalidOperationException($"Programming Error: Cell with a key '{key}' since such cell does not exist.");
        }

        private object? GetValue(object key)
        {
            if (_props.TryGetValue(key, out Cell? cell))
            {
                return cell.Value;
            }

            CellExistsError(key);

            return null; // never get to here (just for the method to compile)
        }

        private void SetValue(object key, object? value)
        {
            if (_props.TryGetValue(key, out Cell? cell))
            {
                if (value.ObjEquals(cell?.Value))
                {
                    return;
                }

                cell!.SetValue(value, BindingPriority.LocalValue);
                return;
            }
            // cell does not exist
            CellExistsError(key);
        }

        internal Cell GetCell(object key)
        {
            if (!_props.ContainsKey(key))
            {
                throw new InvalidOperationException($"Programming Error: cannot get a cell for a key '{key}' since such cell does not exist.");
            }

            return _props[key];
        }


        public object? this[object key]
        {
            get 
            { 
                return GetValue(key);
            }

            set
            {
                SetValue(key, value);
            }
        }

        protected virtual Cell CreateCell(object key, Type cellType, bool isBindable)
        {
            if (!isBindable)
            {
                return new Cell(key, cellType);
            }
            
            return new BindableCell(key, cellType);
        }

        public void SetCell(object key, Type cellType, bool isBindable)
        {
            if (_stringKeys.Contains(key.ToStr()))
            {
                CellExistsError(key);
            }

            Cell cell = CreateCell(key, cellType, isBindable);

            _props[key] = cell;
        }

        public bool ContainsKey(object key)
        {
            return _props.ContainsKey(key);
        }
    }
}
