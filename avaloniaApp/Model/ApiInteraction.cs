using Avalonia;
using Avalonia.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Dynamic;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using avaloniaApp.ViewModels;
using avaloniaApp.Views;
using Avalonia.Controls.ApplicationLifetimes;
using System.Net;

namespace avaloniaApp.Model
{
    internal static class ApiInteraction
    {
        private static string _antiforgeryToken;
        private static string _cookieForVerificationToken;

        public async static Task<(bool, string)> Login(string login, string password)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7170/api/users/token");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("Login", login));
            collection.Add(new("Password", password));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await client.SendAsync(request);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex) 
            {
                return (false, "login failed");            
            }
            return (true, await response.Content.ReadAsStringAsync());
        }

        public async static Task<List<GridData>> GetData(int take, int skip)
        {
            var token = GetAccessToken();
            if (!token.Item1)
                (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow.Content = new MainView{ DataContext = new MainViewModel() };
            
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7170/api/data/grid?take={take}&skip={skip}");
            request.Headers.Add("Authorization", "Bearer "+ token.Item2);
            var response = await client.SendAsync(request);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                return null;
            }
            var jObjectForData = JObject.Parse(await response.Content.ReadAsStringAsync());
            _antiforgeryToken = jObjectForData["token"].ToString();
            var reuslt = JsonConvert.DeserializeObject<FormatForData<GridData>>(jObjectForData["data"]!.ToString());
            _cookieForVerificationToken = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value.FirstOrDefault(cookie => cookie.Contains("VerificationToken"));
            return reuslt!.Records;
        }

        public static bool SaveData(GridData dataGrid)
        {
            var token = GetAccessToken();
            if (!token.Item1)
                (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow.Content = new MainView { DataContext = new MainViewModel() };

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7170/api/data/save");
            request.Headers.Add("Authorization", "Bearer " + token.Item2);
            request.Headers.Add("RequestVerificationToken", _antiforgeryToken);
            request.Headers.Add("Cookie", _cookieForVerificationToken);
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("Title", dataGrid.Title));
            collection.Add(new("Description", dataGrid.Description));
            collection.Add(new("Status", dataGrid.Status.ToString()));
            collection.Add(new("Id", dataGrid.Id.ToString()));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = client.SendAsync(request).Result;
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch 
            {
                return false;            
            }
            var result = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            return false;
        }

        private static bool TokentIsValid(string token)
        {
            JwtSecurityToken jwtSecurityToken;
            try
            {
                jwtSecurityToken = new JwtSecurityToken(token);
            }
            catch (Exception)
            {
                return false;
            }

            return jwtSecurityToken.ValidTo > DateTime.UtcNow;
        }

        private static (bool, string) GetAccessToken()
        {
            var fileText = File.ReadAllText("./Cache/tkSave");
            var tokens = JsonConvert.DeserializeObject(fileText) as JObject;

            var accessToken = tokens["succes"].Value<string>();
            var refreshToken = tokens["refresh"].Value<string>();

            if (TokentIsValid(accessToken))
                return (true, accessToken);
            else if (TokentIsValid(refreshToken))
            {
                var get = RefreshToken(refreshToken);
                var token = (JsonConvert.DeserializeObject(get.Item2) as JObject)["succes"];
                tokens["succes"] = token;
                File.WriteAllText("./Cache/tkSave", tokens.ToString());
                return (true, token.ToString());
            }
            return (false, "");
        }

        private static (bool, string) RefreshToken(string token)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7170/api/refresh");
            request.Headers.Add("Authorization", "Bearer " + token);
            var response = client.SendAsync(request).Result;
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                return (false, "");
            }

            return (true , response.Content.ReadAsStringAsync().Result);

        }
    }
}
