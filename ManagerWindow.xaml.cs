using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CompanyWithDepartments
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ClientsRepository clientsRepository;
        public ChangesRepository changesRepository;

        Manager newManager = new Manager();
        Client client;
        List<string> textList = new List<string>();
        string rootClients;
        string rootChanges;
        public int index;
        public Employee.Position position;

        public ManagerWindow()
        {
            InitializeComponent();

            clientItems.ItemsSource = null;
            recordItems.ItemsSource = null;

            position = Employee.Position.Manager;

            clientsRepository = new ClientsRepository(this);
            changesRepository = new ChangesRepository(this);

            clientsRepository.ClientsList = newManager.GetClietsItemSourse(position);
            if (clientsRepository.ClientsList != null)
            {
                clientItems.ItemsSource = clientsRepository.ClientsList;

                changesRepository.ChangesList = newManager.GetChangesItemSourse();
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
                clientsRepository.ClientsList = new ObservableCollection<Client>();
                MessageBox.Show("Файл пока пустой. Добавьте клиента и сохраните.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

            firstName.Text = client.FirstName;
            lastName.Text = client.LastName;
            fathersName.Text = client.FathersName;
            phone.Text = client.Phone.ToString();
            passportNumber.Text = client.PassportNumber;
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
        /// Проверка корректности введенного номера паспорта
        /// </summary>
        private void OnPassportChanged(object sender, TextChangedEventArgs e)
        {
            string pNumber = passportNumber.Text.ToString();

            if (!String.IsNullOrEmpty(pNumber))
            {
                Regex regex = new Regex(@"\d{4}-\d{6}$");
                var bc = new BrushConverter();
                if (regex.IsMatch(passportNumber.Text))
                {
                    passportNumber.Background = (Brush)bc.ConvertFrom("#abffbe");
                }
                else
                {
                    passportNumber.Background = (Brush)bc.ConvertFrom("#ffa4a4");
                }
            }
        }
        /// <summary>
        /// Действия при нажатии кнопки "Изменить"
        /// </summary>
        private void OnClickChange(object sender, RoutedEventArgs e)
        {
            if (clientItems.SelectedItem != null)
            {
                index = clientItems.SelectedIndex;

                var client = clientsRepository.ClientsList.ElementAt(index);

                if (newManager.CheckTextBoxIsNullOrEmpty(firstName.Text, "Имя")) return;
                if (newManager.CheckTextBoxIsNullOrEmpty(lastName.Text, "Фамилия")) return;
                if (newManager.CheckTextBoxIsNullOrEmpty(phone.Text, "Телефон")) return;
                if (newManager.CheckTextBoxIsNullOrEmpty(passportNumber.Text, "Номер паспорта")) return;

                bool parse = newManager.CheckParsePhone(phone.Text, out long phoneNumber);
                if (parse && phoneNumber != 0)
                {
                    if (!newManager.CheckPhoneMatchesPattern(phone.Text)) return;
                    if (!newManager.CheckPassportMatchesPattern(passportNumber.Text)) return;

                    if (newManager.CheckPhoneExistInBase(clientsRepository.ClientsList, client, phoneNumber)) return;
                    if (newManager.CheckPassportExistInBase(clientsRepository.ClientsList, client, passportNumber.Text)) return;
                }
                else
                {
                    return;
                }

                textList = new List<string>() { lastName.Text, firstName.Text,
                                                fathersName.Text, phone.Text, passportNumber.Text };

                var fieldsList = FieldsChanged(textList, client);
                if (!String.IsNullOrEmpty(fieldsList))
                {
                    var newRecordChange = newManager.NewRecord(fieldsList, Change.DataChange.ChangingRecord, position);
                    changesRepository.ChangesList.Add(newRecordChange);
                    clientsRepository.ClientsList.RemoveAt(index);
                    client.FirstName = firstName.Text.Trim();
                    client.LastName = lastName.Text.Trim();
                    client.FathersName = fathersName.Text.Trim();
                    client.Phone = phoneNumber;
                    client.PassportNumber = passportNumber.Text.Trim();
                    clientsRepository.ClientsList.Insert(index, client);
                }
                else
                {
                    return;
                }
                clientItems.ItemsSource = null;
                recordItems.ItemsSource = null;

                clientItems.ItemsSource = clientsRepository.ClientsList;
                recordItems.ItemsSource = changesRepository.ChangesList;
            }
            else
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Действия при нажатии кнопки "Добаить клиента"
        /// </summary>
        private void OnClickAddClient(object sender, RoutedEventArgs e)
        {
            if (newManager.CheckTextBoxIsNullOrEmpty(firstName.Text, "Имя")) return;
            if (newManager.CheckTextBoxIsNullOrEmpty(lastName.Text, "Фамилия")) return;
            if (newManager.CheckTextBoxIsNullOrEmpty(phone.Text, "Телефон")) return;
            if (newManager.CheckTextBoxIsNullOrEmpty(passportNumber.Text,
                "Паспорт\" в формате \"1234-567890")) return;

            bool parse = newManager.CheckParsePhone(phone.Text, out long phoneNumber);
            if (parse && phoneNumber != 0)
            {
                if (!newManager.CheckPhoneMatchesPattern(phone.Text)) return;
                if (!newManager.CheckPassportMatchesPattern(passportNumber.Text)) return;

                if (newManager.CheckPhoneExistInBase(clientsRepository.ClientsList, phoneNumber)) return;
                if (newManager.CheckPassportExistInBase(clientsRepository.ClientsList, passportNumber.Text)) return;
            }
            else
            {
                return;
            }

            

            var newClient = newManager.AddClient(firstName.Text.Trim(),
                           lastName.Text.Trim(), fathersName.Text.Trim(),
                           phoneNumber, passportNumber.Text.Trim());
            clientsRepository.ClientsList.Add(newClient);

            clientItems.ItemsSource = null;
            recordItems.ItemsSource = null;

            var fieldsAdded = CheckFieldsAdded();
            var newRecordAddClient = newManager.NewRecord(fieldsAdded, Change.DataChange.AddNewClient, position);
            changesRepository.ChangesList.Add(newRecordAddClient);

            clientItems.ItemsSource = clientsRepository.ClientsList;
            recordItems.ItemsSource = changesRepository.ChangesList;
        }
        /// <summary>
        /// Проверка какие поля TextBox были изменены
        /// </summary>
        /// <returns>Строка с названиями полей</returns>
        public string FieldsChanged(List<string> textList, Client client)
        {
            string totalString = "";

            List<string> fieldNames = new List<string>() { "Фамилия ", "Имя ", "Отчество ", "Телефон ", "Паспорт" };

            List<string> clientProperty = new List<string>()
            {
                client.LastName.Trim(), client.FirstName.Trim(),
                client.FathersName.Trim(), client.Phone.ToString().Trim(),
                client.PassportNumber.Trim()
            };

            for (int i = 0; i < clientProperty.Count; i++)
            {
                if (!String.Equals(clientProperty[i], textList[i].Trim()))
                {
                    totalString = string.Concat(totalString, fieldNames[i]);
                }
            }
            return totalString;
        }
        /// <summary>
        /// Проверка какие поля были добавлены при создании нового клиента
        /// </summary>
        /// <returns>Строку с названиями полей</returns>
        public string CheckFieldsAdded()
        {
            string totalString = "Фамилия Имя Телефон Паспорт";

            var fathers = (TextBox)TextBoxList.Children[2];

            if (!String.IsNullOrEmpty(fathers.Text.Trim()))
            {
                totalString = "Фамилия Имя Отчество Телефон Паспорт";
            }
            return totalString;
        }
        /// <summary>
        /// Действия при нажатии на кнопку "Сохранить в файл"
        /// Сохранение клиентов и изменений в файлы
        /// </summary>
        private void OnClickSaveToFiles(object sender, RoutedEventArgs e)
        {
            if (clientsRepository.ClientsList.Count <= 0)
            {
                MessageBox.Show("Список клиентов пока пустой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                rootClients = newManager.ConvertToJsonClients(clientsRepository.ClientsList);
            }
            if (changesRepository.ChangesList.Count > 0)
            {
                rootChanges = newManager.ConvertToJsonChanges(changesRepository.ChangesList);
            }

            newManager.WriteToFile(rootClients, newManager.fileNameClients);
            newManager.WriteToFile(rootChanges, newManager.fileNameChanges);

            MessageBox.Show("Файл сохранен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        /// <summary>
        /// Очистка поля TextBox LastName при получении фокуса элементом
        /// </summary>
        private void OnFocusLastName(object sender, RoutedEventArgs e)
        {
            if (lastName.Text == "Фамилия")
            {
                lastName.Text = "";
            }
        }
        /// <summary>
        /// Очистка поля TextBox FirstName при получении фокуса элементом
        /// </summary>
        private void OnFocusFirstName(object sender, RoutedEventArgs e)
        {
            if (firstName.Text == "Имя")
            {
                firstName.Text = "";
            }
        }
        /// <summary>
        /// Очистка поля TextBox FathersName при получении фокуса элементом
        /// </summary>
        private void OnFocusFathersName(object sender, RoutedEventArgs e)
        {
            if (fathersName.Text == "Отчество")
            {
                fathersName.Text = "";
            }
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
        /// Очистка поля TextBox Phone при получении фокуса элементом
        /// </summary>
        private void OnFocusPassportNumber(object sender, RoutedEventArgs e)
        {
            if (passportNumber.Text == "Паспорт")
            {
                passportNumber.Text = "";
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
