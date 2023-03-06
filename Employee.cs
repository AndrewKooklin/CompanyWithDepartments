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
    public abstract class Employee : IGetRecordsFromBase, IWorkWithJson,
                          IWorkWithFiles, ICheckMethods
    {
        public readonly string fileNameDepartments = "\\Departaments.json";
        public readonly string fileNameChanges = "\\Changes.json";

        protected Employee()
        {
        }

        public enum Position
        {
            Consultant,
            Manager
        }

        /// <summary>
        /// Создание новой записи изменений
        /// </summary>
        /// <returns>new Change</returns>
        public Change NewRecord(string totalString, Change.DataChange dataChange, Employee.Position position)
        {
            Change newDataChange = new Change(totalString, dataChange, position);

            return newDataChange;
        }

        /// <summary>
        /// Получение списка объектов "Клиент" из файла
        /// </summary>
        public List<Department> GetDepartmentsItemSourse(Position position)
        {
            var departmentsRepository = new List<Department>();

            string textFromFileDepartments = ReadFromFile(fileNameDepartments);
            if (!String.IsNullOrEmpty(textFromFileDepartments))
            {
                departmentsRepository = ParseJsonDepartments(textFromFileDepartments, position);
                if (departmentsRepository != null)
                {
                    return departmentsRepository;
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
        public List<Change> GetChangesItemSourse()
        {
            var changesRepository = new List<Change>();

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

        ///// <summary>
        ///// Преобразование объектов "Клиент" к json строке
        ///// </summary>
        public string ConvertToJsonDepartment(List<Department> departments)
        {
            TreeJsonDepartments treeJsonDepartments;
            string rootClients;

            if (departments != null)
            {
                treeJsonDepartments = new TreeJsonDepartments(departments)
                {
                    Departments = departments
                };
                rootClients = JsonConvert.SerializeObject(treeJsonDepartments, Formatting.Indented);
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
        public string ConvertToJsonChanges(List<Change> dataChanges)
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
        /// Приведение json строки к списку объектов "Департамент"
        /// </summary>
        public List<Department> ParseJsonDepartments(string root, Position position)
        {
            List<Department> departments = new List<Department>();

            JObject json = JObject.Parse(root);

            var jToken = json["Departments"];

            var departmentsList = jToken.ToObject<List<Department>>();

            foreach (var department in departmentsList)
            {
                departments.Add(department);
            }

            if (departments == null || departments.Count <= 0)
            {
                MessageBox.Show("Список департаментов пустой," +
                    "\nдобавьте новый департамент", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
            return departments;
        }

        /// <summary>
        /// Приведение json строки к объектам "Запись"
        /// </summary>
        public List<Change> ParseJsonChanges(string root)
        {
            if (String.IsNullOrEmpty(root))
            {
                return null;
            }
            List<Change> dataChanges = new List<Change>();

            JObject json = JObject.Parse(root);

            var jToken = json["DataChangeList"];

            var changes = jToken.ToObject<List<Change>>();

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
        public bool CheckPhoneExistInBase(List<Client> clients, Client client, long phoneNumber)
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
        public bool CheckPhoneExistInBase(List<Client> clients, long phoneNumber)
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
                    MessageBox.Show("Такой номер телефона уже есть в этом департаменте",
                                    "Ошибка", 
                                    MessageBoxButton.OK, 
                                    MessageBoxImage.Error);
                    exist = true;
                    return exist;
                }
            }
            return exist;
        }

        /// <summary>
        /// Проверка на наличие измененного паспорта выбранного клиента в базе
        /// </summary>
        public bool CheckPassportExistInBase(List<Client> clients, Client client, string passportNumberText)
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
        public bool CheckPassportExistInBase(List<Client> clients, string passportNumberText)
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
