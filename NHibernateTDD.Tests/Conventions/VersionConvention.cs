using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateTDD.Tests.Conventions
{
    public class VersionConvention : IAmConvention
    {
        public void ProcessMapper(NHibernate.Mapping.ByCode.ConventionModelMapper mapper)
        {
            mapper.Class<Entity>(map =>
            {
                map.Version("Version",m=>m.Generated(VersionGeneration.Never));
            });
        }
    }
}
