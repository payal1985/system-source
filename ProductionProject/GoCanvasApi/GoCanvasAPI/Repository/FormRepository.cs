using GoCanvasAPI.DBContext;
using GoCanvasAPI.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoCanvasAPI.Repository
{
    public class FormRepository : IFormRepository
    {
        GoCanvasContext _dbContext;
        public FormRepository(GoCanvasContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public async Task InsertForm(FormModel form)
        //{
        //    try
        //    {
        //        var entity = _dbContext.FormModels.FirstOrDefault(item => item.FormId == form.FormId);

        //        // Validate entity is not null
        //        if (entity != null)
        //        {
        //            _dbContext.FormModels.Update(entity);

        //        }
        //        else
        //        {
        //            await _dbContext.AddAsync(form);
        //        }
        //        await _dbContext.SaveChangesAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


        public async Task InsertForm(List<FormModel> form)
        {
            try
            {
                var entity = _dbContext.FormModels.Where(item => form.Select(x=>x.FormId).Contains(item.FormId)).ToList();

                // var notentity = _dbContext.FormModels.Except(entity).ToList();

                //var notentity = _dbContext.FormModels.Where(x => !form.Select(y => y.FormId).Contains(x.FormId)).ToList();
                List<FormModel> notentity = new List<FormModel>();

                if (entity != null && entity.Count > 0)
                {
                    notentity = form.Where(frm => entity.All(e => e.FormId != frm.FormId)).ToList();
                }

                // Validate entity is not null
                if (entity != null && entity.Count > 0 && notentity.Count == 0)
                {
                    _dbContext.UpdateRange(entity);
                    _dbContext.SaveChanges();
                }
                else if(notentity != null && notentity.Count > 0)
                {
                    await _dbContext.AddRangeAsync(notentity);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    await _dbContext.AddRangeAsync(form);
                    await _dbContext.SaveChangesAsync();
                }
               
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
