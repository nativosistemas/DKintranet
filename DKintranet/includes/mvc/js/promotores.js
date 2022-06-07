$(document).ready(function () {
    $(".cmbCliente").select2();

    $("#cmbCliente").change(function () {
        var IdCliente = $(this).val().trim();

        $.ajax({
            type: "POST",
            url: "/ctacte/CambiarClientePromotor",
            data: { IdCliente },
            success:
                function (response) {
                    console.log(response);
                    window.location.href = "/ctacte/composicionsaldo";
                },
            failure: function (response) {
                //hideCargandoBuscador();
                OnFail(response);
            },
            error: function (response) {
                //hideCargandoBuscador();
                OnFail(response);
            }
        });
    });

    //
    $(".cmbCliente_intranet").select2();

    $("#cmbCliente_intranet").change(function () {
        var IdCliente = $(this).val().trim();
        CambiarCliente(IdCliente);
    })
});

var width_old = $(window).width();
$(window).resize(function () {
    if ($(this).width() != width_old) {
        $(".cmbCliente").select2();
        $(".cmbCliente_intranet").select2();
        width_old = $(this).width();
    }
})

