using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Collections
{
    public class OrderedDictionary<K, V> : IDictionary<K, V>
    {
        private readonly IDictionary<K, LinkedListNode<(K Key, V Value)>> _dictionary;
        private readonly LinkedList<(K Key, V Value)> _linkedList;

        public OrderedDictionary()
            : this(EqualityComparer<K>.Default)
        {
        }

        public OrderedDictionary(IEqualityComparer<K> comparer)
        {
            _dictionary = new Dictionary<K, LinkedListNode<(K Key, V Value)>>(comparer);
            _linkedList = new LinkedList<(K Key, V Value)>();
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return _dictionary.IsReadOnly; }
        }

        public ICollection<K> Keys => _linkedList.Select(i => i.Key).ToList();

        public ICollection<V> Values => _linkedList.Select(i => i.Value).ToList();

        public V this[K key]
        {
            get => _dictionary[key].Value.Value;
            set => AddOrUpdate(key, value, (_, __) => value);
        }

        public void Clear()
        {
            _linkedList.Clear();
            _dictionary.Clear();
        }

        public bool Remove(K key)
        {
            LinkedListNode<(K Key, V Value)> node;
            bool found = _dictionary.TryGetValue(key, out node);
            if (!found) return false;
            _dictionary.Remove(key);
            _linkedList.Remove(node);
            return true;
        }

        public IEnumerator<V> GetEnumerator()
        {
            return _linkedList.Select(i => i.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _linkedList.Select(i => i.Value).GetEnumerator();
        }

        public void CopyTo(V[] array, int arrayIndex)
        {
            _linkedList.Select(i => i.Value).ToList().CopyTo(array, arrayIndex);
        }

        public bool Add(K key, V item)
        {
            if (_dictionary.ContainsKey(key)) return false;
            LinkedListNode<(K Key, V Value)> node = _linkedList.AddLast((key, item));
            _dictionary.Add(key, node);
            return true;
        }

        void IDictionary<K, V>.Add(K key, V value)
        {
            Add(key, value);
        }

        public V AddOrUpdate(K key, V item, Func<K, V, V> updateFunction)
        {
            if (_dictionary.ContainsKey(key)) {
                var node = _dictionary[key];
                var newItem = updateFunction(key, node.Value.Value);
                node.Value = (key, newItem);
                return newItem;
            }
            else
            {
                LinkedListNode<(K Key, V Value)> node = _linkedList.AddLast((key, item));
                _dictionary[key] = node;
                return item;
            }
        }

        public bool ContainsKey(K key) => _dictionary.ContainsKey(key);

        public bool TryGetValue(K key, out V value)
        {
            value = default(V);

            if (ContainsKey(key))
            {
                value = _dictionary[key].Value.Value;
                return true;
            }

            return false;
        }

        public void Add(KeyValuePair<K, V> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<K, V> item) => _dictionary.ContainsKey(item.Key);

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex) => _linkedList.Select(i => new KeyValuePair<K, V>(i.Key, i.Value)).ToList().CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<K, V> item) => Remove(item.Key);

        IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator() => _linkedList.Select(i => new KeyValuePair<K, V>(i.Key, i.Value)).ToList().GetEnumerator();
    }
}