/* ---------------------------------------------------
    Error message (top) - invalid user
----------------------------------------------------- */
window.onload = function (e) {
    var error_message = document.getElementById("lblMsg").innerHTML;

    if (error_message == null || error_message == undefined || error_message == "") {
        document.getElementById("message-alert").classList.add('d-none');
        }
    else {
        document.getElementById("message-alert").classList.remove('d-none');
        }
}