using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ClientsRepository clientsRepository;
        public ChangesRepository changesRepository;
        Consultant newConsultant = new Consultant();
        Client client;
        Dictionary<int, long> phoneChanges = new Dictionary<int, long>();
        string rootClients;
        string rootChanges;
        public int index;
        public Employee.Position position;

        public ConsultantWindow()
        {
            InitializeComponent();

            clientItems.ItemsSource = null;
            recordItems.ItemsSource = null;

            position = Employee.Position.Consultant;

            clientsRepository = new ClientsRepository(this);
            changesRepository = new ChangesRepository(this);

            clientsRepository.ClientsList = newConsultant.GetClietsItemSourse(position);
            if (clientsRepository.ClientsList != null)
            {
                clientItems.ItemsSource = clientsRepository.ClientsList;

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
                MessageBox.Show("Файл пока пустой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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
            if (clientItems.SelectedItem != null)
            {
                index = clientItems.SelectedIndex;

                var client = clientsRepository.ClientsList.ElementAt(index);

                bool parse = newConsultant.CheckParsePhone(phone.Text, out long phoneNumber);
                if (parse && phoneNumber != 0)
                {
                    if (!newConsultant.CheckPhoneMatchesPattern(phone.Text)) return;
                    if (newConsultant.CheckPhoneExistInBase(clientsRepository.ClientsList, client, phoneNumber)) return;
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

                if (phoneChanges.ContainsKey(index))
                {
                    phoneChanges.Remove(index);
                    phoneChanges.Add(index, phoneNumber);
                }
                else
                {
                    phoneChanges.Add(index, phoneNumber);
                }

                clientItems.ItemsSource = clientsRepository.ClientsList;
                recordItems.ItemsSource = changesRepository.ChangesList;
            }
            else
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            clientsRepository.ClientsList = newConsultant.GetClietsItemSourse(Employee.Position.Manager);
            if (clientsRepository.ClientsList == null || clientsRepository.ClientsList.Count <= 0)
            {
                MessageBox.Show("Список клиентов пока пустой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                var keys = phoneChanges.Keys.ToList();

                for (int i = 0; i < keys.Count; i++)
                {
                    clientsRepository.ClientsList.ElementAt(keys[i]).Phone = phoneChanges[keys[i]];
                }
                rootClients = newConsultant.ConvertToJsonClients(clientsRepository.ClientsList);
            }
            if (changesRepository.ChangesList.Count > 0)
            {
                rootChanges = newConsultant.ConvertToJsonChanges(changesRepository.ChangesList);
            }
            else
            {
                return;
            }

            newConsultant.WriteToFile(rootClients, newConsultant.fileNameClients);
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
