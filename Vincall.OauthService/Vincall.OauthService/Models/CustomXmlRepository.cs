using Microsoft.AspNetCore.DataProtection.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Vincall.OauthService.Models
{
    public class CustomXmlRepository : IXmlRepository
    {
        public IReadOnlyCollection<XElement> GetAllElements()
        {
            throw new NotImplementedException();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            throw new NotImplementedException();
        }
    }
}
