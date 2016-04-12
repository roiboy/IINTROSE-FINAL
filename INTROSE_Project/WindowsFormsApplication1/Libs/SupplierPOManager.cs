﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Data;


namespace introse_project.Libs
{
    class SupplierPOManager
    {
        private static SupplierPOManager theInstance = new SupplierPOManager();

        private SupplierPOManager(){}

        public void viewAll(DataGridView dataGridView)      //Displays all supplier purchase orders, its related customer purchase order and the PO's ordered items
        {
            string query = "SELECT	 A.supplierPONumber		    AS 'Supplier PO Number'," +
                                    "A.customerPONumber		    AS 'Related Customer PO Number'," +
		                            "A.supplierName			    AS 'Supplier Name'," +
		                            "A.dateIssued			    AS 'Date Issued'," +
                                    "A.expectedDeliveryDate 	AS 'Expected Delivery Date'," +
                                    "A.isFinished               AS 'Supplier PO Status' " +
                            "FROM 	supplier_po A " +
                            "ORDER BY A.supplierPONumber DESC;";     
                                                                                                                                                              
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["poConn"].ConnectionString);
            MySqlCommand command = new MySqlCommand(query, connection);

            try
            {
                MySqlDataAdapter data = new MySqlDataAdapter();
                data.SelectCommand = command;

                connection.Open();

                DataTable table = new DataTable();
                data.Fill(table);

                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = table;

                dataGridView.DataSource = bindingSource;
                data.Update(table);

                connection.Close();

            }
            catch
            {
                MessageBox.Show("Error: Unable to show table due to connection problems", "ERROR");
            }
            finally
            {
                connection.Close();
            }


        }

        public string test(string customerPONumber)
        {
            string query = "SELECT COUNT(*) FROM customer_po A WHERE A.customerPONumber = '" + customerPONumber + "' ";

            int value = 0;

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["poConn"].ConnectionString);
            MySqlCommand command = new MySqlCommand(query, connection);

            try
            {
                connection.Open();

                value = Convert.ToInt32(command.ExecuteScalar().ToString());

                connection.Close();

                return value.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return value.ToString();
        }

        public void addData(string supplierPONumber, string customerPONumber, string supplierName, string dateIssued, string expectedDeliveryDate)
        {
            string query = "INSERT INTO supplier_po (supplierPONumber, customerPONumber, supplierName, dateIssued, expectedDeliveryDate, isFinished) " +
                            "values (@supplierPONumber, '"+customerPONumber+"', @supplierName, @dateIssued, @expectedDeliveryDate, false) ";

            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["poConn"].ConnectionString);
            MySqlCommand command = new MySqlCommand(query, connection);

            try
            {
                connection.Open();

                command.Parameters.AddWithValue("@supplierPONumber", supplierPONumber);
                command.Parameters.AddWithValue("@supplierName", supplierName);
                command.Parameters.AddWithValue("@dateIssued", dateIssued);
                command.Parameters.AddWithValue("@expectedDeliveryDate", expectedDeliveryDate);

                command.ExecuteNonQuery();

                connection.Close();
            }
            catch
            {
                MessageBox.Show("Unable to add supplier purchase order", "ERROR");
            }
            finally
            {
                connection.Close();
            }

        }

        public int getCount()
        {
            string query = "SELECT  COUNT(*) FROM supplier_po";
            int value = 0;
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["poConn"].ConnectionString);
            MySqlCommand command = new MySqlCommand(query, connection);

            try
            {
                connection.Open();

                value = Convert.ToInt32(command.ExecuteScalar().ToString());

                connection.Close();

                return value;
            }
            catch
            {
                MessageBox.Show("Unable to retrieve count data", "ERROR");
            }
            finally
            {
                connection.Close();
            }

            return value;

        }

        public bool pkExists(string sPONumber)
        {
            string query = "SELECT  COUNT(supplierPONumber) FROM supplier_po A WHERE A.supplierPONumber = '" + sPONumber + "'";
            int count = 0;
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["poConn"].ConnectionString);
            MySqlCommand command = new MySqlCommand(query, connection);

            try
            {
                connection.Open();

                count = Convert.ToInt32(command.ExecuteScalar().ToString());

                connection.Close();

                if (count > 0)
                {
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("Unable to retrieve data due to connection problems", "ERROR");
            }
            finally
            {
                connection.Close();
            }

            return false;

        }

        public static SupplierPOManager instance
        {
            get { return theInstance; }
        }

    }
}
