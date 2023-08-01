using FerrumCapital.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FerrumCapital.Domain.Entities
{
    public class Product: BaseAuditableEntity
    {
        public string Name { get; set; }
        //[Required]
        //[DataType(DataType.Upload)]
        //[MaxFileSize(5 * 1024 * 1024)] // Maximum file size (e.g., 5 MB)
        //[AllowedExtensions(new string[] { ".txt", ".pdf", ".docx" })]
        public string FilePath { get; set; }
    }
}
