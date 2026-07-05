using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace Punto.Forms
{
    public partial class frmProductos : Form
    {
        //creamos instancia de la clase ConnectionData
        private ConnectionData acesso_Datos;

        public frmProductos()
        {
            InitializeComponent();
        }

        //metodo para cargar los datos de la base de datos en el datagridview
        private void cargaDatos()
        {
            //conectamos con base de datos
            acesso_Datos = new ConnectionData();
            MySqlConnection conexionDB = acesso_Datos.getConection();

            // verificar que la conexion
            if (conexionDB != null)
            {
                //creamos la consulta
                string consulta = "SELECT * FROM productos";

                //creamos un adaptador para almacenar los resultados de la consulta
                MySqlDataAdapter adapter = new MySqlDataAdapter(consulta, conexionDB);

                //creamos un datatable para almacenar los datos
                DataTable dt = new DataTable();
                //vaciamos el adapter al datatable
                adapter.Fill(dt);

                //asignamos el datatable al datagridview
                dgvProductos.DataSource = dt;
                //ocultamos el ID del producto
                dgvProductos.Columns["producto_id"].Visible = false;
            }

        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            cargaDatos();
        }

        private void dgvProductos_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //verificamos que este seleccionada una celda
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProductos.Rows[e.RowIndex];
                lblId.Text = row.Cells[0].Value.ToString();
                txtCodigo.Text = row.Cells[1].Value.ToString();
                txtNombre.Text = row.Cells[2].Value.ToString();
                txtPrecio.Text = row.Cells[3].Value.ToString();
                txtStock.Text = row.Cells[4].Value.ToString();
                cmbCategorias.Text = row.Cells[5].Value.ToString();
            }

        }
    }
}
