using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste03.ClassesComuns;
using Teste03.Controllers;
using Teste03.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Teste03.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LgDadosBancarios : ContentPage
	{
        #region Parâmetros / Controllers

        //public static int id = Session.Instance.IdCliente;
        public static int id  = Session.Instance.IdCliente;
        public int        idContaBancaria;
        public string     cpf = Session.Instance.Ccpf;

        ContaBancariaController contaControl = new ContaBancariaController();

        #endregion

        public LgDadosBancarios ()
		{
			InitializeComponent ();

            // Chama a lista das contas já cadastradas
            ListaAsync();
		}

        public async void BtnMinhasContas_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushModalAsync(new Views.LgDadosBancarios());
        }

        public async void BtnVoltar2_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            await Navigation.PushModalAsync(new Views.LgDadosBancarios());
        }
    
        #region Popula os campos com dados do campo

        public void Popula(ContaBancaria conta)
        {
            if (conta != null)
            {
                slMinhasContas.IsVisible = false;
                slAdicionar.IsVisible    = true;

                #region Popula os campos

                etMotoristaBanco.Text         = conta.BancoDesc;
                etMotoristaAgencia.Text       = conta.MAgencia.ToString();
                etMotoristaDigitoAgencia.Text = conta.MDigAgencia.ToString();
                etMotoristaConta.Text         = conta.MConta.ToString();
                etMotoristaDigitoConta.Text   = conta.MDigConta.ToString();
                etMotoristaTipoConta.Text     = conta.TipoContaBanDesc;

                #endregion

                // Bloqueia os campos
                ContaEnabledFalse();

                stBtnVoltar.IsVisible = true;

                slEditarConta.IsVisible  = true;
                slExcluirConta.IsVisible = true;

                idContaBancaria = conta.IdContaBancaria;
            }
        }

        #endregion

        #region Btn - Buscar - Editar - Excluir - VERIFICAR!!!
        /*
        #region Btn - Avançar - Buscar
        private void BtnAvancar2_Clicked(object sender, EventArgs e)
        {
            ChassiNotVisible();
            DimensoesVisible();
            stBtnAvancar.IsVisible = false;

            slEditarVeiculos.IsVisible = true;
            slExcluirVeiculos.IsVisible = true;
        }
        #endregion

        #region Btn - Voltar - Busca
        private void BtnVoltar2_Clicked(object sender, EventArgs e)
        {
            if (stBtnAvancar.IsVisible)
            {
                slMeusVeiculos.IsVisible = true;
                slAdicionar.IsVisible = false;
                slEditarVeiculos.IsVisible = false;
                slExcluirVeiculos.IsVisible = false;
            }
            else
            {
                DimensoesNotVisible();
                ChassiVisible();
                stBtnAvancar.IsVisible = true;
                slEditarVeiculos.IsVisible = true;
                slExcluirVeiculos.IsVisible = true;
            }
        }
        #endregion
         */
        #endregion

        #region Botões

        #region Btn - CRUD

        #region Btn - Editar
        private async void BtnEditarConta_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            ContaEnabledTrue();

            // Esconde os botões 'Editar' e 'Excluir'
            slEditarConta.IsVisible  = false;
            slExcluirConta.IsVisible = false;

            // Mostra o botão 'Salvar'
            btnSalvar2.IsVisible = true;
        }

        #endregion

        #region Btn - Excluir

        private async void BtnExcluirConta_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            await contaControl.DeleteConta(idContaBancaria);

            await DisplayAlert("Excluído!", "Cadastro excluído com sucesso!", "OK");
        }

        #endregion

        #region Btn - Salvar - Cadastro

        public async void BtnAvancar_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            await VerificaCamposAsync(1);
        }

        #endregion

        #region Btn - Salvar - Edição 

        public async void BtnAvancar2_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            await VerificaCamposAsync(2);
        }

        #endregion

        #endregion

        #region Btn - Adicionar
        public void BtnAdicionarConta_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            slMinhasContas.IsVisible    = false;
            slAdicionar.IsVisible       = true;
            lblCadastroConta.IsVisible  = true;

            btnVoltar.IsVisible  = true;
            btnSalvar.IsVisible  = true;

            btnAdicionarConta.IsVisible = false;
        }
        #endregion
        
        #region Btn - Voltar
        public void BtnVoltar_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            if (btnSalvar.IsVisible)
            {

            }       
        }
        #endregion

        #endregion

        #region Bloqueia e libera campos

        #region Conta Enabled - False

        void ContaEnabledFalse()
        {
            etMotoristaBanco.IsEnabled         = false;
            etMotoristaAgencia.IsEnabled       = false;
            etMotoristaDigitoAgencia.IsEnabled = false;
            etMotoristaConta.IsEnabled         = false;
            etMotoristaDigitoConta.IsEnabled   = false;
            etMotoristaTipoConta.IsEnabled     = false;
        }

        #endregion

        #region Conta Enabled - True

        void ContaEnabledTrue()
        {
            etMotoristaBanco.IsEnabled         = true;
            etMotoristaAgencia.IsEnabled       = true;
            etMotoristaDigitoAgencia.IsEnabled = true;
            etMotoristaConta.IsEnabled         = true;
            etMotoristaDigitoConta.IsEnabled   = true;
            etMotoristaTipoConta.IsEnabled     = true;
        }

        #endregion

        #endregion

        #region Lista

        #region ListaAsync()

        public async void ListaAsync()
        {
            List<ContaBancaria> _list = await contaControl.GetListConta(id);

            if (_list == null)
            {
                LstConta.IsVisible = false;

                lbListaVazia.IsVisible = true;
            }
            else
            {
                LstConta.ItemsSource = _list;
            }
        }
        #endregion

        #region Lista de contas bancárias - Item selecionado
        private async void LstConta_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            // obtem o item do listview
            var conta = e.SelectedItem as ContaBancaria;

            // deseleciona o item do listview
            LstConta.SelectedItem = null;

            // Popula os campos como o objeto retornado
            Popula(conta);
            
            // 
            btnAdicionarConta.IsVisible = false;
        }
        #endregion

        #endregion

        #region VerificaCampos(int i)

        private async Task VerificaCamposAsync(int i)
        {
            #region Mensagens de retorno
            string nulo       = "Preencha o campo: ";
            string finalizado = "Cadastro finalizado com sucesso!";
            string dadosInsuf = "Dados insuficientes no campo: ";
            string caracter   = "Retire os caracteres especiais de: ";
            #endregion

            #region Dados bancários

            //int      idContaBancaria = 0;
            int      idBanco         = 1;
            int      agencia         = Convert.ToInt32(etMotoristaAgencia.Text);
            int      digAgencia      = Convert.ToInt32(etMotoristaDigitoAgencia.Text);
            int      conta           = Convert.ToInt32(etMotoristaConta.Text);
            int      digConta        = Convert.ToInt32(etMotoristaDigitoConta.Text);
            int      idTipoConta     = 1;
            int      idStatus        = 4;
            DateTime dataCadastro    = DateTime.Now;
            DateTime ultimaAtualizacao;
            string   bancoDesc       = etMotoristaBanco.Text;
            string   tipoContaBan    = etMotoristaTipoConta.Text;

            #endregion

            #region Valida() - Dados bancários

            if (string.IsNullOrEmpty(bancoDesc))                       // BANCO - DESCRIÇÃO
            {
                await DisplayAlert("Campo vazio", nulo + bancoDesc.ToString(), "OK");
            }
            else if (bancoDesc.Length < 2)
            {
                await DisplayAlert("Dados insuficientes", dadosInsuf + lblMotoristaBanco.Text, "OK");
            }

            else if (agencia < 0)                                              // AGENCIA
            {
                await DisplayAlert("Campo vazio", nulo + lblMotoristaAgencia.Text, "OK");
            }
            else if (ValidaCampos.CaracterEspecial(agencia.ToString()))
            {
                await DisplayAlert("Caracteres inválidos", caracter + lblMotoristaAgencia.Text, "OK");
            }
            else if (conta < 0)                                             // DADOS CONTA
            {
                await DisplayAlert("Campo vazio", nulo + lblMotoristaConta.Text, "OK");
            }
            else if (ValidaCampos.CaracterEspecial(conta.ToString()))
            {
                await DisplayAlert("Caracteres inválidos", caracter + lblMotoristaConta.Text, "OK");
            }

            else if (digConta < 0)                                          // DIGITO CONTA
            {
                await DisplayAlert("Campo vazio", nulo + lblMotoristaDigitoConta.Text, "OK");
            }
            else if (ValidaCampos.CaracterEspecial(digConta.ToString()))
            {
                await DisplayAlert("Caracteres inválidos", caracter + lblMotoristaDigitoConta.Text, "OK");
            }

            else if (string.IsNullOrEmpty(tipoContaBan))                    // TIPO CONTA BANCÁRIA
            {
                await DisplayAlert("Campo vazio", nulo + lblMotoristaTipoConta.Text, "OK");
            }

            #endregion

            else
            {
                // Conta bancária ---------------------------------------------------------

                #region ContaBancaria()
                ContaBancaria contaBan = new ContaBancaria()
                {
                    IdCliente          = id,
                    IdBanco            = idBanco,
                    MAgencia           = agencia,
                    MDigAgencia        = digAgencia,
                    MConta             = conta,
                    MDigConta          = digConta,
                    IdTipoConta        = idTipoConta,
                    MDataCadastro      = DateTime.Now,
                    IdStatus           = idStatus,
                    Ccpf               = cpf,
                    MUltimaAtualizacao = null,
                    BancoDesc          = bancoDesc,
                    TipoContaBanDesc   = tipoContaBan
                };
                #endregion
                
                if (i == 1)              // INSERT ----------------------------------------
                {
                    await contaControl.PostContaAsync(contaBan);

                    await DisplayAlert("Sucesso!", "Cadastro finalizado com sucesso!", "OK");

                    await Navigation.PushModalAsync(new Views.LgDadosBancarios());
                }
                else                   // UPDATE ----------------------------------------
                {
                    contaBan.IdContaBancaria    = idContaBancaria;
                    contaBan.MUltimaAtualizacao = DateTime.Now;

                    await contaControl.UpdateConta(contaBan);

                    await DisplayAlert("Sucesso!", "Cadastro atualizado com sucesso!", "OK");

                    await Navigation.PushModalAsync(new Views.LgDadosBancarios());
                }
                
                //-------------------------------------------------------------------------
                
                // Esconde os botões
                btnVoltar.IsVisible  = false;
                btnSalvar.IsVisible  = false;
                btnSalvar2.IsVisible = false;

                // Esconde os campos
                slAdicionar.IsVisible    = false;

                // Mostra a lista de contas cadastradas e o botão 'Adicionar conta bancária'
                btnAdicionarConta.IsVisible = true;
                slMinhasContas.IsVisible    = true;
            }
        }
        #endregion

        #region Navegação entre as telas

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
        
    }
}