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
using System.IO;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace GNEBeautySalon
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        GNEBeautySalonEntities context;
        bool b;
        public Window1(GNEBeautySalonEntities context, Client RedClient, bool b)
        {
            InitializeComponent();
            this.context = context;
            ComboBoxPol.ItemsSource = context.Gender.ToList();
            this.DataContext = RedClient;
            this.b = b;
            if (b)
            {
                TextBlockId.Visibility = Visibility.Hidden;
                TextBoxId.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Title = "Редактирование";
            }
            DatePickerBirthday.SelectedDate = DateTime.Now;
        }

        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "*.png, *.jpeg|*.png, *.jpeg";
            file.ShowDialog();
            if (file.FileName.Length != 0)
            {
                string fileName = file.FileName;
                FileInfo i = new FileInfo(fileName);
                string path = $@"d:\users\is12303\Desktop\Клиенты{i.Name}";
                if (!File.Exists(path))
                {
                    i.CopyTo(path);
                }
                var cl = (Client)this.DataContext;
                cl.PhotoPath = $@"Клиенты\{i.Name}";
                ImgMin.Source = new BitmapImage(new Uri(path));
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxPol.SelectedIndex == -1 || TextBoxImya.Text == "" || TextBoxFamiliya.Text == "" || TextBoxTelefon.Text == "")
            {
                MessageBox.Show("Обязательные поля не заполнены.\nСписок Обязательных полей: Пол, Имя, Фамилия, Дата рождения, Номер телефона.");
            }
            else
            {
                string mail = @"\w+([-+.]*\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                var email = TextBoxEmail.Text.Trim().ToLowerInvariant();
                if (Regex.Match(email, mail).Success || TextBoxEmail.Text == "")
                {
                    var cl = (Client)this.DataContext;
                    cl.RegistrationDate = DateTime.Now;
                    cl.Birthday = (DateTime)DatePickerBirthday.SelectedDate;
                    context.SaveChanges();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неправельно введён адрес электронной почты");
                    TextBoxEmail.Text = "";
                }
            }
        }

        private void TextBoxImya_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetter(e.Text,0))
            {
                e.Handled = true;
            }
        }

        private void TextBoxFamiliya_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetter(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void TextBoxOtchestvo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetter(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void TextBoxTelefon_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && e.Text != "+" && e.Text != "(" && e.Text != ")")
            {
                e.Handled = true;
            }
        }
    }
}
