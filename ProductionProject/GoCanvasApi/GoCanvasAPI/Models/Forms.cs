using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GoCanvasAPI.Models
{

    [XmlRoot(ElementName = "Forms")]
    public class Forms
    {
        [XmlElement(ElementName = "Form")]
        public List<Form> Form { get; set; }
    }

}
