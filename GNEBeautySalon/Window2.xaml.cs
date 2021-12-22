using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GNEBeautySalon
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        GNEBeautySalonEntities context;
        Client client;
        public Window2(GNEBeautySalonEntities context, Client client)
        {
            InitializeComponent();
            this.context = context;
            this.client = client;
            var list = from c in context.ClientService where c.ClientID == client.ID select c;
            List.ItemsSource = list.ToList();
        }
    }
}