using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

namespace Programming.DataStructure
{
    public class Dictionary<tkey, tvalue> : IEnumerable<KeyValuePair<tkey, tvalue>>
    {
        private int?[] _hashBucket;
        private tkey[] _keys;
        private tvalue[] _values;

        private int NextValueIndex { get; set; }

        public int Count { get; private set; }

        public Dictionary()
        {
            InitializeDictionary();
        }

        /// <summary>
        /// Index returns value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public tvalue this[tkey key]
        {
            get { return GetValue(key); }
        }

        /// <summary>
        /// Adds a value to the dictionary indexed by the key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void Add(tkey key, tvalue value)
        {
            if (key == default)
                throw new ArgumentException($"Key cannot be default value for type {typeof(tkey)}", "key");

            CheckValueArray();
            CheckHashArray();

            var hashIndex = GetHashIndex(key, _hashBucket.Length);
            
            if (!_hashBucket[hashIndex].HasValue)
            {
                _hashBucket[hashIndex] = NextValueIndex;
                _keys[NextValueIndex] = key;
                _values[NextValueIndex] = value;
                ++Count;
                ++NextValueIndex;
            }
            else
            {
                throw new InvalidOperationException("Key already exists");
            }
        }

        /// <summary>
        /// Removes key from dictionary if exists
        /// </summary>
        /// <param name="key"></param>
        public void Remove(tkey key)
        {
            var hashIndex = GetHashIndex(key, _hashBucket.Length);
            var valueIndex = _hashBucket[hashIndex];

            if (!valueIndex.HasValue)
                throw new InvalidOperationException("Does not contain key");

            _values[valueIndex.Value] = default;
            _keys[valueIndex.Value] = default;
            _hashBucket[hashIndex] = default;
            --Count;
        }

        /// <summary>
        /// Clears the dictionary of all items
        /// </summary>
        public void Clear()
        {
            InitializeDictionary();
        }

        /// <summary>
        /// Set the dictionary variables to initial values
        /// </summary>
        private void InitializeDictionary()
        {
            Count = 0;
            NextValueIndex = 0;
            _hashBucket = new int?[128];
            _values = new tvalue[15];
            _keys = new tkey[15];
        }

        /// <summary>
        /// Checks that there is enough space in the hash array to add another key
        /// </summary>
        private void CheckHashArray()
        {
            if (_hashBucket.Length - Count <= 5)
            {
                IncreaseHashBucket();
            }
        }

        /// <summary>
        /// Increases the hash bucket size and populates it with the current keys
        /// </summary>
        private void IncreaseHashBucket()
        {
            _hashBucket = new int?[_hashBucket.Length * 2];

            Parallel.For(0, _keys.Length, (i) =>
            {
                if (_keys[i] != null)
                {
                    var hashIndex = GetHashIndex(_keys[i], _hashBucket.Length);

                    //i is the same index for the value the key is tied to 
                    _hashBucket[hashIndex] = i;
                }
            });
        }

        /// <summary>
        /// Checks that the value array has enough room for more values
        /// </summary>
        private void CheckValueArray()
        {
            if (_values.Length - Count <= 5)
            {
                IncreaseValueAndKeyArrays();
            }
        }

        /// <summary>
        /// Increases the size of the value and key arrays
        /// </summary>
        private void IncreaseValueAndKeyArrays()
        {
            var placeHolder = new tvalue[_values.Length * 2];
            Array.Copy(_values, 0, placeHolder, 0, _values.Length); //Can't really get faster than Array.Copy();
            _values = placeHolder;

            var keyHolder = new tkey[_keys.Length * 2];
            Array.Copy(_keys, 0, keyHolder, 0, _keys.Length);
            _keys = keyHolder;
        }

        /// <summary>
        /// Get's the value based on the key's hashcode
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private tvalue GetValue(tkey key)
        {
            var hashIndex = GetHashIndex(key, _hashBucket.Length);
            var valueIndex = _hashBucket[hashIndex];

            if (!valueIndex.HasValue)
                throw new InvalidOperationException("Does not contain key");

            return _values[valueIndex.Value];
        }

        /// <summary>
        /// Calculates what the index should be based on the hashcode
        /// </summary>
        /// <param name="key"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        private int GetHashIndex(tkey key, int maxLength)
        {
            var keyHash = Math.Abs(key.GetHashCode());
            var hashIndex = keyHash % maxLength;

            return hashIndex;
        }

        /// <summary>
        /// Returns key value pairs of saved values
        /// </summary>
        /// <returns></returns>
        IEnumerator<KeyValuePair<tkey, tvalue>> IEnumerable<KeyValuePair<tkey, tvalue>>.GetEnumerator()
        {
            for (int i = 0; i < _keys.Length; i++)
            {
                if (_keys[i] == default) continue;

                yield return new KeyValuePair<tkey, tvalue>(_keys[i], _values[i]);
            }
        }

        /// <summary>
        /// returns implemented enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in this)
            {
                yield return item;
            }
        }
    }
}
