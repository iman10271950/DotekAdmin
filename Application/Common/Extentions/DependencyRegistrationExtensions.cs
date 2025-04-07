
//using Microsoft.Azure.WebJobs;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.Common.Extentions
//{
//    public static class DependencyRegistrationExtensions
//    {
//        public static IServiceCollection AddImplementations(this IServiceCollection services)
//    {
//        IEnumerable<Assembly> source = AppDomain.CurrentDomain.GetAssemblies().Concat(AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly a) => a.GetReferencedAssemblies().Select(Assembly.Load)));
//        foreach (Assembly item in source.Distinct())
//        {
//            Type[] array;
//            try
//            {
//                array = item.GetTypes();
//            }
//            catch (ReflectionTypeLoadException ex)
//            {
//                array = ex.Types.Where((Type t) => t != null).ToArray();
//            }

//            array = (from t in array.Concat(array.SelectMany((Type t) => t.GetNestedTypes(BindingFlags.NonPublic)))
//                     where t.GetCustomAttributes(typeof(ScopedAttribute), inherit: true).Length != 0 || t.GetCustomAttributes(typeof(SingletonAttribute), inherit: true).Length != 0 || t.GetCustomAttributes(typeof(TransientAttribute), inherit: true).Length != 0
//                     select t).ToArray();
//            Type[] array2 = array;
//            foreach (Type type in array2)
//            {
//                Type[] interfaces = type.GetInterfaces();
//                Type[] array3 = interfaces;
//                foreach (Type serviceType in array3)
//                {
//                    if (type.GetCustomAttributes(typeof(ScopedAttribute), inherit: true).Length != 0)
//                    {
//                        services.AddScoped(serviceType, type);
//                    }
//                    else if (type.GetCustomAttributes(typeof(SingletonAttribute), inherit: true).Length != 0)
//                    {
//                        services.AddSingleton(serviceType, type);
//                    }
//                    else if (type.GetCustomAttributes(typeof(TransientAttribute), inherit: true).Length != 0)
//                    {
//                        services.AddTransient(serviceType, type);
//                    }
//                }
//            }
//        }

//        return services;
//    }
//}
//}
