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
	public partial class LgHome : ContentPage
	{
        #region Variáveis - Populando
        public string nome            = Models.Session.Instance.Cnome;
        
        public int idMotorista =  Models.Session.Instance.IdMotorista;    // Motorista: 1; //
        public int idCliente   =  7; //  Models.Session.Instance.IdCliente;      // Motorista: 8; //
        public int idTipoUser  =  2; // Models.Session.Instance.IdTipoUsuario;  // Motorista: 3; //
            
        #endregion

        public LgHome ()
		{
			InitializeComponent ();

            #region Verifica o tipo de usuário

            if(Models.Session.Instance.IdTipoUsuario == 3)                  // Motorista
            {
                slMotorista.IsVisible    = true;
                lbBemVindoMotorista.Text = "Bem-vindo(a) " + nome.ToString();
            }
            else                                                            // Cliente
            {
                slCliente.IsVisible    = true;
                lbBemVindoCliente.Text = "Bem-vindo(a) " + nome.ToString();

                #region Verifica notificações
                
                NotificaOrcamento_Cliente();            // Orcamento

                NotificaColetaEmAndamento_Cliente();    // Coleta em andamento
                #endregion
            }
            #endregion
        }

        #region Cliente 

        #region Notificações

        #region NotificaOrcamento_Cliente()

        private void NotificaOrcamento_Cliente()
        {

        }

        #endregion

        #region NotificaOrcamento_Cliente()

        private void NotificaColetaEmAndamento_Cliente()
        {

        }

        #endregion

        #endregion

        #endregion

        #region Motorista

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

        #region Others
        private void BtnCliente_Clicked(object sender, EventArgs e)
        {
            slCliente.IsVisible = true;
            slMotorista.IsVisible = false;
            
            lbBemVindoCliente.Text = "Bem-vindo(a) " + nome.ToString();
        }

        private void BtnMotorista_Clicked(object sender, EventArgs e)
        {
            slCliente.IsVisible   = false;
            slMotorista.IsVisible = true;

            lbBemVindoMotorista.Text = "Bem-vindo(a) " + nome.ToString();
        }
        #endregion

    }
}