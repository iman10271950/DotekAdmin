using Application.Business.DotekRequest.ViewModel;
using Application.Common.BaseEntities;
using Application.Common.InterFaces.Messager;
using Application.Common.InterFaces.Services;
using Application.Common.Messager.Enums;
using Application.Common.Models;
using MediatR;
using Newtonsoft.Json;

namespace Application.Business.DotekRequest.Query;

public class GetAllInvoiceListQuery:IRequest<BaseResult_VM<PaginatedList<Invoice_VM>>>
{
    public OtherServicesAuth_VM Auth { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
public class GetAllInvoiceListQueryHandler:IRequestHandler<GetAllInvoiceListQuery,BaseResult_VM<PaginatedList<Invoice_VM>>>
{
    private readonly IClientMessager _clientMessager;
    private readonly IAuthHelper _auth;

    public GetAllInvoiceListQueryHandler(IClientMessager clientMessager, IAuthHelper auth)
    {
        _clientMessager = clientMessager;
        _auth = auth;
    }
    public async Task<BaseResult_VM<PaginatedList<Invoice_VM>>> Handle(GetAllInvoiceListQuery request, CancellationToken cancellationToken)
    {
        request.Auth = _auth.GetDefaultAuth();
        var methodResult = await _clientMessager.CallMethodDirectly<BaseResult_VM<PaginatedList<Invoice_VM>>>(MicroServiceName.Dotek, JsonConvert.SerializeObject(request), "Admin_GetAllInvoiceForAdmin");
        if (methodResult.Code == 103)
        {
            return methodResult.Result;
        }
        if (!methodResult.Success)
        {
            return new BaseResult_VM<PaginatedList<Invoice_VM>>(null, methodResult.Code, "خطا در فراخوانی سرویس دریافت لیست فاکتور ها", false);
        }

        return methodResult.Result;
    }
}