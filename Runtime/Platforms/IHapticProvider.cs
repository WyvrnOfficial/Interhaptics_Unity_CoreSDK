/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

namespace Interhaptics.Platforms
{

    public interface IHapticProvider
    {

        #region HAPTIC CHARACTERISTICS
        /// <summary>
        ///     Define the display name to use for the device
        /// </summary>
        /// <returns>The device name</returns>
        string DisplayName();
        /// <summary>
        ///     Define the tracking description
        /// </summary>
        /// <returns>The tracking description</returns>
        string Description();
        /// <summary>
        ///     Define the Manufacturer
        /// </summary>
        /// <returns>The manufacturer name</returns>
        string Manufacturer();
        /// <summary>
        ///     Define the provider version
        /// </summary>
        /// <returns>The provider version</returns>
        string Version();
        #endregion


        #region PROVIDER SETUP
        /// <summary>
        ///     Initialize the provider
        /// </summary>
        /// <returns>Return true if the provider was correctly initialized. False otherwise</returns>
        bool Init();
        /// <summary>
        ///     Clean the provider and dispose resources allowed
        /// </summary>
        /// <returns>Return true if the provider was properly cleaned. False otherwise</returns>
        bool Clean();
        #endregion


        #region PROVIDER RENDERING
        /// <summary>
        ///     Define if the tracking APIs are available or not
        /// </summary>
        /// <returns>True if ready to receive haptics data. False otherwise</returns>
        bool IsPresent();

        void RenderHaptics();
        #endregion

    }

}