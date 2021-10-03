<%@ Page Title="" Language="C#" MasterPageFile="~/master/home.Master" AutoEventWireup="true" CodeBehind="recalls.aspx.cs" Inherits="DKintranet.home.recalls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script type="text/javascript">
        jQuery(document).ready(function () {
            prepararListaReCall($('#hiddenListaReCall').val());
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="section-gris-claro">
        <div class="container">
            <div id="divContenedorRecall" class="row">

            </div>
        </div>
    </section>
 <%  AgregarHtmlOculto(); %>
</asp:Content>
