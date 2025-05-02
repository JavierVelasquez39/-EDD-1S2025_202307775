# Manual Técnico #

## **INTRODUCCIÓN**

Este manual técnico ofrece una visión detallada de la lógica de funcionamiento de la aplicación **AutoGest Pro**, enfocándose en la gestión de usuarios, vehículos, repuestos, servicios y facturas. El proyecto utiliza estructuras de datos avanzadas como **Blockchain**, **Árbol AVL**, **Árbol Binario**, **Árbol de Merkle**, y **Lista Doble Enlazada** para garantizar un manejo eficiente de la información.

Se describen los siguientes aspectos:

- Estructura de la aplicación y cómo se implementa utilizando clases y estructuras de datos.
- Proceso de gestión de usuarios, vehículos, repuestos, servicios y facturas.
- Detalles sobre la interfaz gráfica utilizando **GTK**.
- Componentes clave de la aplicación y cómo interactúan entre sí.
- Criterios utilizados para la gestión de inventarios y servicios.

---

## **OBJETIVOS**

### **GENERAL**

Proporcionar una guía detallada sobre la implementación y funcionamiento del sistema de gestión de inventario desarrollado en **C#**.

### **ESPECÍFICOS**

1. Explicar la implementación de las estructuras de datos en **C#**, describiendo los métodos utilizados para la gestión de usuarios, vehículos, repuestos, servicios y facturas.
2. Detallar el proceso de integración entre las estructuras de datos y la interfaz gráfica en **GTK**, destacando la interacción entre estos componentes.

---

## **ALCANCES DEL SISTEMA**

El manual cubre todos los aspectos técnicos del sistema, incluyendo la lógica de programación, las estructuras de datos utilizadas y los algoritmos aplicados para la gestión de usuarios, vehículos, repuestos, servicios y facturas. Además, se explica cómo el código fue diseñado para cumplir con las especificaciones del proyecto, respetando las restricciones impuestas, y cómo se puede adaptar o mejorar para futuros proyectos o necesidades similares.

Este documento tiene como fin asegurar que cualquier persona con conocimientos básicos de programación en **C#** y **GTK** pueda replicar, mantener o mejorar el sistema descrito, comprendiendo cada uno de sus componentes, la lógica de gestión de datos, la interacción con la interfaz gráfica y la generación de reportes.

---

## **ESPECIFICACIÓN TÉCNICA**

### **REQUISITOS DE HARDWARE**

- **Memoria RAM:** 4 GB como mínimo.
- **Almacenamiento:** 500 MB de espacio libre en disco duro.

### **REQUISITOS DE SOFTWARE**

- **SDK de .NET:** .NET 9.0 o superior.
- **Editor de Código:** Visual Studio Code, Visual Studio, o cualquier editor de texto con soporte para **C#**.

---

## **DESCRIPCIÓN DE LA SOLUCIÓN**

Se identificaron las funcionalidades esenciales que el sistema debía cumplir, como la gestión de usuarios, vehículos, repuestos, servicios y facturas. Cada una de estas funciones se desglosó en tareas más pequeñas para facilitar su implementación y asegurar su correcto funcionamiento.

Basándonos en los requerimientos del proyecto, se diseñó una estructura modular para el programa. Cada módulo se encarga de una funcionalidad específica, como la gestión de usuarios, vehículos, repuestos, servicios y facturas. Este enfoque modular facilita la comprensión, mantenimiento y futura ampliación del código, permitiendo que cada componente funcione de manera independiente pero coordinada.

Para almacenar y procesar los datos, se optó por utilizar estructuras avanzadas como **Blockchain** para usuarios, **Lista Doble Enlazada** para vehículos, **Árbol AVL** para repuestos, **Árbol Binario** para servicios y **Árbol de Merkle** para facturas. Además, se emplearon estructuras en **GTK** para manejar la interfaz gráfica, lo que permitió una integración fluida entre las dos tecnologías.

