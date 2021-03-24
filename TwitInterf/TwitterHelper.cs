using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TweetSharp;

namespace TwitInterf
{
   class TwitterHelper
   {
        const string consumerKey = "ZY4BFJOOziWVfhK7d8rvBx0Fu";
        const string consumerSecret = "mUKFkJcmf9hRgYxusbvBJ66nKelYd8oqZc79OVXT3BXg774Ryb";

        static TwitterService twitterService;

        public TwitterHelper()
        { }

        /// <summary>
        /// метод для авторизации пользователя
        /// </summary>
        public void Authorization()
        {
            OAuthRequestToken requestToken;
            Uri uri;

            try
            {
                twitterService = new TwitterService(consumerKey, consumerSecret);

                requestToken = twitterService.GetRequestToken();
                if (requestToken == null)
                {
                    MessageBox.Show("Не удалось выполнить запрос.");
                }

                uri = twitterService.GetAuthorizationUri(requestToken);
                Process.Start(uri.ToString());

                var login = new LogIn();
                var result = login.ShowDialog();

                if (result == true)
                {
                    var access = twitterService.GetAccessToken(requestToken, login.Verifier);

                    twitterService.AuthenticateWith(access.Token, access.TokenSecret);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Oopss", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        ///  данный метод получает список твитов из ленты пользователя
        ///  и имена других пользователей, которые их опубликовали
        /// </summary>

        public List<TwitterStatus> GetTweets()
        {
            //Thread.Sleep(2000);
            var source = twitterService.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions()).ToList();

            List<TwitterStatus> result = new List<TwitterStatus>();

            if (source != null)
            {
                foreach (var src in source)
                {
                    result.Add(src);
                }
            }
            return result;
        }

        /// <summary>
        /// метод для получения всемирных трендов для ползователя
        /// </summary>

        public List<TwitterTrend> GetTrends()
        {
            var trends = twitterService.ListLocalTrendsFor(new ListLocalTrendsForOptions { Id = 1 }); // 1 - весь мир

            List<TwitterTrend> listTrends = new List<TwitterTrend>();

            if (trends != null)
            {
                foreach (var trend in trends)
                {
                    listTrends.Add(trend);
                }
            }

            return listTrends;
        }

        /// <summary>
        /// метод для получения ника, имени, числа читателей и подписок
        /// </summary>

        public TwitterUser GetUser()
        {
            return twitterService.GetUserProfile(new GetUserProfileOptions());
        }

        /// <summary>
        /// метод получения изображения пользователя
        /// </summary>
        /// <returns></returns>

        public ImageSource GetImage()
        {
            var ImageUrl = twitterService.GetUserProfile(new GetUserProfileOptions()).ProfileImageUrl;
            return new BitmapImage(new Uri(ImageUrl.ToString()));
        }

        /// <summary>
        /// сохранение аватарки и шапки профиля
        /// </summary>
        public void SafeImage() 
        {
            using (var twitterWebClient = new WebClient())
            {
                var user = twitterService.GetUserProfile(new GetUserProfileOptions()); //получение профиля пользователя
                var ProfileImage = user.ProfileImageUrl.ToString(); // ссылка изображения пользователя

                var BackgroundImage = user.IsProfileBackgroundTiled; // ссылка изображения шапки пользователя
                var ProfileBackgroundImage = user.ProfileBackgroundImageUrl.ToString();

                if (BackgroundImage == true) // есть ли изображение шапки пользователя
                {
                 
                  twitterWebClient.DownloadFile(ProfileBackgroundImage, user.ScreenName.ToString() +Path.GetExtension(ProfileBackgroundImage)); // загрузка шапки профиля
                }

                twitterWebClient.DownloadFile(ProfileImage, user.ScreenName.ToString() + Path.GetExtension(ProfileImage)); // загрузка изображения пользователя
            }
        }
    }
}
