using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Entidades;

namespace Bussiness
{
    public class B_Usuario
    {
        public List<E_Usuario> Obtenertodos()
        {
            List<E_Usuario> Usuarios = new List<E_Usuario>();
            D_Usuario datos = new D_Usuario();
            Usuarios = datos.ReadTodos();
            //Usuarios = Buscador("ARTURO");
            RFCduplicados(Usuarios);
            return Usuarios;
        }
        public void RFCduplicados(List<E_Usuario> usuarios)
        {
            //Obtener RFC
            //Obtener duplicados
            for (int i = 0; i < usuarios.Count; i++)
            {
                int cont = 0;
                for (int j = 0; j < usuarios.Count; j++)
                {
                    string palabraI = usuarios[i].codigoRFC;
                    string palabraJ = usuarios[j].codigoRFC;
                    if (palabraI == palabraJ)
                    {
                        cont++;
                    }
                    if (cont > 1)
                    {
                        cont = 1;
                        //Con i incluye los todos
                        //con j incluye los solo los que se repiten y los orginales no los marca
                        usuarios[j].RFCduplicado = true;
                    }
                }
            }
        }
        public List<E_Usuario> Buscador(string cadena)
        {
            if (cadena == null)
            {
                cadena = "";
            }
            cadena = cadena.ToUpper();
            List<E_Usuario> Usuarios = new List<E_Usuario>();
            List<E_Usuario> match = new List<E_Usuario>();
            D_Usuario datos = new D_Usuario();
            Usuarios = datos.ReadTodos();
            RFCduplicados(Usuarios);
            foreach (E_Usuario user in Usuarios)
            {
                if (user.nombre.Contains(cadena) || user.apellidoPaterno.Contains(cadena) || user.apellidoMaterno.Contains(cadena) || user.fechaNacimiento.ToString("d/MMM/yyyy", new CultureInfo("es-ES")).ToUpper().Contains(cadena) || user.codigoRFC.Contains(cadena))
                {
                    match.Add(user);
                }
            }
            return match;

        }

        public E_Usuario ObtenerUsuario(int ID)
        {
            E_Usuario usuario = new E_Usuario();
            D_Usuario datos = new D_Usuario();
            usuario = datos.ReadUsuario(ID);
            return usuario;
        }
        public void AgregarUsuario(E_Usuario usuario)
        {
            usuario.codigoRFC = CrearRFC(usuario);
            D_Usuario datos = new D_Usuario();
            datos.CreateUsuario(usuario);
        }
        public void ActualizarUsuario(E_Usuario usuario)
        {
            usuario.codigoRFC = CrearRFC(usuario);
            D_Usuario datos = new D_Usuario();
            datos.UpdateUsuario(usuario);
        }
        public void BorrarUsuario(int ID)
        {
            D_Usuario datos = new D_Usuario();
            datos.DeleteUsuario(ID);
        }
        public string CrearRFC(E_Usuario usuario)
        {
            //mayusuclas y quitar espacios
            usuario.nombre = usuario.nombre.Trim().ToUpper();
            usuario.apellidoPaterno = usuario.apellidoPaterno.Trim().ToUpper();

            string Letras2;
            string Letras3;


            Letras2 = ObtenerVocalInterna(usuario.apellidoPaterno);
            //Excepción 5
            // Reviso si existe apelldio materno
            //si existen dos apellidos, si el primer apellido no tiene vocal interna, se le asignará una "X" en la segunda posición.
            if (usuario.apellidoMaterno != null)
            {
                //Darle el formato al aplldio materno
                usuario.apellidoMaterno = usuario.apellidoMaterno.Trim().ToUpper();
                Letras3 = usuario.apellidoMaterno.Substring(0, 1);
                //Excepción 1
                //Si el apellido materno empieza con "Ñ, se cambia por "X""
                if (Letras3 == "Ñ")
                {
                    Letras3 = "X";
                }
            }
            //si no existe el segundo apellido, se asignará una X en la tercera posición.
            else
            {
                Letras3 = "X";  // Si no tiene apellido materno, se coloca "X"
                usuario.apellidoMaterno = "";
            }

            //Excepción 3
            //No importa si el apellido paterno son 10 palabras, tomara siempre la primera
            string Letras1 = usuario.apellidoPaterno.Substring(0, 1);
            //Excepción 1
            // Si el apellido paterno comienza con "Ñ", se cambia por "X"
            if (Letras1 == "Ñ")
            {
                Letras1 = "X";
            }

            //Excepción 2
            // Si hay 2 nombres debo elegir el primero siempre y cuando no se maria, jose o derivados
            string Letras4 = ObtenerLetraNombre(usuario.nombre);

            //reviso si hay malaspalabras
            Letras1 = RevisarCaracteresEspeciales(Letras1);
            Letras3 = RevisarCaracteresEspeciales(Letras3);
            Letras4 = RevisarCaracteresEspeciales(Letras4);
            string correccion = RevisarPalabras(Letras1 + Letras2 + Letras3 + Letras4);

            string[] fecha = usuario.fechaNacimiento.ToShortDateString().Split('/');
            string Letras510 = fecha[2].Substring(2) + fecha[1] + fecha[0];

            return correccion + Letras510;
        }

