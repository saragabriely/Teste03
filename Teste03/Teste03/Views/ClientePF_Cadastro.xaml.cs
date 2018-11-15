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
using Teste03.Controllers;
using Teste03.ClassesComuns;

namespace Teste03.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClientePF_Cadastro : ContentPage
	{
        // public string url = "https://webapptestem.azurewebsites.net/api/cliente/";

        #region Variáveis / Controllers

        public int verificaOperacao;
        public int idCli;

        ClienteController control       = new ClienteController();
        CartaoController  controlCartao = new CartaoController();

        // Session.Instance.IdCliente = id;

        #endregion

        public ClientePF_Cadastro ()
		{
            InitializeComponent();

            #region Verifica se o usuário está logado

            #region Não logado
            if (Session.Instance.IdCliente == 0)
            {
                // Botões - Menu superior
                stCadastrarSe.IsVisible = true;
                stMeuCadastro.IsVisible = false;
                grdNaoLogado.IsVisible  = true;
            }
            #endregion

            #region Logado
            else
            {
                MeuCadastroAsync();
            
                 // Mostrando o menu correspondente (logado)
                 grdNaoLogado.IsVisible = false;
                 grdLogado.IsVisible    = true;
            
                 // Ícones que permitem atualizar ou excluir cadastro
                 slEditarVeiculos.IsVisible  = true;
                 // slExcluirVeiculos.IsVisible = true;
            
                 verificaOperacao = 2;
            
                 DadosNotEnabled();
            }
            #endregion

            #endregion

        }

        #region Logado

        #region Botões superiores

        #region Btn - Meu Cadastro - Dados Pessoais

        public void BtnDadosPessoais(object sender, EventArgs e)
        {
            // Mostra os dados pessoais
            DadosPessoaisVisible();

            // Esconde os demais campos
            EnderecoNotVisible();
            EmailSenhaNotVisible();
        }

        #endregion

        #region Btn - Meu Cadastro - Endereço

        public void BtnEndereco(object sender, EventArgs e)
        {
            // Mostra os dados do endereço
            EnderecoVisible();

            // Esconde os demais campos
            DadosPessoaisNotVisible();
            EmailSenhaNotVisible();
        }

        #endregion

        #region Btn - Meu Cadastro - Email e senha

        public void BtnEmailSenha(object sender, EventArgs e)
        {
            // Mostra os dados pessoas
            EmailSenhaVisible();

            // Esconde os demais campos
            DadosPessoaisNotVisible();
            EnderecoNotVisible();
        }

        #endregion

        #endregion

        #region Btn - CRUD

        #region Btn - Editar 
        private void BtnEditarCadastro_Clicked(object sender, EventArgs e)
        {
           //  Libera os campos para serem alterados
            DadosEnabled();
            
            #region Botões - Mostrar e esconder
            
            slEditarVeiculos.IsVisible  = false;        // EDITAR
            slExcluirVeiculos.IsVisible = false;        // EXCLUIR

            if (etClienteNome.IsVisible)
            {
                btnAvancar.IsVisible = true;               // AVANÇAR
            }
            else if (etClienteEndereco.IsVisible)
            {
                btnAvancar2.IsVisible = true;             // AVANÇAR
                stBtnVoltar.IsVisible = true;
            }
            else if (etClienteEmail.IsVisible)
            {
                btnSalvar.IsVisible = true;             // SALVAR
            }
                         
            stBtnVoltar.IsVisible  = false;              // VOLTAR
            stBtnAvancar.IsVisible = false;              // AVANÇAR
            
            verificaOperacao = 2;

            #endregion
        }
        #endregion

        #region Btn - Excluir cadastro
        private async void BtnExcluirCadastro_Clicked(object sender, EventArgs e)
        {
            
            if (await DisplayAlert("Excluir", "Deseja mesmo excluir o seu cadastro?", "OK", "Cancelar"))
            {
                //await veiculoController.DeleteVeiculo(idVeiculo);

                await DisplayAlert("Excluído", "Cadastro excluído com sucesso!", "OK");

                await Navigation.PushModalAsync(new Views.Login());
            } 
        }
        #endregion

        #endregion
        
        #region MeuCadastroAsync()

        private void MeuCadastroAsync()
        {
            #region Populando 

            // Session.Instance.IdCliente = cliente.IdCliente;
            // Session.Instance.IdTipoUsuario = cliente.IdTipoUsuario;

             etClienteNome.Text        = Session.Instance.Cnome;
             etClienteRg.Text          = Session.Instance.Crg;
             etClienteCpf.Text         = Session.Instance.Ccpf;
             //etClienteSexo.Text      
             etClienteDataNascto.Text  = Session.Instance.CdataNascto;
             etClienteCelular.Text     = Session.Instance.Ccelular;
             etClienteCelular02.Text   = Session.Instance.Ccelular2;
                                       
             etClienteEndereco.Text    = Session.Instance.Cendereco;
             etClienteNumero.Text      = Session.Instance.Cnumero;
             etClienteComplemento.Text = Session.Instance.Ccomplemento;
             etClienteBairro.Text      = Session.Instance.Cbairro;
             etClienteCidade.Text      = Session.Instance.Ccidade;
             etClienteCep.Text         = Session.Instance.Ccep;
             etClienteUf.Text          = Session.Instance.Cuf;
             
             etClienteEmail.Text       = Session.Instance.Email;
             etClienteSenha.Text       = Session.Instance.Senha;
             
             int idStatus              = Session.Instance.IdStatus;

            #endregion

            // Captura o ID do cliente logado
            idCli = Session.Instance.IdCliente;

            // Esconde o label 'Cadastrar-se'
            lblCadastrarSe.IsVisible = false;

            // Mostra o label 'Meu cadastro'
            lblCadastrarSe.IsVisible = true;

            // Bloqueia os campos para alteração
            DadosEnabled();

            // Esconde o botão 'Avançar'
            btnAvancar.IsVisible = false;

            // Mostra os botões 'Voltar' e 'Avançar'
            stBtnVoltar.IsVisible = true;
            stBtnAvancar.IsVisible = true;
        }

        #endregion

        #region Bloqueia e libera campo para alteração

        #region DadosEnabled()

        public void DadosEnabled()
        {
            #region Enabled = true;

            etClienteNome.IsEnabled        = true;
            etClienteRg.IsEnabled          = true;
            etClienteCpf.IsEnabled         = true;
            etClienteDataNascto.IsEnabled  = true;
            etClienteSexo.IsEnabled        = true;
            etClienteCelular.IsEnabled     = true;
            etClienteCelular02.IsEnabled   = true;
            etClienteEndereco.IsEnabled    = true;
            etClienteNumero.IsEnabled      = true;
            etClienteComplemento.IsEnabled = true;
            etClienteBairro.IsEnabled      = true;
            etClienteCidade.IsEnabled      = true;
            etClienteCep.IsEnabled         = true;
            etClienteUf.IsEnabled          = true;
            etClienteEmail.IsEnabled       = true;
            etClienteConfEmail.IsEnabled   = true;
            etClienteSenha.IsEnabled       = true;
            etClienteConfSenha.IsEnabled   = true;

            #endregion
        }

        #endregion

        #region DadosNotEnabled()

        public void DadosNotEnabled()
        {
            #region Enabled = false

            etClienteNome.IsEnabled        = false;
            etClienteRg.IsEnabled          = false;
            etClienteCpf.IsEnabled         = false;
            etClienteDataNascto.IsEnabled  = false;
            etClienteSexo.IsEnabled        = false;
            etClienteCelular.IsEnabled     = false;
            etClienteCelular02.IsEnabled   = false;
            etClienteEndereco.IsEnabled    = false;
            etClienteNumero.IsEnabled      = false;
            etClienteComplemento.IsEnabled = false;
            etClienteBairro.IsEnabled      = false;
            etClienteCidade.IsEnabled      = false;
            etClienteCep.IsEnabled         = false;
            etClienteUf.IsEnabled          = false;
            etClienteEmail.IsEnabled       = false;
            etClienteConfEmail.IsEnabled   = false;
            etClienteSenha.IsEnabled       = false;
            etClienteConfSenha.IsEnabled   = false;

            #endregion
        }

        #endregion

        #endregion

        #region Menu - Logado

        #region Btn - Home
        private async void BtnHome_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new Views.LgHome());
        }
        #endregion

        #region Btn - Coletas
        private async void BtnColetas_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.LgColetas());
        }
        #endregion

        #region Btn - Pesquisar
        private async void BtnPesquisar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.LgPesquisar());
        }
        #endregion

        #region Btn - Orcamentos
        private async void BtnOrcamentos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.LgOrcamentos());
        }
        #endregion

        #region Btn - Minha Conta
        private async void BtnMinhaConta_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.LgMinhaContaa());
        }
        #endregion

        #endregion
        
        #endregion

        #region Btn - Voltar - Busca
        private async void BtnVoltar2_Clicked(object sender, EventArgs e)
        {
            if (etClienteNome.IsVisible)
            {
                // Esconde os campos dos dados pessoais
                DadosPessoaisNotVisible();

                // mostra o botão 'Avançar'
                stBtnAvancar.IsVisible = true;

                // Mostra e esconde labels
                lblMeuCadastro.IsVisible = false;

                // Direciona para a tela 'Minha Conta'
                await Navigation.PushModalAsync(new Views.LgMinhaContaa());
            }
            else if(etClienteEndereco.IsVisible)
            {
                EnderecoNotVisible();
                DadosPessoaisVisible();

                stBtnAvancar.IsVisible = true;
            }
            else if (etClienteEmail.IsVisible)
            {
                EmailSenhaNotVisible();
                EnderecoVisible();
                stBtnAvancar.IsVisible = true;
            }
        }
        #endregion

        #region Btn - Avançar - Buscar
        private void BtnAvancar2_Clicked(object sender, EventArgs e)
        {
            if (etClienteNome.IsVisible)
            {
                DadosPessoaisNotVisible();
                EnderecoVisible();
            }
            else if (etClienteEndereco.IsVisible)
            {
                EnderecoNotVisible();
                EmailSenhaVisible();

                btnAvancar2.IsVisible  = false;
                stBtnAvancar.IsVisible = false;
            }
        }
        #endregion
        
        #region VerificaCampos(id) - Cadastro
        private async Task VerificaCamposAsync(int id)
        {
            Cliente cliente;

            #region Mensagens de retorno
            string nulo       = "Preencha o campo: ";
            string finalizado = "Cadastro finalizado com sucesso!";
            string emails     = "Os e-mails não coincidem!";
            string senhas     = "As senhas não coincidem!";
            string dadosInsuf = "Dados insuficientes no campo: ";
            string caracter   = "Retire os caracteres especiais de: ";
            string senhaCurta = "A senha deve ter mais de 6 caracteres!";
            string atualizado = "Cadastro atualizado com sucesso!";
            #endregion

            #region Others
            /* String categoria    = etClienteCategoriaCnh.Items[etClienteCategoriaCnh.SelectedIndex]; 
               String uf           = etClienteUf.Items[etClienteUf.SelectedIndex]; 
               String conta        = etClienteTipoConta.Items[etClienteTipoConta.SelectedIndex];*/
            #endregion

            #region Dados Pessoais
            string nome         = etClienteNome.Text;
            string rg           = etClienteRg.Text;
            string cpf          = etClienteCpf.Text;
            string sexo         = etClienteSexo.Items[etClienteSexo.SelectedIndex];
            string dataNascto   = etClienteDataNascto.Text;
            string celular      = etClienteCelular.Text;
            string celular2     = etClienteCelular02.Text;
            int    IdTipoUsuario = 2;
            int    IdStatus     = 4;
            int    idCliente    = 1;
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
            int      idBandeira     = 1;
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
                if (etClienteNome.IsVisible)
                {
                    #region Campos
                    if (string.IsNullOrEmpty(nome))                         // NOME
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteNome.Text;
                    }
                    else if (nome.Length < 6)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = dadosInsuf + lblClienteNome.Text;
                    }
                    /*
                    else if (ValidaCampos.CaracterEspecial(nome))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteNome.Text;
                    } */

                    else if (string.IsNullOrEmpty(rg))                      // RG
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteRg.Text;
                    }
                    else if (rg.Length < 9)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = dadosInsuf + lblClienteRg.Text;
                    }
                    else if (ValidaCampos.CaracterEspecial(rg))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteRg.Text;
                    }

                    else if (string.IsNullOrEmpty(cpf))                     // CPF
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCpf.Text;
                    }
                    else if (cpf.Length < 11)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = dadosInsuf + lblClienteCpf.Text;
                    }
                    else if (ValidaCampos.CaracterEspecial(cpf))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteCpf.Text;
                    }

                    else if (string.IsNullOrEmpty(sexo))                    // SEXO
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteSexo.Text;
                    }
                    
                    else if (string.IsNullOrEmpty(dataNascto))              // DATA DE NASCIMENTO
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteDataNascto.Text;
                    }
                    else if (dataNascto.Length < 8)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = dadosInsuf + lblClienteDataNascto.Text;
                    }
                    /*
                    else if (ValidaCampos.CaracterEspecial(dataNascto))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteDataNascto.Text;
                    } */

                    else if (string.IsNullOrEmpty(celular))                 // CELULAR 
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCelular.Text;
                    }
                    else if (celular.Length < 9)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = dadosInsuf + lblClienteCelular.Text;
                    }
                    else if (ValidaCampos.CaracterEspecial(celular))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteCelular.Text;
                    }

                    else if (string.IsNullOrEmpty(celular2))                // CELULAR 02
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCelular02.Text;
                    }
                    else if (celular2.Length < 9)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = dadosInsuf + lblClienteCelular02.Text;
                    }
                    else if (ValidaCampos.CaracterEspecial(celular2))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteCelular02.Text;
                    }
                    #endregion

                    else
                    {
                        #region Verifica o CPF

                        /* var verificar = await control.GetCpf(cpf);

                        string verificaCpf = verificar.Ccpf.ToString();

                        if (verificaCpf != null)
                        {
                            lblAlerta.IsVisible = true;
                            lblAlerta.Text = "";
                            lblAlerta.Text = "CPF já cadastrado!";
                        }
                        else
                        { */

                        #endregion

                        DadosPessoaisNotVisible();

                        if(verificaOperacao == 1)
                        {
                            btnEndereco.IsEnabled = true;
                        }

                        btnAvancar.IsVisible  = false;

                        btnAvancar2.IsVisible = true;
                                    
                        if(verificaOperacao == 1)       // INSERT
                        {
                            btnVoltar.IsVisible = true;
                        }
                        else                            // UPDATE
                        {   stBtnVoltar.IsVisible = true; }                        

                        lblAlerta.IsVisible   = false;

                        EnderecoVisible();
                    }
                }
                #endregion

                #region Valida - Endereço
                else if (etClienteEndereco.IsVisible)
                {
                    #region Campos
                    if (string.IsNullOrEmpty(endereco))                         // ENDEREÇO
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteEndereco.Text;
                    }
                    else if (endereco.Length < 5)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = dadosInsuf + lblClienteEndereco.Text;
                    }
                    /*
                    else if (ValidaCampos.CaracterEspecial(endereco))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteEndereco.Text;
                    }*/

                    else if (string.IsNullOrEmpty(numero))                      // NUMERO
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteNumero.Text;
                    }
                    else if (ValidaCampos.CaracterEspecial(numero))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteNumero.Text;
                    }

                    else if (string.IsNullOrEmpty(complemento))                 // COMPLEMENTO
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCompl.Text;
                    }

                    else if (string.IsNullOrEmpty(bairro))                      // BAIRRO
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteBairro.Text;
                    }
                    else if (bairro.Length < 5)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = dadosInsuf + lblClienteBairro.Text;
                    }

                    else if (string.IsNullOrEmpty(cidade))                      // CIDADE
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCidade.Text;
                    }
                    else if (cidade.Length < 6)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = dadosInsuf + lblClienteCidade.Text;
                    }
                    /*
                    else if (ValidaCampos.CaracterEspecial(cidade))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteCidade.Text;
                    }*/

                    else if (string.IsNullOrEmpty(cep))                         // CEP
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCep.Text;
                    }
                    else if (cep.Length < 8)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = dadosInsuf + lblClienteCep.Text;
                    }
                    else if (ValidaCampos.CaracterEspecial(cep))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteCep.Text;
                    }

                    else if (string.IsNullOrEmpty(uf))                          // UF
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteUf.Text;
                    }
                    else if (uf.Length < 2)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = dadosInsuf + lblClienteUf.Text;
                    }
                    else if (ValidaCampos.CaracterEspecial(uf))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteUf.Text;
                    }
                    #endregion

                    else
                    {
                        EnderecoNotVisible();
                        
                        if (verificaOperacao != 2)  // insert
                        {
                            btnDadosBancarios.IsEnabled = true;

                            DadosBancariosVisible();                            
                        }
                        else                        // update
                        {
                            btnAvancar2.IsVisible = false;
                            btnSalvar.IsVisible   = true;

                            EmailSenhaVisible();
                        }
                        lblAlerta.IsVisible = false;
                    }
                }
                #endregion

                #region Valida - Dados bancários
                else if (btnDadosBancarios.IsEnabled && etClienteNumeroCartao.IsVisible)
                {
                    if (string.IsNullOrEmpty(numeroCartao))                     // NUMERO CARTAO
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteNumeroCartao.Text;
                    }
                    else if (numeroCartao.Length < 15)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteNumeroCartao.Text;
                    }
                    else if (ValidaCampos.CaracterEspecial(numeroCartao))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteNumeroCartao.Text;
                    }

                    else if (string.IsNullOrEmpty(dataValidade))                // DATA VALIDADE CARTAO
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteValidadeCartao.Text;
                    }
                    else if (dataValidade.Length < 6)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteValidadeCartao.Text;
                    }
                    else if (ValidaCampos.CaracterEspecial(dataValidade))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteValidadeCartao.Text;
                    }

                    else if (string.IsNullOrEmpty(codSeguranca.ToString()))     // CODIGO SEGURANÇA
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCodSeguranca.Text;
                    }
                    else if (codSeguranca.ToString().Length < 3)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteCodSeguranca.Text;
                    }
                    else if (ValidaCampos.CaracterEspecial(codSeguranca.ToString()))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteCodSeguranca.Text;
                    }

                    else if (string.IsNullOrEmpty(nomeImpresso))                // NOME IMPRESSO
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteNomeImpresso.Text;
                    }
                    else if (nomeImpresso.Length < 6)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteNomeImpresso.Text;
                    }
                    /*
                    else if (ValidaCampos.CaracterEspecial(nomeImpresso))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteNomeImpresso.Text;
                    }*/

                    else if (string.IsNullOrEmpty(bandeira))                    // BANDEIRA
                    {       
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteBandeira.Text;
                    }
                    else if (bandeira.Length < 4)
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteBandeira.Text;
                    }
                    /*
                    else if (ValidaCampos.CaracterEspecial(bandeira))
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = caracter + lblClienteBandeira.Text;
                    }*/

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
                
                else if (etClienteEmail.IsVisible)
                {
                    #region Valida - Email e senha
                    if (string.IsNullOrEmpty(email))                        // EMAIL
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteEmail.Text;
                    }

                    else if (verificaOperacao == 1)
                    {
                        if (string.IsNullOrEmpty(confemail))               // CONFIRMAÇÃO DE EMAIL
                        {
                            lblAlerta.IsVisible = true;
                            lblAlerta.Text = "";
                            lblAlerta.Text = nulo + lblClienteConfEmail.Text;
                        }
                        else if (!email.Equals(confemail))                      // COMPARA E-MAIL
                        {
                            lblAlerta.IsVisible = true;
                            lblAlerta.Text = "";
                            lblAlerta.Text = emails;
                        }
                    }

                    else if (string.IsNullOrEmpty(senha))                   // SENHA
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = nulo + lblClienteSenha.Text;
                    }

                    else if (verificaOperacao == 1)
                    {
                        if (string.IsNullOrEmpty(confsenha))               // CONFIRMAÇÃO DE SENHA
                        {
                            lblAlerta.IsVisible = true;
                            lblAlerta.Text = "";
                            lblAlerta.Text = nulo + lblClienteConfSenha.Text;
                        }
                        else if (!senha.Equals(confsenha))                  // COMPARA SENHA
                        {
                            lblAlerta.IsVisible = true;
                            lblAlerta.Text = "";
                            lblAlerta.Text = senhas;
                        }
                    }

                    else if (senha.Length < 6)                              // SENHA
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = senhaCurta;
                    }

                    else if (!ValidaCampos.IsEmail(email))                  // VALIDANDO E-MAIL
                    {
                        lblAlerta.IsVisible = true;
                        lblAlerta.Text = "";
                        lblAlerta.Text = "E-mail inválido!";
                    }
                    
                    #endregion

                    else
                    {
                        EmailSenhaNotVisible();

                        //-------------------------------------------------------------------------
                        // Salvando ...

                        #region Cliente()
                        cliente = new Cliente
                            (nome, rg, cpf, sexo, dataNascto, celular, celular2, endereco, numero,
                               complemento, bairro, cidade, cep, uf, email, senha, IdTipoUsuario, IdStatus
                            );
                        #endregion

                        #region INSERT

                        if (id == 1)
                        {
                            await control.PostAsync(cliente);       // Insert -Tab. Cliente

                            // Captura o IdCliente gerado no banco 
                            Cliente clien = await control.GetCpf(cpf);

                            #region CartaoCredito()

                            CartaoCredito cartao = new CartaoCredito()
                            {
                                IdCliente = clien.IdCliente,
                                Ccpf = cpf,
                                CNumeroCartao = numeroCartao,
                                IdBandeira = idBandeira,
                                CDataValidade = dataValidade,
                                CCodigoSeg = codSeguranca,
                                CDataCadastro = DateTime.Now,
                                IdStatus = 4,
                                CDataInativacao = null,
                                CUltimaAtualizacao = null,
                                BandeiraDescricao = bandeira,
                                NomeImpresso = nomeImpresso
                            };
                            #endregion

                            await controlCartao.PostCartaoAsync(cartao);    // Insert - Tab. Cartao
                            
                            btnFinalizar.IsVisible = false;
                            btnVoltar.IsVisible    = false;
                            lblAlerta.IsVisible    = false;

                            // Direciona para a tela de login
                            await Navigation.PushModalAsync(new Views.Login());
                            //btnLogar.IsVisible     = true;
                        }

                        #endregion

                        #region UPDATE 

                        if (id == 2)
                        {
                            await control.UpdateCliente(cliente, idCli);

                            await DisplayAlert("Sucesso!", "Cadastro realizado com sucesso!", "OK");  // Confirma a atualização

                            btnSalvar.IsVisible = false;

                            await Navigation.PushModalAsync(new Views.LgMinhaContaa());  // Direciona para 'Minha Conta'

                        }

                        #endregion

                        //-------------------------------------------------------------------------
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Source != null)
                {
                    //await DisplayAlert("Erro", ex.ToString(), "OK");
                    await DisplayAlert("Erro", ex.ToString(), "OK");

                    Console.WriteLine("Exception source: {0}", ex.Source);

                    // Caso não seja possível realizar o processo de cadastro total (de todas as classes),
                    // é excluído o que já foi cadastrado.

                    #region Exclui registros cadastrados caso não tenha sido possível inserir os dados em todas as classes envolvidas
                    Cliente clien = await control.GetCpf(cpf);

                    // Pega o ID do cliente gerado  ----  //int idCliente = clien.IdCliente;
                    
                    if (control.GetCpf(cpf) != null)
                    {
                        await control.DeleteCliente(idCliente);
                    }
                    #endregion
                }
                throw;
            }
        }
        #endregion


        #region Não Logado / Cadastro

        #region Botões

        #region Botão - Avançar
        private async void BtnAvancar_Clicked(object sender, EventArgs e)
        {
            if (verificaOperacao == 1)
            {
                await VerificaCamposAsync(1);
            }
            else
            {
                await VerificaCamposAsync(2);
            }
        }
        #endregion

        #region Botão - Finalizar
        private async void BtnFinalizar(object sender, EventArgs e)
        {
            await VerificaCamposAsync(1);
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

        #region Navegação entre as páginas

        #region Btn - Home
        private async void BtnLogar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.LgHome());
        }
        #endregion

        #region Btn - Conhecer App
        private async void BtnConhecerApp_Clicked(object sender, EventArgs e)
        {
            if ((etClienteNome.Text != null && btnVoltar.IsVisible) || (etClienteNome.Text == null && !btnVoltar.IsVisible))
            {
                await Navigation.PushModalAsync(new Views.ConhecerApp());
            }
            else if (await DisplayAlert("Deseja sair?", "Tem certeza que deseja sair? Todos os dados serão perdidos.",
                    "OK", "Cancelar"))
            {
                await Navigation.PushModalAsync(new Views.ConhecerApp());
            }
        }
        #endregion

        #region Btn - Cadastrar-se
        private async void BtnCadastreSe_Clicked(object sender, EventArgs e)
        {
            if ((etClienteNome.Text != null && btnVoltar.IsVisible) || (etClienteNome.Text == null && !btnVoltar.IsVisible))
            {
                await Navigation.PushModalAsync(new Views.CadastreSe());
            }
            else if (await DisplayAlert("Deseja sair?", "Tem certeza que deseja sair? Todos os dados serão perdidos.",
                    "OK", "Cancelar"))
            {
                await Navigation.PushModalAsync(new Views.CadastreSe());
            }
        }
        #endregion

        #region Btn - Login
        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            if ((etClienteNome.Text != null && btnVoltar.IsVisible) || (etClienteNome.Text == null && !btnVoltar.IsVisible))
            {
                await Navigation.PushModalAsync(new Views.Login());
            }
            else if (await DisplayAlert("Deseja sair?", "Tem certeza que deseja sair? Todos os dados serão perdidos.",
                    "OK", "Cancelar"))
            {
                await Navigation.PushModalAsync(new Views.Login());
            }
        }
        #endregion

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