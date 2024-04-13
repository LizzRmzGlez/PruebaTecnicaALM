using AlM_Examen.Models;
using AlM_Examen.Repositorios.Contrato;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace AlM_Examen.Repositorios.Implementacion
{
  public class ProductoProveedorRepository : IGenericRepository<ProductosProveedor>
  {

    private readonly string _cadenaSQL = "";

    public ProductoProveedorRepository(IConfiguration configuration)
    {
      _cadenaSQL = configuration.GetConnectionString("cadenaSQL");

    }
    public Task<List<ProductosProveedor>> Buscar(ProductosProveedor modelo)
    {
      throw new NotImplementedException();
    }

    public async Task<bool> Editar(ProductosProveedor modelo)
    {
      using (var conexion = new SqlConnection(_cadenaSQL))
      {
        conexion.Open();
        SqlCommand cmd = new SqlCommand("SP_ModificarProductoProveedor", conexion);
        cmd.Parameters.AddWithValue("@IdProductosProveedores", modelo.IdProductosProveedores);
        cmd.Parameters.AddWithValue("@IdProducto", modelo.IdProducto);
        cmd.Parameters.AddWithValue("@IdProveedor", modelo.IdProveedor);
        cmd.Parameters.AddWithValue("@Clave", modelo.Clave);
        cmd.Parameters.AddWithValue("@Precio", modelo.Precio);
        cmd.CommandType = CommandType.StoredProcedure;

        int filas_afectadas = await cmd.ExecuteNonQueryAsync();
        if (filas_afectadas > 0)
          return true;
        else
          return false;
      }
    }

    public async Task<bool> Eliminar(int id)
    {
      using (var conexion = new SqlConnection(_cadenaSQL))
      {
        conexion.Open();
        SqlCommand cmd = new SqlCommand("SP_EliminarProductosProveedores", conexion);
        cmd.Parameters.AddWithValue("@IdProductosProveedores", id);
        cmd.CommandType = CommandType.StoredProcedure;

        int filas_afectadas = await cmd.ExecuteNonQueryAsync();
        if (filas_afectadas > 0)
          return true;
        else
          return false;
      }
    }

    public async Task<bool> Guardar(ProductosProveedor modelo)
    {
      using (var conexion = new SqlConnection(_cadenaSQL))
      {
        conexion.Open();
        SqlCommand cmd = new SqlCommand("SP_AgregarProductoProveedor", conexion);
        cmd.Parameters.AddWithValue("@IdProducto", modelo.IdProducto);
        cmd.Parameters.AddWithValue("@IdProveedor", modelo.IdProveedor);
        cmd.Parameters.AddWithValue("@Clave", modelo.Clave);
        cmd.Parameters.AddWithValue("@Precio", modelo.Precio);
        cmd.CommandType = CommandType.StoredProcedure;

        int filas_afectadas = await cmd.ExecuteNonQueryAsync();
        if (filas_afectadas > 0)
          return true;
        else
          return false;
      }
    }

    public async Task<List<ProductosProveedor>> Lista(int id)
    {
      List<ProductosProveedor> _lista = new List<ProductosProveedor>();
      using (var conexion = new SqlConnection(_cadenaSQL))
      {
        conexion.Open();
        SqlCommand cmd = new SqlCommand("SP_ObtenerProductoProveedor", conexion);
        cmd.Parameters.AddWithValue("@IdProducto", id);
        cmd.CommandType = CommandType.StoredProcedure;

        using (var dr = await cmd.ExecuteReaderAsync())
        {
          while (await dr.ReadAsync())
          {
            _lista.Add(new ProductosProveedor
            {
              IdProductosProveedores = Convert.ToInt32(dr["IdProductosProveedores"]),
              IdProducto = Convert.ToInt32(dr["IdProducto"]),
              IdProveedor = Convert.ToInt32(dr["IdProveedor"]),
              Clave = dr["Clave"].ToString(),
              Precio = Convert.ToInt32(dr["Precio"]),
              NombreProv = dr["Nombre"].ToString()
            });
          }
        }
      }
      return _lista;
    }

    public Task<List<ProductosProveedor>> Lista()
    {
      throw new NotImplementedException();
    }

    public Task<List<Proveedor>> ListaProveedor()
    {
      throw new NotImplementedException();
    }

    public Task<List<TipoProducto>> ListaTipoProducto()
    {
      throw new NotImplementedException();
    }
  }
}
