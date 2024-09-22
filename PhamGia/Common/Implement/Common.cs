using Newtonsoft.Json;
using PhamGia.Common.Interface;
using PhamGia.Data;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PhamGia.Common.Implement
{
    public class Common : ErrorHandle, ICommon
    {
        public Common(ISerilogProvider serilogProvider)
            : base(serilogProvider)
        {
        }

        public string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public string GetDeepCaller()
        {
            string strCallerName = string.Empty;
            for (int i = 3; i >= 2; i--)
            {
                strCallerName += GetCaller(i) + "=>";
            }

            // returns a composite of the namespace, class and method name.
            return strCallerName;
        }

        /// <summary>
        /// get caller function name
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public string GetCaller(int level = 2)
        {
            var m = new StackTrace().GetFrame(level).GetMethod();

            if (m.DeclaringType == null)
            {
                return string.Empty; // 9:33 AM 6/18/2014 Exception Details: System.NullReferenceException: Object reference not set to an instance of an object.
            }

            // .Name is the name only, .FullName includes the namespace
            var className = m.DeclaringType.FullName;

            // the method/function name you are looking for.
            var methodName = m.Name;

            // returns a composite of the namespace, class and method name.
            return className + "->" + methodName;
        }

        public string FormatDate(string strDateTime)
        {
            if (string.IsNullOrEmpty(strDateTime))
            {
                return null;
            }
            else
            {
                string[] s = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    s[i] = string.Empty;
                }

                string str = string.Empty;
                int n = 0;
                for (int i = 0; i < strDateTime.Length; i++)
                {
                    if (strDateTime[i] != '/')
                    {
                        s[n] += strDateTime[i];
                    }
                    else
                    {
                        n++;
                    }
                }

                str = s[2] + '/' + s[1] + '/' + s[0];
                return str;
            }
        }

        public DateTime ConvertStringFromNewsToDateTime(string strDateTime)
        {
            DateTimeFormatInfo usDtfi = new CultureInfo("vi-VN", false).DateTimeFormat;
            return Convert.ToDateTime(strDateTime, usDtfi);
        }

        public byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }

        /// <summary>
        /// serialize object to string for
        /// </summary>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public string SerializeObject(object objectToSerialize)
        {
            try
            {
                return JsonConvert.SerializeObject(objectToSerialize); // SLOW SPEED
            }
            catch
            {
                return string.Empty; // failed
            }
        }

        /// <summary>
        /// Serialize param array to string
        /// </summary>
        /// <param name="paramArray"></param>
        /// <returns></returns>
        public string ParametersToString(IDbDataParameter[] paramArray)
        {
            const string CHAR_CRLF = "\r\n";
            const string CHAR_TAB = "\t";

            if (paramArray == null || paramArray.Length == 0)
                return string.Empty;

            var infoString = CHAR_CRLF;
            foreach (var parameter in paramArray)
                infoString += CHAR_TAB + parameter.ParameterName + "='" + parameter.Value + "'," + CHAR_CRLF;

            return infoString;
        }

        /// <summary>
        /// 2019-11-26 10:20:55 ngocta2
        /// lay thong tin ve dataset
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public string GetResultInfo(DataSet dataSet)
        {
            var tableCount = 0;
            string infoString;
            if (dataSet == null)
            {
                infoString = "dataSet=null";
            }
            else
            {
                infoString = $"dataSet.Tables.Count={dataSet.Tables.Count}; ";
                foreach (DataTable dataTable in dataSet.Tables)
                    infoString += $"dataTable[{tableCount++}].Rows.Count={dataTable.Rows.Count}; ";
            }

            return infoString;
        }

        /// <summary>
        /// 2019-11-26 10:20:55 ngocta2
        /// lay thong tin ve cac row da bi anh huong bi sql
        /// </summary>
        /// <param name="affectedRowCount"></param>
        /// <returns></returns>
        public string GetResultInfo(int affectedRowCount)
        {
            return $"affectedRowCount={affectedRowCount}";
        }

        // read file to string
        public string ReadFileToString(string strFullPath)
        {
            try
            {
                // read
                string strBody = string.Empty;

                using (StreamReader sr = new StreamReader(strFullPath))
                {
                    strBody = sr.ReadToEnd();
                }

                return strBody;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return string.Empty;
            }
        }

        // check null của DataRow và ép kiểu dữ liệu
        public int GetSafeInt<T>(T col)
        {
            if (col != null)
            {
                bool isNumber = int.TryParse(col.ToString(), out int number);

                if (isNumber) return number;
            }

            return 0;
        }

        public long GetSafeLong<T>(T col)
        {
            if (col != null)
            {
                bool isNumber = long.TryParse(col.ToString(), out long number);

                if (isNumber) return number;
            }

            return 0;
        }

        public float GetSafeFloat<T>(T col)
        {
            if (col != null)
            {
                bool isNumber = float.TryParse(col.ToString(), out float number);

                if (isNumber) return number;
            }

            return 0;
        }

        public double GetSafeDouble<T>(T col)
        {
            if (col != null)
            {
                bool isNumber = double.TryParse(col.ToString(), out double number);

                if (isNumber) return number;
            }

            return 0;
        }

        public decimal GetSafeDecimal<T>(T col)
        {
            if (col != null)
            {
                bool isNumber = decimal.TryParse(col.ToString(), out decimal number);

                if (isNumber) return number;
            }

            return 0;
        }

        public string GetSafeString<T>(T col)
        {
            if (col != null)
            {
                return col.ToString();
            }

            return string.Empty;
        }

        public bool GetSafeBool<T>(T col)
        {
            bool isTrue = false;

            if (col != null)
            {
                if (col.GetType() == typeof(bool))
                {
                    return bool.Parse(col.ToString());
                }

                isTrue = (col.ToString() == "0") ? false : true;

                return isTrue;
            }

            return isTrue;
        }

        public DateTime GetSafeDate<T>(T col)
        {
            if (col != null)
            {
                bool isNumber = DateTime.TryParse(col.ToString(), out DateTime date);

                if (isNumber) return date;
            }

            return DateTime.MinValue;
        }

        public void MapProp(object sourceObj, object targetObj)
        {
            Type T1 = sourceObj.GetType();
            Type T2 = targetObj.GetType();

            PropertyInfo[] sourceProprties = T1.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            PropertyInfo[] targetProprties = T2.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var sourceProp in sourceProprties)
            {
                object osourceVal = sourceProp.GetValue(sourceObj, null);
                int entIndex = Array.IndexOf(targetProprties, sourceProp);
                if (entIndex >= 0)
                {
                    var targetProp = targetProprties[entIndex];
                    targetProp.SetValue(targetObj, osourceVal);
                }
            }
        }

        public bool IsValidMail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        //public async Task<ResponseMessage> SaveFile(IFormFile file, string targetFolder, IHostingEnvironment hostingEnvironment, IConfiguration _configuration)
        //{
        //    var response = new ResponseMessage { Code = "-1" };

        //    try
        //    {
        //        targetFolder = targetFolder.Replace(@"/", @"\");

        //        var filePath = hostingEnvironment.ContentRootPath;

        //        // check và đặt tên file để không trùng tên cũ
        //        var i = 1;
        //        var fileName = $"File_{i}_{file.FileName.Replace(" ", "")}";
        //        var filePathsave = Path.Combine(filePath, targetFolder, fileName);
        //        while (File.Exists(filePathsave))
        //        {
        //            i++;
        //            fileName = $"File_{i}_{file.FileName}";
        //            filePathsave = Path.Combine(filePath, targetFolder, fileName);
        //        }

        //        using (var stream = new FileStream(filePathsave, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //        var SubPath = targetFolder + @"\" + fileName;
        //        //if (File.Exists(filePathsave))
        //        //{
        //        //    //Copy file từ server chứa code sang server moi truong
        //        //    CommonUtil.CopyFileUploaded(filePathsave, SubPath, _configuration);
        //        //    //Xóa file vừa copy
        //        //    //File.Delete(filePathsave);
        //        //}
        //        response.Code = "0";
        //        response.Message = "UPLOAD_FILE_SUCCESS";
        //        response.Data = Path.Combine(targetFolder, fileName);
        //    }
        //    catch (Exception e)
        //    {
        //        response.Code = "-1";
        //        response.Message = e.Message;
        //    }

        //    return response;
        //}
        public bool HasSpecialChars(string str)
        {
            return str.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch));
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        //public Task<ResponseMessage> SaveFile(IFormFile file, string targetFolder, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IConfiguration _configuration)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
