namespace Identity.PL.API.Common.Validator
{
    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
}
