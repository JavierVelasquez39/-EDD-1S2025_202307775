# Manual Técnico #

## **INTRODUCCIÓN**

Este manual técnico ofrece una visión detallada de la lógica de funcionamiento de la aplicación **AutoGest Pro**, enfocándose en la gestión de usuarios, vehículos, repuestos, servicios y facturas. El proyecto tiene como objetivo aplicar conceptos de estructuras de datos y programación orientada a objetos para crear una aplicación de gestión eficiente.

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

Para almacenar y procesar los datos, se optó por utilizar estructuras definidas en **C#** que permiten un acceso rápido y eficiente a la información durante la gestión de inventarios y servicios. Además, se emplearon estructuras en **GTK** para manejar la interfaz gráfica, lo que permitió una integración fluida entre las dos tecnologías.

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

### **2. VentanaLogin**
- **Descripción:** Ventana de inicio de sesión.
- **Función:** Valida las credenciales del usuario y redirige al menú correspondiente (Administrador o Usuario).
- **Código Relevante:**
```csharp
public VentanaLogin() : base("Inicio de Sesión")
{
    Button btnLogin = new Button("Iniciar Sesión");
    btnLogin.Clicked += (sender, e) =>
    {
        string correo = txtCorreo.Text;
        string contrasenia = txtContrasenia.Text;

        // Verificar si es el administrador
        if (correo == "admin@usac.com" && contrasenia == "admint123")
        {
            VentanaAdmin ventanaAdmin = new VentanaAdmin();
            ventanaAdmin.Show();
        }
        else
        {
            // Buscar al usuario en la lista de usuarios
            Usuario? usuario = DatosCompartidos.ListaUsuarios.BuscarUsuarioPorCredenciales(correo, contrasenia);
            if (usuario != null)
            {
                VentanaUsuarios ventanaUsuarios = new VentanaUsuarios();
                ventanaUsuarios.Show();
            }
        }
    };
}
```
### **3. VentanaAdmin**
- **Descripción:** Menú principal para el administrador.
- **Función:** Ofrece opciones como cargas masivas, gestión de entidades, actualización de repuestos, generación de servicios, control de logeo y generación de reportes.
- **Código Relevante:**
```csharp
public VentanaAdmin() : base("Menú Administrador")
{
    string[] opciones = {
        "Cargas Masivas",
        "Gestión de Entidades",
        "Actualización de Repuestos",
        "Visualización de Repuestos",
        "Generar Servicios",
        "Control de Logeo",
        "Generar Reportes"
    };

    foreach (string opcion in opciones)
    {
        Button boton = new Button(opcion);
        boton.Clicked += OnBotonClicked;
        contenedor.PackStart(boton, false, false, 0);
    }
}

private void OnBotonClicked(object sender, EventArgs e)
{
    Button boton = (Button)sender;
    switch (boton.Label)
    {
        case "Cargas Masivas":
            VentanaCargaMasiva ventanaCargaMasiva = new VentanaCargaMasiva();
            ventanaCargaMasiva.Show();
            break;
        case "Gestión de Entidades":
            VentanaGestionEntidades ventanaGestionEntidades = new VentanaGestionEntidades();
            ventanaGestionEntidades.Show();
            break;
        // Otros casos...
    }
}
```
### **4. VentanaUsuarios**
- **Descripción:** Menú principal para los usuarios registrados.
- **Función:** Permite insertar vehículos, visualizar servicios, visualizar facturas y cancelar facturas.
- **Código Relevante:**
```csharp
public VentanaUsuarios() : base("Menú de Usuario")
{
    string[] opciones = {
        "Insertar Vehículo",
        "Visualización de Servicios",
        "Visualización de Facturas",
        "Cancelar Facturas"
    };

    foreach (string opcion in opciones)
    {
        Button boton = new Button(opcion);
        boton.Clicked += OnBotonClicked;
        contenedor.PackStart(boton, false, false, 0);
    }
}

private void OnBotonClicked(object sender, EventArgs e)
{
    Button boton = (Button)sender;
    switch (boton.Label)
    {
        case "Insertar Vehículo":
            InsertarVehiculo ventanaInsertar = new InsertarVehiculo();
            ventanaInsertar.ShowAll();
            break;
        case "Visualización de Servicios":
            VisualizacionServicios ventanaServicios = new VisualizacionServicios();
            ventanaServicios.ShowAll();
            break;
        case "Visualización de Facturas":
            VisualizacionFacturas ventanaFacturas = new VisualizacionFacturas();
            ventanaFacturas.ShowAll();
            break;
        case "Cancelar Facturas":
            CancelarFacturas ventanaCancelar = new CancelarFacturas();
            ventanaCancelar.ShowAll();
            break;
    }
}
```
# ESTRUCTURAS DE DATOS

