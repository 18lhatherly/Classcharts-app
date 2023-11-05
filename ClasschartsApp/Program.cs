using RestSharp;
using System.Text.Json;

namespace Classcharts_app
{
    internal class Program
    {
        public static int Main(string[] args)
        {
            const int pupilID = 4768732;
            const string loginEmail = "emma.hatherly@gmail.com";
            //const string recipientEmail = "emma@hatherly.com";
            const string recipientName = "Emma Hatherly";
            const string senderEmail = "classcharts@hatherly.com";

            if (args.Length != 3)
            {
                System.Console.WriteLine("Please call this with three parameters - your classcharts password, and the Brevo API key, and the recipient email address");
                return 1;
            }

            string password = args[0];
            string APIkey = args[1];
            string recipientEmail = args[2];

            SentAnnouncements sentAnnouncements = new SentAnnouncements();

            Console.WriteLine("Checking for new ClassCharts Announcements");

            var options = new RestClientOptions("https://www.classcharts.com/apiv2parent");
            RestClient client = new RestClient(options);
            var request = new RestRequest("login", Method.Post);
            request.AddParameter("email", loginEmail, ParameterType.GetOrPost);
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
                Console.WriteLine("Processing Announcement with title: " + announcement.title);

                if (sentAnnouncements.alreadySent(announcement))
                {
                    Console.WriteLine("Skipping - Announcement sent previously");
                }
                else
                {
                    sentAnnouncements.addToSent(announcement);
                    var emailOptions = new RestClientOptions("https://api.brevo.com/v3/smtp");
                    RestClient emailClient = new RestClient(emailOptions);
                    var emailRequest = new RestRequest("email", Method.Post);
                    emailRequest.AddHeader("Accept", "application/json");
                    emailRequest.AddHeader("api-key", APIkey);
                    Email email = new Email();
                    email.sender = new Sender("Classcharts", senderEmail);
                    email.to = new System.Collections.Generic.List<Sender>();
                    email.to.Add(new Sender(recipientName, recipientEmail));
                    email.subject = announcement.title;
                    email.htmlContent = "<p><b>Announcement Date Sent: " + announcement.timestamp + "</b></p>" +
                                        "<p><b>Sender: " + announcement.teacher_name + "</b></p>";

                    foreach (Attachment attachment in announcement.attachments)
                    {
                        if (email.attachment == null)
                        {
                            email.attachment = new System.Collections.Generic.List<EmailAttachment>();
                        }
                        email.attachment.Add(new EmailAttachment(attachment.filename, attachment.url));
                    }
                    email.htmlContent = email.htmlContent + "<p></p>" + announcement.description;

                    emailRequest.AddJsonBody(email);
                    Task<RestResponse> t = emailClient.PostAsync(emailRequest);
                    var emailResponse = t.Result;
                    Console.WriteLine(emailResponse.Content);
                }
            }
            sentAnnouncements.StoreSent();
            Console.WriteLine("Processing Complete");
            return 0;
        }
    }
}
