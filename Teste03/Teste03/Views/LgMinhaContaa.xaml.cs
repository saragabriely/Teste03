using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Teste03.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LgMinhaContaa : ContentPage
	{
        #region Parametros - Captura dados das variáveis globais

        public int id    =  Models.Session.Instance.IdTipoUsuario; //3;
        public int idCli =  Models.Session.Instance.IdCliente; // 8;
        public int idMot =  Models.Session.Instance.IdMotorista; // 1;

        #endregion

        public LgMinhaContaa ()
		{
			InitializeComponent ();

            #region Verifica o tipo de usuário

            if (id == 2) // Id = 2 -> Cliente  // Id = 3 -> Motorista
            {
                slCliente.IsVisible   = true;
                slMotorista.IsVisible = false;
            }
            else
            {
                slCliente.IsVisible   = false;
                slMotorista.IsVisible = true;
            }

            #endregion
        }

        #region Btn - Cadastro - Cliente
        private async void btnClienteMeuCadastro_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.ClientePF_Cadastro());
        }
        #endregion

        #region Navegação entre as páginas

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
        
        #region MeuCadastro
        private async void BtnMeuCadastro_Clicked(object sender, EventArgs e)
        {
           // Models.Session.Instance.IdTipoUsuario = 3;

            if (Models.Session.Instance.IdTipoUsuario == 2)
            {
                await Navigation.PushModalAsync(new Views.ClientePF_Cadastro());
            }
            else if (Models.Session.Instance.IdTipoUsuario == 3)
            {
                await Navigation.PushModalAsync(new Views.Motorista_Cadastro());
            }
        }
        #endregion

        #region Btn - MinhaConta
        private async void BtnMinhaConta_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.LgMinhaContaa());
        }
        #endregion

        #region Btn - Veiculos
        private async void BtnVeiculos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.LgVeiculos());
        }
        #endregion

        #region Dados bancários
        private async void BtnContaBancaria_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.LgDadosBancarios());
        }
        #endregion


        #endregion

        #region Botões - Others

        #region Others (Apagar?)
        private void BtnCliente_Clicked(object sender, EventArgs e)
        {
           // slCliente.IsVisible = true;
           //slMotorista.IsVisible = false;
        }

        private void BtnMotorista_Clicked(object sender, EventArgs e)
        {
            //slCliente.IsVisible = false;
            //slMotorista.IsVisible = true;
        }
        #endregion

        #endregion

        #region LOGOUT
        private async void BtnSair_Clicked(object sender, EventArgs e)
        {
            bool verifica = await DisplayAlert("SAIR", "Deseja mesmo sair?", "Ok", "Cancelar");

            if (verifica)
            {
                #region Limpando as variáveis globais

                Models.Session.Instance.IdCliente     = 0;
                Models.Session.Instance.IdTipoUsuario = 0;
                Models.Session.Instance.Email         = "";
                Models.Session.Instance.Senha         = "";
                Models.Session.Instance.Cnome         = "";
                Models.Session.Instance.Crg           = "";
                Models.Session.Instance.Ccpf          = ""; 
                Models.Session.Instance.Csexo         = "";
                Models.Session.Instance.CdataNascto   = "";
                Models.Session.Instance.Ccelular      = "";
                Models.Session.Instance.Ccelular2     = "";
                Models.Session.Instance.Cendereco     = "";
                Models.Session.Instance.Cnumero       = "";
                Models.Session.Instance.Ccomplemento  = "";
                Models.Session.Instance.Cbairro       = "";
                Models.Session.Instance.Ccidade       = "";
                Models.Session.Instance.Ccep          = "";
                Models.Session.Instance.Cuf           = "";
                Models.Session.Instance.IdStatus      = 0;
                #endregion

                await Navigation.PushModalAsync(new Views.PaginaInicial());
            }
        }
        #endregion
        
    }
}