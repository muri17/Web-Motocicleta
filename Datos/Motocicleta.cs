//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datos
{
    using System;
    using System.Collections.Generic;
    
    public partial class Motocicleta
    {
        public int IdMotocicleta { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string CodigoDeBarra { get; set; }
        public int IdTipoMoto { get; set; }
        public int Stock { get; set; }
        public System.DateTime FechaAlta { get; set; }
        public bool Activo { get; set; }
    
        public virtual TipoMotocicleta TipoMotocicleta { get; set; }
    }
}
