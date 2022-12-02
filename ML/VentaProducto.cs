using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ML
{
    public class VentaProducto
    {
        public int IdVenta { get; set; }
        public int Cantidad { get; set; }
        public ML.Producto Producto { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public List<object> VentaProductos { get; set; }
    }
}
