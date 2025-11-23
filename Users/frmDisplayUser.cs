using Business_Layer;
using DVLD_v1._0.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_v1._0.Users
{
    public partial class frmDisplayUser : Form
    {
        private int _UserID = -1;

        public int UserID
        {
            get { return _UserID; }
        }


        public frmDisplayUser(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;
            ctrlUserCard1.LoadUserInfo(UserID);
        }

        private void frmDisplayUser_Load(object sender, EventArgs e)
        {

        }


    }
}
