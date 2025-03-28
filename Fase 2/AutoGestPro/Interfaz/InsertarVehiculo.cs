using System;
using Gtk;
using AutoGestPro.Estructuras;
using AutoGestPro.Modelos;

public class InsertarVehiculo : Window
{
    private Entry idEntry;
    private Entry idUsuarioEntry;
    private Entry marcaEntry;
    private Entry modeloEntry;
    private Entry placaEntry;
    private Button guardarButton;

    private ListaDobleVehiculos listaVehiculos;
    private ListaSimpleUsuarios listaUsuarios;

    public InsertarVehiculo(ListaDobleVehiculos listaVehiculos, ListaSimpleUsuarios listaUsuarios) : base("Registrar Vehículo")
{
    this.listaVehiculos = listaVehiculos;
    this.listaUsuarios = listaUsuarios;

    // Configuración de la ventana
    SetDefaultSize(400, 300);
    SetPosition(WindowPosition.Center);
    DeleteEvent += OnDeleteEvent;

    // Crear el contenedor principal
    VBox mainBox = new VBox(false, 10);
    mainBox.BorderWidth = 20;

    // Crear la tabla para el formulario
    Table formTable = new Table(5, 2, false);
    formTable.ColumnSpacing = 10;
    formTable.RowSpacing = 10;

    // ID del vehículo
    Label idLabel = new Label("ID del Vehículo:");
    idEntry = new Entry();
    formTable.Attach(idLabel, 0, 1, 0, 1);
    formTable.Attach(idEntry, 1, 2, 0, 1);

    // ID del usuario propietario
    Label idUsuarioLabel = new Label("ID del Usuario:");
    idUsuarioEntry = new Entry();
    formTable.Attach(idUsuarioLabel, 0, 1, 1, 2);
    formTable.Attach(idUsuarioEntry, 1, 2, 1, 2);

    // Marca
    Label marcaLabel = new Label("Marca:");
    marcaEntry = new Entry();
    formTable.Attach(marcaLabel, 0, 1, 2, 3);
    formTable.Attach(marcaEntry, 1, 2, 2, 3);

    // Modelo
    Label modeloLabel = new Label("Modelo:");
    modeloEntry = new Entry();
    formTable.Attach(modeloLabel, 0, 1, 3, 4);
    formTable.Attach(modeloEntry, 1, 2, 3, 4);

    // Placa
    Label placaLabel = new Label("Placa:");
    placaEntry = new Entry();
    formTable.Attach(placaLabel, 0, 1, 4, 5);
    formTable.Attach(placaEntry, 1, 2, 4, 5);

    // Botón Guardar
    guardarButton = new Button("Guardar");
    guardarButton.Clicked += OnGuardarClicked;

    // Agregar componentes al contenedor principal
    mainBox.PackStart(formTable, false, false, 0);
    mainBox.PackStart(guardarButton, false, false, 0);

    // Agregar el contenedor principal a la ventana
    Add(mainBox);
    ShowAll();
}

private void OnGuardarClicked(object sender, EventArgs e)
{
    // Validar los campos
    if (string.IsNullOrWhiteSpace(idEntry.Text) ||
        string.IsNullOrWhiteSpace(idUsuarioEntry.Text) ||
        string.IsNullOrWhiteSpace(marcaEntry.Text) ||
        string.IsNullOrWhiteSpace(modeloEntry.Text) ||
        string.IsNullOrWhiteSpace(placaEntry.Text))
    {
        MostrarMensaje("Por favor, complete todos los campos.", MessageType.Error);
        return;
    }

    try
    {
        // Crear el objeto Vehículo
        int id = int.Parse(idEntry.Text);
        int idUsuario = int.Parse(idUsuarioEntry.Text);

        // Verificar si el ID del usuario existe
        if (listaUsuarios.BuscarUsuario(idUsuario) == null)
        {
            MostrarMensaje("El ID del usuario no existe. No se puede registrar el vehículo.", MessageType.Error);
            return;
        }

        // Verificar si el ID del vehículo ya está registrado
        if (listaVehiculos.BuscarVehiculo(id) != null)
        {
            MostrarMensaje("El ID del vehículo ya está registrado. Use un ID diferente.", MessageType.Error);
            return;
        }

        string marca = marcaEntry.Text;
        int modelo = int.Parse(modeloEntry.Text);
        string placa = placaEntry.Text;

        Vehiculo nuevoVehiculo = new Vehiculo(id, idUsuario, marca, modelo, placa);

        // Agregar el vehículo a la lista doble
        listaVehiculos.AgregarVehiculo(nuevoVehiculo);

        MostrarMensaje("Vehículo registrado exitosamente.", MessageType.Info);

        // Limpiar los campos
        LimpiarCampos();
    }
    catch (Exception ex)
    {
        MostrarMensaje($"Error: {ex.Message}", MessageType.Error);
    }
}

    private void MostrarMensaje(string mensaje, MessageType tipo)
    {
        MessageDialog dialog = new MessageDialog(
            this,
            DialogFlags.Modal,
            tipo,
            ButtonsType.Ok,
            mensaje
        );
        dialog.Run();
        dialog.Destroy();
    }

    private void LimpiarCampos()
    {
        idEntry.Text = string.Empty;
        idUsuarioEntry.Text = string.Empty;
        marcaEntry.Text = string.Empty;
        modeloEntry.Text = string.Empty;
        placaEntry.Text = string.Empty;
    }

    private void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        // Cerrar solo la ventana actual
        Destroy();
        a.RetVal = true;
    }
}