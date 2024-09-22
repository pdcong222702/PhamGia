using PhamGia.Data;
using System.Data;

namespace PhamGia.Common.Interface
{
    public interface IDataProvider
    {
        DataSet GetDatasetFromSp(string sPname, IDbDataParameter[] paramArr, string mConnectionString);
        DataSet GetDatasetFromSp_chart(string sPname, IDbDataParameter[] paramArr, string mConnectionString);

        DataSet GetDatasetFromSp_2(string sPname, string mConnectionString);

        DataSet GetDatasetFromSpPhanAnh(string sPname, string mConnectionString);
        DataTable GetDataTableFromSP(string SPname, IDbDataParameter[] paramArr, string connectionString);


        DataSet GetDatasetFromSpReturn2Out(string sPname, IDbDataParameter[] paramArr, string mConnectionString);

        Task<DataSet> GetDatasetFromSpAsync(string sPname, IDbDataParameter[] paramArr, string mConnectionString);
        Task<DataSet> GetDatasetFromSpAsync_2(string sPname, string m_ConnectionString);

        Task<DataSet> GetDatasetFromSpReturnMultiOutAsync(string sPname, IDbDataParameter[] paramArr, string mConnectionString);

        bool ExecuteSp(string sPname, IDbDataParameter[] paramArr, string mConnectionString);

        Task<bool> ExecuteSpAsync(string sPname, IDbDataParameter[] paramArr, string mConnectionString);

        /// <summary>
        /// Thực thi sp ,
        /// SP tryền vào phải có 2 output parameter ,
        ///  @Message  nvarchar(200) output ,
        ///  @ErrorCode int output
        /// </summary>
        /// <param name="sPname">Tên SP</param>
        /// <param name="paramArr">Các para chứa dữ liệu</param>
        /// <param name="mConnectionString">Chuỗi kết nối</param>
        /// <returns>Respose gồm mã lỗi và message </returns>
        ResponseMessage GetResponseFromExecutedSp(string sPname, IDbDataParameter[] paramArr, string mConnectionString);

        Task<ResponseMessage> GetResponseFromExecutedSpAsync(string sPname, IDbDataParameter[] paramArr, string mConnectionString);

        Task<ResponseMessage> GetResponseAndIdFromExecutedSp(string sPname, IDbDataParameter[] paramArr, string mConnectionString);

        IDbDataParameter CreateParameter(string parameterName, DbType dbType, object value);
    }
}