        public string ObtenerVocalInterna(string apellido)
        {
            string vocales = "AEIOU";
            string caracterespecial = "/.-";
            string prueba = apellido.Substring(1);//revisa el rsto del apelldio
            foreach (char letra in prueba)
            {
                if (vocales.Contains(letra.ToString()))
                {
                    return letra.ToString();//regreso la primera vocal que encuentre
                }
                else if (caracterespecial.Contains(letra.ToString()))
                {
                    return "X";//Si se obtiene 
                }
            }
            return "X";//Regreso X si no encuentro vocal interna
        }

        public string ObtenerLetraNombre(string nombre)
        {
            string[] subs = nombre.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if ((subs[0] == "MARIA" || subs[0] == "MA." || subs[0] == "MA" || subs[0] == "JOSE" || subs[0] == "J" || subs[0] == "J.") && subs.Length > 1)
            {
                return subs[1].Substring(0, 1);  //tooma la primera letra del segundo nombre
            }
            return subs[0].Substring(0, 1);  //toma la primera letra del primer nombre
        }

        public string RevisarPalabras(string codigo)
        {
            HashSet<string> nombres = new HashSet<string> {
        "BUEI", "CACA", "CAGA", "CAKA", "COGE", "COJE", "COJO", "FETO", "JOTO", "KACO", "KAGO", "KOJO", "KULO", "MAMO", "MEAS", "MION", "MULA", "PEDO", "PUTA", "QULO",
        "BUEY", "CACO", "CAGO", "CAKO", "COJA", "COJI", "CULO", "GUEY", "KACA", "KAGA", "KOGE", "KAKA", "MAME", "MEAR", "MEON", "MOCO", "PEDA", "PENE", "PUTO", "RATA"
            };

            //Regresa plabra vonc censura
            if (nombres.Contains(codigo))
            {
                return codigo.Substring(0, 3) + "X";
            }

            return codigo;
        }
        public string RevisarCaracteresEspeciales(string letra)
        {
            string caracterespecial = "/.-";
            if (caracterespecial.Contains(letra.ToString()))
            {
                return "X";//Si se obtiene 
            }
            return letra;
        }
        public string MayusculasInicio(string mensaje)
        {
            string[] res = mensaje.Split(' ');
            for (int i = 0; i < res.Length; i++)
            {
                if (res[i] != "")
                {
                    res[i] = res[i][0].ToString().ToUpper() + res[i].Substring(1);
                }
            }
            string resultado = String.Join(" ", res);
            return resultado;
        }
    }
}
