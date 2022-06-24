function AgregarCarritoHtml(pIndexCarrito) {
    var strHTML = '';
    strHTML += '<div id="divContenedorCarrito_' + pIndexCarrito + '" class="div_carrito">';
    strHTML += '<div class="tit">';
    strHTML += getNombreSucursalCarrito(pIndexCarrito);
    var indiceReferente = 0;
    for (var iNombreSucursal = 0; iNombreSucursal < listaSucursalesDependienteInfo.length; iNombreSucursal++) {
        if (listaSucursalesDependienteInfo[iNombreSucursal].sde_sucursal == listaCarritos[pIndexCarrito].codSucursal) {
            indiceReferente += iNombreSucursal;
            break;
        }
    }

    var isMostrarHorarioCierre = false;
    var visibleCssHorarioCierreTitulo = ' style="visibility:hidden;" ';
    if (isNotNullEmpty(listaCarritos[pIndexCarrito].proximoHorarioEntrega) && cli_codsuc() != listaCarritos[pIndexCarrito].codSucursal) {
        isMostrarHorarioCierre = true;
        visibleCssHorarioCierreTitulo = '';
    }
    strHTML += '<div class="clear5"></div>';
    strHTML += '<span id="contenedorProximaEntrega_' + pIndexCarrito + '" ' + visibleCssHorarioCierreTitulo + '>';
    strHTML += 'Próximo&nbsp;cierre:&nbsp;';
    strHTML += '<font id="proximaEntrega_' + pIndexCarrito + '">';
    if (isMostrarHorarioCierre) {
        strHTML += listaCarritos[pIndexCarrito].proximoHorarioEntrega;
    }
    strHTML += '</font>';
    strHTML += '</span>';
    strHTML += '</div>'; //'<div class="tit">'


    strHTML += '<div class="div_carrito_head scroll">';
    strHTML += '<table class="footable table carrito table-stripped" width="100%" align="center" cellspacing="0" cellpadding="0" border="0"><thead><tr>';
    strHTML += '<th class="col-lg-7 col-md-3 col-sm-3 col-xs-10 text-left thProducto' + listaCarritos[pIndexCarrito].codSucursal + '">Producto</th>';
    strHTML += '<th class="col-lg-2 col-md-1 col-sm-1 col-xs-1 text-center thCant' + listaCarritos[pIndexCarrito].codSucursal + '">Cant.</th>';
    strHTML += '<th class="col-lg-3 col-md-1 col-sm-1 col-xs-1 text-center thPrecio' + listaCarritos[pIndexCarrito].codSucursal + '"><div style="display: inline-block;">Precio</div></th>';
    strHTML += '</tr></thead></table>';
    strHTML += '</div>'; //<div class="div_carrito_head scroll">
    //
    strHTML += '<div id="div_carrito_cont_' + pIndexCarrito + '" class="div_carrito_cont">';
    strHTML += '<table class="footable table carrito table-stripped" data-empty="No hay informacion disponible" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">';
    strHTML += '<tbody>';
    //
    var nroTotalCarrito = parseFloat(0);
    var cantProductosTotales = 0;
    for (var iProductos = 0; iProductos < listaCarritos[pIndexCarrito].listaProductos.length; iProductos++) {
        cantProductosTotales += listaCarritos[pIndexCarrito].listaProductos[iProductos].cantidad;
        // Precio Calcular por producto
        var nroTotalProducto = CalcularPrecioProductosEnCarrito(listaCarritos[pIndexCarrito].listaProductos[iProductos].PrecioFinal, listaCarritos[pIndexCarrito].listaProductos[iProductos].cantidad, listaCarritos[pIndexCarrito].listaProductos[iProductos].pro_ofeunidades, listaCarritos[pIndexCarrito].listaProductos[iProductos].pro_ofeporcentaje);
        var strHtmlColor = 'grs';
        if (iProductos % 2 != 0) {
            strHtmlColor = 'wht';
        }
        /* falta */
        var htmlColor = '';
        if (listaCarritos[pIndexCarrito].listaProductos[iProductos].stk_stock == 'N') {
            htmlColor = ' color_red ';
        }
        /* fin falta */
        strHTML += '<tr class="' + strHtmlColor + '">';
        strHTML += '<td class="col-lg-7 col-md-3 col-sm-3 text-left ' + htmlColor + ' tdProducto' + listaCarritos[pIndexCarrito].codSucursal + '">' + listaCarritos[pIndexCarrito].listaProductos[iProductos].pro_nombre + '</td>';
        strHTML += '<td class="col-lg-2 col-md-1 col-sm-1 col-xs-1 text-center tdCant' + listaCarritos[pIndexCarrito].codSucursal + '">';
        var typeInput = ' type="text" ';
        if (isMobile()) if (isMobile()) {
            typeInput = ' type="number" ';
        }
        var typeInput_SoloTransferFacturacionDirecta = '';
        if (isSoloTransferFacturacionDirecta(listaCarritos[pIndexCarrito].listaProductos[iProductos], listaCarritos[pIndexCarrito].codSucursal, listaCarritos[pIndexCarrito].listaProductos[iProductos].cantidad, false)) {
            typeInput_SoloTransferFacturacionDirecta = ' disabled ';
        }
        strHTML += '<input ' + typeInput_SoloTransferFacturacionDirecta + ' class="form-shop w_100" id="inputCarrito' + pIndexCarrito + '_' + iProductos + '" ' + typeInput + '  value="' + listaCarritos[pIndexCarrito].listaProductos[iProductos].cantidad + '" onblur="onblurInputCarrito(this)" onfocus="onfocusInputCarrito(this)" onkeypress="return onKeypressCantProductos(event)" />';
        strHTML += '</td>';
        var strHtmlPrecioProducto = '';
        if (listaCarritos[pIndexCarrito].listaProductos[iProductos].stk_stock == 'N') {
            nroTotalProducto = 0;
        } else {
            strHtmlPrecioProducto = '$&nbsp;' + FormatoDecimalConDivisorMiles(nroTotalProducto.toFixed(2));
        }
        //<td class="col-lg-4 col-md-1 col-sm-1 text-right">
        strHTML += '<td class="col-lg-4 col-md-1 col-sm-1 text-right tdPrecio' + listaCarritos[pIndexCarrito].codSucursal + '"  id="tdPrecio' + pIndexCarrito + '_' + iProductos + '" > ' + strHtmlPrecioProducto + '</td> ';
        strHTML += '</tr>';
        nroTotalCarrito = nroTotalCarrito + nroTotalProducto;
    }
    //
    strHTML += '</tbody></table>';
    //strHTML += '<div id="Scroll_' + pIndexCarrito + '" style="max-height:250px;overflow-y:scroll;overflow-x:hidden;">';
    strHTML += '</div>'; //<div class="div_carrito_cont">
    //
    strHTML += '<div class="col-lg-9 col-md-9 col-sm-9 text-left div_renglones">Renglones ' + listaCarritos[pIndexCarrito].listaProductos.length + '<span  id="tdUnidades' + pIndexCarrito + '" class="pull-right">' + textUnidades + cantProductosTotales + '</span></div>';
    strHTML += '<div class="col-lg-3 col-md-3 col-sm-3 col-xs-1 text-center"></div>';
    strHTML += '<div class="col-xs-12 div_total">Total<span id="tdTotal' + pIndexCarrito + '" class="pull-right">' + '$&nbsp;' + FormatoDecimalConDivisorMiles(nroTotalCarrito.toFixed(2)) + '</span></div>';
    strHTML += '<div class="col-xs-12 pad_lr_car">';
    strHTML += '<button type="button" id="btn_confirmar_' + indiceReferente + '" class="btn_confirmar" onclick="onclickIsPuedeUsarDll(' + pIndexCarrito + '); return false;" href="#">CONFIRMAR</button>';
    strHTML += '<button type="button" id="btn_vaciar_' + indiceReferente + '" class="btn_vaciar float-left" onclick="onclickVaciarCarrito(' + pIndexCarrito + '); return false;" href="#">VACIAR</button>';
    strHTML += '</div>';
    strHTML += '<div class="clear"></div>';
    //
    strHTML += '</div>';// class="div_carrito"
    //
    CargarContenedorBaseTotales();
    //
    return strHTML;
}
function obtenerCarritoUnidades(pIndexCarrito) {
    var cantProductosTotales = parseInt(0);
    for (var iProductos = 0; iProductos < listaCarritos[pIndexCarrito].listaProductos.length; iProductos++) {
        if (isNotNullEmpty(listaCarritos[pIndexCarrito].listaProductos[iProductos].cantidad)) {
            cantProductosTotales += parseInt(listaCarritos[pIndexCarrito].listaProductos[iProductos].cantidad);
        }
    }
    return cantProductosTotales;
}
function getNombreSucursalCarrito(pIndexCarrito) {
    var prefijo = 'Sucursal ';
    var nombreSucursal = listaCarritos[pIndexCarrito].codSucursal;
    if (listaSucursales != null) {
        for (var i = 0; i < listaSucursales.length; i++) {
            if (listaSucursales[i].sde_sucursal == listaCarritos[pIndexCarrito].codSucursal) {
                nombreSucursal = listaSucursales[i].suc_nombre;
                break;
            }
        }
    }
    return prefijo + nombreSucursal;
}

