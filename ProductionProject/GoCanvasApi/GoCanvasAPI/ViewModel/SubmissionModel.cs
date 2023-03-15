using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI.ViewModel
{
    public class SubmissionRootObject
    {
        [JsonProperty("Submissions")]
        public Submissions Submissions { get; set; }
    }

    public class Submissions
    {
        [JsonProperty("Submission")]
        public List<Submission> Submission { get; set; }
    }

    public class Submission
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Form")]
        public List<Form> Form { get; set; }

        [JsonProperty("Date")]
        public Date Date { get; set; }

        [JsonProperty("DeviceDate")]
        public DeviceDate DeviceDate { get; set; }

        [JsonProperty("UserName")]
        public UserName UserName { get; set; }

        [JsonProperty("FirstName")]
        public FirstName FirstName { get; set; }

        [JsonProperty("LastName")]
        public LastName LastName { get; set; }

        [JsonProperty("ResponseID")]
        public ResponseID ResponseID { get; set; }

        [JsonProperty("WebAccessToken")]
        public WebAccessToken WebAccessToken { get; set; }

        [JsonProperty("no")]
        public No No { get; set; }

        [JsonProperty("SubmissionNumber")]
        public SubmissionNumber SubmissionNumber { get; set; }

        [JsonProperty("Sections")]
        public Sections Sections { get; set; }
    }

    public class Sections
    {
        [JsonProperty("Section")]
        public List<Section> Section { get; set; }
    }

    public class Section
    {
        [JsonProperty("Name")]
        public Name Name { get; set; }

        [JsonProperty("Screens")]
        public Screens Screens { get; set; }
    }

    public class Screens
    {
        [JsonProperty("Screen")]
        public List<Screen> Screen { get; set; }
    }

    public class Screen
    {
        [JsonProperty("Name")]
        public Name Name { get; set; }

        [JsonProperty("Responses")]
        public Responses Responses { get; set; }

        [JsonProperty("ResponseGroups")]
        public ResponseGroups ResponseGroups { get; set; }
    }

    public class Responses
    {
        [JsonProperty("Response")]
        public List<Response> Response { get; set; }
    }


    public class ResponseGroups
    {
        [JsonProperty("ResponseGroup")]
        public List<ResponseGroup> ResponseGroup { get; set; }
    }

    public class ResponseGroup
    {
        [JsonProperty("Guid")]
        public string Guid { get; set; }


        [JsonProperty("Response")]
        public Response Response { get; set; }


        [JsonProperty("Section")]
        public Section Section { get; set; }
    }

    public class Response
    {
        [JsonProperty("Guid")]
        public string Guid { get; set; }

        [JsonProperty("Label")]
        public Label Label { get; set; }

        [JsonProperty("Value")]
        public Value Value { get; set; }

        [JsonProperty("Numbers")]
        public Numbers Numbers { get; set; }

        [JsonProperty("Type")]
        public Type Type { get; set; }
    }

    public class Numbers
    {
        [JsonProperty("Number")]
        public List<Number> Number { get; set; }
    }

    public class Number
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Label
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Value
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Type
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }


    public class Date {[JsonProperty("text")] public string Text { get; set; } }

    public class DeviceDate {[JsonProperty("text")] public string Text { get; set; } }

    public class UserName {[JsonProperty("text")] public string Text { get; set; } }

    public class FirstName {[JsonProperty("text")] public string Text { get; set; } }

    public class LastName {[JsonProperty("text")] public string Text { get; set; } }

    public class ResponseID {[JsonProperty("text")] public string Text { get; set; } }

    public class WebAccessToken {[JsonProperty("text")] public string Text { get; set; } }

    public class No {[JsonProperty("text")] public string Text { get; set; } }

    public class SubmissionNumber {[JsonProperty("text")] public string Text { get; set; } }



}
