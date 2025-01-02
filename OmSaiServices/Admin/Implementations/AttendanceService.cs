using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Interfaces;
using OmSaiServices.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OmSaiServices.Admin.Implementations
{
    public class AttendanceService : Repository<AttendanceModel>, IAttendanceService
    {
        private readonly string sp_cud;
        private readonly string sp_r;
        private readonly Mapper _mapper;

        public AttendanceService()
        {
            sp_cud = "usp_CreateUpdateDeleteRestore_Attendance";
            sp_r = "usp_GelAll_Attendance";
            _mapper = new Mapper();
        }


        public int Create(AttendanceModel model)
        {
            return Create(model, sp_cud, CreateUpdate(model, "create"));
        }

        public void Update(AttendanceModel model)
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

        public List<AttendanceModel> GetAll()
        {

            // Define the mapping function
            var mapEntity = new Func<IDataReader, AttendanceModel>(reader => _mapper.MapEntity<AttendanceModel>(reader));

            return GetAll(sp_r, mapEntity, GetParams());
        }

        public AttendanceModel GetById(int id)
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, AttendanceModel>(reader => _mapper.MapEntity<AttendanceModel>(reader));

            return GetById(id, sp_r, mapEntity, GetParams(id));
        }


        private List<KeyValuePair<string, object>> CreateUpdate(AttendanceModel model, string type)
        {

            var AttendanceId = type == "create" ? 0 : model.AttendanceId;

            return new List<KeyValuePair<string, object>>
            {
                new("@AttendanceId", AttendanceId),
                new("@EmployeeId", model.EmployeeId),
                new("@CheckInTime", model.CheckInTime),
                new("@CheckOutTime", model.CheckOutTime),
                new("@Date", model.Date),
                //new("@CreatedAt", model.CreatedAt),
                new("@SelfiPath", model.SelfiPath),
                new("@GeoLocation", model.GeoLocation),
                new("@LateIn", model.LateIn),
                new("@EarlyOut", model.EarlyOut),
                new("@Remark", model.Remark),
                new("@Status", model.Status)
            };
        }

        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {

            return new List<KeyValuePair<string, object>>
            {
                new("@AttendanceId", id)
            };

        }

        private SqlParameter[] GetParams(int? id = null, int? EmployeeId = null, string? CheckInTime = null, string? CheckOutTime = null, string? Date = null, /*string? CreatedAt = null,*/ string? SelfiPath = null, string? GeoLocation = null, string? LateIn = null, string? EarlyOut = null, string? Remark = null, bool? Status = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@AttendanceId", id),
                new SqlParameter("@EmployeeId", EmployeeId),
                new SqlParameter("@CheckInTime", CheckInTime),
                new SqlParameter("@CheckOutTime", CheckOutTime),
                new SqlParameter("@Date", Date),
                //new SqlParameter("@CreatedAt", CreatedAt),
                new SqlParameter("@SelfiPath", SelfiPath),
                new SqlParameter("@GeoLocation", GeoLocation),
                new SqlParameter("@LateIn", LateIn),
                new SqlParameter("@EarlyOut", EarlyOut),
                new SqlParameter("@Remark", Remark),
                new SqlParameter("@Status", Status)
            };
        }

    }
}
