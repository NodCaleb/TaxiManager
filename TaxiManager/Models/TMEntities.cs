using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaxiManager.Models
{
    public class TMRideRequest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Время создания")]
        public DateTime CreationTime { get; set; }
        [Display(Name = "Время завершения")]
        public DateTime? FinishTime { get; set; }
        [Display(Name = "Откуда")]
        public string StartLocation { get; set; }
        [Display(Name = "Куда")]
        public string FinalDestination { get; set; }
        [Display(Name = "Статус заявки")]
        public int Status { get; set; }
        //0 - поиск водителя, 1 - ожидание, 2- выполняется, 3 - отменена, 4 - выполнена
        [Display(Name = "UserGUID")]
        public string UserGUID { get; set; }
        [Display(Name = "Водитель")]
        public ApplicationUser Driver { get; set; }

        [NotMapped]
        public string StatusDescription
        {
            get
            {
                switch (Status)
                {
                    case 0: return "Поиск водителя";
                    case 1: return "Ожидание";
                    case 2: return "Выполняется";
                    case 3: return "Отменена";
                    case 4: return "Выполнена";
                    default: return "Неизвестен";
                }
            }
        }

        [NotMapped]
        public string DriverName
        {
            get
            {
                return Driver != null ? Driver.PersonalName : "Нет";
            }
        }
    }
}