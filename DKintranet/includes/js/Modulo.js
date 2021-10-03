function RecuperarModulos() {
    PageMethods.GetAll(OnCallBackGetAll, OnFail);
}

function OnCallBackGetAll(args) {
    if (args) {

        args = eval('(' + args + ')');
        if (args.length > 0) {
            var strHtml = '';
            strHtml += '<div class="table-responsive">';
            strHtml += '<table class="table table-striped">';
            strHtml += '<thead>';
            strHtml += '<tr>';
            strHtml += '<th>Laboratorio</th>';
            strHtml += '<th>Descripción</th>';
            strHtml += '<th>Cantidad Mínimos</th>';
            strHtml += '<th></th>';
            strHtml += '</tr>';
            strHtml += '</thead>';
            strHtml += '<tbody>';
            for (var i = 0; i < args.length; i++) {
                strHtml += '<tr>';
                strHtml += '<td>' + args[i].nombre_laboratorio + '</td>'
                strHtml += '<td>' + args[i].descripcion + '</td>';;
                strHtml += '<td>' + args[i].cantidadMinimos + '</td>';
                strHtml += '<td>';
                strHtml += '<button type="button" class="btn btn-warning" onclick="return Editar(\'' + args[i].id + '\');">Modificar</button>' + '&nbsp;' + '&nbsp;';
                strHtml += '<button type="button" class="btn btn-danger" onclick="return Elimimar(\'' + args[i].id + '\');">Eliminar</button>' + '&nbsp;' + '&nbsp;';
                strHtml += '</td>';
                strHtml += '</tr>';
            }
            strHtml += '</tbody>';
            strHtml += '</table>';
            strHtml += '</div>';
            $('#divContenedorGrilla').html(strHtml);
        }
        else {
            $('#divContenedorGrilla').html('');
        }
    }
}
function Editar(pValor) {
    location.href = 'ModuloEditar.aspx?id=' + pValor;
    return false;
}

function Elimimar(pValor) {
    $.confirm({
        title: 'Mensaje',
        content: '¿Desea eliminar?',
        buttons: {
            Aceptar: function () {
                PageMethods.Delete(pValor, OnCallBackDelete, OnFail);
            },
            Cancelar: function () {

            }

        }
    });

    return false;
}
function OnCallBackDelete(args) {
    RecuperarModulos();
}
function Volver() {
    location.href = 'Modulo.aspx';
    return false;
}
function AgregarImagen(pValor) {
    //location.href = 'GestionOfertaEditarAgregar.aspx?id=' + pValor;
    location.href = 'AgregarArchivo.aspx?id=' + pValor + '&t=app&an=1024&al=768';
    return false;
}
function Guardar() {
    var var_nombre = $('#txt_nombre').val();


    PageMethods.AddUpdate(var_nombre, Volver, OnFail);

}