function onclickIsPuedeUsarDll(pIndexCarrito) {
    indexCarritoHorarioCierre = pIndexCarrito;
    IsHacerPedidos(listaCarritos[pIndexCarrito].codSucursal, 'OnCallBackIsHacerPedidos');
}
function OnCallBackIsHacerPedidos(args) {
    if (args == 0) {
        onclickConfirmarCarrito(indexCarritoHorarioCierre);
    } else if (args == 2) {
        if (isCarritoDiferido) {
            location.href = '../mvc/Diferido'; //= 'carrodiferido.aspx';
        } else {
            location.href = '../mvc/Buscador'; //= 'PedidosBuscador.aspx';
        }
    } else if (args == 1) {
        mensaje_informacion(mensajeTareasMantenimiento);
    }
}
function onclickConfirmarCarrito(pIndexCarrito) {
    var creditoRestante = getCreditoRestante();
    if (creditoRestante < 0) {
        var htmlMensaje = '<p>' + cuerpo_creditoInsuficiente + '</p>';
        mensaje_credito(titulo_creditoInsuficiente, htmlMensaje);
    } else {
        MostrarConfirmarCarrito(pIndexCarrito, false);
        onChangeTipoEnvio();
        $("#hiddenIndexCarrito").val(pIndexCarrito);
        //// Acutalizar horario reparto
        indexCarritoHorarioCierre = pIndexCarrito;
        ObtenerHorarioCierre(listaCarritos[indexCarritoHorarioCierre].codSucursal, 'OnCallBackObtenerHorarioCierre');
        //// fin Acutalizar horario reparto
    }
}
//function CargarHtmlTipoEnvio(pSucursal) {
//    var strHtml = '';
//    strHtml += '<select id="comboTipoEnvio" class="select_gral" onchange="onChangeTipoEnvio()">';
//    strHtml += CargarHtmlOptionTipoEnvio(pSucursal);
//    strHtml += '</select>';
//    $('#tdTipoEnvio').html(strHtml);
//    onChangeTipoEnvio();
//}
// Armar tipo envio
function CargarHtmlOptionTipoEnvio(pSucursal) {
    var strHtml = '';
    if (listaTipoEnviosSucursal != null) {
        var isSeEncontro = false;
        for (var i = 0; i < listaTipoEnviosSucursal.length; i++) {
            if (listaTipoEnviosSucursal[i].sucursal == pSucursal && listaTipoEnviosSucursal[i].tipoEnvio == cli_codtpoenv()) {
                isSeEncontro = true;
                for (var y = 0; y < listaTipoEnviosSucursal[i].lista.length; y++) {
                    strHtml += '<option value="' + listaTipoEnviosSucursal[i].lista[y].env_codigo + '">' + listaTipoEnviosSucursal[i].lista[y].env_nombre + '</option>';
                }
                break;
            }
        }
        if (!isSeEncontro) {
            for (var i = 0; i < listaTipoEnviosSucursal.length; i++) {
                if (listaTipoEnviosSucursal[i].sucursal == pSucursal && listaTipoEnviosSucursal[i].tipoEnvio == null) {
                    for (var y = 0; y < listaTipoEnviosSucursal[i].lista.length; y++) {
                        strHtml += '<option value="' + listaTipoEnviosSucursal[i].lista[y].env_codigo + '">' + listaTipoEnviosSucursal[i].lista[y].env_nombre + '</option>';
                    }
                    break;
                }
            }
        }
    }
    return strHtml;
}
// fin armar tipo envio
function onChangeTipoEnvio() {
    $('#checkboxIsUrgentePedido').removeAttr("checked");
    $('#tdIsUrgente').css('visibility', 'hidden');
}
function OnCallBackObtenerHorarioCierre(args) {
    if (args != null && args != '') {
        if (indexCarritoHorarioCierre != null) {
            listaCarritos[indexCarritoHorarioCierre].proximoHorarioEntrega = args;
            if (cli_codsuc() != listaCarritos[indexCarritoHorarioCierre].codSucursal) {
                $('#proximaEntrega_' + indexCarritoHorarioCierre).html(listaCarritos[indexCarritoHorarioCierre].proximoHorarioEntrega);
                $('#contenedorProximaEntrega_' + indexCarritoHorarioCierre).css('visibility', 'visible');
            }
        }
    }
    indexCarritoHorarioCierre = null;
}
function MostrarConfirmarCarrito(pIndexCarrito, pIsTransfer) {
    var onclick = '';
    var codSucursal = '';
    if (pIsTransfer) {
        onclick = 'onclickConfimarTransferPedidoOk()';
        codSucursal = listaCarritoTransferPorSucursal[pIndexCarrito].Sucursal;
    }
    else {
        onclick = 'onclickHacerPedido()';
        codSucursal = listaCarritos[pIndexCarrito].codSucursal;
    }
    var strHtml = '';
    strHtml += '<div class="modal-background">&nbsp;</div>';
    strHtml += '<div class="modal-dialog modal-lg"><div class="modal-content">';
    strHtml += '<div class="modal-header no-padding-bottom">';
    strHtml += '<div class="row">';
    strHtml += '<div class="col-lg-12">';
    strHtml += '<div class="modulo_icon check"></div>';
    strHtml += '<h4>Confirmar</h4>';
    strHtml += '</div>';
    strHtml += '</div>';
    strHtml += '<div class="close-modal" data-dismiss="modal"><i class="fa fa-times"></i></div>';
    strHtml += '</div>';
    strHtml += '<div class="modal-body">';
    strHtml += '<div class="col-md-6 div_msj">';
    strHtml += '<textarea class="" id="txtMensajeFactura" name="txtMensajeFactura" placeholder="Mensaje en factura" maxlength="40" onchange="MensajeFacturaRemitoLength(this)" onkeyup="MensajeFacturaRemitoLength(this)" onpaste="MensajeFacturaRemitoLength(this)"></textarea>';
    strHtml += '</div>';
    strHtml += '<div class="col-md-6 div_msj">';
    strHtml += '<textarea class="" id="txtMensajeRemito" name="txtMensajeRemito" placeholder="Mensaje en remito" maxlength="40" onchange="MensajeFacturaRemitoLength(this)" onkeyup="MensajeFacturaRemitoLength(this)" onpaste="MensajeFacturaRemitoLength(this)"></textarea>';
    strHtml += '</div>';
    strHtml += '<div class="col-md-6">Tipo de envio:&nbsp;';
    strHtml += '<select class="form-shop" name="comboTipoEnvio" id="comboTipoEnvio" onchange="onChangeTipoEnvio()" style="width:200px">';
    strHtml += CargarHtmlOptionTipoEnvio(codSucursal);
    strHtml += '</select>';
    strHtml += '</div>';
    strHtml += '<div id="tdIsUrgente" class="col-md-6">';
    strHtml += '<input id="checkboxIsUrgentePedido" type="checkbox" style="width: 16px; height: 16px;" />';
    strHtml += '&nbsp;Es urgente';
    strHtml += '</div>';
    strHtml += '<div class="clear10"></div>';
    strHtml += '<button type="button" class="btn_confirmar" id="btn_confirmar_HACER_PEDIDO" onclick="' + onclick + '; return false;" href="#">HACER PEDIDO</button>';
    strHtml += '</div>'; // '<div class="modal-body">'
    strHtml += '<div class="clear"></div>';
    strHtml += '</div></div>'; //'<div class="modal-dialog modal-lg"><div class="modal-content">'
    $('#modalModulo').html(strHtml);
    $('#modalModulo').modal();

    $('#btn_confirmar_HACER_PEDIDO').focus();
}
function MensajeFacturaRemitoLength(ta) {
    if (ta.value.length > maxLengthMensajeFacturaRemito) {
        ta.value = ta.value.substring(0, maxLengthMensajeFacturaRemito);
    }
}
function onclickHacerPedido() {
    if (isBotonNoEstaEnProceso) {
        var indexCarrito = $("#hiddenIndexCarrito").val();
        ConfirmarCarrito(indexCarrito);
    }
}
function ConfirmarCarrito(pIndexCarrito) {
    isBotonNoEstaEnProceso = false;
    var montoMinimo = '';
    var precio = ObtenerPrecioConFormato($('#tdTotal' + pIndexCarrito).html());
    var isTomarPedido = true;
    textTipoEnvioCarrito = $('#comboTipoEnvio option:selected').text();
    var codTipoEnvioCarrito = $('#comboTipoEnvio option:selected').val();
    var isOkCadeteriaRestricciones = true;
    var msgCategoriaRestricciones = '';
    if (cli_codtpoenv() != 'C') { // C -> cadeteria
        if (codTipoEnvioCarrito == 'C') {
            for (var i = 0; i < listaCadeteriaRestricciones.length; i++) {
                if (listaCadeteriaRestricciones[i].tcr_codigoSucursal == listaCarritos[pIndexCarrito].codSucursal) {
                    if (listaCadeteriaRestricciones[i].tcr_MontoIgnorar <= precio) {
                        isOkCadeteriaRestricciones = true;
                    } else {
                        var unidades = obtenerCarritoUnidades(pIndexCarrito);// parseInt($('#tdUnidades' + pIndexCarrito).html().replace(textUnidades, ''));
                        if (listaCadeteriaRestricciones[i].tcr_UnidadesMinimas > unidades) {
                            isOkCadeteriaRestricciones = false;
                            msgCategoriaRestricciones = 'Para seleccionar Cadetería como Tipo de Envío el pedido debe tener ' + listaCadeteriaRestricciones[i].tcr_UnidadesMinimas + ' unidades mínimas y ' + listaCadeteriaRestricciones[i].tcr_UnidadesMaximas + ' unidades máximas'; //'UnidadesMinimas';
                        } else if (listaCadeteriaRestricciones[i].tcr_UnidadesMaximas < unidades) {
                            isOkCadeteriaRestricciones = false;
                            msgCategoriaRestricciones = 'Para seleccionar Cadetería como Tipo de Envío el pedido debe tener ' + listaCadeteriaRestricciones[i].tcr_UnidadesMinimas + ' unidades mínimas y ' + listaCadeteriaRestricciones[i].tcr_UnidadesMaximas + ' unidades máximas'; //'UnidadesMaximas';
                        } else if (listaCadeteriaRestricciones[i].tcr_MontoMinimo > precio) {
                            isOkCadeteriaRestricciones = false;
                            msgCategoriaRestricciones = 'Para seleccionar Cadetería como Tipo de Envío el pedido debe superar los $ ' + listaCadeteriaRestricciones[i].tcr_MontoMinimo;  //'MontoMinimo';
                        }
                    }
                    break;
                }
            }
        }
    }
    if (isOkCadeteriaRestricciones) {
        if (textTipoEnvioCarrito == 'Mostrador') {
            if (listaSucursales != null) {
                for (var i = 0; i < listaSucursales.length; i++) {
                    if (listaSucursales[i].suc_codigo == listaCarritos[pIndexCarrito].codSucursal) {
                        if (listaSucursales[i].suc_montoMinimo > 0) {
                            if (listaSucursales[i].suc_montoMinimo > precio) {
                                isTomarPedido = false;
                                montoMinimo = listaSucursales[i].suc_montoMinimo;
                            }
                        }
                        break;
                    } // fin  if (listaSucursales[i].suc_codigo == listaCarritos[pIndexCarrito].codSucursal) {
                }
            }
        }
        if (isTomarPedido) {
            var textFactura = $('#txtMensajeFactura').val();
            var textRemito = $('#txtMensajeRemito').val();
            var isUrgente = $('#checkboxIsUrgentePedido').is(":checked");
            var idTipoEnvio = $('#comboTipoEnvio').val();
            if (isCarritoDiferido) {
                TomarPedidoCarritoDiferido(listaCarritos[pIndexCarrito].codSucursal, textFactura, textRemito, idTipoEnvio, isUrgente);
            }
            else {
                TomarPedidoCarrito(listaCarritos[pIndexCarrito].codSucursal, textFactura, textRemito, idTipoEnvio, isUrgente);
            }
            //
            $('#modalModulo').modal('hide');
            showCargando();
            //
        } else {
            //alert('Para hacer el pedido se debe superar el monto mínimo de ' + '$ ' + montoMinimo);
            mensaje_informacion('Para hacer el pedido se debe superar el monto mínimo de ' + '$ ' + montoMinimo);
            isBotonNoEstaEnProceso = true;
        }
    } // fin   if (isOkCadeteriaRestricciones) {
    else {
        //alert(msgCategoriaRestricciones);
        mensaje_informacion(msgCategoriaRestricciones);
        isBotonNoEstaEnProceso = true;
    }
}
function OnFailBotonEnProceso(args) {
    OnFail(args);
    hideCargando();
    isBotonNoEstaEnProceso = true;
}
function OnCallBackTomarPedidoCarrito(args) {
    isBotonNoEstaEnProceso = true;
    /// mostrar faltantes y problema crediticio
    if (!isNotNullEmpty(args)) {
        args = null;
    } else {
        args = eval('(' + args + ')');
    }
    if (args == null) {
        mensaje_alert_base(mensajeCuandoSeMuestraError, 'volverBuscador()');
    } else {
        // Error dsd dll pedido
        if (args.Error != '') {
            mensaje_alert_base(args.Error, 'volverBuscador()');
            // Fin Error dsd dll pedido
        } else {
            isHacerBorradoCarritos = true;
            CargarRespuestaDePedido(args);
            creditoInicial = args.CreditoInicial;

        }
    }
}
function getHtml_ProductosFacturados(pValor) {
    var listaFaltantes = pValor.Items;
    var isProductosPedidos = false;
    var strHtml = '';
    var cant = 0;
    strHtml += '<div class="col-xs-12">';
    strHtml += '<div class="titleConfirmar">PRODUCTOS FACTURADOS</div>';
    strHtml += '<table class="footable table carrito table-stripped" width="100%" align="center" cellspacing="0" cellpadding="0" border="0"><thead><tr>';
    strHtml += '<th class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">Producto</th>';
    strHtml += '<th class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">Cantidad</th>';
    strHtml += '</tr></thead></table>';
    strHtml += '<table class="footable table popup table-stripped" data-empty="No hay informacion disponible" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">';
    strHtml += '<tbody>';
    for (var iFaltantes = 0; iFaltantes < listaFaltantes.length; iFaltantes++) {
        if (listaFaltantes[iFaltantes].Cantidad > 0) {
            isProductosPedidos = true;
            var strHtmlColorFondo = 'grs';
            if (cant % 2 != 0) {
                strHtmlColorFondo = 'wht';
            }
            strHtml += '<tr class="' + strHtmlColorFondo + '">';
            strHtml += '<td class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">' + listaFaltantes[iFaltantes].NombreObjetoComercial + '</td>';
            strHtml += '<td class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">' + listaFaltantes[iFaltantes].Cantidad + '</td>';
            strHtml += '</tr>';
            cant++;
        }
    }
    strHtml += '</tbody></table>';
    strHtml += '<div style="font-size: 12px;text-align:left;margin-top:2px;background-color: #E5F3E4;"><b>MONTO TOTAL:</b> <span style="color:#0B890A;"> $ ' + FormatoDecimalConDivisorMiles(pValor.MontoTotal.toFixed(2)) + '</span> </div>';
    strHtml += '<div style="font-size: 12px;text-align:left; ">TIPO DE ENVIO: ' + textTipoEnvioCarrito + ' </div>';
    strHtml += '</div>';
    if (!isProductosPedidos)
        return '';
    return strHtml;
}
function getHtml_ProductosEnFalta(pValor) {
    var listaFaltantes = pValor.Items;
    var isFaltantes = false;
    var strHtml = '';
    var cant = 0;
    strHtml += '<div class="col-xs-12">';
    strHtml += '<div class="titleConfirmar">PRODUCTOS EN FALTA</div>';
    strHtml += '<table class="footable table carrito table-stripped" width="100%" align="center" cellspacing="0" cellpadding="0" border="0"><thead><tr>';
    strHtml += '<th class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">Producto</th>';
    strHtml += '<th class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">Cantidad</th>';
    strHtml += '</tr></thead></table>';
    strHtml += '<table class="footable table popup table-stripped" data-empty="No hay informacion disponible" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">';
    strHtml += '<tbody>';
    for (var iFaltantes = 0; iFaltantes < listaFaltantes.length; iFaltantes++) {
        if (listaFaltantes[iFaltantes].Faltas > 0) {
            isFaltantes = true;
            var strHtmlColorFondo = 'grs';
            if (cant % 2 != 0) {
                strHtmlColorFondo = 'wht';
            }
            strHtml += '<tr class="' + strHtmlColorFondo + '">';
            strHtml += '<td class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">' + listaFaltantes[iFaltantes].NombreObjetoComercial + '</td>';
            strHtml += '<td class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">' + listaFaltantes[iFaltantes].Faltas + '</td>';
            strHtml += '</tr>';
            cant++;
        }
    }
    strHtml += '</tbody></table>';
    strHtml += '</div>';
    if (!isFaltantes)
        return '';
    return strHtml;
}
function getHtml_ProductosConProblemasDeCredito(pValor) {
    var listaProblemaCrediticio = pValor.ItemsConProblemasDeCreditos;
    var isFaltantes = false;
    var strHtml = '';
    var cant = 0;
    strHtml += '<div class="col-xs-12">';
    strHtml += '<div class="titleConfirmar">PRODUCTOS CON PROBLEMAS DE CREDITO</div>';
    strHtml += '<table class="footable table carrito table-stripped" width="100%" align="center" cellspacing="0" cellpadding="0" border="0"><thead><tr>';
    strHtml += '<th class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">Producto</th>';
    strHtml += '<th class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">Cantidad</th>';
    strHtml += '</tr></thead></table>';
    strHtml += '<table class="footable table popup table-stripped" data-empty="No hay informacion disponible" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">';
    strHtml += '<tbody>';
    for (var iProblemaCrediticio = 0; iProblemaCrediticio < listaProblemaCrediticio.length; iProblemaCrediticio++) {
        var cantidadProblemasCrediticios = listaProblemaCrediticio[iProblemaCrediticio].Cantidad + listaProblemaCrediticio[iProblemaCrediticio].Faltas;
        if (cantidadProblemasCrediticios > 0) {
            isFaltantes = true;
            var strHtmlColorFondo = 'grs';
            if (cant % 2 != 0) {
                strHtmlColorFondo = 'wht';
            }
            strHtml += '<tr class="' + strHtmlColorFondo + '">';
            strHtml += '<td class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">' + listaProblemaCrediticio[iProblemaCrediticio].NombreObjetoComercial + '</td>';
            strHtml += '<td class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">' + cantidadProblemasCrediticios + '</td>';
            strHtml += '</tr>';
            cant++;
        }
    }
    strHtml += '</tbody></table>';
    // Mensaje de problema crediticio
    strHtml += '<div style="font-size: 14px;font-align:left;  width: 100%;">';
    strHtml += 'Le recordamos que estos productos se encuentran en el RECUPERADOR DE CREDITO para ser reprocesados.';
    strHtml += '</div>';
    // Fin Mensaje de problema crediticio
    strHtml += '</div>';
    if (!isFaltantes)
        return '';
    return strHtml;
}
function CargarRespuestaDePedido(pValor) {
    var strHtmlProductosPedidos = getHtml_ProductosFacturados(pValor);
    var strHtmlFaltantes = getHtml_ProductosEnFalta(pValor);
    var strHtmlProblemasCrediticios = getHtml_ProductosConProblemasDeCredito(pValor);
    var strHtml = '';
    strHtml += '<div class="modal-background">&nbsp;</div>';
    strHtml += '<div class="modal-dialog modal-lg"><div class="modal-content">';
    strHtml += '<div class="modal-header no-padding-bottom">';
    strHtml += '<div class="row">';
    strHtml += '<div class="col-lg-12">';
    if (strHtmlProblemasCrediticios == '' && strHtmlFaltantes == '') {
        strHtml += '<div class="modulo_icon ok"></div>';
    } else {
        strHtml += '<div class="modulo_icon alert"></div>';
    }
    strHtml += '<h4>Resultado del pedido</h4>';
    strHtml += '</div>';
    strHtml += '</div>';
    strHtml += '<div class="close-modal" data-dismiss="modal"><i class="fa fa-times"></i></div>';
    strHtml += '</div>';
    strHtml += '<div class="modal-body">';
    //
    if (strHtmlProductosPedidos != '') {

        strHtml += strHtmlProductosPedidos;
    }
    if (strHtmlProblemasCrediticios == '' && strHtmlFaltantes == '') {
        strHtml += '<div class="col-xs-12"><div class="alert alert-success">';
        strHtml += 'El pedido se realizo correctamente';
        strHtml += '</div></div>';
    }
    if (strHtmlFaltantes != '') {

        strHtml += strHtmlFaltantes;
    }
    if (strHtmlProblemasCrediticios != '') {

        strHtml += strHtmlProblemasCrediticios;
    }
    //
    strHtml += '<div class="clear"></div>';
    strHtml += '<button type="button" class="btn_confirmar" href="#" onclick="onclickBtnConfirmarResultadoPedido(); return false;">CONFIRMAR</button>';
    strHtml += '<div class="clear"></div>';
    strHtml += '</div>';
    strHtml += '<div class="clear"></div>';
    strHtml += '</div></div>';
    $('#modalModulo').html(strHtml);
    $('#modalModulo').modal();
    isHacerBorradoCarritos = true;
    $('#divCargandoContenedorGeneralFondo').css('display', 'none');
}
function onclickBtnConfirmarResultadoPedido() {
    //HacerLimpiezaDeCarritosDspDeConfirmarPedido();
    $('#modalModulo').modal('hide');
}
function HacerLimpiezaDeCarritosDspDeConfirmarPedido() {

    if (isHacerBorradoCarritos) {
        if (indexSucursalTransferSeleccionado == null) {
            var indexCarrito = $("#hiddenIndexCarrito").val();
            if (isNotNullEmpty(indexCarrito)) {
                $('#divContenedorCarrito_' + indexCarrito).remove();
                var sucur = listaCarritos[indexCarrito].codSucursal;
                listaCarritos[indexCarrito].codSucursal = '';
                LimpiarTextBoxProductosBuscados(sucur);
                $("#hiddenIndexCarrito").val('');
                carritoNoHayCarritosCelular();
            }
        } else {
            var sucur = listaCarritoTransferPorSucursal[indexSucursalTransferSeleccionado].Sucursal;
            listaCarritoTransferPorSucursal[indexSucursalTransferSeleccionado].Sucursal = '';
            LimpiarTextBoxProductosBuscados(sucur);
            $('#divContenedorBaseTransfer_' + sucur).html('');
            indexSucursalTransferSeleccionado = null;
            carritoNoHayCarritosCelular();
        }
        isHacerBorradoCarritos = false;
    }
    funActulizarHtmlCredito();
}
function onclickVaciarCarrito_ButtonOK(pIndexCarrito) {
    $('#hiddenIndexCarrito').val(pIndexCarrito);

    if (isCarritoDiferido) {
        BorrarCarritosDiferidos(listaCarritos[pIndexCarrito].lrc_id, listaCarritos[pIndexCarrito].codSucursal);
    } else {
        BorrarCarrito(listaCarritos[pIndexCarrito].lrc_id, listaCarritos[pIndexCarrito].codSucursal);
    }
    return false;
}
function onclickVaciarCarrito(pIndexCarrito) {
    var clickButton = 'return onclickVaciarCarrito_ButtonOK(' + pIndexCarrito + ');';
    mensaje_vaciarCarrito(clickButton);
}
function OnCallBackBorrarCarrito(args) {
    if (args) {
        var indexCarrito = $("#hiddenIndexCarrito").val();
        $('#divContenedorCarrito_' + indexCarrito).remove();
        var sucur = listaCarritos[indexCarrito].codSucursal;
        listaCarritos[indexCarrito].codSucursal = '';
        modalModuloAlertHide();
        LimpiarTextBoxProductosBuscados(sucur);
        carritoNoHayCarritosCelular();
        funActulizarHtmlCredito();
        CargarContenedorBaseTotales_borrar();
    }
}

