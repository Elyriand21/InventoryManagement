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
    public partial class CustomerForm : Form
    {
        // Creates the connection to the SQL server
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Felix\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();   // Creates the variable for SQL commands
        SqlDataReader dr;   // Creates a null DataReader object so that we can read the information from the SQL database

        public CustomerForm()
        {
            InitializeComponent();
            // Loads the customers into the form
            LoadCustomer();
        }

        public void LoadCustomer()
        {
            int i = 0;  // Creates a variable to track the rows
            dgvCustomers.Rows.Clear();  // Clears all of the rows in the grid so no confusion
            cm = new SqlCommand("SELECT * FROM tbCustomer", con);   // Creates a new command telling the database to select ALL users from the users table
            con.Open(); // Initiates the request to the database
            dr = cm.ExecuteReader();    // We use ExecuteReader since we will be receiving multiple entries from the database

            // While the DataReader is receiving information
            while (dr.Read())
            {
                i++;    // Increase the row number
                dgvCustomers.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());   // Add the user that corresponds with the row
            }
            dr.Close(); // Since we're finished reading the information, close the reader
            con.Close();    // Close the connection to the SQL server
        }

        // Method for adding a new user
        private void btnCustomerAdd_Click(object sender, EventArgs e)
        {
            CustomerModuleForm customerModule = new CustomerModuleForm();   // Creates a new CustomerModule form
            customerModule.btnSaveCustomer.Enabled = true;  // Since we're adding a new customer, we need to save to the database
            customerModule.btnUpdateCustomer.Enabled = false;   // No need to update as this customer does NOT exist within the database 
            customerModule.ShowDialog();    // Shows the form
            LoadCustomer(); // Loads the customer(s) from the database
        }
    }
}
