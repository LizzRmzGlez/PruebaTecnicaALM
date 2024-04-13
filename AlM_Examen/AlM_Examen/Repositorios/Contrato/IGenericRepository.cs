using AlM_Examen.Models;

namespace AlM_Examen.Repositorios.Contrato
{
    public interface IGenericRepository<T> where T : class 
    {
        Task<List<T>> Lista();
        Task<List<T>> Lista(int id);
        Task<bool> Guardar(T modelo);
        Task<bool> Editar(T modelo);
        Task<bool> Eliminar(int id);
        Task<List<T>> Buscar(T modelo);
        Task<List<Proveedor>> ListaProveedor();
        Task<List<TipoProducto>> ListaTipoProducto();
  }
}
