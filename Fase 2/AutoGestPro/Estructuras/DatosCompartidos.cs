using AutoGestPro.Estructuras;
using AutoGestPro.Interfaz;
using AutoGestPro.Modelos;

namespace AutoGestPro
{
    public static class DatosCompartidos
    {
        public static ListaSimpleUsuarios ListaUsuarios { get; set; } = new ListaSimpleUsuarios();
        public static ListaDobleVehiculos ListaVehiculos { get; set; } = new ListaDobleVehiculos();
        public static ArbolAVLRepuestos ArbolRepuestos { get; set; } = new ArbolAVLRepuestos();
        public static ArbolBinarioServicios ArbolServicios { get; set; } = new ArbolBinarioServicios();
        public static ArbolBFacturas ArbolFacturas { get; set; } = new ArbolBFacturas();

        // Instancia para el control de logueo
        public static VentanaControlLogueo ControlLog { get; set; } = new VentanaControlLogueo();

        // Usuario actual que ha iniciado sesión
        public static Usuario? UsuarioActual { get; set; }
    }
}