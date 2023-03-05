using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CompanyWithDepartments
{
    /// <summary>
    /// Логика взаимодействия для ConsultantWindow.xaml
    /// </summary>
    public partial class ConsultantWindow : Window
    {
        public DepartmentsRepository departmentRepository;
        public ChangesRepository changesRepository;

        Department newDepartment;
        Consultant newConsultant = new Consultant();
        Client client;
        public int index;
        public Employee.Position position;

        public ConsultantWindow()
        {
            InitializeComponent();

            clientItems.ItemsSource = null;
            recordItems.ItemsSource = null;

            position = Employee.Position.Consultant;

            departmentRepository = new DepartmentsRepository(this);
            changesRepository = new ChangesRepository(this);

            departmentRepository.Departments = newConsultant.GetDepartmentsItemSourse(position);
            if (departmentRepository.Departments != null)
            {
                cbDepartment.ItemsSource = departmentRepository.Departments;


                changesRepository.ChangesList = newConsultant.GetChangesItemSourse();
                if (changesRepository.ChangesList != null)
                {
                    recordItems.ItemsSource = changesRepository.ChangesList;
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Файл департаментов пока пустой." +
                                "\n Департамент может добавить только менеджер.",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// Обработка события выбора департамента
        /// </summary>
        private void CbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbDepartment.SelectedItem == null)
            {
                return;
            }

            var selectedItem = (Department)cbDepartment.SelectedItem;
            string value = selectedItem.NameDepartment;

            newDepartment = departmentRepository.Departments.Single(p => p.NameDepartment.Equals(value));

            clientItems.ItemsSource = newDepartment.Clients;
        }

        /// <summary>
        /// Обработка события выбора строки из списка клиентов
        /// </summary>
        private void Row_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            client = new Client();
            client = (Client)clientItems.SelectedItem;

            if (client == null)
            {
                return;
            }

            phone.Text = client.Phone.ToString();
        }

        /// <summary>
        /// Проверка корректности введенного номера телефона
        /// </summary>
        private void OnPhoneChanged(object sender, TextChangedEventArgs e)
        {
            var bc = new BrushConverter();
            bool parse = long.TryParse(phone.Text, out long phoneNumber);
            if (parse)
            {
                Regex regex = new Regex(@"\d{11}$");

                if (regex.IsMatch(phone.Text) && phoneNumber > 10000000000 && phoneNumber < 99999999999)
                {
                    phone.Background = (Brush)bc.ConvertFrom("#abffbe");
                }
                else
                {
                    phone.Background = (Brush)bc.ConvertFrom("#ffa4a4");
                }
            }
            else
            {
                phone.Background = (Brush)bc.ConvertFrom("#ffa4a4");
                return;
            }
        }

        /// <summary>
        /// Действия при нажатии кнопки "Изменить"
        /// </summary>
        private void OnClickChangePhone(object sender, RoutedEventArgs e)
        {
            if (cbDepartment.SelectedItem == null)
            {
                MessageBox.Show("Выберите департамент",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            if (clientItems.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }
            else if (clientItems.SelectedItem != null)
            {
                var indexDepartment = cbDepartment.SelectedIndex;
                var indexClient = clientItems.SelectedIndex;

                var clientsList = departmentRepository.Departments[indexDepartment].Clients;
                var client = departmentRepository.Departments[indexDepartment].Clients[indexClient];

                if (newConsultant.CheckTextBoxIsNullOrEmpty(phone.Text, "Телефон")) return;

                bool parse = newConsultant.CheckParsePhone(phone.Text, out long phoneNumber);
                
                if (parse && phoneNumber != 0)
                {
                    if (!newConsultant.CheckPhoneMatchesPattern(phone.Text)) return;
                    if (newConsultant.CheckPhoneExistInBase(clientsList, client, phoneNumber)) return;
                }
                else
                {
                    return;
                }

                string fieldsList = CheckFieldPhoneChanged(client);
                if (!String.IsNullOrEmpty(fieldsList))
                {
                    var newRecordChange = newConsultant.NewRecord(fieldsList, Change.DataChange.ChangingRecord, position);
                    changesRepository.ChangesList.Add(newRecordChange);
                }
                else
                {
                    return;
                }

                clientItems.ItemsSource = null;
                recordItems.ItemsSource = null;

                client.Phone = phoneNumber;

                clientItems.ItemsSource = departmentRepository.Departments[indexDepartment].Clients;
                recordItems.ItemsSource = changesRepository.ChangesList;
                clientItems.Items.Refresh();
                recordItems.Items.Refresh();
            }
        }


        /// <summary>
        /// Сравнение телефона выбранного клиента и поля TextBox Phone.Text
        /// </summary>
        /// <returns>
        /// Название поля
        /// </returns>
        public string CheckFieldPhoneChanged(Client client)
        {
            string totalString;

            if (!String.Equals(client.Phone.ToString(), phone.Text.Trim()))
            {
                totalString = "Телефон";
            }
            else
            {
                totalString = "";
            }
            return totalString;
        }

        /// <summary>
        /// Действия при нажатии на кнопку "Сохранить в файл"
        /// Сохранение измененных телефонов и новых записей в файлы
        /// </summary>
        private void OnClickSaveToFiles(object sender, RoutedEventArgs e)
        {
            string rootChanges = "";

            string rootDepartments = "";

            if (departmentRepository.Departments.Count <= 0)
            {
                MessageBox.Show("Список департаментов пока пустой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                rootDepartments = newConsultant.ConvertToJsonDepartment(departmentRepository.Departments);
            }
            if (changesRepository.ChangesList.Count > 0)
            {
                rootChanges = newConsultant.ConvertToJsonChanges(changesRepository.ChangesList);
            }

            newConsultant.WriteToFile(rootDepartments, newConsultant.fileNameDepartments);
            newConsultant.WriteToFile(rootChanges, newConsultant.fileNameChanges);

            MessageBox.Show("Файл сохранен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Очистка поля TextBox Phone при получении фокуса элементом
        /// </summary>
        private void OnFocusPhone(object sender, RoutedEventArgs e)
        {
            if (phone.Text == "Телефон")
            {
                phone.Text = "";
            }
        }
        /// <summary>
        /// Действия при нажатии на кнопку "Выйти"
        /// </summary>
        private void OnClickExit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
