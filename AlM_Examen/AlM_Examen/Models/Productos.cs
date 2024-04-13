namespace AlM_Examen.Models
{
    public class Productos
    {
        public int IdProducto { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public decimal Precio { get; set; } = 0;
        public int EsActivo { get; set; } = 0;
        public int IdTipoProducto { get; set; } = 0;
    }
}
