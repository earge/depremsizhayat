using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.IService
{
    public interface IMailService
    {
        bool SendMail(string receiver, string subject, string body);
    }
}