### **1. ListaSimpleUsuarios**
- **Descripción:** Lista enlazada para gestionar usuarios.
- **Métodos Clave:**
  - `AgregarUsuario`: Agrega un nuevo usuario.
  - `BuscarUsuarioPorCredenciales`: Busca un usuario por correo y contraseña.
  - `EliminarUsuario`: Elimina un usuario por ID.
- **Código Relevante:**
```csharp
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

public void EliminarUsuario(int id)
{
    if (cabeza == null)
    {
        Console.WriteLine("❌ Error: La lista está vacía.");
        return;
    }

    if (cabeza.Usuario.Id == id)
    {
        cabeza = cabeza.Siguiente;
        Console.WriteLine("✅ Usuario eliminado con éxito.");
        return;
    }

    NodoUsuario? actual = cabeza;
    while (actual.Siguiente != null && actual.Siguiente.Usuario.Id != id)
    {
        actual = actual.Siguiente;
    }

    if (actual.Siguiente == null)
    {
        Console.WriteLine("❌ Error: Usuario no encontrado.");
    }
    else
    {
        actual.Siguiente = actual.Siguiente.Siguiente;
        Console.WriteLine("✅ Usuario eliminado con éxito.");
    }
}
```
### **2. ListaDobleVehiculos**
- **Descripción:** Lista doblemente enlazada para gestionar vehículos.
- **Métodos Clave:**
  - `AgregarVehiculo`: Agrega un nuevo vehículo.
  - `EliminarVehiculo`: Elimina un vehículo por ID.
  - `MostrarVehiculos`: Muestra los vehículos en orden normal o inverso.
- **Código Relevante:**
```csharp
public void AgregarVehiculo(Vehiculo nuevoVehiculo)
{
    if (ExisteVehiculo(nuevoVehiculo.Id))
    {
        Console.WriteLine("❌ Error: El ID del vehículo ya existe en el sistema.");
        return;
    }

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

    Console.WriteLine("✅ Vehículo agregado con éxito.");
}

public void EliminarVehiculo(int id)
{
    if (cabeza == null)
    {
        Console.WriteLine("❌ Error: La lista está vacía.");
        return;
    }

    if (cabeza.Vehiculo.Id == id)
    {
        cabeza = cabeza.Siguiente;
        if (cabeza != null)
        {
            cabeza.Anterior = null;
        }
        else
        {
            cola = null;
        }
        Console.WriteLine("✅ Vehículo eliminado con éxito.");
        return;
    }

    NodoVehiculo? actual = cabeza;
    while (actual != null && actual.Vehiculo.Id != id)
    {
        actual = actual.Siguiente;
    }

    if (actual == null)
    {
        Console.WriteLine("❌ Error: Vehículo no encontrado.");
    }
    else
    {
        if (actual.Siguiente != null)
        {
            actual.Siguiente.Anterior = actual.Anterior;
        }
        else
        {
            cola = actual.Anterior;
        }

        if (actual.Anterior != null)
        {
            actual.Anterior.Siguiente = actual.Siguiente;
        }

        Console.WriteLine("✅ Vehículo eliminado con éxito.");
    }
}

public void MostrarVehiculos(bool enOrdenInverso = false)
{
    if (cabeza == null)
    {
        Console.WriteLine("❌ La lista está vacía.");
        return;
    }

    if (enOrdenInverso)
    {
        NodoVehiculo? actual = cola;
        while (actual != null)
        {
            Console.WriteLine(actual.Vehiculo);
            actual = actual.Anterior;
        }
    }
    else
    {
        NodoVehiculo? actual = cabeza;
        while (actual != null)
        {
            Console.WriteLine(actual.Vehiculo);
            actual = actual.Siguiente;
        }
    }
}
```
### **3. ArbolAVLRepuestos**
- **Descripción:** Árbol AVL para gestionar repuestos.
- **Métodos Clave:**
  - `Insertar`: Inserta un nuevo repuesto.
  - `Buscar`: Busca un repuesto por ID.
  - `RecorrerInOrden`: Recorre el árbol en orden.
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

    if (repuesto.Id < nodo.Repuesto.Id)
        nodo.Izquierdo = InsertarNodo(nodo.Izquierdo, repuesto);
    else if (repuesto.Id > nodo.Repuesto.Id)
        nodo.Derecho = InsertarNodo(nodo.Derecho, repuesto);
    else
        throw new Exception("❌ Error: El ID del repuesto ya existe en el sistema.");

    nodo.Altura = 1 + Math.Max(ObtenerAltura(nodo.Izquierdo), ObtenerAltura(nodo.Derecho));

    int balance = ObtenerFactorBalance(nodo);

    // Rotaciones para balancear el árbol
    if (balance > 1 && repuesto.Id < nodo.Izquierdo!.Repuesto.Id)
        return RotacionDerecha(nodo);

    if (balance < -1 && repuesto.Id > nodo.Derecho!.Repuesto.Id)
        return RotacionIzquierda(nodo);

    if (balance > 1 && repuesto.Id > nodo.Izquierdo!.Repuesto.Id)
    {
        nodo.Izquierdo = RotacionIzquierda(nodo.Izquierdo);
        return RotacionDerecha(nodo);
    }

    if (balance < -1 && repuesto.Id < nodo.Derecho!.Repuesto.Id)
    {
        nodo.Derecho = RotacionDerecha(nodo.Derecho);
        return RotacionIzquierda(nodo);
    }

    return nodo;
}

