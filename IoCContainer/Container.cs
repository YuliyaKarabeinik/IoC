using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IoCContainer.Attributes;

namespace IoCContainer
{
    public class Container
    {
        private readonly IDictionary<Type, Type> types = new Dictionary<Type, Type>();
        
        public void AddAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (HasCustomAttribute<ImportConstructorAttribute>(type)) AddType(type);
                else if (HasCustomAttribute<ExportAttribute>(type) && type.GetCustomAttributesData().Any(d => d.ConstructorArguments.Count > 0)) RegisterWithBaseType(type);
                else if (HasCustomAttribute<ExportAttribute>(type)) AddType(type);
                else if (HasPropertyInjected(type)) AddType(type);
            }
        }

        public void AddType(Type type)
        {
            types[type] = type;
        }

        public void AddType(Type type, Type baseType)
        {
            types[baseType] = type;
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }
        public object CreateInstance(Type contract)
        {
            var typeToCreate = types[contract];
            return HasPropertyInjected(typeToCreate) ? InjectProperties(typeToCreate) : InjectCtor(typeToCreate);
        }

        private object InjectProperties(Type typeToCreate)
        {
            var instance = Activator.CreateInstance(typeToCreate);
            var properties = typeToCreate.GetProperties();

            foreach (var property in properties)
            {
                property.SetValue(instance, CreateInstance(property.PropertyType), null);
            }
            return instance;
        }

        private object InjectCtor(Type typeToCreate)
        {
            var constructor = typeToCreate.GetConstructors().First();
            if (HasCustomAttribute<ExportAttribute>(typeToCreate)) return Activator.CreateInstance(typeToCreate);
            var constructorParameters = constructor.GetParameters();
            if (constructorParameters.Length == 0)
            {
                return Activator.CreateInstance(typeToCreate);
            }
            IList<object> parameters = constructorParameters.Select(parameterInfo => CreateInstance(parameterInfo.ParameterType)).ToList();

            return constructor.Invoke(parameters.ToArray());
        }

        private void RegisterWithBaseType(Type type)
        {
            var value = type.GetCustomAttributesData().First().ConstructorArguments.Select(arg => arg.Value).First();
            AddType(type, (Type)value);
        }

        private bool HasPropertyInjected(Type type)
        {
            return type.GetProperties().Any(p => p.GetCustomAttributes(typeof(ImportAttribute)).Any());
        }

        private bool HasCustomAttribute<T>(Type type)
        {
            return type.GetCustomAttributes(typeof(T)).Any();
        }
    }
}
