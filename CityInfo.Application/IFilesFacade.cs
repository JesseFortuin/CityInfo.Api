using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CItyInfo.Application
{
    public interface IFilesFacade
    {
        ActionResult GetFile(string fileId);
    }
}
