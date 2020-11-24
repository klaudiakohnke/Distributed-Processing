using IdentityModel.Client;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PR.Client
{
    class Program
    {
        public static object JsonConvert { get; private set; }
        static async Task Main(string[] args)
        {
            bool showMenu = true;
            while(showMenu)
            {
                showMenu = ShowMenu();
            }
        }

        private static bool ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("Wybierz z menu:");
            Console.WriteLine("1) Dodaj pacjenta");
            Console.WriteLine("2) Wyswietl liste pacjentow");
            Console.WriteLine("3) Wywolaj wyjatek");
            Console.WriteLine("4) Wyjscie");
            Console.Write("\r\nWybierz: ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddPatient();
                    return true;
                case "2":
                    ShowPatients();
                    return true;
                case "3":
                    PutPatientException();
                    return true;
                case "4":
                    return false;
                default:
                    return true;
            }
        }


            public static async void AddPatient()
        {
            Console.WriteLine("[DODAJ PACJENTA]");
            Console.WriteLine("First Name:");
            string FirstNameRl = Console.ReadLine();
            Console.WriteLine("Surname:");
            string SurnameRl = Console.ReadLine();
            Console.WriteLine("Age:");
            string AgeRl = Console.ReadLine();
            Console.WriteLine("Email:");
            string email = Console.ReadLine();

            HttpClient client = new HttpClient();

            var app = PublicClientApplicationBuilder.Create("fce95216-40e5-4a34-b041-f287e46532be")
                .WithAuthority("https://login.microsoftonline.com/146ab906-a33d-47df-ae47-fb16c039ef96/v2.0/")
                .WithDefaultRedirectUri()
                .Build();

            var result =  await app.AcquireTokenInteractive(new[] { "api://fce95216-40e5-4a34-b041-f287e46532be/.default" }).ExecuteAsync();

            string token = result.AccessToken;

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + token);

            Patient p = new Patient()
            {
                FirstName = FirstNameRl,
                Surname = SurnameRl,
                PositiveTestDate = DateTime.Now,
                Age = Int16.Parse(AgeRl),
                Email = email
            };

            string patientJson = System.Text.Json.JsonSerializer.Serialize(p);

            await client.PostAsync("https://localhost:5001/api/patients",
                 new StringContent(patientJson, Encoding.UTF8, "application/json"));
        }

        public static async void PutPatientException()
        {
            Console.WriteLine("[DODAJ PACJENTA]");
            Console.WriteLine("First Name:");
            string FirstNameRl = Console.ReadLine();
            Console.WriteLine("Surname:");
            string SurnameRl = Console.ReadLine();
            Console.WriteLine("Age:");
            string AgeRl = Console.ReadLine();
            Console.WriteLine("Email:");
            string email = Console.ReadLine();

            HttpClient client = new HttpClient();

            var app = PublicClientApplicationBuilder.Create("fce95216-40e5-4a34-b041-f287e46532be")
                .WithAuthority("https://login.microsoftonline.com/146ab906-a33d-47df-ae47-fb16c039ef96/v2.0/")
                .WithDefaultRedirectUri()
                .Build();

            var result = await app.AcquireTokenInteractive(new[] { "api://fce95216-40e5-4a34-b041-f287e46532be/.default" }).ExecuteAsync();

            string token = result.AccessToken;

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + token);

            Patient p = new Patient()
            {
                FirstName = FirstNameRl,
                Surname = SurnameRl,
                PositiveTestDate = DateTime.Now,
                Age = Int16.Parse(AgeRl),
                Email = email
            };

            string patientJson = System.Text.Json.JsonSerializer.Serialize(p);

            await client.PutAsync("https://localhost:5001/api/patients",
                 new StringContent(patientJson, Encoding.UTF8, "application/json"));
        }

        public static void ShowPatients()
        {

            WebClient webclient = new WebClient();
            string response = webclient.DownloadString("https://localhost:5001/api/patients");

            JToken parsedResponse = JToken.Parse(response);

            Console.WriteLine(parsedResponse);
            Console.WriteLine("Nacisnij klawisz");
            Console.ReadKey();
        }
    }
    }

    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime PositiveTestDate { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }
