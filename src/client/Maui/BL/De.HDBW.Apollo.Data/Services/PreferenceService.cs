namespace De.HDBW.Apollo.Data.Services
{
    using De.HDBW.Apollo.SharedContracts.Enums;
    using De.HDBW.Apollo.SharedContracts.Services;
    using Microsoft.Extensions.Logging;

    public class PreferenceService : IPreferenceService
    {
        public PreferenceService(ILogger<PreferenceService> logger, IPreferences preferences)
        {
            this.Logger = logger;
            this.Preferences = preferences;
        }

        private ILogger Logger { get; }

        private IPreferences Preferences { get;  }

        public bool SetValue<TU>(Preference key, TU value)
        {
            try
            {
                this.Preferences.Remove(key.ToString());
                this.Preferences.Set<TU>(key.ToString(), value);
                return true;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Unknown error while SetValue<TU> in {this.GetType().Name}.");
            }

            return false;
        }

        public TU GetValue<TU>(Preference key, TU defaultValue)
        {
            object? result = null;

            try
            {
                result = this.Preferences.Get<TU>(key.ToString(), defaultValue);
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, $"Unknown error while GetValue<TU> in {this.GetType().Name}.");
            }

            return result is TU ? (TU)result : defaultValue;
        }
    }
}