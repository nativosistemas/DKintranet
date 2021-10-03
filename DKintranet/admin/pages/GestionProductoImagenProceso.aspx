<%@ Page Title="" Language="C#" MasterPageFile="~/master/BaseAdminJs3.Master" AutoEventWireup="true" CodeBehind="GestionProductoImagenProceso.aspx.cs" Inherits="DKintranet.admin.pages.GestionProductoImagenProceso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../includes/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../../includes/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../includes/js/GestionProductoImagen.js" type="text/javascript"></script>
    <script type="text/javascript">

        jQuery(document).ready(function () {


        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="sub-header">Iimágenes para grabar ancho y alto predeterminado al ampliar
    </h2>
    <asp:Button ID="Button1" runat="server" Text="Procesar" CssClass="btn btn-primary" OnClick="Button1_Click" />
    <br />
    <asp:CheckBox ID="CheckBoxTodosLosNull" runat="server" Text="Todos los null" Checked="true" />
    <br />
    <hr />
    <br />
     <asp:Button ID="Button2" runat="server" Text="Borrar ancho y alto default" CssClass="btn btn-primary" OnClick="Button2_Click" />
</asp:Content>