function CalcularPrecioProductosEnCarrito(pPrecioFinal, pCantidad, pOfertaPorUnidad, pOfertaPorcentaje) {
    var resultado = 0.0;
    if (cli_tomaOfertas()) {
        if (pOfertaPorUnidad == 0 || pOfertaPorcentaje == 0) {
            resultado = parseFloat(pCantidad) * pPrecioFinal;
        } else {
            if (pOfertaPorUnidad > pCantidad) {
                resultado = parseFloat(pCantidad) * pPrecioFinal;
            } else {
                resultado = parseFloat(pCantidad) * (pPrecioFinal * (1 - (parseFloat(pOfertaPorcentaje) / parseFloat(100))));
            }
        }
    } else {
        // Cliente si permiso para tomar oferta
        resultado = parseFloat(pCantidad) * pPrecioFinal;
    }
    return resultado;
}

function setScrollFinDeCarrito(pIndexCarrito) {
    //setTimeout(function () { $('#Scroll_' + pIndexCarrito).scrollTop($('#Scroll_' + pIndexCarrito).prop('scrollHeight')); }, 40);
    setTimeout(function () { $('#div_carrito_cont_' + pIndexCarrito).scrollTop($('#div_carrito_cont_' + pIndexCarrito).prop('scrollHeight')); }, 40);
    setTimeout(function () { ReAjustarColumnasCarritos(listaCarritos[pIndexCarrito].codSucursal, false); }, 40);
}
function onfocusInputCarrito(pValor) {
    DesmarcarFilaSeleccionada();
    selectedInput = null;
    selectedInputTransfer = null;
    selectInputCarrito = pValor;
    nameInput_focus_anterior = document.activeElement.id;
    setTimeout(function () { selectInputCarrito.select(); }, 20);
}
function onblurInputCarrito(pValor) {
    if (isMoverCursor) {
        var nombre = pValor.id;
        nombre = nombre.replace('inputCarrito', '');
        var palabrasBase = nombre.split("_");
        var fila = parseInt(palabrasBase[1]);
        var columna = parseInt(palabrasBase[0]);

        if (pValor.value != '') {

            var cantidadProducto = parseInt(pValor.value);
            var cantidadAnterior_temp = listaCarritos[columna].listaProductos[fila].cantidad;
            if (cantidadProducto != cantidadAnterior_temp) {
                var isNotMaximaCantidadSuperada = true;
                if (listaCarritos[columna].listaProductos[fila].pro_canmaxima != null) {
                    if (listaCarritos[columna].listaProductos[fila].pro_canmaxima < cantidadProducto) {
                        isNotMaximaCantidadSuperada = false;
                    }
                }
                if (isNotMaximaCantidadSuperada) {

                    ExcedeImporteFilaCarrito = fila;
                    ExcedeImporteColumnaCarrito = columna;
                    ExcedeImporteValorCarrito = cantidadProducto;
                    var isCantidadMaximaParametrizada = false;
                    //Inicio Cantidad producto parametrizada
                    if (listaCarritos[columna].listaProductos[fila].cantidad != cantidadProducto) {
                        if (cantidadMaximaParametrizada != null) {
                            if (parseInt(cantidadMaximaParametrizada) > 0) {
                                if (parseInt(cantidadMaximaParametrizada) < cantidadProducto) {
                                    isCantidadMaximaParametrizada = true;
                                    funMostrarMensajeCantidadSuperadaCarrito();
                                }
                            }
                        }
                    }
                    //Fin Cantidad producto parametrizada
                    if (!isCantidadMaximaParametrizada) {
                        cantidadAnterior_carrito = listaCarritos[columna].listaProductos[fila].cantidad;
                        var cantidadFinalCarrito = CargarProductoCantidadDependiendoTransferCarrito(fila, columna, cantidadProducto);
                        if (cantidadFinalCarrito != cantidadProducto) {
                            pValor.value = cantidadFinalCarrito;
                        }
                    }
                } else {
                    //alert(MostrarTextoSuperaCantidadMaxima(listaCarritos[columna].listaProductos[fila].pro_nombre, listaCarritos[columna].listaProductos[fila].pro_canmaxima));
                    mensaje_informacion(MostrarTextoSuperaCantidadMaxima(listaCarritos[columna].listaProductos[fila].pro_nombre, listaCarritos[columna].listaProductos[fila].pro_canmaxima));
                    var cantidadAnterior = listaCarritos[columna].listaProductos[fila].cantidad;
                    if (cantidadAnterior != '') {
                        pValor.value = cantidadAnterior;
                    }

                }
            }
            ///
        } else {
            // Borrar en el carrito o colocarlo en 0     
            var cantidad = ObtenerCantidadProducto(listaCarritos[columna].codSucursal, listaCarritos[columna].listaProductos[fila].codProducto);
            if (cantidad != '') {
                CargarOActualizarListaCarrito(listaCarritos[columna].codSucursal, listaCarritos[columna].listaProductos[fila], 0, true);
                CargarHtmlCantidadDeCarritoABuscador(listaCarritos[columna].codSucursal, listaCarritos[columna].listaProductos[fila].codProducto, 0);
                //AgregarAlHistorialProductoCarrito(fila, columna, 0, true);
                //isModificoBD = true;
            }
        }
    }
}
function funMostrarMensajeCantidadSuperadaCarrito() {
    isMoverCursor = false;
    var htmlMensaje = '';
    htmlMensaje += '<button type="button" id="btn_confirmar_ACEPTAR_CantidadSuperada" onclick="funExcedeImporteAceptarCarrito(); return false;" class="btn_confirmar" href="#">Aceptar</button>';
    htmlMensaje += '<button type="button" onclick="funExcedeImporteCancelarCarrito(); return false;" class="btn_vaciar" href="#">Cancelar</button>';

    mensaje_SuperaCantidad(mensajeCantidadSuperaElMaximoParametrizado1 + cantidadMaximaParametrizada + mensajeCantidadSuperaElMaximoParametrizado2, htmlMensaje);



}
function funExcedeImporteCancelarCarrito() {
    if (ExcedeImporteColumnaCarrito != null && ExcedeImporteFilaCarrito != null && ExcedeImporteValorCarrito != null) {
        //document.getElementById('divContenedorExcedeImporte').style.display = 'none';
        //document.getElementById('divConfirmarExcedeImporteContenedorGeneralCarrito').style.display = 'none';
        $('#modalModulo').modal('hide');
        var objCarrito = $("#inputCarrito" + ExcedeImporteIndiceCarrito + "_" + ExcedeImporteIndiceCarritoProducto);
        if (objCarrito.length > 0) {
        } else {
            objCarrito = null;
        }
        if (objCarrito != null) {

            tempIdSucursal = listaCarritos[ExcedeImporteColumnaCarrito].codSucursal;
            tempIdProduco = listaCarritos[ExcedeImporteColumnaCarrito].listaProductos[ExcedeImporteFilaCarrito].pro_codigo;
            volverCantidadAnterior_carrito(listaCarritos[ExcedeImporteColumnaCarrito].codSucursal, listaCarritos[ExcedeImporteColumnaCarrito].listaProductos[ExcedeImporteFilaCarrito].pro_codigo);
            // CargarProductoCantidadDependiendoTransferCarrito(pFila, pColumna, pCantidad) {
            objCarrito.select();
        }
        isMoverCursor = true;
        ExcedeImporteColumnaCarrito = null;
        ExcedeImporteFilaCarrito = null;
        ExcedeImporteValorCarrito = null;
    }
}
function funExcedeImporteAceptarCarrito() {
    if (ExcedeImporteColumnaCarrito != null && ExcedeImporteFilaCarrito != null && ExcedeImporteValorCarrito != null) {
        var cantidadFinalCarrito = CargarProductoCantidadDependiendoTransferCarrito(ExcedeImporteFilaCarrito, ExcedeImporteColumnaCarrito, ExcedeImporteValorCarrito);
        if (cantidadFinalCarrito != ExcedeImporteValorCarrito) {
            var objCarrito = $("#inputCarrito" + ExcedeImporteIndiceCarrito + "_" + ExcedeImporteIndiceCarritoProducto);
            if (objCarrito.length > 0) {
            } else {
                objCarrito = null;
            }
            if (objCarrito != null) {
                objCarrito.val(cantidadFinalCarrito);
            }
        }
        $('#modalModulo').modal('hide');

        setTimeout('ActualizarFocusCarritoExcedeImporte()', 5);
        isMoverCursor = true;
        ExcedeImporteColumnaCarrito = null;
        ExcedeImporteFilaCarrito = null;
        ExcedeImporteValorCarrito = null;
    }

}
function ActualizarFocusCarritoExcedeImporte() {
    var objCarrito = $("#inputCarrito" + ExcedeImporteSiguienteIndiceCarrito + "_" + ExcedeImporteSiguienteIndiceCarritoProducto);
    if (objCarrito.length > 0) {
    } else {
        objCarrito = null;
    }
    if (objCarrito != null) {
        objCarrito.select();
    }
}
function CargarHtmlCantidadDeCarritoABuscador(pIdSucursal, pIdProduco, pCantidad) {
    if (!(typeof listaProductosBuscados == 'undefined') && isNotNullEmpty(listaProductosBuscados)) {
        for (var i = 0; i < listaProductosBuscados.length; i++) {
            if (listaProductosBuscados[i].pro_codigo == pIdProduco) {
                for (var iEncabezadoSucursal = 0; iEncabezadoSucursal < listaSucursal.length; iEncabezadoSucursal++) {
                    if (listaSucursal[iEncabezadoSucursal] == pIdSucursal) {
                        var mytext = $("#inputSuc" + i + "_" + iEncabezadoSucursal);
                        if (mytext.length <= 0) {
                            mytext = null;
                        }
                        if (mytext != null) {
                            mytext.val(pCantidad);
                        }
                        break;
                    }

                }
                break;
            }
        }
    }
}
function CargarProductoCantidadDependiendoTransferCarrito(pFila, pColumna, pCantidad) {
    var resultadoReturn = pCantidad;
    var isPasarDirectamente = false;
    var cantTransferViejo = ObtenerCantidadProductoTransfer(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila].pro_nombre);
    var cantidadMasCantTransferViejo = cantTransferViejo + pCantidad;
    var cantidadCarritoTransfer = 0;
    var cantidadCarritoComun = 0;

    if (listaCarritos[pColumna].listaProductos[pFila].isProductoFacturacionDirecta) { // facturacion directa
        // Combiene transfer o promocion                      
        var precioConDescuentoDependiendoCantidad = CalcularPrecioProductosEnCarrito(listaCarritos[pColumna].listaProductos[pFila].PrecioFinal, pCantidad, listaCarritos[pColumna].listaProductos[pFila].pro_ofeunidades, listaCarritos[pColumna].listaProductos[pFila].pro_ofeporcentaje);
        if (pCantidad == 0) {
            //
        } else {
            precioConDescuentoDependiendoCantidad = precioConDescuentoDependiendoCantidad / pCantidad;
        }
        if (parseFloat(precioConDescuentoDependiendoCantidad) > parseFloat(listaCarritos[pColumna].listaProductos[pFila].PrecioFinalTransfer)) {
            var isSumarTransfer = false;

            if (listaCarritos[pColumna].listaProductos[pFila].tde_muluni != null && listaCarritos[pColumna].listaProductos[pFila].tde_unidadesbonificadas != null) {
                if ((cantidadMasCantTransferViejo >= listaCarritos[pColumna].listaProductos[pFila].tde_muluni) && (cantidadMasCantTransferViejo <= (listaCarritos[pColumna].listaProductos[pFila].tde_muluni + listaCarritos[pColumna].listaProductos[pFila].tde_unidadesbonificadas))) {
                    // es multiplo
                    isSumarTransfer = true;
                    cantidadCarritoTransfer = listaCarritos[pColumna].listaProductos[pFila].tde_muluni + listaCarritos[pColumna].listaProductos[pFila].tde_unidadesbonificadas;
                } else if (cantidadMasCantTransferViejo > (listaCarritos[pColumna].listaProductos[pFila].tde_muluni + listaCarritos[pColumna].listaProductos[pFila].tde_unidadesbonificadas)) {
                    isSumarTransfer = true;
                    var cantidadMultiplicar = parseInt(cantidadMasCantTransferViejo / listaCarritos[pColumna].listaProductos[pFila].tde_muluni);
                    cantidadCarritoTransfer = cantidadMultiplicar * (listaCarritos[pColumna].listaProductos[pFila].tde_muluni + listaCarritos[pColumna].listaProductos[pFila].tde_unidadesbonificadas);
                    //
                    for (var iCantMulti = 0; iCantMulti < cantidadMultiplicar; iCantMulti++) {
                        var cantTemp = iCantMulti * (listaCarritos[pColumna].listaProductos[pFila].tde_muluni + listaCarritos[pColumna].listaProductos[pFila].tde_unidadesbonificadas);
                        if (cantTemp >= cantidadMasCantTransferViejo) {
                            cantidadCarritoTransfer = cantTemp;
                            break;
                        }
                    }
                    //
                    if (cantidadCarritoTransfer == cantidadMasCantTransferViejo) {

                    } else {
                        if (cantidadMasCantTransferViejo < cantidadCarritoTransfer) {
                            cantidadCarritoComun = 0;
                        } else {
                            cantidadCarritoComun = cantidadMasCantTransferViejo - cantidadCarritoTransfer;
                        }
                        //resultadoReturn = cantidadCarritoComun;
                        if ((cantidadCarritoComun >= listaCarritos[pColumna].listaProductos[pFila].tde_muluni) && (cantidadCarritoComun <= (listaCarritos[pColumna].listaProductos[pFila].tde_muluni + listaCarritos[pColumna].listaProductos[pFila].tde_unidadesbonificadas))) {
                            cantidadCarritoTransfer += listaCarritos[pColumna].listaProductos[pFila].tde_muluni + listaCarritos[pColumna].listaProductos[pFila].tde_unidadesbonificadas;
                            cantidadCarritoComun = 0;
                        }
                    }
                }
                if (isSumarTransfer) {
                    // sumar a transfer
                    if (cantTransferViejo != cantidadCarritoTransfer) {
                        var tempListaProductos = [];
                        var objProducto = new jcTransfersProductos();
                        objProducto.codProductoNombre = listaCarritos[pColumna].listaProductos[pFila].tde_codpro; // Para la funcion en el servidor
                        objProducto.tde_codpro = listaCarritos[pColumna].listaProductos[pFila].tde_codpro;
                        objProducto.cantidad = cantidadCarritoTransfer; //  cantidadCarritoTransfer + cantidadTransfer;
                        objProducto.obj = listaCarritos[pColumna].listaProductos[pFila];
                        tempListaProductos.push(objProducto);
                        AgregarProductosTransfersAlCarrito(tempListaProductos, listaCarritos[pColumna].listaProductos[pFila].tde_codtfr, listaCarritos[pColumna].codSucursal, 'OnCallBackAgregarProductosTransfersAlCarritoDesdeBuscador');
                    }
                    if (cantidadCarritoComun == 0) {
                        var cantidad = ObtenerCantidadProducto(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila].codProducto);
                        if (cantidad != '') {
                            CargarOActualizarListaCarrito(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila], 0, false);
                        }
                    } else {
                        CargarOActualizarListaCarrito(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila], cantidadCarritoComun, false);
                    }
                    CargarHtmlCantidadDeCarritoABuscador(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila].codProducto, parseInt(cantidadCarritoComun) + parseInt(cantidadCarritoTransfer));
                } else {
                    isPasarDirectamente = true;
                }
                /// fin NUEVO facturacion directa
            }
            else if (listaCarritos[pColumna].listaProductos[pFila].tde_fijuni != null) {

                // UNIDAD FIJA
                if (cantidadMasCantTransferViejo == listaCarritos[pColumna].listaProductos[pFila].tde_fijuni) {
                    isSumarTransfer = true;
                    cantidadCarritoTransfer = listaCarritos[pColumna].listaProductos[pFila].tde_fijuni;
                } else if (cantidadMasCantTransferViejo > listaCarritos[pColumna].listaProductos[pFila].tde_fijuni) {
                    isSumarTransfer = true;
                    cantidadCarritoTransfer = listaCarritos[pColumna].listaProductos[pFila].tde_fijuni;
                    cantidadCarritoComun = cantidadMasCantTransferViejo - listaCarritos[pColumna].listaProductos[pFila].tde_fijuni;
                }
                if (isSumarTransfer) {
                    // sumar a transfer
                    var tempListaProductos = [];
                    var objProducto = new jcTransfersProductos();
                    objProducto.codProductoNombre = listaCarritos[pColumna].listaProductos[pFila].tde_codpro; // Para la funcion en el servidor
                    objProducto.tde_codpro = listaCarritos[pColumna].listaProductos[pFila].tde_codpro;
                    objProducto.cantidad = cantidadCarritoTransfer;
                    objProducto.obj = listaCarritos[pColumna].listaProductos[pFila];
                    tempListaProductos.push(objProducto);
                    AgregarProductosTransfersAlCarrito(tempListaProductos, listaCarritos[pColumna].listaProductos[pFila].tde_codtfr, listaCarritos[pColumna].codSucursal, 'OnCallBackAgregarProductosTransfersAlCarritoDesdeBuscador');
                    if (cantidadCarritoComun == 0) {
                        var cantidad = ObtenerCantidadProducto(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila].codProducto);
                        if (cantidad != '') {
                            CargarOActualizarListaCarrito(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila], 0, false);
                        }
                    } else {
                        CargarOActualizarListaCarrito(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila], cantidadCarritoComun, false);
                    }
                    CargarHtmlCantidadDeCarritoABuscador(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila].codProducto, parseInt(cantidadCarritoComun) + parseInt(cantidadCarritoTransfer));
                } else {
                    if (cantTransferViejo != 0) {
                        var tempListaProductos = [];
                        var objProducto = new jcTransfersProductos();
                        objProducto.codProductoNombre = listaCarritos[pColumna].listaProductos[pFila].tde_codpro;
                        objProducto.tde_codpro = listaCarritos[pColumna].listaProductos[pFila].tde_codpro;
                        objProducto.cantidad = 0;
                        objProducto.obj = listaCarritos[pColumna].listaProductos[pFila];
                        tempListaProductos.push(objProducto);
                        AgregarProductosTransfersAlCarrito(tempListaProductos, listaCarritos[pColumna].listaProductos[pFila].tde_codtfr, listaCarritos[pColumna].codSucursal, 'OnCallBackAgregarProductosTransfersAlCarritoDesdeBuscador');
                        $('#divContenedorBaseTransfer_' + listaSucursal[pColumna]).html('');
                    }
                    isPasarDirectamente = true;
                }
                // FIN UNIDAD FIJA

            }
            else if (listaCarritos[pColumna].listaProductos[pFila].tde_minuni != null && listaCarritos[pColumna].listaProductos[pFila].tde_maxuni != null) {
                // UNIDAD MAXIMA Y MINIMA
                if (listaCarritos[pColumna].listaProductos[pFila].tde_minuni <= cantidadMasCantTransferViejo && listaCarritos[pColumna].listaProductos[pFila].tde_maxuni >= cantidadMasCantTransferViejo) {
                    isSumarTransfer = true;
                    cantidadCarritoTransfer = cantidadMasCantTransferViejo;
                } else if (listaCarritos[pColumna].listaProductos[pFila].tde_maxuni < cantidadMasCantTransferViejo) {
                    isSumarTransfer = true;
                    cantidadCarritoTransfer = listaCarritos[pColumna].listaProductos[pFila].tde_maxuni;
                    cantidadCarritoComun = cantidadMasCantTransferViejo - listaCarritos[pColumna].listaProductos[pFila].tde_maxuni;
                }
                if (isSumarTransfer) {
                    // sumar a transfer
                    var tempListaProductos = [];
                    var objProducto = new jcTransfersProductos();
                    objProducto.codProductoNombre = listaCarritos[pColumna].listaProductos[pFila].tde_codpro; // Para la funcion en el servidor
                    objProducto.tde_codpro = listaCarritos[pColumna].listaProductos[pFila].tde_codpro;
                    objProducto.cantidad = cantidadCarritoTransfer;
                    objProducto.obj = listaCarritos[pColumna].listaProductos[pFila];
                    tempListaProductos.push(objProducto);
                    AgregarProductosTransfersAlCarrito(tempListaProductos, listaCarritos[pColumna].listaProductos[pFila].tde_codtfr, listaCarritos[pColumna].codSucursal, 'OnCallBackAgregarProductosTransfersAlCarritoDesdeBuscador');
                    if (cantidadCarritoComun == 0) {
                        var cantidad = ObtenerCantidadProducto(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila].codProducto);
                        if (cantidad != '') {
                            CargarOActualizarListaCarrito(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila], 0, false);
                        }
                    } else {
                        CargarOActualizarListaCarrito(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila], cantidadCarritoComun, false);
                    }
                    CargarHtmlCantidadDeCarritoABuscador(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila].codProducto, parseInt(cantidadCarritoComun) + parseInt(cantidadCarritoTransfer));
                } else {
                    if (cantTransferViejo != 0) {
                        var tempListaProductos = [];
                        var objProducto = new jcTransfersProductos();
                        objProducto.codProductoNombre = listaCarritos[pColumna].listaProductos[pFila].tde_codpro;
                        objProducto.tde_codpro = listaCarritos[pColumna].listaProductos[pFila].tde_codpro;
                        objProducto.cantidad = 0;
                        objProducto.obj = listaCarritos[pColumna].listaProductos[pFila];
                        tempListaProductos.push(objProducto);
                        AgregarProductosTransfersAlCarrito(tempListaProductos, listaCarritos[pColumna].listaProductos[pFila].tde_codtfr, listaCarritos[pColumna].codSucursal, 'OnCallBackAgregarProductosTransfersAlCarritoDesdeBuscador');
                        $('#divContenedorBaseTransfer_' + listaSucursal[pColumna]).html('');
                    }
                    isPasarDirectamente = true;
                }
                // FIN UNIDAD MAXIMA Y MINIMA

            }
            else if (listaCarritos[pColumna].listaProductos[pFila].tde_minuni != null) {

                // UNIDAD MINIMA
                if (listaCarritos[pColumna].listaProductos[pFila].tde_minuni <= cantidadMasCantTransferViejo) {
                    isSumarTransfer = true;
                    cantidadCarritoTransfer = cantidadMasCantTransferViejo;
                }
                if (isSumarTransfer) {
                    // sumar a transfer
                    var tempListaProductos = [];
                    var objProducto = new jcTransfersProductos();
                    objProducto.codProductoNombre = listaCarritos[pColumna].listaProductos[pFila].tde_codpro; // Para la funcion en el servidor
                    objProducto.tde_codpro = listaCarritos[pColumna].listaProductos[pFila].tde_codpro;
                    objProducto.cantidad = cantidadCarritoTransfer;
                    objProducto.obj = listaCarritos[pColumna].listaProductos[pFila];
                    tempListaProductos.push(objProducto);
                    AgregarProductosTransfersAlCarrito(tempListaProductos, listaCarritos[pColumna].listaProductos[pFila].tde_codtfr, listaCarritos[pColumna].codSucursal, 'OnCallBackAgregarProductosTransfersAlCarritoDesdeBuscador');
                    if (cantidadCarritoComun == 0) {
                        var cantidad = ObtenerCantidadProducto(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila].codProducto);
                        if (cantidad != '') {
                            CargarOActualizarListaCarrito(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila], 0, false);
                        }
                    } else {
                        CargarOActualizarListaCarrito(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila], cantidadCarritoComun, false);
                    }
                    CargarHtmlCantidadDeCarritoABuscador(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila].codProducto, parseInt(cantidadCarritoComun) + parseInt(cantidadCarritoTransfer));
                } else {
                    if (cantTransferViejo != 0) {
                        var tempListaProductos = [];
                        var objProducto = new jcTransfersProductos();
                        objProducto.codProductoNombre = listaCarritos[pColumna].listaProductos[pFila].tde_codpro;
                        objProducto.tde_codpro = listaCarritos[pColumna].listaProductos[pFila].tde_codpro;
                        objProducto.cantidad = 0;
                        objProducto.obj = listaCarritos[pColumna].listaProductos[pFila];
                        tempListaProductos.push(objProducto);
                        AgregarProductosTransfersAlCarrito(tempListaProductos, listaCarritos[pColumna].listaProductos[pFila].tde_codtfr, listaCarritos[pColumna].codSucursal, 'OnCallBackAgregarProductosTransfersAlCarritoDesdeBuscador');
                        $('#divContenedorBaseTransfer_' + listaSucursal[pColumna]).html('');
                    }
                    isPasarDirectamente = true;
                }
                // FIN UNIDAD MINIMA
            }

        } // fin if (listaProductosBuscados[pFila].PrecioConDescuentoOferta > listaProductosBuscados[pFila].PrecioFinalTransfer){
        else {
            isPasarDirectamente = true;
        }
    } else {
        isPasarDirectamente = true;
    }
    if (isPasarDirectamente) {
        CargarOActualizarListaCarrito(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila], pCantidad, false);
        CargarHtmlCantidadDeCarritoABuscador(listaCarritos[pColumna].codSucursal, listaCarritos[pColumna].listaProductos[pFila].codProducto, parseInt(pCantidad) + parseInt(cantTransferViejo));
    }
    else {
        resultadoReturn = parseInt(cantidadCarritoComun);
    }
    return resultadoReturn;
}
//////////
function ReAjustarColumnasCarritos(pSucursal, pIsTransfer) {
    var nameTransfer = '';
    if (pIsTransfer)
        nameTransfer = 'Transf';

    var wProducto = $('.thProducto' + nameTransfer + pSucursal).css('width');
    var wCant = $('.thCant' + nameTransfer + pSucursal).css('width');
    var wPrecio = $('.thPrecio' + nameTransfer + pSucursal).css('width');

    $('.tdProducto' + nameTransfer + pSucursal).css('max-width', wProducto);
    $('.tdCant' + nameTransfer + pSucursal).css('max-width', wCant);
    $('.tdPrecio' + nameTransfer + pSucursal).css('max-width', wPrecio);

    $('.tdProducto' + nameTransfer + pSucursal).css('min-width', wProducto);
    $('.tdCant' + nameTransfer + pSucursal).css('min-width', wCant);
    $('.tdPrecio' + nameTransfer + pSucursal).css('min-width', wPrecio);

    $('.tdProducto' + nameTransfer + pSucursal).css('overflow-x', 'auto');
    $('.tdCant' + nameTransfer + pSucursal).css('overflow-x', 'auto');
    $('.tdPrecio' + nameTransfer + pSucursal).css('overflow-xh', 'auto');
}
function obtenerMontoProductoDeCarritoSucursal(pSucursal, pProducto) {
    var result = parseInt(0);
    for (var iCarritos = 0; iCarritos < listaCarritos.length; iCarritos++) {
        if (isNotNullEmpty(listaCarritos[iCarritos].codSucursal)) {
            for (var iProductos = 0; iProductos < listaCarritos[iCarritos].listaProductos.length; iProductos++) {
                if (isNotNullEmpty(listaCarritos[iCarritos].listaProductos[iProductos].cantidad) && listaCarritos[iCarritos].listaProductos[iProductos].pro_codigo == pProducto.pro_codigo) {
                    result = CalcularPrecioProductosEnCarrito(listaCarritos[iCarritos].listaProductos[iProductos].PrecioFinal, listaCarritos[iCarritos].listaProductos[iProductos].cantidad, listaCarritos[iCarritos].listaProductos[iProductos].pro_ofeunidades, listaCarritos[iCarritos].listaProductos[iProductos].pro_ofeporcentaje);
                }
            }
        }//f (isNotNullEmpty(listaCarritos[iCarritos].codSucursal)) {
    }//  for (var iCarritos = 0; iCarritos < listaCarritos.length; iCarritos++) {
    return result;
}

