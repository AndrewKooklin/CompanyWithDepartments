using System.Collections.ObjectModel;

namespace CompanyWithDepartments
{
     public interface ICheckMethods
     {
        /// <summary>
        /// Проверка на пустоту поля TextBox
        /// </summary>
        bool CheckTextBoxIsNullOrEmpty(string text, string name);
        /// <summary>
        /// Проверка приведения введеного телефона к типу long
        /// </summary>
        bool CheckParsePhone(string phoneText, out long phoneNumber);
        /// <summary>
        /// Проверка на соответствие формату введенного телефона
        /// </summary>
        bool CheckPhoneMatchesPattern(string phoneText);
        /// <summary>
        /// Проверка на соответствие формату введенного паспорта
        /// </summary>
        bool CheckPassportMatchesPattern(string passportNumberText);
        /// <summary>
        /// Проверка на наличие измененного телефона выбранного клиента в базе
        /// </summary>
        bool CheckPhoneExistInBase(ObservableCollection<Client> clients, Client client, long phoneNumber);
        /// <summary>
        /// Проверка на наличие введенного телефона в базе
        /// </summary>
        bool CheckPhoneExistInBase(ObservableCollection<Client> clients, long phoneNumber);
        /// <summary>
        /// Проверка на наличие измененного паспорта выбранного клиента в базе
        /// </summary>
        bool CheckPassportExistInBase(ObservableCollection<Client> clients, Client client, string passportNumberText);
        /// <summary>
        /// Проверка на наличие введенного паспорта в базе
        /// </summary>
        bool CheckPassportExistInBase(ObservableCollection<Client> clients, string passportNumberText);
     }
}
