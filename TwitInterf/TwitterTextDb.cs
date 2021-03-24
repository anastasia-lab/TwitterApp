using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TwitInterf
{
    /// <summary>
    /// класс сущности "Текст"
    /// </summary>
    class TwitterTextDb
    {

        public int Id { get; set; } //первичный ключ
        public DateTime Date { get; set; }

        [Required]// обязательные свойства
        public string Text { get; set; }

        public virtual UsersDb UsersDb { get; set; } //ссылка на текст
    
    }
}
