<%@ Page Title="" Language="C#" MasterPageFile="~/master/BaseAdminJs3.Master" AutoEventWireup="true" CodeBehind="GestionOfertaEditar.aspx.cs" Inherits="DKintranet.admin.pages.GestionOfertaEditar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../includes/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="lib/css/prettify.css" />
    <link rel="stylesheet" type="text/css" href="lib/css/bootstrap-wysihtml5.css" />
    <script src="../../includes/js/bootstrap.min.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
    <script type="text/javascript" src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="lib/js/wysihtml5-0.3.0.js" type="text/javascript"></script>
    <script src="lib/js/prettify.js" type="text/javascript"></script>
    <script src="lib/js/bootstrap-wysihtml5.js" type="text/javascript"></script>
    <script src="../../includes/js/GestionOferta.js?n=4" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            RecuperarTodasOfertaDetalles();
            var titulo = $('#hiddenOfe_titulo').val();
            if (typeof titulo == 'undefined') {
                titulo = null;
            } else {
                $('#txt_titulo').val(titulo);
            }
            var descr = $('#hiddenOfe_descr').val();
            if (typeof descr == 'undefined') {
                descr = null;
            } else {
                $('#txt_descr').val(descr);
            }
            var descuento = $('#hiddenOfe_descuento').val();
            if (typeof descuento == 'undefined') {
                descuento = null;
            } else {
                $('#txt_descuento').val(descuento);
            }

            var etiqueta = $('#hiddenOfe_etiqueta').val();
            if (typeof etiqueta == 'undefined') {
                etiqueta = null;
            } else {
                $('#txt_etiqueta').val(etiqueta);
            }

            var etiquetaColor = $('#hiddenOfe_etiquetaColor').val();
            if (typeof etiquetaColor == 'undefined') {
                etiquetaColor = null;
            } else {
                $('#cmdEtiquetaColor').val(etiquetaColor);
            }
            var idOferta = $('#hiddenOfe_idOferta').val();
            if (typeof idOferta == 'undefined') {
                idOferta = null;
            } else {
                if (parseInt(idOferta) == 0) {
                    $('#contenedorProductoTransfer').css('display', 'none');
                }
            }
            var nombreTransfer = $('#hiddenOfe_nombreTransfer').val();
            if (typeof nombreTransfer == 'undefined') {
                nombreTransfer = null;
            } else {

                $('#cmdTransfer').val(nombreTransfer);
            }
            //
            $('#txt_nuevosLanzamiento').attr('checked', false);
            var nuevosLanzamiento = $('#hiddenOfe_nuevosLanzamiento').val();
            if (typeof nuevosLanzamiento == 'undefined') {
                nuevosLanzamiento = null;
            } else {
                if (nuevosLanzamiento === '1') {
                    $('#txt_nuevosLanzamiento').attr('checked', true);
                }
            }
            var isNuevosLanzamiento = false;
            var hidden_NuevosLanzamiento = $('#hiddenIsNuevosLanzamiento').val();
            if (typeof hidden_NuevosLanzamiento == 'undefined') {
                hidden_NuevosLanzamiento = null;
            } else {
                if (hidden_NuevosLanzamiento === '1') {
                    isNuevosLanzamiento = true;
                }
            }

            if (idOferta != null) {
                if (parseInt(idOferta) == 0 && isNuevosLanzamiento) {
                    $('#txt_nuevosLanzamiento').attr('checked', true);
                }
            }
            //
            if (parseInt(idOferta) != 0) {
                var idTipo = $('#hiddenOfe_tipo').val();
                if (typeof idTipo == 'undefined') {
                    idTipo = null;
                } else {
                    if (parseInt(idTipo) == 1) {
                        $('#opProducto').attr('checked', true);// $("#radio").attr('checked', true); 
                        $('#opTransfer').attr('checked', false);
                    } else if (parseInt(idTipo) == 2) {
                        $('#opTransfer').attr('checked', true);
                        $('#opProducto').attr('checked', false);
                    }
                }
                onclickRadio();
            }
            $('#txt_fechaFin').datepicker();

            if (isNuevosLanzamiento) {
                var varHtml = $('#hidden_descrHtml').val();
                if (typeof varHtml == 'undefined') {
                    varHtml = '';
                }
                $('#textareaHtml').wysihtml5({
                    "html": true, //Button which allows you to edit the generated HTML. Default false
                    "color": true
                }).html(varHtml);
            } else {
                $('.divformGroupTxtHtml').hide();
            }
        });
        function onclickRadio() {
            var val = $('input[name=opciones]:checked', '#Form2').val();
            if (val == 1) {
                $('#contenedorProductos').css('visibility', 'visible');
                $('#contenedorTransfer').css('visibility', 'hidden');
            }
            else if (val == 2) {
                $('#contenedorTransfer').css('visibility', 'visible');
                $('#contenedorProductos').css('visibility', 'hidden');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="sub-header">Editar/Agregar
    </h2>
    <button type="button" class="btn btn-info" onclick="GuardarOferta();  return false;">Guardar</button>
    &nbsp;&nbsp;
    <button type="button" class="btn btn-info" onclick="return VolverOferta();">Volver</button>
    <div class="form-check" style="visibility: hidden">
        <label for="txt_nuevosLanzamiento" class="form-check-label">Nuevo Lanzamiento:</label>
        <input type="checkbox" class="form-check-input" id="txt_nuevosLanzamiento" disabled="disabled" />
    </div>
    <div class="form-group">
        <label for="txt_titulo">Título:</label>
        <input type="text" class="form-control" id="txt_titulo" style="text-transform: uppercase;" />
    </div>
    <div class="form-group">
        <label for="txt_descr">Descrición:</label>
        <input type="text" class="form-control" id="txt_descr" />
    </div>
    <div class="form-group">
        <label for="txt_descuento">Descuento:</label>
        <input type="text" class="form-control" id="txt_descuento" style="text-transform: uppercase;" />
    </div>
    <div class="form-group">
        <label for="txt_etiqueta">Etiqueta:</label>
        <input type="text" class="form-control" id="txt_etiqueta" style="text-transform: uppercase;" />
    </div>
    <div class="form-group">
        <label for="txt_etiquetaColor">Etiqueta Color:</label>
        <select id="cmdEtiquetaColor" class="selectpicker form-control">
            <option value="E74C3C" style="background-color: #E74C3C; color: #FFF;">Rojo</option>
            <option value="2ECC71" style="background-color: #2ECC71; color: #FFF;">Verde</option>
        </select>

    </div>
    <div class="form-group divformGroupTxtHtml">
        <label for="txtHtml">Html:</label>
        <textarea id="textareaHtml" class="textarea" placeholder="" style="width: 810px; height: 200px"></textarea>
    </div>
    <div class="form-group">
        <label for="txt_fechaFin">Fecha Fin:</label>
        <input type="text" class="form-control" id="txt_fechaFin" />
    </div>

    <div id="contenedorProductoTransfer" class="row">
        <div class="col-md-6">
            <div class="radio">
                <label>
                    <input type="radio" name="opciones" id="opProducto" value="1" checked="checked" onclick="onclickRadio()" />
                    Producto
                </label>
            </div>
            <div id="contenedorProductos">
                <h3 class="sub-header">Productos de la oferta
                </h3>
                <div id="divContenedorGrillaOfertaProducto"></div>
                <hr />

                <h4 class="sub-header">Buscar productos
                </h4>
                <div class="form-group">
                    <label for="txtBuscar">Palabra clave:</label>
                    <input type="text" class="form-control" id="txtBuscar" onkeypress="onkeypressAgregarBuscar(event)" />
                </div>
                <button id="btnBuscar" type="button" class="btn btn-primary" onclick="return busOferta();">Buscar</button>

                <div id="divContenedorGrilla"></div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="radio">
                <label>
                    <input type="radio" name="opciones" id="opTransfer" value="2" onclick="onclickRadio()" />
                    Transfer
                </label>
            </div>
            <div id="contenedorTransfer">
                <h3 class="sub-header">Seleccionar Transfer
                </h3>

                <div class="form-group">
                    <label for="cmdTransfer">Transfer:</label>
                    <select id="cmdTransfer" class="selectpicker">
                        <%Response.Write(getHtmlOptionTransfer()); %>
                    </select>
                </div>
            </div>
        </div>
    </div>
    <%  AgregarHtmlOculto(); %>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            var ofe_fechaFin = $('#hiddenOfe_fechaFin').val();
            if (typeof ofe_fechaFin == 'undefined') {
                ofe_fechaFin = null;
            } else {
            }

            var $datepicker = $('#txt_fechaFin');
            $datepicker.datepicker("option", "dateFormat", 'dd/mm/yy');
            if (ofe_fechaFin != null)
                $datepicker.datepicker('setDate', ofe_fechaFin);
        });
    </script>
</asp:Content>
