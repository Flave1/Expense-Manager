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