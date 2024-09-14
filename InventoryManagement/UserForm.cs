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

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Felix\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public UserForm()
        {
            // Creates the form
            InitializeComponent();
            // Immediately loads all of the users into the form
            LoadUser();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvUsers.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                UserModuleForm userModule = new UserModuleForm();
                userModule.txtUserName.Text = dgvUsers.Rows[e.RowIndex].Cells[1].Value.ToString();
                userModule.txtFullName.Text = dgvUsers.Rows[e.RowIndex].Cells[2].Value.ToString();
                userModule.txtPassword.Text = dgvUsers.Rows[e.RowIndex].Cells[3].Value.ToString();
                userModule.txtPhone.Text = dgvUsers.Rows[e.RowIndex].Cells[4].Value.ToString();

                userModule.btnSaveUser.Enabled = false;
                userModule.btnUpdateUser.Enabled = true;
                userModule.txtUserName.Enabled = false;
                userModule.ShowDialog();
            }
            else if(colName== "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this user?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbUser WHERE username LIKE '" + dgvUsers.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("User has been successfully deleted!");
                }
            }
            LoadUser();
        }

        // Method used to load the users
        public void LoadUser()
        {
            // Creates a variable that adds the users in the corresponding rows
            int i = 0;
            // Clears all of the rows in the grid so no confusion
            dgvUsers.Rows.Clear();
            // Creates a new command telling the database to select ALL users from the users table
            cm = new SqlCommand("SELECT * FROM tbUser", con);
            // Initiates the request to the database
            con.Open();
            dr = cm.ExecuteReader();
            // While the DataReader is receiving information
            while (dr.Read())
            {
                // Add a row
                i++;
                // Add the user that corresponds with the row
                dgvUsers.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void btnUserAdd_Click(object sender, EventArgs e)
        {
            UserModuleForm userModule = new UserModuleForm();
            userModule.btnSaveUser.Enabled = true;
            userModule.btnUpdateUser.Enabled = false;
            userModule.ShowDialog();
            LoadUser();
        }

        private void btnUpdateUser_Click(object sender, EventArgs e)
        {
            LoadUser();
        }
    }
}
