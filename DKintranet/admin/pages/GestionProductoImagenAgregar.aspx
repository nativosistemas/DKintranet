<%@ Page Title="" Language="C#" MasterPageFile="~/master/BaseAdminJs3.Master" AutoEventWireup="true" CodeBehind="GestionProductoImagenAgregar.aspx.cs" Inherits="DKintranet.admin.pages.GestionProductoImagenAgregar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="../../includes/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../../includes/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../includes/js/GestionProductoImagen.js" type="text/javascript"></script>
    <script type="text/javascript">

        jQuery(document).ready(function () {
            $('#txtNumero').val($('#hiddenNumero').val());
            $('#txtNombre').val($('#hiddenNombre').val());

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="sub-header">Agregar imagen al producto
    </h2>
    <div class="form-group">
        <label for="txtBuscar">Número:</label>
        <input type="text" class="form-control" id="txtNumero" disabled="disabled" />
    </div>
    <div class="form-group">
        <label for="txtBuscar">Nombre:</label>
        <input type="text" class="form-control" id="txtNombre" disabled="disabled" />
    </div>
    <asp:FileUpload ID="FileUpload1" runat="server" />
    <br />
    <button type="submit" class="btn btn-primary">Subir</button>
    <button type="button" class="btn btn-primary" onclick="onclickVolverProductoImagen(); return false;">Volver</button>
</asp:Content>
