using Application.Common.BaseEntities;
using Application.Common.Interfaces.Auth;
using Application.Common.InterFaces.DbContext;
using Domain.Common;
using Domain.Enum;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Business.Auth.Activation.Command
{
    public class InsertActivationCommand : IRequest<BaseResult_VM<string>>
    {
        public int UserId { get; set; }
        public string phoneNumber { get; set; }

        public int code { get; set; }

        public InsertActivationCommand()
        {

        }
        public InsertActivationCommand(int userId)
        {
            UserId = userId;
        }
    }

    public class InsertActivationHandler : IRequestHandler<InsertActivationCommand, BaseResult_VM<string>>
    {
        private readonly IAdminDbContext _dbContext;
        private readonly ISMS _sms;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InsertActivationHandler(IAdminDbContext dbContext, ISMS sms,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _sms = sms;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResult_VM<string>> Handle(InsertActivationCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == null || request.UserId == 0) throw new Exception();

            //غیر فعال کردن اکتیویشن های فعال
            var activeList = await _dbContext.Activation.Where(m => m.UserId == request.UserId && m.Status == (int)Status.Active).AsTracking().ToListAsync();
            if (activeList != null && activeList.Count > 0)
            {
                foreach (var item in activeList)
                {
                    item.Status = (int)Status.Inactive;
                }
            }

            var activation = new Domain.Entities.Auth.Activation()
            {
                UserId = request.UserId,
                CreateTime = DateTime.Now,
                ExpireTime = DateTime.Now.AddMinutes(3),
                ActivationCode = request.code,
                Status = (int)Status.Active,
                IpAddress = await GetIP()
            };

            _dbContext.Activation.Add(activation);
            if (await _dbContext.SaveChangesAsync(cancellationToken) <= 0) throw new Exception();

            return new BaseResult_VM<string>(request.code.ToString(), 0, "");
        }


        public async Task<string> GetIP()
        {
            var ipAddressStr = string.Empty;
            ipAddressStr = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            if (string.IsNullOrEmpty(ipAddressStr))
            {
                var forwarededIp = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
                if (!string.IsNullOrEmpty(ipAddressStr))
                    ipAddressStr = forwarededIp;
            }

            IPAddress ip;
            if (IPAddress.TryParse(ipAddressStr, out ip))
                ipAddressStr = ip.ToString();
            else
                ipAddressStr = "Unknown";

            return ipAddressStr;
        }
    }
}