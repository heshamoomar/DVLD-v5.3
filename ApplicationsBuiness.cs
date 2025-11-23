using Data_Access_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class clsApplicationType
    {
        public int ApplicationTypeID { set; get; }
        public string ApplicationTypeTitle { set; get; }
        public decimal ApplicationFees { set; get; }

        // default empty constructor
        public clsApplicationType()
        {
            ApplicationTypeID = -1;
            ApplicationTypeTitle = string.Empty;
            ApplicationFees = -1;
        }

        private clsApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, decimal ApplicationFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationTypeTitle;
            this.ApplicationFees = ApplicationFees;
        }


        public static DataTable GetApplicationsTypes()
        {
            return ApplicationsData.GetApplicationsTypes();
        }

        public bool UpdateApplicationType()
        {
            return ApplicationsData.UpdateApplicationTypes(this.ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationFees);
        }

        public static clsApplicationType Find(string ApplicationTypeTitle)
        {
            int ApplicationTypeID = -1;
            decimal ApplicationFees = -1;

            if (ApplicationsData.getApplicationTypeInfo(ApplicationTypeTitle, ref ApplicationTypeID, ref ApplicationFees))
            {
                return new clsApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationFees);
            }
            else
            {
                return null;
            }


        }


    }
}
