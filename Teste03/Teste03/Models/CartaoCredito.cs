using System;
using System.Collections.Generic;
using System.Text;

namespace Teste03.Models
{
    public class CartaoCredito
    {
        public int      IdCartao            { get; set; }

        public Nullable<int> IdCliente      { get; set; }
        public string   Ccpf                { get; set; }
        public string   CnumeroCartao       { get; set; }
        public int      IdBandeira          { get; set; }
        public string   CdataValidade       { get; set; }
        public int      CcodigoSeg          { get; set; }
        public DateTime CdataCadastro       { get; set; }
        public int      IdStatus            { get; set; }
        public string   BandeiraDescricao   { get; set; }
        public string   NomeImpresso        { get; set; }

        public Nullable<DateTime> CdataInativacao    { get; set; }

        public Nullable<DateTime> CultimaAtualizacao { get; set; }

        public CartaoCredito() { }

        public CartaoCredito(int id)
        {
            this.IdCartao = id;
        }

        public CartaoCredito(string cpf)
        {
            this.Ccpf = cpf;
        }
    }
}
