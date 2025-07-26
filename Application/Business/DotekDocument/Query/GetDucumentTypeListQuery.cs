using Application.Business.DotekDocument.ViewModel;
using Application.Business.DotekRequest.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekDocument.Query;

public class GetDucumentTypeListQuery:IRequest<BaseResult_VM<List<DocumentType_VM>>>
{
    
}
public class GetDucumentTypeListQueryHandler:IRequestHandler<GetDucumentTypeListQuery,BaseResult_VM<List<DocumentType_VM>>>
{
    private readonly IClientMessager _clientMessager;
    private readonly IAuthHelper _authHelper;

    public GetDucumentTypeListQueryHandler(IClientMessager clientMessager,IAuthHelper  authHelper)
    {
        _clientMessager = clientMessager;
        _authHelper = authHelper;
    }
    public async Task<BaseResult_VM<List<DocumentType_VM>>> Handle(GetDucumentTypeListQuery request, CancellationToken cancellationToken)
    {
        var input = new GetDucumentTypeListQueryInput
        {
            Auth = _authHelper.GetDefaultAuth(),
         
        };
        var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<List<DocumentType_VM>>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_GetDucumentTypeList");
       
        if (!methodResult.Success)
        {
            return new BaseResult_VM<List<DocumentType_VM>>(null, methodResult.Code, "خطا در فراخوانی سرویس دریافت لیست نوع  اسناد", false);
        }

        return methodResult.Result;
    }
    private class  GetDucumentTypeListQueryInput
    {
        public OtherServicesAuth_VM Auth { get; set; }
    }
}
