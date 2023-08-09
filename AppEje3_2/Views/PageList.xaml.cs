using AppEje3_2.Models;
using AppEje3_2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEje3_2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageList : ContentPage
    {
        public PageList()
        {
            InitializeComponent();
            BindingContext = new AlumnosViewModels(App.DbFullPath);
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Alumnos alumno)
            {
                bool result = await DisplayAlert("Borrar o Actualizar", "¿Qué acción deseas realizar?", "Borrar", "Actualizar");

                if (result)
                {
                    // Se confirma el borrado, llama al comando AlumnDelete con el alumno a borrar.
                    ((AlumnosViewModels)BindingContext).AlumnDelete.Execute(alumno);
                }
                else
                {
                    // Se selecciona la opción de Actualizar, navega a la pantalla de actualización pasando el objeto alumno como parámetro.
                    await Navigation.PushAsync(new PageActualizar(alumno));
                }

                // Deselecciona el elemento del ListView.
                ((ListView)sender).SelectedItem = null;
            }
        }

    }
}