using Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class D_Usuario
    {
        private string cadenaconexion = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
        public void CreateUsuario(E_Usuario usuario)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            try
            {
                conexion.Open();

                SqlCommand comando = new SqlCommand("spRFCAgregarUsuario", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@sp_nombre", usuario.nombre);
                comando.Parameters.AddWithValue("@sp_aPat", usuario.apellidoPaterno);
                comando.Parameters.AddWithValue("@sp_aMat", usuario.apellidoMaterno);
                comando.Parameters.AddWithValue("@sp_fecha", usuario.fechaNacimiento);
                comando.Parameters.AddWithValue("@sp_codRFC", usuario.codigoRFC);
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }
        public List<E_Usuario> ReadTodos()
        {
            List<E_Usuario> lista = new List<E_Usuario>();
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand("spRFCObtenerTodo", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    E_Usuario usuario = new E_Usuario();
                    usuario.idUsuario = Convert.ToInt32(reader["idUsuario"]);
                    usuario.nombre = Convert.ToString(reader["nombre"]);
                    usuario.apellidoPaterno = Convert.ToString(reader["apPaterno"]);
                    usuario.apellidoMaterno = Convert.ToString(reader["apMaterno"]);
                    usuario.fechaNacimiento = Convert.ToDateTime(reader["fechaNacimiento"]);
                    usuario.codigoRFC = Convert.ToString(reader["codigoRFC"]);
                    lista.Add(usuario);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
            return lista;
        }
        public E_Usuario ReadUsuario(int ID)
        {
            E_Usuario usuario = new E_Usuario();
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand("spRFCObtenerUsuario", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@sp_id", ID);

                SqlDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    usuario.idUsuario = Convert.ToInt32(reader["idUsuario"]);
                    usuario.nombre = Convert.ToString(reader["nombre"]);
                    usuario.apellidoPaterno = Convert.ToString(reader["apPaterno"]);
                    usuario.apellidoMaterno = Convert.ToString(reader["apMaterno"]);
                    usuario.fechaNacimiento = Convert.ToDateTime(reader["fechaNacimiento"]);
                    usuario.codigoRFC = Convert.ToString(reader["codigoRFC"]);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
            return usuario;
        }
        public void UpdateUsuario(E_Usuario usuario)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand("spRFCActaulizarUsuario", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@sp_id", usuario.idUsuario);
                comando.Parameters.AddWithValue("@sp_nombre", usuario.nombre);
                comando.Parameters.AddWithValue("@sp_aPat", usuario.apellidoPaterno);
                comando.Parameters.AddWithValue("@sp_aMat", usuario.apellidoMaterno);
                comando.Parameters.AddWithValue("@sp_fecha", usuario.fechaNacimiento);
                comando.Parameters.AddWithValue("@sp_codRFC", usuario.codigoRFC);
                comando.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }
        public void DeleteUsuario(int ID)
        {
            SqlConnection conexion = new SqlConnection(cadenaconexion);
            try
            {
                conexion.Open();
                SqlCommand comando = new SqlCommand("spRFCEliminarUsuario", conexion);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@sp_id", ID);
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conexion.Close();
            }
        }
    }
}
