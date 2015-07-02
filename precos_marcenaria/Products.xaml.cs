using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using precos_marcenaria.Classes;
using System.IO;
using System.Xml.Linq;

namespace precos_marcenaria
{
    public partial class Products : PhoneApplicationPage
    {
        public Products()
        {
            InitializeComponent();
            
        
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string msg = "";

            if (NavigationContext.QueryString.TryGetValue("q", out msg))
            {
                WebClient client = new WebClient();
                client.OpenReadCompleted += client_OpenReadCompleted;
                client.OpenReadAsync(new Uri(msg, UriKind.Absolute));
                status.Text = "Buscando produtos.. Aguarde";
            }
            // now your parameter is in the msg variable, and you could do stuff here.

        }

        private void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            List<Item> itens = new List<Item>();
            Stream str = e.Result;
            XDocument loadedData = XDocument.Load(str);
            try
            {
                try
                {
                    String eve = "product";
                    foreach (var item in loadedData.Descendants(eve))
                    {
                        Item c = new Item();
                        c.name = item.Element("name").Value;
                        c.value = "Valor: R$"+item.Element("value").Value;
                        c.description = item.Element("description").Value;
                        c.updated_at = item.Element("last_update").Value;
                        c.branch_name = item.Element("branch_name").Value;
                        c.branch_phone = item.Element("branch_phone").Value;
                        c.image = "http://" + item.Element("image").Value;

                        itens.Add(c);

                        
                    }
                    LstItem.ItemsSource = itens;
                    if (itens.Count > 0)
                    {
                        status.Text = "Buscado com sucesso.  Foram achados " + itens.Count + " itens";
                    }
                    else
                    {
                        status.Text = "Não há itens cadastrados para está cidade.";
                    }
                }
                catch (Exception ex)
                {

                }

            }
            catch (Exception ex)
            {

            }
        }

    }
}