using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

namespace Programming.DataStructure
{
    public class Dictionary<tkey, tvalue> : IEnumerable<KeyValuePair<tkey, tvalue>>
    {
        private KeyValuePair<tkey, int?>[] _hashBucket;
        private tvalue[] _valueList;

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
            CheckValueArray();
            CheckHashArray();

            var hashIndex = GetHashIndex(key, _hashBucket.Length);
            
            if (!_hashBucket[hashIndex].Value.HasValue)
            {
                _hashBucket[hashIndex] = new KeyValuePair<tkey, int?>(key, NextValueIndex);
                _valueList[NextValueIndex] = value;
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

            if (!valueIndex.Value.HasValue)
                throw new InvalidOperationException("Does not contain key");

            _valueList[valueIndex.Value.Value] = default;
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
            _hashBucket = new KeyValuePair<tkey, int?>[128];
            _valueList = new tvalue[5];
        }

        /// <summary>
        /// Checks that there is enough space in the hash array to add another key
        /// </summary>
        private void CheckHashArray()
        {
            if (_hashBucket.Length - Count <= 5)
            {
                //TODO: figure out a better way of increasing bucket limit size instead of doubling
                var placeHolder = new KeyValuePair<tkey, int?>[_hashBucket.Length * 2];
                CopyHashArray(_hashBucket, placeHolder);
                _hashBucket = placeHolder;
            }
        }

        /// <summary>
        /// Copys the current hash table and regrabs the hash index
        /// </summary>
        /// <param name="primary"></param>
        /// <param name="secondary"></param>
        private void CopyHashArray(KeyValuePair<tkey, int?>[] primary, KeyValuePair<tkey, int?>[] secondary)
        {
            Parallel.For(0, primary.Length, (i) =>
            {
                if (primary[i].Value.HasValue)
                {
                    var hashIndex = GetHashIndex(primary[i].Key, secondary.Length);
                    secondary[hashIndex] = primary[i];
                }
            });
        }

        /// <summary>
        /// Checks that the value array has enough room for more values
        /// </summary>
        private void CheckValueArray()
        {
            if (_valueList.Length - Count <= 5)
            {
                var placeHolder = new tvalue[_valueList.Length * 2];
                Array.Copy(_valueList, 0, placeHolder, 0, _valueList.Length); //Can't really get faster than Array.Copy();
                _valueList = placeHolder;
            }
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

            if (!valueIndex.Value.HasValue)
                throw new InvalidOperationException("Does not contain key");

            return _valueList[valueIndex.Value.Value];
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
            for (int i = 0; i < _hashBucket.Length; i++)
            {
                if (!_hashBucket[i].Value.HasValue) continue;

                yield return new KeyValuePair<tkey, tvalue>(_hashBucket[i].Key, _valueList[_hashBucket[i].Value.Value]);
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
