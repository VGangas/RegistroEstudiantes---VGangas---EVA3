
using RegistroEstudiantes.Vistas;

namespace RegistroEstudiantes
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ListarEstudiantes());
        }
    }
}
