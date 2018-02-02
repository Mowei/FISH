using System;
using System.Linq;
using System.Reflection;

namespace FISH.Utility
{
    public class ViewModelMapping<TDestinate> where TDestinate : class
    {
        public TDestinate Mapping<TSource>(TSource source)
        {
            if (source == null)
            {
                return null;
            }
            var sourProperties = source.GetType().GetProperties();
            var destinate = (TDestinate)Activator.CreateInstance(typeof(TDestinate));
            var destProperties = destinate.GetType().GetProperties();

            foreach (var item in sourProperties)
            {
                var dest = destProperties.FirstOrDefault(a => a.Name == item.Name);
                if (dest != null)
                {
                    dest.SetValue(destinate, item.GetValue(source, null));
                }
            }

            return destinate;
        }
    }
}