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
using DepremsizHayat.DTO;
using DepremsizHayat.DTO.Admin;
using System.Web.Security;
using System.Security.Claims;

namespace DepremsizHayat.Business.Service
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IMailRepository _mailRepository;
        private IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMailRepository mailRepository)
        {
            this._userRepository = userRepository;
            this._mailRepository = mailRepository;
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
        public ResetForgottenPaswordResponse CheckResetAuth(string code)
        {
            USER_ACCOUNT user = _userRepository.GetByResetAuth(code);
            ResetForgottenPaswordResponse result = new ResetForgottenPaswordResponse();
            if (user != null)
            {
                if (user.PASSWORD_RESET_REQUEST_TIME != null && ((DateTime)user.PASSWORD_RESET_REQUEST_TIME).AddDays(1) > DateTime.Now)
                {
                    if (user.PASSWORD_RESET_IS_USED != null && user.PASSWORD_RESET_IS_USED == false)
                    {
                        result.USER = _userRepository.GetByResetAuth(code);
                        result.Status = true;
                    }
                    else
                    {
                        result.USER = null;
                        result.Status = false;
                        result.Message.Add("Bu şifre sıfırlama bağlantısı, daha önce kullanılmış.");
                    }
                }
                else
                {
                    result.USER = null;
                    result.Status = false;
                    result.Message.Add("Bu şifre sıfırlama bağlantısının süresi geçmiş.");
                }
            }
            return result;
        }
        public USER_ACCOUNT CreateUser(UserModel user)
        {
            USER_ACCOUNT rUser = _userRepository.CreateUser(user);
            _unitOfWork.Commit();
            return rUser;
        }

        public BaseResponse EditProfile(EditProfileRequest request)
        {
            EditProfileResponse response = new EditProfileResponse();
            USER_ACCOUNT user = _userRepository.GetById(Decryptor.DecryptInt(request.USER_ACCOUNT_ID));
            response.NewNameSurname = string.Concat(user.FIRST_NAME, "*", user.LAST_NAME);
            if ((request.Name != null && request.Name != "") || (request.Surname != null && request.Surname != "") || (request.Password != null && request.Password != ""))
            {
                if (request.Name != null && request.Name != "")
                {
                    if (request.Name != user.FIRST_NAME)
                    {
                        user.FIRST_NAME = request.Name;
                        response.NewNameSurname = response.NewNameSurname.Replace(response.NewNameSurname.Split('*')[0], request.Name);
                        response.Message.Add("İsim güncellendi.");
                        response.Status = true;
                    }
                }
                if (request.Surname != null && request.Surname != "")
                {
                    if (request.Surname != user.LAST_NAME)
                    {
                        user.LAST_NAME = request.Surname;
                        response.NewNameSurname = response.NewNameSurname.Replace(response.NewNameSurname.Split('*')[1], request.Surname);
                        response.Message.Add("Soyisim güncellendi.");
                        response.Status = true;
                    }
                }
                if (request.Password != null && request.Password != "")
                {
                    _userRepository.ResetPassword(user.E_MAIL, request.Password);
                    response.Message.Add("Şifre güncellendi.");
                    response.Status = true;
                }
                _userRepository.Update(user);
                _unitOfWork.Commit();
            }
            else
            {
                response.Message.Add("Güncelleme yapmak için lütfen en az bir alanı dolu girin.");
            }
            response.NewNameSurname = response.NewNameSurname.Replace("*", " ");
            return response;
        }

        public List<USER_ACCOUNT> GetAll()
        {
            return _userRepository.GetAll();
        }

        public USER_ACCOUNT GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        public USER_ACCOUNT GetByMail(string mail)
        {
            return _userRepository.GetByMail(mail);
        }
        public USER_ACCOUNT GetByResetAuth(string authCode)
        {
            return _userRepository.GetByResetAuth(authCode);
        }
        public bool Login(string mail, string pwd)
        {
            bool result = false;
            if (_userRepository.Login(new UserModel() { E_MAIL = mail, PASSWORD = pwd }))
            {
                result = true;
            }
            return result;
        }
        public BaseResponse ResetForgottenPassword(ResetPasswordRequest request)
        {
            var user = _userRepository.GetByMail(request.Mail);
            BaseResponse response = new BaseResponse();
            if (_userRepository.ResetForgottenPassword(request))
            {
                response.Message.Add("Şifreniz başarıyla sıfırlandı.");
                response.Status = true;
            }
            else
            {
                response.Message.Add("Bir hata oluştu. Lütfen tekrar deneyin ya da yeni bir şifre sıfırlama talebi oluşturun.");
            }
            return response;
        }
        public BaseResponse SendResetMail(string mail)
        {
            var user = _userRepository.GetByMail(mail);
            BaseResponse result = new BaseResponse();
            if (user != null)
            {
                if (_userRepository.CreateForgottenPwdResetRequest(mail))
                {
                    user = _userRepository.GetByMailForProcedure(mail);
                    var subject = "Şifre Sıfırlama Talebi";
                    var body = "<b>Talebiniz </b>üzerine iletilen şifre sıfırlama linki: http://app.depremsizhayat.com/Account/SetForgottenPassword?authCode=" + Encryptor.Encrypt(user.PASSWORD_RESET_HELPER);
                    if (_mailRepository.SendMail("app", mail, subject, body))
                    {
                        result.Status = true;
                        result.Message.Add("Şifre sıfırlama linki mail adresinize gönderildi.");
                    }
                    else
                    {
                        result.Message.Add("Bir hata oluştu! Lütfen tekrar deneyin.");
                    }
                }
                else
                {
                    result.Message.Add("Bir hata oluştu. Lütfen mail adresinizi doğru girdiğinizden emin olunuz.");
                }
            }
            else
            {
                result.Message.Add("Bir hata oluştu. Lütfen mail adresinizi doğru girdiğinizden emin olunuz.");
            }
            return result;
        }

        public bool UpdateUserRole(EditRoleRequest request)
        {
            USER_ACCOUNT updating = GetById(Decryptor.DecryptInt(request.USER_ACCOUNT_ID));
            updating.ROLE_ID = request.NEW_ROLE_ID;
            try
            {
                _userRepository.Update(updating);
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
