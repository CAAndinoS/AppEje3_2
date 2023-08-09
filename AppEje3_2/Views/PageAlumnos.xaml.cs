using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AppEje3_2.ViewModels;
using Plugin.Media;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEje3_2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAlumnos : ContentPage
    {
        Plugin.Media.Abstractions.MediaFile photo = null;
        private string dbfulp;
        public PageAlumnos()
        {
            InitializeComponent();
            dbfulp = App.DbFullPath;

            BindingContext = new AlumnosViewModels(dbfulp);
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

        private async void btnfoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Elige Una Opcion",
                "Cancelar",
                null,
                "Galeria",
                "Camara");

            if (source == "Cancelar")
            {
                photo = null;
                return;
            }
            if (source == "Camara")
            {
                photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "FotosAplicacion",
                    Name = "PhotoAlbum.jpg",
                    SaveToAlbum = true
                });
            }
            else
            {
                photo = await CrossMedia.Current.PickPhotoAsync();
            }

            if (photo != null)
            {
                foto.Source = ImageSource.FromStream(() =>
                {
                    var stream = photo.GetStream();
                    return stream;
                });

                // Set the foto property in the view model to the Base64 string
                ((AlumnosViewModels)BindingContext).foto = traeImagenToBase64();
            }
        }

        private async void Buttolista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PageList());
        }
    }
}