using AlM_Examen.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AlM_Examen.Repositorios.Contrato;
using AlM_Examen.Repositorios.Implementacion;

namespace AlM_Examen.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericRepository<Productos> _productosRepository;
        private readonly IGenericRepository<ProductosProveedor> _productosProveedorRepository;


        public HomeController(ILogger<HomeController> logger,
            IGenericRepository<Productos> productosRepository,
            IGenericRepository<ProductosProveedor> productosProveedorRepository)
        {
            _logger = logger;
            _productosRepository = productosRepository;
            _productosProveedorRepository = productosProveedorRepository;

        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ListaProductos()
        {
            List<Productos> _lista = await _productosRepository.Lista();
            return StatusCode(StatusCodes.Status200OK, _lista);
        }

        [HttpGet]
        public async Task<IActionResult> ListaProveedores()
        {
            List<Proveedor> _lista = await _productosRepository.ListaProveedor();
            return StatusCode(StatusCodes.Status200OK, _lista);
        }

        [HttpGet]
        public async Task<IActionResult> ListaTipoProducto()
        {
            List<TipoProducto> _lista = await _productosRepository.ListaTipoProducto();
            return StatusCode(StatusCodes.Status200OK, _lista);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarProducto([FromBody] Productos modelo)
        {
            bool _resultado = await _productosRepository.Guardar(modelo);
            if (_resultado)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado, msg = "ok" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado, msg = "error" });
        }

        [HttpPut]
        public async Task<IActionResult> EditarProducto([FromBody] Productos modelo)
        {
            bool _resultado = await _productosRepository.Editar(modelo);
            if (_resultado)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado, msg = "ok" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado, msg = "error" });
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarProducto(int idProducto)
        {
            bool _resultado = await _productosRepository.Eliminar(idProducto);
            if (_resultado)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado, msg = "ok" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado, msg = "error" });
        }

        [HttpGet]
        public async Task<IActionResult> ListaProductoProveedor(int id)
        {
            List<ProductosProveedor> _lista = await _productosProveedorRepository.Lista(id);
            return StatusCode(StatusCodes.Status200OK, _lista);
        }
    
        [HttpPost]
        public async Task<IActionResult> GuardarProductoProveedor([FromBody] ProductosProveedor modelo)
        {
            bool _resultado = await _productosProveedorRepository.Guardar(modelo);
            if (_resultado)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado, msg = "ok" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado, msg = "error" });
        }
        
        [HttpPut]
        public async Task<IActionResult> EditarProductoProveedor([FromBody] ProductosProveedor modelo)
        {
            bool _resultado = await _productosProveedorRepository.Editar(modelo);
            if (_resultado)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado, msg = "ok" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado, msg = "error" });
        }

        [HttpDelete]
        public async Task<IActionResult> EliminarProductoProveedor(int id)
        {
            bool _resultado = await _productosProveedorRepository.Eliminar(id);
            if (_resultado)
                return StatusCode(StatusCodes.Status200OK, new { valor = _resultado, msg = "ok" });
            else
                return StatusCode(StatusCodes.Status500InternalServerError, new { valor = _resultado, msg = "error" });
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }

}
