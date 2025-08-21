using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WeightScaleGen2.BGC.Models.ViewModels.Base
{
    public class BaseUpload
    {
        public string file_desc { get; set; }

        public IFormFile file_upload { get; set; }

    }
    public class UploadVm
    {
        public string Type { set; get; }
        public string Id { set; get; }
        public IList<IFormFile> Files { get; set; }
    }

    public class ImgObj
    {
        #region Properties  

        /// <summary>  
        /// Gets or sets Image ID.  
        /// </summary>  
        public int FileId { get; set; }

        /// <summary>  
        /// Gets or sets Image name.  
        /// </summary>  
        public string FileName { get; set; }

        /// <summary>  
        /// Gets or sets Image extension.  
        /// </summary>  
        public string FileContentType { get; set; }

        #endregion
    }
    public class FileUpload
    {
        #region Properties  

        /// <summary>  
        /// Gets or sets Image file.  
        /// </summary>
        public List<IFormFile> file { get; set; }

        /// <summary>  
        /// Gets or sets Image file list.  
        /// </summary>  
        public string file_name { get; set; }

        #endregion
    }
}
