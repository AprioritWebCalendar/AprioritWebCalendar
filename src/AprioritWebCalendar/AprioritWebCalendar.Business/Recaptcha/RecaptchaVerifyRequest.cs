using System;
using System.Web;
using Newtonsoft.Json;

namespace AprioritWebCalendar.Business.Recaptcha
{
    public class RecaptchaVerifyRequest
    {
        [JsonProperty("response")]
        public string RecaptchaResponse { get; set; }

        [JsonProperty("secret")]
        public string PrivateKey { get; set; }

        public override string ToString()
        {
            var collection = HttpUtility.ParseQueryString(string.Empty);
            collection["response"] = RecaptchaResponse;
            collection["secret"] = PrivateKey;

            var builder = new UriBuilder() { Query = collection.ToString() };
            return builder.Query.Substring(1);
        }
    }
}
