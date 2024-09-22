namespace PhamGia.Data
{
    public class ResponseMessage
    {
        public const string MSG_NO_ACCESS = "Không có quyền thực hiện hành động này";
        public const string CODE_NO_ACCESS = "-99999";
        /// <summary>
        /// Mã lỗi
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// DỮ liệu đi kèm
        /// </summary>
        public object Data { get; set; }

        public int ID { get; set; }

        public ResponseMessage()
        {
        }

        public ResponseMessage(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
        public ResponseMessage(string code, string message, int id)
        {
            this.Code = code;
            this.Message = message;
            this.ID = id;
        }
        public static ResponseMessage ResponseMessageNonAccess()
        {
            return new ResponseMessage { Code = CODE_NO_ACCESS, Message = MSG_NO_ACCESS };
        }
    }
}
