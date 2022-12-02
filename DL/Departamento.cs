using System;
using System.Collections.Generic;

namespace DL;

public partial class Departamento
{
    public int IdDepartamento { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdArea { get; set; }

    public virtual Area IdAreaNavigation { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; } = new List<Producto>();

    //agregados departamento

    public string AreaNombre { get; set; }
}
