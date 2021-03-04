using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyIdp.Services
{
    public interface ISmsService
    {
        Task<bool> ValidatePhoneNumAsync(string phone, string code);
    }
}
