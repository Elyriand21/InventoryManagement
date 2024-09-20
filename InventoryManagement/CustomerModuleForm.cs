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
    public partial class CustomerModuleForm : Form
    {
        // Creates the connection to the SQL server
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Felix\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();   // Creates the variable for SQL commands

        public CustomerModuleForm()
        {
            InitializeComponent();
        }

        private void btnSaveCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this customer?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Sends a command to the SQL Server to insert a new customer with the values into the Customer table in the database
                    cm = new SqlCommand("INSERT INTO tbCustomer(customerName,customerPhone)VALUES(@customerName,@customerPhone)", con);
                    cm.Parameters.AddWithValue("@customerName", txtCustomerName.Text);      //
                    cm.Parameters.AddWithValue("@customerPhone", txtCustomerPhone.Text);
                    con.Open(); // Opens the connection to the SQL server
                    cm.ExecuteNonQuery();   // Tells the database to insert. We use this instead of ExecuteQuery because we're modifying, not querying
                    con.Close();    // Closes the connection to the SQL server
                    MessageBox.Show("Customer has been saved");
                    Clear();    // Clears the textboxes for a new customer to be added
                }

            }
            catch (Exception ec)    // If anything messes up, indicate what's happening
            {
                MessageBox.Show(ec.Message);
            }
        }

        // Method to link the CLEAR button to the Clear() method
        private void btnClearCustomer_Click(object sender, EventArgs e)
        {
            Clear();
            btnSaveCustomer.Enabled = true;
            btnUpdateCustomer.Enabled = false;
        }

        // Method to remove all text from boxes in CustomerModuleForm
        public void Clear()
        {
            txtCustomerName.Clear();
            txtCustomerPhone.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to exit?", "Return to Customer Form",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Closes the window
                this.Dispose();
            }
        }
    }
}
