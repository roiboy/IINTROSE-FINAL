﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;
using introse_project.Libs;

namespace introse_project.sub_windows.Item_Management
{
    public partial class addIM : Form
    {

        private static addIM theInstance = new addIM();

        private addIM()
        {
            InitializeComponent();
        }

        private void addIM_Load(object sender, EventArgs e)
        {
            SupplierManager.instance.fillComboBox(supplierComboBox);
            supplierComboBox.SelectedIndex = 0;
        }

        private void itemAddBtn_Click(object sender, EventArgs e)
        {
            ItemManager.instance.addData(supplierComboBox.SelectedItem.ToString(), descTxtBox.Text);
            this.Close();
        }

        public static addIM instance
        {
            get { return theInstance; }
        }
        
    }
}
