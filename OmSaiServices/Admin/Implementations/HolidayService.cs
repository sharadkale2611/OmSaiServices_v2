using Microsoft.Data.SqlClient;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Interfaces;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Admin.Implementations
{
    public  class HolidayService : Repository<HolidayModel>, IHolidayService
    {
        private readonly string sp_cud;
        private readonly string sp_r;
        private readonly Mapper _mapper;
        public HolidayService() 
        {
            sp_cud = "usp_CreateUpdateDeleteRestore_Holidays";
            sp_r = "usp_GetAll_Holidays";
            _mapper = new Mapper();

        }
        public int Create(HolidayModel model)
        {
            return Create(model, sp_cud, CreateUpdate(model, "create"));
        }

        public void Update(HolidayModel model)
        {
            Update(model, sp_cud, CreateUpdate(model, "update"));
        }


        public void Delete(int id)
        {
            Delete(sp_cud, DeleteRestore(id));
        }

        public void Restore(int id)
        {
            Restore(sp_cud, DeleteRestore(id));

        }

        public List<HolidayModel> GetAll()
        {

            // Define the mapping function
            var mapEntity = new Func<IDataReader, HolidayModel>(reader => _mapper.MapEntity<HolidayModel>(reader));

            return GetAll(sp_r, mapEntity, GetParams());
        }

        public HolidayModel GetById(int id)
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, HolidayModel>(reader => _mapper.MapEntity<HolidayModel>(reader));

            return GetById(id, sp_r, mapEntity, GetParams(id));
        }


        private List<KeyValuePair<string, object>> CreateUpdate(HolidayModel model, string type)
        {

            var HolidayId = type == "create" ? 0 : model.HolidayId;

            return new List<KeyValuePair<string, object>>
            {
                new("@HolidayId", HolidayId),
                new("@HolidayName", model.HolidayName),
                new("@HolidayDate",model.HolidayDate),
                new("@IsRecurring",model.IsRecurring),
                new("@Status", model.Status)
            };
        }

        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {

            return new List<KeyValuePair<string, object>>
            {
                new("@HolidayId", id)
            };

        }

        private SqlParameter[] GetParams(int? id = null, string? HolidayName = null,  bool? Status = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@HolidayId", id),
                new SqlParameter("@HolidayName", HolidayName),
                new SqlParameter("@Status", Status)
            };
        }
    }
}
