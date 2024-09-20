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
        // Creates the connection to the SQL server
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Felix\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();   // Creates the variable for SQL commands

        public UserModuleForm()
        {
            InitializeComponent();
        }

        // Method for EXIT button
        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Return to Customer Form", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Closes the window
                this.Dispose();
            }
        }

        // Method for SAVE button
        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            try
            {
                // Checking to see if the passwords match
                if(txtPassword.Text != txtConfirmPass.Text)
                {
                    // If passwords do not match, display a messagebox indicating the passwords do not match
                    MessageBox.Show("Passwords do not match. Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;     // Skip the rest of the code until the passwords match
                }

                // If both passwords match
                if (MessageBox.Show("Are you sure you want to save this user?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Sends a command to the SQL Server to insert a new user with the values into the database
                    cm = new SqlCommand("INSERT INTO tbUser(username,fullName,password,phone)VALUES(@username,@fullName,@password,@phone)", con);
                    cm.Parameters.AddWithValue("@username", txtUserName.Text);      //
                    cm.Parameters.AddWithValue("@fullName", txtFullName.Text);      //  This section adds onto the command to the SQL server telling it what information
                    cm.Parameters.AddWithValue("@password", txtPassword.Text);      //  to put and what variable is associated with the information
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);            //
                    con.Open(); // Opens the connection to the SQL server
                    cm.ExecuteNonQuery();   // Tells the database to insert. We use this instead of ExecuteQuery because we're modifying, not querying
                    con.Close();    // Closes the connection to the SQL server
                    MessageBox.Show("User has been saved");
                    Clear();    // Clears the textboxes for a new user to be added
                }

            } 
            catch (Exception ec)    // If anything messes up, indicate what's happening
            {
                MessageBox.Show(ec.Message);
            }
        }

        // Method for Clear button
        private void BtnClearUser_Click(object sender, EventArgs e)
        {
            Clear();    // Clears the textboxes
            btnSaveUser.Enabled = true; // Allows the Save button to be used
            btnUpdateUser.Enabled = false;  // Since this button clears the textboxes, we will only be adding users so there's no need for the update button.
        }

        // Method for the UPDATE button
        private void btnUpdateUser_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Checking to see if the passwords match
                if (txtConfirmPass.Text != txtPassword.Text)
                {
                    // If the passwords do not match, display a textbox indicating the passwords do not match
                    MessageBox.Show("Passwords do not match. Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Skip the rest of the code until the passwords match
                }

                // If both passwords match
                if (MessageBox.Show("Are you sure you want to update this user?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Sends a command to the SQL Server to UPDATE the place in the database where the usernames match
                    cm = new SqlCommand("UPDATE tbUser SET fullName = @fullName,password = @password,phone = @phone WHERE username LIKE '" + txtUserName.Text + "' ", con);
                    cm.Parameters.AddWithValue("@fullName", txtFullName.Text);          //
                    cm.Parameters.AddWithValue("@password", txtPassword.Text);          //  This section is adding onto the SQL command to put the information in the 
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);                //  correct spot in the database
                    con.Open(); // Opens the connection to the SQL server
                    cm.ExecuteNonQuery();   // Tells the database to insert. We use this instead of ExecuteQuery because we're modifying, not querying
                    con.Close();    // Closes the connection to the SQL server
                    MessageBox.Show("User has been successfully updated");
                    this.Dispose(); // Closes the window
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

        private void checkBoxShowPass_CheckedChanged(object sender, EventArgs e)
        {
            // If the box is not checked
            if (checkBoxShowPass.Checked == false)
            {
                txtPassword.UseSystemPasswordChar = true;   // Use the little ****** characters
                txtConfirmPass.UseSystemPasswordChar = true;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = false;  // Show the password in plain text
                txtConfirmPass.UseSystemPasswordChar = false;
            }
        }
    }
}
