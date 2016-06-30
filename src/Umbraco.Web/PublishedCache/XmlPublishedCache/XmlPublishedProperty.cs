using System;
using System.Xml;
using System.Xml.Serialization;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;
using Umbraco.Core.Xml;

namespace Umbraco.Web.PublishedCache.XmlPublishedCache
{

	/// <summary>
	/// Represents an IDocumentProperty which is created based on an Xml structure.
	/// </summary>
	[Serializable]
	[XmlType(Namespace = "http://umbraco.org/webservices/")]
	internal class XmlPublishedProperty : PublishedPropertyBase
	{
		private readonly string _sourceValue; // the raw, xml node value

        // the xml cache does not need to implement XPath value, really
        // as for the rest... we're single threaded here, keep it simple
        private object _objectValue;
        private bool _objectValueComputed;
        private readonly bool _isPreviewing;

        /// <summary>
        /// Gets the raw value of the property.
        /// </summary>
        public override object SourceValue => _sourceValue;

	    // in the Xml cache, everything is a string, and to have a value
        // you want to have a non-null, non-empty string.
	    public override bool HasValue => _sourceValue.Trim().Length > 0;

	    public override object Value
	    {
	        get
	        {
                // NOT caching the source (intermediate) value since we'll never need it
                // everything in Xml cache in v7 is per-request anyways
                // also, properties should not be shared between requests and therefore
                // are single threaded, so the following code should be safe & fast

                if (_objectValueComputed) return _objectValue;
                var inter = PropertyType.ConvertSourceToInter(_sourceValue, _isPreviewing);
                // initial reference cache level always is .Content
                _objectValue = PropertyType.ConvertInterToObject(PropertyCacheLevel.Content, inter, _isPreviewing);
                _objectValueComputed = true;
                return _objectValue;
            }
        }

        public override object XPathValue { get { throw new NotImplementedException(); } }

        public XmlPublishedProperty(PublishedPropertyType propertyType, bool isPreviewing, XmlNode propertyXmlData)
            : this(propertyType, isPreviewing)
		{
		    if (propertyXmlData == null)
		        throw new ArgumentNullException(nameof(propertyXmlData), "Property xml source is null");
		    _sourceValue = XmlHelper.GetNodeValue(propertyXmlData);
        }

        public XmlPublishedProperty(PublishedPropertyType propertyType, bool isPreviewing, string propertyData)
            : this(propertyType, isPreviewing)
        {
            if (propertyData == null)
                throw new ArgumentNullException(nameof(propertyData));
            _sourceValue = propertyData;
        }

        public XmlPublishedProperty(PublishedPropertyType propertyType, bool isPreviewing)
            : base(propertyType, PropertyCacheLevel.Unknown) // cache level is ignored
        {
            _sourceValue = string.Empty;
            _isPreviewing = isPreviewing;
        }
	}
}