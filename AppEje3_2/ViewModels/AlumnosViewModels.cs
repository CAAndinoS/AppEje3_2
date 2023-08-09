using AppEje3_2.Controllers;
using AppEje3_2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppEje3_2.ViewModels
{
    public class AlumnosViewModels : BaseViewModels
    {

        public async void ActualizarAlumno(Alumnos alumno)
        {
            // Verifica si el alumno no es nulo
            if (alumno != null)
            {
                // Llamar al método UpdateAlumn en la clase DBProc para guardar los cambios en la base de datos
                await database.AddAlumn(alumno);

                // Recargar la lista de alumnos después de actualizar uno.
                LoadAlumnosList();
            }
        }

        private bool isListViewVisible = true;
        public bool IsListViewVisible
        {
            get => isListViewVisible;
            set => SetProperty(ref isListViewVisible, value);
        }

        private DBProc database;
        private List<Alumnos> alumnosList;

        public AlumnosViewModels(string dbPath)
        {
            AlumnAdd = new Command(() => _AlumnAdd());
            AlumnDelete = new Command<Alumnos>(async (alumno) => await _AlumnDelete(alumno));

            AlumnUpdate = new Command(() => _AlumnUpdate());
            AlumnEdit = new Command(() => _AlumnEdit());
            AlumnList = new Command(() => _AlumnList());
            database = new DBProc(dbPath); // Inicializa el objeto database con el dbPath proporcionado
                                           // Cargar la lista de alumnos al inicio si es necesario.
            LoadAlumnosList();
        }

        public int _id;
        public string _nombres;
        public string _apellidos;
        public string _sexo;
        public string _direccion;
        public string _foto;

        public int id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        public string nombres
        {
            get { return _nombres; }
            set { _nombres = value; OnPropertyChanged(); }
        }

        public string apellidos
        {
            get { return _apellidos; }
            set { _apellidos = value; OnPropertyChanged(); }
        }

        public string sexo
        {
            get { return _sexo; }
            set { _sexo = value; OnPropertyChanged(); }
        }

        public string direccion
        {
            get { return _direccion; }
            set { _direccion = value; OnPropertyChanged(); }
        }

        public string foto
        {
            get { return _foto; }
            set { _foto = value; OnPropertyChanged(); }
        }

        // Lista de alumnos para mostrar en la interfaz de usuario.
        public List<Alumnos> AlumnosList
        {
            get { return alumnosList; }
            set { alumnosList = value; OnPropertyChanged(); }
        }
        public ICommand AlumnAdd { get; set; }
        public ICommand AlumnDelete { get; set; }
        public ICommand AlumnUpdate { get; set; }
        public ICommand AlumnEdit { get; set; }
        public ICommand AlumnList { get; set; }

        public async void _AlumnAdd()
        {
            //SQlite
            //REstApi
            Alumnos alumno = new Alumnos
            {
                nombres = _nombres,
                apellidos = _apellidos,
                sexo = _sexo,
                direccion = _direccion,
                foto = _foto
            };

            await database.AddAlumn(alumno);       
            // Recargar la lista de alumnos después de agregar uno nuevo.
            LoadAlumnosList();

        }

        public async Task _AlumnDelete(Alumnos alumno)
        {
            //SQlite
            //REstApi
            if (alumno != null)
            {
                await database.DeleteAlumn(alumno);
                // Recargar la lista de alumnos después de eliminar uno.
                await LoadAlumnosList();
            }
        }

        public async void _AlumnUpdate()
        {
            // Obtener el alumno seleccionado de la base de datos por su ID
            Alumnos alumno = await database.GetById(_id);

            // Actualizar las propiedades del alumno con los valores actuales del ViewModel
            alumno.nombres = _nombres;
            alumno.apellidos = _apellidos;
            alumno.sexo = _sexo;
            alumno.direccion = _direccion;
            alumno.foto = _foto;

            // Llamar al método UpdateAlumn en la clase DBProc para guardar los cambios en la base de datos
            await database.AddAlumn(alumno);

            // Recargar la lista de alumnos después de actualizar uno.
            LoadAlumnosList();
        }


        public async void _AlumnEdit()
        {
            //SQlite
            //REstApi
            Alumnos alumno = await database.GetById(_id);
            if (alumno != null)
            {
                _nombres = alumno.nombres;
                _apellidos = alumno.apellidos;
                _sexo = alumno.sexo;
                _direccion = alumno.direccion;
                _foto = alumno.foto;
            }
        }

        public void _AlumnList()
        {
            //SQlite
            //REstApi
        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        public bool IsDataAvailable => AlumnosList?.Count > 0;

        private async Task LoadAlumnosList()
        {
            IsLoading = true; // Mostrar el ActivityIndicator

            var alumnos = await database.GetAll();
            AlumnosList = new List<Alumnos>(alumnos);

            IsLoading = false; // Ocultar el ActivityIndicator
        }


        public AlumnosViewModels()
        {
            AlumnAdd = new Command(() => _AlumnAdd());
            //AlumnDelete = new Command(() => _AlumnDelete());
            AlumnUpdate = new Command(() => _AlumnUpdate());
            AlumnEdit = new Command(() => _AlumnEdit());
            AlumnList = new Command(() => _AlumnList());
        }
    }
}
