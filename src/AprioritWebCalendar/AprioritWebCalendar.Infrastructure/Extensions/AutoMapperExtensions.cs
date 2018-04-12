using AutoMapper;

namespace AprioritWebCalendar.Infrastructure.Extensions
{
    public static class AutoMapperExtensions
    {
        public static void MapToEntity<TSource, TDest>(this IMapper mapper, TSource source, TDest destination)
        {
            Mapper.Map(source, destination, typeof(TSource), typeof(TDest));
        }
    }
}
