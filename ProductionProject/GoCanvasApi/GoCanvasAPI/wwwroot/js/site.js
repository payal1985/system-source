// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    loadFormData();
});

function loadFormData() {
  //  $("#divProgress").show();
   // $("#divProgress").css("display", "block");
    $.ajax({
        url: 'api/GoCanvas',
        //data: { RoutineId: routineid, SiteId: siteid },
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {

            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.id + '</td>';
                html += '<td id="formname">' + item.name.text + '</td>';
                html += '<td>' + item.status.text + '</td>';
                html += '<td>' + item.version.text + '</td>';
                html += '<td><a href="#" class="btn btn-primary" onclick="loadSubmissionData(this)">Submission Data</a></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
          //  $("#divProgress").hide();

            //console.log(html);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + ': ' + errorThrown + ':' + XMLHttpRequest.responseText);
        }
    });
}

function loadSubmissionData(e) {
   // $("#divProgress").show();
    //$('.table').prop('disabled', true);
    $('a').attr('disabled');

    var tr = $(e).closest("tr");
    var formname = tr.find("#formname").html();

    return new Promise(function (resolve, reject) {
        $.ajax({
            url: 'api/Submissions',
            data: { formName: formname },
            success: function (result) {
                resolve(result); // Resolve promise and go to then()
                if (result) {
                    alert("Record Inserted Successfully !!!!!! ");
                }
                else {
                    alert("Records not found for insertion...........");
                }
               // $("#divProgress").hide();
                $('.table').prop('disabled', true);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                reject(XMLHttpRequest.responseText); // Reject the promise and go to catch()
                alert(textStatus + ': ' + errorThrown + ':' + XMLHttpRequest.responseText);
               //$("#divProgress").hide();
                $('.table').prop('disabled', true);
            }
        });
    });

    //$.ajax({
    //    url: '/api/Submissions',
    //    data: { formName: formname},
    //    type: 'GET',
    //    contentType: 'application/json; charset=utf-8',
    //    async: false,
    //    success: function (result) {
    //        //if (result != null ) {                
    //            if (result)
    //                alert("Record Inserted Successfully !!!!!! ");
    //            else
    //                alert("Records not found for insertion...........");
    //        //}
    //     //   $("#divProgress").hide();

    //    },
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        alert(textStatus + ': ' + errorThrown + ':' + XMLHttpRequest.responseText);
    //    }
    //});
}



function sendClick() {
    // alert("test");
    //if (!$("#referencedataform").isValid()) {
        if (!$("#referencedataform").valid()) {
        event.preventDefault();
        return false;
    }

    var files = $("#fileupload").get(0).files;

    var refdatasetname = $("#referencedatasetname").val();
    var fileData = new FormData();
    fileData.append("refdataset", refdatasetname);
    fileData.append("file", files[0]);

    //for (var i = 0; i < files.length; i++) {
    //    fileData.append("imagefile", files[i]);
    //}

    $.ajax({
        type: "POST",
        url: 'api/ReferenceData',    // CALL WEB API TO SAVE THE FILES.
        contentType: false,
        async: false,
        processData: false,         // PREVENT AUTOMATIC DATA PROCESSING.
        data: fileData, 		        // DATA OR FILES IN THIS CONTEXT.
        success: function (data) {
            alert(data);
            $("#referencedatasetname").val("");
            $("#fileupload").val("");
        //    return true;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(textStatus + ': ' + errorThrown + ':' + XMLHttpRequest.responseText);
            //return true;
        }
    });

}