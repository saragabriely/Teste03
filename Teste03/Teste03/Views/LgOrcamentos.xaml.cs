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

        #region Listas - Cliente

        #region Lista 01 - Coletas com orçamento

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

            idCol = orcamento.IdColeta;

            ListaColetas_Orcamento_(orcamento.IdColeta);

            stBtnVoltar_Cliente_.IsVisible = false;
        }
        #endregion

        #endregion

        #region Lista 02 - Orçamentos

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
                // Mostra a lista 02
                stListaMoto_02.IsVisible = true;

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

            // Captura as informações do motorista
            motorista = await motoristaController.GetMotorista(orcamento.IdMotorista);

            // Captura o nome do motorista
            cliMotorista = await clienteController.GetCliente(motorista.IdCliente);

            // Nome do motorista
            lblNomeMotorista_.Text = cliMotorista.Cnome;

            #region Popula

            // Popula as informações da coleta
            PopulaColeta_Cliente(coleta);

            // Popula Dados Orçamento
            PopulaOrcamento_Cliente(orcamento);

            #endregion

            // deseleciona o item do listview
            LstOrcamento_Motorista.SelectedItem = null;

            #region Esconde

            // Esconde a lista
            stListaMoto_02.IsVisible = false;

            stBtnVoltar_Cliente_.IsVisible = false;

            #endregion

            slClienteEncontrarColeta_Lista.IsVisible = true;

            // Mostra os campos
            MostraOrcamento_();

            lbExpandir.IsVisible = true;

            // Variáveis 
            idOrca              = orcamento.IdOrcamento;
            idStatusOrcamento   = orcamento.IdStatus;
            idStatusColeta      = coleta.IdStatus;
            value               = orcamento.Valor;

            // Concatena o endereço (nome da rua, numero e bairro)
            enderecoRetirada = coleta.EndRetEndereco + ", " + coleta.EndRetNumero + " - " + coleta.EndRetBairro;
            enderecoEntrega  = coleta.EndEntEndereco + ", " + coleta.EndEntNumero + " - " + coleta.EndEntBairro;

            // Mostra o botão 'Voltar'
            stBtnVoltar_Cliente.IsVisible = true;
            
        }
        #endregion

        private async void Teste_Btn(object sender, SelectedItemChangedEventArgs e)
        {
            await DisplayAlert("OK", "OOOKK", "OK");
        }

        #endregion

        #region Popula campos 

        #region PopulaOrcamento_Cliente(orcamento)

        private async void PopulaOrcamento_Cliente(Orcamento orcamento)
        {
            #region Busca status

            status = new Status();
            status = await statusController.GetStatus(orcamento.IdStatus);

            #endregion

            #region Campos

            etStatusOrcamento_C.Text       = status.DescricaoStatus.ToUpper();

            // Popula Dados Orçamento
            etValor_Cliente.Text = orcamento.Valor;
            etObservacoes_Orcamento_.Text = orcamento.Observacoes;

            #endregion
        }

        #endregion

        #region PopulaColeta_Cliente(coleta)

        private void PopulaColeta_Cliente(Coleta coleta)
        {
            #region Campos
            
            etEndRet_Cliente.Text              = coleta.EndRetEndereco;
            etEndEnt_Cliente.Text              = coleta.EndEntEndereco;
            lbTipoMaterial_Cliente.Text        = coleta.MatTipo;
            lbFragilidadeMaterial_Cliente.Text = coleta.MatFragilidade;
            lbDescricaoMaterial_Cliente.Text   = coleta.MatDescricao;
            etPeso_Cliente.Text                = coleta.MatPeso;
            etVolume_Cliente.Text              = coleta.MatVolume;
            etLargura_Cliente.Text             = coleta.MatLargura;
            etAltura_Cliente.Text              = coleta.MatAltura;
            etDataMax_Cliente.Text             = coleta.DataMaxima;
            etHorario_Cliente.Text             = coleta.HorarioLimite + ":" + coleta.HorarioLimite02;
            etValorPretendido_Cliente.Text     = coleta.ValorPretendido;
            etObservacoes_Cliente.Text         = coleta.Observacoes;
            etTipoVeiculo_Cliente.Text         = coleta.TipoVeiculo;
            #endregion
        }

        #endregion

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

        #endregion

        #region Botões

        #region BtnVoltar_Clente_Clicked
        private async void BtnVoltar_Clente_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            if (slClienteEncontrarColeta_Lista.IsVisible)                           // Volta para a lista de orçamento
            {
                // Esconde os campos
                slClienteEncontrarColeta_Lista.IsVisible = false;

                stListaMoto_02.IsVisible = true;

                // Esconde o botão 'Voltar'
                stBtnVoltar_Cliente.IsVisible = false;

                // Atualiza a lista de orçamentos
                ListaColetas_Orcamento_(idCol);
            }
            else                                                          // Volta para a primeira lista (coletas)
            {
                // Mostra

                stListaCliente.IsVisible = true;

                stListaMoto_02.IsVisible = false;

                //  Esconde
                stListaCliente.IsVisible       = false;
                lbColetaOrcamento.IsVisible    = false;
                stBtnVoltar_Cliente_.IsVisible = false;

                ListaColetas_Orcamento(idCliente);

                ListaColetas_Orcamento_(idCol);
            }
        }
        #endregion

        #endregion

        #region Aceite e Recusa 

        #region Recusar Orçamento

        private async void LblRecusarOrcamento(object sender, SelectedItemChangedEventArgs e)
        { 
            if (idStatusOrcamento == 1)      
            {
                await DisplayAlert("Aceito", "Este orçamento já foi aceito!", "OK");
            }
            else if(idStatusOrcamento == 14)
            {
                await DisplayAlert("Recusado", "Este orçamento já foi recusado!", "OK");
            }
            else if (await DisplayAlert("Recusa", "Deseja mesmo recusar este orçamento?", "OK", "Cancelar"))
            {                
                #region Atualiza o status do orçamento
                // Captura o objeto
                orcam = await orcaControl.GetOrcamento(idOrca);

                // Atualiza o status
                orcam.IdStatus = 14;  // 14 - Recusado

                // Atualiza o status no banco
                await orcaControl.UpdateOrcamento(orcam, idOrca);

                #endregion

                await DisplayAlert("Recusado", "Orçamento recusado com sucesso!", "OK");

                // Volta para os orçamentos da coleta em questão
                slClienteEncontrarColeta_Lista.IsVisible = false;

                // Atualiza a lista de orçamentos
                ListaColetas_Orcamento_(idCol);

                stListaMoto_02.IsVisible = true;
            }
        }

        #endregion

        #region AceitarRecusar(i)

        private void AceitarRecusar(int tipo) // Verifica o tipo de operação e faz o que for necessário
        {

        }

        #endregion


        #region Aceitar Orçamento

        private async void LblAceitarOrcamento(object sender, SelectedItemChangedEventArgs e)
        {
            if (idStatusOrcamento == 1)
            {
                await DisplayAlert("Aceito", "Este orçamento já foi aceito!", "OK");
            }
            else if (idStatusOrcamento == 14)
            {
                await DisplayAlert("Recusado", "Este orçamento já foi recusado!", "OK");
            }
            else if (await DisplayAlert("Aceite", "Deseja mesmo aceitar este orçamento?", "OK", "Cancelar"))
            {
                #region Atualiza status - Orçamento

                // Captura o objeto
                orcam = await orcaControl.GetOrcamento(idOrca);

                // Atualiza o status
                orcam.IdStatus = 1;

                // Atualiza o status no banco
                await orcaControl.UpdateOrcamento(orcam, idOrca);

                // Atualiza o status dos demais orçamentos como 'Recusado' (ID: 14)
                AtualizaStatus(orcam.IdColeta, 14, orcam.IdOrcamento);


                #endregion

                await DisplayAlert("Aceito!", "Orçamento aceito com sucesso!", "OK");

                // Volta para os orçamentos da coleta em questão
                slClienteEncontrarColeta_Lista.IsVisible = false;

                // Atualiza a lista de orçamentos
                ListaColetas_Orcamento_(idCol);

                stListaMoto_02.IsVisible = true;
            }
        }

        #endregion

        #region Aceite - Atualizar os demais orçamentos

        private async void AtualizaStatus(int idColeta, int idStatus, int idOrcaAceito)
        {
            // Assim que um orçamento é aceito, os demais orçamentos (relacionados a coleta em questão)
            // deverão ser recusados automaticamente

            await orcaControl.GetRecusaOrcamentos(idColeta, idOrcaAceito, idStatus);
            
        }

        #endregion



        #endregion

        #region Mostra e esconde campos

        #region VerDetalhes

        private void LblVerDetalhes(object sender, SelectedItemChangedEventArgs e)
        {
            MostraDados_();

            // Esconde a opção 'Ver detalhes da coleta'
            lbExpandir.IsVisible = false;

            // Mostra a opção 'Esconder detalhes da coleta'
            lbEsconder.IsVisible = true;
        }

        #endregion

        #region EsconderDetalhes

        private void LblEsconderDetalhes(object sender, SelectedItemChangedEventArgs e)
        {
            EscondeDados_Cliente();

            // Esconde a opção 'Ver detalhes da coleta'
            lbExpandir.IsVisible = true;

            // Mostra a opção 'Esconder detalhes da coleta'
            lbEsconder.IsVisible = false;
        }

        #endregion


        #region EscondeOrcamento_Cliente()

        public void EscondeOrcamento_Cliente()
        {
            #region Orçamento

            lbStatusOrcamento_C.IsVisible      = false;
            etStatusOrcamento_C.IsVisible      = false;
            lbValorOrcamento_.IsVisible        = false;
            etValor_Cliente.IsVisible          = false;
            etValorRS_Cliente.IsVisible        = false;
            lbObs_Orcamento_.IsVisible         = false;
            etObservacoes_Orcamento_.IsVisible = false;
            lbNomeMotorista.IsVisible          = false;
            lblNomeMotorista_.IsVisible        = false;
            lbQtdeMotorista.IsVisible          = false;
            lblQtdeMotorista_.IsVisible        = false;

            #endregion
        }

        #endregion

        #region EscondeDados_cliente()

        public void EscondeDados_Cliente()
        {
            #region Campos

            #region Endereços e descrição do material

            lbEndRet_.IsVisible                     = false;
            etEndRet_Cliente.IsVisible              = false;
            lbEndEnt_.IsVisible                     = false;
            etEndEnt_Cliente.IsVisible              = false;
            lbTipoMaterial_.IsVisible               = false;
            lbTipoMaterial_Cliente.IsVisible        = false;
            lbFragilidadeMaterial_.IsVisible        = false;
            lbFragilidadeMaterial_Cliente.IsVisible = false;
            lbDescricaoMaterial_.IsVisible          = false;
            lbDescricaoMaterial_Cliente.IsVisible   = false;

            #endregion

            #region Peso e dimensões

            lbPeso_.IsVisible           = false;
            etPeso_Cliente.IsVisible    = false;
            lbPeso2_.IsVisible          = false;
            lbVolume_.IsVisible         = false;
            etVolume_Cliente.IsVisible  = false;
            lbLargura_.IsVisible        = false;
            etLargura_Cliente.IsVisible = false;
            lbLargura2_.IsVisible       = false;
            lbAltura_.IsVisible         = false;
            etAltura_Cliente.IsVisible  = false;
            lbAltura2_.IsVisible        = false;

            #endregion

            #region Data, valor, obs e tipo de veiculo 

            lbDataMax_.IsVisible                  = false;
            etDataMax_Cliente.IsVisible           = false;
            lbHorario_.IsVisible                  = false;
            etHorario_Cliente.IsVisible           = false;
            lbValorPretendido_.IsVisible          = false;
            etValorPretendidoRS_Cliente.IsVisible = false;
            etValorPretendido_Cliente.IsVisible   = false;
            lbObservacoes_.IsVisible              = false;
            etObservacoes_Cliente.IsVisible       = false;
            lbTipoVeiculo_.IsVisible              = false;
            etTipoVeiculo_Cliente.IsVisible       = false;

            #endregion
            
            #endregion
        }

        #endregion


        #region MostraDados_()

        public void MostraDados_()
        {
            #region Campos
            
            #region Endereços e descrição do material

            lbEndRet_.IsVisible                     = true;
            etEndRet_Cliente.IsVisible              = true;
            lbEndEnt_.IsVisible                     = true;
            etEndEnt_Cliente.IsVisible              = true;
            lbTipoMaterial_.IsVisible               = true;
            lbTipoMaterial_Cliente.IsVisible        = true;
            lbFragilidadeMaterial_.IsVisible        = true;
            lbFragilidadeMaterial_Cliente.IsVisible = true;
            lbDescricaoMaterial_.IsVisible          = true;
            lbDescricaoMaterial_Cliente.IsVisible   = true;

            #endregion

            #region Dimensões

            lbPeso_.IsVisible            = true;
            etPeso_Cliente.IsVisible     = true;
            lbPeso2_.IsVisible           = true;
            lbVolume_.IsVisible          = true;
            etVolume_Cliente.IsVisible   = true;
            lbLargura_.IsVisible         = true;
            etLargura_Cliente.IsVisible  = true;
            lbLargura2_.IsVisible        = true;
            lbAltura_.IsVisible          = true;
            etAltura_Cliente.IsVisible   = true;
            lbAltura2_.IsVisible         = true;

            #endregion

            #region Data, valor, obs e tipo veiculo

            lbDataMax_.IsVisible                  = true;
            etDataMax_Cliente.IsVisible           = true;
            lbHorario_.IsVisible                  = true;
            etHorario_Cliente.IsVisible           = true;
            lbValorPretendido_.IsVisible          = true;
            etValorPretendidoRS_Cliente.IsVisible = true;
            etValorPretendido_Cliente.IsVisible   = true;
            lbObservacoes_.IsVisible              = true;
            etObservacoes_Cliente.IsVisible       = true;
            lbTipoVeiculo_.IsVisible              = true;
            etTipoVeiculo_Cliente.IsVisible       = true;

            #endregion

            #endregion
        }

        #endregion
                
        #region MostraOrcamento_()

        public void MostraOrcamento_()
        {
            #region Orçamento

            lbStatusOrcamento_C.IsVisible      = true;
            etStatusOrcamento_C.IsVisible      = true;
            lbValorOrcamento_.IsVisible        = true;
            etValor_Cliente.IsVisible          = true;
            etValorRS_Cliente.IsVisible        = true;
            lbObs_Orcamento_.IsVisible         = true;
            etObservacoes_Orcamento_.IsVisible = true;
            lbNomeMotorista.IsVisible          = true;
            lblNomeMotorista_.IsVisible        = true;
            lbQtdeMotorista.IsVisible          = true;
            lblQtdeMotorista_.IsVisible        = true;

            #endregion
        }

        #endregion

        #endregion

        #endregion

        //----------------------------------------------------------------------------------------------------------

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