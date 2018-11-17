using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Teste03.Models;

namespace Teste03.Controllers
{
    public class OrcamentoController
    {
        public string url = "https://webapptestem.azurewebsites.net/api/orcamento/";
       
        #region Acesso a banco
                    
        #region INSERT - OK
        public async Task<bool> PostOrcamebtoAsync(Orcamento orcamento)
        {
            HttpClient httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(orcamento);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(url, httpContent);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao cadastrar orçamento!");
            }
            else
            {
                return result.IsSuccessStatusCode;
            }
        }
        #endregion
        
        #region GET - GetOrcamentos(int id)
        public async Task<Orcamento> GetOrcamentos(int id)
        {
            HttpClient client = new HttpClient();
            try
            {
                string webService = url + id.ToString();

                var response = await client.GetStringAsync(webService);

                var orcamento = JsonConvert.DeserializeObject<Orcamento>(response);

                return orcamento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GET - Lista - Cliente (IdCliente)
        public async Task<List<Orcamento>> GetListOrcamento_Cliente(int idCliente)
        {
            HttpClient client = new HttpClient();

            List<Orcamento> _lista;
            
            try
            {
                string webService = url;

                var response = await client.GetStringAsync(webService);

                var orcamento = JsonConvert.DeserializeObject<List<Orcamento>>(response);

                // Seleciona os orçamentos do cliente em questão
                var enti = orcamento.Where(i => i.IdCliente == idCliente).ToList();

                _lista = new List<Orcamento>(enti);

                return _lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GET - Lista - Cliente (IdColeta)
        public async Task<List<Orcamento>> GetListOrcamento_Cliente_(int idColeta)
        {
            HttpClient client = new HttpClient();

            List<Orcamento> _lista;

            try
            {
                string webService = url;

                var response = await client.GetStringAsync(webService);

                var orcamento = JsonConvert.DeserializeObject<List<Orcamento>>(response);

                // Seleciona os orçamentos do cliente em questão
                var enti = orcamento.Where(i => i.IdColeta == idColeta).ToList();

                _lista = new List<Orcamento>(enti);

                return _lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GET - Lista - Cliente (idColeta, idOrcaAceito, idStatus)

        public async Task GetRecusaOrcamentos(int idColeta, int idOrcaAceito, int idStatus)
        {
            // Recebe o ID da coleta em questão, do orçamento aceito e do status que os demais orçamentos receberão
            
            List<Orcamento> _lista = new List<Orcamento>();

            List<int> _listaId = new List<int>();

            try
            {
                // Recebe a lista de orçamentos referentes a coleta em questão
                _lista = await GetListOrcamento_Cliente_(idColeta);
                
                // Seleciona apenas o ID dos orçamentos da consulta acima
                var pendentes = _lista.Select(l => l.IdOrcamento).ToList();

                // Seleciona os orçamentos descartando o que foi aceito
                var recusar = _lista.Where(l => l.IdOrcamento != idOrcaAceito).ToList();

                // Seleciona orçamento por orçamento, altera o idStatus e atualiza (update)
                for(int i = 0; i < recusar.Count; i++)
                {
                    var orcam = recusar.Where(l => l.IdStatus == 13).First();

                    orcam.IdStatus = 14;

                    await UpdateOrcamento(orcam, orcam.IdOrcamento);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        

        #region GET - GetListOrcamento_Geral - Motorista
        public async Task<List<Orcamento>> GetListOrcamento_Geral(int idMotorista)
        {
            HttpClient client = new HttpClient();

            List<Orcamento> _lista;

            try
            {
                string webService = url;

                var response = await client.GetStringAsync(webService);

                var orcamento = JsonConvert.DeserializeObject<List<Orcamento>>(response);

                // Seleciona os orçamentos do motorista em questão
                var enti = orcamento.Where(i => i.IdMotorista == idMotorista).ToList();

                _lista = new List<Orcamento>(enti);

                return _lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GET - GetListOrcamento(int idStatus, int idMotorista)
        public async Task<List<Orcamento>> GetListOrcamento(int idStatus, int idMotorista)
        {
            HttpClient client = new HttpClient();
            
            List<Orcamento> _lista;

            try
            {   
                string webService = url;

                var response  = await client.GetStringAsync(webService);
                
                var orcamento = JsonConvert.DeserializeObject<List<Orcamento>>(response);
                
                var enti      = orcamento.Where(i => i.IdStatus    == idStatus)
                                         .Where(i => i.IdMotorista == idMotorista)
                                         .ToList();
                
                _lista = new List<Orcamento>(enti);
                
                return _lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region GET - GetOrcamento(int id)
        public async Task<Orcamento> GetOrcamento(int id)
        {
            HttpClient client = new HttpClient();
            try
            {
                string webService = url + id.ToString();

                var response      = await client.GetStringAsync(webService);

                var orcamento     = JsonConvert.DeserializeObject<Orcamento>(response);
                
                return orcamento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GET - IdMotorista + IdColeta
        public async Task<Orcamento> GetOrcamento(int idMotorista, int idColeta)
        {
            HttpClient client = new HttpClient();
            try
            {
                string link = idMotorista.ToString() + "/" + idColeta.ToString();

                // Busca todos os orçamentos
                string webService = url + link;

                var response = await client.GetStringAsync(webService);

                var orcamento = JsonConvert.DeserializeObject<Orcamento>(response);
                
                return orcamento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UPDATE
        public async Task UpdateOrcamento(Orcamento orcamento, int idOrcamento)
        {
            HttpClient client = new HttpClient();

            string webService = url;// + idVeiculo.ToString();

            orcamento.IdOrcamento = idOrcamento;

            var uri     = new Uri(string.Format(webService, orcamento));
            var data    = JsonConvert.SerializeObject(orcamento);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PutAsync(uri, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao atualizar os dados do orçamento.");
            }
        }

        #endregion

        #region DELETE - ID
        public async Task DeleteOrcamento(int id)
        {
            HttpClient client = new HttpClient();

            string webService = url + id.ToString();
            var    uri        = new Uri(string.Format(webService, id));

            await client.DeleteAsync(uri);
        }
        #endregion

        #endregion
        
    }
}
