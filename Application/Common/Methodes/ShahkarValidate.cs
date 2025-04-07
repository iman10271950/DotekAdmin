using Application.Business.Auth.User.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.Interfaces.Auth;
using Bogus.DataSets;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Bogus.DataSets.Name;

namespace Application.Common.Methodes
{
    public class ShahkarValidate : IShahkarValidation
    {
        public async Task<BaseResult_VM<string>> PostCodeValidationAndGetAddress(string PostalCode)
        {
            var inputserviec = new
            {
                postal_code = PostalCode,
            };


            var options = new RestClientOptions("https://service.zohal.io")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("https://service.zohal.io/api/v0/services/inquiry/postal_code_inquiry", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer a420a7bef64f3349b4655af534d55c5e4a16ec99");
            var body = JsonConvert.SerializeObject(inputserviec);
            request.AddStringBody(body, RestSharp.DataFormat.Json);
            try
            {
                RestResponse response = await client.ExecuteAsync(request);

                // Checking if the response is successful
                if (response.IsSuccessful)
                {
                    // Parsing the JSON response
                    var responseObject = JsonConvert.DeserializeObject<ShahkarResponse<PostResponse>>(response.Content);
                    if (responseObject.ResponseBody.Data == null)
                    {
                        return new BaseResult_VM<string>
                        {
                            Code = -9999,
                            Message = responseObject.ResponseBody.Message,

                        };
                    }
                    var addres = responseObject.ResponseBody.Data.address;

                    // Returning the "matched" value
                    return new BaseResult_VM<string>
                    {
                        Code = 0,
                        Message = "با موفقیت انجام شد",
                        Result = MergeAddress(addres),
                    };
                }
                else
                {

                    return new BaseResult_VM<string>
                    {
                        Code = -2,
                        Message = response.ErrorMessage,

                    };
                }
            }
            catch (Exception ex)
            {

                return new BaseResult_VM<string>
                {
                    Code = -3,
                    Message = ex.Message,

                };
            }
        }

        public async Task<bool> ShabaValidation(string NationalCode, string BirthDate, string IBAN)
        {
            var inputserviec = new
            {
                national_code = NationalCode,
                birth_date = BirthDate,
                IBAN = IBAN,
            };


            var options = new RestClientOptions("https://service.zohal.io")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("https://service.zohal.io/api/v0/services/inquiry/check_iban_with_national_code", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer 3eb9293d3693322ec98b622a3072a24f33049682");
            var body = JsonConvert.SerializeObject(inputserviec);
            request.AddStringBody(body, RestSharp.DataFormat.Json);
            try
            {
                RestResponse response = await client.ExecuteAsync(request);

                // Checking if the response is successful
                if (response.IsSuccessful)
                {
                    // Parsing the JSON response
                    var responseObject = JsonConvert.DeserializeObject<ShahkarResponse<ResponseData>>(response.Content);

                    // Returning the "matched" value
                    return responseObject?.ResponseBody?.Data?.Matched ?? false;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}, Details: {response.Content}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }

        }

        public async Task<bool> Validation(string PhoneNumber, string NationalCode)
        {
            var inputserviec = new
            {

                mobile = PhoneNumber,
                national_code = NationalCode
            };


            var options = new RestClientOptions("https://service.zohal.io")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("https://service.zohal.io/api/v0/services/inquiry/shahkar", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer 1cae54ae2f9b6ab638f8aa5fc750a0cffb79a741");
            var body = JsonConvert.SerializeObject(inputserviec);
            request.AddStringBody(body, RestSharp.DataFormat.Json);


            try
            {
                RestResponse response = await client.ExecuteAsync(request);

                // Checking if the response is successful
                if (response.IsSuccessful)
                {
                    // Parsing the JSON response
                    var responseObject = JsonConvert.DeserializeObject<ShahkarResponse<ResponseData>>(response.Content);

                    // Returning the "matched" value
                    return responseObject?.ResponseBody?.Data?.Matched ?? false;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}, Details: {response.Content}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }
        public class ShahkarResponse<T>
        {
            [JsonProperty("response_body")]
            public ResponseBody<T> ResponseBody { get; set; }

            [JsonProperty("result")]
            public int Result { get; set; }
        }

        public class ResponseBody<T>
        {
            [JsonProperty("data")]
            public T Data { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("error_code")]
            public string ErrorCode { get; set; }
        }

        public class ResponseData
        {
            [JsonProperty("matched")]
            public bool Matched { get; set; }
        }
        private class PostResponse
        {
            public AddressPost address { get; set; }
        }
        private class AddressPost
        {
            public string province { get; set; }
            public string town { get; set; }
            public string district { get; set; }
            public string street { get; set; }

            public string street2 { get; set; }

            public string number { get; set; }

            public string floor { get; set; }
            public string side_floor { get; set; }
            public string building_name { get; set; }
            public string description { get; set; }


        }
        private string MergeAddress(AddressPost address)
        {

            if (address == null)
            {
                return string.Empty;
            }

            var addressParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(address.province))
                addressParts.Add(address.province);

            if (!string.IsNullOrWhiteSpace(address.town))
                addressParts.Add(address.town);

            if (!string.IsNullOrWhiteSpace(address.district))
                addressParts.Add(address.district);

            if (!string.IsNullOrWhiteSpace(address.street))
                addressParts.Add(address.street);

            if (!string.IsNullOrWhiteSpace(address.street2))
                addressParts.Add(address.street2);

            if (!string.IsNullOrWhiteSpace(address.number))
                addressParts.Add($"پلاک {address.number}");

            if (!string.IsNullOrWhiteSpace(address.floor))
                addressParts.Add($"طبقه {address.floor}");

            if (!string.IsNullOrWhiteSpace(address.side_floor))
                addressParts.Add($"واحد {address.side_floor}");

            if (!string.IsNullOrWhiteSpace(address.building_name))
                addressParts.Add($"ساختمان {address.building_name}");

            if (!string.IsNullOrWhiteSpace(address.description))
                addressParts.Add(address.description);

            return string.Join(", ", addressParts);
        }

        public async Task<BaseResult_VM<NationalIdentity_VM>> NationalIdentityInquiry(string NationalCode, string ShamsiBirthDate)
        {
            // ایجاد ورودی برای ارسال به سرویس
            var inputService = new
            {
                national_code = NationalCode,
                birth_date = ShamsiBirthDate,
                gender = true // می‌توانید این مقدار را بر اساس نیاز تغییر دهید
            };

            // تنظیمات RestClient
            var options = new RestClientOptions("https://service.zohal.io")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);

            // تنظیم درخواست HTTP
            var request = new RestRequest("https://service.zohal.io/api/v0/services/inquiry/national_identity_inquiry", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "Bearer 1ed69eab4f64284468356145d0df9b47e9237611"); // جایگزین با توکن مناسب

            // تبدیل ورودی به JSON و افزودن به درخواست
            var body = JsonConvert.SerializeObject(inputService);
            request.AddStringBody(body, RestSharp.DataFormat.Json);

            try
            {
                // ارسال درخواست به سرور
                RestResponse response = await client.ExecuteAsync(request);

                // بررسی موفقیت‌آمیز بودن پاسخ
                if (response.IsSuccessful)
                {
                    // تبدیل پاسخ JSON به شیء موردنظر
                    var responseObject = JsonConvert.DeserializeObject<ShahkarResponse<NationalIdentity_VM>>(response.Content);

                    if (responseObject.ResponseBody.Data == null)
                    {
                        return new BaseResult_VM<NationalIdentity_VM>
                        {
                            Code = -9999,
                            Message = responseObject.ResponseBody.Message
                        };
                    }

                    // بازگشت نتیجه موفقیت‌آمیز
                    return new BaseResult_VM<NationalIdentity_VM>
                    {
                        Code = 0,
                        Message = "درخواست با موفقیت انجام شد",
                        Result = responseObject.ResponseBody.Data
                    };
                }
                else
                {
                    // بازگشت خطای مربوط به سرور
                    return new BaseResult_VM<NationalIdentity_VM>
                    {
                        Code = -2,
                        Message = response.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                // بازگشت خطای مربوط به استثنا
                return new BaseResult_VM<NationalIdentity_VM>
                {
                    Code = -3,
                    Message = ex.Message
                };
            }
        }




    }
}


