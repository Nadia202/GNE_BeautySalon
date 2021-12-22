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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.Entity;
using System.ComponentModel;


namespace GNEBeautySalon
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public GNEBeautySalonEntities context;
        int page = 0;
        int countPage;
        public MainWindow()
        {
            InitializeComponent();
            context = new GNEBeautySalonEntities();
            CountRow.SelectedIndex = 0;
            SortGender.SelectedIndex = 0;
        }

        public void Sort()
        {
            string index = "0";
            int countRow = 0;

            if (SortGender.SelectedIndex == 0 && CountRow.SelectedIndex == 0)
            {
                var select0 = (from c in context.Client where c.Email.StartsWith(TextBoxEmail.Text) && c.FirstName.StartsWith(TextBoxImya.Text) && c.Phone.StartsWith(TextBoxPhone.Text) orderby(c.ID) select c).ToList();
                DataGridClient.ItemsSource = select0;
                countPage = select0.Count;
                ShowRow.Text = $"{select0.Count} из {countPage}";
                return;
            }

            if (CountRow.SelectedIndex == 1)
                countRow = 10;

            if (CountRow.SelectedIndex == 2)
                countRow = 50;

            if (CountRow.SelectedIndex == 3)
                countRow = 200;

            if (SortGender.SelectedIndex == 1)
                index = "1";

            if (SortGender.SelectedIndex == 2)
                index = "2";

            if (SortGender.SelectedIndex != 0 && CountRow.SelectedIndex == 0)
            {
                var select0 = (from c in context.Client where c.GenderCode.Contains(index) && c.Email.StartsWith(TextBoxEmail.Text) && c.FirstName.StartsWith(TextBoxImya.Text) && c.Phone.StartsWith(TextBoxPhone.Text) select c).ToList();
                DataGridClient.ItemsSource = select0;
                ShowRow.Text = $"{countPage} из {countPage}";
                return;
            }

            if (SortGender.SelectedIndex == 0 && CountRow.SelectedIndex != 0)
            {
                var select0 = (from c in context.Client where c.Email.StartsWith(TextBoxEmail.Text) && c.FirstName.StartsWith(TextBoxImya.Text) && c.Phone.StartsWith(TextBoxPhone.Text) orderby (c.ID) select c).Skip(page).Take(countRow).ToList();
                DataGridClient.ItemsSource = select0;
                ShowRow.Text = $"{page + select0.Count} из {countPage}";
                return;
            }

            var select = (from c in context.Client where c.GenderCode.Contains(index) && c.Email.StartsWith(TextBoxEmail.Text) && c.FirstName.StartsWith(TextBoxImya.Text) && c.Phone.StartsWith(TextBoxPhone.Text) orderby(c.ID) select c).Skip(page).Take(countRow).ToList();
            DataGridClient.ItemsSource = select;
            ShowRow.Text = $"{page+ select.Count} из {countPage}";
        }

        private void SortGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sort();
        }

        private void TextBoxImya_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sort();
        }

        private void TextBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sort();
        }

        private void TextBoxPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sort();
        }

        private void CountRow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            page = 0;
            Sort();
        }

        private void PreviousList_Click(object sender, RoutedEventArgs e)
        {
            if (page == 0)
                return;

            switch (CountRow.SelectedIndex)
            {
                case 0:
                    page -= 0;
                    break;
                case 1:
                    page -= 10;
                    break;
                case 2:
                    page -= 50;
                    break;
                case 3:
                    page -= 200;
                    break;
            }
            Sort();
        }

        private void NextList_Click(object sender, RoutedEventArgs e)
        {
            if (page == countPage-DataGridClient.Items.Count)
                return;

            switch(CountRow.SelectedIndex)
            {
                case 0:
                    page += 0;
                    break;
                case 1:
                    page += 10;
                    break;
                case 2:
                    page += 50;
                    break;
                case 3:
                    page += 200;
                    break;
            }
            Sort();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var newClient = new Client();
            context.Client.Add(newClient);
            var window = new Window1(context,newClient, true);
            window.ShowDialog();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var r = DataGridClient.SelectedItem as Client;
            if (r == null)
            {
                MessageBox.Show("Выберите строку в таблице!");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Удаление клиент", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                context.Client.Remove(r);
                context.SaveChanges();
                Sort();
            }
        }

        private void BtnEdite_Click(object sender, RoutedEventArgs e)
        {
            var redClient = DataGridClient.SelectedItem as Client;
            var window = new Window1(context,redClient, false);
            window.ShowDialog();
        }

        private void DataGridClient_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnDelete.IsEnabled = true;
            BtnEdite.IsEnabled = true;
        }

        private void TextBoxImya_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetter(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void TextBoxPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && e.Text != "+" && e.Text != "(" && e.Text != ")")
            {
                e.Handled = true;
            }
        }

        private void VistList_Click(object sender, RoutedEventArgs e)
        {
            var Client = DataGridClient.SelectedItem as Client;
            var window = new Window2(context, Client);
            window.ShowDialog();
        }
    }
}
