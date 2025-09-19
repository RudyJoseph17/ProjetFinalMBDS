using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Shared.Domain.Helpers
{
    public static class JsonSettings
    {
        public static readonly JsonSerializerSettings CamelCase = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = true,
                    OverrideSpecifiedNames = false
                }
            },
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatString = "yyyy-MM-dd"
        };
    }
}
