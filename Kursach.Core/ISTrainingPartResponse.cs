namespace ISTraining_Part.Core
{
   
    public class ISTrainingPartResponse<T, TArg> : ISTrainingPartResponse<T>
    {
        public TArg Arg { get; }

        public ISTrainingPartResponse(ISTrainingPartResponseCode code, TArg arg, T response) : base(code, response)
        {
            Arg = arg;
        }
    }


    public class ISTrainingPartResponse<T>
    {

        public ISTrainingPartResponseCode Code { get; }

        public T Response { get; }

        public ISTrainingPartResponse(ISTrainingPartResponseCode code, T response)
        {
            Code = code;
            Response = response;
        }

        public static implicit operator bool(ISTrainingPartResponse<T> response) => response.Code == ISTrainingPartResponseCode.Ok;
        public static implicit operator string(ISTrainingPartResponse<T> response) => response.ToString();

        public override string ToString()
        {
            switch (Code)
            {
                case ISTrainingPartResponseCode.Ok:
                    return "请求成功...";

                case ISTrainingPartResponseCode.ServerError:
                    return "服务器错误";

                case ISTrainingPartResponseCode.DbError:
                    return "数据库错误";

                default:
                    return "未处理的错误";
            }
        }
    }
}
