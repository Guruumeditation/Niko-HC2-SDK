using System;
using System.Collections.Generic;
using System.Text;

namespace HC2.Arcanastudio.Net.Models.Interfaces
{
    public interface IError
    {
        string ErrCode { get; }
        string ErrMessage { get; }
    }
}
