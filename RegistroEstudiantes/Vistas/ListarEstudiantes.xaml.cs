using System.Collections.ObjectModel;
using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;

namespace RegistroEstudiantes.Vistas;

public partial class ListarEstudiantes : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://primer-proyecto-279cd-default-rtdb.firebaseio.com/");
    public ObservableCollection<Estudiante> Lista { get; set; } = new ObservableCollection<Estudiante>();

    public ListarEstudiantes()
    {
        InitializeComponent();
        BindingContext = this;
        CargarLista();
    }

    private async void CargarLista()
    {
        try
        {
            Lista.Clear(); // Limpia la lista antes de recargarla

            // Cargar los estudiantes desde Firebase
            var estudiantes = await client.Child("Estudiantes").OnceAsync<Estudiante>();
            foreach (var estudiante in estudiantes)
            {
                // Asignar la clave como Id al modelo
                estudiante.Object.Id = estudiante.Key;
                Lista.Add(estudiante.Object);
            }

            // Suscribirse a cambios en tiempo real
            client.Child("Estudiante").AsObservable<Estudiante>().Subscribe(estudiante =>
            {
                if (estudiante.Object != null && !Lista.Any(e => e.Id == estudiante.Key))
                {
                    estudiante.Object.Id = estudiante.Key; // Asignar la clave como Id
                    Lista.Add(estudiante.Object);
                }
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al cargar la lista: {ex.Message}", "OK");
        }
    }

    private void FilterSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filter = filterSearchBar?.Text?.ToLower() ?? string.Empty;

        if (!string.IsNullOrEmpty(filter))
        {
            CollectionList.ItemsSource = Lista.Where(x => x.NombreCompleto.ToLower().Contains(filter));
        }
        else
        {
            CollectionList.ItemsSource = Lista;
        }
    }

    private async void NuevoEstudianteBoton_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new EstudianteCrear());
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void EditarEstudiante_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var estudiante = boton?.CommandParameter as Estudiante;

        if (estudiante != null && !string.IsNullOrEmpty(estudiante.Id))
        {
            await Navigation.PushAsync(new EditarEstudiante(estudiante.Id));
        }
        else
        {
            await DisplayAlert("Error", "No se pudo obtener la información del empleado", "OK");
        }
    }
}
