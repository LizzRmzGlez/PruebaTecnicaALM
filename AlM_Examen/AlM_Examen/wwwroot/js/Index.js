
const _modeloProducto = {
    idProducto:0,
    nombre: "",
    clave: "",
    precio: 0,
    idTipoProducto: 0,
    esActivo: 0
}

function MostrarProductos() {
    fetch("/Home/listaProductos")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            $("#tablaProductos tbody").html("");
            if (responseJson.length > 0) {
                responseJson.forEach((producto) => {
                    $("#tablaProductos tbody").append(
                        $("<tr>").append(
                            $("<td>").text(producto.nombre),
                            $("<td>").text(producto.clave),
                            $("<td>").text(producto.precio),
                            $("<td>").append(
                                $("<button>").addClass("btn btn-primary btn-sm boton-editar-producto").text("Editar").data("dataProducto", producto),
                                $("<button>").addClass("btn btn-danger btn-sm ms-2 boton-eliminar-producto").text("Eliminar").data("dataProducto", producto)
                            )
                        )
                    )
                })
            }
        })
}


document.addEventListener("DOMContentLoaded", function () {
    MostrarProductos();
    //MostrarProveedores();
    fetch("/Home/ListaTipoProducto")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboTipoProducto").append(
                        $("<option>").val(item.idTipoProducto).text(item.nombre)
                    )
                })
            }
        })
    fetch("/Home/ListaProveedores")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            if (responseJson.length > 0) {
                responseJson.forEach((item) => {
                    $("#cboProveedor").append(
                        $("<option>").val(item.idProveedor).text(item.nombre)
                    )
                })
            }
        })

}, false)


function MostrarModal()
{
    $("#txtNombre").val(_modeloProducto.nombre);
    $("#txtClave").val(_modeloProducto.clave);
    $("#txtPrecio").val(_modeloProducto.precio);
    $("#cboTipoProducto").val(_modeloProducto.idTipoProducto == 0 ? $("#cboTipoProducto option:first").val() : _modeloProducto.idTipoProducto);
   


    $("#ModalProductos").modal("show");

    if (_modeloProducto.idProducto == 0) {
        $("#tablaProveedores").hide();
        $(".boton-agregar-proveedor").hide()
    } else {
        MostrarProveedores();
        $("#tablaProveedores").show();
        $(".boton-agregar-proveedor").show()
    }
}

$(document).on("click", ".boton-nuevo-producto", function () {
    _modeloProducto.idProducto = 0;
    _modeloProducto.nombre = "";
    _modeloProducto.clave = "";
    _modeloProducto.precio = 0;
    _modeloProducto.idTipoProducto = 0;
    _modeloProducto.esActivo = 0;

    MostrarModal();
})

$(document).on("click", ".boton-editar-producto", function () {

    const _producto = $(this).data("dataProducto");

    _modeloProducto.idProducto = _producto.idProducto;
    _modeloProducto.nombre = _producto.nombre;
    _modeloProducto.clave = _producto.clave;
    _modeloProducto.precio = _producto.precio;
    _modeloProducto.idTipoProducto = _producto.idTipoProducto;
    _modeloProducto.esActivo = _producto.esActivo;

    MostrarModal();
})

$(document).on("click", ".boton-guardar-cambios-producto", function () {

    const modelo = {
        idProducto: _modeloProducto.idProducto,
        nombre: $("#txtNombre").val(),
        clave: $("#txtClave").val(),
        precio: $("#txtPrecio").val(),
        idTipoProducto: $("#cboTipoProducto").val(), 
        esActivo: document.getElementById("checkActivo").checked ? 1 : 0
    }
    if (_modeloProducto.idProducto == 0) {
        fetch("/Home/GuardarProducto", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {
                if (responseJson.valor) {
                    $("#ModalProductos").modal("hide");
                    Swal.fire("Listo!", "Producto agregado", "success");
                    MostrarProductos();
                }
                else
                    Swal.fire("Lo siento", "No se pudo agregar", "error");
            })
    } else {
        fetch("/Home/EditarProducto", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {
                if (responseJson.valor) {
                    $("#ModalProductos").modal("hide");
                    Swal.fire("Listo!", "Producto actualizado", "success");
                    MostrarProductos();
                }
                else
                    Swal.fire("Lo siento", "No se pudo actualizar", "error");
            })
    }
})

function search() {
    let Producto = document.getElementById("txtBuscaNombre").value.toUpperCase();
    let Clave = document.getElementById("txtBuscaClave").value.toUpperCase();
    let table = document.getElementById("tablaProductos");

    let tr = table.rows;
    for (let i = 1; i < tr.length; i++) {
        td = tr[i].cells;
        tdProd = td[0].innerText;
        tdClave = td[1].innerText;
        if (tdProd.toUpperCase().indexOf(Producto) > -1 && tdClave.toUpperCase().indexOf(Clave) > -1) {
            tr[i].style.display = "";
        } else
            tr[i].style.display = "none";
    }
}

$(document).on("click", ".boton-eliminar-producto", function () {
    const _producto = $(this).data("dataProducto");

    Swal.fire({
        title: "Estas seguro?",
        text: `Al eliminar el producto "${_producto.nombre} " se eliminaran todas la referencias de sus proveedores, Deseas continuar?`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, eliminar",
        cancelButtonText: "No, volver"
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`/Home/EliminarProducto?idProducto=${_producto.idProducto}`, {
                method: "DELETE"
            })
                .then(response => {
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then(responseJson => {
                    if (responseJson.valor) {
                        Swal.fire("Listo!", "El producto fue eliminado", "success");
                        MostrarProductos();
                    }
                    else
                        Swal.fire("Lo siento!", "El producto no se pudo eliminar", "error");
                })
        }
    })
})
