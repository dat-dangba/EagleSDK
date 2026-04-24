using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Eagle
{
    public class EagleBuildReflectionConfigBase : ScriptableObject
    {
        public FieldInfo[] GetBuildFields()
        {
            return GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).ToArray();
        }
    }
}