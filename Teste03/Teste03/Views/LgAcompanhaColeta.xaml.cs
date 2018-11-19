using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teste03.Controllers;
using Teste03.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Teste03.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LgAcompanhaColeta : ContentPage
	{
        #region Variáveis e controllers

        public int idCol;
        public int idOrca;
        public int idStatusOrcamento;
        public int idStatusColeta;

        public int idMotorista =  Models.Session.Instance.IdMotorista;    // Motorista: 1; //
        public int idCliente   =  7; //Models.Session.Instance.IdCliente;      // Motorista: 8; //  7; //
        public int idTipoUser  =  2; //Models.Session.Instance.IdTipoUsuario;  // Motorista: 3; // 2; //

        public string nomeMotorista;
        public string telefoneMotorista;

        Cliente   cli, cliMotorista;
        Coleta    coleta;
        Orcamento orcam;
        Status    status;
        Motorista motorista;

        ClienteController   clienteController   = new ClienteController();
        ColetaController    coletaControl       = new ColetaController();
        OrcamentoController orcaControl         = new OrcamentoController();
        StatusController    statusController    = new StatusController();
        MotoristaController motoristaController = new MotoristaController();

        #endregion
                
        public LgAcompanhaColeta ()
		{
			InitializeComponent ();

            // Atualiza a lista
            ListaColetas(idCliente);

		}

        #region Cliente


        #region ListaColetas 

        public async void ListaColetas(int idCliente)
        {
            #region Variáveis e controllers
            
            AcompanhaColeta     acompanha  = new AcompanhaColeta();

            AcompanhaController controller = new AcompanhaController();

            #endregion

            var _list = await coletaControl.GetListColetas_Acompanha(idCliente);

            if (_list == null)
            {
                LstColeta_Cliente.IsVisible = false;
            }
            else
            {
                LstColeta_Cliente.ItemsSource = _list;
            }
        }

        #endregion

        #region Lista - ItemSelected

        private void LstOrcamento_Cliente_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            // obtem o item do listview
            var orcamento = e.SelectedItem as Coleta;

            /*

            lblOrcaColeta.Text = "'" + orcamento.ApelidoColeta + "'";

            // Mostra
            stFiltrarColetas_Cliente.IsVisible = false;
            stOrca.IsVisible = true;

            //  Esconde
            stListaCliente.IsVisible = false;
            lbColetaOrcamento.IsVisible = false;

            idCol = orcamento.IdColeta;

            ListaColetas_Orcamento_(orcamento.IdColeta);

            stBtnVoltar_Cliente_.IsVisible = false; */
        }


        #endregion


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