using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Teste03.Controllers;
using Teste03.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Teste03.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LgOrcamentos : ContentPage
	{
        #region Variáveis e controllers

        public int idCol;
        public int idOrca;
        public int idStatusOrcamento;
        public int idStatusColeta;

        public int idMotorista =  Models.Session.Instance.IdMotorista;    // Motorista: 1; //
        public int idCliente   =  7; //  Models.Session.Instance.IdCliente;      // Motorista: 8; //
        public int idTipoUser  =  2; // Models.Session.Instance.IdTipoUsuario;  // Motorista: 3; //

        public string enderecoRetirada;
        public string enderecoEntrega;
        public string google = "http://www.google.com.br/maps/dir/";
        public string value;

        Cliente   cli;
        Coleta    coleta;
        Orcamento orcam;
        Status    status;

        ClienteController   clienteController  = new ClienteController();
        ColetaController    coletaControl      = new ColetaController();
        OrcamentoController orcaControl        = new OrcamentoController();
        StatusController    statusController   = new StatusController();

        #endregion

        public LgOrcamentos ()
		{
			InitializeComponent ();
            
            #region Verifica o tipo de usuário

            if(idTipoUser == 3)         // CLIENTE
            {
                slMotorista.IsVisible = true;
                slCliente.IsVisible   = false;

                Filtro_Motorista();     // Carrega o filtro
            }
            else                        // MOTORISTA
            {
                slMotorista.IsVisible = false;
                slCliente.IsVisible   = true;

                ListaColetas_Orcamento(idCliente);

                //Filtro_Cliente();
            }
            
            #endregion
        }

        #region Cliente

        #region Filtro

        #region Filtro_Cliente()

        private void Filtro_Cliente()
        {
            #region Lista - Encontrar

            List<string> lstOrcamento_Cliente = new List<string>
            {
                "TODOS OS ORÇAMENTOS",
                "Orçamentos novos",
                "Orçamentos aceitos",
                "Orçamentos rejeitados"
            };

            #endregion

            // 1 - Carrega o dropdown
            etClienteFiltroOrcamento.ItemsSource = lstOrcamento_Cliente;
            etClienteFiltroOrcamento.SelectedIndex = 0;

            // 2 - Atualiza a lista de acordo com a opção escolhida
            ListaCliente_OrcamentoAsync(0);
        }
        #endregion

        #region Filtro - Orçamento - Cliente
        private void pckClienteFiltroOrcamento(object sender, EventArgs e)
        {
            var itemSelecionado = etClienteFiltroOrcamento.Items[etClienteFiltroOrcamento.SelectedIndex];

            if (itemSelecionado.Equals("TODOS OS ORÇAMENTOS"))
            {
                ListaCliente_OrcamentoAsync(0);
            }
            else if (itemSelecionado.Equals("Orçamentos novos"))
            {
                ListaCliente_OrcamentoAsync(1);
            }
            else if (itemSelecionado.Equals("Orçamentos aceitos"))
            {
                ListaCliente_OrcamentoAsync(2);
            }
            else if (itemSelecionado.Equals("Orçamentos rejeitados"))
            {
                ListaCliente_OrcamentoAsync(3);
            }
            else if (itemSelecionado.Equals("Orçamentos cancelados"))
            {
                ListaCliente_OrcamentoAsync(4);
            }
        }
        #endregion

        #endregion

        #region Lista - Cliente

        #region ListaColetas_Orcamento (coletas que possuem orçamento)

        public async void ListaColetas_Orcamento(int idCliente)
        {
            List<Coleta> _list;

            _list = null;

            _list = await coletaControl.GetListColeta_Orcamento(idCliente);

            if (_list == null)
            {
                LstOrcamento_Cliente.IsVisible = false;
            }
            else
            {
                LstOrcamento_Cliente.ItemsSource = _list;
            }
            
        }

        #endregion

        #region Lista de coletas - Item selecionado
        private async void LstOrcamentoCliente_01_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {            
            if (e.SelectedItem == null)
            {
                return;
            }
            // obtem o item do listview
            var orcamento = e.SelectedItem as Coleta;

            lblOrcaColeta.Text = "'" + orcamento.ApelidoColeta + "'";

            // Mostra
            stFiltrarColetas_Cliente.IsVisible = false;
            stOrca.IsVisible                   = true;

            //  Esconde
            stListaCliente.IsVisible    = false;
            lbColetaOrcamento.IsVisible = false;

            ListaColetas_Orcamento_(orcamento.IdColeta);



        }
        #endregion


        #region ListaColetas_Orcamento (coletas que possuem orçamento)

        public async void ListaColetas_Orcamento_(int idColeta)
        {
            List<Orcamento> _list;

            _list = null;

            _list = await orcaControl.GetListOrcamento_Cliente_(idColeta);

            if (_list == null)
            {
                LstOrcamento_Cliente_02.IsVisible = false;
            }
            else
            {
                LstOrcamento_Cliente_02.ItemsSource = _list;
            }
            
        }
        #endregion

        #region Lista de coletas - Item selecionado
        private async void LstOrcamentoCliente_02_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
            if (e.SelectedItem == null)
            {
                return;
            }
            // obtem o item do listview
            var orcamento = e.SelectedItem as Orcamento;

            orcam = orcamento;

            // Popula a parte do orçamento
            PopulaOrcamento_Motorista(orcamento);

            // Captura a coleta referente ao Orçamento selecionado
            coleta = await coletaControl.GetColeta(orcamento.IdColeta);

            // Popula as informações da coleta
            PopulaColeta_Motorista_(coleta);

            // deseleciona o item do listview
            LstOrcamento_Motorista.SelectedItem = null;

            #region Esconde

            // Esconde a lista
            stListaMoto_02.IsVisible = false;

            #endregion

            slMotoristaEncontrarColeta_Lista_.IsVisible = true;

            // Mostra os campos
            MostraOrcamento_();
            
            // Variáveis 
            idOrca              = orcamento.IdOrcamento;
            idStatusOrcamento   = orcamento.IdStatus;
            idStatusColeta      = coleta.IdStatus;
            value               = orcamento.Valor;

            // Concatena o endereço (nome da rua, numero e bairro)
            enderecoRetirada = coleta.EndRetEndereco + ", " + coleta.EndRetNumero + " - " + coleta.EndRetBairro;
            enderecoEntrega  = coleta.EndEntEndereco + ", " + coleta.EndEntNumero + " - " + coleta.EndEntBairro;
            
        }
        #endregion

        #region PopulaColeta_Motorista(coleta)

        private void PopulaColeta_Motorista_(Coleta coleta)
        {
            #region Campos

            etEndRet_Moto_.Text              = coleta.EndRetEndereco;
            etEndEnt_Moto_.Text              = coleta.EndEntEndereco;
            lbTipoMaterial_Moto_.Text        = coleta.MatTipo;
            lbFragilidadeMaterial_Moto_.Text = coleta.MatFragilidade;
            lbDescricaoMaterial_Moto_.Text   = coleta.MatDescricao;
            etPeso_Moto_.Text                = coleta.MatPeso;
            etVolume_Moto_.Text              = coleta.MatVolume;
            etLargura_Moto_.Text             = coleta.MatLargura;
            etAltura_Moto_.Text              = coleta.MatAltura;
            etDataMax_Moto_.Text             = coleta.DataMaxima;
            etHorario_Moto_.Text             = coleta.HorarioLimite + ":" + coleta.HorarioLimite02;
            etValorPretendido_Moto_.Text     = coleta.ValorPretendido;
            etObservacoes_Moto_.Text         = coleta.Observacoes;
            etTipoVeiculo_Moto_.Text         = coleta.TipoVeiculo;


            #endregion
        }

        #endregion


        #region ListaCliente_OrcamentoAsync(int i)  (lista dos orçamentos por coleta)

        public async void ListaCliente_OrcamentoAsync(int i)
        {
            List<Orcamento> _list;

            _list = null;

            if (i == 0)                                                             // TODOS OS ORÇAMENTOS
            {
                List<Orcamento> listaa = new List<Orcamento>();

               // _list = await orcaControl.GetListOrcamentos(idCliente);             // IdMotorista e IdStatus

                if (_list == null)
                {
                    LstOrcamento_Motorista.IsVisible = false;
                }
                else
                {
                    LstOrcamento_Motorista.ItemsSource = _list;
                }
            }
            else if (i == 1)                                                        // ORÇAMENTOS PENDENTES DE APROVAÇÃO 
            {                                                                       // (Aguardando aprovacao - 13)
                _list = await orcaControl.GetListOrcamento(idMotorista, 13);

                LstOrcamento_Motorista.ItemsSource = _list;
            }
            else if (i == 2)                                                        // ORÇAMENTOS ACEITOS (Aceito - 1)
            {
                _list = await orcaControl.GetListOrcamento(idMotorista, 1);

                LstOrcamento_Motorista.ItemsSource = _list;
            }
            else if (i == 3)                                                        // ORÇAMENTOS REJEITADOS (Rejeitado - 14)
            {
                _list = await orcaControl.GetListOrcamento(idMotorista, 14); ;

                LstOrcamento_Motorista.ItemsSource = _list;
            }
            else if (i == 4)                                                        // ORÇAMENTOS CANCELADOS (Cancelado - 6)
            {
                _list = await orcaControl.GetListOrcamento(idMotorista, 6);

                LstOrcamento_Motorista.ItemsSource = _list;
            }
        }
        #endregion 
        

        #region EscondeDados_Moto()

        public void EscondeDados_Moto_()
        {
            #region Campos

            lbEndRet_.IsVisible                   = false;
            etEndRet_Moto_.IsVisible              = false;
            lbEndEnt_.IsVisible                   = false;
            etEndEnt_Moto_.IsVisible              = false;
            lbTipoMaterial_.IsVisible             = false;
            lbTipoMaterial_Moto_.IsVisible        = false;
            lbFragilidadeMaterial_.IsVisible      = false;
            lbFragilidadeMaterial_Moto_.IsVisible = false;
            lbDescricaoMaterial_.IsVisible        = false;
            lbDescricaoMaterial_Moto_.IsVisible   = false;

             lbPeso_.IsVisible         = false;
            etPeso_Moto_.IsVisible    = false;
            lbPeso2_.IsVisible        = false;
            lbVolume_.IsVisible       = false;
            etVolume_Moto_.IsVisible  = false;
            lbLargura_.IsVisible      = false;
            etLargura_Moto_.IsVisible = false;
            lbLargura2_.IsVisible     = false;
            lbAltura_.IsVisible       = false;
            etAltura_Moto_.IsVisible  = false;
            lbAltura2_.IsVisible      = false;

            lbDataMax_.IsVisible      = false;
            etDataMax_Moto_.IsVisible = false;
            lbHorario_.IsVisible      = false;
            etHorario_Moto_.IsVisible = false;
            lbValorPretendido_.IsVisible      = false;
            etValorPretendidoRS_Moto_.IsVisible = false;
            etValorPretendido_Moto_.IsVisible = false;
            lbObservacoes_.IsVisible          = false;
            etObservacoes_Moto_.IsVisible     = false;
            lbTipoVeiculo_.IsVisible          = false;
            etTipoVeiculo_Moto_.IsVisible     = false;

            //lbStatusOrcamento_.IsVisible       = false;
           // etStatusOrcamento_.IsVisible       = false;
            lbValorOrcamento_.IsVisible        = false;
            etValor_Moto_.IsVisible            = false;
            etValorRS_Moto_.IsVisible          = false;
            lbObs_Orcamento_.IsVisible         = false;
            etObservacoes_Orcamento_.IsVisible = false;

            #endregion
        }

        #endregion
        
        
        #region MostraOrcamento()

        public void MostraOrcamento_()
        {
            
           // lbStatusOrcamento_.IsVisible       = true;
           // etStatusOrcamento_.IsVisible       = true;
            lbValorOrcamento_.IsVisible        = true;
            etValor_Moto_.IsVisible            = true;
            etValorRS_Moto_.IsVisible          = true;
            lbObs_Orcamento_.IsVisible         = true;
            etObservacoes_Orcamento_.IsVisible = true;

            lbPeso_.IsVisible            = true;
            etPeso_Moto_.IsVisible       = true;
            lbPeso2_.IsVisible           = true;
            lbVolume_.IsVisible          = true;
            etVolume_Moto_.IsVisible     = true;
            lbLargura_.IsVisible         = true;
            etLargura_Moto_.IsVisible    = true;
            lbLargura2_.IsVisible        = true;
            lbAltura_.IsVisible          = true;
            etAltura_Moto_.IsVisible     = true;
            lbAltura2_.IsVisible         = true;
                                        
            lbDataMax_.IsVisible              = true;
            etDataMax_Moto_.IsVisible         = true;
            lbHorario_.IsVisible              = true;
            etHorario_Moto_.IsVisible         = true;
            lbValorPretendido_.IsVisible      = true;
            etValorPretendidoRS_Moto_.IsVisible = true;
            etValorPretendido_Moto_.IsVisible = true;
            lbObservacoes_.IsVisible          = true;
            etObservacoes_Moto_.IsVisible     = true;
            lbTipoVeiculo_.IsVisible          = true;
            etTipoVeiculo_Moto_.IsVisible     = true;

            lbEndRet_.IsVisible                   = true;
            etEndRet_Moto_.IsVisible              = true;
            lbEndEnt_.IsVisible                   = true;
            etEndEnt_Moto_.IsVisible              = true;
            lbTipoMaterial_.IsVisible             = true;
            lbTipoMaterial_Moto_.IsVisible        = true;
            lbFragilidadeMaterial_.IsVisible      = true;
            lbFragilidadeMaterial_Moto_.IsVisible = true;
            lbDescricaoMaterial_.IsVisible        = true;
            lbDescricaoMaterial_Moto_.IsVisible   = true;
            
        }

        #endregion

        
        #endregion
        
        #endregion


        #region Motorista

        #region Filtro

        #region Filtro_Motorista()

        private void Filtro_Motorista()
        {
            #region Lista - Encontrar

            List<string> lstOrcamento_Motorista = new List<string>
            {
                "TODOS OS ORÇAMENTOS",
                "Orçamentos pendentes de aprovação",
                "Orçamentos aceitos",
                "Orçamentos rejeitados",
                "Orçamentos cancelados"
            };

            #endregion

            // 1 - Carrega o dropdown
            etMotoristaFiltroOrcamento.ItemsSource   = lstOrcamento_Motorista;
            etMotoristaFiltroOrcamento.SelectedIndex = 0;

            // 2 - Atualiza a lista de acordo com a opção escolhida
            ListaMotorista_OrcamentoAsync(0);
        }
        #endregion

        #region Filtro - Orçamento - Motorista
        private void pckMotoristaFiltroOrcamento(object sender, EventArgs e)
        {
            var itemSelecionado = etMotoristaFiltroOrcamento.Items[etMotoristaFiltroOrcamento.SelectedIndex];

            if (itemSelecionado.Equals("TODOS OS ORÇAMENTOS"))
            {
                ListaMotorista_OrcamentoAsync(0);
            }
            else if (itemSelecionado.Equals("Orçamentos pendentes de aprovação"))
            {
                ListaMotorista_OrcamentoAsync(1);
            }
            else if (itemSelecionado.Equals("Orçamentos aceitos"))
            {
                ListaMotorista_OrcamentoAsync(2);
            }
            else if (itemSelecionado.Equals("Orçamentos rejeitados"))
            {
                ListaMotorista_OrcamentoAsync(3);
            }
            else if (itemSelecionado.Equals("Orçamentos cancelados"))
            {
                ListaMotorista_OrcamentoAsync(4);
            }
        }
        #endregion

        #endregion

        #region Lista - Motorista

        #region ListaMotorista_OrcamentoAsync(int i)

        public async void ListaMotorista_OrcamentoAsync(int i)
        {
            List<Orcamento> _list;

            _list = null;

            if (i == 0)                                                             // TODOS OS ORÇAMENTOS
            {
                List<Orcamento> listaa = new List<Orcamento>();

                 _list = await orcaControl.GetListOrcamento_Geral(idMotorista);   // IdMotorista e IdStatus

                if (_list == null)
                {
                    LstOrcamento_Motorista.IsVisible = false;
                }
                else
                {
                    LstOrcamento_Motorista.ItemsSource = _list;
                }
            }
            else if (i == 1)                                                        // ORÇAMENTOS PENDENTES DE APROVAÇÃO 
            {                                                                       // (Aguardando aprovacao - 13)
                _list = await orcaControl.GetListOrcamento(idMotorista, 13);

                LstOrcamento_Motorista.ItemsSource = _list;
            }
            else if (i == 2)                                                        // ORÇAMENTOS ACEITOS (Aceito - 1)
            {
                _list = await orcaControl.GetListOrcamento(idMotorista, 1);

                LstOrcamento_Motorista.ItemsSource = _list;
            }
            else if (i == 3)                                                        // ORÇAMENTOS REJEITADOS (Rejeitado - 14)
            {
                _list = await orcaControl.GetListOrcamento(idMotorista, 14); ;

                LstOrcamento_Motorista.ItemsSource = _list;
            }
            else if (i == 4)                                                        // ORÇAMENTOS CANCELADOS (Cancelado - 6)
            {
                _list = await orcaControl.GetListOrcamento(idMotorista, 6);

                LstOrcamento_Motorista.ItemsSource = _list;
            }
        }
        #endregion
        
        #region Lista de coletas - Item selecionado
        private async void LstOrcamentoMotorista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            // obtem o item do listview
            var orcamento = e.SelectedItem as Orcamento;

            orcam = orcamento;

            // Popula a parte do orçamento
            PopulaOrcamento_Motorista(orcamento);

            // Captura a coleta referente ao Orçamento selecionado
             coleta = await coletaControl.GetColeta(orcamento.IdColeta);

            // Popula as informações da coleta
            PopulaColeta_Motorista(coleta);

            // deseleciona o item do listview
            LstOrcamento_Motorista.SelectedItem = null;
            
            #region Esconde

            // Esconde o filtro
            stFiltrarColetas_Moto.IsVisible = false;

            // Esconde a lista
            stListaMoto.IsVisible = false;

            #endregion

            slMotoristaEncontrarColeta_Lista.IsVisible = true;

            // Mostra os campos
            MostraOrcamento();
            MostraDados_Moto();
            MostraDados_2_Moto();

            stBtnVoltar_Moto.IsVisible = true;

            // Variáveis 
            idOrca            = orcamento.IdOrcamento;
            idStatusOrcamento = orcamento.IdStatus;
            idStatusColeta    = coleta.IdStatus;
            value             = orcamento.Valor;

            // Concatena o endereço (nome da rua, numero e bairro)
            enderecoRetirada = coleta.EndRetEndereco + ", " + coleta.EndRetNumero + " - " + coleta.EndRetBairro;
            enderecoEntrega  = coleta.EndEntEndereco + ", " + coleta.EndEntNumero + " - " + coleta.EndEntBairro;

            // Mostra o botão 'Realizar Coleta', caso o orçamento tenha sido aceito
            if (idStatusOrcamento == 1 && idStatusColeta != 10)       // IdStatus: 1 - Aceito + 10 - Finalizada
            {
                btnRealizarColeta.IsVisible = true;
            }        
        }
        #endregion

        #region PopulaOrcamento_Motorista(orcamento)

        private async void PopulaOrcamento_Motorista(Orcamento orcamento)
        {
            #region Busca status

            status = new Status();
            status = await statusController.GetStatus(orcamento.IdStatus);

            #endregion

            #region Campos

            etStatusOrcamento.Text       = status.DescricaoStatus.ToUpper();
            etValor_Moto.Text            = orcamento.Valor;
            etObservacoes_Orcamento.Text = orcamento.Observacoes;

            #endregion
        }

        #endregion

        #region PopulaColeta_Motorista(coleta)

        private void PopulaColeta_Motorista(Coleta coleta)
        {
            #region Campos

            etEndRet_Moto.Text              = coleta.EndRetEndereco;
            etEndEnt_Moto.Text              = coleta.EndEntEndereco;
            lbTipoMaterial_Moto.Text        = coleta.MatTipo;
            lbFragilidadeMaterial_Moto.Text = coleta.MatFragilidade;
            lbDescricaoMaterial_Moto.Text   = coleta.MatDescricao;
            etPeso_Moto.Text                = coleta.MatPeso;
            etVolume_Moto.Text              = coleta.MatVolume;
            etLargura_Moto.Text             = coleta.MatLargura;
            etAltura_Moto.Text              = coleta.MatAltura;
            etDataMax_Moto.Text             = coleta.DataMaxima;
            etHorario_Moto.Text             = coleta.HorarioLimite + ":" + coleta.HorarioLimite02;
            etValorPretendido_Moto.Text     = coleta.ValorPretendido;
            etObservacoes_Moto.Text         = coleta.Observacoes;
            etTipoVeiculo_Moto.Text         = coleta.TipoVeiculo;


            #endregion
        }

        #endregion

        #region Botões

        #region Btn - Realizar Coleta (direciona)

        private async void BtnRealizarColeta_Motorista_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            // Esconde o ScrollView
            slMotoristaEncontrarColeta_Lista.IsVisible = false;

            // Mostra a view 'Realizar Coleta'
            stRealizarColeta.IsVisible = true;

            stFiltrarColetas_Moto.IsVisible = false;

            #region Encontra cliente

            cli = await clienteController.GetCliente(idCliente);

            lblNomeCliente.Text = cli.Cnome;

            #endregion
            
        }

        #endregion


        #region Btn - Voltar

        private async void BtnVoltar_Moto_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            if (slMotoristaEncontrarColeta_Lista.IsVisible)
            {
                // Esconde o ScrollView
                slMotoristaEncontrarColeta_Lista.IsVisible = false;

                // Esconde o botão 'Voltar'
                stBtnVoltar_Moto.IsVisible = false;

                btnRealizarColeta.IsVisible = false;

                // Mostra o filtro e a lista
                stFiltrarColetas_Moto.IsVisible = true;
                stListaMoto.IsVisible = true;
            }
            else 
            {
                // Mostra o ScrollView
                slMotoristaEncontrarColeta_Lista.IsVisible = true;

                // Esconde o botão 'Voltar'
                stBtnVoltar_Moto.IsVisible = true;

                if (idStatusOrcamento == 1 && idStatusColeta != 10)  // IdStatus: 1 - Aceito  // IdStatus: 10 - Finalizada (coleta)
                {
                    btnRealizarColeta.IsVisible = true;
                }

                // Esconde a view 'Realizar Coleta'
                stRealizarColeta.IsVisible = false;

                // Mostra o filtro e a lista
                stFiltrarColetas_Moto.IsVisible = false;
                stListaMoto.IsVisible           = false;
            }
        }

        #endregion
        
        #region Btn - Avançar

        private async void BtnAvancar_Motorista_Clicked(object sender, SelectedItemChangedEventArgs e)
        {

        }

        #endregion

        #region Btn - Finalizar

        private async void BtnFinalizar_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            // Mostra e esconde GRID
            gridRealizar.IsVisible = false;
            gridValor.IsVisible    = true;

            lblValor_Final.Text    = value;
        }

        #endregion

        #region Btn - Realizar Cobrança

        private async void BtnRealizarCobranca_Motorista_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            await DisplayAlert("Sucesso!", "Cobrança realizada com sucesso!", "OK");

            // Esconde a view de 'Realizar Coleta'
            stRealizarColeta.IsVisible = false;

            slMotoristaEncontrarColeta_Lista.IsVisible = false;

            #region Atualiza o orçamento e a coleta (como finalizados)

            // Orçamento -------------------------------------------
            orcam.IdStatus = 9;  // IdStatus: 9 - Finalizado 

            await orcaControl.UpdateOrcamento(orcam, orcam.IdOrcamento);

            // Coleta -----------------------------------------------
            coleta.IdStatus = 9;

            await coletaControl.UpdateColeta(coleta);

            #endregion

            // Recarrega a página
            await Navigation.PushModalAsync(new Views.LgOrcamentos());
        }

        #endregion

        
        #region Btn - Retira

        private async void BtnRetirar_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            string retira = enderecoRetirada.Replace(" ", "+");

            Uri uri = new Uri(google + "/" + retira);

            await OpenBrowser(uri);
        }

        #endregion

        #region Btn - Entrega

        private async void BtnEntregar_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            string entrega = enderecoEntrega.Replace(" ", "+");  // Substitui os espaços

            Uri uri = new Uri(google + "/" + entrega);

            await OpenBrowser(uri);     // Abre o browser
        }

        #endregion


        #region Btn - Ligar

        private async void BtnLigarCliente(object sender, SelectedItemChangedEventArgs e)
        {

        }

        #endregion

        #region Btn - Desistir

        private async void BtnDesistir(object sender, SelectedItemChangedEventArgs e)
        {

        }

        #endregion
        
        #endregion
        
        #region OpenBrowser - Abre aplicativo externo (GoogleMaps)
        
        public async Task OpenBrowser(Uri uri)
        {
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        } 

        #endregion

        #endregion

        #region Mostra e esconde campos

        #region EscondeDados_Moto()

        public void EscondeDados_Moto()
        {
            #region Campos

            lbEndRet.IsVisible                   = false;
            etEndRet_Moto.IsVisible              = false;
            lbEndEnt.IsVisible                   = false;
            etEndEnt_Moto.IsVisible              = false;
            lbTipoMaterial.IsVisible             = false;
            lbTipoMaterial_Moto.IsVisible        = false;
            lbFragilidadeMaterial.IsVisible      = false;
            lbFragilidadeMaterial_Moto.IsVisible = false;
            lbDescricaoMaterial.IsVisible        = false;
            lbDescricaoMaterial_Moto.IsVisible   = false;

            #endregion
        }

        #endregion

        #region MostraDados_Moto()

        public void MostraDados_Moto()
        {
            #region Campos

            lbEndRet.IsVisible                   = true;
            etEndRet_Moto.IsVisible              = true;
            lbEndEnt.IsVisible                   = true;
            etEndEnt_Moto.IsVisible              = true;
            lbTipoMaterial.IsVisible             = true;
            lbTipoMaterial_Moto.IsVisible        = true;
            lbFragilidadeMaterial.IsVisible      = true;
            lbFragilidadeMaterial_Moto.IsVisible = true;
            lbDescricaoMaterial.IsVisible        = true;
            lbDescricaoMaterial_Moto.IsVisible   = true;

            #endregion
        }

        #endregion

        #region EscondeDados_2_Moto()

        public void EscondeDados_2_Moto()
        {
            #region Campos
            lbPeso.IsVisible         = false;
            etPeso_Moto.IsVisible    = false;
            lbPeso2.IsVisible        = false;
            lbVolume.IsVisible       = false;
            etVolume_Moto.IsVisible  = false;
            lbLargura.IsVisible      = false;
            etLargura_Moto.IsVisible = false;
            lbLargura2.IsVisible     = false;
            lbAltura.IsVisible       = false;
            etAltura_Moto.IsVisible  = false;
            lbAltura2.IsVisible      = false;

            lbDataMax.IsVisible      = false;
            etDataMax_Moto.IsVisible = false;
            lbHorario.IsVisible      = false;
            etHorario_Moto.IsVisible = false;
            lbValorPretendido.IsVisible      = false;
            etValorPretendidoRS_Moto.IsVisible = false;
            etValorPretendido_Moto.IsVisible = false;
            lbObservacoes.IsVisible          = false;
            etObservacoes_Moto.IsVisible     = false;
            lbTipoVeiculo.IsVisible          = false;
            etTipoVeiculo_Moto.IsVisible     = false;

            #endregion
        }

        #endregion

        #region MostraDados_2_Moto()

        public void MostraDados_2_Moto()
        {
            #region Campos
            lbPeso.IsVisible            = true;
            etPeso_Moto.IsVisible       = true;
            lbPeso2.IsVisible           = true;
            lbVolume.IsVisible          = true;
            etVolume_Moto.IsVisible     = true;
            lbLargura.IsVisible         = true;
            etLargura_Moto.IsVisible    = true;
            lbLargura2.IsVisible        = true;
            lbAltura.IsVisible          = true;
            etAltura_Moto.IsVisible     = true;
            lbAltura2.IsVisible         = true;
                                        
            lbDataMax.IsVisible              = true;
            etDataMax_Moto.IsVisible         = true;
            lbHorario.IsVisible              = true;
            etHorario_Moto.IsVisible         = true;
            lbValorPretendido.IsVisible      = true;
            etValorPretendidoRS_Moto.IsVisible = true;
            etValorPretendido_Moto.IsVisible = true;
            lbObservacoes.IsVisible          = true;
            etObservacoes_Moto.IsVisible     = true;
            lbTipoVeiculo.IsVisible          = true;
            etTipoVeiculo_Moto.IsVisible     = true;

            #endregion
        }

        #endregion

        #region EscondeOrcamento()

        public void EscondeOrcamento()
        {
            #region Campos
            
            lbStatusOrcamento.IsVisible       = false;
            etStatusOrcamento.IsVisible       = false;
            lbValorOrcamento.IsVisible        = false;
            etValor_Moto.IsVisible            = false;
            etValorRS_Moto.IsVisible          = false;
            lbObs_Orcamento.IsVisible         = false;
            etObservacoes_Orcamento.IsVisible = false;

            #endregion
        }

        #endregion

        #region MostraOrcamento()

        public void MostraOrcamento()
        {
            #region Campos
            
            lbStatusOrcamento.IsVisible       = true;
            etStatusOrcamento.IsVisible       = true;
            lbValorOrcamento.IsVisible        = true;
            etValor_Moto.IsVisible            = true;
            etValorRS_Moto.IsVisible          = true;
            lbObs_Orcamento.IsVisible         = true;
            etObservacoes_Orcamento.IsVisible = true;

            #endregion
        }

        #endregion

        #endregion

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

        #region Btn - Orçamentos 
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