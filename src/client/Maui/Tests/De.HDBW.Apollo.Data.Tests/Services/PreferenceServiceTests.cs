// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.

using De.HDBW.Apollo.Data.Services;
using De.HDBW.Apollo.Data.Tests.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace De.HDBW.Apollo.Data.Tests.Services;
public class PreferenceServiceTests : IDisposable
{
    private Dictionary<string, object> _storage = new Dictionary<string, object>();
    private ILogger<PreferenceService> _logger;

    public PreferenceServiceTests()
    {
        _logger = this.SetupLogger<PreferenceService>();
    }

    public void Dispose()
    {
        _storage.Clear();
    }

    [Fact]
    public void TestCreation()
    {
        IPreferences? preferences = null;

        var service = new PreferenceService(null, preferences);
        Assert.NotNull(service);

        preferences = null;
        service = new PreferenceService(_logger, preferences);
        Assert.NotNull(service);

        preferences = SetupPreference();
        service = new PreferenceService(_logger, preferences);
        Assert.NotNull(service);
    }

    [Fact]
    public void TestGetSetValueWithException()
    {
        var preferences = SetupPreference(true);
        var service = new PreferenceService(_logger, preferences);
        var result = service.GetValue(SharedContracts.Enums.Preference.Unknown, true);
        Assert.True(result);

        result = service.SetValue(SharedContracts.Enums.Preference.Unknown, true);
        Assert.False(result);
    }

    [Fact]
    public void TestGetSetValue()
    {
        var preferences = SetupPreference();
        var service = new PreferenceService(_logger, preferences);
        var result = service.SetValue(SharedContracts.Enums.Preference.Unknown, true);
        Assert.True(result);
        result = service.GetValue(SharedContracts.Enums.Preference.Unknown, false);
        Assert.True(result);

        result = service.SetValue(SharedContracts.Enums.Preference.Unknown, 2);
        Assert.True(result);
        var intResult = service.GetValue(SharedContracts.Enums.Preference.Unknown, 0);
        Assert.Equal(2, intResult);

        result = service.SetValue(SharedContracts.Enums.Preference.Unknown, 2m);
        Assert.True(result);
        var decimalResult = service.GetValue(SharedContracts.Enums.Preference.Unknown, 0m);
        Assert.Equal(2m, decimalResult);

        result = service.SetValue(SharedContracts.Enums.Preference.Unknown, "hallo");
        Assert.True(result);
        var stringResult = service.GetValue(SharedContracts.Enums.Preference.Unknown, string.Empty);
        Assert.Equal("hallo", stringResult);

        var date = DateTime.Now;
        result = service.SetValue(SharedContracts.Enums.Preference.Unknown, date);
        Assert.True(result);
        var dateResult = service.GetValue(SharedContracts.Enums.Preference.Unknown, DateTime.MinValue);
        Assert.Equal(date, dateResult);

        var doubelResult = service.GetValue(SharedContracts.Enums.Preference.Unknown, 0d);
        Assert.Equal(0d, doubelResult);
    }

    private IPreferences SetupPreference(bool throwException = false)
    {
        var mock = new Mock<IPreferences>();
        if (throwException)
        {
            mock.Setup(m => m.Get<It.IsAnyType>(It.IsAny<string>(), It.IsAny<It.IsAnyType>(), It.IsAny<string?>())).Returns((string s, object d, string? x) => { throw new NotSupportedException(); });
            mock.Setup(m => m.Set<It.IsAnyType>(It.IsAny<string>(), It.IsAny<It.IsAnyType>(), It.IsAny<string?>())).Callback((string s, object d, string? x) => { throw new NotSupportedException(); });
        }
        else
        {
            mock.Setup(m => m.Get<It.IsAnyType>(It.IsAny<string>(), It.IsAny<It.IsAnyType>(), It.IsAny<string?>())).Returns((string key, object defaultValue, string? sharedName) =>
            {
                if (_storage.ContainsKey(key))
                {
                    return _storage[key];
                }
                else
                {
                    return defaultValue;
                }
            });

            mock.Setup(m => m.Set<It.IsAnyType>(It.IsAny<string>(), It.IsAny<It.IsAnyType>(), It.IsAny<string?>())).Callback((string key, object value, string? sharedName) =>
            {
                if (_storage.ContainsKey(key))
                {
                    _storage[key] = value;
                }
                else
                {
                    _storage.Add(key, value);
                }
            });
        }

        return mock.Object;
    }
}
