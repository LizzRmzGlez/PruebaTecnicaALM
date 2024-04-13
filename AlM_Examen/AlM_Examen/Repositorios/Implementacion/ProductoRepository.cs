using AlM_Examen.Models;
using AlM_Examen.Repositorios.Contrato;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace AlM_Examen.Repositorios.Implementacion
{
    public class ProductoRepository : IGenericRepository<Productos>
    {
        private readonly string _cadenaSQL = "";

        public ProductoRepository(IConfiguration configuration)
        {
            _cadenaSQL = configuration.GetConnectionString("cadenaSQL");

        }

        public async Task<List<Productos>> Lista()
        {
            List<Productos> _lista = new List<Productos>();
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SP_ObtenerProductos", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        _lista.Add(new Productos
                        {
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            Nombre = dr["Nombre"].ToString(),
                            Clave = dr["Clave"].ToString(),
                            Precio = Convert.ToInt32(dr["Precio"])
                        });
                    }
                }
            }
            return _lista;
        }

        public async Task<bool> Editar(Productos modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SP_ModificarProducto", conexion);
                cmd.Parameters.AddWithValue("@IdProducto", modelo.IdProducto);
                cmd.Parameters.AddWithValue("@Nombre", modelo.Nombre);
                cmd.Parameters.AddWithValue("@Clave", modelo.Clave);
                cmd.Parameters.AddWithValue("@Precio", modelo.Precio);
                cmd.Parameters.AddWithValue("@IdTipoProducto", modelo.IdTipoProducto);
                cmd.Parameters.AddWithValue("@EsActivo", modelo.EsActivo);
                cmd.CommandType = CommandType.StoredProcedure;

                int filas_afectadas = await cmd.ExecuteNonQueryAsync();
                if (filas_afectadas > 0)
                    return true;
                else
                    return false;
            }
        }
        public async Task<bool> Guardar(Productos modelo)
        {
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SP_AgregarProducto", conexion);
                cmd.Parameters.AddWithValue("@Nombre", modelo.Nombre);
                cmd.Parameters.AddWithValue("@Clave", modelo.Clave);
                cmd.Parameters.AddWithValue("@Precio", modelo.Precio);
                cmd.Parameters.AddWithValue("@EsActivo", modelo.EsActivo);
                cmd.Parameters.AddWithValue("@IdTipoProducto", modelo.IdTipoProducto);
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
                SqlCommand cmd = new SqlCommand("SP_EliminarProducto", conexion);
                cmd.Parameters.AddWithValue("@IdProducto", id);
                cmd.CommandType = CommandType.StoredProcedure;

                int filas_afectadas = await cmd.ExecuteNonQueryAsync();
                if (filas_afectadas > 0)
                    return true;
                else
                    return false;
            }
        }

        public async Task<List<Productos>> Buscar(Productos modelo)
        {
            List<Productos> _lista = new List<Productos>();
            using (SqlConnection conexion = new(_cadenaSQL))
            {
                using (SqlCommand cmd = new("SP_BuscarProducto", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Clave", modelo.Clave);
                    cmd.Parameters.AddWithValue("@IdTipoProducto", modelo.IdTipoProducto);
                    conexion.Open();

                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            Productos producto = new Productos
                            {
                                IdProducto = Convert.ToInt32(dr["IdProducto"]),
                                Nombre = dr["Nombre"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Precio = Convert.ToInt32(dr["Precio"]),
                                EsActivo = Convert.ToInt32(dr["IdProducto"]),
                                IdTipoProducto = Convert.ToInt32(dr["IdProducto"])
                            };
                            _lista.Add(producto);
                        }
                        return _lista;
                    }
                }
            }
        }

        public async Task<List<Proveedor>> ListaProveedor()
        {
            List<Proveedor> _lista = new List<Proveedor>();
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SP_ObtenerProveedores", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        _lista.Add(new Proveedor
                        {
                            IdProveedor = Convert.ToInt32(dr["IdProveedor"]),
                            Nombre = dr["Nombre"].ToString(),
                            Descripcion = dr["Descripcion"].ToString()
                        });
                    }
                }
            }
            return _lista;
        }

        public async Task<List<TipoProducto>> ListaTipoProducto()
        {
            List<TipoProducto> _lista = new List<TipoProducto>();
            using (var conexion = new SqlConnection(_cadenaSQL))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SP_ObtenerTipoProducto", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        _lista.Add(new TipoProducto
                        {
                            IdTipoProducto = Convert.ToInt32(dr["IdTipoProducto"]),
                            Nombre = dr["Nombre"].ToString(),
                            Descripcion = dr["Descripcion"].ToString()
                        });
                    }
                }
            }
            return _lista;
        }

    public Task<List<Productos>> Lista(int id)
    {
      throw new NotImplementedException();
    }
  }
}


