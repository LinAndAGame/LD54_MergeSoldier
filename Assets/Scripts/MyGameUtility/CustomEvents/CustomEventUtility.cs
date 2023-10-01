using System;
using System.Linq;
using System.Reflection;

namespace MyGameUtility {
    public static class CustomEventUtility {
        public static void CreateCustomEventInClass<T>(T target) {
            foreach (FieldInfo fieldInfo in typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
                if (fieldInfo.FieldType.GetCustomAttribute<CustomEventAttribute>() != null) {
                    if (fieldInfo.GetValue(target) == null) {
                        fieldInfo.SetValue(target, Activator.CreateInstance(fieldInfo.FieldType,"",""));
                    }
                }
            }
            
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) {
                if (propertyInfo.PropertyType.GetCustomAttribute<CustomEventAttribute>() != null) {
                    if (propertyInfo.GetValue(target) == null) {
                        propertyInfo.SetValue(target, Activator.CreateInstance(propertyInfo.PropertyType,"",""));
                    }
                }
            }
        }
    }
}