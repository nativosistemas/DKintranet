function RecuperarLaboratorios() {
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
            strHtml += '<th>Nombre</th>';
            strHtml += '<th></th>';
            strHtml += '</tr>';
            strHtml += '</thead>';
            strHtml += '<tbody>';
            for (var i = 0; i < args.length; i++) {
                strHtml += '<tr>';
                strHtml += '<td>' + args[i].nombre + '</td>';
                strHtml += '<td>';
               // strHtml += '<button type="button" class="btn btn-warning" onclick="return Editar(\'' + args[i].id + '\');">Modificar</button>' + '&nbsp;' + '&nbsp;';
               // strHtml += '<button type="button" class="btn btn-danger" onclick="return Elimimar(\'' + args[i].id + '\');">Eliminar</button>' + '&nbsp;' + '&nbsp;';
                strHtml += '<button type="button" class="btn btn-info" onclick="return AgregarImagen(\'' + args[i].idParaArchivo + '\');">Imagen</button>' + '&nbsp;' + '&nbsp;';
                //strHtml += '<button type="button" class="btn btn-info" onclick="return AgregarOfertaFolleto(\'' + args[i].ofe_idOferta + '\');">Folleto</button>' + '&nbsp;' + '&nbsp;';
                //strHtml += '<button type="button" class="btn btn-warning" onclick="return IrVistaPreviaId(\'' + args[i].ofe_idOferta + '\');">Vista Previa</button>';  //
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
    location.href = 'LaboratorioEditar.aspx?id=' + pValor;
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
    RecuperarLaboratorios();
}
function Volver() {
    location.href = 'Laboratorio.aspx';
    return false;
}
function AgregarImagen(pValor) {
    //location.href = 'GestionOfertaEditarAgregar.aspx?id=' + pValor;
    location.href = 'AgregarArchivo.aspx?id=' + pValor + '&t=laboratorio&an=1024&al=768';
    return false;
}
function Guardar() {
    var var_nombre = $('#txt_nombre').val();


    PageMethods.AddUpdate(var_nombre, Volver, OnFail);

}