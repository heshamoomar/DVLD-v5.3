using Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Cryptography;

namespace DVLD_v1._0
{
    public partial class frmLogin_Screen : Form
    {

        clsUser user = new clsUser();

        //+-+-+-+- For Registry -+-+-+-+

        /// <summary>
        /// Checks if specified registry key and subkey path exists.
        /// </summary>
        /// <param name="baseKeyPath">Path of the base key, e.g., "Software".</param>
        /// <param name="subKeyPath">Path of the subkey, e.g., "MyApplication".</param>
        /// <returns>True or False.</returns>
        public static bool DoesRegistrySubKeyExist(string baseKeyPath, string subKeyPath)
        {
            // Open the base registry key
            using (RegistryKey baseKey = Registry.CurrentUser.OpenSubKey(baseKeyPath))
            {
                if (baseKey == null)
                {
                    // The base key does not exist
                    return false;
                }

                // Try to open the specified subkey
                using (RegistryKey subKey = baseKey.OpenSubKey(subKeyPath))
                {
                    // Return true if subKey exists, otherwise false
                    return subKey != null;
                }
            }
        }

        public frmLogin_Screen()
        {
            InitializeComponent();
            btnLogin.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbUsername.Text == "" || tbPassword.Text == "")
            {
                MessageBox.Show("Please enter Username and Password");
                return;
            }

            user = clsUser.Find(tbUsername.Text.ToString());

            if (user != null)   // username is found in database
            {
                if (user.Password == clsGlobalSettings.ComputeHash(tbPassword.Text))   //  username and password combination found in db
                {
                    if (user.IsActive == true)
                    {
                        this.Hide();
                        //MessageBox.Show("User found :)");   //  temporary

                        string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
                        try
                        {
                            // Write the values to the Registry
                            Registry.SetValue(keyPath, "UserName", tbUsername.Text, RegistryValueKind.String);
                            Registry.SetValue(keyPath, "Password", tbPassword.Text, RegistryValueKind.String);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}\n While attempting to write to app registry folder.", "Error");
                            EventLog.WriteEntry("DVLD", $"An error occurred: {ex.Message}\n While attempting to write to app registry folder.",
                                EventLogEntryType.Error);
                        }

                        clsGlobalSettings.createLoggedInUser(tbUsername.Text, clsGlobalSettings.ComputeHash(tbPassword.Text));

                        frmMain_Screen frmMain = new frmMain_Screen(tbUsername.Text);
                        frmMain.ShowDialog();



                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Account not active, talk with your administrator.");
                    }
                }
                else
                {
                    MessageBox.Show("password incorrect.");
                }
            }

            else
            {
                MessageBox.Show("username incorrect.");
            }

        }

        private void loginKeyboardPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

            }
        }

        private void login_screen_Load(object sender, EventArgs e)
        {
            // register an event source for the app once and for all
            string sourceName = "DVLD";
            // Create the event source if it does not exist
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
            }

            EventLog.WriteEntry("DVLD", $"This is a test.",
                EventLogEntryType.Error);


            // if application registry path not yet created for logged-in windows user, create it
            if (!DoesRegistrySubKeyExist("Software", "DVLD"))
            {
                string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
                try
                {
                    // Perpare empty value names in registry
                    Registry.SetValue(keyPath, "DateCreated", DateTime.Now.ToString("g"), RegistryValueKind.String);
                    Registry.SetValue(keyPath, "UserName", "", RegistryValueKind.String);
                    Registry.SetValue(keyPath, "Password", "", RegistryValueKind.String);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}\n While attempting to write to app registry folder.", "Error");
                    EventLog.WriteEntry("DVLD", $"An error occurred: {ex.Message}\n While attempting to write to app registry folder.",
                        EventLogEntryType.Error);

                }
            }
            // else load the last saved username & password to form
            else
            {
                string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";

                tbUsername.Text = Registry.GetValue(keyPath, "UserName", "").ToString();
                tbPassword.Text = Registry.GetValue(keyPath, "Password", "").ToString();
            }

        }
    }
}
