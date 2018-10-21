using System;
using System.Collections.Generic;
using System.Text;

namespace Teste03.Models
{
    public class Cliente
    {
        public int    IdCliente     { get; set; }

        public string Cnome         { get; set; }
        public string Crg           { get; set; }
        public string Ccpf          { get; set; }
        public string Csexo         { get; set; }
        public string CdataNascto { get; set; }
        public string Ccelular      { get; set; }
        public string Ccelular2     { get; set; }
        public string Cendereco     { get; set; }
        public string Cnumero       { get; set; }
        public string Ccomplemento  { get; set; }
        public string Cbairro       { get; set; }
        public string Ccidade       { get; set; }
        public string Ccep          { get; set; }
        public string Cuf           { get; set; }
        public string Cemail        { get; set; }
        public Nullable<int> IdTipoUsuario { get; set; }
        public Nullable<int> IdStatus      { get; set; }
        public string Csenha        { get; set; }


    }
}
