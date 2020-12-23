using DepremsizHayat.Data;
using DepremsizHayat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business
{
    public class UserAccountManager : BaseManager
    {
        public void CreateUser(UserAccountCreateModel model)
        {
            _db.USER_ACCOUNT.Add(new USER_ACCOUNT
            {
                E_MAIL = model.E_MAIL,
                FIRST_NAME = model.FIRST_NAME,
                IS_ACTIVE = true,
                PASSWORD = model.PASSWORD,
                USER_ROLE_ID = model.ROLE_ID,
                LAST_NAME = model.LAST_NAME
            });
            _db.SaveChanges();
        }
    }
}
