// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
categoryList();
subCategoryList();
//$.validate();

//ShowInPopup = (url, title) => {
//    $.ajax({
//        type: "GET",
//        url: url,
//        success: function (res) {
//            $("#form-modal .modal-body").html(res);
//            $("#form-modal .modal-title").html(title);
//            $("#form-modal").show();
//        }
//    });
//}

//$('#productform').validate({
//    rules: {
//        Manufacturer: 'required',
//        Mfg: 'required'
//        //user_email: {
//        //    required: true,
//        //    email: true,
//        //},
//        //psword: {
//        //    required: true,
//        //    minlength: 8,
//        //}
//    },
//    messages: {
//        Manufacturer: 'Enter required value',
//        Mfg: 'Enter required value'
//        //user_email: 'Enter a valid email',
//        //psword: {
//        //    minlength: 'Password must be at least 8 characters long'
//        //}
//    }
//});

var imgfileExtension = ["jpg", "jpeg", "bmp", "gif", "png","dwg"];

/*function method start*/
function handleFiles(fInput, event) {
    var files = fInput.files;
    //var id = event.target.id;

    var itemcode = $("#imagepartnumber").val();
    if (itemcode == '') {
        alert("Sorry, Please enter the Part Number");
        fInput.value = "";
        return false;
    }
    else if (!files.length) {
        alert("Must be select file to upload as per allowed format : " + imgfileExtension.join(', ') + "etc...");
        fInput.value = "";
        return false;
    }
    //else {
    //    for (var i = 0; i < files.length; i++) {
    //        var ext = files[i].name.split('.').pop().toLowerCase();
    //        if (ext == null || ext == '') {
    //            alert("Must be select file to upload as per allowed format : " + imgfileExtension.join(', '));
    //            event.preventDefault();
    //        }
    //        else if (ext != '') {
    //            if ($.inArray(ext, imgfileExtension) == -1) {
    //                alert("Only formats are allowed : " + imgfileExtension.join(', '));
    //                event.preventDefault();
    //            }
    //         }
    //    }
    //}
    return true;
}


function addProduct() {
   // $.validate;
    if (!$("#productform").isValid()) {
   //if (!$("#productform").valid()) {       
       event.preventDefault();
       return false;
    }

        var ProductModel = {
            Manufacturer: $("#Manufacturer").val(),
            Rep: $("#Rep").val(),
            Mfg: $("#Mfg").val(),
            Cat: $("#Cat").val(),
            Manufactured: $("#Manufactured").val(),
            PartNumber: $("#PartNumber").val(),
            Components: $("#Components").val(),
            PartDescription: $("#PartDescription").val(),
            Series: $("#Series").val(),
            Category: $("#Category").val(),
            SubCategory: $("#SubCategory").val(),
            HeightDD: $("#HeightDD").val(),
            Misc: $("#Misc").val(),
            SeatActions: $("#SeatActions").val(),
            FabricDetail: $("#FabricDetail").val(),
            Modular: $("#Modular").val(),
            Shape: $("#Shape").val(),
            Seats: $("#Seats").val(),
            BaseAttribute: $("#BaseAttribute").val(),
            BaseAttributeOther: $("#BaseAttributeOther").val(),
            COM: $("#COM").val(),
            GRADE3_C_List: $("#GRADE3_C_List").val(),
            LeadTime: $("#LeadTime").val(),
            Height: $("#Height").val(),
            Depth: $("#Depth").val(),
            Width: $("#Width").val(),
            SeatHeight: $("#SeatHeight").val(),
            ArmHeight: $("#ArmHeight").val(),
            DIA: $("#DIA").val(),
            Designer: $("#Designer").val(),
            Yardage: $("#Yardage").val(),
            LeatherSqFeet: $("#LeatherSqFeet").val(),
            leatherSqMeters: $("#leatherSqMeters").val(),
            cf: $("#cf").val(),
            Environmental_1: $("#Environmental_1").val(),
            Environmental_2: $("#Environmental_2").val(),
            Environmental_3: $("#Environmental_3").val(),
            Environmental_4: $("#Environmental_4").val()
        }

        $.ajax({
            url: "/api/product",
            type: 'POST',
            contentType: "application/json;charset=utf-8",
            async: false,
            data: JSON.stringify(ProductModel),
            success: function (result) {
                alert(result);
                formClear();
                //location.href ='http://localhost:2345/index.html'
                //productAddSuccess(product);
                //return false;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus + ': ' + errorThrown + ':' + XMLHttpRequest.responseText);
                //return false;
            }
        });

}

