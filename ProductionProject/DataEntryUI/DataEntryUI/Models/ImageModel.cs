using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntryUI.Models
{
    public class ImageModel
    {
        [Required]
        [DisplayName("Part Number")]
        public string PartNumber { get; set; }

        [DisplayName("Image")]
        public string ImageFileRegular { get; set; }

        [DisplayName("2D")]
        public string ImageFile2D { get; set; }

        [DisplayName("3D")]
        public string ImageFile3D { get; set; }

        [DisplayName("Revit")]
        public string ImageFileRevit { get; set; }

        [DisplayName("Sketchup")]
        public string ImageFileSketchup { get; set; }

        [DisplayName("3dStudioMax")]
        public string ImageFile3dStudioMax { get; set; }
    }
}
