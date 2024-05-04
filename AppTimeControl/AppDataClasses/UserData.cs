using System;

namespace AppTimeControl.AppDataClasses
{
    public sealed class UserData
    {
        public string UserNickname;
        public DateTime RegistrationDate;

        public UserData(string nickname)
        {
            UserNickname = nickname;
            RegistrationDate = DateTime.Now;
        }
    }
}
