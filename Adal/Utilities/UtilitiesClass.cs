using AutoMapper;
using Core.CoreClass;
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

    public class SessionUtilities
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionUtilities(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddDetailsInSessions(Users users)
        {
            _httpContextAccessor.HttpContext.Session.SetInt32("Id", users.Id);
            _httpContextAccessor.HttpContext.Session.SetString("Email", users.Email);
            _httpContextAccessor.HttpContext.Session.SetString("UserName", users.FirstName +" "+ users.LastName);
            _httpContextAccessor.HttpContext.Session.SetString("UserRole", (users.UserRoleId == 1 ? "Admin" : users.UserRoleId == 2 ? "Lawyer" :"Client"));
        }
    }
}
