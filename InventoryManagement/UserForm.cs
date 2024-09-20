using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagement
{
    public partial class UserForm : Form
    {
        // Creates the connection to the SQL server
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Felix\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();   // Creates the variable for SQL commands
        SqlDataReader dr;   // Creates a null DataReader object so that we can read the information from the SQL database
        public UserForm()
        {
            InitializeComponent();            // Creates the form
            LoadUser();                       // Immediately loads all of the users into the form
        }

        // Method to handle what happens when the cell is clicked
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvUsers.Columns[e.ColumnIndex].Name;  // Whenever a cell is clicked, this indicates the name of the column
            
            // If the column of the cell that's selected is EDIT
            if (colName == "Edit")
            {
                // Create a new UserModuleForm
                UserModuleForm userModule = new UserModuleForm();
                userModule.txtUserName.Text = dgvUsers.Rows[e.RowIndex].Cells[1].Value.ToString();      //
                userModule.txtFullName.Text = dgvUsers.Rows[e.RowIndex].Cells[2].Value.ToString();      //  Fill the appropriate textboxes with the information from
                userModule.txtPassword.Text = dgvUsers.Rows[e.RowIndex].Cells[3].Value.ToString();      //  the datagrid
                userModule.txtPhone.Text = dgvUsers.Rows[e.RowIndex].Cells[4].Value.ToString();         //

                userModule.btnSaveUser.Enabled = false; // Disable the save button since we're not adding new users
                userModule.btnUpdateUser.Enabled = true;    // Since we're editing the user, we need to update the user
                userModule.txtUserName.Enabled = false; // Remove the ability to edit the username from the user
                userModule.ShowDialog();    // Show the UserModuleForm
            }
            // If the column is DELETE
            else if(colName== "Delete")
            {
                // Display a box assuring that the operator wants to delete the selected user
                if (MessageBox.Show("Are you sure you want to delete this user?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open(); // Opens the connection to the SQL server
                    // Creates a SQL commands to delete the entry in the database that matches the username
                    cm = new SqlCommand("DELETE FROM tbUser WHERE username LIKE '" + dgvUsers.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();   // Tells the database to delete. We use this instead of ExecuteQuery because we're modifying, not querying
                    con.Close();    // Closes the connection to the SQL server
                    MessageBox.Show("User has been successfully deleted!");
                }
            }
            LoadUser(); // Reloads users
        }

        // Method used to load the users
        public void LoadUser()
        {
            int i = 0;  // Creates a variable to track the rows
            dgvUsers.Rows.Clear();  // Clears all of the rows in the grid so no confusion
            cm = new SqlCommand("SELECT * FROM tbUser", con);   // Creates a new command telling the database to select ALL users from the users table
            con.Open(); // Initiates the request to the database
            dr = cm.ExecuteReader();    // We use ExecuteReader since we will be receiving multiple entries from the database

            // While the DataReader is receiving information
            while (dr.Read())
            {
                i++;    // Increase the row number
                dgvUsers.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());   // Add the user that corresponds with the row
            }
            dr.Close(); // Since we're finished reading the information, close the reader
            con.Close();    // Close the connection to the SQL server
        }

        // Method for adding a new user
        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            UserModuleForm userModule = new UserModuleForm();           // Creates a new UserModule form
            userModule.btnSaveUser.Enabled = true;                      // Since we're adding a new user, we need to save to the database
            userModule.btnUpdateUser.Enabled = false;                   // No need to update as this user does NOT exist within the database 
            userModule.ShowDialog();                                    // Shows the form
            LoadUser();                                                 // Loads the users from the database
        }
    }
}
