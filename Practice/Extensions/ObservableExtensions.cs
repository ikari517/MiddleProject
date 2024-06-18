using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Practice.Extensions
{
    public static class ObservableExtensions
    {
        /// <summary>
        /// JSON 직렬화/역직렬화 DeepCopy
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T source) where T : ObservableObject
        {
            var options = new JsonSerializerOptions { WriteIndented = false };
            var json = JsonSerializer.Serialize(source, options);

            return JsonSerializer.Deserialize<T>(json, options);
        }

        /// <summary>
        /// PK(KeyAttribute)를 제외한 속성을 복사
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void PropertyCopy<T>(this T source, T target) where T : ObservableObject
        {
            if (source == null || target == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // PK 제외
                if(property.GetCustomAttributes().Any(attribute => attribute is KeyAttribute)) continue;
                
                if (property.CanRead && property.CanWrite)
                {
                    var value = property.GetValue(target, null);
                    property.SetValue(source, value, null);
                }
            }
        }
    }
}
