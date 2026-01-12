using System.Data;
using System.Reflection;

namespace Arihant.Services
{
    public static class EntityMapper
    {
        public static EnumerableRowCollection<TSource> Map<TSource>(this DataTableCollection input) where TSource : new()
        {
            return input[0]
                .AsEnumerable()
                .Select(item => MapProperties<TSource>(item));
        }
        private static TSource MapProperties<TSource>(DataRow source)
        {

            Type type = typeof(TSource);
            PropertyInfo[] properties = type.GetProperties();

            TSource result = Activator.CreateInstance<TSource>();

            foreach (var property in properties)
            {
                if (source.Table.Columns.Contains(property.Name))
                {
                    var SourceValue = source[property.Name];

                    if (property.PropertyType == SourceValue.GetType())
                    {
                        property.SetValue(result, SourceValue);
                    }
                }
            }

            return result;
        }
    }
}
