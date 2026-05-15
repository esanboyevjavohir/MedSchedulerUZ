using MedSchedulerUZ.Core.Common;

namespace MedSchedulerUZ.Application.Models
{
    public class ApiResult<T>
    {
        private ApiResult() { }

        public bool Succedded { get; set; }
        public T Result { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public ApiResult(bool succedded, T result, IEnumerable<string> errors)
        {
            Succedded = succedded;
            Result = result;
            Errors = errors;
        }

        public static ApiResult<T> Success(T result)
        {
            return new ApiResult<T>(true, result, new List<string>());
        }

        public static ApiResult<T> Failure(IEnumerable<string> errors)
        {
            return new ApiResult<T>(false, default, errors);
        }

        public static ApiResult<T> Failure(Errors errors)
        {
            return new ApiResult<T>
            {
                Succedded = false,
                Errors = new List<string> { errors.message }
            };
        }
    }
}
