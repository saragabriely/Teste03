using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste03.Models;
using Teste03.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Globalization;

namespace Teste03.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClientePF_Cadastro : ContentPage
	{
        // DataService   dataService;

       // Cliente cliente;

        string url = "https://webapptestem.azurewebsites.net/api/cliente/";

        public ClientePF_Cadastro ()
		{
			InitializeComponent ();
            //dataService = new DataService();
		}

        #region Botões - Navegação
        private async void BtnHome_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.PaginaInicial());
        }

        private async void BtnConhecerApp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.ConhecerApp());
        }

        private async void BtnCadastreSe_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.CadastreSe());
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.Login());
        }
        #endregion

        #region VerificaCampos() - Valida os campos
        private async Task VerificaCamposAsync()
        {
            string nulo         = "Preencha o campo: ";
            string finalizado   = "Cadastro finalizado com sucesso!";
            /*String categoria    = etClienteCategoriaCnh.Items[etClienteCategoriaCnh.SelectedIndex]; 
            String uf           = etClienteUf.Items[etClienteUf.SelectedIndex]; 
            String conta        = etClienteTipoConta.Items[etClienteTipoConta.SelectedIndex];*/
            
            #region Dados Pessoais
            string nome         = etClienteNome.Text;
            string rg           = etClienteRg.Text;
            string cpf          = etClienteCpf.Text;
            string sexo         = etClienteSexo.Items[etClienteSexo.SelectedIndex];
            string dataNascto   = etClienteDataNascto.Text;
            string celular      = etClienteCelular.Text;
            string celular2     = etClienteCelular02.Text;
            int IdTipoUsuario;
            int IdStatus;
            #endregion

            #region Endereço
            string endereco     = etClienteEndereco.Text;
            string numero       = etClienteNumero.Text;
            string complemento  = etClienteComplemento.Text;
            string bairro       = etClienteBairro.Text;
            string cidade       = etClienteCidade.Text;
            string cep          = etClienteCep.Text;
            string uf           = etClienteUf.Text;
            #endregion

            #region Dados bancários   
            string   numeroCartao   = etClienteNumeroCartao.Text;
            int      idBandeira     = 0;
            string   bandeira       = etClienteBandeira.Text;
            string   dataValidade   = etClienteValidadeCartao.Text;
            string   nomeImpresso   = etClienteNomeImpresso.Text;
            int      codSeguranca   = Convert.ToInt32(etClienteCodSeguranca.Text);
            DateTime dataCadastro   = DateTime.Now;
            #endregion 

            #region Email e senha
            string email     = etClienteEmail.Text;
            string confemail = etClienteConfEmail.Text;
            string senha     = etClienteSenha.Text;
            string confsenha = etClienteConfSenha.Text;
            #endregion

            try
            {
                #region Valida - Dados Pessoais
                if (btnDadosPessoais.IsEnabled && etClienteNome.IsVisible)
                {
                    if (string.IsNullOrEmpty(nome))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteNome.Text;
                    }
                    else if (string.IsNullOrEmpty(rg))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteRg.Text;
                    }
                    else if (string.IsNullOrEmpty(cpf))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCpf.Text;
                    }
                    else if (string.IsNullOrEmpty(sexo))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteSexo.Text;
                    }
                    else if (string.IsNullOrEmpty(dataNascto))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteDataNascto.Text;
                    }
                    else if (string.IsNullOrEmpty(celular))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCelular.Text;
                    }
                    else
                    {
                        DadosPessoaisNotVisible();

                        btnEndereco.IsEnabled   = true;

                        btnAvancar.IsVisible    = false;
                        btnAvancar2.IsVisible   = true;
                        btnVoltar.IsVisible     = true;

                        lblAlerta.IsVisible     = false;

                        EnderecoVisible();
                    }
                }
                #endregion

                #region Valida - Endereço
                else if (btnEndereco.IsEnabled && etClienteEndereco.IsVisible)
                {
                    if (string.IsNullOrEmpty(endereco))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteEndereco.Text;
                    }
                    else if (string.IsNullOrEmpty(numero))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteNumero.Text;
                    }
                    else if (string.IsNullOrEmpty(bairro))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteBairro.Text;
                    }
                    else if (string.IsNullOrEmpty(cidade))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCidade.Text;
                    }
                    else if (string.IsNullOrEmpty(cep))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCep.Text;
                    }
                    else if (string.IsNullOrEmpty(uf))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteUf.Text;
                    }
                    else
                    {
                        EnderecoNotVisible();

                        btnDadosBancarios.IsEnabled = true;

                        DadosBancariosVisible();

                        lblAlerta.IsVisible = false;
                    }
                }
                #endregion

                #region Valida - Dados bancários
                else if (btnDadosBancarios.IsEnabled && etClienteNumeroCartao.IsVisible)
                {
                    if (string.IsNullOrEmpty(numeroCartao))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteNumeroCartao.Text;
                    }
                    else if (string.IsNullOrEmpty(dataValidade))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteValidadeCartao.Text;
                    }
                    else if (string.IsNullOrEmpty(codSeguranca.ToString()))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCodSeguranca.Text;
                    }
                    else if (string.IsNullOrEmpty(nomeImpresso))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteNomeImpresso.Text;
                    }
                    else if (string.IsNullOrEmpty(bandeira))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteBandeira.Text;
                    }
                    else
                    {
                        DadosBancariosNotVisible();

                        btnAvancar2.IsVisible = false;
                        btnFinalizar.IsVisible = true;

                        lblAlerta.IsVisible = false;

                        EmailSenhaVisible();
                    }
                }
                #endregion
                
                else if (btnEmailSenha.IsEnabled && etClienteEmail.IsVisible)
                {
                    #region Valida - Email e senha
                    if (string.IsNullOrEmpty(email))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteEmail.Text;
                    }
                    else if (string.IsNullOrEmpty(confemail))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteConfEmail.Text;
                    }
                    else if (string.IsNullOrEmpty(senha))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteSenha.Text;
                    }
                    else if (string.IsNullOrEmpty(confsenha))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteConfSenha.Text;
                    }
                    #endregion

                    else
                    {
                        EmailSenhaNotVisible();

                        //-------------------------------------------------------------------------
                        // Salvando ...

                        //DateTime.TryParseExact(data, "M/d/yyyy hh:mm:ss tt"

                        #region Cliente()
                        Cliente cliente = new Cliente()
                        {
                            IdCliente     = 0,
                            Cnome         = nome,
                            Crg           = rg,
                            Ccpf          = cpf,
                            Csexo         = sexo,
                            CdataNascto   = dataNascto,
                            Ccelular      = celular,
                            Ccelular2     = celular2,
                            Cendereco     = endereco,
                            Cnumero       = numero,
                            Ccomplemento  = complemento,
                            Cbairro       = bairro,
                            Ccidade       = cidade,
                            Ccep          = cep,
                            Cuf           = uf,
                            Cemail        = email,
                            IdTipoUsuario = 4,
                            IdStatus      = 2,
                            Csenha        = senha
                        };
                        #endregion

                         await PostClienteAsync(cliente);
                        
                        #region CartaoCredito()

                        CartaoCredito cartao = new CartaoCredito()
                        {
                            IdCartao            = 0,
                            IdCliente           = 0,
                            Ccpf                = cpf,
                            CnumeroCartao       = numeroCartao,
                            IdBandeira          = idBandeira,
                            CdataValidade       = dataValidade,
                            CcodigoSeg          = codSeguranca,
                            CdataCadastro       = DateTime.Now,
                            IdStatus            = 2,
                            CdataInativacao     = null,
                            CultimaAtualizacao  = null
                        };
                        #endregion
                        
                       // await PostCartaoAsync(cartao);

                        //-------------------------------------------------------------------------

                        lblFinalizado.IsVisible = true;
                        lblFinalizado.Text      = finalizado;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Source != null)

                    await DisplayAlert("Erro", ex.ToString(), "OK");

                    Console.WriteLine("Exception source: {0}", ex.Source);
                throw;
            }
        }
        #endregion

        #region Acesso a banco

        #region Update

        #region Update - Cliente

        public async Task UpdateCliente(Cliente cliente)
        {
            HttpClient client = new HttpClient();

            string url = "https://webapptestem.azurewebsites.net/api/cliente/" + cliente.IdCliente.ToString();

            var uri     = new Uri(string.Format(url, cliente.IdCliente));
            var data    = JsonConvert.SerializeObject(cliente);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PutAsync(uri, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao atualizar o cadastro do cliente");
            }
        }

        #endregion

        #region Update - CartaoCredito

        public async Task UpdateCartao(CartaoCredito cartao)
        {
            HttpClient client = new HttpClient();

            string url = "https://webapptestem.azurewebsites.net/api/cartaocredito/" + cartao.IdCartao.ToString();

            var uri     = new Uri(string.Format(url, cartao.IdCartao));
            var data    = JsonConvert.SerializeObject(cartao);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;

            response = await client.PutAsync(uri, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao atualizar os dados do cartão de crédito");
            }
        }

        #endregion

        #endregion

        #region Insert

        #region Cliente
        public async Task<bool> PostClienteAsync(Cliente cliente)
        {
            HttpClient httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(cliente);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(url, httpContent);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao incluir cliente");
            }
            else
            {
                return result.IsSuccessStatusCode;
            }
        }
        #endregion

        #region CartaoCredito
        public async Task<bool> PostCartaoAsync(CartaoCredito cartao)
        {
            HttpClient httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(cartao);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(url, httpContent);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception("Erro ao incluir dados bancários");
            }
            else
            {
                return result.IsSuccessStatusCode;
            }
        }
        #endregion

        #endregion

        #region Busca

        #region Busca - Cliente - ID
        public async Task<Cliente> GetCliente(int id)
        {
            HttpClient client = new HttpClient();
            try
            {
                string url = "https://webapptestem.azurewebsites.net/api/cliente/" + id.ToString();

                var response = await client.GetStringAsync(url);

                var cliente  = JsonConvert.DeserializeObject<Cliente>(response);

                #region Popula os campos

                #region Dados Pessoais
                etClienteNome.Text          = cliente.Cnome;
                etClienteRg.Text            = cliente.Crg;
                etClienteCpf.Text           = cliente.Ccpf;
                etClienteSexo.Items[etClienteSexo.SelectedIndex] = cliente.Csexo;
                etClienteDataNascto.Text    = cliente.CdataNascto.ToString();
                etClienteCelular.Text       = cliente.Ccelular;
                etClienteCelular02.Text     = cliente.Ccelular2;
                #endregion

                #region Endereço
                etClienteEndereco.Text    = cliente.Cendereco;
                etClienteNumero.Text      = cliente.Cnumero;
                etClienteComplemento.Text = cliente.Ccomplemento;
                etClienteBairro.Text      = cliente.Cbairro;
                etClienteCidade.Text      = cliente.Ccidade;
                etClienteCep.Text         = cliente.Ccep;
                etClienteUf.Text          = cliente.Cuf;
                #endregion

                #region Email e senha
                etClienteEmail.Text     = cliente.Cemail;
                //etClienteConfEmail.Text = cliente.;
                etClienteSenha.Text     = cliente.Csenha;
               // etClienteConfSenha.Text = cliente.Cconfsenha;
                #endregion

                #endregion

                return cliente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Busca - CartaoCredito - ID

        public async Task<CartaoCredito> GetCartao(int id)
        {
            HttpClient client = new HttpClient();
            try
            {
                string url = "https://webapptestem.azurewebsites.net/api/cartaocredito/" + id.ToString();

                var response = await client.GetStringAsync(url);

                var cartao = JsonConvert.DeserializeObject<CartaoCredito>(response);

                #region Popula os campos
                
                etClienteNumeroCartao.Text   = cartao.CnumeroCartao;
                etClienteBandeira.Text       = cartao.BandeiraDescricao;
                etClienteValidadeCartao.Text = cartao.CdataValidade;
                etClienteNomeImpresso.Text   = cartao.NomeImpresso;
                etClienteCodSeguranca.Text   = cartao.CcodigoSeg.ToString();

                #endregion

                return cartao;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #endregion

        #region Deletar

        #region Deletar - Cliente - ID

        public async Task DeleteCliente(int id)
        {
            HttpClient client = new HttpClient();

            string url = "https://webapptestem.azurewebsites.net/api/cliente/" + id.ToString();
            var uri   = new Uri(string.Format(url, id));
            await client.DeleteAsync(uri);
        }

        #endregion

        #region Deletar - CartaoCredito - ID

        public async Task DeleteCartao(int id)
        {
            HttpClient client = new HttpClient();

            string url = "https://webapptestem.azurewebsites.net/api/cartaocredito/" + id.ToString();
            var    uri = new Uri(string.Format(url, id));
            await client.DeleteAsync(uri);
        }

        #endregion

        #endregion

        #endregion

        #region Botões

        #region Botão - Avançar
        private async void BtnAvancar_Clicked(object sender, EventArgs e)
        {
            await VerificaCamposAsync();
        }
        #endregion

        #region Botão - Finalizar
            private void BtnFinalizar_Clicked(object sender, EventArgs e)
            {
                VerificaCamposAsync();
            }
            #endregion

        #region Botão - Voltar
            private void BtnVoltar_Clicked(object sender, EventArgs e)
            {
                if (etClienteEndereco.IsVisible)
                {
                    EnderecoNotVisible();
                    DadosPessoaisVisible();
                    btnAvancar2.IsVisible   = false;
                    btnVoltar.IsVisible     = false;
                    btnAvancar.IsVisible    = true;
                    btnDadosPessoais.IsEnabled = true;
                }
                else if (etClienteNumeroCartao.IsVisible)
                {
                    DadosBancariosNotVisible();
                    EnderecoVisible();

                }
                else if (etClienteEmail.IsVisible)
                {
                    EmailSenhaNotVisible();
                    DadosBancariosVisible();
                    btnFinalizar.IsVisible = false;
                    btnAvancar2.IsVisible = true;
                }
            }
        #endregion

        #endregion

        #region Mostrar e esconder campos

            #region Dados pessoais visiveis
            void DadosPessoaisVisible()
            {
                etClienteNome.IsVisible         = true;
                lblClienteNome.IsVisible        = true;
                etClienteRg.IsVisible           = true;
                lblClienteRg.IsVisible          = true;
                etClienteCpf.IsVisible          = true;
                lblClienteCpf.IsVisible         = true;
                etClienteSexo.IsVisible         = true;
                lblClienteSexo.IsVisible        = true;
                etClienteDataNascto.IsVisible   = true;
                lblClienteDataNascto.IsVisible  = true;
                etClienteCelular.IsVisible      = true;
                lblClienteCelular.IsVisible     = true;
                etClienteCelular02.IsVisible    = true;
                lblClienteCelular02.IsVisible   = true;

                btnDadosPessoais.IsEnabled = false;
            }
            #endregion

            #region Dados pessoais invisíveis
            void DadosPessoaisNotVisible()
            {
                etClienteNome.IsVisible         = false;
                lblClienteNome.IsVisible        = false;
                etClienteRg.IsVisible           = false;
                lblClienteRg.IsVisible          = false;
                etClienteCpf.IsVisible          = false;
                lblClienteCpf.IsVisible         = false;
                etClienteSexo.IsVisible         = false;
                lblClienteSexo.IsVisible        = false;
                etClienteDataNascto.IsVisible   = false;
                lblClienteDataNascto.IsVisible  = false;
                etClienteCelular.IsVisible      = false;
                lblClienteCelular.IsVisible     = false;
                etClienteCelular02.IsVisible    = false;
                lblClienteCelular02.IsVisible   = false;

                btnDadosPessoais.IsEnabled = true;
            }
            #endregion

            #region Endereço visível
            void EnderecoVisible()
            {
                etClienteEndereco.IsVisible    = true;
                lblClienteEndereco.IsVisible   = true;
                etClienteNumero.IsVisible      = true;
                lblClienteNumero.IsVisible     = true;
                etClienteComplemento.IsVisible = true;
                lblClienteCompl.IsVisible      = true;
                etClienteBairro.IsVisible      = true;
                lblClienteBairro.IsVisible     = true;
                etClienteCidade.IsVisible      = true;
                lblClienteCidade.IsVisible     = true;
                etClienteCep.IsVisible         = true;
                lblClienteCep.IsVisible        = true;
                etClienteUf.IsVisible          = true;
                lblClienteUf.IsVisible         = true;

                btnEndereco.IsEnabled = true;
            }
            #endregion

            #region Endereço invisível
            void EnderecoNotVisible()
            {
                etClienteEndereco.IsVisible    = false;
                lblClienteEndereco.IsVisible   = false;
                etClienteNumero.IsVisible      = false;
                lblClienteNumero.IsVisible     = false;
                etClienteComplemento.IsVisible = false;
                lblClienteCompl.IsVisible      = false;
                etClienteBairro.IsVisible      = false;
                lblClienteBairro.IsVisible     = false;
                etClienteCidade.IsVisible      = false;
                lblClienteCidade.IsVisible     = false;
                etClienteCep.IsVisible         = false;
                lblClienteCep.IsVisible        = false;
                etClienteUf.IsVisible          = false;
                lblClienteUf.IsVisible         = false;
            }
            #endregion

            #region Dados bancários visíveis
            void DadosBancariosVisible()
            {
                lblClienteNumeroCartao.IsVisible   = true;
                etClienteNumeroCartao.IsVisible    = true;
                lblClienteValidadeCartao.IsVisible = true;
                etClienteValidadeCartao.IsVisible  = true;
                lblClienteCodSeguranca.IsVisible   = true;
                etClienteCodSeguranca.IsVisible    = true;
                lblClienteNomeImpresso.IsVisible   = true;
                etClienteNomeImpresso.IsVisible    = true;
                lblClienteBandeira.IsVisible       = true;
                etClienteBandeira.IsVisible        = true;

                btnDadosBancarios.IsEnabled      = true;
            }
            #endregion

            #region Dados Bancários invisíveis
            void DadosBancariosNotVisible()
            {
                lblClienteNumeroCartao.IsVisible   = false;
                etClienteNumeroCartao.IsVisible    = false;
                lblClienteValidadeCartao.IsVisible = false;
                etClienteValidadeCartao.IsVisible  = false;
                lblClienteCodSeguranca.IsVisible   = false;
                etClienteCodSeguranca.IsVisible    = false;
                lblClienteNomeImpresso.IsVisible   = false;
                etClienteNomeImpresso.IsVisible    = false;
                lblClienteBandeira.IsVisible       = false;
                etClienteBandeira.IsVisible        = false;
            }
            #endregion

            #region Email e senha visíveis
            void EmailSenhaVisible()
            {
                lblClienteEmail.IsVisible     = true;
                etClienteEmail.IsVisible      = true;
                lblClienteConfEmail.IsVisible = true;
                etClienteConfEmail.IsVisible  = true;
                lblClienteSenha.IsVisible     = true;
                etClienteSenha.IsVisible      = true;
                lblClienteConfSenha.IsVisible = true;
                etClienteConfSenha.IsVisible  = true;

                btnEmailSenha.IsEnabled       = true;
            }
            #endregion

            #region Email e senha invisíveis
            void EmailSenhaNotVisible()
            {
                lblClienteEmail.IsVisible     = false;
                etClienteEmail.IsVisible      = false;
                lblClienteConfEmail.IsVisible = false;
                etClienteConfEmail.IsVisible  = false;
                lblClienteSenha.IsVisible     = false;
                etClienteSenha.IsVisible      = false;
                lblClienteConfSenha.IsVisible = false;
                etClienteConfSenha.IsVisible  = false;
            }
        #endregion

        #endregion

    }
}