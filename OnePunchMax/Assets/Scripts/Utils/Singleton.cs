using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public static class Singleton<T> where T : Object
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    if (typeof(Component).IsAssignableFrom(typeof(T)))
                    {
                        s_instance = GetComponentSingleton();
                    }
                    else if (typeof(ScriptableObject).IsAssignableFrom(typeof(T)))
                    {
                        s_instance = GetScriptableSingleton();
                    }
                    else
                    {
                        throw new TypeAccessException(
                            $"{typeof(T).Name} cannot be a singleton because it's not a Component or a ScriptableObject");
                    }
                }
                return s_instance;
            }
        }

        private static T GetComponentSingleton()
        {
            T[] instances = Object.FindObjectsOfType<T>();

            return instances.Length switch
            {
                > 1 => throw new Exception($"There is more than one instance of '{typeof(T).Name}' in the scene." +
                                           "\nSingletons must have only one instance in the scene."),
                0 => throw new Exception($"There is no instance if '{typeof(T).Name}' in the scene." +
                                         "\nSingletons must have one instance in the scene."),
                _ => instances[0]
            };
        }

        private static T GetScriptableSingleton()
        {
            T instance = Resources.Load($"{typeof(T).Name}_Singleton", typeof(T)) as T;
            
            if (instance == null) throw new NullReferenceException($"No {typeof(T)} with name '{typeof(T).Name}_Singleton'" +
                                                                   " found directly inside any 'Resources' folder");
            return instance;
        }
    }
}