function ConfirmarCarritoTODOS() {
    isBotonNoEstaEnProceso = false;
    var montoMinimo = '';
    var precio = ObtenerPrecioConFormato($('#tdTotal' + pIndexCarrito).html());
    var isTomarPedido = true;
    textTipoEnvioCarrito = $('#comboTipoEnvio option:selected').text();
    var codTipoEnvioCarrito = $('#comboTipoEnvio option:selected').val();
    var isOkCadeteriaRestricciones = true;
    var msgCategoriaRestricciones = '';
    if (cli_codtpoenv() != 'C') { // C -> cadeteria
        if (codTipoEnvioCarrito == 'C') {
            for (var i = 0; i < listaCadeteriaRestricciones.length; i++) {
                if (listaCadeteriaRestricciones[i].tcr_codigoSucursal == listaCarritos[pIndexCarrito].codSucursal) {
                    if (listaCadeteriaRestricciones[i].tcr_MontoIgnorar <= precio) {
                        isOkCadeteriaRestricciones = true;
                    } else {
                        var unidades = obtenerCarritoUnidades(pIndexCarrito);// parseInt($('#tdUnidades' + pIndexCarrito).html().replace(textUnidades, ''));
                        if (listaCadeteriaRestricciones[i].tcr_UnidadesMinimas > unidades) {
                            isOkCadeteriaRestricciones = false;
                            msgCategoriaRestricciones = 'Para seleccionar Cadetería como Tipo de Envío el pedido debe tener ' + listaCadeteriaRestricciones[i].tcr_UnidadesMinimas + ' unidades mínimas y ' + listaCadeteriaRestricciones[i].tcr_UnidadesMaximas + ' unidades máximas'; //'UnidadesMinimas';
                        } else if (listaCadeteriaRestricciones[i].tcr_UnidadesMaximas < unidades) {
                            isOkCadeteriaRestricciones = false;
                            msgCategoriaRestricciones = 'Para seleccionar Cadetería como Tipo de Envío el pedido debe tener ' + listaCadeteriaRestricciones[i].tcr_UnidadesMinimas + ' unidades mínimas y ' + listaCadeteriaRestricciones[i].tcr_UnidadesMaximas + ' unidades máximas'; //'UnidadesMaximas';
                        } else if (listaCadeteriaRestricciones[i].tcr_MontoMinimo > precio) {
                            isOkCadeteriaRestricciones = false;
                            msgCategoriaRestricciones = 'Para seleccionar Cadetería como Tipo de Envío el pedido debe superar los $ ' + listaCadeteriaRestricciones[i].tcr_MontoMinimo;  //'MontoMinimo';
                        }
                    }
                    break;
                }
            }
        }
    }
    if (isOkCadeteriaRestricciones) {
        if (textTipoEnvioCarrito == 'Mostrador') {
            if (listaSucursales != null) {
                for (var i = 0; i < listaSucursales.length; i++) {
                    if (listaSucursales[i].suc_codigo == listaCarritos[pIndexCarrito].codSucursal) {
                        if (listaSucursales[i].suc_montoMinimo > 0) {
                            if (listaSucursales[i].suc_montoMinimo > precio) {
                                isTomarPedido = false;
                                montoMinimo = listaSucursales[i].suc_montoMinimo;
                            }
                        }
                        break;
                    } // fin  if (listaSucursales[i].suc_codigo == listaCarritos[pIndexCarrito].codSucursal) {
                }
            }
        }
        if (isTomarPedido) {
            var textFactura = $('#txtMensajeFactura').val();
            var textRemito = $('#txtMensajeRemito').val();
            var isUrgente = $('#checkboxIsUrgentePedido').is(":checked");
            var idTipoEnvio = $('#comboTipoEnvio').val();
            if (isCarritoDiferido) {
                TomarPedidoCarritoDiferido(listaCarritos[pIndexCarrito].codSucursal, textFactura, textRemito, idTipoEnvio, isUrgente);
            }
            else {
                TomarPedidoCarrito(listaCarritos[pIndexCarrito].codSucursal, textFactura, textRemito, idTipoEnvio, isUrgente);
            }
            //
            $('#modalModulo').modal('hide');
            showCargando();
            //
        } else {
            //alert('Para hacer el pedido se debe superar el monto mínimo de ' + '$ ' + montoMinimo);
            mensaje_informacion('Para hacer el pedido se debe superar el monto mínimo de ' + '$ ' + montoMinimo);
            isBotonNoEstaEnProceso = true;
        }
    } // fin   if (isOkCadeteriaRestricciones) {
    else {
        //alert(msgCategoriaRestricciones);
        mensaje_informacion(msgCategoriaRestricciones);
        isBotonNoEstaEnProceso = true;
    }
}
function MostrarConfirmarCarrito_todos() {
    var onclick = '';
    var codSucursal = '';
    onclick = 'onclickConfimarTransferPedidoOk_todos()';
    //if (pIsTransfer) {
    //    onclick = 'onclickConfimarTransferPedidoOk()';
    //    codSucursal = listaCarritoTransferPorSucursal[pIndexCarrito].Sucursal;
    //}
    //else {
    //    onclick = 'onclickHacerPedido()';
    //    codSucursal = listaCarritos[pIndexCarrito].codSucursal;
    //}
    var strHtml = '';
    strHtml += '<div class="modal-background">&nbsp;</div>';
    strHtml += '<div class="modal-dialog modal-lg"><div class="modal-content">';
    strHtml += '<div class="modal-header no-padding-bottom">';
    strHtml += '<div class="row">';
    strHtml += '<div class="col-lg-12">';
    strHtml += '<div class="modulo_icon check"></div>';
    strHtml += '<h4>Confirmar</h4>';
    strHtml += '</div>';
    strHtml += '</div>';
    strHtml += '<div class="close-modal" data-dismiss="modal"><i class="fa fa-times"></i></div>';
    strHtml += '</div>';
    strHtml += '<div class="modal-body">';
    strHtml += '<div class="col-md-6 div_msj">';
    strHtml += '<textarea class="" id="txtMensajeFactura" name="txtMensajeFactura" placeholder="Mensaje en factura" maxlength="40" onchange="MensajeFacturaRemitoLength(this)" onkeyup="MensajeFacturaRemitoLength(this)" onpaste="MensajeFacturaRemitoLength(this)"></textarea>';
    strHtml += '</div>';
    strHtml += '<div class="col-md-6 div_msj">';
    strHtml += '<textarea class="" id="txtMensajeRemito" name="txtMensajeRemito" placeholder="Mensaje en remito" maxlength="40" onchange="MensajeFacturaRemitoLength(this)" onkeyup="MensajeFacturaRemitoLength(this)" onpaste="MensajeFacturaRemitoLength(this)"></textarea>';
    strHtml += '</div>';
    strHtml += '<div class="col-md-6">Tipo de envio:&nbsp;';
    strHtml += '<select class="form-shop" name="comboTipoEnvio" id="comboTipoEnvio" onchange="onChangeTipoEnvio()" style="width:200px">';
    strHtml += CargarHtmlOptionTipoEnvio_todos();
    strHtml += '</select>';
    strHtml += '</div>';
    //strHtml += '<div id="tdIsUrgente" class="col-md-6">';
    //strHtml += '<input id="checkboxIsUrgentePedido" type="checkbox" style="width: 16px; height: 16px;" />';
    //strHtml += '&nbsp;Es urgente';
    //strHtml += '</div>';
    strHtml += '<div class="clear10"></div>';
    strHtml += '<button type="button" class="btn_confirmar" id="btn_confirmar_HACER_PEDIDO" onclick="' + onclick + '; return false;" href="#">HACER PEDIDO</button>';
    strHtml += '</div>'; // '<div class="modal-body">'
    strHtml += '<div class="clear"></div>';
    strHtml += '</div></div>'; //'<div class="modal-dialog modal-lg"><div class="modal-content">'
    $('#modalModulo').html(strHtml);
    $('#modalModulo').modal();

    $('#btn_confirmar_HACER_PEDIDO').focus();
}
function CargarHtmlOptionTipoEnvio_todos() {
    var strHtml = '';
    if (listaTipoEnviosSucursal != null) {
        //var isSeEncontro = false;
        for (var i = 0; i < listaTipoEnviosSucursal.length; i++) {
            if (listaTipoEnviosSucursal[i].sucursal == cli_codsuc() && listaTipoEnviosSucursal[i].tipoEnvio == null) {
                //isSeEncontro = true;
                for (var y = 0; y < listaTipoEnviosSucursal[i].lista.length; y++) {
                    strHtml += '<option value="' + listaTipoEnviosSucursal[i].lista[y].env_codigo + '">' + listaTipoEnviosSucursal[i].lista[y].env_nombre + '</option>';
                }
                //break;
            }
        }
        //if (!isSeEncontro) {
        //    for (var i = 0; i < listaTipoEnviosSucursal.length; i++) {
        //        if (listaTipoEnviosSucursal[i].sucursal == pSucursal && listaTipoEnviosSucursal[i].tipoEnvio == null) {
        //            for (var y = 0; y < listaTipoEnviosSucursal[i].lista.length; y++) {
        //                strHtml += '<option value="' + listaTipoEnviosSucursal[i].lista[y].env_codigo + '">' + listaTipoEnviosSucursal[i].lista[y].env_nombre + '</option>';
        //            }
        //            break;
        //        }
        //    }
        //}
    }
    return strHtml;
}
function onclickConfimarTransferPedidoOk_todos() {
    isBotonNoEstaEnProceso = false;
    textTipoEnvioCarrito = $('#comboTipoEnvio option:selected').text();
    var codTipoEnvioCarrito = $('#comboTipoEnvio option:selected').val();


    var textFactura = $('#txtMensajeFactura').val();
    var textRemito = $('#txtMensajeRemito').val();
    var idTipoEnvio = $('#comboTipoEnvio').val();

    TomarPedidoCarritoTODOS(textFactura, textRemito, idTipoEnvio);

    //
    $('#modalModulo').modal('hide');
    showCargando();
    //
}
function getNombreSucursal(pIdSucursal) {
    var result = pIdSucursal;
    for (var iNombreSucursal = 0; iNombreSucursal < listaSucursalesDependienteInfo.length; iNombreSucursal++) {
        if (listaSucursalesDependienteInfo[iNombreSucursal].sde_sucursal == pIdSucursal) {
            nombreSucursal = listaSucursalesDependienteInfo[iNombreSucursal].suc_nombre;
            break;
        }
    }
    return result;
}
function getEncabezadoSucursal(pIdSucursal) {
    var result = '';
    result += '<div class="alert alert-primary">';
    result += '<h2>' + getNombreSucursal(pIdSucursal) + '</h2>';
    result += '</div>';
    return result;
}
function OnCallBackTomarPedidoCarritoTODOS(args) {
    isBotonNoEstaEnProceso = true;
    /// mostrar faltantes y problema crediticio
    if (!isNotNullEmpty(args)) {
        args = null;
    } else {
        args = eval('(' + args + ')');
    }
    if (args == null) {
        mensaje_alert_base(mensajeCuandoSeMuestraError, 'volverBuscador()');
    } else {

        isHacerBorradoCarritos = true;
        $('#divCargandoContenedorGeneralFondo').css('display', 'none');
        var strHTML = '';
        var index = 0;
        var max = (args.length + 1) / 3;
        strHTML += '<div class="modal-background">&nbsp;</div>';
        strHTML += '<div class="modal-dialog modal-lg"><div class="modal-content">';

        //strHTML += '<div class="modal-header no-padding-bottom">';
        //strHTML += '<div class="clear"></div>';
        //strHTML += '<a class="btn_vaciar float-left" href="#" data-dismiss="modal">CERRAR</a>';
        //strHTML += '<div class="modulo_icon ok"></div>';
        //strHTML += '<div class="clear"></div>';

        strHTML += '<div class="modal-body">';
        var isMostrarResultado = false;
        for (var i = 0; i < (args.length + 1) / 3; i++) {
            if (i == 0) {
                index = 0;
            } else {
                index = index + 3;
            }
            if (index >= (max + 3)) {
                break;
            }
            var strHTML_sucursal = '';
            var isCarritoSucursal = false;
            //strHTML_sucursal += listaSucursales[index].sde_sucursal + '<br>';
         
            //var nombreSucursal = listaSucursales[index].sde_sucursal;
            //for (var iNombreSucursal = 0; iNombreSucursal < listaSucursalesDependienteInfo.length; iNombreSucursal++) {
            //    if (listaSucursalesDependienteInfo[iNombreSucursal].sde_sucursal == listaSucursales[index].sde_sucursal) {
            //        nombreSucursal = listaSucursalesDependienteInfo[iNombreSucursal].suc_nombre;
            //        break;
            //    }
            //}

           var strHTML_sucursal_encabezado = '';


            var resultCarrrito = args[index + 1];
            if (resultCarrrito != '') {
                resultCarrrito = eval('(' + resultCarrrito + ')');
                //
                if (resultCarrrito == null) {
                    // mensaje_alert_base(mensajeCuandoSeMuestraError, 'volverBuscador()');
                } else {
                    if (strHTML_sucursal_encabezado == '') {
                        strHTML_sucursal_encabezado = getEncabezadoSucursal(resultCarrrito.web_Sucursal);
                    }
                    strHTML_sucursal += '<div class="alert alert-info">';
                    strHTML_sucursal += 'CARRITO';
                    strHTML_sucursal += '</div>';

                    // Error dsd dll pedido
                    if (isNotNullEmpty(resultCarrrito.web_Error)) {
                        strHTML_sucursal += '<b>Carrito Error: </b>' + args.web_Error + '<br>';
                        isCarritoSucursal = true;
                    } else if (isNotNullEmpty(resultCarrrito.Error)) {
                        strHTML_sucursal += '<b>Carrito Error: </b>' + args.Error + '<br>';
                        isCarritoSucursal = true;
                        // mensaje_alert_base(args.Error, 'volverBuscador()');
                        // Fin Error dsd dll pedido
                    } else {
;
                        strHTML_sucursal += CargarRespuestaDePedido_todos(resultCarrrito);
                        isCarritoSucursal = true;
                        creditoInicial = resultCarrrito.CreditoInicial;
                        for (var iCarrito = 0; iCarrito < listaCarritos.length; iCarrito++) {
                            if (listaCarritos[iCarrito].codSucursal == resultCarrrito.web_Sucursal) {
                                $('#divContenedorCarrito_' + iCarrito).remove();
                                //var sucur = listaCarritos[iCarrito].codSucursal;
                                listaCarritos[iCarrito].codSucursal = '';
                                LimpiarTextBoxProductosBuscados(resultCarrrito.web_Sucursal);
                                carritoNoHayCarritosCelular();
                                break;
                            }
                        }
                    }
                    //
                }              
            }
            var resultTransfer = args[index + 2];
            if (resultTransfer != '') {
                resultTransfer = eval('(' + resultTransfer + ')');
                var error_Transfer = '';
                if (resultTransfer.length > 0) {
                    var sucur = resultTransfer[0].web_Sucursal;
                    if (strHTML_sucursal_encabezado == '') {
                        strHTML_sucursal_encabezado = getEncabezadoSucursal(resultTransfer[0].web_Sucursal);
                    }
                    //
                    if (isNotNullEmpty(strHTML_sucursal)) {
                        strHTML_sucursal += '<div class="clear"></div>';
                    }
                    //
                    if (isNotNullEmpty(resultTransfer[0].web_Error)) {
                        error_Transfer = resultTransfer[0].web_Error;
                    } else if (isNotNullEmpty(resultTransfer[0].Error)) {
                        error_Transfer = resultTransfer[0].Error;
                    } else {
                        for (var iCarritoTransfer = 0; iCarritoTransfer < listaCarritoTransferPorSucursal.length; iCarritoTransfer++) {
                            if (listaCarritoTransferPorSucursal[iCarritoTransfer].Sucursal == sucur) {
                                listaCarritoTransferPorSucursal[iCarritoTransfer].Sucursal = '';
                                LimpiarTextBoxProductosBuscados(sucur);
                                $('#divContenedorBaseTransfer_' + sucur).html('');
                                carritoNoHayCarritosCelular();
                                break;
                            }
                        }
                    }
                }
                strHTML_sucursal += '<div class="alert alert-info">';
                strHTML_sucursal += 'CARRITO TRANSFER';
                strHTML_sucursal += '</div>';
                if (isNotNullEmpty(error_Transfer)) {
                    strHTML_sucursal += '<b>Carrito Transfer Error: </b>' + error_Transfer + '<br>';
                } else {


                    strHTML_sucursal += CargarRespuestaDePedidoTransfer_todos(resultTransfer);
                }
                isCarritoSucursal = true;
            }
            if (isCarritoSucursal) {
                strHTML += strHTML_sucursal_encabezado + strHTML_sucursal;
                isMostrarResultado = true;
            };

        }
        strHTML += '<div class="clear"></div>';
        strHTML += '</div>';
        strHTML += '</div></div>';
        if (isMostrarResultado) {
            $('#modalModulo').html(strHTML);
            $('#modalModulo').modal();
        }
    }
}
function CargarRespuestaDePedido_todos(pValor) {
    var strHtmlProductosPedidos = getHtml_ProductosFacturados(pValor);
    var strHtmlFaltantes = getHtml_ProductosEnFalta(pValor);
    var strHtmlProblemasCrediticios = getHtml_ProductosConProblemasDeCredito(pValor);
    var strHtml = '';
    //strHtml += '<div class="modal-background">&nbsp;</div>';
    //strHtml += '<div class="modal-dialog modal-lg"><div class="modal-content">';
    //strHtml += '<div class="modal-header no-padding-bottom">';
    //strHtml += '<div class="row">';
    //strHtml += '<div class="col-lg-12">';
    //if (strHtmlProblemasCrediticios == '' && strHtmlFaltantes == '') {
    //    strHtml += '<div class="modulo_icon ok"></div>';
    //} else {
    //    strHtml += '<div class="modulo_icon alert"></div>';
    //}
    strHtml += '<h4>Resultado del pedido</h4>';
    //strHtml += '</div>';
    //strHtml += '</div>';
    // strHtml += '<div class="close-modal" data-dismiss="modal"><i class="fa fa-times"></i></div>';
    //strHtml += '</div>';
    //  strHtml += '<div class="modal-body">';
    //
    if (strHtmlProductosPedidos != '') {

        strHtml += strHtmlProductosPedidos;
    }
    if (strHtmlProblemasCrediticios == '' && strHtmlFaltantes == '') {
        strHtml += '<div class="col-xs-12"><div class="alert alert-success">';
        strHtml += 'El pedido se realizo correctamente';
        strHtml += '</div></div>';
    }
    if (strHtmlFaltantes != '') {

        strHtml += strHtmlFaltantes;
    }
    if (strHtmlProblemasCrediticios != '') {

        strHtml += strHtmlProblemasCrediticios;
    }
    //
    //strHtml += '<div class="clear"></div>';
    //strHtml += '<button type="button" class="btn_confirmar" href="#" onclick="onclickBtnConfirmarResultadoPedido(); return false;">CONFIRMAR</button>';
    //strHtml += '<div class="clear"></div>';
    //strHtml += '</div>';
    //strHtml += '<div class="clear"></div>';
    //strHtml += '</div></div>';
    //$('#modalModulo').html(strHtml);
    //$('#modalModulo').modal();
    return strHtml;

}
function CargarRespuestaDePedidoTransfer_todos(pValor) {
    var MontoTotal = 0;
    var isProductosTransferPedidos = false;
    var isProductosTransferPedidosFaltantes = false;
    var isProductosTransferPedidosRevision = false;
    var isProductosPedidoFacturarseHabitual = false;
    var strHtmlFaltantes = '';
    var strHtml = '';
    var strHtmlMensajeFinales = '';
    var strHtmlEnRevision = '';
    var strHtmlPedidoFacturarseHabitual = '';
    if (pValor != null) {
        if (pValor.length > 0) {
            strHtmlMensajeFinales += '<div  class="col-xs-12" style="font-size: 12px;">TIPO DE ENVIO: ' + textTipoEnvioCarritoTransfer + ' </div>';
            strHtml += '<div class="col-xs-12">';
            strHtml += '<div class="titleConfirmar">PRODUCTOS FACTURADOS</div>';
            strHtml += '<table class="footable table carrito table-stripped" width="100%" align="center" cellspacing="0" cellpadding="0" border="0"><thead><tr>';
            strHtml += '<th class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">Nombre producto</th>';
            strHtml += '<th class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">Cantidad</th>';
            strHtml += '</tr></thead></table>';
            strHtml += '<table class="footable table popup table-stripped" data-empty="No hay informacion disponible" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">';
            strHtml += '<tbody>';


            // Encabezado PRODUCTOS EN FALTA
            strHtmlFaltantes += '<div class="col-xs-12">';
            strHtmlFaltantes += '<div class="titleConfirmar">PRODUCTOS EN FALTA</div>';
            strHtmlFaltantes += '<table class="footable table carrito table-stripped" width="100%" align="center" cellspacing="0" cellpadding="0" border="0"><thead><tr>';
            strHtmlFaltantes += '<th class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">Nombre producto</th>';
            strHtmlFaltantes += '<th class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">Cantidad</th>';
            strHtmlFaltantes += '</tr></thead></table>';
            strHtmlFaltantes += '<table class="footable table popup table-stripped" data-empty="No hay informacion disponible" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">';
            strHtmlFaltantes += '<tbody>';
            // Fin Encabezado PRODUCTOS EN FALTA

            var cantFaltantes = 0;
            var cantFacturados = 0;
            for (var i = 0; i < pValor.length; i++) {

                // EnRevision
                if (pValor[i].Login == 'REVISION') {
                    //Inicio Mensaje solamente a revicion
                    if (indexSucursalTransferSeleccionado != null) {
                        var sucursal = listaCarritoTransferPorSucursal[indexSucursalTransferSeleccionado].Sucursal;
                        strHtmlMensajeFinales = '';
                        strHtmlMensajeFinales += '<div  class="col-xs-12" style="font-size: 14px;">' + 'Transfers a Revisión. Se ha generado un mail a ';
                        if (sucursal == 'CC') {
                            strHtmlMensajeFinales += 'pedidos@DKintranet.com.ar';
                        } else if (sucursal == 'CH') {
                            strHtmlMensajeFinales += 'sucursalchanarladeado@DKintranet.com.ar';
                        } else if (sucursal == 'SF') {
                            strHtmlMensajeFinales += 'sucursalsantafe@DKintranet.com.ar';
                        } else if (sucursal == 'CB') {
                            strHtmlMensajeFinales += 'sucursalcordoba@DKintranet.com.ar';
                        } else if (sucursal == 'CO') {
                            strHtmlMensajeFinales += 'sucursalconcepcion@DKintranet.com.ar';
                        } else if (sucursal == 'CD') {
                            strHtmlMensajeFinales += 'sucursalconcordia@DKintranet.com.ar';
                        } else if (sucursal == 'VH') {
                            strHtmlMensajeFinales += 'terapiasespeciales@DKintranet.com.ar';
                        }
                        strHtmlMensajeFinales += ' </div>';
                    }
                    //Fin Mensaje solamente a revicion

                    strHtmlEnRevision += '<div class="col-xs-12">';
                    strHtmlEnRevision += '<div class="titleConfirmar">PRODUCTOS PENDIENTES EN FACTURACION </div>';
                    strHtmlEnRevision += '<table class="footable table carrito table-stripped" width="100%" align="center" cellspacing="0" cellpadding="0" border="0"><thead><tr>';
                    strHtmlEnRevision += '<th class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">Nombre producto</th>';
                    strHtmlEnRevision += '<th class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">Cantidad</th>';
                    strHtmlEnRevision += '</tr></thead></table>';
                    strHtmlEnRevision += '<table class="footable table popup table-stripped" data-empty="No hay informacion disponible" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">';
                    strHtmlEnRevision += '<tbody>';

                    for (var x = 0; x < pValor[i].Items.length; x++) {

                        var strHtmlColorFondo = 'grs';
                        if (x % 2 != 0) {
                            strHtmlColorFondo = 'wht';
                        }
                        isProductosTransferPedidosRevision = true;
                        strHtmlEnRevision += '<tr class="' + strHtmlColorFondo + '">';
                        strHtmlEnRevision += '<td class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">' + pValor[i].Items[x].NombreObjetoComercial + '</td>';
                        strHtmlEnRevision += '<td class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">' + pValor[i].Items[x].Cantidad + '</td>';
                        strHtmlEnRevision += '</tr>';
                    }
                    //Fin EnRevision
                } else if (pValor[i].Login == 'CONFIRMACION') {
                    // facturarse de forma Habitual 

                    strHtmlPedidoFacturarseHabitual += '<div class="col-xs-12">';
                    //strHtmlPedidoFacturarseHabitual += '<div class="titleConfirmar">Productos en transfer no procesados por falta de stock para llegar a condición mínima o exceso en el cupo de unidades. Confirme cuantas unidades quiere con su descuento habitual.</div>';
                    strHtmlPedidoFacturarseHabitual += '<div class="titleNoCumplenConCondicion">No cumplen con la condición mínima por falta de stock o exceden la cantidad de unidades semanales autorizadas por el laboratorio.<br>';
                    strHtmlPedidoFacturarseHabitual += '<u><b>Si continúa, las unidades se facturarán con su descuento habitual.</b></u></div>';
                    strHtmlPedidoFacturarseHabitual += '<table class="footable table carrito table-stripped" width="100%" align="center" cellspacing="0" cellpadding="0" border="0"><thead><tr>';
                    strHtmlPedidoFacturarseHabitual += '<th class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">Nombre producto</th>';
                    strHtmlPedidoFacturarseHabitual += '<th class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">Cantidad</th>';
                    strHtmlPedidoFacturarseHabitual += '</tr></thead></table>';
                    strHtmlPedidoFacturarseHabitual += '<table class="footable table popup table-stripped" data-empty="No hay informacion disponible" width="100%" align="center" cellspacing="0" cellpadding="0" border="0">';
                    strHtmlPedidoFacturarseHabitual += '<tbody>';

                    for (var x = 0; x < pValor[i].Items.length; x++) {
                        var strHtmlColorFondo = 'grs';
                        if (x % 2 != 0) {
                            strHtmlColorFondo = 'wht';
                        }
                        isProductosPedidoFacturarseHabitual = true;
                        strHtmlPedidoFacturarseHabitual += '<tr class="' + strHtmlColorFondo + '">';
                        strHtmlPedidoFacturarseHabitual += '<td class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">' + pValor[i].Items[x].NombreObjetoComercial + '</td>';
                        strHtmlPedidoFacturarseHabitual += '<td class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">';
                        var typeInput = ' type="text" ';
                        if (isMobile())
                            typeInput = ' type="number" ';
                        strHtmlPedidoFacturarseHabitual += '<input class="form-shop" id="inputPedidoCant' + i + "_" + x + '" ' + typeInput + '  value="' + pValor[i].Items[x].Cantidad + '" ></input>';
                        strHtmlPedidoFacturarseHabitual += '</td>';
                        strHtmlPedidoFacturarseHabitual += '</tr>';
                    }

                    // fin facturarse de forma Habitual 
                } else {
                    if (isProductosTransferPedidos) {
                        //strHtml += '<tr>';
                        //strHtml += '<td align="left"colspan="2">';
                        //strHtml += '<div style="border-bottom: 1px solid #333333;line-height: 27px;width: 100%;"></div>';
                        //strHtml += '</td>';
                        //strHtml += '</tr>';
                    }
                    for (var x = 0; x < pValor[i].Items.length; x++) {
                        if (pValor[i].Items[x].Cantidad > 0) {

                            var strHtmlColorFondo = 'grs';
                            if (cantFacturados % 2 != 0) {
                                strHtmlColorFondo = 'wht';
                            }
                            cantFacturados++;
                            isProductosTransferPedidos = true;
                            strHtml += '<tr class="' + strHtmlColorFondo + '">';
                            strHtml += '<td class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">' + pValor[i].Items[x].NombreObjetoComercial + '</td>';
                            strHtml += '<td class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">' + pValor[i].Items[x].Cantidad + '</td>';
                            strHtml += '</tr>';
                        }
                        if (pValor[i].Items[x].Faltas > 0) {
                            var strHtmlColorFondo = 'grs';
                            if (cantFaltantes % 2 != 0) {
                                strHtmlColorFondo = 'wht';
                            }
                            cantFaltantes++;
                            isProductosTransferPedidosFaltantes = true;
                            strHtmlFaltantes += '<tr class="' + strHtmlColorFondo + '">';
                            strHtmlFaltantes += '<td class="col-lg-10 col-md-10 col-sm-10 col-xs-9 text-left">' + pValor[i].Items[x].NombreObjetoComercial + '</td>';
                            strHtmlFaltantes += '<td class="col-lg-2 col-md-2 col-sm-10 col-xs-3 text-center">' + pValor[i].Items[x].Faltas + '</td>';
                            strHtmlFaltantes += '</tr>';
                        }
                    }
                    MontoTotal += pValor[i].MontoTotal;
                }
            }

        } // fin  if (pValor.length > 0) 
        else {
            strHtmlMensajeFinales = '<div>' + 'Su pedido ha sido enviado con éxito a la sucursal' + ' </div>';
        }
    } else {  // fin if (pValor != null){
        // Se produjo un error
    }

    if (isProductosTransferPedidos) {
        strHtml += '</tbody></table>';
        strHtml += '<div  class="col-xs-12" style="font-size: 12px;background-color: #E5F3E4;"><b>MONTO TOTAL:</b>  <span style="color:#0B890A;"> $ ' + FormatoDecimalConDivisorMiles(MontoTotal.toFixed(2)) + ' </span>  </div>';
        strHtml += '</div>';
    } else {
        strHtml = '';
    }
    if (isProductosTransferPedidosFaltantes) {
        strHtmlFaltantes += '</tbody></table>';
        strHtmlFaltantes += '</div>';
    } else {
        strHtmlFaltantes = '';
    }
    if (isProductosTransferPedidosRevision) {
        strHtmlEnRevision += '</tbody></table>';
        strHtmlEnRevision += '</div>';
    } else {
        strHtmlEnRevision = '';
    }
    if (isProductosPedidoFacturarseHabitual) {
        strHtmlPedidoFacturarseHabitual += '</tbody></table>';
        strHtmlPedidoFacturarseHabitual += '</div>';


        //strHtmlPedidoFacturarseHabitual += '<button type="button" class="btn_confirmar" onclick="onclickPedidoFacturarseHabitualConfirmar(); return false;" href="#">Confirmar</button>';
        //strHtmlPedidoFacturarseHabitual += '<button type="button" class="btn_confirmar" onclick="CerrarContenedorTransfer(); return false;"  href="#">Descartar</button>';
        strHtmlPedidoFacturarseHabitual += '<div class="clear">';
        strHtmlPedidoFacturarseHabitual += '</div>';
        //document.getElementById('resultadoPedidoBotonOk').style.display = 'none';
    } else {
        strHtmlPedidoFacturarseHabitual = '';
    }


    var strHtml_modal = '';
    //strHtml_modal += '<div class="modal-background">&nbsp;</div>';
    //strHtml_modal += '<div class="modal-dialog modal-lg"><div class="modal-content">';
    //strHtml_modal += '<div class="modal-header no-padding-bottom">';
    //strHtml_modal += '<div class="row">';
    //strHtml_modal += '<div class="col-lg-12">';
    if (strHtml != '' && strHtmlPedidoFacturarseHabitual == '' && strHtmlEnRevision == '' && strHtmlFaltantes == '') {
        //strHtml_modal += '<div class="modulo_icon ok"></div>';
        strHtml_modal += '<h4>Resultado del pedido</h4>';
    }
    else {
        // strHtml_modal += '<div class="modulo_icon alert"></div>';
        strHtml_modal += '<h4>ALGUNOS PRODUCTOS DE TRANSFER NO PUDIERON SER PROCESADOS</h4>';
    }
    //strHtml_modal += '</div>';
    //strHtml_modal += '</div>';
    //strHtml_modal += '<div class="close-modal" data-dismiss="modal"><i class="fa fa-times"></i></div>';
    //strHtml_modal += '</div>';
    strHtml_modal += '<div class="modal-body">';
    strHtml_modal += strHtml + strHtmlFaltantes + strHtmlPedidoFacturarseHabitual + strHtmlEnRevision + strHtmlMensajeFinales;
    //strHtml_modal += '<div class="clear"></div>';
    //if (!isProductosPedidoFacturarseHabitual) {
    //    strHtml_modal += '<button type="button" class="btn_confirmar" href="#" onclick="modalModuloHide(); return false;">CONFIRMAR</button>';
    //}
    //strHtml_modal += '<div class="clear"></div>';
    strHtml_modal += '</div>';
    //strHtml_modal += '<div class="clear"></div>';
    //strHtml_modal += '</div></div>';

    return strHtml_modal;
}