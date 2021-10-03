<%@ Page Title="" Language="C#" MasterPageFile="~/master/BaseAdminJs3.Master" AutoEventWireup="true" CodeBehind="GestionOfertaHome.aspx.cs" Inherits="DKintranet.admin.pages.GestionOfertaHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../includes/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../../includes/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../includes/js/GestionOferta.js?n=0" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            $('#cmdOfertaHome_1').val(-1);
            $('#cmdOfertaHome_2').val(-1);
            $('#cmdOfertaHome_3').val(-1);
            $('#cmdOfertaHome_4').val(-1);
            $('#cmdLanzamientoHome_1').val(-1);
            $('#cmdLanzamientoHome_2').val(-1);
            $('#cmdLanzamientoHome_3').val(-1);
            $('#cmdLanzamientoHome_4').val(-1);
            var listOfertaHome = $('#hiddenListOfeta').val();
            if (typeof listOfertaHome == 'undefined') {
                listOfertaHome = null;
            } else {
                listOfertaHome = eval('(' + listOfertaHome + ')');

                for (var i = 0; i < listOfertaHome.length; i++) {
                    switch (listOfertaHome[i].ofh_orden)
                    {
                        case 1:
                            $('#cmdOfertaHome_1').val(listOfertaHome[i].ofh_idOferta);
                            break;
                        case 2:
                            $('#cmdOfertaHome_2').val(listOfertaHome[i].ofh_idOferta);
                            break;
                        case 3:
                            $('#cmdOfertaHome_3').val(listOfertaHome[i].ofh_idOferta);
                            break;
                        case 4:
                            $('#cmdOfertaHome_4').val(listOfertaHome[i].ofh_idOferta);
                            break;
                        case 5:
                            $('#cmdLanzamientoHome_1').val(listOfertaHome[i].ofh_idOferta);
                            break;
                        case 6:
                            $('#cmdLanzamientoHome_2').val(listOfertaHome[i].ofh_idOferta);
                            break;
                        case 7:
                            $('#cmdLanzamientoHome_3').val(listOfertaHome[i].ofh_idOferta);
                            break;
                        case 8:
                            $('#cmdLanzamientoHome_4').val(listOfertaHome[i].ofh_idOferta);
                            break;
                    }
                }
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="sub-header">Configuración  Home</h2>
    <button type="button" class="btn btn-info" onclick="GuardarOfertaHome();  return false;">Guardar</button>
    &nbsp;&nbsp;
    <br />
    <br />
        <h3 class="sub-header">Ofertas Home</h3>
        <br />
    <br />
    <div class="row">
        <div class="col-md-3">
            <div class="form-group">
                <label for="cmdOfertaHome_1">Oferta Home 1:</label>
                <select id="cmdOfertaHome_1" class="selectpicker">
                    <%Response.Write(getHtmlOptionOferta()); %>
                </select>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label for="cmdOfertaHome_2">Oferta Home 2:</label>
                <select id="cmdOfertaHome_2" class="selectpicker">
                    <%Response.Write(getHtmlOptionOferta()); %>
                </select>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label for="cmdOfertaHome_3">Oferta Home 3:</label>
                <select id="cmdOfertaHome_3" class="selectpicker">
                    <%Response.Write(getHtmlOptionOferta()); %>
                </select>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label for="cmdOfertaHome_4">Oferta Home 4:</label>
                <select id="cmdOfertaHome_4" class="selectpicker">
                    <%Response.Write(getHtmlOptionOferta()); %>
                </select>
            </div>
        </div>
    </div>
<br />
    <h3 class="sub-header">Lanzamiento Home</h3>
    <br />
    <br />
    <div class="row">
        <div class="col-md-3">
            <div class="form-group">
                <label for="cmdLanzamientoHome_1">Lanzamiento Home 1:</label>
                <select id="cmdLanzamientoHome_1" class="selectpicker">
                    <%Response.Write(getHtmlOptionLanzamiento()); %>
                </select>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label for="cmdLanzamientoHome_2">Lanzamiento Home 2:</label>
                <select id="cmdLanzamientoHome_2" class="selectpicker">
                    <%Response.Write(getHtmlOptionLanzamiento()); %>
                </select>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label for="cmdLanzamientoHome_3">Lanzamiento Home 3:</label>
                <select id="cmdLanzamientoHome_3" class="selectpicker">
                    <%Response.Write(getHtmlOptionLanzamiento()); %>
                </select>
            </div>
        </div>
        <div class="col-md-3">
            <div class="form-group">
                <label for="cmdLanzamientoHome_4">Lanzamiento Home 4:</label>
                <select id="cmdLanzamientoHome_4" class="selectpicker">
                    <%Response.Write(getHtmlOptionLanzamiento()); %>
                </select>
            </div>
        </div>
    </div>
    <%AgregarHtmlOculto(); %>

</asp:Content>
