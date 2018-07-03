using System.Reflection;
using UnityEngine;

public static class ComponentExtensions
{
    public static T CopyComponent<T>(this Component component, T source) where T : Component
    {
        var type = component.GetType();

        if (type != source.GetType()) return null; 

        var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        var props = type.GetProperties(flags);

        foreach (var prop in props)
            if (prop.CanWrite) 
                prop.SetValue(component, prop.GetValue(source, null), null);
        
        var fields = type.GetFields(flags);
        foreach (var field in fields)
            field.SetValue(component, field.GetValue(source));
        
        return component as T;
    }
}
