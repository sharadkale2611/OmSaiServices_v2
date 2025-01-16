using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
    public class ProfileImageModel
    {
        public int? WorkerId { get; set; }
        public string ProfileImage { get; set; }

        List<WorkerModel> list {  get; set; }
    }


}
