using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateTDD.Tests
{
    public interface IAmConvention
    {
       void ProcessMapper(ConventionModelMapper mapper);
    }
}
