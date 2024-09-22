using PhamGia.Common.Interface;
using PhamGia.Data;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using System.Data.SqlClient;

namespace PhamGia.Common.Implement
{
    public class DataProvider : ErrorHandle, IDataProvider, IDisposable
    {
        // const
        public const int DEFAULT_PARAM_OUTPUT_CODE = -1;
        public const string DEFAULT_PARAM_OUTPUT_MSG = "out";

        private SqlDataAdapter _dataAdapter;
        private double _dblDuration;

        // props
        public string ConnectionString { get; set; }

        /// <summary>
        /// 2019-03-13 9:13:58 CHIENND
        /// contructor, lay conn string, logger instance
        /// </summary>
        /// <param name="strConnectionString"></param>
        public DataProvider(ISerilogProvider serilogProvider)
            : base(serilogProvider)
        {
            Command = new SqlCommand();
        }

        public SqlParameterCollection Parameters
        {
            get
            {
                Command.Parameters.Clear();
                return Command.Parameters;
            }
        }

        public SqlCommand Command { get; set; }

        public SqlConnection Connection { get; set; }

        public SqlDataReader DataReader { get; set; }

        public int ExecDuration => Convert.ToInt32(this._dblDuration);

        /// <summary>
        /// 2019-02-12 2:02:54 chiennd
        /// lay thong tin parma de ghi log sql
        /// </summary>
        /// <param name="dP"></param>
        /// <returns></returns>
    //    public string GetParamInfo(DynamicParameters dP)
    //    {
    //        try
    //        {
    //            if (dP == null)
    //            {
    //                return string.Empty;
    //            }

    //            var sb = new StringBuilder();
    //            foreach (var name in dP.ParameterNames)
    //            {
    //                var pValue = dP.Get<dynamic>(name);
    //                sb.AppendFormat("\n@{0}=N'{1}',", name, pValue.ToString());
    //            }

    //            sb.Length--;

    //            string processedScript = ReplacePassword(sb.ToString());
    //            return processedScript; // sb.ToString();
    //        }
    //        catch (Exception ex)
    //        {
    //            // log error
    //            this.WriteToFile(ex);

    //            return string.Empty;
    //        }
    //}

