using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer;

namespace Business_Layer
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PersonID { get; set; } = -1;
        public string NationalNo { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string ThirdName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = DateTime.Now;
        public string Address { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public int NationalityCountryID { get; set; } = -1;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        //  default constructor
        public clsPerson() { }

        // private constructor for internal use by me to fill a user datafields
        private clsPerson(int PersonID, string NationalNo, string FirstName, string SecondName,
             string ThirdName, string LastName, DateTime DateOfBirth, string Gender, 
            string Address, string Nationality, string Phone, string Email, string ImagePath, int NationalityCountryID)
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Nationality = Nationality;
            this.Phone = Phone;
            this.Email = Email;
            this.ImagePath = ImagePath;
            this.NationalityCountryID = NationalityCountryID;

        }


        public static DataTable GetPeopleDatatable()
        {
            return PeopleData.GetAllPeopleInfo();
        }

        public static clsPerson Find(int PersonID)
        {
            string NationalNo = string.Empty;
            string FirstName = string.Empty;
            string SecondName = string.Empty;
            string ThirdName = string.Empty;
            string LastName = string.Empty;
            DateTime DateOfBirth = DateTime.Now;
            string Gender = string.Empty;
            string Address = string.Empty;
            string Nationality = string.Empty;
            string Phone = string.Empty;
            string Email = string.Empty;
            string ImagePath = string.Empty;
            int NationalityCountryID = -1;


            if (PeopleData.getPersonInfo(PersonID, ref NationalNo, ref FirstName, ref SecondName,
            ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address,
            ref Nationality, ref Phone, ref Email, ref ImagePath, ref NationalityCountryID))
            {
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName,
                    ThirdName, LastName, DateOfBirth, Gender, Address,
                    Nationality, Phone, Email, ImagePath, NationalityCountryID);
            }
            else
            {
                return null;
            }
        }

        public static clsPerson Find(string NationanlNo)
        {
            int PersonID = -1;
            string FirstName = string.Empty;
            string SecondName = string.Empty;
            string ThirdName = string.Empty;
            string LastName = string.Empty;
            DateTime DateOfBirth = DateTime.Now;
            string Gender = string.Empty;
            string Address = string.Empty;
            string Nationality = string.Empty;
            string Phone = string.Empty;
            string Email = string.Empty;
            string ImagePath = string.Empty;
            int NationalityCountryID = -1;

            if (PeopleData.getPersonInfo(NationanlNo, ref PersonID, ref FirstName, ref SecondName,
            ref ThirdName, ref LastName, ref DateOfBirth, ref Gender, ref Address,
            ref Nationality, ref Phone, ref Email, ref ImagePath, ref NationalityCountryID))
            {
                return new clsPerson(PersonID, NationanlNo, FirstName, SecondName,
                    ThirdName, LastName, DateOfBirth, Gender, Address,
                    Nationality, Phone, Email, ImagePath, NationalityCountryID);
            }
            else
            {
                return null;
            }

            
        }

        public int AddNewPerson()
        {
            return this.PersonID = PeopleData.addPerson(this.NationalNo, this.FirstName, this.SecondName,
                            this.ThirdName, this.LastName, this.DateOfBirth, this.Gender,
                            this.Address, this.NationalityCountryID, this.Phone, this.Email, this.ImagePath);
        }

        public bool UpdatePerson()
        {
            return PeopleData.updatePerson(this.PersonID, this.NationalNo, this.FirstName, this.SecondName,
                            this.ThirdName, this.LastName, this.DateOfBirth, this.Gender,
                            this.Address, this.NationalityCountryID, this.Phone, this.Email, this.ImagePath);
        }

        public string getImagePathForPerson()
        {
            return PeopleData.getImagePathForPerson(this.PersonID);
        }

        public static bool isPersonExists(string NationalNo)
        {
            return (PeopleData.IsPersonExists(NationalNo));
        }

        public static bool IsPersonUser(int PersonID)
        {
            return (PeopleData.IsPersonUser(PersonID));
        }

        public int Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    int newPersonID = AddNewPerson();
                    if (newPersonID != -1)
                    {
                        Mode = enMode.Update;
                    }                           
                    return newPersonID;         // returning a number (added person id) means that person added successfully
                                                // returning -1 means that person added failed
                case enMode.Update:

                    if (UpdatePerson())
                        return 1;
                    return -1;

            }
            return -1;
        }

        public static bool deletePerson(int PersonID)
        {
            // if exists references in other tables in database for this Record then don't delete it -Referencial Integrity-
            if (DatabaseMeta.checkPersonHasReferences(PersonID))
                return false;

            return PeopleData.DeletePerson(PersonID);
        }

        public bool deletePerson()
        {
            return PeopleData.DeletePerson(this.PersonID);
        }
    }
}
