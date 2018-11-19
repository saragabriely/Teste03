using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Teste03.Models;

namespace Teste03.Controllers
{
    public class AcompanhaController
    {
        public string url = "https://webapptestem.azurewebsites.net/api/acompanhacoleta/";

        #region Acesso a banco

        #region INSERT
        public async Task<bool> PostAcompanhaAsync(AcompanhaColeta acompanha)
        {
            HttpClient httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(acompanha);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(url, httpContent);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao incluir dados para acompanhar a coleta.");
            }
            else
            {
                return result.IsSuccessStatusCode;
            }
        }
        #endregion
        
        #region GET - IdAcompanha
        public async Task<AcompanhaColeta> GetAcompanha(int idAcompanha)
        {
            HttpClient client = new HttpClient();
            try
            {
                string webService = url + id.ToString();

                var response = await client.GetStringAsync(webService);

                var acompanha = JsonConvert.DeserializeObject<AcompanhaColeta>(response);
                
                return acompanha;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GET - idColeta
        public async Task<AcompanhaColeta> GetAcompanha_Coleta(int idColeta)
        {
            HttpClient client = new HttpClient();
            try
            {
                string webService = url + "coleta/" + idColeta.ToString();

                var response = await client.GetStringAsync(webService);

                var acompanha = JsonConvert.DeserializeObject<AcompanhaColeta>(response);

                return acompanha;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GET - idOrcamento
        public async Task<AcompanhaColeta> GetAcompanha_Orcamento(int idOrcamento)
        {
            HttpClient client = new HttpClient();
            try
            {
                string webService = url + "orcamento/" + idOrcamento.ToString();

                var response = await client.GetStringAsync(webService);

                var acompanha = JsonConvert.DeserializeObject<AcompanhaColeta>(response);

                return acompanha;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UPDATE 
        public async Task UpdateMaterial(AcompanhaColeta acompanha)
        {
            HttpClient client = new HttpClient();

            string webService = url; 

            var uri     = new Uri(string.Format(webService, acompanha));
            var data    = JsonConvert.SerializeObject(acompanha);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PutAsync(uri, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao atualizar os dados.");
            }
        }

        #endregion

        #region DELETE - ID
        public async Task Delete(int id)
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
