using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Helpers
{
    public interface IMailHelper
    {

        //parametros:
        /*a quien se lo envío,
         cual va a ser el aunto
         y el cuerpo del mensaje*/
        void SendMail(string to, string subject, string body);


    }
}
