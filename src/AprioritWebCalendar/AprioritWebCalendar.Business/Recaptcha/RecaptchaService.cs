using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using AprioritWebCalendar.Business.Interfaces;

namespace AprioritWebCalendar.Business.Recaptcha
{
    public class RecaptchaService : ICaptchaService
    {
        private readonly RecaptchaKeys _keys;

        public RecaptchaService(IOptions<RecaptchaKeys> options)
        {
            _keys = options.Value;
        }

        public async Task<bool> TryVerifyCaptchaAsync(string token)
        {
            var verifyResponse = await VerifyAsync(new RecaptchaVerifyRequest
            {
                PrivateKey = _keys.PrivateKey,
                RecaptchaResponse = token
            });

            return verifyResponse.IsSuccess;
        }

        private async Task<RecaptchaVerifyResponse> VerifyAsync(RecaptchaVerifyRequest request)
        {
            var client = new HttpClient();
            var resp = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify",
                new StringContent(request.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded"));

            if (!resp.IsSuccessStatusCode)
                throw new Exception();

            return await DeserializeResponseContent<RecaptchaVerifyResponse>(resp);
        }

        private async Task<TObject> DeserializeResponseContent<TObject>(HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<TObject>(await response.Content.ReadAsStringAsync());
        }
    }
}
