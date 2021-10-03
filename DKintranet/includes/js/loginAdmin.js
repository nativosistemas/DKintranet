var isIngresarPageMethods = false;
var name = null;
var pass = null;
jQuery(document).ready(function () {
    var myName = localStorage['nameAdmin'] || '';
    var myPass = localStorage['passAdmin'] || '';
    $('#txtPassword').val(myPass);
    $('#txtUsuario').val(myName);
});

function onclickIngresar() {
    if (!isIngresarPageMethods) {
        $('#lblMensaje').html('');
        isIngresarPageMethods = true;
        name = $('#txtUsuario').val();
        pass = $('#txtPassword').val();

        //PageMethods.login(name, pass, OnCallBackLogin, OnFailLogin);
        PageMethods.LoginAdmin(name, pass, OnCallLoginAdmin, OnFailLoginAdmin);
        //ajaxLogin(name, pass);
    }
    return false;
}
function OnCallLoginAdmin(args) {

    if (args == 'OK') {
        localStorage['nameAdmin'] = name;
        localStorage['passAdmin'] = pass;
        location.href = '../admin/pages/MenuPrincipal.aspx';
    } else {
        isIngresarPageMethods = false;
        $('#lblMensaje').html(args);
    }
}
function OnFailLoginAdmin(ex) {
    isIngresarPageMethods = false;
    OnFail(ex);
}