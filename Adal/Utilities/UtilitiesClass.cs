using AutoMapper;
using System.Linq;

namespace Adal.Utilities
{
    public class UtilitiesClass<TSource, TDestination>
    {
        private MapperConfiguration _config;

        public UtilitiesClass()
        {
            _config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });
        }

        public void CreateMap()
        {
            _config.CreateMapper().ConfigurationProvider.AssertConfigurationIsValid();
        }

        public TDestination Map(TSource source)
        {
            var mapper = _config.CreateMapper();
            return mapper.Map<TSource, TDestination>(source);
        }

        //public void CreateMapWithAutoProperties()
        //{
        //    _config = new MapperConfiguration(cfg =>
        //    {
        //        var map = cfg.CreateMap<TSource, TDestination>();

        //        // Include all properties from the source object
        //        foreach (var property in typeof(TSource).GetProperties())
        //        {
        //            map.ForMember(property.Name, opt => opt.MapFrom(src => src.GetType().GetProperty(property.Name).GetValue(src)));
        //        }
        //    });
        //}
    }
}
