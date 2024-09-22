namespace PhamGia.Common.Interface
{
    public interface IErrorHandle
    {
        void WriteToFile(Exception ex);

        /// <summary>
        /// Log đoạn lỗi
        /// </summary>
        /// <param name="Messegae"></param>
        void WriteStringToFile(string SPname, string paramArr);
    }
}
