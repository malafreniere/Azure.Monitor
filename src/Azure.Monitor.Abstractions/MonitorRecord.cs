using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Azure.Monitor.Abstractions
{
    [DataContract]
    public class MonitorRecords
    {
        [DataMember(Name = "Records")]
        public MonitorRecord[] Records { get; set; }
    }

    [DataContract]
    public class MonitorRecord : IDictionary<string, object>
    {
        [DataMember(Name = "Parent")]
        internal string _parent;

        [DataMember(Name = "ResourceName")]
        internal string _resourceName;

        [DataMember(Name = "ResourceType")]
        internal string _resourceType;

        [DataMember(Name = "Properties")]
        internal IDictionary<string, object> _properties = new Dictionary<string, object>();

        public string Parent => _parent;

        public string ResourceName => _resourceName;

        public string ResourceType => _resourceType;

        public IDictionary<string, object> Properties => _properties;

        internal MonitorRecord() { }

        public MonitorRecord(string parent, string resourceName, string resourceType)
        {
            _parent = parent;
            _resourceName = resourceName;
            _resourceType = resourceType;
        }

        #region Dictionary properties and methods

        public ICollection<string> Keys => _properties.Keys;

        public ICollection<object> Values => _properties.Values;

        public int Count => _properties.Count;

        public bool IsReadOnly => _properties.IsReadOnly;

        public object this[string key] { get => Properties[key]; set => _properties[key] = value; }

        public void Add(string key, object value) => _properties.Add(key, value);

        public bool ContainsKey(string key) => _properties.ContainsKey(key);

        public bool Remove(string key) => _properties.Remove(key);

        public bool TryGetValue(string key, out object value) => _properties.TryGetValue(key, out value);

        public void Add(KeyValuePair<string, object> item) => _properties.Add(item);

        public void Clear() => _properties.Clear();

        public bool Contains(KeyValuePair<string, object> item) => _properties.Contains(item);

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => _properties.CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<string, object> item) => _properties.Remove(item);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => Properties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Properties.GetEnumerator();

        #endregion
    }
}