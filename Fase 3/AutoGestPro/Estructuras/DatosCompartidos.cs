using AutoGestPro.Estructuras;
using AutoGestPro.Interfaz;

namespace AutoGestPro
{
    public static class DatosCompartidos
    {
        // Blockchain de usuarios
        private static Blockchain _blockchainUsuarios;
        public static Blockchain BlockchainUsuarios 
        { 
            get 
            {
                if (_blockchainUsuarios == null)
                {
                    // Cargar el blockchain desde el archivo
                    _blockchainUsuarios = Blockchain.CargarDesdeArchivo("blockchain_usuarios.json");
                }
                return _blockchainUsuarios;
            }
        }

        // Bloque del usuario actual que ha iniciado sesión
        public static Block? UsuarioActual { get; set; }

        // Otras estructuras de datos
        public static ListaDobleVehiculos ListaVehiculos { get; set; } = new ListaDobleVehiculos();
        public static ArbolAVLRepuestos ArbolRepuestos { get; set; } = new ArbolAVLRepuestos();
        public static ArbolBinarioServicios ArbolServicios { get; set; } = new ArbolBinarioServicios();
        public static MerkleTree ArbolFacturas { get; set; } = new MerkleTree();
        public static GrafoNoDirigido GrafoRelaciones { get; set; } = new GrafoNoDirigido();

        // Instancia para el control de logueo
        private static VentanaControlLogueo? _controlLog = null;
        public static VentanaControlLogueo ControlLog 
        { 
            get 
            {
                if (_controlLog == null)
                    _controlLog = new VentanaControlLogueo();
                return _controlLog;
            }
        }
    }
}