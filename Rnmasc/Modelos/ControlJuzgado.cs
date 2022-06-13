using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rnmasc.Modelos
{
    public class ControlJuzgado
    {
        private string centromed;
        private string abrev;
        private string carpetadminmed;
        private string causaexpediente;
        private string distrito;
        private string cveorgano;
        private string fecharadmed;
        private string cveMediador;
        private string paterno;
        private string materno;
        private string nombre;

        public string Centromed { get => centromed; set => centromed = value; }
        public string Abrev { get => abrev; set => abrev = value; }
        public string Carpetadminmed { get => carpetadminmed; set => carpetadminmed = value; }
        public string Causaexpediente { get => causaexpediente; set => causaexpediente = value; }
        public string Distrito { get => distrito; set => distrito = value; }
        public string Cveorgano { get => cveorgano; set => cveorgano = value; }
        public string Fecharadmed { get => fecharadmed; set => fecharadmed = value; }
        public string CveMediador { get => cveMediador; set => cveMediador = value; }
        public string Paterno { get => paterno; set => paterno = value; }
        public string Materno { get => materno; set => materno = value; }
        public string Nombre { get => nombre; set => nombre = value; }
    }
}