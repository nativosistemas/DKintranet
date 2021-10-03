<%@ Page Title="" Language="C#" MasterPageFile="~/master/BaseAdminJs3.Master" AutoEventWireup="true" CodeBehind="GestionOferta.aspx.cs" Inherits="DKintranet.admin.pages.GestionOferta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../includes/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../../includes/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../includes/js/GestionOferta.js?n=5" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            RecuperarTodasOfertas();
            var isNuevoLanzamiento = $('#hiddenOfe_isNuevoLanzamiento').val();
            if (typeof isNuevoLanzamiento == 'undefined') {
                //titulo = null;
            } else {
                if (isNuevoLanzamiento === '1') { 
                    $('#tituloHeader').html('Nuevos Lanzamientos');
                }
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="sub-header" id="tituloHeader">Ofertas</h2>
    <button type="button" class="btn btn-warning" onclick="return EditarOferta(0);">Agregar</button>
    <div id="divContenedorGrilla"></div>
    <%  AgregarHtmlOculto(); %>
</asp:Content>
