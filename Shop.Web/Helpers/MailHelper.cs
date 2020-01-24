
namespace Shop.Web.Helpers
{
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Configuration;
    using MimeKit;
    public class MailHelper : IMailHelper
    {

        //EL MAIL HELPER NECESITA ACCEDER A LA CONFIGURACIÓN
        //y por eso se inyecta el IConfiguration 
        private readonly IConfiguration configuration;

        public MailHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendMail(string to, string subject, string body)
        {
            //quien manda el correo
            var from = this.configuration["Mail:From"];
            //servidor
            var smtp = this.configuration["Mail:Smtp"];
            //puerto
            var port = this.configuration["Mail:Port"];
            //password
            var password = this.configuration["Mail:Password"];

            //mensaje
            var message = new MimeMessage();
            //quien lo manda
            message.From.Add(new MailboxAddress(from));
            //a quien se lo manda
            message.To.Add(new MailboxAddress(to));
            //asunto
            message.Subject = subject;
            //instancia del cuerpo del correo
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            message.Body = bodyBuilder.ToMessageBody();

            
            //creamos una instancia SMTP y mandamos el correo
            using (var client = new SmtpClient())
            {
                client.Connect(smtp, int.Parse(port),false);
                client.Authenticate(from, password);
                client.Send(message);
                client.Disconnect(true);
            }
        }

    }
}
