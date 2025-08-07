using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Syncfusion.XlsIO;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using IWorkbook = Syncfusion.XlsIO.IWorkbook;
using IWorksheet = Syncfusion.XlsIO.IWorksheet;

namespace WeightScaleGen2.BGC.Web.Common
{
    public interface IExcelUtilitiesCommon
    {
        Task<DataTable> ConvertExcelToDataTable(string currentDir, string wwwrootPath, IFormFile excelFile, bool keepFile = false);
        DataTable ConvertExcelToDataTable(string filePath, bool keepFile = false);
    }

    public class ExcelUtilitiesCommon : IExcelUtilitiesCommon
    {
        public const string defaultFileExtension = ".jpg";

        public string CombineFileNameAndExtension(string fileName, string fileExtension)
        {
            try
            {
                if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(fileExtension))
                {
                    if (string.IsNullOrEmpty(Path.GetExtension(fileName)))
                    {
                        fileExtension = fileExtension.Trim();

                        if (fileExtension.IndexOf(".") < 0)
                            fileExtension = "." + fileExtension;

                        fileName += fileExtension.ToLower();
                    }
                    return fileName.Trim();
                }
                else
                {
                    throw new Exception("File name or file extension has empty.");
                }
            }
            catch (Exception)
            {
                throw new Exception("File name or file extension has empty.");
            }
        }

        public string GetUniqueFileName(string fileName, string dateFormat = "yyyyMMddHHmmssfff")
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(fileName) && Path.HasExtension(fileName))
                {
                    string prefixFileName = DateTime.Now.ToString(dateFormat);
                    string fileExtension = (Path.HasExtension(fileName) ? Path.GetExtension(fileName) : defaultFileExtension);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    string uniqueFileName = string.Format("{0}_{1}", prefixFileName, fileNameWithoutExtension.Replace(" ", "_").Replace(".", "_"));
                    result = this.CombineFileNameAndExtension(fileName: uniqueFileName, fileExtension: fileExtension);
                }
                return result;
            }
            catch (Exception)
            {
                throw new Exception("File name or file extension has empty.");
            }
        }

        public async Task<DataTable> ConvertExcelToDataTable(string currentDir, string wwwrootPath, IFormFile excelFile, bool keepFile = false)
        {
            DataTable dt = new DataTable();
            try
            {
                var filename = this.GetUniqueFileName(fileName: excelFile.FileName);
                var MainPath = Path.Combine(currentDir, "wwwroot", wwwrootPath);

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var enc1252 = Encoding.GetEncoding(1252);

                if (!Directory.Exists(MainPath))
                {
                    Directory.CreateDirectory(MainPath);
                }

                var filePath = Path.Combine(MainPath, filename);
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    await excelFile.CopyToAsync(stream);
                }

                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        dt = reader.AsDataSet().Tables[0];
                    }
                }

                if (keepFile == false)
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception) { throw new Exception("File name or file extension has empty."); }
            return dt;
        }

        public DataTable ConvertExcelToDataTable(string filePath, bool keepFile = false)
        {
            DataTable dt = new DataTable();
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (ExcelEngine excelEngine = new ExcelEngine())
                    {
                        IApplication application = excelEngine.Excel;
                        IWorkbook workbook = application.Workbooks.Open(stream);
                        IWorksheet worksheet = workbook.Worksheets[0];

                        DataTable dataTable = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
                        dt = dataTable;
                    }
                }

                if (keepFile == false)
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception) { throw new Exception("File name or file extension has empty."); }
            return dt;
        }
    }
}
