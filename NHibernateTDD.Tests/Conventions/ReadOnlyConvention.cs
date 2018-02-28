using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;

namespace NHibernateTDD.Tests.Conventions
{
    public class ReadOnlyConvention : IAmConvention
    {
      
        protected ICollection<Type> entities = new HashSet<Type>();

        public IEnumerable<Type> Entities
        {
            get
            {
                return this.entities;
            }
            set
            {
                if (value == null)
                    throw new NullReferenceException("Entities");
                else
                    this.entities = new HashSet<Type>(value);
            }
        }

        private Type baseEntityType = null;

        public Type BaseEntityType
        {
            get
            {
                return this.baseEntityType;
            }
            set
            {
                this.entities.Clear();
                this.baseEntityType = value;
                this.entities = baseEntityType.Assembly.GetExportedTypes().Where(typeFilter).ToList();
            }
        }

        private Func<Type, bool> typeFilter = t => true;

        public Func<Type, bool> TypeFilter
        {
            get
            {
                return this.typeFilter;
            }
            set
            {
                this.entities.Clear();
                this.typeFilter = value;
                this.entities = baseEntityType.Assembly.GetExportedTypes().Where(typeFilter).ToList();

            }
        }

        public static void MapReadOnly<TControllingEntity, TReadOnlyProperty>(
         ModelMapper mapper, PropertyInfo property
        )
            where TControllingEntity : class
        {
            var controllingPropertyName = property.PropertyType.Name;
            var controllingColumnName = string.Format("{0}Id", controllingPropertyName);
            mapper.Class<TControllingEntity>(map => map.Property(property.Name, m => m.Access(Accessor.ReadOnly)));

        }

        public void ProcessMapper(NHibernate.Mapping.ByCode.ConventionModelMapper mapper)
        {
            foreach (var entity in entities)
            {
                var properties = entity.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    var propertyTypeToCheck = property.PropertyType;
                    if (propertyTypeToCheck.DetermineCollectionElementOrDictionaryValueType() != null)
                        continue;
                    if (entities.Contains(propertyTypeToCheck))
                        continue;
                    if (property.CanWrite == false)
                    {
                        var method = typeof(ReadOnlyConvention).GetMethod("MapReadOnly", BindingFlags.Public | BindingFlags.Static)
                                                 .MakeGenericMethod(new[] { property.DeclaringType, property.PropertyType });
                        method.Invoke(null, new object[] { mapper, property });
                    }
                }
            }


        }
    }
}
