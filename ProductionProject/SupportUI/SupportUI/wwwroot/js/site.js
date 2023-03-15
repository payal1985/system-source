// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

ShowInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").show();
        }
    });
}

var imgfileExtension = ["jpg", "jpeg", "bmp", "gif", "png"];


function handleFiles(fInput, event) {
    var files = fInput.files;
    //var id = event.target.id;

    var itemcode = $("#partnumber").val();
    if (itemcode == '') {
        alert("Sorry, Please enter the Part Number");
        fInput.value = "";
        return false;
    }
    else if (!files.length) {
        alert("Must be select file to upload as per allowed format : " + imgfileExtension.join(', '));
        fInput.value = "";
        return false;
    }
    else {
        for (var i = 0; i < files.length; i++) {
            var ext = files[i].name.split('.').pop().toLowerCase();
            if (ext == null || ext == '') {
                alert("Must be select file to upload as per allowed format : " + imgfileExtension.join(', '));
                event.preventDefault();
            }
            else if (ext != '') {
                if ($.inArray(ext, imgfileExtension) == -1) {
                    alert("Only formats are allowed : " + imgfileExtension.join(', '));
                    event.preventDefault();
                }
                //else {
                //    //if (id == 'imagefile') {
                //    //    if (i == 0)
                //    //        files[i].name = itemcode + "." + ext;
                //    //    else
                //    //        files[i].name = itemcode + "_" + i + "." + ext;
                //    //}
                //    //else if (id == '2dimagefile') {
                //    //    if (i == 0)
                //    //        files[i].name = itemcode + "_" + "2D" +  "." + ext;
                //    //    else
                //    //        files[i].name = itemcode + "_" + "2D_" + i + "." + ext;
                //    //}
                //    //else if (id == '3dimagefile') {
                //    //    if (i == 0)
                //    //        files[i].name = itemcode + "_" + "3D" + "." + ext;
                //    //    else
                //    //        files[i].name = itemcode + "_" + "3D_" + i + "." + ext;
                //    //}


                //}
            }
        }
    }
    return true;
}


$(document).ready(function () {
    $(".close").click(function () {
        $("#form-modal").hide();
   })

});

/* Future Refenece code
//$(document).ready(function () {

//    Dropzone.autoDiscover = false;
//    $("#imagefile").dropzone({
//        renameFile: function (file) {
//           var id = $("#partnumber").val();
//            console.log(id); console.log("rename file");
//            console.log("renameFile " + file.name + ".jpg");
//            return id + ".jpg";

//        },
//        complete: function (file) {
//            console.log("DZ 1 One complete");
//        },
//        url: "/home/imageupload",
//        addRemoveLinks: true,
//        success: function (file, response) {
//            var imgName = response;
//            file.previewElement.classList.add("dz-success");
//            console.log("Successfully uploaded :" + imgName);
//        },
//        error: function (file, response) {
//            file.previewElement.classList.add("dz-error");
//        }
//    });
//});

//var _validFileExtensions = [".jpg", ".jpeg", ".bmp", ".gif", ".png"];

//function ValidateSingleInput(oInput) {
//    if ($("#partnumber").val() == '') {
//        alert("Sorry, Please enter the Part Number");
//        oInput.value = "";
//        return false;
//    }
//    else if (oInput.type == "file") {
//        var sFileName = oInput.value;
//        if (sFileName.length > 0) {
//            var blnValid = false;
//            for (var j = 0; j < _validFileExtensions.length; j++) {
//                var sCurExtension = _validFileExtensions[j];
//                if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
//                    blnValid = true;
//                    break;
//                }
//            }

//            if (!blnValid) {
//                alert("Sorry, " + sFileName + " is invalid, allowed extensions are: " + _validFileExtensions.join(", "));
//                oInput.value = "";
//                return false;
//            }
//        }
//    }
//    return true;
//}
*/