using Application.Business.DotekDocument.ViewModel;
using Application.Business.DotekRequest.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekDocument.Query;

public class GetRequestDocumentQuery:IRequest<BaseResult_VM<List<Document_VM>>>
{
    public long RequestId  { get; set; } 
   
}
public class GetRequestDocumentQueryHandler:IRequestHandler<GetRequestDocumentQuery,BaseResult_VM<List<Document_VM>>>
{
    private readonly IClientMessager _clientMessager;
    private readonly IAuthHelper _authHelper;

    public GetRequestDocumentQueryHandler(IClientMessager clientMessager,IAuthHelper  authHelper)
    {
        _clientMessager = clientMessager;
        _authHelper = authHelper;
    }
    public async Task<BaseResult_VM<List<Document_VM>>> Handle(GetRequestDocumentQuery request, CancellationToken cancellationToken)
    {
        var input = new GetRequestDocumentQueryInput
        {
            Auth = _authHelper.GetDefaultAuth(),
            RequestId = request.RequestId,
        };
        var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<List<Document_VM>>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_GetRequestDocument");
       
        if (!methodResult.Success)
        {
            return new BaseResult_VM<List<Document_VM>>(null, methodResult.Code, "خطا در فراخوانی سرویس دریافت لیست اسناد", false);
        }

        return methodResult.Result;
    }

    private class GetRequestDocumentQueryInput
    {
        public OtherServicesAuth_VM Auth { get; set; }
        public long RequestId  { get; set; } 
    }
}