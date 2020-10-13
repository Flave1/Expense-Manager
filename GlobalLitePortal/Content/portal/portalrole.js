'use strict';

var loginPath = "";
var addPath = "";
var editPath = "";
var deletePath = "";

function init(pParams) {
    if (pParams == null) return;
    loginPath = pParams.login;
    addPath = pParams.add;
    editPath = pParams.edit;
    deletePath = pParams.delete;
}

function deleteRole(id) {
    if (id === "" || parseInt(id) < 1) {
        InlineErrorMessage(data.Error ? data.Error : "Invalid Selection!", "dvError");
        return false;
    }
    if (!confirm("Are you sure you want to delete this portal role?")) {
        return false;
    }

    $.ajax({
        type: "POST",
        url: deletePath,
        data: JSON.stringify({ myRoleId: id }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (!data.IsAuthenticated) {
                window.location.href = loginPath;
                return;
            }
            if (!data.IsSuccessful) {
                InlineErrorMessage(data.Error ? data.Error : "Unknown error occured. Please try again later!", "dvError");
                swal({
                    title: "",
                    text: data.Error ? data.Error : "Unknown error occured. Please try again later!",
                    type: "error",
                    showCancelButton: false,
                    confirmButtonText: "Close",
                    closeOnConfirm: true,
                }, function (isConfirm) {
                    if (isConfirm) {

                    }
                });

            } else {
                $('#dvModal').modal('hide');
                swal({
                    title: "",
                    text: "Portal Role was deleted successfully",
                    type: "success",
                    showCancelButton: false,
                    confirmButtonText: "Ok",
                    closeOnConfirm: false,
                }, function (isConfirm) {
                    if (isConfirm) {
                        window.location.reload();
                    }
                });
            }
        },
        complete: function () {

        }
    });
    return false;
}