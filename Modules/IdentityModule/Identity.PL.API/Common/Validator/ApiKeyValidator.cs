using Identity.PL.API.Configurations;
using Microsoft.Extensions.Options;

namespace Identity.PL.API.Common.Validator
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly ApiKeySettings _settings;

        public ApiKeyValidator(IOptions<ApiKeySettings> options)
        {
            _settings = options.Value;
        }

        public bool IsValid(string apiKey)
        {
            return _settings.ApiKey == apiKey;
        }
    }
}
