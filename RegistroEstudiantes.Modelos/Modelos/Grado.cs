using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroEstudiantes.Modelos.Modelos
{
    public class Grado
    {
        public string? Nombre { get; set; }

        public override string ToString()
        {
            return Nombre ?? string.Empty;
        }
    }
}
