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
    public partial class ProductForm : Form
    {
        // Creates the connection to the SQL server
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Felix\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();   // Creates the variable for SQL commands
        SqlDataReader dr;   // Creates a null DataReader object so that we can read the information from the SQL database

        public ProductForm()
        {
            InitializeComponent();
            LoadProduct();
        }

        private void btnProductAdd_Click(object sender, EventArgs e)
        {
            ProductModuleForm productModule = new ProductModuleForm();   // Creates a new ProductModuleForm
            productModule.btnSaveProduct.Enabled = true;  // Since we're adding a new product, we need to save to the database
            productModule.btnUpdateProduct.Enabled = false;   // No need to update as this product does NOT exist within the database 
            productModule.ShowDialog();    // Shows the form
            LoadProduct();
        }

        // Method to handle loading all products
        public void LoadProduct()
        {
            int i = 0;  // Creates a variable to track the rows
            dgvProduct.Rows.Clear();  // Clears all of the rows in the grid so no confusion

            // Creates a new command telling the database to select ALL products from the product table. If the search box is used, it will also allow for searching.
            cm = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(productId, productName, productPrice, productDescription, productCategory) LIKE '%"+txtSearch.Text+"%'", con);
            con.Open(); // Initiates the request to the database
            dr = cm.ExecuteReader();    // We use ExecuteReader since we will be receiving multiple entries from the database

            // While the DataReader is receiving information
            while (dr.Read())
            {
                i++;    // Increase the row number
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());   // Add the product that corresponds with the row
            }
            dr.Close(); // Since we're finished reading the information, close the reader
            con.Close();    // Close the connection to the SQL server
        }

        // Method to handle what happens when the cell is clicked
        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;  // Whenever a cell is clicked, this indicates the name of the column

            // If the column of the cell that's selected is EDIT
            if (colName == "Edit")
            {
                ProductModuleForm productModule = new ProductModuleForm();   // Create a new ProductModuleForm
                productModule.lblProductID.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();            //
                productModule.txtProductName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();          //
                productModule.txtProductQty.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();           //  Fills the appropriate information from the datagrid
                productModule.txtProductPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();         //
                productModule.txtProductDescription.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();   //  
                productModule.comboCategory.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();           //  

                productModule.btnSaveProduct.Enabled = false; // Disable the save button since we're not adding new products
                productModule.btnUpdateProduct.Enabled = true;    // Since we're editing the product, we need to update the product
                productModule.ShowDialog();    // Show the ProductModuleForm
            }
            // If the column is DELETE
            else if (colName == "Delete")
            {
                // Display a box assuring that operator wants to delete the selected product
                if (MessageBox.Show("Are you sure you want to delete this product?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open(); // Opens the connection to the SQL server

                    // Creates a SQL commands to delete the entry in the database that matches the productId
                    cm = new SqlCommand("DELETE FROM tbProduct WHERE productId LIKE '" + dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();   // Tells the database to delete. We use this instead of ExecuteQuery because we're modifying, not querying
                    con.Close();    // Closes the connection to the SQL server
                    MessageBox.Show("Product has been successfully deleted!");
                }
            }
            LoadProduct();      // Re-loads the products
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}
