using Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class clsTakeTest
    {
        public int TestID { get; set; } = -1;
        public int TestAppointmentID { get; set; } = -1;
        public bool TestResult { get; set; } = false;
        public string Notes { get; set; } = string.Empty;
        public int CreatedByUserID { get; set; } = -1;
        public clsTakeTest() { }

        // private constructor for internal use by me to fill a user datafields
        private clsTakeTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;

        }

        public int TakeTest()
        {
            return this.TestID = Take_TestsData.AddNewTest(this.TestAppointmentID, this.TestResult,
                            this.Notes, this.CreatedByUserID);
        }

        public static clsTakeTest Find(int TestAppointmentID)
        {
            int TestID = -1;
            bool TestResult = false;
            string Notes = string.Empty;
            int CreatedByUserID = -1;

            if (Take_TestsData.getTestInfoByAppointmentID(TestAppointmentID, ref TestID, ref TestResult, ref Notes, ref CreatedByUserID))
            {
                return new clsTakeTest(TestAppointmentID, TestID, TestResult, Notes, CreatedByUserID);
            }
            else
            {
                return null;
            }
        }


    }
}