        /// <summary>
        /// 2019-02-12 2:02:54 chiennd
        /// lay thong tin parma de ghi log sql
        /// </summary>
        /// <param name="dP"></param>
        /// <returns></returns>
        public string GetParamInfo(IDbDataParameter[] dP)
        {
            try
            {
                if (dP == null)
                {
                    return string.Empty;
                }

                var sb = new StringBuilder();
                foreach (var parameter in dP)
                {
                    if (parameter.Value == null) parameter.Value = "NULL";
                    sb.Append(parameter).Append("=").Append(parameter.Value).Append(" ");
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                // log error
                this.WriteToFile(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 2018-12-23 12:36:43 ngocta2
        /// xoa mat khau
        /// </summary>
        /// <param name="sqlScript">
        /// prc_USER_LOGIN
        ///     @UserName=N'skidrow2',
        ///     @Password=N'5AE42FE64D206B2E34DE2CFF4201CD14',
        ///     @Code=N'-1',
        ///     @Msg=N'out'
        /// </param>
        /// <returns>
        /// prc_USER_LOGIN
        ///     @UserName=N'skidrow2',
        ///     @Password=N'******',
        ///     @Code=N'-1',
        ///     @Msg=N'out'
        /// </returns>
        private string ReplacePassword(string sqlScript)
        {
            var regexObj = new Regex("@Password=N'(.*?)'");
            try
            {
                var matchResults = regexObj.Match(sqlScript);
                if (matchResults.Success)
                {
                    var text = matchResults.Groups[1].Value;
                    var processedScript = sqlScript.Replace(text, "******");
                    return processedScript;
                }
                else
                {
                    return sqlScript;
                }
            }
            catch (Exception ex)
            {
                // log error
                this.WriteToFile(ex);

                return sqlScript;
            }
        }

        // Ham nay thuc hien viec ket noi voi database
        // COMMAND => The time in seconds to wait for the command to execute. The default is 30 seconds.
        public bool OpenConnection(string mConnectionString)
        {
            bool functionReturnValue = false;

            try
            {
                if (Connection == null || Connection.State == ConnectionState.Closed) // 2015-07-16 09:33:40 ngocta2 => bug: open 2 connection, close 1
                {
                    // CHUA OPEN roi thi tao new connection
                    Connection = new SqlConnection(mConnectionString);
                    Connection.Open();
                    functionReturnValue = true;
                }
                else
                {
                    if (Connection.State == ConnectionState.Open)
                    {
                        // OPEN roi thi ko tao connection nua
                        functionReturnValue = true;
                    }
                }
            }
            catch (Exception ex)
            {
                functionReturnValue = false;
                this.WriteToFile(ex);
            }

            return functionReturnValue;
        }

        // Ham nay dung de dong 1 connection den database
        public void CloseConnection()
        {
            try
            {
                if (Connection.State != ConnectionState.Open) return;
                Connection.Close();
                Connection.Dispose();
                Command.Dispose();
            }
            catch (Exception ex)
            {
                this.WriteToFile(ex);
            }
        }

        /// <summary>
        /// GetDatasetFromSP
        /// </summary>
        /// <param name="sPname">sp_selectDataset</param>
        /// <returns></returns>
        public DataSet GetDatasetFromSp(string sPname, IDbDataParameter[] paramArr, string m_ConnectionString)
        {
            paramArr.ToList();
            DataSet functionReturnValue;
            if (OpenConnection(m_ConnectionString) == false) // Open Connection
            {
                return null;
            }

            var ds = new DataSet();
            try
            {

                Command.Connection = Connection;
                Command.CommandText = sPname;
                Command.CommandType = CommandType.StoredProcedure;
                if (paramArr != null)
                {
                    if (paramArr.Length > 0)
                    {
                        foreach (SqlParameter param in paramArr)
                        {
                            Command.Parameters.Add(param);
                        }
                    }
                }

                var dtBegin = DateTime.Now; // duration
                _dataAdapter = new SqlDataAdapter(Command);
                _dataAdapter.Fill(ds);
                this._dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration
                Command.Parameters.Clear();
                functionReturnValue = ds;
            }
            catch (Exception ex)
            {
                functionReturnValue = null;
                this.WriteToFile(ex);
            }
            finally
            {
                Command.Parameters.Clear();
                CloseConnection();     // Close Connection
            }

            return functionReturnValue;
        }


        public DataSet GetDatasetFromSp_chart(string sPname, IDbDataParameter[] paramArr, string m_ConnectionString)
        {
            paramArr.ToList();
            DataSet functionReturnValue;
            if (OpenConnection(m_ConnectionString) == false) // Open Connection
            {
                return null;
            }

            var ds = new DataSet();
            try
            {

                Command.Connection = Connection;
                Command.CommandText = sPname;
                Command.CommandType = CommandType.StoredProcedure;
                if (paramArr != null)
                {
                    if (paramArr.Length > 0)
                    {
                        foreach (SqlParameter param in paramArr)
                        {
                            Command.Parameters.Add(param);
                        }
                    }
                }

                var dtBegin = DateTime.Now; // duration
                _dataAdapter = new SqlDataAdapter(Command);
                _dataAdapter.Fill(ds);
                this._dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration
                Command.Parameters.Clear();
                functionReturnValue = ds;
            }
            catch (Exception ex)
            {
                functionReturnValue = null;
                this.WriteToFile(ex);
            }
            finally
            {
                Command.Parameters.Clear();
                CloseConnection();     // Close Connection
            }

            return functionReturnValue;
        }
        public DataSet GetDatasetFromSp_2(string sPname, string m_ConnectionString)
        {
            DataSet functionReturnValue;
            if (OpenConnection(m_ConnectionString) == false) // Open Connection
            {
                return null;
            }

            var ds = new DataSet();
            try
            {

                Command.Connection = Connection;
                Command.CommandText = sPname;
                Command.CommandType = CommandType.StoredProcedure;

                var dtBegin = DateTime.Now; // duration
                _dataAdapter = new SqlDataAdapter(Command);
                _dataAdapter.Fill(ds);
                this._dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration
                Command.Parameters.Clear();
                functionReturnValue = ds;
            }
            catch (Exception ex)
            {
                functionReturnValue = new DataSet();
                this.WriteToFile(ex);
            }
            finally
            {
                Command.Parameters.Clear();
                CloseConnection();     // Close Connection
            }

            return functionReturnValue;
        }
        public DataSet GetDatasetFromSpReturn2Out(string sPname, IDbDataParameter[] paramArr, string m_ConnectionString)
        {
            paramArr.ToList();
            DataSet functionReturnValue;
            if (OpenConnection(m_ConnectionString) == false) // Open Connection
            {
                return null;
            }

            var ds = new DataSet();
            try
            {

                Command.Connection = Connection;
                Command.CommandText = sPname;
                Command.CommandType = CommandType.StoredProcedure;
                if (paramArr != null)
                {
                    if (paramArr.Length > 0)
                    {
                        foreach (SqlParameter param in paramArr)
                        {
                            Command.Parameters.Add(param);
                        }
                    }
                }

                var dtBegin = DateTime.Now; // duration
                _dataAdapter = new SqlDataAdapter(Command);
                _dataAdapter.Fill(ds);
                this._dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration
                Command.Parameters.Clear();
                functionReturnValue = ds;
            }
            catch (Exception ex)
            {
                functionReturnValue = null;
                this.WriteToFile(ex);
            }
            finally
            {
                Command.Parameters.Clear();
                CloseConnection();     // Close Connection
            }

            return functionReturnValue;
        }

        public async Task<DataSet> GetDatasetFromSpAsync(string sPname, IDbDataParameter[] paramArr, string m_ConnectionString)
        {
            if (OpenConnection(m_ConnectionString) == false) // Open Connection
            {
                return null;
            }

            var ds = new DataSet();
            try
            {

                Command.Connection = Connection;
                Command.CommandText = sPname;
                Command.CommandType = CommandType.StoredProcedure;
                if (paramArr != null)
                {
                    if (paramArr.Length > 0)
                    {
                        foreach (var param in paramArr)
                        {
                            Command.Parameters.Add(param);
                        }
                    }
                }

                var dtBegin = DateTime.Now; // duration
                _dataAdapter = new SqlDataAdapter(Command);
                await Task.Run(() => _dataAdapter.Fill(ds));
                this._dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration
                Command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                this.WriteToFile(ex);
            }
            finally
            {
                Command.Parameters.Clear();
                CloseConnection();     // Close Connection
            }

            return ds;
        }

        public async Task<DataSet> GetDatasetFromSpAsync_2(string sPname, string m_ConnectionString)
        {
            if (OpenConnection(m_ConnectionString) == false) // Open Connection
            {
                return null;
            }

            var ds = new DataSet();
            try
            {

                Command.Connection = Connection;
                Command.CommandText = sPname;
                Command.CommandType = CommandType.StoredProcedure;

                var dtBegin = DateTime.Now; // duration
                _dataAdapter = new SqlDataAdapter(Command);
                await Task.Run(() => _dataAdapter.Fill(ds));
                this._dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration
                Command.Parameters.Clear();
            }
            catch (Exception ex)
            {
                this.WriteToFile(ex);
            }
            finally
            {
                Command.Parameters.Clear();
                CloseConnection();     // Close Connection
            }

            return ds;
        }

        public bool ExecuteSp(string sPname, IDbDataParameter[] paramArr, string m_ConnectionString)
        {
            if (sPname is null) throw new ArgumentNullException(nameof(sPname));
            bool executeSuccess = false;
            if (OpenConnection(m_ConnectionString) == false) // Open Connection
            {
                return false;
            }

            try
            {

                Command.Connection = Connection;
                Command.CommandText = sPname;
                Command.CommandType = CommandType.StoredProcedure;
                if (paramArr != null)
                {
                    if (paramArr.Length > 0)
                    {
                        foreach (SqlParameter param in paramArr)
                        {
                            Command.Parameters.Add(param);
                        }
                    }
                }

                DateTime dtBegin = DateTime.Now; // duration

                if (Command.ExecuteNonQuery() > 0)
                {
                    executeSuccess = true;

                    this.WriteStringToFile("Ater connect DB SQL -> Store Procedure Name:" + sPname + " | Parameter:", GetParamInfo(paramArr));
                }

                this._dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration
            }
            catch (Exception ex)
            {
                this.WriteToFile(ex);
                executeSuccess = false;
            }
            finally
            {
                Command.Parameters.Clear();
                CloseConnection();     // Close Connection
            }

            return executeSuccess;
        }

        public async Task<bool> ExecuteSpAsync(string sPname, IDbDataParameter[] paramArr, string m_ConnectionString)
        {
            if (sPname is null) throw new ArgumentNullException(nameof(sPname));
            bool executeSuccess = false;
            if (OpenConnection(m_ConnectionString) == false) // Open Connection
            {
                return false;
            }

            try
            {

                Command.Connection = Connection;
                Command.CommandText = sPname;
                Command.CommandType = CommandType.StoredProcedure;
                if (paramArr != null)
                {
                    if (paramArr.Length > 0)
                    {
                        foreach (var param in paramArr)
                        {
                            Command.Parameters.Add(param);
                        }
                    }
                }

                DateTime dtBegin = DateTime.Now; // duration
                if (await Command.ExecuteNonQueryAsync() > 0)
                {
                    executeSuccess = true;

                    this.WriteStringToFile("Ater connect DB SQL -> Store Procedure Name:" + sPname + " | Parameter:", GetParamInfo(paramArr));
                }

                this._dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration
            }
            catch (Exception ex)
            {
                this.WriteToFile(ex);
                executeSuccess = false;
            }
            finally
            {
                Command.Parameters.Clear();
                CloseConnection();     // Close Connection
            }
            return executeSuccess;
        }

        public ResponseMessage GetResponseFromExecutedSp(string sPname, IDbDataParameter[] paramArr, string m_ConnectionString)
        {
            var message = new SqlParameter()
            {
                ParameterName = "Message",
                SqlDbType = SqlDbType.NVarChar,
                Size = 100,
                Direction = ParameterDirection.Output,
                Value = "Không thể thực hiện",
            };
            var errorCode = new SqlParameter()
            {
                ParameterName = "ErrorCode",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Output,
                Value = -1,
            };

            var paramList = paramArr.ToList();
            paramList.Add(message);
            paramList.Add(errorCode);

            paramArr = paramList.ToArray();

            this.ExecuteSp(sPname, paramArr, m_ConnectionString);

            var responeMessage = new ResponseMessage()
            {
                Code = errorCode.SqlValue?.ToString(),
                Message = message.SqlValue?.ToString(),
            };
            return responeMessage;
        }

        public async Task<ResponseMessage> GetResponseFromExecutedSpAsync(string sPname, IDbDataParameter[] paramArr, string m_ConnectionString)
        {
            var message = new SqlParameter()
            {
                ParameterName = "Message",
                SqlDbType = SqlDbType.NVarChar,
                Size = 200,
                Direction = ParameterDirection.Output,
                Value = "Không thể thực hiện",
            };
            var errorCode = new SqlParameter()
            {
                ParameterName = "ErrorCode",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
                Value = 0,
            };
            var paramList = paramArr.ToList();
            paramList.Add(message);
            paramList.Add(errorCode);
            paramArr = paramList.ToArray();
            await this.ExecuteSpAsync(sPname, paramArr, m_ConnectionString);
            var responeMessage = new ResponseMessage()
            {
                Code = errorCode.SqlValue?.ToString(),
                Message = message.SqlValue?.ToString(),
            };
            return responeMessage;
        }

        public IDbDataParameter CreateParameter(string parameterName, DbType dbType, object value)
        {
            return new SqlParameter
            {
                ParameterName = parameterName,
                DbType = dbType,
                Value = value,
            };
        }

        public void Dispose()
        {
            Command?.Dispose();
            Connection?.Dispose();
            _dataAdapter?.Dispose();
            DataReader?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<DataSet> GetDatasetFromSpReturnMultiOutAsync(string sPname, IDbDataParameter[] paramArr, string connectionString)
        {

            throw new Exception();
        }

        public DataTable GetDataTableFromSP(string SPname, IDbDataParameter[] paramArr, string connectionString)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ghi log tên sp và chuỗi đầu vào
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="paramArr"></param>
        public void ConvertSPnameToLog(string spName, IDbDataParameter[] paramArr)
        {
            try
            {
                string stringParam = string.Empty;

                foreach (IDbDataParameter re in paramArr)
                {
                    stringParam += re.ParameterName + ":" + re.Value + ";";
                }

                this.WriteStringToFile(spName, stringParam);
            }
            catch (Exception ex)
            {
            }
        }

        public Task<ResponseMessage> GetResponseAndIdFromExecutedSp(string sPname, IDbDataParameter[] paramArr, string mConnectionString)
        {
            throw new NotImplementedException();
        }

        public DataSet GetDatasetFromSpPhanAnh(string sPname, string mConnectionString)
        {
            DataSet functionReturnValue;
            if (OpenConnection(mConnectionString) == false) // Open Connection
            {
                return null;
            }

            var ds = new DataSet();
            try
            {

                Command.Connection = Connection;
                Command.CommandText = sPname;
                Command.CommandType = CommandType.StoredProcedure;

                var dtBegin = DateTime.Now; // duration
                _dataAdapter = new SqlDataAdapter(Command);
                _dataAdapter.Fill(ds);
                this._dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration
                Command.Parameters.Clear();
                functionReturnValue = ds;
            }
            catch (Exception ex)
            {
                functionReturnValue = new DataSet();
                this.WriteToFile(ex);
            }
            finally
            {
                Command.Parameters.Clear();
                CloseConnection();     // Close Connection
            }

            return functionReturnValue;
        }
    }
}
