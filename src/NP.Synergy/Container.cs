using Avalonia.Data;
using NP.Utilities;
using NP.Utilities.Attributes;
using System.Reflection;

namespace NP.Synergy
{
    public class Container
    {
        private Dictionary<object, Cell> _props = new Dictionary<object, Cell>();

        private Dictionary<string, Cell> _bindableProps = new Dictionary<string, Cell>();

        private List<SynergyBehavior> _behaviors = new List<SynergyBehavior>();

        private void CellExistsError(string key) 
        {
            throw new InvalidOperationException($"Programming Error: Cell with a key '{key}' since such cell does not exist.");
        }

        private object? GetValue(object key)
        {
            string keyStr = key.ToStr();
            if (_props.TryGetValue(keyStr, out Cell? cell))
            {
                return cell.Value;
            }

            CellExistsError(keyStr);

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
            CellExistsError(key.Sq());
        }

        internal Cell GetCell(object key)
        {
            if (!_props.ContainsKey(key))
            {
                throw new InvalidOperationException($"Programming Error: cannot get a cell for a key {key} since such cell does not exist.");
            }

            return _props[key];
        }

        internal Cell GetCellByKeyStr(string keyStr)
        {
            if (!_bindableProps.ContainsKey(keyStr))
            {
                throw new InvalidOperationException($"Programming Error: cannot get a cell for a key '{keyStr}' since such cell does not exist.");
            }

            return _bindableProps[keyStr];
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

        internal Cell CreateCell(object key, Type cellType, DataPointDirection direction, bool isBindable)
        {
            if (!isBindable)
            {
                return new Cell(key, cellType, direction);
            }
            
            return new AvaloniaBindableCell(key, cellType, direction);
        }

        public void SetCell(object key, Type cellType, DataPointDirection direction, bool isBindable)
        {
            string keyStr = key.ToStr();
            if (_props.ContainsKey(key) || _bindableProps.ContainsKey(keyStr))
            {
                CellExistsError(keyStr);
            }

            Cell cell = CreateCell(key, cellType, direction, isBindable);

            _props[key] = cell;

            if (isBindable)
            {
                _bindableProps[keyStr] = cell;
            }

        }

        public bool ContainsKey(object key)
        {
            string keyStr = key.ToStr();
            return _props.ContainsKey(keyStr);
        }

        public void AddBehavior(object behavior, IEnumerable<KeyValuePair<string, object>> propMap)
        {
            SynergyBehavior synergyBehavior = 
                new SynergyBehavior(behavior);

            foreach((string propName, object cellKey) in propMap)
            {
                Cell cell = GetCell(cellKey);
                cell.ThrowIfNull($"cell for cell key '{cellKey}' is not found");

                cell.AddBehavior(synergyBehavior, propName);
            }

            this._behaviors.Add(synergyBehavior);
        }
    }
}
