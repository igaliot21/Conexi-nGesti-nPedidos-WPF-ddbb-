using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConexiónGestiónPedidos
{
    /// <summary>
    /// Interaction logic for ActualizarClienteWindow.xaml
    /// </summary>
    public partial class ActualizarClienteWindow : Window
    {
        string clienteId;
        SqlConnection conn;
        public ActualizarClienteWindow(string id, SqlConnection conn)
        {
            InitializeComponent();
            this.clienteId = id;
            this.conn = conn;
            CargaCliente();
        }

        private void btnActualizarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "UPDATE Cliente SET NombreCliente=@NombreCliente, Direccion=@Direccion, Poblacion=@Poblacion, Telefono=@Telefono WHERE pkCliente=@ClienteID";
                SqlCommand sqlComando = new SqlCommand(consulta, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlComando);
                conn.Open();
                sqlComando.Parameters.AddWithValue("@ClienteID", clienteId);
                sqlComando.Parameters.AddWithValue("@NombreCliente", txtNombre.Text);
                sqlComando.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                sqlComando.Parameters.AddWithValue("@Poblacion", txtPoblacion.Text);
                sqlComando.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                sqlComando.ExecuteNonQuery();
                conn.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CargaCliente() {
            try
            {
                string consulta = "Select * from Cliente WHERE pkCliente=@ClienteID"; 
                SqlCommand sqlComando = new SqlCommand(consulta, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlComando);
                using (sqlDataAdapter)
                {
                    sqlComando.Parameters.AddWithValue("@ClienteID", clienteId);
                    DataTable tablaCliente = new DataTable();
                    sqlDataAdapter.Fill(tablaCliente);
                    txtNombre.Text = tablaCliente.Rows[0]["NombreCliente"].ToString();
                    txtDireccion.Text = tablaCliente.Rows[0]["Direccion"].ToString();
                    txtPoblacion.Text = tablaCliente.Rows[0]["Poblacion"].ToString();
                    txtTelefono.Text = tablaCliente.Rows[0]["Telefono"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
