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
using System.Data.SqlClient;

namespace InventoryManagement
{
    public partial class CategoryModuleForm : Form
    {
        // Creates the connection to the SQL server
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Felix\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();   // Creates the variable for SQL commands

        public CategoryModuleForm()
        {
            InitializeComponent();
        }

        private void btnSaveCategory_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this category?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Sends a command to the SQL Server to insert a new category with the value into the Category table in the database
                    cm = new SqlCommand("INSERT INTO tbCategory(categoryName)VALUES(@categoryName)", con);
                    cm.Parameters.AddWithValue("@categoryName", txtCategoryName.Text);      //
                    con.Open(); // Opens the connection to the SQL server
                    cm.ExecuteNonQuery();   // Tells the database to insert. We use this instead of ExecuteQuery because we're modifying, not querying
                    con.Close();    // Closes the connection to the SQL server
                    MessageBox.Show("Category has been saved");
                    Clear();    // Clears the textboxes for a new category to be added
                }

            }
            catch (Exception ec)    // If anything messes up, indicate what's happening
            {
                MessageBox.Show(ec.Message);
            }
        }

        // Method to remove text from text fields
        public void Clear()
        {
            txtCategoryName.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Return to Category Form", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Closes the window
                this.Dispose();
            }
        }

        private void btnUpdateCategory_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this category?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Sends a command to the SQL Server to UPDATE the place in the database where the usernames match
                    cm = new SqlCommand("UPDATE tbCategory SET categoryName = @categoryName WHERE categoryID LIKE '" + lblCategoryID.Text + "' ", con);
                    cm.Parameters.AddWithValue("@categoryName", txtCategoryName.Text);          //
                    con.Open(); // Opens the connection to the SQL server
                    cm.ExecuteNonQuery();   // Tells the database to insert. We use this instead of ExecuteQuery because we're modifying, not querying
                    con.Close();    // Closes the connection to the SQL server
                    MessageBox.Show("Category has been successfully updated");
                    this.Dispose(); // Closes the window
                }

            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message);
            }
        }

        private void btnClearCategory_Click(object sender, EventArgs e)
        {
            Clear();
            btnSaveCategory.Enabled = true;
            btnUpdateCategory.Enabled = false; 
        }
    }
}
