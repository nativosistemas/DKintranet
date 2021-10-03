<%@ Page Title="" Language="C#" MasterPageFile="~/master/home.Master" AutoEventWireup="true" CodeBehind="recall.aspx.cs" Inherits="DKintranet.home.recall" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .wysiwyg-color-black {
  color: black;
}

.wysiwyg-color-silver {
  color: silver;
}

.wysiwyg-color-gray {
  color: gray;
}

.wysiwyg-color-white {
  color: white;
}

.wysiwyg-color-maroon {
  color: maroon;
}

.wysiwyg-color-red {
  color: red;
}

.wysiwyg-color-purple {
  color: purple;
}

.wysiwyg-color-fuchsia {
  color: fuchsia;
}

.wysiwyg-color-green {
  color: green;
}

.wysiwyg-color-lime {
  color: lime;
}

.wysiwyg-color-olive {
  color: olive;
}

.wysiwyg-color-yellow {
  color: yellow;
}

.wysiwyg-color-navy {
  color: navy;
}

.wysiwyg-color-blue {
  color: blue;
}

.wysiwyg-color-teal {
  color: teal;
}

.wysiwyg-color-aqua {
  color: aqua;
}

.wysiwyg-color-orange {
  color: orange;
}
    </style>
     <script type="text/javascript">
        jQuery(document).ready(function () {
            prepararReCall($('#hiddenObjReCall').val());
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <section class="section-gris-claro">
        <div class="container">
        	<div id="divContenedorObjRecall"  class="row">
        	</div>
        </div>
    </section>
     <%  AgregarHtmlOculto(); %>
</asp:Content>
