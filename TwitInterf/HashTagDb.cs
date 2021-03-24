using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TwitInterf
{
    /// <summary>
    /// класс сущности "Хэштег"
    /// </summary>
    class HashTagDb
    {
        public int Id { get; set; } //первичный ключ

        [Required] //обязательные поля
        public string Name { get; set; }
    }
}
