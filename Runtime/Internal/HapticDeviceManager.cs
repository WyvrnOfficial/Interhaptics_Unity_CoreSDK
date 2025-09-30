/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using System.Collections.Generic;
using System.Linq;

using Interhaptics.Platforms;


namespace Interhaptics.Internal
{

    internal static class LinqExtension
    {

        internal static void ForEach<T>(this IEnumerable<T> enumeration, System.Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

    }

    internal static class HapticDeviceManager
    {

        private static Dictionary<System.Type, object> m_haptic_providers = new Dictionary<System.Type, object>();
        
        public static void DeviceInitLoop()
        {
            ReflectionNames.GetCompatibleAssemblies().ForEach(assembly => {
                assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IHapticProvider))).ForEach(hapticProviderType => {
                    object instance = System.Activator.CreateInstance(hapticProviderType);
                    if (instance == null)
                    {
                        return;
                    }

                    //if Init exists and is successful, adds it to the providers
                    if ((bool)hapticProviderType.GetMethod(ReflectionNames.INIT_PROVIDER_METHOD_NAME)?.Invoke(instance, null))
                    {
                        m_haptic_providers.Add(hapticProviderType, instance);
                    }
                });
            }); //Get from AssemblyCsharp and Interhaptics Provider
        }

        public static void DeviceRenderLoop()
        {
            m_haptic_providers.ForEach(provider => {
                //Check if device is present
                if ((bool)provider.Key.GetMethod(ReflectionNames.IS_PRESENT_PROVIDER_METHOD_NAME)?.Invoke(provider.Value, null))
                {
                    //Send haptics to the device
                    provider.Key.GetMethod(ReflectionNames.RENDER_HAPTICS_PROVIDER_METHOD_NAME)?.Invoke(provider.Value, null);
                }
            });
        }

        public static void DeviceCleanLoop()
        {
            m_haptic_providers.ForEach(provider => {
                //Clean device COM
                provider.Key.GetMethod(ReflectionNames.CLEAN_PROVIDER_METHOD_NAME)?.Invoke(provider.Value, null);
            });
        }

    }

}
