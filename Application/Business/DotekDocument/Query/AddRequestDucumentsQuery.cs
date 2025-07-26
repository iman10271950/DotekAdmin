using Application.Business.DotekDocument.ViewModel;
using Application.Business.DotekRequest.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekDocument.Query;

public class AddRequestDucumentsQuery:IRequest<BaseResult_VM<bool>>
{
    
    public List<AddRequestDocument_VM> Documents { get; set; }
    public long RequestId { get; set; }
}
public class AddRequestDucumentsQueryHandler:IRequestHandler<AddRequestDucumentsQuery,BaseResult_VM<bool>>
{
    private readonly IClientMessager _clientMessager;
    private readonly IAuthHelper _authHelper;

    public AddRequestDucumentsQueryHandler(IClientMessager clientMessager,IAuthHelper  authHelper)
    {
        _clientMessager = clientMessager;
        _authHelper = authHelper;
    }
    public async Task<BaseResult_VM<bool>> Handle(AddRequestDucumentsQuery request, CancellationToken cancellationToken)
    {
        var input = new AddRequestDucumentsQueryInput
        {
            Auth = _authHelper.GetDefaultAuth(),
            RequestId = request.RequestId,
            Documents = request.Documents
        };
        var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<bool>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_AddRequestDucuments");
       
        if (!methodResult.Success)
        {
            return new BaseResult_VM<bool>(false, methodResult.Code, "خطا در فراخوانی سرویس افزودن اسناد", false);
        }

        return methodResult.Result;
    }

    public class AddRequestDucumentsQueryInput
    {
        public OtherServicesAuth_VM Auth { get; set; }
        public List<AddRequestDocument_VM> Documents { get; set; }
        public long RequestId { get; set; }
    }
}