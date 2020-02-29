using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenderApplication.Interface
{
    interface ICSharpPythonRESTfulAPI
    {
        bool CSharpPythonRestfulApiSimpleTest(object input_Data, out dynamic _Result,out string exceptionMessage);
    }
}
