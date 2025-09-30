/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using System.Linq;


namespace Interhaptics.Internal
{

    internal static class ReflectionNames
    {

        #region ASSEMBLY NAMES
        public const string DEFAULT_ASSEMBLY_NAME = "Assembly-CSharp";
        public const string ASSEMBLY_PREFIX_NAME_FOR_PROVIDERS = "GoTouchVR.Interhaptics";
        #endregion


        #region PROVIDERS METHODS NAMES
        // TRACKING CHARACTERISTICS
        public const string DESCRIPTION_PROVIDER_METHOD_NAME = "Description";
        public const string DISPLAY_NAME_PROVIDER_METHOD_NAME = "DisplayName";
        public const string MANUFACTURER_PROVIDER_METHOD_NAME = "Manufacturer";
        public const string VERSION_PROVIDER_METHOD_NAME = "Version";

        // PROVIDER SETUP
        public const string INIT_PROVIDER_METHOD_NAME = "Init";
        public const string CLEAN_PROVIDER_METHOD_NAME = "Clean";

        // PROVIDER RENDERING
        public const string IS_PRESENT_PROVIDER_METHOD_NAME = "IsPresent";
        public const string RENDER_HAPTICS_PROVIDER_METHOD_NAME = "RenderHaptics";
        #endregion


        #region PUBLIC METHODS
        /// <summary>
        ///     Get interhaptics assemblies in which a haptic provider can be
        /// </summary>
        /// <returns>An assembly collection</returns>
        public static System.Collections.Generic.IEnumerable<System.Reflection.Assembly>
            GetInterhapticsHapticProviderAssemblies()
        {
            return GetAssemblies(assembly => assembly.FullName.StartsWith(ASSEMBLY_PREFIX_NAME_FOR_PROVIDERS));
        }

        /// <summary>
        ///     Get assemblies in which a haptic provider can be
        /// </summary>
        /// <returns>An assembly collection</returns>
        public static System.Collections.Generic.IEnumerable<System.Reflection.Assembly> GetCompatibleAssemblies()
        {
            return GetAssemblies(assembly =>
                assembly.FullName.StartsWith(ASSEMBLY_PREFIX_NAME_FOR_PROVIDERS) ||
                assembly.GetName().Name == DEFAULT_ASSEMBLY_NAME ||
                assembly == System.Reflection.Assembly.GetExecutingAssembly());
        }

        /// <summary>
        ///     Get assemblies in which a haptic provider can be depending on a parametrized checking method
        /// </summary>
        /// <param name="checker">A checking method</param>
        /// <returns>An assembly collection</returns>
        private static System.Collections.Generic.IEnumerable<System.Reflection.Assembly> GetAssemblies(
            System.Func<System.Reflection.Assembly, bool> checker)
        {
            return System.AppDomain.CurrentDomain.GetAssemblies().Where(checker);
        }
        #endregion

    }

}