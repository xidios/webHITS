using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend4.Models;
using Microsoft.Extensions.Logging;

namespace Backend4.Services
{
    public class UserInfoCheckService : IUserInfoCheckService
    {

        private readonly ILogger logger;
        public UserInfoCheckService(ILogger<IUserInfoCheckService> logger)
        {
            this.logger = logger;
        }
        public List<Person> Persons { get; set; } = new List<Person>();
        public Person p { get; set; } = new Person();
        public bool CheckUserExists(ref RegisterUserMailModel model)
        {
            foreach (var item in Persons)
            {
                if (item.EqualsEmail(model))
                {                   
                    model.Remember = item.Remeber;
                    this.logger.LogInformation($"User with the mail {model.Email} already exists");
                    return true;
                }
            }
            this.logger.LogInformation($"User with the mail {model.Email} is registered");
            return false;

        }
        public bool CheckUserInfo(ref RegisterUserModel model)
        {
            foreach (var item in Persons)
            {
                if (item.Equals(model))
                    return true;
            }
            return false;
        }
    }
}
