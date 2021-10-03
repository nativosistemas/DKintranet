<%@ Page Title="" Language="C#" MasterPageFile="~/master/BaseAdminJs3.Master" AutoEventWireup="true" CodeBehind="ModuloEditar.aspx.cs" Inherits="DKintranet.admin.pages.ModuloEditar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../includes/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../../includes/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../includes/js/Modulo.js?n=0" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            var nombre = $('#hidden_nombre').val();
            if (typeof nombre == 'undefined') {
                nombre = null;
            } else {
                $('#txt_nombre').val(nombre);
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="sub-header">Editar/Agregar
    </h2>
    <button type="button" class="btn btn-info" onclick="Guardar();  return false;">Guardar</button>
    &nbsp;&nbsp;
    <button type="button" class="btn btn-info" onclick="return Volver();">Volver</button>
    <div class="form-group">
        <label for="txt_titulo">Nombre:</label>
        <input type="text" class="form-control" id="txt_nombre" style="width: 810px;" />
    </div>
    <%  AgregarHtmlOculto(); %>
</asp:Content>
