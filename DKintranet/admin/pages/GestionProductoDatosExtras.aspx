<%@ Page Title="" Language="C#" MasterPageFile="~/master/BaseAdmin.Master" AutoEventWireup="true" CodeBehind="GestionProductoDatosExtras.aspx.cs" Inherits="DKintranet.admin.pages.GestionProductoDatosExtras" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <div class="titulo_pagina">
        Productos Parametrizar Cantidad</div>
    <table>
        <tr>
            <td>
                Cantidad producto
            </td>
            <td>
                <asp:TextBox ID="txt_Cantidad" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="cmd_guardar" runat="server" Text="Guardar" CssClass="btn_abm" CausesValidation="False"
                    OnClick="cmd_guardar_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
