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
        public string nome = Models.Session.Instance.Cnome;
        /*
        public int    IdCliente       = Models.Session.Instance.IdCliente;
        public int    IdMotorista     = Models.Session.Instance.IdMotorista;
        public int    IdUsuario       = Models.Session.Instance.
        public int    IdTipoUsuario   = Models.Session.Instance.
        public string Email           = Models.Session.Instance.
        public string Senha           = Models.Session.Instance.
                                      = Models.Session.Instance.
        public string Cnome           = Models.Session.Instance.
        public string Crg             = Models.Session.Instance.
        public string Ccpf            = Models.Session.Instance.
        public string Csexo           = Models.Session.Instance.
        public string CdataNascto     = Models.Session.Instance.
        public string Ccelular        = Models.Session.Instance.
        public string Ccelular2       = Models.Session.Instance.
        public string Cendereco       = Models.Session.Instance.
        public string Cnumero         = Models.Session.Instance.
        public string Ccomplemento    = Models.Session.Instance.
        public string Cbairro         = Models.Session.Instance.
        public string Ccidade         = Models.Session.Instance.
        public string Ccep            = Models.Session.Instance.
        public string Cuf             = Models.Session.Instance.
        public int    IdStatus        = Models.Session.Instance.
            */
        #endregion

        public LgHome ()
		{
			InitializeComponent ();

            if(Models.Session.Instance.IdTipoUsuario == 3)
            {
                slMotorista.IsVisible    = true;
                lbBemVindoMotorista.Text = "Bem-vindo(a) " + nome.ToString();
            }
            else
            {
                slCliente.IsVisible    = true;
                lbBemVindoCliente.Text = "Bem-vindo(a) " + nome.ToString();
            }
        }

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