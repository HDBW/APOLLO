// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.SharedContracts.Enums;
using De.HDBW.Apollo.SharedContracts.Services;
using Microsoft.Extensions.Logging;

namespace De.HDBW.Apollo.Data.Services
{
    public class PreferenceService : IPreferenceService
    {
        public PreferenceService(ILogger<PreferenceService>? logger, IPreferences? preferences)
        {
            Logger = logger;
            Preferences = preferences;
        }

        private ILogger? Logger { get; }

        private IPreferences? Preferences { get;  }

        public bool SetValue<TU>(Preference key, TU value)
        {
            try
            {
                Preferences?.Remove(key.ToString());
                Preferences?.Set<TU>(key.ToString(), value);
                return true;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while SetValue<TU> in {GetType().Name}.");
            }

            return false;
        }

        public TU GetValue<TU>(Preference key, TU defaultValue)
        {
            object? result = null;

            try
            {
                if (Preferences != null)
                {
                    result = Preferences.Get<TU>(key.ToString(), defaultValue);
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, $"Unknown error while GetValue<TU> in {GetType().Name}.");
            }

            return result is TU ? (TU)result : defaultValue;
        }
    }
}
