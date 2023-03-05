using System.Windows;
using System.Windows.Controls;

namespace CompanyWithDepartments
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Обработка события выбора сотрудника из списка
        /// </summary>
        private void OnSelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectPosition.SelectedItem != null)
            {
                ComboBoxItem cbi = (ComboBoxItem)SelectPosition.SelectedItem;
                string name = cbi.Name;
                switch (name)
                {
                    case "Manager":
                        {
                            ManagerWindow managerWindow = new ManagerWindow();
                            managerWindow.Show();
                            this.Close();
                            break;
                        }
                    //case "Consultant":
                    //    {
                    //        ConsultantWindow consultantWindow = new ConsultantWindow();
                    //        consultantWindow.Show();
                    //        this.Close();
                    //        break;
                    //    }
                    default:
                        {
                            MessageBox.Show("Выберите должность", "Ошибка", MessageBoxButton.OK);
                            break;
                        }
                }
            }
        }
        /// <summary>
        /// Обработка события нажатия кнопки "Выйти"
        /// </summary>
        private void OnClickExit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
