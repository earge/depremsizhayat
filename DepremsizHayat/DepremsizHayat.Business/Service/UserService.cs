using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.Business.UnitOfWork;
using DepremsizHayat.DataAccess;
using DepremsizHayat.DTO.User;
using DepremsizHayat.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.Service
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;
        IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            this._userRepository = userRepository;
            this._unitOfWork = unitOfWork;
        }
        public bool Activate(string actCode, string mail)
        {
            var user = _userRepository.GetByMail(mail);
            bool result = false;
            /* 1=user.ACTIVATONCODE olacak. */
            if (Decryptor.DecryptInt(actCode) != 1)
            {
                result = false;
            }
            else
            {
                user.ACTIVE = true;
                _unitOfWork.Commit();
                result = true;
            }
            return result;
        }
        public USER CreateUser(USER user)
        {
            user = _userRepository.Add(user);
            _unitOfWork.Commit();
            return user;
        }
        public List<USER> GetAll()
        {
            return _userRepository.GetAll();
        }

        public USER GetByMail(string mail)
        {
            return _userRepository.GetByMail(mail);
        }

        public bool Login(string mail, string pwd)
        {
            bool result = false;
            var userFromDb = _userRepository.GetByMail(mail);
            if (userFromDb != null)
            {
                if (Decryptor.Decrypt(userFromDb.PASSWORD) == Decryptor.Decrypt(pwd))
                {
                    result = true;
                }
            }
            return result;
        }

        public bool ResetPassword(ResetPasswordRequest request)
        {
            USER user = _userRepository.GetByMail(request.Mail);
            if (user != null)
            {
                user.PASSWORD = request.Password;
                _unitOfWork.Commit();
                return true;
            }
            return false;
        }

        public void SendResetMail(string mail)
        {
            if (_userRepository.GetByMail(mail) != null)
            {
                var subject = "Şifre Sıfırlama Talebi";
                var body = "";
                _userRepository.SendMail(mail, subject, body);
            }
            else
            {

            }
            throw new NotImplementedException();
        }
    }
}
