function onkeypressIngresar(e) {
    if (!e) e = window.event;
    var keyCode = e.keyCode || e.which;
    if (keyCode == '13') {
        // Enter pressed
        onclickIngresar();
        return false;
    }
}
function onclickIngresar() {
    //if (!isIngresarPageMethods) {
       // $('#lblMensaje').html('');
      //  isIngresarPageMethods = true;
        name = $('#txtUsuario').val();
        pass = $('#txtPassword').val();

        //PageMethods.login(name, pass, OnCallBackLogin, OnFailLogin);
        //PageMethods.LoginAdmin(name, pass, OnCallLoginAdmin, OnFailLoginAdmin);
        ajaxLogin(name, pass);
    //}
    return false;
}
function ajaxLogin(name, pass) {
    $('#infoLogin').html('');
    if (isNotNullEmpty(name) && isNotNullEmpty(pass)) {
        $.ajax({
            type: "POST",
            url: "../config/login",
            data: { pName: name, pPass: pass },
            success:
                function (response) {
                    OnCallBackLogin(response);
                },
            failure: function (response) {
                OnFail(response);
            },
            error: function (response) {
                OnFail(response);
            }
        });
    }
}

function OnCallBackLogin(args) {
    isIngresarPageMethods = false;
    console.log(args);
    if (args == 'Ok') {
        localStorage['name'] = name;
        localStorage['pass'] = pass;
        funIrIntranet();
    }
    else if (args == 'OkPromotor') {
        localStorage['name'] = name;
        localStorage['pass'] = pass;
        funIrIntranetPromotor();
    } else if (args == 'OkTomarPedido') {
        localStorage['name'] = name;
        localStorage['pass'] = pass;
        funIrIntranet();
    } else {
        $('#infoLogin').html(args);
        
        /*$.alert({
            title: 'Información',
            content: args,
        });*/
    }
}
function funIrIntranet() {
    location.href = '../mvc/Buscador';
}
function funIrIntranetPromotor() {
    location.href = '../ctacte/composicionsaldo';
}
