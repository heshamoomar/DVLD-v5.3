using Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class clsTestsBusiness
    {
        public int TestTypeID { set; get; }
        public string TestTypeTitle { set; get; }
        public string TestTypeDescription { set; get; }
        public decimal TestTypeFees { set; get; }

        // default empty constructor
        public clsTestsBusiness()
        {
            TestTypeID = -1;
            TestTypeTitle = string.Empty;
            TestTypeDescription = string.Empty;
            TestTypeFees = -1;
        }

        private clsTestsBusiness(int TestTypeID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFees)
        {
            this.TestTypeID = TestTypeID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.TestTypeFees = TestTypeFees;
        }

        public static DataTable GetTestsTypes()
        {
            return TestsData.GetTestsTypes();
        }

        public static clsTestsBusiness Find(int TestTypeID)
        {
            string TestTypeTitle = string.Empty;
            string TestTypeDescription = string.Empty;
            decimal TestTypeFees = -1;

            if (TestsData.GetTestTypeInfo(TestTypeID, ref TestTypeTitle, ref TestTypeDescription, ref TestTypeFees))
            {
                return new clsTestsBusiness(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);
            }
            else
            {
                return null;
            }
        }


        public static DataTable GetTestAppointmentsDatatable_Vision(int LocalDrivingLicenseApplicationID)
        {
            return TestsData.GetTestAppointmentsDatatable_Vision(LocalDrivingLicenseApplicationID);
        }

        public static DataTable GetTestAppointmentsDatatable_Written(int LocalDrivingLicenseApplicationID)
        {
            return TestsData.GetTestAppointmentsDatatable_Written(LocalDrivingLicenseApplicationID);
        }

        public static DataTable GetTestAppointmentsDatatable_Street(int LocalDrivingLicenseApplicationID)
        {
            return TestsData.GetTestAppointmentsDatatable_Street(LocalDrivingLicenseApplicationID);
        }

        public bool UpdateTestType()
        {
            return TestsData.UpdateTestTypes(this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
        }



    }
}
