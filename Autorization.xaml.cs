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

namespace KitchenUtensilsStore
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        public Autorization()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Кнопка проверки Логина и Пароля для Авторицации
        /// </summary>
        private void Vhod_Click(object sender, RoutedEventArgs e)
        {
            var login = TextBoxLogin.Text;
            var password = TextBoxPassworc.Text;
            var bd = new KitchenUtensilsStoreDb();
            var user = bd.User.Where(w => w.Login == login && w.Password == password).FirstOrDefault();
            if (user != null)
            {
                new MainWindow().Show();
                this.Close();
            }
            else MessageBox.Show("Не верный Логин или Пароль!");
        }

        /// <summary>
        /// Кнопка входа как гость
        /// </summary>
        private void VhodGost_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
