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

function deleteTab(id) {
    if (id === "" || parseInt(id) < 1) {
        InlineErrorMessage(data.Error ? data.Error : "Invalid Selection!", "dvError");
        return false;
    }
    if (!confirm("Are you sure you want to delete this tab?")) {
        return false;
    }

    $.ajax({
        type: "POST",
        url: deletePath,
        data: { tabId: id }, //JSON.stringify(),
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
                    closeOnConfirm: true
                }, function (isConfirm) {
                        if (isConfirm) {
                            //window.location.reload();
                        }
                });

            } else {
                $('#dvModal').modal('hide');
                swal({
                    title: "",
                    text: "Tab was deleted successfully",
                    type: "success",
                    showCancelButton: false,
                    confirmButtonText: "Ok",
                    closeOnConfirm: false
                }, function (isConfirm) {
                    if (isConfirm) {
                        window.location.reload();
                    }
                });
            }
        },
        complete: function () {
            //$(".loader").hide();
            //$("#loadingMessage").html("");
        }
    });
    return false;
}