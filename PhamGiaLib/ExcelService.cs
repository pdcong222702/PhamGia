using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Asn1.Ocsp;
using Syncfusion.XlsIO;
using System.Data;
using IWorkbook = Syncfusion.XlsIO.IWorkbook;

namespace PhamGia.PhamGiaLib
{
    public  class ExcelService
    {



        public MemoryStream createExcel(DataTable table)
        {
            //Create an instance of ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2016;

                //Create a workbook
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];

                //Initialize DataTable and data get from SampleDataTable method

                //Import data from DataTable
                worksheet.ImportDataTable(table, true, 1, 1, true);

                //Save the document as a stream and retrun the stream.
                using (MemoryStream stream = new MemoryStream())
                {
                    //Save the created Excel document to MemoryStream
                    workbook.SaveAs(stream);
                    return stream;
                }
            }
        }

        private DataTable SampleDataTable()
        {
            //Here we create three columns
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Item", typeof(string));
            table.Columns.Add("Name", typeof(string));

            //Here we add four DataRows
            table.Rows.Add(1, "Soap", "David");
            table.Rows.Add(2, "Paste", "Sam");
            table.Rows.Add(3, "Cream", "Christoff");
            table.Rows.Add(4, "Powder", "Janet");

            return table;
        }
    }
}
