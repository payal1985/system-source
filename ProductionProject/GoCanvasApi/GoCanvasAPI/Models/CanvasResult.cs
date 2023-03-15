using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GoCanvasAPI.Models
{
    [XmlRoot(ElementName = "CanvasResult")]
    public class CanvasResult
    {
        [XmlElement(ElementName = "Forms")]
        public Forms Forms { get; set; }

        [XmlElement(ElementName = "Submissions")]
        public Submissions Submissions { get; set; }

        [XmlElement(ElementName = "TotalPages")]
        public TotalPages TotalPages { get; set; }

        [XmlElement(ElementName = "CurrentPage")]
        public CurrentPage CurrentPage { get; set; }
    }

    [XmlRoot(ElementName = "Form")]
    public class Form
    {
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "OriginatingLibraryTemplateId")]
        public string OriginatingLibraryTemplateId { get; set; }

        [XmlAttribute(AttributeName = "GUID")]
        public string GUID { get; set; }

        [XmlElement(ElementName = "Name")]
        public Name Name { get; set; }

        [XmlElement(ElementName = "Status")]
        public Status Status { get; set; }


        [XmlElement(ElementName = "Version")]
        public Version Version { get; set; }

    }

    [XmlRoot(ElementName = "Name")]
    public class Name
    {
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Status")]
    public class Status
    {
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Version")]
    public class Version
    {
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "TotalPages")]
    public class TotalPages
    {
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "CurrentPage")]
    public class CurrentPage
    {
        [XmlText]
        public string Text { get; set; }

    }
}
