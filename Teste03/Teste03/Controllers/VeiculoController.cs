using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Teste03.Models;

namespace Teste03.Controllers
{
    public class VeiculoController
    {
        public string url = "https://webapptestem.azurewebsites.net/api/veiculo/";

        public List<Veiculo> _listaVeiculos;

        #region Acesso a banco

        #region GET - ALL
        /*
        public async Task<List<Veiculo>> GetAllVeiculo()
        {
            HttpClient client = new HttpClient();
            try
            {
                
                string webService = url;// + id.ToString();

                var response = await client.GetStringAsync(webService);

                if(response.StatusCode == HttpStatusCode.NotFound)
                {

                }

                var veiculo = JsonConvert.DeserializeObject<Veiculo>(response);

                return veiculo; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } */
        #endregion
            
        #region INSERT 
        public async Task<bool> PostVeiculoAsync(Veiculo veiculo)
        {
            HttpClient httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(veiculo);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(url, httpContent);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao cadastrar veiculo.");
            }
            else
            {
                return result.IsSuccessStatusCode;
            }
        }
        #endregion

        #region GET - LIST
        public async Task<List<Veiculo>> GetListVeiculo(int id)
        {
            HttpClient client = new HttpClient();
            
            List<Veiculo> _lista;
            List<Veiculo> _listaA;

            try
            {   
                string webService = url;

                var response = await client.GetStringAsync(webService);
                
                var veiculo = JsonConvert.DeserializeObject<List<Veiculo>>(response);
                
                var enti = veiculo.Where(i => i.IdMotorista == id).ToList();

                var veic = enti.Select(i => new { Texto = string.Format("{0} - {1}", i.Placa, i.Modelo), Valor = i.IdVeiculo }).ToList();

                //_lista = new List<Veiculo>(veiculo);

                _lista = new List<Veiculo>(enti);
                
                var teste = _lista.Where(i => i.IdMotorista == id);

                _listaA = new List<Veiculo>(teste);
                
                return _listaA;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        #region GET - ID
        public async Task<Veiculo> GetConta(int id)
        {
            HttpClient client = new HttpClient();
            try
            {
                string webService = url + id.ToString();

                var response = await client.GetStringAsync(webService);

                var veiculo = JsonConvert.DeserializeObject<Veiculo>(response);
                
                return veiculo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UPDATE
        public async Task UpdateVeiculo(Veiculo veiculo, int idVeiculo)
        {
            HttpClient client = new HttpClient();

            string webService = url;// + idVeiculo.ToString();

            veiculo.IdVeiculo = idVeiculo;

            var uri     = new Uri(string.Format(webService, veiculo));
            var data    = JsonConvert.SerializeObject(veiculo);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PutAsync(uri, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao atualizar os dados do veículo.");
            }
          //  else
          //  {
               // return response.IsSuccessStatusCode;
          //  }
        }

        #endregion

        #region DELETE - ID
        public async Task DeleteVeiculo(int id)
        {
            HttpClient client = new HttpClient();

            string webService = url + id.ToString();
            var uri = new Uri(string.Format(webService, id));

            await client.DeleteAsync(uri);
            
        }
        #endregion

        #endregion

    }
}
