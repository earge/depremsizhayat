using DepremsizHayat.Business.IService;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.Business.UnitOfWork;
using DepremsizHayat.DataAccess;
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

        public bool Login(string mail,string pwd)
        {
            bool result = false;
            var userFromDb = _userRepository.GetByMail(mail);
            if (userFromDb != null)
            {
                if (userFromDb.PASSWORD == pwd)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
