using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;

namespace TwitInterf
{
    /// <summary>
    /// класс базы данных
    /// </summary>
    class TwitterAppContext :DbContext
    {
        public TwitterAppContext() //подключение к бд
          : base(@"Data Source= .\SQLEXPRESS;Initial Catalog=Entity.TwitterDB;Integrated Security=True")
        {
        }

        public DbSet<HashTagDb> hashTags { get; set; } // для хранения хэштегов
        public DbSet<UsersDb> users { get; set; } // хранение никнеймов и имен пользователей
        public DbSet<TwitterTextDb> twitterTexts { get; set; } // хранение текста твита, датыЫ создания

    }
}
