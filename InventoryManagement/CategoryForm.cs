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
    public partial class CategoryForm : Form
    {
        // Creates the connection to the SQL server
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Felix\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();   // Creates the variable for SQL commands
        SqlDataReader dr;   // Creates a null DataReader object so that we can read the information from the SQL database
        public CategoryForm()
        {
            InitializeComponent();
            LoadCategories();
        }

        public void LoadCategories()
        {
            int i = 0;  // Creates a variable to track the rows
            dgvCategories.Rows.Clear();  // Clears all of the rows in the grid so no confusion
            cm = new SqlCommand("SELECT * FROM tbCategory", con);   // Creates a new command telling the database to select ALL customers from the customer table
            con.Open(); // Initiates the request to the database
            dr = cm.ExecuteReader();    // We use ExecuteReader since we will be receiving multiple entries from the database

            // While the DataReader is receiving information
            while (dr.Read())
            {
                i++;    // Increase the row number
                dgvCategories.Rows.Add(i, dr[0].ToString(), dr[1].ToString());   // Add the customer that corresponds with the row
            }
            dr.Close(); // Since we're finished reading the information, close the reader
            con.Close();    // Close the connection to the SQL server
        }

        private void btnCategoryAdd_Click(object sender, EventArgs e)
        {
            CategoryModuleForm categoryModule = new CategoryModuleForm();   // Creates a new CategoryModuleForm
            categoryModule.btnSaveCategory.Enabled = true;  // Since we're adding a new customer, we need to save to the database
            categoryModule.btnUpdateCategory.Enabled = false;   // No need to update as this customer does NOT exist within the database 
            categoryModule.ShowDialog();    // Shows the form
            LoadCategories(); // Loads the customer(s) from the database
        }

        private void dgvCategories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCategories.Columns[e.ColumnIndex].Name;  // Whenever a cell is clicked, this indicates the name of the column

            // If the column of the cell that's selected is EDIT
            if (colName == "Edit")
            {
                CategoryModuleForm categoryModule = new CategoryModuleForm();   // Create a new categoryModuleForm
                categoryModule.lblCategoryID.Text = dgvCategories.Rows[e.RowIndex].Cells[1].Value.ToString();        //
                categoryModule.txtCategoryName.Text = dgvCategories.Rows[e.RowIndex].Cells[2].Value.ToString();      //  Fills the appropriate information from the

                categoryModule.btnSaveCategory.Enabled = false; // Disable the save button since we're not adding new customers
                categoryModule.btnUpdateCategory.Enabled = true;    // Since we're editing the customer, we need to update the customer
                categoryModule.ShowDialog();    // Show the CustomerModuleForm
            }
            // If the column is DELETE
            else if (colName == "Delete")
            {
                // Display a box assuring that user wants to delete the selected customer
                if (MessageBox.Show("Are you sure you want to delete this category?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open(); // Opens the connection to the SQL server

                    // Creates a SQL commands to delete the entry in the database that matches the customerId
                    cm = new SqlCommand("DELETE FROM tbCategory WHERE categoryID LIKE '" + dgvCategories.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();   // Tells the database to insert. We use this instead of ExecuteQuery because we're modifying, not querying
                    con.Close();    // Closes the connection to the SQL server
                    MessageBox.Show("Category has been successfully deleted!");
                }
            }
            LoadCategories(); // Reloads customer(s)
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
