namespace De.HDBW.Apollo.SharedContracts.Services
{
    using De.HDBW.Apollo.SharedContracts.Enums;

    public interface IPreferenceService
    {
        TU GetValue<TU>(Preference key, TU defaultValue);

        bool SetValue<TU>(Preference key, TU value);
    }
}
