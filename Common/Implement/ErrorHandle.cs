using PhamGia.Common.Interface;

namespace PhamGia.Common.Implement
{
    public class ErrorHandle : IErrorHandle
    {
        // props
        public ILogger Logger { get; }

        /// <summary>
        /// 2018-12-22 21:13:58 ngocta2
        /// contructor, lay conn string, logger instance
        /// </summary>
        /// <param name="serilogProvider"></param>
        public ErrorHandle(ISerilogProvider serilogProvider)
        {
            this.Logger = serilogProvider?.Logger;
        }

        public void WriteToFile(Exception ex)
        {
            string template = "\r\n-----Message-----\r\n{0}\r\n-----Source ---\r\n{1}\r\n-----StackTrace ---\r\n{2}\r\n-----TargetSite ---\r\n{3}";
            //this.Logger.Error(template, ex?.Message, ex?.Source, ex?.StackTrace, ex?.TargetSite);
        }

        //log đoạn lỗi
        public void WriteStringToFile(string SPname, string paramArr)
        {
            string template = "\r\n-----SPname-----\r\n{0}\r\n-----paramArr ---\r\n{1}\r\n";
            //this.Logger.Error(template, SPname, paramArr);
        }
    }
}
