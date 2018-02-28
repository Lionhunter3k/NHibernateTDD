using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace NHibernateTDD.Tests.Conventions
{
    public abstract class AbstractRelationConvention : IAmConvention
    {
        protected PluralizationService service = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));

        public PluralizationService Service
        {
            get
            {
                return this.service;
            }
            set
            {
                if (value == null)
                    throw new NullReferenceException("service");
                this.service = value;
            }
        }

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
                this.entities = baseEntityType.Assembly.GetExportedTypes().Where(t => t.IsSubclassOf(this.baseEntityType)).Where(typeFilter).ToList();
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
                this.entities = baseEntityType.Assembly.GetExportedTypes().Where(t => t.IsSubclassOf(this.baseEntityType)).Where(typeFilter).ToList();

            }
        }

        protected static bool isSelfReferencingObject(PropertyInfo property, PropertyInfo inverseProperty)
        {
            return property.PropertyType.FullName == inverseProperty.PropertyType.FullName;
        }

        //this method check the property of an object to see if it's a collection type, then if it's a collection type, gets the generic collection type, then create a concrete impl of that generic collection
        //using the declaring class type, then checks the object used for the collection type to see if it has a property which implements the collection of declaring class type
        protected abstract PropertyInfo GetInverseProperty(PropertyInfo property);

        protected abstract PropertyInfo GetInverseProperty(MemberInfo member);

        public void ProcessMapper(NHibernate.Mapping.ByCode.ConventionModelMapper mapper)
        {
            var mappedItemsCache = new List<string>();
            foreach (var entity in entities)
            {
                var properties = entity.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    this.ShouldProcess = false;
                    var propertyTypeToCheck = property.PropertyType;
                    if (propertyTypeToCheck.DetermineCollectionElementOrDictionaryValueType() != null)
                    {
                        propertyTypeToCheck = propertyTypeToCheck.GetGenericArguments()[0];
                    }
                    if (this.BaseEntityType != null && !propertyTypeToCheck.IsSubclassOf(this.BaseEntityType))
                        continue;
                    if (!this.entities.Contains(propertyTypeToCheck))
                        continue;
                    var inverseProperty = GetInverseProperty(property);//should return inverse property if conditions are met or return null otherwise
                    if ((inverseProperty != null && mappedItemsCache.Contains(inverseProperty.PropertyType.FullName + inverseProperty.Name + inverseProperty.DeclaringType.FullName)) || mappedItemsCache.Contains(property.PropertyType.FullName + property.Name + property.DeclaringType.FullName))
                        continue;
                    if (ShouldProcess)
                    {
                        CallGenericRelationMethod(mapper, property, inverseProperty);
                        if (inverseProperty != null)
                            mappedItemsCache.Add(inverseProperty.PropertyType.FullName + inverseProperty.Name + inverseProperty.DeclaringType.FullName);
                        mappedItemsCache.Add(property.PropertyType.FullName + property.Name + property.DeclaringType.FullName);
                    }
                }
            }
        }

        protected abstract void CallGenericRelationMethod(NHibernate.Mapping.ByCode.ConventionModelMapper mapper, PropertyInfo property, PropertyInfo inverseProperty);

        protected bool ShouldProcess = false;
    }
}
