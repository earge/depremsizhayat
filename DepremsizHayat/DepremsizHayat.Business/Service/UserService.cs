using DepremsizHayat.DTO.Models;
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
            if (actCode != user.ACTIVATION_CODE)
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
        public bool CheckResetAuth(string code, string mail)
        {
            bool result = (Decryptor.Decrypt(_userRepository.GetByMail(mail).ACTIVATION_CODE) == Decryptor.Decrypt(code)) ? true : false;
            return result;
        }
        public USER_ACCOUNT CreateUser(UserModel user)
        {
            USER_ACCOUNT rUser = _userRepository.CreateUser(user);
            _unitOfWork.Commit();
            return rUser;
        }
        public List<USER_ACCOUNT> GetAll()
        {
            return _userRepository.GetAll();
        }
        public USER_ACCOUNT GetByMail(string mail)
        {
            return _userRepository.GetByMail(mail);
        }
        public bool Login(string mail, string pwd)
        {
            bool result = false;
            //var userFromDb = _userRepository.GetByMail(mail);
            //if (userFromDb != null)
            //{

            if (/*Decryptor.Decrypt(userFromDb.PASSWORD) == Decryptor.Decrypt(pwd)*/_userRepository.Login(new UserModel() { E_MAIL = mail, PASSWORD = pwd }))
            {
                result = true;
            }
            //}
            return result;
        }
        public bool ResetPassword(ResetPasswordRequest request)
        {
            //Prosedüre ihtiyaç var.
            //USER user = _userRepository.GetByMail(request.Mail);
            //if (user != null)
            //{
            //    UserModel newUser = new UserModel();
            //    user.PASSWORD = request.Password;
            //    _unitOfWork.Commit();
            //    return true;
            //}
            return false;
        }
        public string SendResetMail(string mail)
        {
            var user = _userRepository.GetByMail(mail);
            if (user != null)
            {
                var code = Guid.NewGuid().ToString();
                code = Encryptor.Encrypt(code);
                user.ACTIVATION_CODE = code;
                var subject = "Şifre Sıfırlama Talebi";
                var body = "Talebiniz üzerine iletilen şifre sıfırlama linki: /Account/SetNewPassword?authCode=" + code;
                if (_userRepository.SendMail(mail, subject, body))
                {
                    return "+_Şifre sıfırlama linki mail adresinize gönderildi.";
                }
                else
                {
                    return "-_Bir hata oluştu! Lütfen tekrar deneyin.";
                }
            }
            else
            {
                return "-_Girmiş olduğunuz mail adresiyle eşleşen bir kullanıcı hesabı bulunamadı.";
            }
        }
    }
}
