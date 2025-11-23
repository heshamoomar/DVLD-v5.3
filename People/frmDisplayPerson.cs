using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_v1._0.People
{
    public partial class frmDisplayPerson : Form
    {

        private int _PersonID = -1;

        public int PersonID
        {
            get { return _PersonID; }
        }

        public frmDisplayPerson(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;

        }

        private void frmDisplayPerson_Load(object sender, EventArgs e)
        {
            ctrlPersonCard1.LoadPersonInfo(PersonID);
        }

        private void linkLabelEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
