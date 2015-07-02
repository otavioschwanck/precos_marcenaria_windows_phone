using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using precos_marcenaria.Classes;
using System.Xml.Linq;
using System.IO;

namespace precos_marcenaria
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnCidade_click(object sender, RoutedEventArgs e)
        {
           
            WebClient client = new WebClient();
            client.OpenReadCompleted +=client_OpenReadCompleted;
            client.OpenReadAsync(new Uri("http://precosmarcenaria.herokuapp.com/cities.xml?q="+txtCidade.Text, UriKind.Absolute));
            txtBCidade.Text = "Procurando cidades...Por Favor, aguarde";
            
        }

        private void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            List<Cidade> cidades = new List<Cidade>();
            Stream str = e.Result;
            XDocument loadedData = XDocument.Load(str);
            try
            {
                try
                {
                    String eve = "city";
                    foreach (var item in loadedData.Descendants(eve))
                    {
                        Cidade c = new Cidade();
                        c.name = item.Element("name").Value;
                        c.xml_url = item.Element("info").Value;
                        cidades.Add(c);
                    }
                    LstItem.ItemsSource = cidades;
                    txtBCidade.Text = "Digite a cidade:";
                }
                catch (Exception ex)
                {
                    txtBCidade.Text = "Erro no xml.";
                }

            }
            catch (Exception ex)
            {
                txtBCidade.Text = "Digite a cidade:";
                MessageBox.Show("Não foi possível procurar as cidades.");
            }
        }

        private void btnBuscar_click(object sender, RoutedEventArgs e)
        {
            if (LstItem.SelectedItem != null)
            {
                Cidade cidade = LstItem.SelectedItem as Cidade;
                NavigationService.Navigate(new Uri("/Products.xaml?q=" + cidade.xml_url, UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Por favor, selecione algum item");
            }
        }

       

       
    }
}