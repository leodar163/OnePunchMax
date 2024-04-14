using System;
using UnityEngine;

namespace Utils.Serializable
{
    /// <summary>
    /// Attribute that require implementation of the provided interface.
    /// From https://www.patrykgalach.com/2020/01/27/assigning-interface-in-unity-inspector/
    /// </summary>
    public class RequireInterfaceAttribute : PropertyAttribute
    {
        // Interface type.
        public Type requiredType { get; private set; }

        /// <summary>
        /// Requiring implementation of the <see cref="T:RequireInterfaceAttribute"/> interface.
        /// </summary>
        /// <param name="type">Interface type.</param>
        public RequireInterfaceAttribute(Type type)
        {
            requiredType = type;
        }
    }
}