public Repuesto? Buscar(int id)
{
    NodoAVL? actual = raiz;
    while (actual != null)
    {
        if (id == actual.Repuesto.Id)
            return actual.Repuesto;
        else if (id < actual.Repuesto.Id)
            actual = actual.Izquierdo;
        else
            actual = actual.Derecho;
    }
    return null;
}

public void RecorrerInOrden()
{
    RecorrerInOrden(raiz);
}

private void RecorrerInOrden(NodoAVL? nodo)
{
    if (nodo == null)
        return;

    RecorrerInOrden(nodo.Izquierdo);
    Console.WriteLine(nodo.Repuesto);
    RecorrerInOrden(nodo.Derecho);
}
```
### **4. ArbolBinarioServicios**
- **Descripción:** Árbol binario para gestionar servicios.
- **Métodos Clave:**
  - `Insertar`: Inserta un nuevo servicio.
  - `Buscar`: Busca un servicio por ID.
  - `MostrarServicios`: Muestra los servicios en orden.
- **Código Relevante:**
```csharp
public void Insertar(Servicio servicio, ArbolAVLRepuestos arbolRepuestos, ListaDobleVehiculos listaVehiculos)
{
    if (arbolRepuestos.Buscar(servicio.Id_Repuesto) == null)
    {
        throw new Exception($"❌ Error: El repuesto con ID {servicio.Id_Repuesto} no existe.");
    }

    if (listaVehiculos.BuscarVehiculo(servicio.Id_Vehiculo) == null)
    {
        throw new Exception($"❌ Error: El vehículo con ID {servicio.Id_Vehiculo} no existe.");
    }

    raiz = InsertarNodo(raiz, servicio);
}

private NodoBinario InsertarNodo(NodoBinario? nodo, Servicio servicio)
{
    if (nodo == null)
        return new NodoBinario(servicio);

    if (servicio.Id < nodo.Servicio.Id)
        nodo.Izquierdo = InsertarNodo(nodo.Izquierdo, servicio);
    else if (servicio.Id > nodo.Servicio.Id)
        nodo.Derecho = InsertarNodo(nodo.Derecho, servicio);
    else
        throw new Exception("❌ Error: El ID del servicio ya existe en el sistema.");

    return nodo;
}

