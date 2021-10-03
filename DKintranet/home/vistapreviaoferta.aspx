<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="vistapreviaoferta.aspx.cs" Inherits="DKintranet.home.vistapreviaoferta" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" href="favicon.ico" />
    <title>Droguería Kellerhoff</title>
    <link href="../includes/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../includes/home/css/main.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Montserrat:300,400,600" rel="stylesheet" />


</head>
<body>
    <form id="form1" name="form1" runat="server">
         <div id="modalModulo" class="modal md-effect-1 md-content portfolio-modal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true"></div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"
            EnablePageMethods="true" EnableScriptGlobalization="True">
        </asp:ScriptManager>

        <div id="divContenedorOfertas"></div>
    </form>
    <script src="../includes/js/jquery-3.1.0.min.js"></script>
    <script src="../includes/js/bootstrap.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="../includes/home/js/jquery/function.js"></script>
    <script type="text/javascript" src="../includes/home/js/jquery/validate.js"></script>
    <script type="text/javascript" src="../includes/home/js/jquery/function_forms.js"></script>
    <script type="text/javascript" src="../includes/home/js/jquery/owl.carousel.min.js"></script>
    <script src="../includes/js/Libreria.js?n=13"></script>
  <script type="text/javascript" src="../includes/mvc/js/mensaje.js?n=30"></script>
    <script src="../includes/home/js/oferta.js?n=14"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            prepararListaOfertas($('#hiddenListaOfertas').val());
        });
    </script>
    <%  AgregarHtmlOculto(); %>
</body>
</html>
