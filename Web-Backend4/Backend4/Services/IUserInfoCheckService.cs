using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend4.Models;

namespace Backend4.Services
{
    public interface IUserInfoCheckService
    {
        List<Person> Persons { get; set; }
        Person p { get; set; }

        bool CheckUserExists(ref RegisterUserMailModel model);
        bool CheckUserInfo(ref RegisterUserModel model);




    }
}
