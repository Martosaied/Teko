using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Teko.Web.Extensions
{
    public class GlobalEnums
    {
        static readonly Dictionary<int, string> Budget = new Dictionary<int, string>
         {
            {1, "fa fa-file-o"},
            {2, "fa fa-file-word-o"},
            {3, "fa fa-file-excel-o"},
            {4, "fa fa-file-powerpoint-o"},
            {5, "fa fa-file-pdf-o"}
        };

    }
}