Finalmente, las funciones se implementaron de acuerdo con el diseño modular y se realizaron pruebas exhaustivas para asegurar que el sistema cumpliera con todos los requerimientos especificados. Esto incluyó la correcta manipulación de datos, la detección de errores, la generación de reportes precisos y la presentación clara de los resultados en la interfaz gráfica.

---

## **CLASES PRINCIPALES**

### **1. Program**
- **Descripción:** Punto de entrada de la aplicación.
- **Función:** Inicializa la aplicación GTK y muestra la ventana de inicio de sesión.
- **Código Relevante:**
```csharp
class Program
{
    static void Main(string[] args)
    {
        Application.Init();
        VentanaLogin ventanaLogin = new VentanaLogin();
        ventanaLogin.Show();
        Application.Run();
    }
}
```

### **2. Blockchain**
- **Descripción:** Estructura de datos utilizada para gestionar usuarios.
- **Función:** Almacena los usuarios en bloques encadenados, asegurando la integridad de los datos mediante hashes.
- **Código Relevante:**
```csharp
public void AddBlock(Usuario data)
{
    // Hashear la contraseña antes de agregarla al blockchain
    data.Contrasenia = CalcularHash(data.Contrasenia.Trim());

    // Crear un nuevo bloque
    Block previousBlock = Chain[Chain.Count - 1];
    Block newBlock = new Block(previousBlock.Index + 1, data, previousBlock.Hash);

    // Minar el bloque con la dificultad establecida
    newBlock.MineBlock(Difficulty);

    // Agregar el bloque a la cadena
    Chain.Add(newBlock);

    // Guardar el blockchain en un archivo
    GuardarEnArchivo("blockchain_usuarios.json");
}
```
### **3. ListaDobleVehiculos**
- **Descripción:** Lista doblemente enlazada para gestionar vehículos.
- **Función:** Permite agregar, eliminar y buscar vehículos, además de generar reportes gráficos.
- **Código Relevante:**
```csharp
public void AgregarVehiculo(Vehiculo nuevoVehiculo)
{
    NodoVehiculo nuevoNodo = new NodoVehiculo(nuevoVehiculo);
    if (cabeza == null)
    {
        cabeza = nuevoNodo;
        cola = nuevoNodo;
    }
    else
    {
        cola!.Siguiente = nuevoNodo;
        nuevoNodo.Anterior = cola;
        cola = nuevoNodo;
    }
}

public Vehiculo? BuscarVehiculo(int id)
{
    NodoVehiculo? actual = cabeza;
    while (actual != null)
    {
        if (actual.Dato.Id == id)
        {
            return actual.Dato;
        }
        actual = actual.Siguiente;
    }
    return null;
}

public void EliminarVehiculo(int id)
{
    NodoVehiculo? actual = cabeza;
    while (actual != null)
    {
        if (actual.Dato.Id == id)
        {
            if (actual.Anterior != null)
                actual.Anterior.Siguiente = actual.Siguiente;
            else
                cabeza = actual.Siguiente;

            if (actual.Siguiente != null)
                actual.Siguiente.Anterior = actual.Anterior;
            else
                cola = actual.Anterior;

            return;
        }
        actual = actual.Siguiente;
    }
}
```
### **4. ArbolAVLRepuestos**
- **Descripción:** Árbol AVL para gestionar repuestos.
- **Función:** Almacena los repuestos de manera balanceada, permitiendo búsquedas rápidas y eficientes.
- **Código Relevante:**
```csharp
public void Insertar(Repuesto repuesto)
{
    raiz = InsertarNodo(raiz, repuesto);
}

private NodoAVL InsertarNodo(NodoAVL? nodo, Repuesto repuesto)
{
    if (nodo == null)
        return new NodoAVL(repuesto);

    if (repuesto.Id < nodo.Dato.Id)
        nodo.Izquierdo = InsertarNodo(nodo.Izquierdo, repuesto);
    else if (repuesto.Id > nodo.Dato.Id)
        nodo.Derecho = InsertarNodo(nodo.Derecho, repuesto);
    else
        throw new Exception("El repuesto ya existe en el árbol.");

    nodo = Balancear(nodo);
    return nodo;
}

private NodoAVL Balancear(NodoAVL nodo)
{
    int balance = ObtenerFactorBalance(nodo);

    if (balance > 1)
    {
        if (ObtenerFactorBalance(nodo.Izquierdo) < 0)
            nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo!);
        return RotarDerecha(nodo);
    }

    if (balance < -1)
    {
        if (ObtenerFactorBalance(nodo.Derecho) > 0)
            nodo.Derecho = RotarDerecha(nodo.Derecho!);
        return RotarIzquierda(nodo);
    }

    return nodo;
}

private int ObtenerFactorBalance(NodoAVL? nodo)
{
    return (nodo == null) ? 0 : Altura(nodo.Izquierdo) - Altura(nodo.Derecho);
}

private int Altura(NodoAVL? nodo)
{
    return (nodo == null) ? 0 : nodo.Altura;
}
```
### **5. ArbolBinarioServicios**
- **Descripción:** Árbol binario para gestionar servicios.
- **Función:** Almacena los servicios asociados a vehículos, permitiendo búsquedas y generación de reportes.
- **Código Relevante:**
```csharp
public void Insertar(Servicio servicio)
{
    raiz = InsertarNodo(raiz, servicio);
}

private NodoBinario InsertarNodo(NodoBinario? nodo, Servicio servicio)
{
    if (nodo == null)
        return new NodoBinario(servicio);

    if (servicio.Id < nodo.Dato.Id)
        nodo.Izquierdo = InsertarNodo(nodo.Izquierdo, servicio);
    else if (servicio.Id > nodo.Dato.Id)
        nodo.Derecho = InsertarNodo(nodo.Derecho, servicio);
    else
        throw new Exception("El servicio ya existe en el árbol.");

    return nodo;
}

public Servicio? BuscarServicio(int id)
{
    NodoBinario? actual = raiz;
    while (actual != null)
    {
        if (id == actual.Dato.Id)
            return actual.Dato;
        else if (id < actual.Dato.Id)
            actual = actual.Izquierdo;
        else
            actual = actual.Derecho;
    }
    return null;
}
```
### **6. MerkleTree**
- **Descripción:** Árbol de Merkle para gestionar facturas.
- **Función:** Almacena las facturas de manera segura, permitiendo verificar la integridad de los datos.
- **Código Relevante:**
```csharp
public void AddFactura(Factura factura)
{
    string data = $"{factura.Id}|{factura.Id_Servicio}|{factura.Id_Vehiculo}|{factura.Total}|{factura.Fecha:yyyy-MM-dd}|{factura.MetodoPago}";
    Leaves.Add(new MerkleNode(data));
    BuildTree();
}

private void BuildTree()
{
    List<MerkleNode> currentLevel = Leaves;

    while (currentLevel.Count > 1)
    {
        List<MerkleNode> nextLevel = new List<MerkleNode>();

        for (int i = 0; i < currentLevel.Count; i += 2)
        {
            if (i + 1 < currentLevel.Count)
            {
                string combinedHash = Hash(currentLevel[i].Hash + currentLevel[i + 1].Hash);
                nextLevel.Add(new MerkleNode(combinedHash));
            }
            else
            {
                nextLevel.Add(currentLevel[i]);
            }
        }

        currentLevel = nextLevel;
    }

    Root = currentLevel.FirstOrDefault();
}

private string Hash(string input)
{
    using (SHA256 sha256 = SHA256.Create())
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}
```
## **INTEGRACIÓN CON LA INTERFAZ GRÁFICA**

