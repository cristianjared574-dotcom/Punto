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

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            //rescatamos valores
            string codigo = txtCodigo.Text;
            string descripcion = txtNombre.Text;
            decimal precio;
            int stock;
            string categoria = cmbCategorias.Text;

            //Validar que los campos obligatorios no estén vacíos
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("El código del producto es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCodigo.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("La descripción del producto es obligatoria.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show("El precio es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("El stock es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(cmbCategorias.Text))
            {
                MessageBox.Show("La categoría es obligatoria.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategorias.Focus();
                return;
            }

            //Validar precio y stock
            if (!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("Ingrese un precio válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecio.Focus();
                return;
            }

            if (!int.TryParse(txtStock.Text, out stock))
            {
                MessageBox.Show("Ingrese un stock válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return;
            }

            //Conectar a la base de datos
            acesso_Datos = new ConnectionData();
            MySqlConnection conexionDB = acesso_Datos.getConection();

            if (conexionDB == null)
            {
                MessageBox.Show("No fue posible conectar con la base de datos.");
                return;
            }

            //Crear la consulta
            string consulta = "INSERT INTO productos (codigo, descripcion, precio, stock, categoria) " + "VALUES (@codigo, @descripcion, @precio, @stock, @categoria)";

            //Crear el comando
            MySqlCommand comando = new MySqlCommand(consulta, conexionDB);

            comando.Parameters.AddWithValue("@codigo", codigo);
            comando.Parameters.AddWithValue("@descripcion", descripcion);
            comando.Parameters.AddWithValue("@precio", precio);
            comando.Parameters.AddWithValue("@stock", stock);
            comando.Parameters.AddWithValue("@categoria", categoria);

            //Ejecutar la consulta
            int filasAfectadas = comando.ExecuteNonQuery();
            conexionDB.Close();

            //Verificar el registro
            if (filasAfectadas > 0)
            {
                MessageBox.Show("Producto registrado correctamente.");
                cargaDatos();
                LimpiarFormulario();
            }
            else
            {
                MessageBox.Show("No fue posible registrar el producto.");
            }

        }

        private void LimpiarFormulario()
        {
            lblId.Text = "";
            txtCodigo.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            cmbCategorias.SelectedIndex = -1;
        }

    }
}
