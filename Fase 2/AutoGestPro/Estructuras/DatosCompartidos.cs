using AutoGestPro.Estructuras;

namespace AutoGestPro
{
    public static class DatosCompartidos
    {
        public static ListaSimpleUsuarios ListaUsuarios { get; set; } = new ListaSimpleUsuarios();
        public static ListaDobleVehiculos ListaVehiculos { get; set; } = new ListaDobleVehiculos();
        public static ArbolAVLRepuestos ArbolRepuestos { get; set; } = new ArbolAVLRepuestos();
        public static ArbolBinarioServicios ArbolServicios { get; set; } = new ArbolBinarioServicios();
        public static ArbolBFacturas ArbolFacturas { get; set; } = new ArbolBFacturas();
    }
}