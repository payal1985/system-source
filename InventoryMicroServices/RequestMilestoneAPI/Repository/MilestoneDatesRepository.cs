using RequestMilestoneAPI.DBContext;
using RequestMilestoneAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RequestMilestoneAPI.DBModels;

namespace RequestMilestoneAPI.Repository
{
    public class MilestoneDatesRepository : IMilestoneDatesRepository
    {
        MilestoneContext _dbContext;
     
        public MilestoneDatesRepository(MilestoneContext dbContext)
        {
            _dbContext = dbContext;            
        }
        public async Task<List<MilestoneDatesModel>> GetMilestoneDates(int request_id)
        {
            List<MilestoneSubtaskModel> result = new List<MilestoneSubtaskModel>();

            result = (from crd in _dbContext.ClientRequestDates
                      join cds in _dbContext.ClientDateSettings on crd.client_date_setting_ID equals cds.client_date_setting_ID
                      where crd.request_ID == request_id
                      select new MilestoneSubtaskModel()
                      {
                          client_request_date_ID = crd.client_request_date_ID,
                          client_ID = crd.client_ID,
                          request_ID = crd.request_ID,
                          client_date_setting_ID = crd.client_date_setting_ID,
                          begin_date = crd.begin_date,
                          end_date = crd.end_date,
                          complete_date = crd.complete_date,
                          subtask_name = crd.subtask_name,
                          assignee = crd.assignee,
                          comments = crd.comments,
                          task_name = cds.date_name
                      }).AsQueryable().ToList();
 
            var item = result.Select(r => r.task_name).Distinct().ToList();

            List<MilestoneDatesModel> milestoneDatesModel = new List<MilestoneDatesModel>();

            foreach (var data in item)
            {
                List<MilestoneSubtaskModel> milestoneSubtaskModels = result.Where(r=>r.task_name == data).ToList();

                milestoneDatesModel.Add(new MilestoneDatesModel() { TaskName = data, MilestoneSubtaskModels = milestoneSubtaskModels }); 
            }

            return await Task.Run(() => milestoneDatesModel.ToList());

        }

        public async Task<bool> InsertMilestoneDates(List<MilestoneDatesModel> model)
        {
            bool result = false;

            using (IDbContextTransaction transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach(var item in model)
                    {
                        var ext_client_date_setting_ID = _dbContext.ClientDateSettings.Where(cds => cds.date_name == item.TaskName).FirstOrDefault().client_date_setting_ID;

                        if(ext_client_date_setting_ID == 0)
                        {
                            ClientDateSettings clientDateSettings = new ClientDateSettings();
                            clientDateSettings.client_ID = item.MilestoneSubtaskModels.FirstOrDefault().client_ID;
                            clientDateSettings.date_name = item.TaskName;
                             clientDateSettings.default_fl = true;
                            clientDateSettings.active_fl = true;
                            clientDateSettings.date_sort = _dbContext.ClientDateSettings.OrderByDescending(ord=>ord.date_sort).FirstOrDefault().date_sort + 10;
                            clientDateSettings.createid = 1;
                            clientDateSettings.createdate = System.DateTime.Now;
                            clientDateSettings.createprocess = 0;
                            clientDateSettings.updateid = 1;
                            clientDateSettings.updatedate = System.DateTime.Now;
                            clientDateSettings.updateprocess = 0;

                            await _dbContext.AddAsync(clientDateSettings);
                            await _dbContext.SaveChangesAsync();

                            ext_client_date_setting_ID = clientDateSettings.client_date_setting_ID;

                        }

                        List<ClientRequestDates> dbclientRequestDates = new List<ClientRequestDates>();

                        foreach (var data in item.MilestoneSubtaskModels)
                        {
                            var entity = _dbContext.ClientRequestDates.Where(crd => crd.client_ID == data.client_ID && crd.request_ID == data.request_ID
                                                                             && crd.client_date_setting_ID == ext_client_date_setting_ID && crd.subtask_name == data.subtask_name).FirstOrDefault();

                            if(entity == null)
                            {
                                dbclientRequestDates.Add(new ClientRequestDates()
                                {
                                    client_ID = data.client_ID,
                                    request_ID = data.request_ID,
                                    client_date_setting_ID = ext_client_date_setting_ID,
                                    begin_date = data.begin_date,
                                    end_date = data.end_date,
                                    createid = 1,
                                    createdate = System.DateTime.Now,
                                    createprocess = 0,
                                    updateid = 1,
                                    updatedate = System.DateTime.Now,
                                    updateprocess = 0,
                                    complete_date = data.complete_date,
                                    subtask_name = data.subtask_name,
                                    assignee = data.assignee,
                                    comments = data.comments
                                });
                            }                      

                        }

                        if (dbclientRequestDates.Count > 0)
                        {
                            await _dbContext.AddRangeAsync(dbclientRequestDates);
                            await _dbContext.SaveChangesAsync();
                        }
                    }

                    await transaction.CommitAsync();
                    result = true;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
           return result;
        }
    }
}
