using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using Teste03.Models;
using Teste03.Controllers;

namespace Teste03.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LgColetas : ContentPage
	{
        #region Variaveis e controllers

        public int idCliente     = 8; // 3; //  Session.Instance.IdCliente;
        public int idTipoCliente = 3; // Session.Instance.IdTipoUsuario;
        public int verificaOperacao;
        public int idCol;
        public int verifica;

        public int idMotorista   = 1; // Session.Instance.IdMotorista;
        public int idColetaCliente;
        public int idColetaOrcamento;

        private List<Coleta> coletas;

        ColetaController coletaControl = new ColetaController();
        
        #endregion

        
        public LgColetas ()
		{ 
			InitializeComponent ();
            
            idTipoCliente = 3;

            #region Verifica o tipo de usuário
            if (idTipoCliente == 2)
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

        #region Cliente

        #region Lista

        #region ListaAsync(int i)
        public async void ListaAsync(int i)
        {
            List<Coleta> _list;// = await coletaControl.GetListColeta(idCliente);

            _list = null;
            
            if(i == 0)                              // TODAS AS COLETAS
            {
                 _list = await coletaControl.GetListColeta(idCliente);

                if (_list == null)
                {
                    LstColeta.IsVisible = false;

                    //  lbListaVazia.IsVisible = true;
                }
                else
                {
                    LstColeta.ItemsSource = _list;
                }
            }
            else if(i == 10)                        // COLETAS FINALIZADAS
            {
                _list = await coletaControl.GetListColetaEsp(idCliente, 10);

                LstColeta.ItemsSource = _list;
            }
            else if (i == 12)                        // COLETAS PENDENTES
            {
                _list = await coletaControl.GetListColetaEsp(idCliente, 12);

                LstColeta.ItemsSource = _list;
            }
            else if (i == 6)                        // COLETAS CANCELADAS
            {
                _list = await coletaControl.GetListColetaEsp(idCliente, 6);

                LstColeta.ItemsSource = _list;
            }
            else if (i == 8)                        // COLETAS EM ANDAMENTO
            {
                _list = await coletaControl.GetListColetaEsp(idCliente, 8);

                LstColeta.ItemsSource = _list;
            }
        }
        #endregion

        #region Lista de coletas - Item selecionado
        private async void LstColeta_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            // obtem o item do listview
            var coleta = e.SelectedItem as Coleta;

            // deseleciona o item do listview
            LstColeta.SelectedItem = null;

            // Popula os campos como o objeto retornado
            Popula(coleta);

            idCol = coleta.IdColeta;
        }
        #endregion

        #region Filtro - Coleta - Cliente
        private void pckFiltroColeta_Cliente(object sender, EventArgs e)
        {
            int idStatus = 0;
            var itemSelecionado = etClienteFiltroColeta.Items[etClienteFiltroColeta.SelectedIndex];

            if(itemSelecionado.Equals("TODAS AS COLETAS"))
            {
                idStatus = 0;
            }
            else if (itemSelecionado.Equals("COLETAS FINALIZADAS"))
            {
                idStatus = 10;
            }
            else if (itemSelecionado.Equals("COLETAS PENDENTES"))
            {
                idStatus = 12;
            }
            else if (itemSelecionado.Equals("COLETAS CANCELADAS"))
            {
                idStatus = 6;
            }
            else if (itemSelecionado.Equals("COLETAS EM ANDAMENTO"))
            {
                idStatus = 8;
            }
            
            ListaAsync(idStatus);
        }
        #endregion

        #endregion

        #region Popula os campos com dados do campo

        public void Popula(Coleta coleta)
        {
            if(coleta != null)
            {
                stLista.IsVisible = false;

                DadosNotEnabled();

                #region Popula os campos

                #region Material
                etTipoMaterial.Text         = coleta.MatTipo;      
                etFragilidadeMaterial.Text  = coleta.MatFragilidade;       
                etDescricaoMaterial.Text    = coleta.MatDescricao; 
                etPeso.Text                 = coleta.MatPeso;              
                etVolume.Text               = coleta.MatVolume;            
                etLargura.Text              = coleta.MatLargura;           
                etAltura.Text               = coleta.MatAltura;
                #endregion

                #region Endereço de retirada
                etEndRetCep.Text            = coleta.EndRetCep;            
                etEndRetUf.Text             = coleta.EndRetUf;             
                etEndRet.Text               = coleta.EndRetEndereco;               
                etEndRetNumero.Text         = coleta.EndRetNumero.ToString();         
                etEndRetCompl.Text          = coleta.EndRetComplemento;          
                etEndRetBairro.Text         = coleta.EndRetBairro;         
                etEndRetCidade.Text         = coleta.EndRetCidade;         
                etEndRetResponsavel.Text    = coleta.EndRetNomeResponsavel;
                etEndRetResponsavelTel.Text = coleta.EndRetRespTelefone;
                #endregion

                #region Endereço de entrega
                etEndEntCep.Text            = coleta.EndEntCep;           
                etEndEntUf.Text             = coleta.EndEntUf;            
                etEndEnt.Text               = coleta.EndEntEndereco;              
                etEndRetNumero.Text         = coleta.EndEntNumero.ToString();        
                etEndEntCompl.Text          = coleta.EndEntComplemento;         
                etEndEntBairro.Text         = coleta.EndEntBairro;        
                etEndEntCidade.Text         = coleta.EndEntCidade;        
                etEndEntResponsavel.Text    = coleta.EndEntNomeResponsavel;   
                etEndEntResponsavelTel.Text = coleta.EndEntRespTelefone;
                #endregion

                #region Datas e horários
                etDataMax.Text          = coleta.DataMaxima;
                etHorario.Text          = coleta.HorarioLimite;
                etHorario2.Text         = coleta.HorarioLimite02;       
                etValorPretendido.Text  = coleta.ValorPretendido;
                etObservacoes.Text      = coleta.Observacoes;
                etApelido.Text          = coleta.ApelidoColeta;
                etTipoVeiculo.Text      = coleta.TipoVeiculo;
                #endregion

                #endregion

                slClienteCadastrarColeta.IsVisible = true;

                stFiltrarColetas.IsVisible = false;

                DadosMaterialVisible();
                
                btnAvancar.IsVisible      = false;
                stBtnVoltar.IsVisible     = true;
                stBtnAvancar.IsVisible    = true;
                slEditarColeta.IsVisible  = true;
                slExcluirColeta.IsVisible = true;

                idCol = coleta.IdColeta;
            }
        }

        #endregion

        #region Bloqueia e libera os campos

        #region DadosNotEnabled()

        public void DadosNotEnabled()
        {
            #region Enabled = true
            
            #region Material
            etTipoMaterial.IsEnabled         = false;
            etFragilidadeMaterial.IsEnabled  = false;      
            etDescricaoMaterial.IsEnabled    = false;
            etPeso.IsEnabled                 = false;      
            etVolume.IsEnabled               = false;      
            etLargura.IsEnabled              = false;      
            etAltura.IsEnabled               = false;
            #endregion

            #region Endereço de retirada
            etEndRetCep.IsEnabled            = false;
            etEndRetUf.IsEnabled             = false;
            etEndRet.IsEnabled               = false;      
            etEndRetNumero.IsEnabled         = false;         
            etEndRetCompl.IsEnabled          = false;    
            etEndRetBairro.IsEnabled         = false;
            etEndRetCidade.IsEnabled         = false;
            etEndRetResponsavel.IsEnabled    = false;
            etEndRetResponsavelTel.IsEnabled = false;
            #endregion                   

            #region Endereço de entrega
            etEndEntCep.IsEnabled            = false;
            etEndEntUf.IsEnabled             = false;
            etEndEnt.IsEnabled               = false;      
            etEndRetNumero.IsEnabled         = false;       
            etEndEntCompl.IsEnabled          = false;    
            etEndEntBairro.IsEnabled         = false;
            etEndEntCidade.IsEnabled         = false;
            etEndEntResponsavel.IsEnabled    = false;
            etEndEntResponsavelTel.IsEnabled = false;
            #endregion

            #region Datas e horários
            etDataMax.IsEnabled          = false;
            etHorario.IsEnabled          = false;
            etHorario2.IsEnabled         = false;      
            etValorPretendido.IsEnabled  = false;
            etObservacoes.IsEnabled      = false;
            etApelido.IsEnabled          = false;
            etTipoVeiculo.IsEnabled      = false;
            #endregion

            #endregion
        }

        #endregion

        #region DadosEnabled()

        public void DadosEnabled()
        {
            #region Enabled = true
            
            #region Material
            etTipoMaterial.IsEnabled         = true;
            etFragilidadeMaterial.IsEnabled  = true;      
            etDescricaoMaterial.IsEnabled    = true;
            etPeso.IsEnabled                 = true;      
            etVolume.IsEnabled               = true;      
            etLargura.IsEnabled              = true;      
            etAltura.IsEnabled               = true;
            #endregion

            #region Endereço de retirada
            etEndRetCep.IsEnabled            = true;
            etEndRetUf.IsEnabled             = true;
            etEndRet.IsEnabled               = true;      
            etEndRetNumero.IsEnabled         = true;         
            etEndRetCompl.IsEnabled          = true;    
            etEndRetBairro.IsEnabled         = true;
            etEndRetCidade.IsEnabled         = true;
            etEndRetResponsavel.IsEnabled    = true;
            etEndRetResponsavelTel.IsEnabled = true;
            #endregion                   

            #region Endereço de entrega
            etEndEntCep.IsEnabled            = true;
            etEndEntUf.IsEnabled             = true;
            etEndEnt.IsEnabled               = true;      
            etEndRetNumero.IsEnabled         = true;       
            etEndEntCompl.IsEnabled          = true;    
            etEndEntBairro.IsEnabled         = true;
            etEndEntCidade.IsEnabled         = true;
            etEndEntResponsavel.IsEnabled    = true;
            etEndEntResponsavelTel.IsEnabled = true;
            #endregion

            #region Datas e horários
            etDataMax.IsEnabled          = true;
            etHorario.IsEnabled          = true;
            etHorario2.IsEnabled         = true;      
            etValorPretendido.IsEnabled  = true;
            etObservacoes.IsEnabled      = true;
            etApelido.IsEnabled          = true;
            etTipoVeiculo.IsEnabled      = true;
            #endregion

            #endregion
        }

        #endregion

        #endregion

        #region Botões

        #region Btn - Cadastrar Coleta
        private void BtnCadastrarColeta_Clicked(object sender, EventArgs e)
        {
            // Esconde a parte de opções 'Cadastrar' / 'Pesquisar'
            stBtnCliente.IsVisible = false;

            // Mostra os campos para cadastro
            slClienteCadastrarColeta.IsVisible = true;
        }
        #endregion

        #region Btn - Pesquisar Coleta
        private void BtnPesquisarColeta_Clicked(object sender, EventArgs e)
        {
            stBtnCliente.IsVisible = false;

            stFiltrarColetas.IsVisible = true;
            
            stLista.IsVisible          = true;

            ListaAsync(0);  // Gera  a lista de coletas
        }
        #endregion

        #region Btn - Excluir Coleta
        private async void BtnExcluirColeta_Clicked(object sender, EventArgs e)
        {
            if(await DisplayAlert("Excluir", "Deseja mesmo excluir esta coleta?", "OK", "Cancelar"))
            {
                await coletaControl.DeleteColeta(idCol);

                await DisplayAlert("Excluído", "Coleta excluída com sucesso!", "OK");

                await Navigation.PushModalAsync(new Views.LgVeiculos());
            }
        }
        #endregion

        #region Btn - Editar Coleta
        private void BtnEditarColeta_Clicked(object sender, EventArgs e)
        {
            // Libera os campos para serem alterados
            DadosEnabled();

            #region Botões - Mostrar e esconder

            slEditarColeta.IsVisible  = false;        // EDITAR
            slExcluirColeta.IsVisible = false;        // EXCLUIR

            btnAvancar.IsVisible = true;                // AVANÇAR

            stBtnVoltar.IsVisible  = false;              // VOLTAR
            stBtnAvancar.IsVisible = false;              // AVANÇAR

            verificaOperacao = 2;

            #endregion
        }
        #endregion

        #region Btn - Voltar - Busca
        private async void BtnVoltar2_Clicked(object sender, EventArgs e)
        {
            #region Comentário
            /*
            if (lblDescricaoMaterial.IsVisible && etDescricaoMaterial.IsEnabled)
            {
                if ((await DisplayAlert("Deseja sair?", "Tem certeza que deseja sair? Todos os dados serão perdidos.",
                    "OK", "Cancelar")))
                {
                    stLista.IsVisible = true;
                    slClienteCadastrarColeta.IsVisible = false;
                    slEditarColeta.IsVisible = false;
                    slExcluirColeta.IsVisible = false;

                    DadosMaterialNotVisible();
                }
            }
            else */
            #endregion

            if (lblDescricaoMaterial.IsVisible)
            {
                stLista.IsVisible                  = true;
                stFiltrarColetas.IsVisible         = true;
                slClienteCadastrarColeta.IsVisible = false;
                slEditarColeta.IsVisible           = false;
                slExcluirColeta.IsVisible          = false;

                DadosMaterialNotVisible();
            }
            else if (lblEndRet.IsVisible)
            {
                // Deixa os botões visiveis
                slEditarColeta.IsVisible = true;
                slExcluirColeta.IsVisible = true;
                stBtnAvancar.IsVisible = true;

                EnderecoRetiradaNotVisible();

                DadosMaterialVisible();
            }
            else if (lblEndEnt.IsVisible)
            {
                // Deixa os botões visiveis
                slEditarColeta.IsVisible  = true;
                slExcluirColeta.IsVisible = true;
                stBtnAvancar.IsVisible    = true;

                EnderecoEntregaNotVisible();

                EnderecoRetiradaVisible();
            }
            else if (lblValorPretendido.IsVisible)
            {
                // Deixa os botões visiveis
                slEditarColeta.IsVisible  = true;
                slExcluirColeta.IsVisible = true;
                stBtnAvancar.IsVisible    = true;
                btnSalvar.IsVisible       = false;

                ValorNotVisible();

                EnderecoEntregaVisible();
            }
        }
        #endregion

        #region Btn - Avançar - Buscar
        private void BtnAvancar2_Clicked(object sender, EventArgs e)
        {
            slEditarColeta.IsVisible  = true;
            slExcluirColeta.IsVisible = true;

            if (lblDescricaoMaterial.IsVisible)
            {
                DadosMaterialNotVisible();

                EnderecoRetiradaVisible();
            }
            else if (lblEndRet.IsVisible)
            {
                EnderecoRetiradaNotVisible();

                EnderecoEntregaVisible();
            }
            else if (lblEndEnt.IsVisible)
            {
                EnderecoEntregaNotVisible();

                ValorVisible();

                stBtnAvancar.IsVisible = false;
            }

        }
        #endregion


        #region Btn - Avançar
        private async void BtnAvancar_Clicked(object sender, EventArgs e)
        {
            if (verificaOperacao == 2)         // UPDATE
            {
                verificaOperacao = 2;
                
                await VerificaCamposAsync(2);
            }
            else                              // INSERT
            {
                await VerificaCamposAsync(1);
            }
        }
        #endregion

        #region Btn - Finalizar
        private async void BtnFinalizar_Clicked(object sender, EventArgs e)
        {
            await VerificaCamposAsync(1);

        }
        #endregion

        #region Btn - Voltar - Cadastro
        private void BtnVoltar_Clicked(object sender, EventArgs e)
        {
            if (etEndRetCep.IsVisible)
            {
                EnderecoRetiradaNotVisible();
                DadosMaterialVisible();
                btnAvancar2.IsVisible      = false;
                stBtnAvancar2.IsVisible    = false;
                btnVoltar.IsVisible        = false;
                btnAvancar.IsVisible       = true;
                btnDadosMaterial.IsEnabled = true;

                slEditarColeta.IsVisible  = false;
                slExcluirColeta.IsVisible = false;

            }
            else if (etEndEntCep.IsVisible)
            {
                EnderecoEntregaNotVisible();
                EnderecoRetiradaVisible();

                slEditarColeta.IsVisible  = false;
                slExcluirColeta.IsVisible = false;
            }
            else if (etDataMax.IsVisible)
            {
                ValorNotVisible();
                EnderecoEntregaVisible();
                btnFinalizar.IsVisible = false;
                btnAvancar2.IsVisible  = true;

                slEditarColeta.IsVisible  = false;
                slExcluirColeta.IsVisible = false;
            }
        }
        #endregion

        #endregion

        #region VerificaCampos(int i)

        private async Task VerificaCamposAsync(int i)
        {
            #region Mensagens de retorno
            string vazio        = "Campo vazio";       
            string nulo         = "Preencha o campo: ";
            string finalizado   = "Cadastro finalizado com sucesso!";
            string dadosInsuf   = "Dados insuficientes no campo: ";
            string caracter     = "': este campos não aceita caracteres especiais.";
            string caracterInv  = "Caracteres inválidos";
            #endregion

            #region Objetos e Controllers

            Coleta   coleta;
            Material material;

            ColetaController   coletaControl = new ColetaController();
            MaterialController matControl    = new MaterialController();

            #endregion

            #region Menu superior - Enabled

            if(verificaOperacao == 2)
            {
                btnDadosMaterial.IsEnabled    = true;
                btnEnderecoRetirada.IsEnabled = true;
                btnEnderecoEntrega.IsEnabled  = true;
                btnValor.IsEnabled            = true;
            }

            #endregion

            #region Variáveis

            int idStatus = 2;  // Status 2 - Aguardando Orçamento

            #region Material
            string tipoMaterial      = etTipoMaterial.Text;
            string fragilidade       = etFragilidadeMaterial.Text;
            string descricaoMaterial = etDescricaoMaterial.Text;
            string peso              = etPeso.Text;
            string volume            = etVolume.Text;
            string largura           = etLargura.Text;
            string altura            = etAltura.Text;
            #endregion

            #region Endereço de retirada
            string endRetCep            = etEndRetCep.Text;
            string endRetUf             = etEndRetUf.Text;
            string endRet               = etEndRet.Text;
            int    endRetNumero         = Convert.ToInt32(etEndRetNumero.Text);
            string endRetCompl          = etEndRetCompl.Text;
            string endRetBairro         = etEndRetBairro.Text;
            string endRetCidade         = etEndRetCidade.Text;
            string endRetResponsavel    = etEndRetResponsavel.Text;
            string endRetResponsavelTel = etEndRetResponsavelTel.Text;
            #endregion

            #region Endereço de entrega
            string endEntCep            = etEndEntCep.Text;
            string endEntUf             = etEndEntUf.Text;
            string endEnt               = etEndEnt.Text;
            int    endEntNumero         = Convert.ToInt32(etEndRetNumero.Text);
            string endEntCompl          = etEndEntCompl.Text;
            string endEntBairro         = etEndEntBairro.Text;
            string endEntCidade         = etEndEntCidade.Text;
            string endEntResponsavel    = etEndEntResponsavel.Text;
            string endEntResponsavelTel = etEndEntResponsavelTel.Text;
            #endregion

            #region Datas e horários
            string dataMax         = etDataMax.Text;
            string horario         = etHorario.Text;
            string horario2        = etHorario2.Text;
            string valorPretendido = etValorPretendido.Text;
            string observacoes     = etObservacoes.Text;
            string apelido         = etApelido.Text;
            string tipoVeiculo     = etTipoVeiculo.Text;
            #endregion

            #endregion

            try
            {
                //      INSERT                                                      UPDATE
                if ((btnDadosMaterial.IsEnabled && etTipoMaterial.IsVisible) || (verificaOperacao == 2 && lblDescricaoMaterial.IsVisible))
                {
                    #region Descrição do Material

                    if (string.IsNullOrEmpty(tipoMaterial))                     // TIPO DE MATERIAL
                    {
                        await DisplayAlert(vazio, nulo + lblTipoMaterial.Text, "OK");
                    }

                    else if (string.IsNullOrEmpty(fragilidade))                 // FRAGILIDADE
                    {
                        await DisplayAlert(vazio, nulo + lblFragilidadeMaterial.Text, "OK");
                    }

                    else if (string.IsNullOrEmpty(descricaoMaterial))    // DESCRIÇÃO DO MATERIAL
                    {
                        await DisplayAlert(vazio, nulo + lblDescricaoMaterial.Text, "OK");
                    }

                    else if (string.IsNullOrEmpty(peso))                 // PESO
                    {
                        await DisplayAlert(vazio, nulo + lblPeso.Text, "OK");
                    }

                    else if (string.IsNullOrEmpty(volume))               // VOLUME
                    {
                        await DisplayAlert(vazio, nulo + lblVolume.Text, "OK");
                    }

                    else if (string.IsNullOrEmpty(largura))              // LARGURA
                    {
                        await DisplayAlert(vazio, nulo + lblLargura.Text, "OK");
                    }

                    else if (string.IsNullOrEmpty(altura))               // ALTURA
                    {
                        await DisplayAlert(vazio, nulo + lblAltura.Text, "OK");
                    }

                    #endregion

                    else
                    {
                        DadosMaterialNotVisible(); // Esconde os campos 

                        btnEnderecoRetirada.IsEnabled = true;

                        btnAvancar.IsVisible    = false;
                        stBtnAvancar2.IsVisible = true;
                        btnVoltar.IsVisible     = true;

                        EnderecoRetiradaVisible();
                    }
                }
                //          INSERT                                                      UPDATE
                else if ((btnEnderecoRetirada.IsEnabled && etEndRetCep.IsVisible) || (verificaOperacao == 2 &&  lblEndRet.IsVisible))
                {
                    #region Endereço de retirada 

                    if (string.IsNullOrEmpty(endRetCep))
                    {
                        await DisplayAlert("Campo obrigatório", nulo + lblEndRetCep.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endRetUf))
                    {
                        await DisplayAlert(vazio, nulo + lblEndRetUf.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endRet))
                    {
                        await DisplayAlert(vazio, nulo + lblEndRet.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endRetNumero.ToString()))
                    {
                        await DisplayAlert(vazio, nulo + lblEndRetNumero.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endRetCompl))
                    {
                        await DisplayAlert(vazio, nulo + lblEndRetCompl.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endRetBairro))
                    {
                        await DisplayAlert(vazio, nulo + lblEndRetBairro.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endRetCidade))
                    {
                        await DisplayAlert(vazio, nulo + lblEndRetCidade.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endRetResponsavel))
                    {
                        await DisplayAlert(vazio, nulo + lblEndRetResponsavel.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endRetResponsavelTel))
                    {
                        await DisplayAlert(vazio, nulo + lblEndRetResponsavelTel.Text, "OK");
                    }

                    #endregion

                    else
                    {
                        EnderecoRetiradaNotVisible(); // Esconde os campos

                        btnEnderecoEntrega.IsEnabled = true;

                        EnderecoEntregaVisible();
                    }
                }
                else if ((btnEnderecoEntrega.IsEnabled && etEndEntCep.IsVisible) || (verificaOperacao == 2 && lblEndEnt.IsVisible) )
                {
                    #region Endereço de entrega
                    if (string.IsNullOrEmpty(endEntCep))
                    {
                        await DisplayAlert(vazio, nulo + lblEndEntCep.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endRetUf))
                    {
                        await DisplayAlert(vazio, nulo + lblEndEntUf.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endEnt))
                    {
                        await DisplayAlert(vazio, nulo + lblEndEnt.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endEntNumero.ToString()))
                    {
                        await DisplayAlert(vazio, nulo + lblEndEntNumero.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(etEndEntCompl.Text))
                    {
                        await DisplayAlert(vazio, nulo + lblEndEntCompl.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endEntBairro))
                    {
                        await DisplayAlert(vazio, nulo + lblEndEntBairro.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endEntCidade))
                    {
                        await DisplayAlert(vazio, nulo + lblEndEntCidade.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endEntResponsavel))
                    {
                        await DisplayAlert(vazio, nulo + lblEndEntResponsavel.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(endEntResponsavelTel))
                    {
                        await DisplayAlert(vazio, nulo + lblEndEntResponsavelTel.Text, "OK");
                    }
                    #endregion

                    else
                    {
                        EnderecoEntregaNotVisible(); // Esconde os campos 

                        btnAvancar2.IsVisible   = false;
                        stBtnAvancar2.IsVisible = false;

                        if (verificaOperacao != 2)
                        {
                            btnFinalizar.IsVisible  = true;                        }
                        else
                        {
                            btnSalvar.IsVisible = true;
                        }

                        ValorVisible();
                    }
                }
                else if ((btnValor.IsEnabled && etDataMax.IsVisible) || (verificaOperacao == 2 && lblValorPretendido.IsVisible))
                {
                    #region Informações complementares
                    if (string.IsNullOrEmpty(dataMax))
                    {
                        await DisplayAlert(vazio, nulo + lblDataMax.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(horario))
                    {
                        await DisplayAlert(vazio, nulo + lblHorario.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(horario2))
                    {
                        await DisplayAlert(vazio, nulo + lblHorario.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(valorPretendido))
                    {
                        await DisplayAlert(vazio, nulo + lblValorPretendido.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(observacoes))
                    {
                        await DisplayAlert(vazio, nulo + lblObservacoes.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(apelido))
                    {
                        await DisplayAlert(vazio, nulo + lblApelido.Text, "OK");
                    }
                    else if (string.IsNullOrEmpty(tipoVeiculo))
                    {
                        await DisplayAlert(vazio, nulo + lblTipoVeiculo.Text, "OK");
                    }
                    #endregion

                    else
                    {
                        ValorNotVisible();

                        // COLETA ---------------------------------------

                        #region Coleta () 

                        coleta = new Coleta()
                            {                                
                                EndRetCep             = endRetCep,
                                EndRetUf              = endRetUf,
                                EndRetEndereco        = endRet,
                                EndRetNumero          = endRetNumero,
                                EndRetComplemento     = endRetCompl,
                                EndRetBairro          = endRetBairro,
                                EndRetCidade          = endRetCidade,
                                EndRetNomeResponsavel = endRetResponsavel,
                                EndRetRespTelefone    = endRetResponsavelTel,
                                EndEntCep             = endEntCep,
                                EndEntUf              = endEntUf,
                                EndEntEndereco        = endEnt,
                                EndEntNumero          = endEntNumero,
                                EndEntComplemento     = endEntCompl,
                                EndEntBairro          = endEntBairro,
                                EndEntCidade          = endEntCidade,
                                EndEntNomeResponsavel = endEntResponsavel,
                                EndEntRespTelefone    = endEntResponsavelTel,
                                DataMaxima            = dataMax,
                                HorarioLimite         = horario,
                                ValorPretendido       = valorPretendido,
                                Observacoes           = observacoes,
                                ApelidoColeta         = apelido,
                                IdStatus              = idStatus,
                                MatTipo               = tipoMaterial,
                                MatFragilidade        = fragilidade,
                                MatDescricao          = descricaoMaterial,
                                MatPeso               = peso,
                                MatVolume             = volume,
                                MatAltura             = altura,
                                MatLargura            = largura,
                                DataCadastro          = DateTime.Now,
                                UltimaAtualizacao     = null,
                                IdCliente             = idCliente,
                                HorarioLimite02       = horario2,
                                TipoVeiculo           = tipoVeiculo
                            };

                            #endregion
                        
                        #region INSERT

                        if (i == 1)          
                        {
                            await coletaControl.PostColetaAsync(coleta);

                            await DisplayAlert("Finalizado", finalizado, "OK");
                            
                            await Navigation.PushModalAsync(new Views.LgColetas());
                        }
                        #endregion

                        #region UPDATE 

                        else if (i == 2)    // UPDATE
                        {
                            coleta.IdColeta = idCol;

                            await coletaControl.UpdateColeta(coleta);

                            await DisplayAlert("Finalizado", finalizado, "OK");

                            await Navigation.PushModalAsync(new Views.LgColetas());
                        }

                        #endregion

                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Source != null)
                    Console.WriteLine("Exception source: {0}", ex.Source);
                throw;
            }
        }


        #endregion


        #endregion

        #region Motorista

        #region Botões 

        #region Btn - Pesquisar Coleta
        private void  BtnMotoristaPesquisarColeta_Clicked(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region Btn - Encontrar Coleta
        private void BtnEncontrarColeta_Clicked(object sender, EventArgs e)
        {
            #region Lista - Encontrar

            List<string> lstEncontrar = new List<string>
            {
                "COLETAS DISPONÍVEIS",
                "COLETAS VISUALIZADAS",
                "COLETAS QUE ENVIEI ORÇAMENTO"
            };

            #endregion

            #region Esconde botões e views
           // slMotoristaPesquisarColetas.IsVisible = false;

            slMotoristaEncontrarColetas.IsVisible = true;

            btnsMotorista.IsVisible               = false;
            #endregion

            #region Motra botões e views
            etMotoristaFiltroColeta.ItemsSource   = lstEncontrar;
            etMotoristaFiltroColeta.SelectedIndex = 0;

            stFiltrarColetas_Moto.IsVisible       = true;

            stListaMoto.IsVisible                 = true;
            #endregion

            // 2 - Gera  a lista de coletas disponíveis, e que o motorista ainda não enviou orçamento nenhum
            ListaMotoristaAsync(0);  
        }
        #endregion 

        #region Botões - Voltar, Avançar

        #region Voltar - Motorista
        private async void BtnVoltar_Moto_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            if (lbEndEnt.IsVisible)                 // primeira parte
            {
                // Esconde os botões
                stBtnAvancar_Moto.IsVisible = false;
                stBtnVoltar_Moto.IsVisible = false;

                // Esconde os campos
                slMotoristaEncontrarColeta_Lista.IsVisible = false;

                // Mostra a lista e o filtro
                stFiltrarColetas_Moto.IsVisible = true;
                stListaMoto.IsVisible = true;

            }
            else if (lbPeso.IsVisible)
            {
                // Mostra e esconde campos
                MostraDados_Moto();
                EscondeDados_2_Moto();
            }
            else if (etObservacoes_Orcamento.IsVisible)
            {
                EscondeOrcamento();
                MostraDados_2_Moto();

                // Mostra o botão 'Avançar'
                stBtnAvancar_Moto.IsVisible = true;

                btnEnviar.IsVisible = false;
            }
        }
        #endregion

        #region Avancar - Motorista
        private async void BtnAvancar_Motorista_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            if (etEndRet_Moto.IsVisible)
            {
                // Mostra e esconde campos
                EscondeDados_Moto();
                MostraDados_2_Moto();
            }
            else if (etPeso_Moto.IsVisible)
            {
                EscondeDados_2_Moto();
                MostraOrcamento();

                // Esconde o botão 'Avançar'
                stBtnAvancar_Moto.IsVisible = false;

               // VerificaOrcamento_Existente();

              //  if (verifica == 2)
              //  {
                    btnEnviar.IsVisible = true;
              //  }
            }
        }
        #endregion

        #region VerificaOrcamento_Existente
        public async void VerificaOrcamento_Existente()
        {
            await DisplayAlert("Ok", "Motorista: " + idMotorista + " - Coleta: " + idCol, "OK");


            #region Verifica se o motorista já orçou a coleta em questão e popula campos

            Orcamento orca = new Orcamento();
            OrcamentoController orcamentoController = new OrcamentoController();

            
            orca = await orcamentoController.GetOrcamento(idMotorista, idCol);

            if (orca != null) // Orçamento existente
            {
                verifica = 1;

                // Esconde o botão 'Enviar orçamento'
                btnEnviar.IsVisible = false;

                // Popula os campos

                await DisplayAlert("Orçamento", "Valor: " + orca.Valor + " - Obs: " + orca.Observacoes, "OK");

               // etValor_Moto.Text = orca.Valor;
               // etObservacoes_Orcamento.Text = orca.Observacoes;

                // Bloqueia os campos (não permite alterações)
                etValor_Moto.IsEnabled            = false;
                etObservacoes_Orcamento.IsEnabled = false;
            }
            else
            {
                verifica = 2;
            }
            #endregion
        }

        #endregion

        #endregion

        #region Btn - Enviar (Orçamento)

        private async void BtnEnviar_Clicked(object sender, SelectedItemChangedEventArgs e)
        {
            ValidaCampos_Orcamento();
        }

        #endregion

        #endregion

        #region Lista

        #region ListaMotoristaAsync(int i)
        public async void ListaMotoristaAsync(int i)
        {
            List<Coleta> _list;

            _list = null;

            if (i == 0)                              // TODAS AS COLETAS DISPONÍVEIS E NÃO VISUALIZADAS
            {
                List<Coleta> listaa = new List<Coleta>();

                _list = await coletaControl.GetListColeta_Geral(idMotorista, i);   // IdMotorista e IdStatus

                if (_list == null)
                {
                    LstColetaMotorista.IsVisible = false;
                }
                else
                {
                    LstColetaMotorista.ItemsSource = _list;
                }
            }
            else if (i == 1)                         // COLETAS VISUALIZADAS
            {
                _list = await coletaControl.GetListColetaMotorista(idMotorista, 1);

                LstColetaMotorista.ItemsSource = _list;
            }
            else if (i == 2)                        // COLETAS QUE ENVIEI ORÇAMENTO
            {
                _list = await coletaControl.GetListColetaMotorista(idMotorista, 2);

                LstColetaMotorista.ItemsSource = _list;
            }
        }
        #endregion
        
        #region Lista de coletas - Item selecionado
        private async void LstColetaMotorista_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            // obtem o item do listview
            var coleta = e.SelectedItem as Coleta;

            // deseleciona o item do listview
            LstColeta.SelectedItem = null;
                        
            slMotoristaEncontrarColeta_Lista.IsVisible = true;

            #region Esconde

            // Esconde o filtro
            stFiltrarColetas_Moto.IsVisible = false;

            // Esconde a lista
            stListaMoto.IsVisible = false;

            #endregion

            idColetaCliente   = coleta.IdCliente;
            idColetaOrcamento = coleta.IdColeta;

            // Popula os campos como o objeto retornado
            PopulaColeta_Orcamento(coleta);

            stBtnVoltar_Moto.IsVisible  = true;     // Mostra botões
            stBtnAvancar_Moto.IsVisible = true;

            idCol = coleta.IdColeta;

            // Insere registro para mostrar que a coleta foi visualizada
            #region Verifica tab. 'ColetaVisualiza'

            ColetaVisualizaController visualiza = new ColetaVisualizaController();
            List<ColetaVisualiza>     lista     = new List<ColetaVisualiza>();

            lista = await visualiza.GetListVisualiza_(idMotorista, idCol);

            int conta = lista.Count;

            if(conta == 0)
            {
                InsereVisualizacao(idCol, idMotorista, idColetaCliente);
            }

            #endregion
        }
        #endregion

        #region InsereVisualizacao()
        private void InsereVisualizacao(int idColeta, int idMotorista, int idCliente)
        {
            ColetaVisualiza visualizacao;
            ColetaVisualizaController visualizaController = new ColetaVisualizaController();

            #region ColetaVisualiza()

            visualizacao = new ColetaVisualiza()
            {
                IdColeta         = idColeta,
                IdMotorista      = idMotorista,
                IdCliente        = idCliente,
                DataVisualizacao = DateTime.Now
            };

            #endregion

            // Insert
            visualizaController.PostVisualizaAsync(visualizacao);
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

            lbEnvia.IsVisible                 = false;
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

            lbEnvia.IsVisible                 = true;
            lbValorOrcamento.IsVisible        = true;
            etValor_Moto.IsVisible            = true;
            etValorRS_Moto.IsVisible          = true;
            lbObs_Orcamento.IsVisible         = true;
            etObservacoes_Orcamento.IsVisible = true;

            #endregion
        }

        #endregion

        #endregion

        #region PopulaColeta_Orcamento(coleta)

        private void PopulaColeta_Orcamento(Coleta coleta)
        {
            #region Popula campos

            // Endereços 
            etEndRet_Moto.Text              = coleta.EndRetEndereco;
            etEndEnt_Moto.Text              = coleta.EndEntEndereco;

            // Material
            lbTipoMaterial_Moto.Text        = coleta.MatTipo;
            lbFragilidadeMaterial_Moto.Text = coleta.MatFragilidade;
            lbDescricaoMaterial_Moto.Text   = coleta.MatDescricao;

            // Dimensões, peso, data, horário, valor, observações e tipo de carro
            etPeso_Moto.Text            = coleta.MatPeso;
            etVolume_Moto.Text          = coleta.MatVolume;
            etLargura_Moto.Text         = coleta.MatLargura;
            etAltura_Moto.Text          = coleta.MatAltura;
            etDataMax_Moto.Text         = coleta.DataMaxima;
            etHorario_Moto.Text         = coleta.HorarioLimite + ":" + coleta.HorarioLimite02;
            etValorPretendido_Moto.Text = "R$ " + coleta.ValorPretendido;

            etObservacoes_Moto.Text     = coleta.Observacoes;
            etTipoVeiculo_Moto.Text     = coleta.TipoVeiculo;

            #endregion
        }

        #endregion
        
        #region Filtro - Coleta - Motorista
        private void pckFiltroColeta_Motorista(object sender, EventArgs e)
        {
            var itemSelecionado = etMotoristaFiltroColeta.Items[etMotoristaFiltroColeta.SelectedIndex];

            if (itemSelecionado.Equals("COLETAS DISPONÍVEIS"))
            {
                ListaMotoristaAsync(0);
            }
            else if (itemSelecionado.Equals("COLETAS VISUALIZADAS"))
            {
                ListaMotoristaAsync(1);
            }
            else if (itemSelecionado.Equals("COLETAS QUE ENVIEI ORÇAMENTO"))
            {
                ListaMotoristaAsync(2);
            }
        }
        #endregion
        
        #region Valida Campos (Orçamento)

        private async void ValidaCampos_Orcamento()
        {
            #region Variáveis / Controllers

            int status   = 13;  // idStatus = Aguardando aprovação

            string vazio = "Campo vazio";
            string nulo  = "Preencha o campo: ";

            string valor       = etValor_Moto.Text;
            string observacoes = etObservacoes_Orcamento.Text;

            Orcamento orcamento;
            OrcamentoController orcamentoController = new OrcamentoController();
            
            #endregion
            
            #region Verifica campos

            try
            {
                if (string.IsNullOrEmpty(valor))                      // Valor
                {
                    await DisplayAlert(vazio, nulo + lbValorOrcamento.Text, "OK");
                }
                else
                {
                    // INSERT
                    #region Orcamento()                      

                        orcamento = new Orcamento()
                        {
                            IdColeta = idColetaOrcamento,
                            IdCliente = idColetaCliente,
                            IdMotorista = idMotorista,
                            Valor = valor,
                            DataCadastro = DateTime.Now,
                            IdStatus = status,
                            DataAceite = null,
                            DataRecusa = null,
                            Observacoes = observacoes
                        };

                        #endregion
                
                    await orcamentoController.PostOrcamebtoAsync(orcamento);
                
                    await DisplayAlert("Sucesso!", "Cadastro realizado com sucesso!", "OK");
                
                    #region Esconde botões

                        btnEnviar.IsVisible = false;
                        stBtnVoltar_Moto.IsVisible = false;

                        EscondeOrcamento();

                        #endregion
                
                    await Navigation.PushModalAsync(new Views.LgColetas());
                }
            }
            catch (Exception ex)
            {
                if (ex.Source != null)
                    Console.WriteLine("Exception source: {0}", ex.Source);
                throw;
            }


            #endregion
        }

        #endregion
        
        #endregion

        #region Mostra e esconde os campos

        #region Dados do material - Visivel
        void DadosMaterialVisible()
        {
            lblTipoMaterial.IsVisible        = true;
            etTipoMaterial.IsVisible         = true;
            lblFragilidadeMaterial.IsVisible = true;
            etFragilidadeMaterial.IsVisible  = true;
            etDescricaoMaterial.IsVisible    = true;
            lblDescricaoMaterial.IsVisible   = true;
            etPeso.IsVisible                 = true;
            lblPeso.IsVisible                = true;
            lblPeso2.IsVisible               = true;
            etVolume.IsVisible               = true;
            lblVolume.IsVisible              = true;
            etLargura.IsVisible              = true;
            lblLargura.IsVisible             = true;
            lblLargura2.IsVisible            = true;
            etAltura.IsVisible               = true;
            lblAltura.IsVisible              = true;
            lblAltura2.IsVisible             = true;

            btnDadosMaterial.IsEnabled = false;
        }
        #endregion

        #region Dados do material - Invisivel
        void DadosMaterialNotVisible()
        {
            lblTipoMaterial.IsVisible        = false;
            etTipoMaterial.IsVisible         = false;
            lblFragilidadeMaterial.IsVisible = false;
            etFragilidadeMaterial.IsVisible  = false;
            etDescricaoMaterial.IsVisible    = false;
            lblDescricaoMaterial.IsVisible   = false;
            etPeso.IsVisible                 = false;
            lblPeso.IsVisible                = false;
            lblPeso2.IsVisible               = false;
            etVolume.IsVisible               = false;
            lblVolume.IsVisible              = false;
            etLargura.IsVisible              = false;
            lblLargura.IsVisible             = false;
            lblLargura2.IsVisible            = false;
            etAltura.IsVisible               = false;
            lblAltura.IsVisible              = false;
            lblAltura2.IsVisible             = false;

            btnDadosMaterial.IsEnabled       = true;
        }
        #endregion

        #region Endereço - Retirada - Visivel
        void EnderecoRetiradaVisible()
        {
            etEndRetCep.IsVisible             = true;
            lblEndRetCep.IsVisible            = true;
            etEndRetUf.IsVisible              = true;
            lblEndRetUf.IsVisible             = true;
            etEndRet.IsVisible                = true;
            lblEndRet.IsVisible               = true;
            etEndRetNumero.IsVisible          = true;
            lblEndRetNumero.IsVisible         = true;
            etEndRetCompl.IsVisible           = true;
            lblEndRetCompl.IsVisible          = true;
            etEndRetBairro.IsVisible          = true;
            lblEndRetBairro.IsVisible         = true;
            etEndRetCidade.IsVisible          = true;
            lblEndRetCidade.IsVisible         = true;
            etEndRetResponsavel.IsVisible     = true;
            lblEndRetResponsavel.IsVisible    = true;
            etEndRetResponsavelTel.IsVisible  = true;
            lblEndRetResponsavelTel.IsVisible = true;

            btnEnderecoRetirada.IsEnabled = true;
        }
        #endregion

        #region Endereço - Retirada - Invisivel
        void EnderecoRetiradaNotVisible()
        {
            etEndRetCep.IsVisible             = false;
            lblEndRetCep.IsVisible            = false;
            etEndRetUf.IsVisible              = false;
            lblEndRetUf.IsVisible             = false;
            etEndRet.IsVisible                = false;
            lblEndRet.IsVisible               = false;
            etEndRetNumero.IsVisible          = false;
            lblEndRetNumero.IsVisible         = false;
            etEndRetCompl.IsVisible           = false;
            lblEndRetCompl.IsVisible          = false;
            etEndRetBairro.IsVisible          = false;
            lblEndRetBairro.IsVisible         = false;
            etEndRetCidade.IsVisible          = false;
            lblEndRetCidade.IsVisible         = false;
            etEndRetResponsavel.IsVisible     = false;
            lblEndRetResponsavel.IsVisible    = false;
            etEndRetResponsavelTel.IsVisible  = false;
            lblEndRetResponsavelTel.IsVisible = false;

            btnEnderecoRetirada.IsEnabled = true;
        }
        #endregion

        #region Endereco Entrega Visivel
        void EnderecoEntregaVisible()
        {
            etEndEntCep.IsVisible             = true;
            lblEndEntCep.IsVisible            = true;
            etEndEntUf.IsVisible              = true;
            lblEndEntUf.IsVisible             = true;
            etEndEnt.IsVisible                = true;
            lblEndEnt.IsVisible               = true;
            etEndEntNumero.IsVisible          = true;
            lblEndEntNumero.IsVisible         = true;
            etEndEntCompl.IsVisible           = true;
            lblEndEntCompl.IsVisible          = true;
            etEndEntBairro.IsVisible          = true;
            lblEndEntBairro.IsVisible         = true;
            etEndEntCidade.IsVisible          = true;
            lblEndEntCidade.IsVisible         = true;
            etEndEntResponsavel.IsVisible     = true;
            lblEndEntResponsavel.IsVisible    = true;
            etEndEntResponsavelTel.IsVisible  = true;
            lblEndEntResponsavelTel.IsVisible = true;

            btnEnderecoEntrega.IsEnabled = true;
        }
        #endregion

        #region Endereco Entrega - Visivel
        void EnderecoEntregaNotVisible()
        {
            etEndEntCep.IsVisible             = false;
            lblEndEntCep.IsVisible            = false;
            etEndEntUf.IsVisible              = false;
            lblEndEntUf.IsVisible             = false;
            etEndEnt.IsVisible                = false;
            lblEndEnt.IsVisible               = false;
            etEndEntNumero.IsVisible          = false;
            lblEndEntNumero.IsVisible         = false;
            etEndEntCompl.IsVisible           = false;
            lblEndEntCompl.IsVisible          = false;
            etEndEntBairro.IsVisible          = false;
            lblEndEntBairro.IsVisible         = false;
            etEndEntCidade.IsVisible          = false;
            lblEndEntCidade.IsVisible         = false;
            etEndEntResponsavel.IsVisible     = false;
            lblEndEntResponsavel.IsVisible    = false;
            etEndEntResponsavelTel.IsVisible  = false;
            lblEndEntResponsavelTel.IsVisible = false;

            btnEnderecoEntrega.IsEnabled      = true;
        }
        #endregion

        #region Valor visivel
        void ValorVisible()
        {
            lblDataMax.IsVisible         = true;
            etDataMax.IsVisible          = true;
            lblHorario.IsVisible         = true;
            lblHorarioMeio.IsVisible     = true;
            etHorario2.IsVisible         = true;
            etHorario.IsVisible          = true;
            lblValorPretendido.IsVisible = true;
            lblValorRS.IsVisible         = true;
            etValorPretendido.IsVisible  = true;
            lblObservacoes.IsVisible     = true;
            etObservacoes.IsVisible      = true;
            lblApelido.IsVisible         = true;
            etApelido.IsVisible          = true;
            lblTipoVeiculo.IsVisible     = true;
            etTipoVeiculo.IsVisible      = true;

            btnValor.IsEnabled           = true;
        }
        #endregion

        #region Valor invisivel
        void ValorNotVisible()
        {
            lblDataMax.IsVisible         = false;
            etDataMax.IsVisible          = false;
            lblHorario.IsVisible         = false;
            lblHorarioMeio.IsVisible     = false;
            etHorario2.IsVisible         = false;
            etHorario.IsVisible          = false;
            lblValorPretendido.IsVisible = false;
            lblValorRS.IsVisible         = false;
            etValorPretendido.IsVisible  = false;
            lblObservacoes.IsVisible     = false;
            etObservacoes.IsVisible      = false;
            lblApelido.IsVisible         = false;
            etApelido.IsVisible          = false;
            lblTipoVeiculo.IsVisible     = false;
            etTipoVeiculo.IsVisible      = false;

            btnValor.IsEnabled           = true;
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

        #region Btn - MinhaConta
        private async void BtnMinhaConta_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.LgMinhaContaa());
        }
        #endregion

        #endregion

    }
}