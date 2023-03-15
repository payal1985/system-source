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





Dropzone.autoDiscover = false;
Dropzone.options.myAwesomeDropzone = {
    //url: url,
    //paramName: "image",
    //dictDefaultMessage: 'Selecciona tus archivos..',
    //dictRemoveFile: "Eliminar",
    //dictCancelUpload: "Cancelar carga",
    //addRemoveLinks: true,
    uploadMultiple: false,
    renameFile: function (file) {
        let newName = new Date().getTime() + '_' + file.name;
        return newName;
    }
   // new Dropzone("input#imagefile");
}

//function pt() {
//    return `
//    <div class="dz-preview dz-file-preview">
//      <div class="dz-details">
//        <img data-dz-thumbnail />
//      </div>
//    </div>`;
//}

//$('.ImageFile2D').dropzone({
//    thumbnailWidth: 50,
//    thumbnailHeight: 50,
//    previewTemplate: pt(),
//    renameFile: function (file) {
//        // getItemCode(event);
//        // setTimeout(function () { "", 2000 });
//        console.log("rename file");
//        console.log("renameFile " + file.name + ".jpg");
//        return currItemCode + ".jpg";

//    },
//    complete: function (file) {
//        console.log("DZ 1 One complete");
//    },
//    //error: function(){
//    //console.log("test");
//    //},
//    url: "/home/fileupload"
//});