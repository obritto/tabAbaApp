using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using tabAbaApp.Collections;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace tabAbaApp
{
    public class AbaDado
    {
        public string titulo { get; set; }

        public object pagina { get; set; }
    }


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<AbaDado> listaAbas { get; set; }

        private IObservableMap<String, Object> DefaultViewModel
        {
            get
            {
                return this.GetValue(DefaultViewModelProperty) as IObservableMap<String, Object>;
            }
            set
            {
                this.SetValue(DefaultViewModelProperty, value);
            }
        }

        private int totalAbas;
        public int TotalAbas
        {
            get
            {
                return totalAbas;
            }

            set
            {
                totalAbas = value;

                txtTot.Text = TotalAbas.ToString();
            }
        }

        public static readonly DependencyProperty DefaultViewModelProperty =
            DependencyProperty.Register("DefaultViewModel",
                typeof(IObservableMap<String, Object>), typeof(MainPage), null);

        public MainPage()
        {
            DefaultViewModel = new ObservableDictionary<String, Object>();

            listaAbas = new ObservableCollection<AbaDado>();

            this.InitializeComponent();

            DefaultViewModel["NavigationItemList"] = listaAbas;
        }

        
        
        public async void CriarAba(Page page, string titulo)
        {
            try
            {
                var aba = new AbaDado
                {
                    titulo = titulo,
                    pagina = page,
                };

               

                listaAbas.Add(aba);
                abasListView.SelectedItem = aba;

               

                TotalAbas = listaAbas.Count;

            }
            catch (Exception ex)
            {
                MessageDialog dialog = new MessageDialog(
                    "Dificuldade ao criar aba\n\nERRO:" + ex.Message, "Dificuldade");
                await dialog.ShowAsync();
            }
        }

        public async void RemoverAba(AbaDado aba)
        {
            if (aba == null)
            {
                return;
            }

           

            listaAbas.Remove(aba);

            if (listaAbas.Count < 1)
            {
                principalFrame.Content = null;
                TogglePaneButton.IsChecked = true;
            }
            else
            {
                SelecionarAba(listaAbas[listaAbas.Count - 1]);
            }

            TotalAbas = listaAbas.Count;
        }

        private async void SelecionarAba(AbaDado aba)
        {
            try
            {
                principalFrame.Content = aba.pagina;
                abasListView.SelectedItem = aba;
              
            }
            catch (Exception ex)
            {
                MessageDialog dialog = new MessageDialog(
                   "Dificuldade ao exibir aba\n\nERRO:" + ex.Message, "Dificuldade");
                await dialog.ShowAsync();
            }
        }

        private void RemoverTodas()
        {
            listaAbas.Clear();
            principalFrame.Content = null;

            TotalAbas = 0;
        }

        private void fecharAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            RemoverAba(abasListView.SelectedItem as AbaDado);
        }

        private async void fecharTodasAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            bool respostaNao = false;

            MessageDialog dialog = new MessageDialog("FECHAR TODAS AS ABAS AGORA ?", "Confirmação");

            dialog.Commands.Add(new UICommand("Sim"));
            dialog.Commands.Add(new UICommand("Não",
                uiCommandInvokedHandler => { respostaNao = true; }));

            dialog.DefaultCommandIndex = 1;
            await dialog.ShowAsync();

            if (respostaNao)
            {
                return;
            }

            RemoverTodas();
        }

        private void abasListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (abasListView.SelectedItem == null)
            {
                return;
            }

            SelecionarAba(abasListView.SelectedItem as AbaDado);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            CriarAba(new Page1(), "PÁGINA 1");
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            CriarAba(new Page2(), "PÁGINA 2");

        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            CriarAba(new Page3(), "PÁGINA 3");

        }
    }
}
