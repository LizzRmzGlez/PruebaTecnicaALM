namespace AlM_Examen.Models
{
    public class ProductosProveedor
    {
        public int IdProductosProveedores { get; set; } = 0;
        public int IdProducto { get; set; } = 0;
        public int IdProveedor { get; set; } = 0;
        public string Clave { get; set; } = string.Empty;
        public decimal Precio { get; set; } = 0;

        public string NombreProv  { get; set; } = string.Empty ;
    }
}
