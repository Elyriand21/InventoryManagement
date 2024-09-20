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
    public partial class ProductModuleForm : Form
    {
        // Creates the connection to the SQL server
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Felix\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();   // Creates the variable for SQL commands
        SqlDataReader dr;
        public ProductModuleForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            comboCategory.Items.Clear();
            cm = new SqlCommand("SELECT categoryName FROM tbCategory", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboCategory.Items.Add(dr[0].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Return to Product Form", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Closes the window
                this.Dispose();
            }
        }

        private void btnSaveProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this product?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Sends a command to the SQL Server to insert a new user with the values into the database
                    cm = new SqlCommand("INSERT INTO tbProduct(productName,productQty,productPrice,productDescription,productCategory)VALUES(@productName, @productQty, @productPrice, @productDescription, @productCategory)", con);
                    cm.Parameters.AddWithValue("@productName", txtProductName.Text);      //
                    cm.Parameters.AddWithValue("@productQty", Convert.ToInt16(txtProductQty.Text));      //  This section adds onto the command to the SQL server telling it what information
                    cm.Parameters.AddWithValue("@productPrice", Convert.ToDouble(txtProductPrice.Text));      //  to put and what variable is associated with the information
                    cm.Parameters.AddWithValue("@productDescription", txtProductDescription.Text);
                    cm.Parameters.AddWithValue("@productCategory", comboCategory.Text);
                    con.Open(); // Opens the connection to the SQL server
                    cm.ExecuteNonQuery();   // Tells the database to insert. We use this instead of ExecuteQuery because we're modifying, not querying
                    con.Close();    // Closes the connection to the SQL server
                    MessageBox.Show("Product has been saved");
                    Clear();    // Clears the textboxes for a new user to be added
                }

            }
            catch (Exception ec)    // If anything messes up, indicate what's happening
            {
                MessageBox.Show(ec.Message);
            }
        }

        public void Clear()
        {
            txtProductName.Clear();
            txtProductQty.Clear();
            txtProductPrice.Clear();
            txtProductDescription.Clear();
            comboCategory.Text = "";
        }

        private void btnClearProduct_Click(object sender, EventArgs e)
        {
            Clear();
            btnSaveProduct.Enabled = true;
            btnUpdateProduct.Enabled = false;
        }

        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this product?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Sends a command to the SQL Server to UPDATE the place in the database where the usernames match
                    cm = new SqlCommand("UPDATE tbProduct SET productName = @productName, productQty = @productQty, productPrice = @productPrice, productDescription = @productDescription, productCategory = @productCategory WHERE productId LIKE '" + lblProductID.Text + "' ", con);
                    cm.Parameters.AddWithValue("@productName", txtProductName.Text);      //
                    cm.Parameters.AddWithValue("@productQty", Convert.ToInt16(txtProductQty.Text));      //  This section adds onto the command to the SQL server telling it what information
                    cm.Parameters.AddWithValue("@productPrice", Convert.ToDouble(txtProductPrice.Text));      //  to put and what variable is associated with the information
                    cm.Parameters.AddWithValue("@productDescription", txtProductDescription.Text);
                    cm.Parameters.AddWithValue("@productCategory", comboCategory.Text);
                    con.Open(); // Opens the connection to the SQL server
                    cm.ExecuteNonQuery();   // Tells the database to insert. We use this instead of ExecuteQuery because we're modifying, not querying
                    con.Close();    // Closes the connection to the SQL server
                    MessageBox.Show("Product has been successfully updated");
                    this.Dispose(); // Closes the window
                }

            }
            catch (Exception ec)
            {
                MessageBox.Show(ec.Message);
            }
        }
    }
}
