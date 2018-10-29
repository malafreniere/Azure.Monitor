using System;
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

    [CollectionDataContract(Name = "Properties", ItemName = "Property", KeyName = "Key", ValueName = "Value")]
    public class RecordProperties : Dictionary<string, object> { }

    [DataContract]
    public class MonitorRecord : IReadOnlyDictionary<string, object>
    {
        [DataMember(Name = "Parent")]
        internal string _parent;

        [DataMember(Name = "ResourceName")]
        internal string _resourceName;

        [DataMember(Name = "ResourceType")]
        internal string _resourceType;

        [DataMember(Name = "Properties")]
        internal RecordProperties _properties = new RecordProperties();

        public string Parent => _parent;

        public string ResourceName => _resourceName;

        public string ResourceType => _resourceType;

        public IDictionary<string, object> Properties => _properties;

        public object this[string key] => _properties[key];

        internal MonitorRecord() { }

        public MonitorRecord(string parent, string resourceName, string resourceType)
        {
            _parent = parent;
            _resourceName = resourceName;
            _resourceType = resourceType;
        }

        #region Dictionary properties and methods

        public IEnumerable<string> Keys => _properties.Keys;

        public IEnumerable<object> Values => _properties.Values;

        public int Count => _properties.Count;

        public bool ContainsKey(string key) => _properties.ContainsKey(key);

        public bool TryGetValue(string key, out object value) => _properties.TryGetValue(key, out value);
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _properties.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _properties.GetEnumerator();

        #endregion
    }
}