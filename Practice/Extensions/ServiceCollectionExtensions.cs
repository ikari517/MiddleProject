using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
// 복사시 네임스페이스 확인 필요
namespace Practice.Extensions
{
    /// <summary>
    /// service 확장메서드
    /// </summary>
    static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Services에 ViewModel 추가
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            var viewModelTypes = Assembly.GetExecutingAssembly()
                                         .GetTypes()
                                         .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("ViewModel"));

            foreach (var viewModelType in viewModelTypes)
            {
                services.AddSingleton(viewModelType);
            }

            return services;
        }

        /// <summary>
        /// Services에 View 추가
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            var viewlTypes = Assembly.GetExecutingAssembly()
                                         .GetTypes()
                                         .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("View"));

            foreach (var viewType in viewlTypes)
            {
                services.AddTransient(viewType);
            }
            // 명칭 예외 view
            services.AddTransient(typeof(MainWindow));

            return services;
        }
    }
}
