namespace Entity.ResponseModels
{
    public abstract class BaseResponse<T>
    {
        public BaseResponse(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        public BaseResponse(bool isSuccess, string message, T data)
            : this(isSuccess, message)
        {
            this.Data = data;
        }

        public T Data { get; }
        public bool IsSuccess { get; }
        public string Message { get; }
    }
}