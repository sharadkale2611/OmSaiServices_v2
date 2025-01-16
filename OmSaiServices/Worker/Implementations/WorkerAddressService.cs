using Microsoft.Data.SqlClient;
using OmSaiModels.Worker;
using OmSaiServices.Common;
using OmSaiServices.Worker.Interfaces;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Worker.Implementations
{
    public class WorkerAddressService : Repository<WorkerAddressModel>, IWorkerAddressService
    {
        private readonly string usp_cud;
        private readonly string usp_r;
        private readonly Mapper _mapper;

        public WorkerAddressService()
        {
            usp_cud = "usp_CreateUpdateDeleteRestore_WorkerAddresses";
            usp_r = "usp_GetAll_WorkerAddresses";
            _mapper = new Mapper();
        }
        public int Create(WorkerAddressModel model)
        {
            return Create(model, usp_cud, CreateUpdate(model, "create"));
        }

        public void Update(WorkerAddressModel model)
        {
            Update(model, usp_cud, CreateUpdate(model, "update"));
        }


        public void Delete(int id)
        {
            Delete(usp_cud, DeleteRestore(id));
        }

        public void Restore(int id)
        {
            Restore(usp_cud, DeleteRestore(id));
        }

        public List<WorkerAddressModel> GetAll()
        {

            // Define the mapping function
            var mapEntity = new Func<IDataReader, WorkerAddressModel>(reader => _mapper.MapEntity<WorkerAddressModel>(reader));

            return GetAll(usp_r, mapEntity, GetParams());
        }


		public List<WorkerAddressModel> GetByWorkerId(int id)
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, WorkerAddressModel>(reader => _mapper.MapEntity<WorkerAddressModel>(reader));

			return GetAll(usp_r, mapEntity, GetParams(null, id));
		}




		public WorkerAddressModel GetById(int id)
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, WorkerAddressModel>(reader => _mapper.MapEntity<WorkerAddressModel>(reader));

            return GetById(id, usp_r, mapEntity, GetParams(id));
        }


        private List<KeyValuePair<string, object>> CreateUpdate(WorkerAddressModel model, string type)
        {

            var WorkerAddressId = type == "create" ? 0 : model.WorkerAddressId;



            return new List<KeyValuePair<string, object>>
            {
                new("@WorkerAddressId", WorkerAddressId),
                new("@WorkerId", model.WorkerId),
                new("@AddressType", model.AddressType),
                new("@Address", model.Address),
                new("@StateId", model.StateId),
                new("@CityId", model.CityId),
                new("@Pincode", model.Pincode)


            };
        }

        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {

            return new List<KeyValuePair<string, object>>
            {
                new("@WorkerAddressId", id),

            };

        }

        private SqlParameter[] GetParams(int? id = null,int? WorkerId = null, int? StateId = null, int? CityId = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@WorkerAddressId", id),
                new SqlParameter("@WorkerId", WorkerId),
                new SqlParameter("@StateId", StateId),
                new SqlParameter("@CityId", CityId),



            };
        }

       
    }
}
