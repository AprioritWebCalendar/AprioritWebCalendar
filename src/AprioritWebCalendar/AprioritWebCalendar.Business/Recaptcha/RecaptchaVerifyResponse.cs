using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AprioritWebCalendar.Business.Recaptcha
{
    public class RecaptchaVerifyResponse
    {
        [JsonProperty("success")]
        public bool IsSuccess { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime ChallengeTimestamp { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("error-codes")]
        public IEnumerable<string> Errors { get; set; }
    }
}