function formClear() {
    $("#Manufacturer").val("");
    $("#Rep").val("");
    $("#Mfg").val("");
    $("#Cat").val("");
    $("#Manufactured").val("");
    $("#PartNumber").val("");
    $("#Components").val("");
    $("#PartDescription").val("");
    $("#Series").val("");
    $("#Category").val("");
    $("#SubCategory").val("");
    $("#HeightDD").val("");
    $("#Misc").val("");
    $("#SeatActions").val("");
    $("#FabricDetail").val("");
    $("#Modular").val("");
    $("#Shape").val("");
    $("#Seats").val("");
    $("#BaseAttribute").val("");
    $("#BaseAttributeOther").val("");
    $("#COM").val("");
    $("#GRADE3_C_List").val("");
    $("#LeadTime").val("");
    $("#Height").val("");
    $("#Depth").val("");
    $("#Width").val("");
    $("#SeatHeight").val("");
    $("#ArmHeight").val("");
    $("#DIA").val("");
    $("#Designer").val("");
    $("#Yardage").val("");
    $("#LeatherSqFeet").val("");
    $("#leatherSqMeters").val("");
    $("#cf").val("");
    $("#Environmental_1").val("");
    $("#Environmental_2").val("");
    $("#Environmental_3").val("");
    $("#Environmental_4").val("");
}

function categoryList() {
    $.ajax({
        url: "/api/product/getcategory",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#Category").append("<option value=''>Select Category</option>");

            $.each(result, function (i, result) {      // bind the dropdown list using json result              
                $('<option>',
                    {
                        value: result.category_ID,
                        text: result.description
                    }).html(result.description).appendTo("#Category");
            });
            $('#Category').trigger("chosen:updated");

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + ': ' + errorThrown + ':' + XMLHttpRequest.responseText);
        }
    });
}

function subCategoryList() {
    $.ajax({
        url: "/api/product/getsubcategory",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#SubCategory").append("<option value=''>Select SubCategory</option>");

            $.each(result, function (i, result) {      // bind the dropdown list using json result              
                $('<option>',
                    {
                        value: result.subcategory_ID,
                        text: result.description
                    }).html(result.description).appendTo("#SubCategory");
            });
            $('#SubCategory').trigger("chosen:updated");

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + ': ' + errorThrown + ':' + XMLHttpRequest.responseText);
        }
    });
}
/*function method end */


$(document).ready(function () {
    //categoryList()
    //subCategoryList()
    //var formvalid = $("#productform").isValid();
    //console.log(formvalid);

    $("input").focusout(function () {
       // $(this).removeClass(".data-validation-error-msg");
       // $(this).siblings("data-validation-error-msg").removeClass();
        $(this).siblings("span.form-error").hide();
        $(this).css("border", "#DCDCDC solid 1px");
        //$(this).s border - color: rgb(185, 74, 72);
        //$(this).siblings("span.help-block").hide();
    });
    $("textarea").focusout(function () {
        $(this).siblings("span.form-error").hide();
        $(this).css("border", "#DCDCDC solid 1px");
    });
    $("select").focusout(function () {
        $(this).siblings("span.form-error").hide();
        $(this).css("border", "#DCDCDC solid 1px");
    });

    $("#BaseAttribute").on("change", function () {
        //alert("changed!");
        if ($("#BaseAttribute").val() == 'Other') {
            //$("#BaseAttributeOther").attr('type', 'text');
            //$("#BaseAttributeOther").show();

            $("#BaseAttributeOther").show();
        }
        else {
            $("#BaseAttributeOther").hide();

        }

    });

    //$(".close").click(function () {
    //    $("#form-modal").hide();
    //})

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