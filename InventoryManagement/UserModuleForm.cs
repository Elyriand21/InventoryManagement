using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InventoryManagement
{
    public partial class UserModuleForm : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Felix\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        public UserModuleForm()
        {
            InitializeComponent();
        }

        // Method for EXIT button
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        // Method for SAVE button
        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtPassword.Text != txtConfirmPass.Text)
                {
                    MessageBox.Show("Passwords do not match. Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to save this user?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbUser(username,fullName,password,phone)VALUES(@username,@fullName,@password,@phone)", con);
                    cm.Parameters.AddWithValue("@username", txtUserName.Text);
                    cm.Parameters.AddWithValue("@fullName", txtFullName.Text);
                    cm.Parameters.AddWithValue("@password", txtPassword.Text);
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("User has been saved");
                    Clear();
                }

            } 
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message);
            }
        }

        // Method for Clear button
        private void BtnClearUser_Click(object sender, EventArgs e)
        {
            Clear();
            btnSaveUser.Enabled = true;
            btnUpdateUser.Enabled = false;
        }

        // Method for the UPDATE button
        private void btnUpdateUser_Click_1(object sender, EventArgs e)
        {
            try
            {
                if(txtConfirmPass.Text != txtPassword.Text)
                {
                    MessageBox.Show("Passwords do not match. Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to update this user?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE tbUser SET fullName = @fullName,password = @password,phone = @phone WHERE username LIKE '" + txtUserName.Text + "' ", con);
                    cm.Parameters.AddWithValue("@fullName", txtFullName.Text);
                    cm.Parameters.AddWithValue("@password", txtPassword.Text);
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("User has been successfully updated");
                    this.Dispose();
                }

            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message);
            }
        }

        // Method to remove all text from boxes in UserModuleForm
        public void Clear()
        {
            txtUserName.Clear();
            txtPassword.Clear();
            txtFullName.Clear();
            txtPhone.Clear();
            txtConfirmPass.Clear();
        }
    }
}
