using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace CompanyWithDepartments
{
    public class Consultant : Employee, IConcultant,
                              IGetRecordsFromBase, IWorkWithJson,
                              IWorkWithFiles, ICheckMethods
    {
        public Consultant()
        {

        }

        /// <summary>
        /// Получение списка объектов "Клиент" из файла
        /// </summary>
        public ObservableCollection<Client> GetClietsItemSourse(Position position)
        {
            var clientsRepository = new ObservableCollection<Client>();

            string textFromFileClients = ReadFromFile(fileNameClients);
            if (!String.IsNullOrEmpty(textFromFileClients))
            {
                clientsRepository = ParseJsonClients(textFromFileClients, position);
                if (clientsRepository != null)
                {
                    return clientsRepository;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Получение списка объектов "Запись" из файла
        /// </summary>
        public ObservableCollection<Change> GetChangesItemSourse()
        {
            var changesRepository = new ObservableCollection<Change>();

            string textFromFileChanges = ReadFromFile(fileNameChanges);
            if (!String.IsNullOrEmpty(textFromFileChanges))
            {
                changesRepository = ParseJsonChanges(textFromFileChanges);
                if (changesRepository != null)
                {
                    return changesRepository;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Преобразование объектов "Клиент" к json строке
        /// </summary>
        public string ConvertToJsonClients(ObservableCollection<Client> clients)
        {
            TreeJsonClient treeJsonClients;
            string rootClients;

            if (clients != null)
            {
                treeJsonClients = new TreeJsonClient(clients)
                {
                    ClientsList = clients
                };
                rootClients = JsonConvert.SerializeObject(treeJsonClients, Formatting.Indented);
            }
            else
            {
                MessageBox.Show("Нет данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            Debug.WriteLine(rootClients);
            return rootClients;
        }

        /// <summary>
        /// Преобразование объектов "Запись" к json строке
        /// </summary>
        public string ConvertToJsonChanges(ObservableCollection<Change> dataChanges)
        {
            TreeJsonChanges treeJsonChanges;
            string rootChanges;

            if (dataChanges != null)
            {
                treeJsonChanges = new TreeJsonChanges(dataChanges)
                {
                    DataChangeList = dataChanges
                };
                rootChanges = JsonConvert.SerializeObject(treeJsonChanges, Formatting.Indented);
            }
            else
            {
                MessageBox.Show("Нет данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            Debug.WriteLine(rootChanges);
            return rootChanges;
        }

        /// <summary>
        /// Приведение json строки к объектам "Клиент"
        /// </summary>
        public ObservableCollection<Client> ParseJsonClients(string root, Position position)
        {
            ObservableCollection<Client> clients = new ObservableCollection<Client>();

            JObject json = JObject.Parse(root);

            var jToken = json["ClientsList"];

            var clientsList = jToken.ToObject<ObservableCollection<Client>>();

            foreach (var client in clientsList)
            {
                client.PassportNumber = client.GetPassportNumber(position);
                clients.Add(client);
            }
            if (clients == null || clients.Count <= 0)
            {
                MessageBox.Show("Список клиентов пустой", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            return clients;
        }

        /// <summary>
        /// Приведение json строки к объектам "Запись"
        /// </summary>
        public ObservableCollection<Change> ParseJsonChanges(string root)
        {
            if (String.IsNullOrEmpty(root))
            {
                return null;
            }
            ObservableCollection<Change> dataChanges = new ObservableCollection<Change>();

            JObject json = JObject.Parse(root);

            var jToken = json["DataChangeList"];

            var changes = jToken.ToObject<ObservableCollection<Change>>();

            foreach (var change in changes)
            {
                dataChanges.Add(change);
            }
            if (dataChanges == null || dataChanges.Count <= 0)
            {
                MessageBox.Show("Список изменений пустой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            return dataChanges;
        }

        /// <summary>
        /// Запись json строки в файл
        /// </summary>
        public void WriteToFile(string root, string fileName)
        {
            string path = Directory.GetCurrentDirectory() + fileName;

            if (string.IsNullOrEmpty(root))
            {
                return;
            }

            File.WriteAllText(path, root);
        }

        /// <summary>
        /// Чтение json строки из файла
        /// </summary>
        public string ReadFromFile(string fileName)
        {
            string jsonString = "";
            string path = Directory.GetCurrentDirectory() + fileName;
            if (File.Exists(path))
            {
                jsonString = File.ReadAllText(path);
                if (String.IsNullOrEmpty(jsonString) || jsonString.Length <= 0)
                {
                    MessageBox.Show("Файл пока пустой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return jsonString;
            }
            else
            {
                MessageBox.Show("Файл еще не создан. Добавьте клиента и сохраните", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return jsonString;
        }

        /// <summary>
        /// Проверка на пустоту поля TextBox
        /// </summary>
        public bool CheckTextBoxIsNullOrEmpty(string text, string name)
        {
            bool check;
            if (String.IsNullOrEmpty(text))
            {
                MessageBox.Show($"Заполните поле \"{name}\"", "Ошибка", MessageBoxButton.OK);
                check = true;
                return check;
            }
            else
            {
                check = false;
                return check;
            }
        }

        /// <summary>
        /// Проверка приведения введеного телефона к типу long
        /// </summary>
        public bool CheckParsePhone(string phoneText, out long phoneNumber)
        {
            bool parsePhone = long.TryParse(phoneText.Trim(), out long phoneNumberTemp);
            if (parsePhone && phoneNumberTemp > 10000000000 && phoneNumberTemp < 99999999999)
            {
                phoneNumber = phoneNumberTemp;
                parsePhone = true;
                return parsePhone;
            }
            else
            {
                MessageBox.Show("Поле \"Телефон\" должно состоять из 11 цифр", "Ошибка", MessageBoxButton.OK);
                parsePhone = false;
                phoneNumber = 0;
                return parsePhone;
            }
        }

        /// <summary>
        /// Проверка на соответствие формату введенного телефона
        /// </summary>
        public bool CheckPhoneMatchesPattern(string phoneText)
        {
            bool check;
            Regex regex = new Regex(@"\d{11}$");
            if (regex.IsMatch(phoneText.Trim()))
            {
                check = true;
                return check;
            }
            else
            {
                MessageBox.Show("Поле \"Телефон\" должно состоять из 11 цифр", "Ошибка", MessageBoxButton.OK);
                check = false;
                return check;
            }
        }

        /// <summary>
        /// Проверка на соответствие формату введенного паспорта
        /// </summary>
        public bool CheckPassportMatchesPattern(string passportNumberText)
        {
            bool check;
            Regex regex = new Regex(@"\d\d\d\d-\d\d\d\d\d\d$");
            if (regex.IsMatch(passportNumberText.Trim()))
            {
                check = true;
                return check;
            }
            else
            {
                MessageBox.Show("Поле \"Номер паспорта\" должно быть в формате 1234-567890", "Ошибка", MessageBoxButton.OK);
                check = false;
                return check;
            }
        }

        /// <summary>
        /// Проверка на наличие измененного телефона выбранного клиента в базе
        /// </summary>
        public bool CheckPhoneExistInBase(ObservableCollection<Client> clients, Client client, long phoneNumber)
        {
            bool exist = false;

            foreach (var item in clients)
            {
                if (item == client)
                {
                    continue;
                }
                if (item.Phone != phoneNumber)
                {
                    continue;
                }
                else
                {
                    MessageBox.Show("Такой номер телефона уже есть в базе", "Ошибка", MessageBoxButton.OK);
                    exist = true;
                    return exist;
                }
            }
            return exist;
        }

        /// <summary>
        /// Проверка на наличие введенного телефона в базе
        /// </summary>
        public bool CheckPhoneExistInBase(ObservableCollection<Client> clients, long phoneNumber)
        {
            bool exist = false;

            if (clients == null)
            {
                return false;
            }

            foreach (var item in clients)
            {
                if (item.Phone != phoneNumber)
                {
                    continue;
                }
                else
                {
                    MessageBox.Show("Такой номер телефона уже есть в базе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    exist = true;
                    return exist;
                }
            }
            return exist;
        }

        /// <summary>
        /// Проверка на наличие измененного паспорта выбранного клиента в базе
        /// </summary>
        public bool CheckPassportExistInBase(ObservableCollection<Client> clients, Client client, string passportNumberText)
        {
            bool exist = false;
            foreach (var item in clients)
            {
                if (item == client)
                {
                    continue;
                }

                bool compare = String.Equals(item.PassportNumber, passportNumberText.Trim());

                if (!compare)
                {
                    continue;
                }
                else
                {
                    MessageBox.Show("Такой номер паспорта уже есть в базе", "Ошибка", MessageBoxButton.OK);
                    exist = true;
                    return exist;
                }
            }
            return exist;
        }

        /// <summary>
        /// Проверка на наличие введенного паспорта в базе
        /// </summary>
        public bool CheckPassportExistInBase(ObservableCollection<Client> clients, string passportNumberText)
        {
            bool exist = false;

            if (clients == null)
            {
                return false;
            }

            foreach (var item in clients)
            {
                bool compare = String.Equals(item.PassportNumber, passportNumberText.Trim());

                if (!compare)
                {
                    continue;
                }
                else
                {
                    MessageBox.Show("Такой номер паспорта уже есть в базе", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    exist = true;
                    return exist;
                }
            }
            return exist;
        }
    }
}
