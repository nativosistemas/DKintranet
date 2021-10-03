<%@ Page Title="" Language="C#" MasterPageFile="~/master/BaseAdminJs3.Master" AutoEventWireup="true" CodeBehind="GestionProductoImagen.aspx.cs" Inherits="DKintranet.admin.pages.GestionProductoImagen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="../../includes/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../../includes/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../includes/js/GestionProductoImagen.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('#txtBuscar').val($('#hiddenText').val());
            bus();
            $('#Form2').submit(function () {
                bus();
            return false;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <h2 class="sub-header">Imagen para los productos</h2>
 <div class="form-group">
  <label for="txtBuscar">Palabra clave:</label>
  <input type="text" class="form-control" id="txtBuscar" />
</div>
  <button id="btnBuscar" type="button" class="btn btn-primary"  onclick="return bus();"   >Buscar</button>

    <div id="divContenedorGrilla"></div>

</asp:Content>
