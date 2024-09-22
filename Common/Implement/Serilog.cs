using PhamGia.Common.Interface;

namespace PhamGia.Common.Implement
{
    public class Serilog : ISerilogProvider
    {
        public ILogger Logger { get; }

        /// <summary>
        /// 2018-12-22 17:30:51 ngocta2
        /// constructor
        /// tao instance, thuong la dung singleton
        /// </summary>
        /// <param name="logger"></param>
        public Serilog(IConfiguration configuration, ILogger logger)
        {
            if (logger == null)
            {
                //if (configuration != null)
                //{
                //    var serilog = new LoggerConfiguration()
                //    .ReadFrom.Configuration(configuration)
                //    .CreateLogger();
                //    this.Logger = serilog;
                //}
            }
            else
            {
                this.Logger = logger;
            }
        }
    }
}
