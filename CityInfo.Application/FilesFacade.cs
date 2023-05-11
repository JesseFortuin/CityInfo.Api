using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CItyInfo.Application
{
    public class FilesFacade :  IFilesFacade
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesFacade(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentException(nameof
                (fileExtensionContentTypeProvider));
        }
        public ActionResult GetFile(string fileId)
        {
            throw new NotImplementedException();
        }
    }
}
