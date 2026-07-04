using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Punto
{
    internal class ConnectionData
    {
        private readonly string cadena; 

        public ConnectionData() 
        { 
            cadena = "Server=localhost; Database=puntodb; UserID=root; Password=; Port=3306; SslMode=None;"; 
        }
        public MySqlConnection getConection()
        {
            try 
            {
                MySqlConnection conexion = new MySqlConnection(cadena);
                conexion.Open(); 
                return conexion; 
            } 
            catch (Exception ex) 
            { 
                MessageBox.Show("Error al conectar a la base de datos: " + ex.Message); 
                return null; 
            }

        }
    }
}
