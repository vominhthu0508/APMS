using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using XT.BusinessService;
using XT.Model;
using XT.Repository;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using XT.Web.External;
using XT.Web.Controllers;
using System.Linq.Expressions;

namespace XT.Web
{
    public class LangId
    {
        public LangId()
        {
        }
        public int Lang_Id
        {
            get
            {
                //if (HttpContext.Current.Session == null)
                //{
                //    return (Int32)LanguageEnum.vi;
                //}
                //else
                //{
                //    var result = HttpContext.Current.Session["Lang_Id"] ?? (Int32)LanguageEnum.vi;
                //    return Convert.ToInt32(result);
                //}
                return XT.Web.External.CultureHelper.Lang_Id;
            }
        }
        public int Center_Id
        {
            get
            {
                if (HttpContext.Current.Session == null)
                {
                    return 0;
                }
                else
                {
                    return AuthenticationManager.Company_Id;
                }
                //return XT.Web.External.CultureHelper.Center_Id;
            }
        }
    }
    public class IoCConfig
    {
        private static Container _container;
        public static void Register()
        {
            // 1. Create a new Simple Injector container
            var container = new Container();
            var webLifestyle = new WebRequestLifestyle();

            // 2. Configure the container (register)
            container.RegisterPerWebRequest<MyDataContext>(
                () => new MyDataContext(
                    ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString
                    ));
            container.RegisterPerWebRequest<LangId>(
                () => new LangId());
            container.RegisterPerWebRequest<IUow>(
                () => new Uow(container.GetInstance<MyDataContext>()
                    , container.GetInstance<LangId>().Lang_Id
                    , container.GetInstance<LangId>().Center_Id));
            //container.RegisterPerWebRequest<IUow>(
            //    () => new Uow(container.GetInstance<MyDataContext>()));
            //container.Register<IUow>(
            //    () => new Uow(container.GetInstance<MyDataContext>()), webLifestyle);

            var assembly = typeof(IService).Assembly;

            var registrations =
                from type in assembly.GetExportedTypes()
                where type.GetInterfaces().Contains(typeof(IService)) && type.IsClass && !type.IsAbstract//CityService
                select new { Service = type.GetInterfaces().Where(i => i.Name.Equals("I" + type.Name)).First(), Implementation = type };

            foreach (var reg in registrations)
            {
                //reg.Service: ICityService
                //reg.Implementation: CityService
                //container.RegisterSingle(reg.Service, reg.Implementation);

                //container.Register(reg.Service, reg.Implementation, webLifestyle);

                if (!reg.Implementation.Name.StartsWith("Service"))
                {
                    container.Register(reg.Service
                        , () => Activator.CreateInstance(reg.Implementation, container.GetInstance<IUow>())
                        , webLifestyle);
                }
            }

            // 3. Optionally verify the container's configuration.
            container.Verify();

            // 4. Store the container for use by Page classes.
            _container = container;
        }

        public static TService GetInstance<TService>() where TService : class
        {
            return _container.GetInstance<TService>();
        }

        public static IService<U, V> ServiceNonregistered<U, V>() where U : class, IEntity<V>
        {
            return new Service<U, V>(GetInstance<XT.Repository.IUow>());
        }

        public static object ServiceNonregisteredObject(string modelName)
        {
            var t = GetTypeModelName(modelName);
            //var service = new Service<U, V>(GetInstance<XT.Repository.IUow>());
            return Activator.CreateInstance(
                                typeof(Service<,>).MakeGenericType(t, typeof(int)), GetInstance<XT.Repository.IUow>());
        }

        public static TService Service<TService>() where TService : class
        {
            return GetInstance<TService>();
        }

        public static object ServiceObject(string modelName)
        {
            //var assembly = typeof(IService).Assembly;
            //var serviceName = modelName + "Service";
            //var service = assembly.GetExportedTypes().Where(a => a.IsClass && !a.IsAbstract && a.Name.Equals(serviceName)).FirstOrDefault();
            var service = GetServiceModelName(modelName);
            if (service != null)
            {
                return _container.GetInstance(service);
            }

            return null;
        }

        public static object ServiceObject<T>() where T : class, IEntity<Int32>
        {
            var type = typeof(T);

            return ServiceObject(type.Name);
        }

        public static IService<T, Int32> ServiceBase<T>() where T : class, IEntity<Int32>
        {
            var type = typeof(T);
            return ServiceObject(type.Name) as IService<T, Int32>;
        }

        public static dynamic ServiceDynamic(string modelName)
        {
            //var assembly = typeof(IService).Assembly;
            //var serviceName = modelName + "Service";
            //var service = assembly.GetExportedTypes().Where(a => a.IsClass && !a.IsAbstract && a.Name.Equals(serviceName)).FirstOrDefault();
            var service = GetServiceModelName(modelName);
            if (service != null)
            {
                return _container.GetInstance(service);
            }

            return null;
        }

        #region Invoke
        public static string GetVariableName<T>(Expression<Func<T>> expr)
        {
            var body = ((MemberExpression)expr.Body);

            return body.Member.Name;
        }

        public static string GetVariableName(object obj)
        {
            return GetVariableName(() => obj);
        }

        public static Type GetTypeModelName(string modelName)
        {
            var modelAssemply = typeof(Account).Assembly;
            var assemblyQualifiedName = modelAssemply.GetName().Name + "." + modelName;
            return modelAssemply.GetExportedTypes().Where(t => t.Name == modelName).FirstOrDefault();
        }

        public static Type GetServiceModelName(string modelName)
        {
            var assembly = typeof(IService).Assembly;
            var serviceName = modelName + "Service";
            var service = assembly.GetExportedTypes().Where(a => a.IsClass && !a.IsAbstract && a.Name.Equals(serviceName)).FirstOrDefault();
            return service;
        }

        public static Type GetIServiceModelName(string modelName)
        {
            var assembly = typeof(IService).Assembly;
            var serviceName = "I" + modelName + "Service";
            var service = assembly.GetExportedTypes().Where(a => a.Name.Equals(serviceName)).FirstOrDefault();
            return service;
        }

        public static object InvokeService(object service, string method_name, object[] parameters)
        {
            var method = service.GetType().GetMethod(method_name);
            var item = method.Invoke(service, parameters);

            return item;
        }

        public static object Invoke(string entity, string method_name, object[] parameters)
        {
            var service = ServiceObject(entity);
            return InvokeService(service, method_name, parameters);
        }

        public static object FindById(string entity, int id)
        {
            return Invoke(entity, "FindById", new object[] { id });
        }

        public static object Update(string entity, object item)
        {
            return Invoke(entity, "Update", new object[] { item });
        }

        public static dynamic Invoke_EntityManagementServiceByName(string modelName)
        {
            return Activator.CreateInstance(typeof(EntityManagementService<,>).MakeGenericType(
                                                    GetTypeModelName(modelName), GetIServiceModelName(modelName)));
        }

        public static dynamic Invoke_EntityManagementService(IEntity model)
        {
            var type = model.GetType();
            if (!type.Namespace.Contains("XT.Model"))
                type = type.BaseType;
            var modelName = type.Name;
            if (modelName.Contains("Model"))
                modelName = modelName.Replace("Model", "");

            return Invoke_EntityManagementServiceByName(modelName);
        }
        #endregion Invoke
    }
}