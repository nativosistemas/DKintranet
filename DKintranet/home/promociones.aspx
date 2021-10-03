<%@ Page Title="" Language="C#" MasterPageFile="~/master/home.Master" AutoEventWireup="true" CodeBehind="promociones.aspx.cs" Inherits="DKintranet.home.promociones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <style type="text/css">
        .buttononVolver {
            float: right;
            right: 10px;
            width: 40px;
            height: 40px;
            background-color: #5E6E7F;
            border: 1px solid #5E6E7F;
            text-align: center;
            text-decoration: none;
            cursor: pointer;
            z-index: 99;
        }

            .buttononVolver:hover {
                background-color: transparent !important;
                -webkit-transition-duration: .5s;
                -moz-transition-duration: .5s;
                -ms-transition-duration: .5s;
                -o-transition-duration: .5s;
                transition-duration: .5s;
            }

            .buttononVolver i {
                top: 0;
                padding-top: 8px !important;
                font-size: 22px;
                color: #fff !important;
            }

            .buttononVolver:hover i {
                color: #5E6E7F !important;
            }

        .section-gris-claro {
            padding-top: 20px;
        }
        .divPromocionesVolver {
          padding-bottom: 20px;
        }
    </style>--%>
    <script type="text/javascript" src="../includes/home/js/oferta.js?n=11"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            // RecuperarTodasOfertas();
            prepararListaOfertas($('#hiddenListaOfertas').val());
        });
        //function funPromocionesVolver() {
        //    location.href = 'index.aspx';
        //}
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <section class="section-gris-claro" id="todasOfertas">

        <%--        <div class="container divPromocionesVolver">
            <div class="buttononVolver" title="Volver" onclick="funPromocionesVolver()"><i class="fa fa-caret-left"></i></div>
        </div>--%>
        <div class="container">
            <div class="col-xs-12 no-padding"><a class="btn_volver float-left" href="index.aspx"><i class="fa fa-play"></i>VOLVER</a></div>
            <div class="clear25"></div>
        </div>
        <div id="divContenedorOfertas" class="container">
        </div>
    </section>
    <%  AgregarHtmlOculto(); %>
</asp:Content>

