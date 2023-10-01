using RestSharp;
using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Classcharts_app
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const int pupilID = 4768732;
            string APIkey = " ";
            string password = " ";
            var options = new RestClientOptions("https://www.classcharts.com/apiv2parent");
            RestClient client = new RestClient(options);
            var request = new RestRequest("login", Method.Post);
            request.AddParameter("email", "emma.hatherly@gmail.com", ParameterType.GetOrPost);
            request.AddParameter("password", password, ParameterType.GetOrPost);
            request.AddParameter("_method", "POST", ParameterType.GetOrPost);
            request.AddParameter("recaptcha-token", "no-token-available", ParameterType.GetOrPost);
            var response = client.PostAsync(request).Result;
            var result = JsonSerializer.Deserialize<LoginResponse>(response.Content);
            string sessionID = result.meta.session_id;
            request = new RestRequest("announcements/" + pupilID);
            request.AddHeader("Authorization", "Basic " + sessionID);
            response = client.PostAsync(request).Result;
            //Console.WriteLine(response.Content);
            var announcementsResult = JsonSerializer.Deserialize<Announcements>(response.Content);
            foreach (var announcement in announcementsResult.data)
            {
                //Console.WriteLine(announcement.title);
                var emailOptions = new RestClientOptions("https://api.brevo.com/v3/smtp");
                RestClient emailClient = new RestClient(emailOptions);
                var emailRequest = new RestRequest("email", Method.Post);
                emailRequest.AddHeader("Accept", "application/json");
                emailRequest.AddHeader("api-key", APIkey);
                Email email = new Email();
                email.sender = new Sender("Classcharts", "classcharts@hatherly.com");
                email.to = new Sender("Emma Hatherly", "adam@hatherly.com");
                email.subject = announcement.title;
                byte[] htmlBytes = Encoding.Unicode.GetBytes(announcement.description);
                byte[] asciiBytes = Encoding.Convert(Encoding.Unicode,Encoding.ASCII,htmlBytes);
                string htmlAscii = Encoding.ASCII.GetString(asciiBytes);
                Console.WriteLine(htmlAscii);
                email.htmlContent = htmlAscii;
                Console.WriteLine(JsonSerializer.Serialize(email));
                emailRequest.AddJsonBody(email);
                var emailResponse = emailClient.PostAsync(emailRequest).Result;
                Console.WriteLine(emailResponse.Content);
            }

            Console.ReadLine();
        }
    }
}
