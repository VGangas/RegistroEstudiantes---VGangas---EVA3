using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;

namespace RegistroEstudiantes.Vistas;

public partial class EstudianteCrear : ContentPage
{

    FirebaseClient client = new FirebaseClient("https://primer-proyecto-279cd-default-rtdb.firebaseio.com/");

    public List<Grado> Grado {  get; set; }

    public EstudianteCrear()
    {
        InitializeComponent();
        ListarGrados();
        BindingContext = this;
    }

    private async void ListarGrados()
    {
        try
        {
            var grados = await client.Child("Grado").OnceAsync<Grado>();
            Grado = grados.Select(x => x.Object).ToList();
            gradoPicker.ItemsSource = Grado;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar los grados: {ex.Message}", "OK");
        }
    }

    private async void guardarButton_Clicked(object sender, EventArgs e)
    {
        Grado grado = gradoPicker.SelectedItem as Grado;

        int selectedIndex = estadoPicker.SelectedIndex;
        bool estado = selectedIndex == 0;

        var estudiante = new Estudiante
        {
            PrimerNombre = primerNombreEntry.Text,
            SegundoNombre = segundoNombreEntry.Text,
            PrimerApellido = primerApellidoEntry.Text,
            SegundoApellido = segundoApellidoEntry.Text,
            Email = emailEntry.Text,
            Edad = int.Parse(edadEntry.Text),
            Grado = grado,
            Estado = estado
        };

        await client.Child("Estudiante").PostAsync(estudiante);

        await DisplayAlert("Exito", $"El estudiante {estudiante.PrimerNombre} {estudiante.SegundoApellido} fue guardado correctamente", "OK");
        await Navigation.PopAsync();
    }
}