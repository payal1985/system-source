using GoCanvasAPI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI.Repository
{
    public interface IFormRepository
    {
        //Task InsertForm(FormModel _object);
        Task InsertForm(List<FormModel> form);
       // public void Update(FormModel _object);

        //public IEnumerable<FormModel> GetAll();

        //public T GetById(int Id);

        //public void Delete(T _object);
    }
}
