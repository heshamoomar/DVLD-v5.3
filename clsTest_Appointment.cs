using Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class clsTest_Appointment
    {
        public int TestAppointmentID { get; set; } = -1;
        public int TestTypeID { get; set; } = -1;
        public int LocalDrivingLicenseApplicationID { get; set; } = -1;
        public DateTime AppointmentDate { get; set; } = DateTime.Now;
        public decimal PaidFees { get; set; } = -1;
        public int CreatedByUserID { get; set; } = -1;
        public bool IsLocked { get; set; } = false;

        public clsTest_Appointment() { }

        // private constructor for internal use by me to fill a user datafields
        private clsTest_Appointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID,
             DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked)
        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
        }

        public int AddNewTest_Appointment()
        {
            return this.TestAppointmentID = Test_AppointmentsData.addTest_Appointment(this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked);
        }

        public bool UpdateTest_Appointment()
        {
            return Test_AppointmentsData.updateTest_Appointment(this.TestAppointmentID, this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked);
        }

        public static clsTest_Appointment Find(int TestAppointmentID)
        {
            int TestTypeID = -1;
            int LocalDrivingLicenseApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            decimal PaidFees = -1;
            int CreatedByUserID = -1;
            bool IsLocked = false;


            if (Test_AppointmentsData.getTest_AppointmentInfo(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID,
            ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked))
            {
                return new clsTest_Appointment( TestAppointmentID,  TestTypeID,  LocalDrivingLicenseApplicationID,
              AppointmentDate,  PaidFees,  CreatedByUserID,  IsLocked);
            }
            else
            {
                return null;
            }
        }


    }
}
