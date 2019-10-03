using System;
using System.Collections.Generic;

namespace MoneyStats.DAL.Models
{
    public partial class Setting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MergedFileFolderPath { get; set; }
        public string MergedFilename { get; set; }
        public string OriginalFileFolderPath { get; set; }
        public string OriginalFileExtensionPattern { get; set; }
        public string DbexportFolderPath { get; set; }
        public string DbexportFilename { get; set; }
        public string DbimportFilePath { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string State { get; set; }

        public virtual User User { get; set; }
    }
}
