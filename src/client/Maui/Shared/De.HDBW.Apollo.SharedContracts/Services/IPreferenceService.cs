using De.HDBW.Apollo.SharedContracts.Enums;

namespace De.HDBW.Apollo.SharedContracts.Services
{
    public interface IPreferenceService
    {
        TU GetValue<TU>(Preference key, TU defaultValue);

        bool SetValue<TU>(Preference key, TU value);
    }
}
