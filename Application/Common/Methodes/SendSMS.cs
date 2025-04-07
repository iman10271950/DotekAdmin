using Application.Common.BaseEntities;
using Application.Common.Interfaces.Auth;
using MediatR;
using Newtonsoft.Json;
using Npgsql.Internal;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Methodes
{
    public class SMS : ISMS
    {
       

        public  async Task<BaseResult_VM<int>> Send(smsInputSingle input)
        {
            var random = new Random();
            int code = random.Next(1000, 9999);
            var inputserviec = new SMSInput
            {
                AddName = true,
                Mobile = input.MpbileNumber,
                SmsCode = input.Code,
            };
            var options = new RestClientOptions("https://sms.parsgreen.ir")
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("https://sms.parsgreen.ir/Apiv2/Message/SendOtp", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Authorization", "basic apikey:B7C14456-923F-4AF0-869A-F7BE34CCDCD6");
            var body = JsonConvert.SerializeObject(inputserviec);
            request.AddStringBody(body, RestSharp.DataFormat.Json);
            RestResponse response = await client.ExecuteAsync(request);
            var res = JsonConvert.DeserializeObject<Response>(response.Content);
            if (res.R_Code == 0)
            {
                return new BaseResult_VM<int>
                {
                    Code = 0,
                    Message = "",
                    Result = code


                };
            }
            return new BaseResult_VM<int> { Code = 0, Message = "لطفا دوباره تلاش بفرمایید " };

        }


        private class SMSInput
        {
            public string Mobile { get; set; }
            public string SmsCode { get; set; }
            public bool AddName { get; set; }
        }
        public class Response
        {
            public bool R_Success { get; set; } // نشان‌دهنده موفقیت عملیات
            public int R_Code { get; set; } // کد مربوط به وضعیت
            public string R_Error { get; set; } // خطا در صورت وجود
            public string R_Message { get; set; } // پیام توضیحات
        }
    }
}