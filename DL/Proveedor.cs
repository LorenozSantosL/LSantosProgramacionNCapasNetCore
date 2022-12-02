using System;
using System.Collections.Generic;

namespace DL;

public partial class Proveedor
{
    public int IdProveedor { get; set; }

    public string Telefono { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; } = new List<Producto>();
}
