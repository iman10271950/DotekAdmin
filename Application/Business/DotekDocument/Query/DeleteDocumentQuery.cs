using Application.Business.DotekRequest.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekDocument.Query;

public class DeleteDocumentQuery:IRequest<BaseResult_VM<bool>>
{
    public long DocumentId { get; set; }
}
public class DeleteDocumentQueryHandler:IRequestHandler<DeleteDocumentQuery,BaseResult_VM<bool>>
{
    private readonly IClientMessager _clientMessager;
    private readonly IAuthHelper _authHelper;

    public DeleteDocumentQueryHandler(IClientMessager clientMessager,IAuthHelper  authHelper)
    {
        _clientMessager = clientMessager;
        _authHelper = authHelper;
    }
    public async Task<BaseResult_VM<bool>> Handle(DeleteDocumentQuery request, CancellationToken cancellationToken)
    {
        var input = new DeleteDocumentQueryInput
        {
            Auth = _authHelper.GetDefaultAuth(),
            DocumentId = request.DocumentId,
        };
        var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<bool>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_DeleteDocument");
       
        if (!methodResult.Success)
        {
            return new BaseResult_VM<bool>(false, methodResult.Code, "خطا در فراخوانی سرویس حذف اسناد", false);
        }

        return methodResult.Result;
    }

    private class DeleteDocumentQueryInput
    {
        public OtherServicesAuth_VM Auth { get; set; }
        public long DocumentId { get; set; }
        
    }
}