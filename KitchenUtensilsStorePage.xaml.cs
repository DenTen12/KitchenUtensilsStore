using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace KitchenUtensilsStore
{
    /// <summary>
    /// Логика взаимодействия для KitchenUtensilsStorePage.xaml
    /// </summary>
    public partial class KitchenUtensilsStorePage : Page
    {
        KitchenUtensilsStoreBD db = new KitchenUtensilsStoreBD();

        public KitchenUtensilsStorePage()
        {
            InitializeComponent();
            foreach (var y in KitchenUtensilsStoreBD.GetContext().Tovar.ToList())
            {

                ListData.Items.Add(y);

            }
            ViewPages(0);
        }
        /// <summary>
        /// Подсвечивание строки с данными о конкретном товаре в зависимости от размера скидки.В случае если размер скидки превышает 15%, в качестве фона применяется цвет #7fff00. Если у товара снижена стоимость, то основная цена перечеркнута, и рядом с ней указана итоговая стоимость
        /// </summary>
        public void ViewPages(int number)
        {
            int count = 14;
            List
            <Tovar> products = db.Tovar.ToList();

            var list = (from a in products
                        join b in db.Vendor on a.TovarVendor equals b.ID_Vendor
                        select new
                        {
                            a.Name,
                            a.Image,
                            a.Description,
                            a.Rebate,
                            producter = b.NameVendor,
                            BackgroundSourse = a.Rebate > 15 ? "#7fff00" : "Transparent",
                            a.Sum,
                            DiscondedPrice = a.Rebate == 0 ? "" : Math.Round(Convert.ToDouble(a.Sum * (100 - a.Rebate) / 100), 3).ToString(),
                            TextDecorations = a.Rebate != 0 ? "Strikethrough" : "None",
                            Width = ((Panel)Application.Current.MainWindow.Content).ActualWidth - 50,
                        }).ToList();

            if (list.Count() - number
                < 15)
                count = list.Count();
            else
                count += number;

            if (list.Count() >= number)
                try
                {
                    ListData.Items.Clear();
                    for (int i = number; i
                    < count; i++)
                        ListData.Items.Add(list[i]);
                }
                catch
                {
                    MessageBox.Show("Записи не найдены", "Внимение!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

        }

        /// <summary>
        /// Пользователь имеет возможность отфильтровать данные  по размеру скидки в диапазонах: 0-9,99%, 10-14,99%, 15% и более. Элемент “Все диапазоны”, при выборе которого настройки фильтра сбрасываются.
        /// </summary>
        private void ComboBoxFiltracia_DropDownClosed(object sender, EventArgs e)
        {
            var list = KitchenUtensilsStoreBD.GetContext().Tovar.ToList();
            if (ComboBoxFiltracia.Text != null)
                switch (ComboBoxFiltracia.Text)
                {
                    case "0-9,99%":
                        list = list.Where(w => w.Rebate >= 0 && w.Rebate
                        <= Convert.ToDecimal(9.99)).ToList();
                        ListData.Items.Clear();
                        for (int i = 0; i < list.Count(); i++)
                            ListData.Items.Add(list[i]);
                        if (list == null)
                            MessageBox.Show("Записи не найдены", "Внимение!", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;

                    case "10-14,99%":
                        list = list.Where(w => w.Rebate >= 10 && w.Rebate
                            <= Convert.ToDecimal(14.99)).ToList();
                        ListData.Items.Clear();
                        for (int i = 0; i < list.Count(); i++)
                            ListData.Items.Add(list[i]);
                        if (list.Count() == 0)
                            MessageBox.Show("Записи не найдены", "Внимение!", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "15% и более":
                        list = list.Where(w => w.Rebate >= 15).ToList();
                        ListData.Items.Clear();
                        for (int i = 0; i
                                < list.Count(); i++)
                            ListData.Items.Add(list[i]);
                        if (list == null)
                            MessageBox.Show("Записи не найдены", "Внимение!", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case "Все  диапазоны":
                        ListData.Items.Clear();
                        for (int i = 0; i < 14; i++)
                            ListData.Items.Add(list[i]);
                        break;
                }
        }

        /// <summary>
        /// Пользователь имеет возможность отсортировать товары (по возрастанию и убыванию) по стоимости.
        /// </summary>
        private void ComboBoxSortirovka_DropDownClosed(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListData.Items);

            switch (ComboBoxSortirovka.Text)
            {
                case "Цена по возрастанию":
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(nameof(Tovar.Sum), ListSortDirection.Ascending));
                    break;

                case "Цена по убыванию":
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(nameof(Tovar.Sum), ListSortDirection.Descending));
                    break;
            }
        }

        /// <summary>
        /// Кнопка обновления окна.
        /// </summary>
        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            ListData.Items.Clear();
            foreach (var y in KitchenUtensilsStoreBD.GetContext().Tovar.ToList())
            {

                ListData.Items.Add(y);

            }
        }
    }
}
