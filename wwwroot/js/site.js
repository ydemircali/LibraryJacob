// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(".addMyLibrary").click(function() {

    $(this).text("Adding...");
    var id = this.id;
   
    $.ajax({
        url: "/addMyLibrary?bookId="+ id,
        type: "POST",
        contentType: "application/json",
        success: function (data) {

            alert(data.message); 
            $(".addMyLibrary").text("Add My Library");
        }
    });

});


$(document).ready(function() {
    $('#userBooksTable').DataTable();
});
