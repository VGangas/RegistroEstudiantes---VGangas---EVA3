using System.Collections.ObjectModel;
using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;
using System.Collections.ObjectModel;
using System.Security;
using static System.Net.Mime.MediaTypeNames;

namespace RegistroEstudiantes.Vistas;

public partial class EditarEstudiante : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://studentmanager-ac31a-default-rtdb.firebaseio.com/");
    public List<Grado> Grados { get; set; }
    public ObservableCollection<string> ListaGrados { get; set; } = new ObservableCollection<string>();
    private Estudiante estudianteActualizado = new Estudiante();
    private string estudianteId;
    public EditarEstudiante (string idEstudiante)
    {
        InitializeComponent();
        BindingContext = this;
        estudianteId = idEstudiante;
        CargarDatosEstudiante(estudianteId);
        CargarListaGrados();
    }

    private async void CargarListaGrados()
    {
        try
        {
            var grados = await client.Child("Grado").OnceAsync<Grado>();
            ListaGrados.Clear();
            foreach (var grado in grados)
            {
                ListaGrados.Add(grado.Object.Nombre);
            }

            EditarGradoPicker.ItemsSource = ListaGrados;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Error:" + ex.Message, "Ok");
        }
    }




private async void CargarDatosEstudiante(string idEstudiante)
{
    try
    {
        var estudiante = await client.Child("Estudiante").Child(idEstudiante).OnceSingleAsync<Estudiante>();
        if (estudiante != null)
        {
            EditarNombreEntry.Text = estudiante.PrimerNombre;
            EditarSegundoNombreEntry.Text = estudiante.SegundoNombre;
            EditarApellidoEntry.Text = estudiante.PrimerApellido;
            EditarSegundoApellidoEntry.Text = estudiante.SegundoApellido;
            EditarCorreoEntry.Text = estudiante.Email;
            EditarEdadEntry.Text = estudiante.Edad.ToString();

            EditarGradoPicker.SelectedItem = estudiante.Grado;
        }
        else
        {
            await DisplayAlert("Error", "No se encontró el estudiante.", "OK");
        }
    }
    catch (Exception ex)
    {
        await DisplayAlert("Error", "Error al cargar los datos del estudiante: " + ex.Message, "OK");
    }
}


    private async void ActualizarBoton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EditarNombreEntry.Text) ||
                string.IsNullOrWhiteSpace(EditarSegundoNombreEntry.Text) ||
                string.IsNullOrWhiteSpace(EditarApellidoEntry.Text) ||
                string.IsNullOrWhiteSpace(EditarSegundoApellidoEntry.Text) ||
                string.IsNullOrWhiteSpace(EditarCorreoEntry.Text) ||
                EditarGradoPicker.SelectedItem == null)
            {

                await DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
                return;
            }

            if (!EditarCorreoEntry.Text.Contains("@"))
            {
                await DisplayAlert("Error", "El correo electrónico no es válido", "OK");
                return;
            }

            estudianteActualizado.Id = estudianteId;
            estudianteActualizado.PrimerNombre = EditarNombreEntry.Text.Trim();
            estudianteActualizado.SegundoNombre = EditarSegundoNombreEntry.Text.Trim();
            estudianteActualizado.PrimerApellido = EditarApellidoEntry.Text.Trim();
            estudianteActualizado.SegundoApellido = EditarSegundoApellidoEntry.Text.Trim();
            estudianteActualizado.Email = EditarCorreoEntry.Text.Trim();
            estudianteActualizado.Grado = new Grado { Nombre = EditarGradoPicker.SelectedItem.ToString() };

            await client.Child("Estudiante").Child(estudianteActualizado.Id).PutAsync(estudianteActualizado);

            await DisplayAlert("Éxito", "El empleado se ha actualizado correctamente", "OK");
            await Navigation.PopAsync();

        }
        catch (Exception ex)
        {

            await DisplayAlert("Error", "Error" + ex.Message, "OK");
        }
    }
}