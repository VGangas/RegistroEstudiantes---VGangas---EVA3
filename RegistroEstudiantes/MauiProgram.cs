using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;
using RegistroEstudiantes.Modelos.Modelos;

namespace RegistroEstudiantes
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            Registro();
            return builder.Build();
        }
        public static void Registro()
        {
            FirebaseClient client = new FirebaseClient("https://primer-proyecto-279cd-default-rtdb.firebaseio.com/");

            var grados = client.Child("Grado").OnceAsync<Grado>();
            if (grados.Result.Count == 0 ) 
            {
                client.Child("Grado").PostAsync(new Grado { Nombre = "1ro Basico" });
                client.Child("Grado").PostAsync(new Grado { Nombre = "2ro Basico" });
                client.Child("Grado").PostAsync(new Grado { Nombre = "3ro Basico" });
                client.Child("Grado").PostAsync(new Grado { Nombre = "4to Basico" });
                client.Child("Grado").PostAsync(new Grado { Nombre = "5to Basico" });
                client.Child("Grado").PostAsync(new Grado { Nombre = "6to Basico" });
                client.Child("Grado").PostAsync(new Grado { Nombre = "7mo Basico" });
                client.Child("Grado").PostAsync(new Grado { Nombre = "8vo Basico" });
                client.Child("Grado").PostAsync(new Grado { Nombre = "1 Medio" });
                client.Child("Grado").PostAsync(new Grado { Nombre = "2 Medio" });
                client.Child("Grado").PostAsync(new Grado { Nombre = "3 Medio" });
                client.Child("Grado").PostAsync(new Grado { Nombre = "4 Medio" });
            }
        }
    }
}
