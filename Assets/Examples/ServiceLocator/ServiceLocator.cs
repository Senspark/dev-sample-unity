using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Examples.ServiceLocator {
    public static class ServiceLocator {
        private static readonly Dictionary<string, object> Services = new();
        private static readonly Dictionary<Type, string> ServiceNames = new();

        [NotNull]
        private static string GetServiceName([NotNull] Type type) {
            if (ServiceNames.TryGetValue(type, out var result)) {
                return result;
            }
            var interfaces = type.GetInterfaces().ToList();
            if (type.IsInterface) {
                interfaces.Add(type);
            }
            foreach (var item in interfaces) {
                var attribute = Attribute.GetCustomAttribute(item, typeof(ServiceAttribute));
                if (attribute is ServiceAttribute serviceAttribute) {
                    var name = serviceAttribute.Name;
                    ServiceNames.Add(type, name);
                    return name;
                }
            }
            throw new Exception($"The requested service is not registered: {type.Name}");
        }

        public static void Provide<T>(T service) {
            var type = service.GetType();
            var name = GetServiceName(type);
            Services.Remove(name);
            Services.Add(name, service);
        }

        public static T Resolve<T>() {
            return (T) Resolve(typeof(T));
        }

        [NotNull]
        private static object Resolve([NotNull] Type type) {
            var name = GetServiceName(type);
            if (Services.TryGetValue(name, out var item)) {
                return item;
            }
            throw new Exception($"Cannot find the requested service: {name}");
        }
    }
}