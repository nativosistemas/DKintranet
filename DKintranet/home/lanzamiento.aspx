<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lanzamiento.aspx.cs" Inherits="DKintranet.home.lanzamiento" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="author" content="Nativo Sistemas" />
    <meta name="distribution" content="Global" />
    <meta name="city" content="Rosario" />
    <meta name="country" content="Argentina" />
    <meta name="geography" content="Pueyrredón 1149 - S2000QIG - Rosario - Santa Fe - Argentina" />
    <meta name="robots" content="noindex,nofollow,noarchive,noodp,nosnippet" />
    <meta name="language" content="Spanish" />
    <meta name="theme-color" content="#2C3E50" />
    <link rel="shortcut icon" href="~/favicon.ico" />

    <link href="../includes/mvc/css/main.css?n=22" rel="stylesheet" />
    <link href="../includes/mvc/css/select2.min.css?n=3" rel="stylesheet" />
    <link href="../includes/mvc/css/main-overwrite.css?n=58" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="../admin/pages/lib/css/wysiwyg-color.css" />
    <title></title>
</head>
<body class="bd_sec">
    <div class="container-fluid">
    <div id="divContenedorOfertas"></div>
    </div>
            

    <script src="../includes/js/jquery-3.1.0.min.js"></script>
    <script src="../includes/js/bootstrap.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="../includes/home/js/jquery/function.js"></script>
    <script type="text/javascript" src="../includes/home/js/jquery/validate.js"></script>
    <script type="text/javascript" src="../includes/home/js/jquery/function_forms.js"></script>
    <script type="text/javascript" src="../includes/home/js/jquery/owl.carousel.min.js"></script>
    <script src="../includes/js/Libreria.js?n=13"></script>
  <script type="text/javascript" src="../includes/mvc/js/mensaje.js?n=30"></script>
    <script src="../includes/mvc/js/oferta.js?n=19"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            prepararListaMsgLanzamiento($('#hiddenListaOfertas').val());
        });
    </script>
    <%  AgregarHtmlOculto(); %>
</body>
</html>
