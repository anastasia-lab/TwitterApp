using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;

namespace TwitInterf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TwitterHelper twitterHelper;
        DataBasedHelper dataBasedHelper;

        public MainWindow()
        {
            InitializeComponent();
            TwitterWindow();
        }

        private void TextBoxSearchText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void TwitterWindow()
        {
            twitterHelper = new TwitterHelper(); // которая позволит работать с данными из твиттера
            dataBasedHelper = new DataBasedHelper();// позволит использовать данные бд

            twitterHelper.Authorization();

            /// <summary>
            /// Получение твитов из ленты пользователя (имя, тескт, никнейм) и сохранение в бд
            /// </summary>
            #region Tweets
            var tweets = twitterHelper.GetTweets();

            var twitList = new List<string>();

            for (int i = 0; i < tweets.Count; i++)
            {
                var name = tweets[i].User.Name;
                var screenNameTweet = tweets[i].User.ScreenName;
                var text = tweets[i].Text;
                var dateTwit = tweets[i].CreatedDate;

                if (!string.IsNullOrEmpty(screenNameTweet) && !screenNameTweet.StartsWith("@"))
                {
                    screenNameTweet = "@" + screenNameTweet;
                }

                twitList.Add(tweets[i].User.Name + "\n" + screenNameTweet + "\n" + text);

                using (var db = new TwitterAppContext()) // сохранение в бд
                {
                    db.twitterTexts.Add(new TwitterTextDb { Text = text, Date = dateTwit });
                    db.users.Add(new UsersDb { UserName = name, ScreenName = screenNameTweet });
                    db.SaveChanges();
                }

            }

            var twitTask = await Task.Run(() => twitList);

            //var DataBasedTweets = dataBasedHelper.Reader();
            //twitterText.ItemsSource = DataBasedTweets;

            /// <summary>
            /// вывод (из бд) последних 10 сохраненных твитов пользователя по дате и по Id
            /// </summary>
            using (var db = new TwitterAppContext())
            {
                List<string> listOut = new List<string>();

                var tweetsOutputId = db.twitterTexts.Where(t => t.Id > 0).OrderByDescending(t => t.Id).Take(10); // выборка по Id 
                var tweetsOutputDate = db.twitterTexts.Where(t => t.Date >= new DateTime(2019, 12, 30)).OrderByDescending(t => t.Date).Take(10); //выборка по дате

                foreach (var tweetText in tweetsOutputId)
                {

                    listOut.Add(tweetText.Text + tweetText.UsersDb);
                }

                var twitTaskDB = await Task.Run(() => listOut);
                twitterText.ItemsSource = twitTaskDB;
            }

            #endregion

            /// <summary>
            /// вывод трендов для пользователя
            /// </summary>
            #region Trends

            var trends = twitterHelper.GetTrends();
            var tweetTrends = trends.Select(tr => new { tr.Name }).ToList();

            foreach (var trend in tweetTrends)
            {
                var trendsTask = Task.Run(() => trend.Name);
                Trends.ItemsSource = await trendsTask;
            }

            #endregion


            /// <summary>
            /// фото профиля пользователя
            /// </summary>
            #region Image
            var UserImageTwitter = twitterHelper.GetImage();

            var ImageTask = Task.Run(() => UserImageTwitter);
            TwitterImage.Source = await ImageTask;

            #endregion


            /// <summary>
            /// инофрмация о пользователе
            /// </summary>
            #region UserInfo
            var users = twitterHelper.GetUser();

            var UserName = await Task.Run(() => users.Name);
            textBlockName.Text = UserName;

            #region ScreenName
            var UserScreenName = users.ScreenName;

            if (!string.IsNullOrEmpty(UserScreenName) && !UserScreenName.StartsWith("@"))
            {
                UserScreenName = "@" + UserScreenName;
            }

            var ScrenNameUser = await Task.Run(() => UserScreenName);
            textBlockScreenName.Text = ScrenNameUser;

            #endregion

            var CountFollowers = await Task.Run(() => users.FollowersCount); // число подписчиков
            textBlockFollower.Text = Convert.ToString(CountFollowers);

            var FriendCount = await Task.Run(() => users.FriendsCount); // число подписок
            textBlockFriend.Text = Convert.ToString(FriendCount);

            #endregion

        }
    }
}
