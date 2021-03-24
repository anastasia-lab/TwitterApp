using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TwitInterf
{
    class DataBasedHelper
    {
        private string path = @"Data Source= .\SQLEXPRESS;Initial Catalog=TwitterDataBase;Integrated Security=True";

        public DataBasedHelper()
        { }

        /// <summary>
        /// этот метод сохраняет данные твита в бд
        /// имя пользователя, ник и текст
        /// </summary>

        public void Insert()
        {
            using (SqlConnection connection = new SqlConnection(path)) 
            {
                connection.Open();

                TwitterHelper twitterHelper = new TwitterHelper();

                var tweetText = twitterHelper.GetTweets();

                var transaction = connection.BeginTransaction();
                var insertUser = new SqlCommand("insert into Users (Name, ScreenName) values (@Name, @ScreenName)", connection);
                var insertTweet = new SqlCommand("insert into Tweets (Text) values (@Text)", connection);

                for (int i = 0; i < tweetText.Count; i++)
                {
                    insertTweet.Parameters.Add(new SqlParameter("@Text", System.Data.SqlDbType.Text));
                    insertTweet.Parameters["@Text"].Value = tweetText[i].Text;

                    insertUser.Parameters.Add(new SqlParameter("@Name", System.Data.SqlDbType.VarChar, 50));
                    insertUser.Parameters.Add(new SqlParameter("@ScreenName", System.Data.SqlDbType.VarChar, 50));
                    insertUser.Parameters["@Name"].Value = tweetText[i].User.Name;
                    insertUser.Parameters["@ScreenName"].Value = tweetText[i].User.ScreenName;
                    insertUser.Transaction = transaction;

                    insertTweet.Transaction = transaction;
                }

                try
                {
                    insertUser.ExecuteNonQuery();
                    insertTweet.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                }
            }
        }

        /// <summary>
        /// метод для чтения сохраненных данных из бд
        /// </summary>
        /// <returns></returns>

        public List<string> Reader()
        {
            using (SqlConnection connection = new SqlConnection(path))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                List<string> list = new List<string>();

                string sqlSelect = "select top 10 Text from Tweets";
                var command = new SqlCommand(sqlSelect, connection);
                command.Transaction = transaction;

                var reader = command.ExecuteReader();
                command.Dispose();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetValue(0).ToString());
                    }
                }

                return list;
            }
        }
    }
}
