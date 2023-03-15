using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GoCanvasAPI.Models
{
    [XmlRoot(ElementName = "Submissions")]
    public class Submissions    
    {        
        [XmlElement(ElementName = "Submission")]
        public List<Submission> Submission { get; set; }
    }

    [XmlRoot(ElementName = "Submission")]
    public class Submission
    {
        [XmlAttribute(AttributeName = "Id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "Form")]
        public List<Form> Form { get; set; }

        [XmlElement(ElementName = "Date")]
        public Date Date { get; set; }

        [XmlElement(ElementName = "DeviceDate")]
        public DeviceDate DeviceDate     { get; set; }

        [XmlElement(ElementName = "UserName")]
        public UserName UserName       { get; set; }

        [XmlElement(ElementName = "FirstName")]
        public FirstName FirstName      { get; set; }

        [XmlElement(ElementName = "LastName")]
        public LastName LastName       { get; set; }

        [XmlElement(ElementName = "ResponseID")]
        public ResponseID ResponseID { get; set; }

        [XmlElement(ElementName = "WebAccessToken")]
        public WebAccessToken WebAccessToken { get; set; }

        [XmlElement(ElementName = "No.")]
        public No No  { get; set; }

        [XmlElement(ElementName = "SubmissionNumber")]
        public SubmissionNumber SubmissionNumber { get; set; }

        [XmlElement(ElementName = "Sections")]
        public Sections Sections { get; set; }
    }

    [XmlRoot(ElementName = "Sections")]
    public class Sections
    {
        [XmlElement(ElementName = "Section")]
        public List<Section> Section { get; set; }
    }

    [XmlRoot(ElementName = "Section")]
    public class Section
    {
        [XmlElement(ElementName = "Name")]
        public Name Name { get; set; }

        [XmlElement(ElementName = "Screens")]
        public Screens Screens { get; set; }
    }

    [XmlRoot(ElementName = "Screens")]
    public class Screens
    {
        [XmlElement(ElementName = "Screen")]
        public List<Screen> Screen { get; set; }   
    }

    [XmlRoot(ElementName = "Screen")]
    public class Screen
    {     
        [XmlElement(ElementName = "Name")]
        public Name Name { get; set; }

        [XmlElement(ElementName = "Responses")]
        public Responses Responses { get; set; }

        [XmlElement(ElementName = "ResponseGroups")]
        public ResponseGroups ResponseGroups { get; set; }
    }

    [XmlRoot(ElementName = "Responses")]
    public class Responses
    {
        [XmlElement(ElementName = "Response")]
        public List<Response> Response { get;set;}
    }


    [XmlRoot(ElementName = "ResponseGroups")]
    public class ResponseGroups 
    {
        [XmlElement(ElementName = "ResponseGroup")]
        public List<ResponseGroup> ResponseGroup { get; set; }
    }

    [XmlRoot(ElementName = "ResponseGroup")]
    public class ResponseGroup
    {
        [XmlAttribute(AttributeName = "Guid")]
        public string Guid { get; set; }


        [XmlElement(ElementName = "Response")]
        public Response Response { get; set; }


        [XmlElement(ElementName = "Section")]
        public Section Section { get; set; }
    }

    [XmlRoot(ElementName = "Response")]
    public class Response
    {
        [XmlAttribute(AttributeName = "Guid")]
        public string Guid { get; set; }

        [XmlElement(ElementName = "Label")]
        public Label Label { get; set; }

        [XmlElement(ElementName = "Value")]
        public Value Value { get; set; }

        [XmlElement(ElementName = "Numbers")]
        public Numbers Numbers { get; set; }

        [XmlElement(ElementName = "Type")]
        public Type Type { get; set; }
    }

    [XmlRoot(ElementName = "Numbers")]
    public class Numbers
    {
        [XmlElement(ElementName = "Number")]
        public List<Number> Number { get; set; }
    }

    [XmlRoot(ElementName = "Number")]
    public class Number
    {
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Label")]
    public class Label
    {
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Value")]
    public class Value
    {
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Type")]
    public class Type
    {
        [XmlText]
        public string Text { get; set; }
    }


    [XmlRoot(ElementName = "Date")]
    public class Date {[XmlText] public string Text {get; set;} }

    [XmlRoot(ElementName = "DeviceDate")]
    public class DeviceDate {[XmlText] public string Text {get; set;} }

    [XmlRoot(ElementName = "UserName")]
    public class UserName {[XmlText] public string Text {get; set;} }

    [XmlRoot(ElementName = "FirstName")]
    public class FirstName {[XmlText] public string Text {get; set;} }

    [XmlRoot(ElementName = "LastName")]
    public class LastName {[XmlText] public string Text {get; set;} }

    [XmlRoot(ElementName = "ResponseID")]
    public class ResponseID {[XmlText] public string Text {get; set;} }

    [XmlRoot(ElementName = "WebAccessToken")]
    public class WebAccessToken {[XmlText] public string Text {get; set;} }

    [XmlRoot(ElementName = "No.")]
    public class No {[XmlText] public string Text {get; set;} }

    [XmlRoot(ElementName = "SubmissionNumber")]
    public class SubmissionNumber {[XmlText] public string Text {get; set;} }




    //[XmlRoot(ElementName = "Form")]
    //class Form
    //{
    //    [XmlAttribute(AttributeName = "Id")]
    //    public string Id { get; set; }

    //    [XmlAttribute(AttributeName = "Name")]
    //    public Name Name { get; set; }

    //    [XmlAttribute(AttributeName = "Status")]
    //    public Status Status { get; set; }

    //    [XmlAttribute(AttributeName = "Version")]
    //    public Version Version { get; set; }

    //}

    //[XmlRoot(ElementName = "Name")]
    //public class Name
    //{
    //    [XmlText]
    //    public string Text { get; set; }
    //}

    //[XmlRoot(ElementName = "Status")]
    //public class Status
    //{
    //    [XmlText]
    //    public string Text { get; set; }
    //}

    //[XmlRoot(ElementName = "Version")]
    //public class Version
    //{
    //    [XmlText]
    //    public string Text { get; set; }
    //}

}
