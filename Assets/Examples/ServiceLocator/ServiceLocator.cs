using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

namespace Examples.ServiceLocator {
    public static class ServiceLocator {
        private static readonly Dictionary<string, object> Services = new();
        private static readonly Dictionary<Type, string> ServiceNames = new();
        
        // ReSharper disable once InconsistentNaming
        private static readonly Dictionary<Type, List<MemberInfo>> _declaredInjectableMembers = new();
        // ReSharper disable once InconsistentNaming
        private static readonly Dictionary<Type, List<MemberInfo>> _allInjectableMembers = new();

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
        
        public static void ResolveInjection<T>(T value) {
            var members = GetAllInjectableMembers(value.GetType());
            foreach (var member in members) {
                switch (member) {
                    case PropertyInfo property:
                        property.SetValue(value, Resolve(property.PropertyType));
                        break;
                    case FieldInfo field:
                        field.SetValue(value, Resolve(field.FieldType));
                        break;
                }
            }
        }

        private static List<MemberInfo> GetAllInjectableMembers(Type runtimeType) {
            if (_allInjectableMembers.TryGetValue(runtimeType, out var cached)) {
                Debug.Log($"ServiceLocator: Resolve from cache lv1: {runtimeType.Name}");
                return cached;
            }
            var members = new List<MemberInfo>();
            var type = runtimeType;
            while (type != null && type != typeof(UnityEngine.MonoBehaviour)) {
                members.AddRange(GetDeclaredInjectableMembers(type));
                type = type.BaseType;
            }
            _allInjectableMembers.Add(runtimeType, members);
            return members;
        }

        private static List<MemberInfo> GetDeclaredInjectableMembers(Type type) {
            if (_declaredInjectableMembers.TryGetValue(type, out var cached)) {
                Debug.Log($"ServiceLocator: Resolve from cache lv2: {type.Name}");
                return cached;
            }
            var members = new List<MemberInfo>();
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public
                | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            foreach (var property in type.GetProperties(flags)) {
                if (property.GetCustomAttributes(typeof(InjectAttribute), true).Length > 0) {
                    members.Add(property);
                }
            }
            foreach (var field in type.GetFields(flags)) {
                if (field.GetCustomAttributes(typeof(InjectAttribute), true).Length > 0) {
                    members.Add(field);
                }
            }
            _declaredInjectableMembers.Add(type, members);
            return members;
        }
    }
}