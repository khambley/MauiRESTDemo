using System;
using System.Text;
using System.Text.Json;
using System.Windows.Input;

namespace MauiRESTDemo
{
    public class MainViewModel
    {
        HttpClient client;
        JsonSerializerOptions _serializerOptions;
        string baseUrl = "https://6383d73e3fa7acb14fe8f6b2.mockapi.io";

        public MainViewModel()
        {
            client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
        }

        public ICommand GetAllUsersCommand => new Command(async () =>
        {
            var url = $"{baseUrl}/users";

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                //var content = await response.Content.ReadAsStringAsync();

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<List<User>>(responseStream, _serializerOptions);
                }
            }
            // add an else to capture http errors, i.e. 500, log it, and display a message.
        });

        public ICommand GetUserCommand => new Command(async () =>
        {
            var url = $"{baseUrl}/users/25";
            var response = await client.GetStringAsync(url);
        });

        public ICommand AddUserCommand => new Command(async () =>
        {
            var url = $"{baseUrl}/users";
            // you could create a form for the user to input their information and pass that into this method
            var user = new User
            {
                CreatedAt = DateTime.Now,
                Name = "Katherine Hambley",
                Avatar = "https://fakeimg.pl/350x200/?text=MAUI"
            };

            string json = JsonSerializer.Serialize<User>(user, _serializerOptions);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
        });
    }
}

