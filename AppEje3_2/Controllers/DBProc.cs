using AppEje3_2.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppEje3_2.Controllers
{
    public class DBProc
    {
        readonly SQLiteAsyncConnection _connection;
        public DBProc() { }
        public DBProc(string dbpath)
        {
            _connection = new SQLiteAsyncConnection(dbpath);
            /*Crear todos mis objetos de base de datos: tablas*/
            _connection.CreateTableAsync<Alumnos>().Wait();
        }
        /*Crear el CRUD de BD*/
        //Create
        public Task<int> AddAlumn(Alumnos alumnos)
        {
            if (alumnos.id == 0)
            {
                return _connection.InsertAsync(alumnos);
            }
            else
            {
                return _connection.UpdateAsync(alumnos);
            }
        }

        //Read
        public Task<List<Alumnos>> GetAll()
        {
            return _connection.Table<Alumnos>().ToListAsync();
        }

        public Task<Alumnos> GetById(int id)
        {
            return _connection.Table<Alumnos>()
                .Where(i => i.id == id)
                .FirstOrDefaultAsync();
        }

        //Delete
        public Task<int> DeleteAlumn(Alumnos alumnos)
        {
            return _connection.DeleteAsync(alumnos);
        }
    }
}
