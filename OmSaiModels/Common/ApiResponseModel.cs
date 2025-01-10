using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Common
{
	public class ApiResponseModel<T>
	{
		public bool Success { get; set; }
		public T Data { get; set; }
		public object Errors { get; set; }

		public ApiResponseModel(bool success, T data, object errors)
		{
			Success = success;
			Data = data;
			Errors = errors;
		}
	}
}
