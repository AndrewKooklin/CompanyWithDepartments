using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWithDepartments
{
    [Serializable]
    [JsonObject]
    public class Change
    {
        public Change(string totalString, DataChange dataChange, Employee.Position position)
        {
            DateTime dateTime = DateTime.Now;
            this.DateAndTime = dateTime;
            this.FieldsChanged = totalString;

            switch (dataChange)
            {
                case DataChange.ChangingRecord:
                    {
                        this.Type = "Изменение данных";
                        break;
                    }
                case DataChange.AddNewClient:
                    {
                        this.Type = "Добавление клиента";
                        break;
                    }
                case DataChange.DeleteClient:
                    {
                        this.Type = "Удаление клиента";
                        break;
                    }
                default:
                    {
                        this.Type = "Действие нераспознано";
                        break;
                    }
            }

            switch (position)
            {
                case Employee.Position.Consultant:
                    {
                        this.Position = "Консультант";
                        break;
                    }
                case Employee.Position.Manager:
                    {
                        this.Position = "Менеджер";
                        break;
                    }
                default:
                    {
                        this.Position = "Неизвестный";
                        break;
                    }
            }
        }

        [JsonConstructor]
        public Change(DateTime dateTime, string totalString, string type, string position)
        {
            this.DateAndTime = dateTime;
            FieldsChanged = totalString;
            Type = type;
            Position = position;
        }

        public enum DataChange
        {
            ChangingRecord,
            AddNewClient,
            DeleteClient
        }

        [JsonProperty]
        public DateTime DateAndTime { get; set; }
        [JsonProperty]
        public string FieldsChanged { get; set; }
        [JsonProperty]
        public string Type { get; set; }
        [JsonProperty]
        public string Position { get; set; }
    }
}
