using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Teste03.Models;

namespace Teste03.Controllers
{
    public class LoginController
    {
        readonly string url = "https://webapptestem.azurewebsites.net/api/login/";
        
        #region Acesso a banco

        #region Permite acesso
        public async Task<LoginModel> GetLogin(string email, string senha)
        {
            HttpClient client = new HttpClient();
            LoginModel login;
            ClienteController clienteControl = new ClienteController();
            Cliente cliente = new Cliente();

            try
            {
                string webService = url + "loga/?email=" + email.ToString() + "&senha=" + senha.ToString();

                var response = await client.GetStringAsync(url);

                var loga = JsonConvert.DeserializeObject<LoginModel>(response);

                if (loga == null)
                {
                    return null;
                }
                else
                {
                    cliente = await clienteControl.GetCpf(loga.Ccpf);

                    Session.Instance.IdCliente     = cliente.IdCliente;
                    Session.Instance.IdTipoUsuario = cliente.IdTipoUsuario;
                    Session.Instance.Email         = cliente.Cemail;
                    Session.Instance.Senha         = cliente.Csenha;
                    Session.Instance.Cnome         = cliente.Cnome;
                    Session.Instance.Crg           = cliente.Crg;
                    Session.Instance.Ccpf          = cliente.Ccpf;
                    Session.Instance.Csexo         = cliente.Csexo;
                    Session.Instance.CdataNascto   = cliente.CdataNascto;
                    Session.Instance.Ccelular      = cliente.Ccelular;
                    Session.Instance.Ccelular2     = cliente.Ccelular2;
                    Session.Instance.Cendereco     = cliente.Cendereco;
                    Session.Instance.Cnumero       = cliente.Cnumero;
                    Session.Instance.Ccomplemento  = cliente.Ccomplemento;
                    Session.Instance.Cbairro       = cliente.Cbairro;
                    Session.Instance.Ccidade       = cliente.Ccidade;
                    Session.Instance.Ccep          = cliente.Ccep;
                    Session.Instance.Cuf           = cliente.Cuf;
                    Session.Instance.IdStatus      = cliente.IdStatus;

                    return loga;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GET - Cliente - CPF
        public async Task<LoginModel> GetCpf(string cpf)
        {
            HttpClient client = new HttpClient();
            LoginModel loginModel;

            try
            {
                string webService = url + "cpf/?cpf=" + cpf.ToString();

                var response = await client.GetStringAsync(webService);

                var loga = JsonConvert.DeserializeObject<LoginModel>(response);
                
                
                return loga;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region UPDATE - Login
        public async Task UpdateLogin(LoginModel loga)
        {
            HttpClient client = new HttpClient();

            string webService = url + loga.IdLogin.ToString();

            LoginModel atualiza = new LoginModel()
            {

            };


            var uri     = new Uri(string.Format(webService, loga.IdLogin));
            var data    = JsonConvert.SerializeObject(loga);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PutAsync(uri, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao atualizar os dados do login");
            }
        }
        #endregion
        
        #endregion
        
    }
}
