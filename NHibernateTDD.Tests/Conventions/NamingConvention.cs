using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateTDD.Tests.Conventions
{
    public class NamingConvention : IAmConvention
    {
        private PluralizationService service = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en"));

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

        public void MakeNotLazy(IModelInspector modelInspector, Type type, IClassAttributesMapper classCustomizer)
        {

        }

        public void ProcessMapper(NHibernate.Mapping.ByCode.ConventionModelMapper mapper)
        {
            mapper.BeforeMapClass += (modelInspector, type, map) =>
            {
                PluralizeEntityName(type, map);
                PrimaryKeyConvention(type, map);
            };
            mapper.BeforeMapClass += MakeNotLazy;
            mapper.BeforeMapManyToOne += ReferenceConvention;
            mapper.BeforeMapBag += OneToManyConvention;
            mapper.BeforeMapManyToMany += ManyToManyConvention;
            mapper.BeforeMapJoinedSubclass += MapJoinedSubclass;
            mapper.BeforeMapAny += MapAny;
            mapper.BeforeMapComponent += MapComponent;
            mapper.BeforeMapElement += MapElement;
            mapper.BeforeMapIdBag += MapIdBag;
            mapper.BeforeMapList += MapList;
            mapper.BeforeMapMap += MapMap;
            mapper.BeforeMapMapKey += MapMapKey;
            mapper.BeforeMapMapKeyManyToMany += MapMapKeyManyToMany;
            mapper.BeforeMapOneToMany += MapOneToMany;
            mapper.BeforeMapOneToOne += MapOneToOne;
            mapper.BeforeMapProperty += MapProperty;
            mapper.BeforeMapSet += MapSet;
            mapper.BeforeMapSubclass += MapSubclass;
            mapper.BeforeMapUnionSubclass += MapUnionSubclass;
            mapper.BeforeMapProperty += ReadOnlyConvention;
        }

        public void ReadOnlyConvention(IModelInspector modelInspector, PropertyPath member, IPropertyMapper propertyCustomizer)
        {
            if (member.LocalMember is PropertyInfo)
            {
                var propInfo = (member.LocalMember as PropertyInfo);
                if (propInfo.CanWrite == false)
                {
                    propertyCustomizer.Access(Accessor.ReadOnly);
                }
            }
        }

        public void ManyToManyConvention(IModelInspector modelInspector, PropertyPath member, IManyToManyMapper map)
        {
            map.ForeignKey(
                string.Format("fk_{0}_{1}",
                       member.LocalMember.Name,
                       member.GetContainerEntity(modelInspector).Name));
        }

        public void MapAny(IModelInspector modelInspector, PropertyPath member, IAnyMapper map)
        {
        }

        public void MapComponent(IModelInspector modelInspector, PropertyPath member, IComponentAttributesMapper map)
        {
        }

        public void MapElement(IModelInspector modelInspector, PropertyPath member, IElementMapper map)
        {
        }

        public void MapIdBag(IModelInspector modelInspector, PropertyPath member, IIdBagPropertiesMapper map)
        {
        }

        public void MapJoinedSubclass(IModelInspector modelInspector, Type type, IJoinedSubclassAttributesMapper map)
        {
            map.Table(Service.Pluralize(type.Name));
            map.Key(x =>
            {
                x.ForeignKey(string.Format("fk_{0}_{1}",
                                                type.BaseType.Name,
                                                type.Name));
                x.Column(type.Name + "Id");
            });
        }

        public void MapList(IModelInspector modelInspector, PropertyPath member, IListPropertiesMapper map)
        {
        }

        public void MapMap(IModelInspector modelInspector, PropertyPath member, IMapPropertiesMapper map)
        {
        }

        public void MapMapKey(IModelInspector modelInspector, PropertyPath member, IMapKeyMapper map)
        {
        }

        public void MapMapKeyManyToMany(IModelInspector modelInspector, PropertyPath member, IMapKeyManyToManyMapper map)
        {
        }

        public void MapOneToMany(IModelInspector modelInspector, PropertyPath member, IOneToManyMapper map)
        {
        }

        public void MapOneToOne(IModelInspector modelInspector, PropertyPath member, IOneToOneMapper map)
        {
        }

        public void MapProperty(IModelInspector modelInspector, PropertyPath member, IPropertyMapper map)
        {
            ComponentNamingConvention(modelInspector, member, map);
        }

        public void ComponentNamingConvention(IModelInspector modelInspector, PropertyPath member, IPropertyMapper map)
        {
            var property = member.LocalMember as PropertyInfo;
            if (modelInspector.IsComponent(property.DeclaringType))
            {
                map.Column(member.PreviousPath.LocalMember.Name + member.LocalMember.Name);
            }
        }

        public void MapSet(IModelInspector modelInspector, PropertyPath member, ISetPropertiesMapper map)
        {
        }

        public void MapSubclass(IModelInspector modelInspector, Type type, ISubclassAttributesMapper map)
        {
        }

        public void MapUnionSubclass(IModelInspector modelInspector, Type type, IUnionSubclassAttributesMapper map)
        {
        }

        public void OneToManyConvention(IModelInspector modelInspector, PropertyPath member, IBagPropertiesMapper map)
        {
            var inv = GetInverseProperty(member.LocalMember);
            if (inv == null)
            {
                map.Key(x => x.Column(member.GetContainerEntity(modelInspector).Name + "Id"));
                map.Cascade(Cascade.All | Cascade.DeleteOrphans);
                map.BatchSize(20);
                map.Inverse(true);
            }
        }

        public void PluralizeEntityName(Type type, IClassAttributesMapper map)
        {
            map.Table(service.Pluralize(type.Name));
        }

        public void PrimaryKeyConvention(Type type, IClassAttributesMapper map)
        {
            map.Id(k =>
            {
                k.Generator(Generators.Native);
                k.Column("Id");
            });
        }

        public void ReferenceConvention(IModelInspector modelInspector, PropertyPath member, IManyToOneMapper map)
        {
            map.Column(k => k.Name(member.LocalMember.GetPropertyOrFieldType().Name + "Id"));
            map.ForeignKey(
                string.Format("fk_{0}_{1}",
                       member.LocalMember.Name,
                       member.GetContainerEntity(modelInspector).Name));
            map.Cascade(Cascade.All | Cascade.DeleteOrphans);
        }

        public PropertyInfo GetInverseProperty(MemberInfo member)
        {
            var type = member.GetPropertyOrFieldType();
            var to = type.DetermineCollectionElementOrDictionaryValueType();
            if (to == null)
            {
                // no generic collection or simple property
                return null;
            }

            var expectedInversePropertyType = type.GetGenericTypeDefinition()
                                                  .MakeGenericType(member.DeclaringType);

            var argument = type.GetGenericArguments()[0];
            return argument.GetProperties().FirstOrDefault(x => x.PropertyType == expectedInversePropertyType);
        }
    }
}
