using AppEje3_2.Models;
using AppEje3_2.ViewModels;
using Plugin.Media;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEje3_2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageActualizar : ContentPage
    {
        Plugin.Media.Abstractions.MediaFile photo = null;
        private AlumnosViewModels viewModel;
        public PageActualizar(Alumnos alumno)
        {
            InitializeComponent();
            // Crear el ViewModel con el mismo dbPath que se utiliza en la página anterior
            viewModel = new AlumnosViewModels(App.DbFullPath);
            BindingContext = alumno;

            
                // Convertir la imagen en Base64 a un objeto ImageSource
                if (!string.IsNullOrEmpty(alumno.foto))
                {
                    byte[] imageBytes = Convert.FromBase64String(alumno.foto);
                    ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    foto.Source = imageSource;
                }
            
        }
        private string traeImagenToBase64()
        {
            if (photo != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = photo.GetStream();
                    stream.CopyTo(memory);
                    byte[] fotobyte = memory.ToArray();

                    byte[] imagenescalada = obtener_imagen_escalada(fotobyte, 50, 500, 500); // Ajusta los valores de ancho y alto según tus necesidades

                    string base64String = Convert.ToBase64String(imagenescalada);
                    return base64String;
                }
            }
            return null;
        }

        private byte[] obtener_imagen_escalada(byte[] imagen, int compresion, int nuevoAncho, int nuevoAlto)
        {
            using (SKBitmap originalBitmap = SKBitmap.Decode(imagen))
            {
                SKImageInfo info = new SKImageInfo(nuevoAncho, nuevoAlto);
                using (SKBitmap scaledBitmap = originalBitmap.Resize(info, SKFilterQuality.High))
                {
                    using (SKData compressedData = scaledBitmap.Encode(SKEncodedImageFormat.Jpeg, compresion))
                    {
                        return compressedData.ToArray();
                    }
                }
            }
        }
        private void btnfoto_Clicked(object sender, EventArgs e)
        {

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            // Obtener el alumno seleccionado del BindingContext
            Alumnos alumno = (Alumnos)BindingContext;

            // Actualizar las propiedades del alumno con los valores actuales de los controles
            alumno.nombres = nom.Text;
            alumno.apellidos = ape.Text;
            alumno.sexo = sex.Text;
            alumno.direccion = dire.Text;
            // Actualizar la foto si es necesario (depende de cómo manejes la lógica para seleccionar la foto)
            if (photo != null)
            {
                foto.Source = ImageSource.FromStream(() =>
                {
                    var stream = photo.GetStream();
                    return stream;
                });

                // Set the foto property in the view model to the Base64 string
                alumno.foto = traeImagenToBase64();
            }
            else
            {
                alumno.foto = alumno.foto;
            }
            // Llamar al método de actualización en el ViewModel
            viewModel.ActualizarAlumno(alumno);

            // Volver a la página anterior
            await Navigation.PopAsync();
        }
    }
}