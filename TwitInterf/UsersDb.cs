using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TwitInterf
{
    /// <summary>
    /// класс сущности "Пользователь"
    /// </summary>
    class UsersDb
    {
        public UsersDb()
        {
            this.TwitterTexts = new List<TwitterTextDb>(); //экземпляр списка пользователей 
        }

        public int Id { get; set; } // первичный ключ

        [Required] // обязательные свойства
        public string UserName { get; set; }
        public string ScreenName { get; set; }
        public int CountFriends { get; set; }
        public int CountFollowers { get; set; }
        public string ImageUrl { get; set; }

        public virtual IList<TwitterTextDb> TwitterTexts { get; set; } // список пользователей
    }

}
