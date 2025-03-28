using System;
using System.IO;
using AutoGestPro.Modelos;

namespace AutoGestPro.Estructuras
{
    [Serializable]
    public class ListaSimpleUsuarios
    {
        private NodoUsuario? cabeza;

        public ListaSimpleUsuarios()
        {
            cabeza = null;
        }

        // Método para agregar un usuario
        public void AgregarUsuario(Usuario nuevoUsuario)
        {
            if (ExisteUsuario(nuevoUsuario.Id, nuevoUsuario.Correo))
            {
                Console.WriteLine("❌ Error: ID o Correo ya existen en el sistema.");
                return;
            }

            NodoUsuario nuevoNodo = new NodoUsuario(nuevoUsuario);

            if (cabeza == null)
            {
                cabeza = nuevoNodo;
            }
            else
            {
                NodoUsuario actual = cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevoNodo;
            }

            Console.WriteLine("✅ Usuario agregado con éxito.");
        }

        // Método para eliminar un usuario
        public void EliminarUsuario(int id)
        {
            if (cabeza == null)
            {
                Console.WriteLine("⚠️ La lista está vacía.");
                return;
            }

            if (cabeza.Usuario.Id == id)
            {
                cabeza = cabeza.Siguiente;
                Console.WriteLine("✅ Usuario eliminado.");
                return;
            }

            NodoUsuario actual = cabeza;
            NodoUsuario? anterior = null;

            while (actual != null && actual.Usuario.Id != id)
            {
                anterior = actual;
                actual = actual.Siguiente;
            }

            if (actual == null)
            {
                Console.WriteLine("❌ Usuario no encontrado.");
                return;
            }

            anterior!.Siguiente = actual.Siguiente;
            Console.WriteLine("✅ Usuario eliminado.");
        }

        // Método para buscar un usuario
        public Usuario? BuscarUsuario(int id)
        {
            NodoUsuario? actual = cabeza;
            while (actual != null)
            {
                if (actual.Usuario.Id == id)
                {
                    return actual.Usuario;
                }
                actual = actual.Siguiente!;
            }
            return null;
        }

        // Método para verificar si un usuario existe (por ID o Correo)
        private bool ExisteUsuario(int id, string correo)
        {
            NodoUsuario? actual = cabeza;
            while (actual != null)
            {
                if (actual.Usuario.Id == id || actual.Usuario.Correo == correo)
                {
                    return true;
                }
                actual = actual.Siguiente!;
            }
            return false;
        }

        // Método para mostrar los usuarios
        public void MostrarUsuarios()
        {
            if (cabeza == null)
            {
                Console.WriteLine("⚠️ No hay usuarios registrados.");
                return;
            }

            NodoUsuario? actual = cabeza;
            Console.WriteLine("📋 Lista de Usuarios:");
            while (actual != null)
            {
                Console.WriteLine(actual.Usuario);
                actual = actual.Siguiente!;
            }
        }

        // Método para buscar usuario por credenciales

        public Usuario? BuscarUsuarioPorCredenciales(string correo, string contrasenia)
        {
            NodoUsuario? actual = cabeza;
            while (actual != null)
            {
                if (actual.Usuario.Correo == correo && actual.Usuario.Contrasenia == contrasenia)
                {
                    return actual.Usuario;
                }
                actual = actual.Siguiente;
            }
            return null;
        }

        // Método para generar el archivo .dot
        public void GenerarDot(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("digraph ListaSimpleUsuarios {");
                writer.WriteLine("rankdir=LR;"); // Dirección de izquierda a derecha
                writer.WriteLine("node [shape=record];");

                NodoUsuario? actual = cabeza;
                int contador = 0;

                while (actual != null)
                {
                    // Crear un nodo para cada usuario
                    writer.WriteLine($"node{contador} [label=\"ID: {actual.Usuario.Id}\\nCorreo: {actual.Usuario.Correo}\"];");

                    // Crear la conexión entre nodos
                    if (actual.Siguiente != null)
                    {
                        writer.WriteLine($"node{contador} -> node{contador + 1};");
                    }

                    actual = actual.Siguiente;
                    contador++;
                }

                writer.WriteLine("}");
            }

            Console.WriteLine($"✅ Archivo .dot generado en: {filePath}");
        }
    }
}