public Servicio? Buscar(int id)
{
    NodoBinario? actual = raiz;
    while (actual != null)
    {
        if (id == actual.Servicio.Id)
            return actual.Servicio;
        else if (id < actual.Servicio.Id)
            actual = actual.Izquierdo;
        else
            actual = actual.Derecho;
    }
    return null;
}

public void MostrarServicios()
{
    MostrarServiciosEnOrden(raiz);
}

private void MostrarServiciosEnOrden(NodoBinario? nodo)
{
    if (nodo == null)
        return;

    MostrarServiciosEnOrden(nodo.Izquierdo);
    Console.WriteLine(nodo.Servicio);
    MostrarServiciosEnOrden(nodo.Derecho);
}
```
### **5. ArbolBFacturas**
- **Descripción:** Árbol B para gestionar facturas.
- **Métodos Clave:**
  - `Insertar`: Inserta una nueva factura.
  - `Eliminar`: Elimina una factura por ID.
  - `MostrarFacturas`: Muestra las facturas en orden.
- **Código Relevante:**
```csharp
public void Insertar(Factura factura)
{
    if (raiz == null)
    {
        raiz = new NodoB(true);
        raiz.Claves.Add(factura);
    }
    else
    {
        if (raiz.Claves.Count == Orden - 1)
        {
            NodoB nuevaRaiz = new NodoB(false);
            nuevaRaiz.Hijos.Add(raiz);
            DividirNodo(nuevaRaiz, 0, raiz);
            raiz = nuevaRaiz;
        }

        InsertarEnNodo(raiz, factura);
    }
}

private void InsertarEnNodo(NodoB nodo, Factura factura)
{
    int i = nodo.Claves.Count - 1;

    if (nodo.EsHoja)
    {
        while (i >= 0 && factura.Id < nodo.Claves[i].Id)
        {
            i--;
        }
        nodo.Claves.Insert(i + 1, factura);
    }
    else
    {
        while (i >= 0 && factura.Id < nodo.Claves[i].Id)
        {
            i--;
        }
        i++;
        if (nodo.Hijos[i].Claves.Count == Orden - 1)
        {
            DividirNodo(nodo, i, nodo.Hijos[i]);
            if (factura.Id > nodo.Claves[i].Id)
            {
                i++;
            }
        }
        InsertarEnNodo(nodo.Hijos[i], factura);
    }
}

public void Eliminar(int id)
{
    if (raiz == null)
    {
        Console.WriteLine("El árbol está vacío.");
        return;
    }

    EliminarEnNodo(raiz, id);

    if (raiz.Claves.Count == 0)
    {
        if (raiz.EsHoja)
        {
            raiz = null;
        }
        else
        {
            raiz = raiz.Hijos[0];
        }
    }
}

private void EliminarEnNodo(NodoB nodo, int id)
{
    int idx = 0;
    while (idx < nodo.Claves.Count && nodo.Claves[idx].Id < id)
    {
        idx++;
    }

    if (idx < nodo.Claves.Count && nodo.Claves[idx].Id == id)
    {
        if (nodo.EsHoja)
        {
            nodo.Claves.RemoveAt(idx);
        }
        else
        {
            EliminarClaveNoHoja(nodo, idx);
        }
    }
    else
    {
        if (nodo.EsHoja)
        {
            Console.WriteLine("La clave no está en el árbol.");
            return;
        }

        bool ultimoHijo = (idx == nodo.Claves.Count);
        if (nodo.Hijos[idx].Claves.Count < (Orden - 1) / 2)
        {
            LlenarNodo(nodo, idx);
        }

        if (ultimoHijo && idx > nodo.Claves.Count)
        {
            EliminarEnNodo(nodo.Hijos[idx - 1], id);
        }
        else
        {
            EliminarEnNodo(nodo.Hijos[idx], id);
        }
    }
}

public void MostrarFacturas()
{
    MostrarFacturasEnOrden(raiz);
}

private void MostrarFacturasEnOrden(NodoB? nodo)
{
    if (nodo == null)
        return;

    for (int i = 0; i < nodo.Claves.Count; i++)
    {
        MostrarFacturasEnOrden(nodo.Hijos[i]);
        Console.WriteLine(nodo.Claves[i]);
    }
    MostrarFacturasEnOrden(nodo.Hijos[nodo.Claves.Count]);
}