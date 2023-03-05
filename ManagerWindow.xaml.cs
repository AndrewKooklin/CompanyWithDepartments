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
        public DepartmentsRepository departmentRepository;
        public ChangesRepository changesRepository;

        Department newDepartment;
        Manager newManager = new Manager();
        Client client;
        public int index;
        public Employee.Position position;

        public ManagerWindow()
        {
            InitializeComponent();

            clientItems.ItemsSource = null;
            recordItems.ItemsSource = null;

            position = Employee.Position.Manager;

            departmentRepository = new DepartmentsRepository(this);
            changesRepository = new ChangesRepository(this);

            departmentRepository.Departments = newManager.GetDepartmentsItemSourse(position);
            if (departmentRepository.Departments != null)
            {
                cbDepartment.ItemsSource = departmentRepository.Departments;
                

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
                departmentRepository.Departments = new List<Department>();
                MessageBox.Show("Файл департаментов пока пустой." +
                                "\n Добавьте департамент и сохраните.", 
                                "Ошибка", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Error);
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
            if(cbDepartment.SelectedItem == null)
            {
                MessageBox.Show("Выберите департамент",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            if(clientItems.SelectedItem == null)
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

                if (newManager.CheckTextBoxIsNullOrEmpty(firstName.Text, "Имя")) return;
                if (newManager.CheckTextBoxIsNullOrEmpty(lastName.Text, "Фамилия")) return;
                if (newManager.CheckTextBoxIsNullOrEmpty(fathersName.Text, "Отчество")) return;
                if (newManager.CheckTextBoxIsNullOrEmpty(phone.Text, "Телефон")) return;
                if (newManager.CheckTextBoxIsNullOrEmpty(passportNumber.Text, "Номер паспорта")) return;

                bool parse = newManager.CheckParsePhone(phone.Text, out long phoneNumber);
                if (parse && phoneNumber != 0)
                {
                    if (!newManager.CheckPhoneMatchesPattern(phone.Text)) return;
                    if (!newManager.CheckPassportMatchesPattern(passportNumber.Text)) return;

                    if (newManager.CheckPhoneExistInBase(clientsList, client, phoneNumber)) return;
                    if (newManager.CheckPassportExistInBase(clientsList, client, passportNumber.Text)) return;
                }
                else
                {
                    return;
                }

                var textList = new List<string>() { lastName.Text, firstName.Text,
                                                fathersName.Text, phone.Text, passportNumber.Text };

                var fieldsList = FieldsChanged(textList, client);
                if (!String.IsNullOrEmpty(fieldsList))
                {
                    var newRecordChange = newManager.NewRecord(fieldsList, Change.DataChange.ChangingRecord, position);
                    changesRepository.ChangesList.Add(newRecordChange);
                    departmentRepository.Departments[indexDepartment].Clients.RemoveAt(indexClient);
                    client.FirstName = firstName.Text.Trim();
                    client.LastName = lastName.Text.Trim();
                    client.FathersName = fathersName.Text.Trim();
                    client.Phone = phoneNumber;
                    client.PassportNumber = passportNumber.Text.Trim();
                    departmentRepository.Departments[indexDepartment].Clients.Insert(indexClient, client);
                }
                else
                {
                    return;
                }
                clientItems.ItemsSource = null;
                recordItems.ItemsSource = null;

                clientItems.ItemsSource = departmentRepository.Departments[indexDepartment].Clients;
                recordItems.ItemsSource = changesRepository.ChangesList;
            }
        }

        /// <summary>
        /// Действия при нажатии кнопки "Добаить клиента"
        /// </summary>
        private void OnClickAddClient(object sender, RoutedEventArgs e)
        {
            if(cbDepartment.SelectedItem == null)
            {
                MessageBox.Show("Выберите департамент", 
                                 "Ошибка", 
                                 MessageBoxButton.OK, 
                                 MessageBoxImage.Error);
                return;
            }

            var selectedItem = (Department)cbDepartment.SelectedItem;
            string value = selectedItem.NameDepartment;

            

            newDepartment = departmentRepository.Departments.Single(p => p.NameDepartment.Equals(value));

            if (newManager.CheckTextBoxIsNullOrEmpty(firstName.Text, "Имя")) return;
            if (newManager.CheckTextBoxIsNullOrEmpty(lastName.Text, "Фамилия")) return;
            if (newManager.CheckTextBoxIsNullOrEmpty(fathersName.Text, "Отчество")) return;
            if (newManager.CheckTextBoxIsNullOrEmpty(phone.Text, "Телефон")) return;
            if (newManager.CheckTextBoxIsNullOrEmpty(passportNumber.Text,
                "Паспорт\" в формате \"1234-567890")) return;

            bool parse = newManager.CheckParsePhone(phone.Text, out long phoneNumber);
            if (parse && phoneNumber != 0)
            {
                if (!newManager.CheckPhoneMatchesPattern(phone.Text)) return;
                if (!newManager.CheckPassportMatchesPattern(passportNumber.Text)) return;

                if (newManager.CheckPhoneExistInBase(newDepartment.Clients, phoneNumber)) return;
                if (newManager.CheckPassportExistInBase(newDepartment.Clients, passportNumber.Text)) return;
            }
            else
            {
                return;
            }



            var newClient = newManager.AddClient(firstName.Text.Trim(),
                           lastName.Text.Trim(), fathersName.Text.Trim(),
                           phoneNumber, passportNumber.Text.Trim());

            if(newDepartment.Clients == null)
            {
                newDepartment = new Department(value, new List<Client>());
            }

            newDepartment.Clients.Add(newClient);

            departmentRepository.Departments[cbDepartment.SelectedIndex].Clients = newDepartment.Clients;

            clientItems.ItemsSource = null;
            recordItems.ItemsSource = null;

            var fieldsAdded = CheckFieldsAdded();
            var newRecordAddClient = newManager.NewRecord(fieldsAdded, 
                                                          Change.DataChange.AddNewClient, 
                                                          position);
            changesRepository.ChangesList.Add(newRecordAddClient);

            clientItems.ItemsSource = newDepartment.Clients;
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
            string rootChanges = "";

            string rootDepartments = "";

            if (departmentRepository.Departments.Count <= 0)
            {
                MessageBox.Show("Список департаментов пока пустой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                rootDepartments = newManager.ConvertToJsonDepartment(departmentRepository.Departments);
            }
            if (changesRepository.ChangesList.Count > 0)
            {
                rootChanges = newManager.ConvertToJsonChanges(changesRepository.ChangesList);
            }

            newManager.WriteToFile(rootDepartments, newManager.fileNameDepartments);
            newManager.WriteToFile(rootChanges, newManager.fileNameChanges);

            MessageBox.Show("Файл сохранен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Очистка поля TextBox "NameDepartment" при получении фокуса элементом
        /// </summary>
        private void OnFocustbNameDepartment(object sender, RoutedEventArgs e)
        {
            if (nameDepartment.Text == "Имя департамента")
            {
                nameDepartment.Text = "";
            }
        }

        /// <summary>
        /// Очистка поля TextBox "LastName" при получении фокуса элементом
        /// </summary>
        private void OnFocusLastName(object sender, RoutedEventArgs e)
        {
            if (lastName.Text == "Фамилия")
            {
                lastName.Text = "";
            }
        }
        /// <summary>
        /// Очистка поля TextBox "FirstName" при получении фокуса элементом
        /// </summary>
        private void OnFocusFirstName(object sender, RoutedEventArgs e)
        {
            if (firstName.Text == "Имя")
            {
                firstName.Text = "";
            }
        }
        /// <summary>
        /// Очистка поля TextBox "FathersName" при получении фокуса элементом
        /// </summary>
        private void OnFocusFathersName(object sender, RoutedEventArgs e)
        {
            if (fathersName.Text == "Отчество")
            {
                fathersName.Text = "";
            }
        }
        /// <summary>
        /// Очистка поля TextBox "Phone" при получении фокуса элементом
        /// </summary>
        private void OnFocusPhone(object sender, RoutedEventArgs e)
        {
            if (phone.Text == "Телефон")
            {
                phone.Text = "";
            }
        }
        /// <summary>
        /// Очистка поля TextBox "PassportNumber" при получении фокуса элементом
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

        /// <summary>
        /// Действия при выборе департамента
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
        /// Действия при нажатии на кнопку "Добавить департамент"
        /// </summary>
        private void OnClickAddDepartment(object sender, RoutedEventArgs e)
        {
            if(nameDepartment.Text.Length <= 0 || nameDepartment.Text == "")
            {
                MessageBox.Show("Имя департамента не может быть пустой строкой.",
                                "Ошибка",
                                 MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach(var departmentName in departmentRepository.Departments)
            {
                if(departmentName.NameDepartment.Equals(nameDepartment.Text))
                {
                    MessageBox.Show("Такой департамент уже есть в базе.",
                                    "Ошибка", 
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            newDepartment = new Department(nameDepartment.Text);
            departmentRepository.Departments.Add(newDepartment);
            cbDepartment.ItemsSource = departmentRepository.Departments;
            cbDepartment.Items.Refresh();
            MessageBox.Show("Департамент добавлен в список.",
                                    "Сообщение",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Действия при нажатии на кнопку "Удалить департамент"
        /// </summary>
        private void OnClickDeleteDepartment(object sender, RoutedEventArgs e)
        {
            if (cbDepartment.SelectedItem == null)
            {
                MessageBox.Show("Выберите департамент",
                                 "Ошибка",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Error);
                return;
            }

            var indexDepartment = cbDepartment.SelectedIndex;
            string nameDepartment = departmentRepository.Departments[indexDepartment].NameDepartment;

            MessageBoxResult messageBoxResult = MessageBox.Show($"Департамент \"{nameDepartment}\" и клиенты будут удалены",
                                 "Предупреждение",
                                 MessageBoxButton.OKCancel,
                                 MessageBoxImage.Warning);
            if (messageBoxResult == MessageBoxResult.Cancel)
            {
                return;
            }

            if (messageBoxResult == MessageBoxResult.OK) 
            {
                
                departmentRepository.Departments.RemoveAt(indexDepartment);
                clientItems.ItemsSource = null;
                clientItems.Items.Refresh();
                cbDepartment.Items.Refresh();
            }
            
        }
    }
}
