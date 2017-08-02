using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

public static class ComponentExtension
{
    public static T GetComponentOnlyInParent<T>(this Component component)
    {
        Transform parent = component.transform.parent;

        while (parent != null)
        {
            if (parent.GetComponent<T>() != null)
                return parent.GetComponent<T>();

            parent = parent.parent;
        }

        return default(T);
    }

    public static T GetComponentOnlyInChildren<T>(this Component component, bool includeInactive = false)
    {
        foreach(Transform child in component.transform)
        {
            var result = child.GetComponentInChildren<T>(includeInactive);
            if (result != null)
                return result;
        }

        return default(T);
    }

    public static T[] GetComponentsOnlyInChildren<T>(this Component component, bool includeInactive = false)
    {
        List<T> components = new List<T>();
        foreach (Transform child in component.transform)
        {
            var result = child.GetComponentsInChildren<T>(includeInactive);
            if (result != null)
                components.AddRange(result);
        }

        if (components.Count != 0)
            return components.ToArray();

        return default(T[]);
    }

    public static Component GetCopyOf(this Component component, Component other)
    {
        if (component == null || other == null)
            return component;

        Type type = component.GetType();
        if (type != other.GetType())
            return null; // type mis-match

        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        foreach (var propertyInfo in type.GetProperties(flags))
        {
            if (propertyInfo.CanWrite)
            {
                try
                {
                    propertyInfo.SetValue(component, propertyInfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }

        foreach (var fieldInfo in type.GetFields(flags))
        {
            fieldInfo.SetValue(component, fieldInfo.GetValue(other));
        }

        return component;
    }
}
