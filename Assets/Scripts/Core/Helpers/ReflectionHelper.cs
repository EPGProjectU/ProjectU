using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ProjectU.Core.Helpers
{
    public static class ReflectionHelper
    {
        private static readonly Assembly[] DomainAssemblies;

        static ReflectionHelper()
        {
            DomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        /// Gets derived types of base across all linked assemblies 
        /// </summary>
        /// <param name="baseType">Type that is derived</param>
        /// <param name="includeBaseType">Should enumerable contain baseType</param>
        /// <returns>Enumerable with all derived types of tye base</returns>
        public static IEnumerable<Type> GetDerivedTypes(this Type baseType, bool includeBaseType = false)
        {
            var assemblies = baseType.Assembly.GetDependentAssemblies().Append(baseType.Assembly);

            // Get all types in the found assemblies
            var types =
                from assembly in assemblies
                from type in assembly.GetTypes()
                select type;

            // Cache all types deriving from CallbackAttribute
            var derivedTypes =
                from type in types
                where type.IsSubclassOf(baseType)
                select type;

            if (includeBaseType)
                derivedTypes = derivedTypes.Prepend(baseType);

            return derivedTypes;
        }

        /// <summary>
        /// Creates default value for type that is not know at compile time
        /// </summary>
        /// <param name="type">Type for which to create default value</param>
        /// <returns>Default value of a type</returns>
        /// <remarks>In case of classes return value will always be null</remarks>
        public static object CreateDefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Checks if value can be assigned to type that is not know at compile time and if it is not creates default value for it
        /// </summary>
        /// <param name="type">Target type</param>
        /// <param name="value">Value that we try to assign to type</param>
        /// <returns>Unchanged value if it can be assigned, otherwise default value of the type</returns>
        /// TODO Better handling of primitive types (e.g. int value would not be assigned to long)
        public static object SetOrDefaultValue(this Type type, object value)
        {
            return type.IsInstanceOfType(value) ? value : type.CreateDefaultValue();
        }

        /// <summary>
        /// Gets assemblies that are referencing the target assembly 
        /// </summary>
        /// <param name="referenceAssembly">Assembly that is referenced by searched assemblies</param>
        /// <returns>Enumerable of assemblies</returns>
        /// <remarks>Only direct references are taken into consideration (e.g. A -> B -> C: C would not be included in assemblies dependent on A) </remarks>
        public static IEnumerable<Assembly> GetDependentAssemblies(this Assembly referenceAssembly)
        {
            var dependentAssemblies = from assembly in DomainAssemblies
                where assembly.GetReferencedAssemblies().Any(name => AssemblyName.ReferenceMatchesDefinition(name, referenceAssembly.GetName()))
                select assembly;

            return dependentAssemblies;
        }

        /// <summary>
        /// Gets value of an object's field base of its name
        /// </summary>
        /// <param name="obj">Object containing the field</param>
        /// <param name="name">Name of the field</param>
        /// <returns>Value of the field with matching name in the object</returns>
        public static object GetReflectionFieldValue(this object obj, string name)
        {
            return obj.GetType().GetField(name).GetValue(obj);
        }

        /// <summary>
        /// Gets types that are using given attribute in class declaration
        /// </summary>
        /// <param name="attributeType">Searched attribute</param>
        /// <param name="includeInherit">If types that themselves do not have attribute (but base type has) should be included in the results</param>
        /// <returns>Types with the attributes</returns>
        public static IEnumerable<Type> GetTypesWithAttribute(this Type attributeType, bool includeInherit = true)
        {
            return
                from assembly in DomainAssemblies
                from type in assembly.GetTypes()
                where type.GetCustomAttributes(attributeType, includeInherit).Length > 0
                select type;
        }
    }
}