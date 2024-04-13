const _modeloProductoProveedor = {
    IdProductosProveedores:0,
    idProducto: 0,
    idProveedor: 0,
    clave: "",
    precio: 0
}

function MostrarModalProveedor() {
    $("#txtClaveP").val(_modeloProductoProveedor.clave);
    $("#txtPrecioP").val(_modeloProductoProveedor.precio);
    $("#cboProveedor").val(_modeloProductoProveedor.idProveedor == 0 ? $("#cboProveedor option:first").val() : _modeloProductoProveedor.idProveedor);

    $("#ModalProveedores").modal("show");
}

function MostrarProveedores() {
    //fetch("/Home/ListaProveedores")
    fetch(`/Home/ListaProductoProveedor?id=${_modeloProducto.idProducto}`)
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            $("#tablaProveedores tbody").html("");
            if (responseJson.length > 0) {

                responseJson.forEach((proveedor) => {
                    $("#tablaProveedores tbody").append(
                        $("<tr>").append(
                            $("<td>").text(proveedor.nombreProv),
                            $("<td>").text(proveedor.clave),
                            $("<td>").text(proveedor.precio),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-primary btn-sm boton-editar-proveedor").text("Editar").data("dataProveedor", proveedor),
                                $("<button>").addClass("btn btn-danger btn-sm ms-2 boton-eliminar-proveedor").text("Eliminar").data("dataProveedor", proveedor)
                            )
                        )
                    )
                })
            }
        });
}

$(document).on("click", ".boton-agregar-proveedor", function () {
    _modeloProductoProveedor.IdProductosProveedores = 0,
    _modeloProductoProveedor.idProducto = _modeloProducto.idProducto,
    _modeloProductoProveedor.idProveedor= 0,
    _modeloProductoProveedor.clave= "",
    _modeloProductoProveedor.precio = 0

    //$("#ModalProductos").modal("hide");
    MostrarModalProveedor();
})

$(document).on("click", ".boton-editar-proveedor", function () {

    const _productoProveedor = $(this).data("dataProveedor");

    _modeloProductoProveedor.IdProductosProveedores = _productoProveedor.idProductosProveedores,
    _modeloProductoProveedor.idProducto = _productoProveedor.idProducto,
    _modeloProductoProveedor.idProveedor = _productoProveedor.idProveedor,
    _modeloProductoProveedor.clave = _productoProveedor.clave,
    _modeloProductoProveedor.precio = _productoProveedor.precio

    MostrarModalProveedor();
})

$(document).on("click", ".boton-eliminar-proveedor", function () {
    const _productoProveedor = $(this).data("dataProveedor");

    Swal.fire({
        title: "Estas seguro?",
        text: `Eliminar proveedor "${_productoProveedor.nombreProv} "`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, eliminar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Home/EliminarProductoProveedor?id=${_productoProveedor.idProductosProveedores}`, {
                method: "DELETE"
            })
                .then(response => {
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then(responseJson => {
                    if (responseJson.valor) {
                        Swal.fire("Listo!", "El regitro fue eliminado", "success");
                        MostrarProveedores();
                    }
                    else
                        Swal.fire("Lo siento!", "El regitro no se pudo eliminar", "error");
                })
        }
    })
})

$(document).on("click", ".boton-guardar-cambios-proveedor", function () {

    const modelo = {
        IdProductosProveedores: _modeloProductoProveedor.IdProductosProveedores,
        idProducto: _modeloProductoProveedor.idProducto,
        idProveedor: $("#cboProveedor").val(),
        clave: $("#txtClaveP").val(),
        precio: $("#txtPrecioP").val(),
    }
    if (_modeloProductoProveedor.IdProductosProveedores == 0) {
        fetch("/Home/GuardarProductoProveedor", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {
                if (responseJson.valor) {
                    $("#ModalProveedores").modal("hide");
                    Swal.fire("Listo!", "Registro agregado", "success");
                    MostrarProveedores();
                }
                else
                    Swal.fire("Lo siento", "No se pudo agregar", "error");
            })
    } else {
        fetch("/Home/EditarProductoProveedor", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {
                if (responseJson.valor) {
                    $("#ModalProveedores").modal("hide");
                    Swal.fire("Listo!", "Registro actualizado", "success");
                    MostrarProveedores();
                }
                else
                    Swal.fire("Lo siento", "No se pudo actualizar", "error");
            })
    }
})