using System.Collections.ObjectModel;
using Firebase.Database;
using RegistroEstudiantes.Modelos.Modelos;

namespace RegistroEstudiantes.Vistas;

public partial class ListarEstudiantes : ContentPage

{

    FirebaseClient client = new FirebaseClient("https://primer-proyecto-279cd-default-rtdb.firebaseio.com/");
	public ObservableCollection<Estudiante> Lista {  get; set; } = new ObservableCollection<Estudiante>();
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
            var estudiantes = await client.Child("Estudiantes").OnceAsync<Estudiante>();
            foreach (var estudiante in estudiantes)
            {
                Lista.Add(estudiante.Object);
            }

            client.Child("Estudiante").AsObservable<Estudiante>().Subscribe(estudiante =>
            {
                if (estudiante.Object != null && !Lista.Any(e => e.Id == estudiante.Key))
                {
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

}