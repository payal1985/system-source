namespace SSInventory.Share.Models
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static ResponseModel Successed(string message = "", int statusCode = 200, object data = null)
        {
            return new ResponseModel { Success = true, StatusCode = statusCode, Message = message, Data = data };
        }
        public static ResponseModel Failed(string message = "", int statusCode = 200, object data = null)
        {
            return new ResponseModel { Success = false, StatusCode = statusCode, Message = message, Data = data };
        }
    }
}
