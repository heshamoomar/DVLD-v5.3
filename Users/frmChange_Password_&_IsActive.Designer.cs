namespace DVLD_v1._0.Users
{
    partial class frmChange_Password___IsActive
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxIsActive = new System.Windows.Forms.CheckBox();
            this.tbNewPassword2 = new System.Windows.Forms.TextBox();
            this.tbNewPassword1 = new System.Windows.Forms.TextBox();
            this.tbOldPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnUpdatePassword = new System.Windows.Forms.Button();
            this.ctrlUserCard1 = new DVLD_v1._0.Controls.ctrlUserCard();
            this.SuspendLayout();
            // 
            // checkBoxIsActive
            // 
            this.checkBoxIsActive.AutoSize = true;
            this.checkBoxIsActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxIsActive.Location = new System.Drawing.Point(175, 556);
            this.checkBoxIsActive.Name = "checkBoxIsActive";
            this.checkBoxIsActive.Size = new System.Drawing.Size(76, 20);
            this.checkBoxIsActive.TabIndex = 31;
            this.checkBoxIsActive.Text = "Is Active";
            this.checkBoxIsActive.UseVisualStyleBackColor = true;
            // 
            // tbNewPassword2
            // 
            this.tbNewPassword2.Location = new System.Drawing.Point(175, 521);
            this.tbNewPassword2.Name = "tbNewPassword2";
            this.tbNewPassword2.PasswordChar = '*';
            this.tbNewPassword2.Size = new System.Drawing.Size(176, 20);
            this.tbNewPassword2.TabIndex = 30;
            this.tbNewPassword2.UseSystemPasswordChar = true;
            this.tbNewPassword2.Validating += new System.ComponentModel.CancelEventHandler(this.tbNewPassword2_Validating);
            // 
            // tbNewPassword1
            // 
            this.tbNewPassword1.Location = new System.Drawing.Point(175, 474);
            this.tbNewPassword1.Name = "tbNewPassword1";
            this.tbNewPassword1.PasswordChar = '*';
            this.tbNewPassword1.Size = new System.Drawing.Size(176, 20);
            this.tbNewPassword1.TabIndex = 29;
            this.tbNewPassword1.UseSystemPasswordChar = true;
            this.tbNewPassword1.Validating += new System.ComponentModel.CancelEventHandler(this.tbNewPassword1_Validating);
            // 
            // tbOldPassword
            // 
            this.tbOldPassword.Location = new System.Drawing.Point(177, 423);
            this.tbOldPassword.Name = "tbOldPassword";
            this.tbOldPassword.Size = new System.Drawing.Size(176, 20);
            this.tbOldPassword.TabIndex = 28;
            this.tbOldPassword.UseSystemPasswordChar = true;
            this.tbOldPassword.Validating += new System.ComponentModel.CancelEventHandler(this.tbOldPassword_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(11, 520);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(158, 18);
            this.label5.TabIndex = 27;
            this.label5.Text = "Confirm Password :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(38, 476);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 18);
            this.label4.TabIndex = 26;
            this.label4.Text = "New Password :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(45, 425);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 18);
            this.label3.TabIndex = 25;
            this.label3.Text = "Old Password :";
            // 
            // btnUpdatePassword
            // 
            this.btnUpdatePassword.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnUpdatePassword.Font = new System.Drawing.Font("Ebrima", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdatePassword.Location = new System.Drawing.Point(597, 529);
            this.btnUpdatePassword.Name = "btnUpdatePassword";
            this.btnUpdatePassword.Size = new System.Drawing.Size(155, 47);
            this.btnUpdatePassword.TabIndex = 24;
            this.btnUpdatePassword.Text = "Update Password";
            this.btnUpdatePassword.UseVisualStyleBackColor = false;
            this.btnUpdatePassword.Click += new System.EventHandler(this.btnUpdatePassword_Click);
            // 
            // ctrlUserCard1
            // 
            this.ctrlUserCard1.Location = new System.Drawing.Point(11, 12);
            this.ctrlUserCard1.Name = "ctrlUserCard1";
            this.ctrlUserCard1.Size = new System.Drawing.Size(741, 393);
            this.ctrlUserCard1.TabIndex = 0;
            // 
            // frmChange_Password___IsActive
            // 
            this.AcceptButton = this.btnUpdatePassword;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 603);
            this.Controls.Add(this.checkBoxIsActive);
            this.Controls.Add(this.tbNewPassword2);
            this.Controls.Add(this.tbNewPassword1);
            this.Controls.Add(this.tbOldPassword);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnUpdatePassword);
            this.Controls.Add(this.ctrlUserCard1);
            this.Name = "frmChange_Password___IsActive";
            this.Text = "Change Password";
            this.Load += new System.EventHandler(this.frmChange_Password___IsActive_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.ctrlUserCard ctrlUserCard1;
        private System.Windows.Forms.CheckBox checkBoxIsActive;
        private System.Windows.Forms.TextBox tbNewPassword2;
        private System.Windows.Forms.TextBox tbNewPassword1;
        private System.Windows.Forms.TextBox tbOldPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnUpdatePassword;
    }
}