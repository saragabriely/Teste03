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
    public class ColetaController
    {
        public string url = "https://webapptestem.azurewebsites.net/api/coleta/";

        #region Acesso a banco

        #region GET - idColeta
        public async Task<Coleta> GetColeta(int id)
        {
            HttpClient client = new HttpClient();
            try
            {
                string webService = url + id.ToString();

                var response = await client.GetStringAsync(webService);

                var coleta = JsonConvert.DeserializeObject<Coleta>(response);

                return coleta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Motorista
        
        #region GET - LIST GERAL - OK
        public async Task<List<Coleta>> GetListColeta_Geral(int idMotorista, int idStatus)
        {
            // Retorna todas as coletas

            #region Variáveis e controllers

            HttpClient client = new HttpClient();

            List<Coleta>          _lista            = new List<Coleta>();
            List<Coleta>          _listaFiltrada;
            List<int>             lista             = new List<int>();
            List<int>             teste             = new List<int>();
            List<int>             orca              = new List<int>();
            List<ColetaVisualiza> _listaVisualizada = new List<ColetaVisualiza>();
            List<Orcamento>       _listaOrcamento   = new List<Orcamento>();

            ColetaVisualizaController visualizaController = new ColetaVisualizaController();
            OrcamentoController       orcamentoController = new OrcamentoController();

            #endregion

            try
            {
                string webService = url;

                var response = await client.GetStringAsync(webService);

                var coleta   = JsonConvert.DeserializeObject<List<Coleta>>(response);
                
                #region Verifica as coletas visualizadas pelo motorista

                var listaVisualizada = await visualizaController.GetListVisualiza(idMotorista); // Captura as coletas visualizadas
                
                teste = listaVisualizada.Select(i => i.IdColeta).ToList();      // Captura os IDs das coletas visualizadas pelo motorista
                
                // Captura os IDs das coletas visualizadas pelo motorista
                teste = listaVisualizada.Select(i => i.IdColeta).ToList();

                #endregion

                if (idStatus == 0)                                      // Coletas Não Visualizadas
                {
                    #region Verifica as coletas visualizadas pelo motorista
                    
                    // Filtra as coletas não visualizadas e com status 2 (Disponíveis para envio de orçamento)
                    _listaFiltrada = coleta.Where(l => !teste.Contains(l.IdColeta))
                                           .Where(l => l.IdStatus == 2)
                                           .ToList();
                    #endregion
                    
                    _lista = new List<Coleta>(_listaFiltrada);

                    return _lista;
                }
                else if(idStatus == 1)                                  // Coletas Visualizadas
                {
                    #region Verifica as coletas visualizadas pelo motorista
                    
                    // Filtra as coletas não visualizadas e com status 2 (Disponíveis para envio de orçamento)
                    _listaFiltrada = coleta.Where(l => teste.Contains(l.IdColeta))
                                           .Where(l => l.IdStatus == 2)
                                           .ToList();
                    #endregion
                    
                    _lista = new List<Coleta>(_listaFiltrada);

                    return _lista;
                }
                else if(idStatus == 2)
                {
                    #region Busca os orçamentos

                    idStatus = 13; // IdStatus: 13 - Aguardando aprovação

                    // Captura os orçamentos enviado pelo Motorista
                    var listaOrca = await orcamentoController.GetListOrcamento(idStatus, idMotorista);
                    
                    orca = listaOrca.Select(i => i.IdColeta).ToList();

                    #endregion

                    // Seleciona os IDs das coletas que tem orçamento e posteriormente a coleta
                    _lista = coleta.Where(l => orca.Contains(l.IdColeta)).ToList();

                    return _lista;
                }

                return _lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GET - LIST GERAL - OLD
        /*
        public async Task<List<Coleta>> GetListColeta_Geral()
        {
            // Retorna todas as coletas

            HttpClient client = new HttpClient();

            List<Coleta> _lista;
            List<Coleta> _listaFiltrada;

            try
            {
                string webService = url;

                var response = await client.GetStringAsync(webService);

                var coleta = JsonConvert.DeserializeObject<List<Coleta>>(response);

                _listaFiltrada = coleta.Where(i => i.IdStatus == 2).ToList();

                _lista = new List<Coleta>(_listaFiltrada);

                return _lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } */
        #endregion

        #region Get - Lista - OLD

        /*
         public async Task<List<Coleta>> GetListColeta_Geral()
        {
            // Retorna todas as coletas
             
            HttpClient client = new HttpClient();

            List<Coleta> _lista;
            List<Coleta> _listaFiltrada;

            try
            {
                string webService = url;

                var response = await client.GetStringAsync(webService);

                var coleta   = JsonConvert.DeserializeObject<List<Coleta>>(response);

                _listaFiltrada = coleta.Where(i => i.IdStatus == 2).ToList();

                _lista = new List<Coleta>(_listaFiltrada);

                return _lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
             */

        #endregion

        #region GET - LIST STATUS
        public async Task<List<Coleta>> GetListColeta_Status(int id)
        {
            // Retorna todas as coletas de acordo com o id selecionado

            HttpClient client = new HttpClient();

            List<Coleta> _lista;
            List<Coleta> _listaFiltrada;
            
            try
            {
                _lista = await GetListColeta_Geral(id, 0);
                
                _listaFiltrada = _lista.Where(i => i.IdStatus == 2).ToList();

                return _listaFiltrada;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        #region GET - LIST STATUS
        public async Task<List<Coleta>> GetListColetaMotorista(int idMotorista, int idStatus) // Retorna coletas com o id selecionado
        {
            #region Controlers e variáveis

            ColetaVisualizaController visualizaControl = new ColetaVisualizaController();

            HttpClient client = new HttpClient();

            List<Coleta> _listaFiltrada;

            List<ColetaVisualiza> _listaVisualiza;
            
            #endregion

            try
            {   // Seleciona todas as coletas
                var lista = await GetListColeta_Geral(idMotorista, idStatus);
                
                // Filtro 01 - Filtra as coletas pelo status 02 (disponível para envio de orçamentos)
                _listaFiltrada = lista.Where(i => i.IdStatus == 2).ToList();
                
                if(idStatus == 0)                               // Coletas disponíveis ainda não visualizadas
                {
                    #region Filtra coletas ainda não visualizadas

                    // Filtro 02 - Filtra as coletas relacionadas ao motorista
                    _listaVisualiza = await visualizaControl.GetListVisualiza(idMotorista);

                    // Filtro 03 - Filtra as coletas não visualizadas
                    #region Seleciona as coletas que não forem visualizadas
                    
                    // Seleciona os IDs das coletas visualizadas
                    var listaVisualizaFiltrada = _listaVisualiza
                                                 .Select(i => new { Valor = i.IdColeta }).ToList();

                    // Transforma a lista anonima de status acima, em uma lista de inteiros (para a comparação)
                    // List<int> listStatus = new List<int>(listaVisualizaFiltrada);

                    // Filtro 04 - Filtra as coletas que foram visualizadas
                    // var _listaFiltra = _listaFiltrada.Where(i => !listStatus.Contains(i.IdColeta));

                    //var teste = _listaFiltra.Where(i => listStatus.Contains(i.IdColeta)).ToList();
                    

                    return _listaFiltrada;

                    #endregion

                    #endregion
                }

                return _listaFiltrada;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GET - LIST STATUS - Comentário
        /*
        public async Task<List<Coleta>> GetListColetaMotorista(int idMotorista, int idStatus) // Retorna coletas com o id selecionado
        {
            #region Controlers e variáveis

            ColetaVisualizaController visualizaControl = new ColetaVisualizaController();

            HttpClient client = new HttpClient();

            List<Coleta> _listaFiltrada;
            List<Coleta> _listaFiltrada_;

            List<ColetaVisualiza> _listaVisualiza;

            #endregion

            try
            {   // Seleciona todas as coletas
                var lista = await GetListColeta_Geral();

                // Filtro 01 - Filtra as coletas pelo status 02 (disponível para envio de orçamentos)
                _listaFiltrada = lista.Where(i => i.IdStatus == 2).ToList();

                if (idStatus == 0)                               // Coletas disponíveis ainda não visualizadas
                {
                    #region Filtra coletas ainda não visualizadas

                    // Filtro 02 - Filtra as coletas relacionadas ao motorista
                    _listaVisualiza = await visualizaControl.GetListVisualiza(idMotorista);

                    // Filtro 03 - Filtra as coletas não visualizadas
                    #region Seleciona as coletas que não forem visualizadas

                    // Seleciona os IDs das coletas visualizadas
                    var listaVisualizaFiltrada = _listaVisualiza
                                                 .Select(i => new { Valor = i.IdColeta }).ToList();

                    // Transforma a lista anonima de status acima, em uma lista de inteiros (para a comparação)
                    // List<int> listStatus = new List<int>(listaVisualizaFiltrada);

                    // Filtro 04 - Filtra as coletas que foram visualizadas
                    // var _listaFiltra = _listaFiltrada.Where(i => !listStatus.Contains(i.IdColeta));

                    //var teste = _listaFiltra.Where(i => listStatus.Contains(i.IdColeta)).ToList();


                    return _listaFiltrada;

                    #endregion

                    #endregion
                }

                return _listaFiltrada;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } */
        #endregion

        #region GET - LIST - MOTORISTA
        public async Task<List<Coleta>> GetListColetaMotorista_2(int idStatus)
        {
            HttpClient client = new HttpClient();

            List<Coleta> _lista;

            try
            {
                string webService = url;

                var response = await client.GetStringAsync(webService);

                var coleta = JsonConvert.DeserializeObject<List<Coleta>>(response);

               // if(idStatus == 0)
               // {
                    var enti = coleta.Where(i => i.IdStatus == 2).ToList();     // Status 2 = Aguardado orçamento

                    _lista = new List<Coleta>(enti);

                    return _lista;
                //}               
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion
        
        #endregion

        #region Cliente 

        #region INSERT
        public async Task<bool> PostColetaAsync(Coleta coleta)
        {
            HttpClient httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(coleta);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(url, httpContent);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao incluir dados da coleta.");
            }
            else
            {
                return result.IsSuccessStatusCode;
            }
        }
        #endregion

        #region GET - GetListColeta_Orcamento(int idCliente)  - Coletas que possuem orçamento
        public async Task<List<Coleta>> GetListColeta_Orcamento(int idCliente)
        {
            HttpClient client = new HttpClient(); 
            
            #region Variáveis e controllers

            List<Coleta>    _lista, _listaFiltrada;
            List<Orcamento> _listaOrca;

            Orcamento           orcamento           = new Orcamento();

            OrcamentoController orcamentoController = new OrcamentoController();

            #endregion

            try
            {
                string webService = url;

                var response = await client.GetStringAsync(webService);

                var coleta = JsonConvert.DeserializeObject<List<Coleta>>(response);

                // Retorna todos os orçamentos relacionados ao cliente
                _listaOrca = await orcamentoController.GetListOrcamento_Cliente(idCliente);

                // Retorna os IDs das coletas
                var retornaId = _listaOrca.Select(i => i.IdColeta).Distinct().ToList();
                
                _listaFiltrada = coleta.Where(l => retornaId.Contains(l.IdColeta)).ToList();
                
              //  _lista = new List<Coleta>(enti);

                return _listaFiltrada;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        #region GET - GetListColeta(int id)
        public async Task<List<Coleta>> GetListColeta(int id)
        {
            HttpClient client = new HttpClient();

            List<Coleta> _lista;

            try
            {
                string webService = url;

                var response = await client.GetStringAsync(webService);

                var coleta   = JsonConvert.DeserializeObject<List<Coleta>>(response);

                var enti     = coleta.Where(i => i.IdCliente == id).ToList();

                var veic     = 
                    enti.Select(i => new { Texto = string.Format("{0} - {1} - {2}", i.IdColeta, i.ApelidoColeta, i.IdStatus),
                    Valor = i.IdColeta }).ToList();
                
                _lista = new List<Coleta>(enti);

                return _lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GET - LIST - CLIENTE - ESPECIFICO
        public async Task<List<Coleta>> GetListColetaEsp(int idCliente, int idStatus)
        {
            HttpClient client = new HttpClient();

            List<Coleta> _lista, _listaA;

            try
            {
                string webService = url;

                var response = await client.GetStringAsync(webService);

                var coleta = JsonConvert.DeserializeObject<List<Coleta>>(response);

                // Seleciona todas as coletas relacionadas a este cliente
                var enti = coleta.Where(i => i.IdCliente == idCliente).ToList();

                var veic =  enti
                           // .Where(i => i.IdStatus == idStatus)
                            .Select(i => new {
                                Texto = string.Format("{0} - {1} - {2}", i.IdColeta, i.ApelidoColeta, i.IdStatus),
                                Valor = i.IdColeta
                            }).ToList();

                _lista = new List<Coleta>(enti);

                var teste = _lista.Where(i => i.IdCliente == idCliente);

                var lista = teste.Where(i => i.IdStatus == idStatus);

                _listaA = new List<Coleta>(lista);

                return _listaA;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        #region UPDATE 
        public async Task UpdateColeta(Coleta coleta)
        {
            HttpClient client = new HttpClient();

            string webService = url;

            var uri     = new Uri(string.Format(webService, coleta));
            var data    = JsonConvert.SerializeObject(coleta);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PutAsync(uri, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao atualizar os dados da coleta.");
            }
        }

        #endregion

        #region DELETE - ID
        public async Task DeleteColeta(int id)
        {
            HttpClient client = new HttpClient();

            string webService = url + id.ToString();

            var uri = new Uri(string.Format(webService, id));

            await client.DeleteAsync(uri);
        }
        #endregion

        #endregion

        #endregion

    }
}