La interfaz gráfica fue desarrollada utilizando **GTK** y se conecta directamente con las estructuras de datos para proporcionar una experiencia interactiva y eficiente. A continuación, se describen las ventanas principales:

### **VentanaLogin**
- **Función:** Permite a los usuarios iniciar sesión en el sistema.
- **Código Relevante:**
```csharp
public VentanaLogin() : base("Inicio de Sesión")
{
    // Crear los elementos de la interfaz
    VBox vbox = new VBox();
    Label lblCorreo = new Label("Correo:");
    Entry txtCorreo = new Entry();
    Label lblContrasenia = new Label("Contraseña:");
    Entry txtContrasenia = new Entry { Visibility = false };
    Button btnLogin = new Button("Iniciar Sesión");

    // Agregar los elementos al contenedor
    vbox.PackStart(lblCorreo, false, false, 5);
    vbox.PackStart(txtCorreo, false, false, 5);
    vbox.PackStart(lblContrasenia, false, false, 5);
    vbox.PackStart(txtContrasenia, false, false, 5);
    vbox.PackStart(btnLogin, false, false, 10);

    Add(vbox);

    // Evento para el botón de inicio de sesión
    btnLogin.Clicked += (sender, e) =>
    {
        string correo = txtCorreo.Text;
        string contrasenia = txtContrasenia.Text;

        // Autenticar al usuario utilizando el Blockchain
        Block? bloqueUsuario = DatosCompartidos.BlockchainUsuarios.AutenticarUsuario(correo, contrasenia);
        if (bloqueUsuario != null)
        {
            Console.WriteLine($"Bienvenido, {bloqueUsuario.Data.Nombres}");
            VentanaUsuario ventanaUsuario = new VentanaUsuario(bloqueUsuario);
            ventanaUsuario.Show();
            this.Destroy();
        }
        else
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Credenciales incorrectas");
            dialog.Run();
            dialog.Destroy();
        }
    };

    ShowAll();
}
### **VentanaAdmin**
- **Función:** Proporciona acceso a las funcionalidades del administrador, como cargas masivas, generación de reportes y gestión de entidades.
- **Código Relevante:**
```csharp
public VentanaAdmin() : base("Menú Administrador")
{
    // Crear los elementos de la interfaz
    VBox vbox = new VBox();
    Button btnCargasMasivas = new Button("Cargas Masivas");
    Button btnGenerarReportes = new Button("Generar Reportes");
    Button btnGestionEntidades = new Button("Gestión de Entidades");

    // Agregar los elementos al contenedor
    vbox.PackStart(btnCargasMasivas, false, false, 10);
    vbox.PackStart(btnGenerarReportes, false, false, 10);
    vbox.PackStart(btnGestionEntidades, false, false, 10);

    Add(vbox);

    // Evento para el botón de cargas masivas
    btnCargasMasivas.Clicked += (sender, e) =>
    {
        VentanaCargaMasiva ventanaCargaMasiva = new VentanaCargaMasiva();
        ventanaCargaMasiva.Show();
    };

    // Evento para el botón de generación de reportes
    btnGenerarReportes.Clicked += (sender, e) =>
    {
        VentanaGenerarGrafica ventanaGenerarGrafica = new VentanaGenerarGrafica();
        ventanaGenerarGrafica.Show();
    };

    // Evento para el botón de gestión de entidades
    btnGestionEntidades.Clicked += (sender, e) =>
    {
        VentanaGestionEntidades ventanaGestionEntidades = new VentanaGestionEntidades();
        ventanaGestionEntidades.Show();
    };

    ShowAll();
}
```
## **CONCLUSIÓN**

Este manual técnico proporciona una guía completa para comprender la implementación de **AutoGest Pro**. Siguiendo los detalles descritos, los desarrolladores podrán mantener, mejorar y ampliar el sistema de manera eficiente.

El uso de estructuras de datos avanzadas como **Blockchain**, **Árbol AVL**, **Árbol Binario**, **Árbol de Merkle**, y **Lista Doble Enlazada** garantiza un manejo eficiente y seguro de la información. Además, la integración con la interfaz gráfica desarrollada en **GTK** permite una experiencia de usuario fluida e intuitiva.

Con este manual, cualquier desarrollador con conocimientos básicos en **C#** y **GTK** podrá comprender la lógica del sistema, realizar modificaciones necesarias y adaptarlo a nuevos requerimientos. Esto asegura la sostenibilidad y escalabilidad del proyecto a largo plazo.