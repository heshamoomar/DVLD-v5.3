using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer;

namespace Business_Layer
{
    public class clsUser
    {
        public int UserID { set; get; }
        public int PersonID { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool IsActive { set; get; }

        // default empty constructor
        public clsUser() 
        {
            UserID = -1;
            UserName = string.Empty;
            PersonID = -1;
            Password = string.Empty;
            IsActive = true;
        }

        // private constructor for internal use by me to fill a user datafields
        private clsUser(int userID, string userName, int PersonID, string password, bool isActive)
        {
            this.UserID = userID;
            this.UserName = userName;
            this.PersonID = PersonID;
            this.Password = password;
            this.IsActive = isActive;
        }

        public static clsUser Find(string UserName)
        {
            int UserID = -1;
            int PersonID = -1;
            string Password = "";
            bool IsActive = false;

            if (UsersData.getUserInfoByUsername(UserName, ref UserID, ref PersonID, ref Password, ref IsActive))
            {
                return new clsUser(UserID, UserName, PersonID, Password, IsActive);
            }
            else
            {
                return null;
            }
        }

        public static clsUser Find(int UserID)
        {
            string UserName = "";
            int PersonID = -1;
            string Password = "";
            bool IsActive = false;

            if (UsersData.getUserInfoByUserID(UserID, ref UserName, ref PersonID, ref Password, ref IsActive))
            {
                return new clsUser(UserID, UserName, PersonID, Password, IsActive);
            }
            else
            {
                return null;
            }
        }

        public static clsUser Find(string UserName, string Password)
        {
            int UserID = -1;
            int PersonID = -1;
            bool IsActive = false;

            if (UsersData.getUserInfoByUserNameAndPassword(UserName, Password, ref UserID, ref PersonID, ref IsActive))
            {
                return new clsUser(UserID, UserName, PersonID, Password, IsActive);
            }
            else
            {
                return null;
            }
        }

        public int AddNewUser()
        {
            return this.UserID = UsersData.addUser(this.PersonID, this.UserName, this.Password, this.IsActive);
        }

        public static DataTable GetUsersDatatable()
        {
            return UsersData.GetAllUsersInfo();
        }

        public static bool UserNameExistsInDatabase(string UserName)
        {
            return UsersData.IsUserExists(UserName);
        }

        public static bool deleteUser(int UserID)
        {
            // if exists references in other tables in database for this Record then don't delete it -Referencial Integrity-
            if (DatabaseMeta.checkUserHasReferences(UserID))
                return false;

            return UsersData.DeleteUser(UserID);
        }

        public bool UpdateUser()
        {
            return UsersData.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.IsActive);
        }


    }
}
