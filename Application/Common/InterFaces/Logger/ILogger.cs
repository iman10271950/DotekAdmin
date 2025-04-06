using Domain.Entities.Log;

namespace Application.Common.Interfaces.Logger
{
    public interface ILogger
    {
        Task<bool> Log(RequestAdminLog request);
        Task<bool> Log(ResponseAdminLog response);

        //Task<bool> Log(RequestSiLog request);
        //Task<bool> Log(ResponseSiLog response);
    }
}