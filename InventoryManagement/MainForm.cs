using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace InventoryManagement
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // To show subform inside mainform
        private Form activeForm = null;
        private void openChildForm(Form childForm)  // Method to open another form inside the main form
        {
            if(activeForm != null)  // If there's another form already loaded
            {
                activeForm.Close(); // Unload the activeForm so that we can add another one
            }
            activeForm = childForm; // Set the activeForm to be childForm that's being passed within the method header
            childForm.TopLevel = false; // Tells the form that it will be inside another form
            childForm.FormBorderStyle = FormBorderStyle.None;   // Tells the form to not have a border
            childForm.Dock = DockStyle.Fill;    // Tells the form to fill the empty space
            panelMain.Controls.Add(childForm);  // Adds the controls from the child form to the main panel on the mainForm
            panelMain.Tag = childForm;  // Tells the panel that there's a childForm
            childForm.BringToFront();   // Pulls the childForm to the front of the window
            childForm.Show();   // Displays the childForm
        }

        // Method indicating what happens when you click the Users button
        private void btnUsers_Click(object sender, EventArgs e)
        {
            openChildForm(new UserForm()); // Tells the main form to open the UserForm
        }

        // Creates a method to write to the console easier
        public void print(String msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerForm()); // Tells the main form to open the CustomerForm

        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            openChildForm(new CategoryForm());
        }
    }
}
