using Business_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_v1._0
{
    internal class clsGlobalSettings
    {
        public static clsUser _loggedUser;
        public static clsPerson _loggedPerson;

        public clsGlobalSettings() 
        {
        }

        public static void createLoggedInUser(string userName, string password)
        {

            _loggedUser = clsUser.Find(userName, password);

            _loggedPerson = clsPerson.Find(_loggedUser.PersonID);

            
        }

        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the UTF-8 encoded input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert the byte array to a lowercase hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
