namespace PhamGia.Common.Interface
{
    public interface ISerilogProvider
    {
        /// <summary>
        /// serilog instance (singleton)
        /// </summary>
        ILogger Logger { get; }
    }
}
