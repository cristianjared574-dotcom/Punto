using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System;

namespace Punto.Forms
{
    public partial class frmLogin : Form
    {
        private frmPrincipal principal;
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            // Rescatamos valores
            string usuario = txtUser.Text.Trim();
            string contrasena = txtPassword.Text.Trim();

            // Validamos que los campos no estén vacíos
            if(string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contrasena))
            {
                MessageBox.Show("Por favor, complete los campos requeridos...","Advertencia",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //conectamos con base de datos
            ConnectionData acessoDatos = new ConnectionData();
            MySqlConnection conexionBD = acessoDatos.getConection();

            // verificar que la conexion se realizo correctamente
            if(conexionBD == null)
            {
                MessageBox.Show("No se pudo conectar a la base de datos.","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                //creamos la consulta
                string consulta = "SELECT * FROM usuarios WHERE BINARY username = @usuario";

                //ejecutamos la consulta
                MySqlCommand comando = new MySqlCommand(consulta, conexionBD);
                comando.Parameters.AddWithValue("@usuario", usuario);
                MySqlDataReader reader = comando.ExecuteReader();

                //verificamos que la consulta trajo resultados
                if(reader.Read())
                {
                    string passwordBD = reader["password"].ToString();

                    //comparar contraseñas
                    if(contrasena == passwordBD)
                    {
                        MessageBox.Show("Acceso correcto...","Éxito",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        reader.Close();
                        conexionBD.Close();
                        this.Hide();
                        principal = new frmPrincipal();
                        principal.Show();
                    }
                    else
                    {
                        MessageBox.Show("Las credenciales son incorrectas...","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Las credenciales son incorrectas...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
