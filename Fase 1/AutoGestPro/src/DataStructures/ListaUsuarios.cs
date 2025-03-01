using System;
using System.Collections.Generic;

namespace AutoGestPro.DataStructures
{
    public unsafe class ListaUsuarios
    {
        private static ListaUsuarios? instancia;
        private NodoUsuario* cabeza; // Puntero al primer nodo

        public ListaUsuarios()
        {
            cabeza = null;
        }

        // Método para obtener la instancia de la única de clase
        public static ListaUsuarios Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ListaUsuarios();
                }
                return instancia;
            }
        }

        // Método para agregar un usuario al final de la lista
        public void AgregarUsuario(int id, string nombres, string apellidos, string correo, string contrasenia)
        {
            NodoUsuario* nuevoUsuario = (NodoUsuario*)System.Runtime.InteropServices.Marshal.AllocHGlobal(sizeof(NodoUsuario));
            *nuevoUsuario = new NodoUsuario(id, nombres, apellidos, correo, contrasenia);

            if (cabeza == null)
            {
                cabeza = nuevoUsuario;
            }
            else
            {
                NodoUsuario* actual = cabeza;
                while (actual->Siguiente != null)
                {
                    actual = actual->Siguiente;
                }
                actual->Siguiente = nuevoUsuario;
            }
        }

        public NodoUsuario* BuscarUsuario(int id)
        {
            NodoUsuario* actual = cabeza;
            while (actual != null)
            {
                if (actual->ID == id)
                {
                    return actual;
                }
                actual = actual->Siguiente;
            }
            return null;
        }

        // Método para editar un usuario por ID

        public void EditarUsuario(int id, string nombres, string apellidos, string correo)
        {
            NodoUsuario* usuario = BuscarUsuario(id);
            if (usuario != null)
            {
            // Usar directamente el método CopyString de NodoUsuario
                NodoUsuario.CopyString(usuario->Nombres, nombres, 50);
                NodoUsuario.CopyString(usuario->Apellidos, apellidos, 50);
                NodoUsuario.CopyString(usuario->Correo, correo, 50);
            }
        }


        // Método para agregar múltiples usuarios desde una lista
        public void AgregarUsuarios(List<Usuario> usuarios)
        {
            foreach (var usuario in usuarios)
            {
                AgregarUsuario(usuario.ID, usuario.Nombres, usuario.Apellidos, usuario.Correo, usuario.Contrasenia);
            }
        }

        // Método para mostrar todos los usuarios
        public void MostrarUsuarios()
        {
            NodoUsuario* actual = cabeza;
            while (actual != null)
            {
                Console.WriteLine($"ID: {actual->ID}, Nombre: {GetString(actual->Nombres)} {GetString(actual->Apellidos)}, Correo: {GetString(actual->Correo)}");
                actual = actual->Siguiente;
            }
        }

    
        // Método para eliminar un usuario por ID
        public void EliminarUsuario(int id)
        {
            if (cabeza == null) return;

            if (cabeza->ID == id)
            {
                NodoUsuario* temp = cabeza;
                cabeza = cabeza->Siguiente;
                System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)temp);
                return;
            }

            NodoUsuario* actual = cabeza;
            while (actual->Siguiente != null && actual->Siguiente->ID != id)
            {
                actual = actual->Siguiente;
            }

            if (actual->Siguiente != null)
            {
                NodoUsuario* temp = actual->Siguiente;
                actual->Siguiente = actual->Siguiente->Siguiente;
                System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)temp);
            }
        }

        // Función auxiliar para convertir char* a string
        private static string GetString(char* ptr)
        {
            return new string(ptr);
        }
    }

    public class Usuario
    {
        public int ID { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contrasenia { get; set; } = string.Empty;
    }
    
}
