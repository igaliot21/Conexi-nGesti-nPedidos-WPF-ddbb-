using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace ConexiónGestiónPedidos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn;
        public MainWindow()
        {
            InitializeComponent();
            string miConexion = ConfigurationManager.ConnectionStrings["ConexiónGestiónPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;
            conn = new SqlConnection(miConexion);
            MuestraClientes();
            CargarPedidos();
        }
        private void MuestraClientes() {
            try {
                string consulta = "Select * from cliente";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conn);
                using (sqlDataAdapter)
                {
                    DataTable tablaCLientes = new DataTable();
                    sqlDataAdapter.Fill(tablaCLientes);
                    lstClientes.ItemsSource = tablaCLientes.DefaultView;
                    lstClientes.DisplayMemberPath = "NombreCliente";
                    lstClientes.SelectedValuePath = "pkCliente";
                }
            }catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
       
        private void lstClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show($"Click en el cliente: {lstClientes.SelectedValue}");
            CargarPedidoCliente(lstClientes.SelectedValue.ToString());
        }
        private void CargarPedidoCliente(string cliente) {
            try{
                /*
                string consulta = "Select * from Pedido where fkCliente = " + cliente;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conn);
                using (sqlDataAdapter)
                {
                    DataTable tablaPedidos = new DataTable();
                    sqlDataAdapter.Fill(tablaPedidos);
                    lstPedidos.ItemsSource = tablaPedidos.DefaultView;
                    lstPedidos.DisplayMemberPath = "FechaPedido";
                    lstPedidos.SelectedValuePath = "pkPedido";
                }
                */
                string consulta = "Select * from Pedido p left join Cliente c on p.fkCliente = c.pkCliente where c.pkCliente=@ClienteID";  // lo otro tb funciona pero ok... puede ser interesante para otros temas
                SqlCommand sqlComando = new SqlCommand(consulta, conn);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlComando);
                using (sqlDataAdapter)
                {
                    sqlComando.Parameters.AddWithValue("@ClienteID", cliente);
                    DataTable tablaPedidos = new DataTable();
                    sqlDataAdapter.Fill(tablaPedidos);
                    lstPedidos.ItemsSource = tablaPedidos.DefaultView;
                    lstPedidos.DisplayMemberPath = "FechaPedido";
                    lstPedidos.SelectedValuePath = "pkPedido";
                }
            }catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void CargarPedidos() {
            try{
                string consulta = "Select CONCAT(FechaPedido, ' ', FormaPago) AS ResumenPedido, pkPedido from Pedido";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(consulta, conn);
                using (sqlDataAdapter)
                {
                    DataTable tablaPedidos = new DataTable();
                    sqlDataAdapter.Fill(tablaPedidos);
                    lstTodosPedidos.ItemsSource = tablaPedidos.DefaultView;
                    lstTodosPedidos.DisplayMemberPath = "ResumenPedido";
                    lstTodosPedidos.SelectedValuePath = "pkPedido";
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnBorrarPedido_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                if (lstTodosPedidos.SelectedValue == null)
                    MessageBox.Show("Selecciona un pedido");
                else
                {
                    MessageBox.Show($"Eliminar el pedido {lstTodosPedidos.SelectedValue.ToString()}");
                }
                */
                string consulta = "DELETE from Pedido WHERE pkPedido=@PedidoId";
                SqlCommand sqlComando = new SqlCommand(consulta, conn);
                conn.Open();
                sqlComando.Parameters.AddWithValue("@PedidoId", lstTodosPedidos.SelectedValue.ToString());
                sqlComando.ExecuteNonQuery();
                conn.Close();
                CargarPedidos();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnInsertarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "Insert into Cliente (NombreCliente, Direccion, Poblacion, Telefono) values (@NombreCliente, @Direccion, @Poblacion, @Telefono)";
                SqlCommand sqlComando = new SqlCommand(consulta, conn);
                conn.Open();
                sqlComando.Parameters.AddWithValue("@NombreCliente", txtNombre.Text);
                sqlComando.Parameters.AddWithValue("@Direccion", txtDireccion.Text);
                sqlComando.Parameters.AddWithValue("@Poblacion", txtPoblacion.Text);
                sqlComando.Parameters.AddWithValue("@Telefono", txtTelefono.Text);
                sqlComando.ExecuteNonQuery();
                conn.Close();
                txtNombre.Text = "";
                txtDireccion.Text = "";
                txtPoblacion.Text = "";
                txtTelefono.Text = "";
                MuestraClientes();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBorrarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "DELETE from Cliente WHERE pkCliente=@ClienteId";
                SqlCommand sqlComando = new SqlCommand(consulta, conn);
                conn.Open();
                sqlComando.Parameters.AddWithValue("@ClienteId", lstClientes.SelectedValue.ToString());
                sqlComando.ExecuteNonQuery();
                conn.Close();
                txtNombre.Text = "";
                txtDireccion.Text = "";
                txtPoblacion.Text = "";
                txtTelefono.Text = "";
                MuestraClientes();
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActualizarCliente_Click(object sender, RoutedEventArgs e)
        {
            ActualizarClienteWindow ActualizarClienteVentana = new ActualizarClienteWindow(lstClientes.SelectedValue.ToString(),conn);
            ActualizarClienteVentana.ShowDialog();
        }
    }
}