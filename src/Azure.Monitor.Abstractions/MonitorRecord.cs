using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Azure.Monitor.Abstractions
{
    [DataContract]
    public class MonitorRecord : IDictionary<string, object>
    {
        [DataMember]
        public string Parent { get; }

        [DataMember]
        public string ResourceName { get; }

        [DataMember]
        public string ResourceType { get; }

        [DataMember]
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public MonitorRecord(string parent, string resourceName, string resourceType)
        {
            Parent = parent;
            ResourceName = resourceName;
            ResourceType = resourceType;
        }

        #region Dictionary properties and methods

        public ICollection<string> Keys => Properties.Keys;

        public ICollection<object> Values => Properties.Values;

        public int Count => Properties.Count;

        public bool IsReadOnly => Properties.IsReadOnly;

        public object this[string key] { get => Properties[key]; set => Properties[key] = value; }

        public void Add(string key, object value) => Properties.Add(key, value);

        public bool ContainsKey(string key) => Properties.ContainsKey(key);

        public bool Remove(string key) => Properties.Remove(key);

        public bool TryGetValue(string key, out object value) => Properties.TryGetValue(key, out value);

        public void Add(KeyValuePair<string, object> item) => Properties.Add(item);

        public void Clear() => Properties.Clear();

        public bool Contains(KeyValuePair<string, object> item) => Properties.Contains(item);

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => Properties.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<string, object> item) => Properties.Remove(item);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => Properties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Properties.GetEnumerator();

        #endregion
    }
}