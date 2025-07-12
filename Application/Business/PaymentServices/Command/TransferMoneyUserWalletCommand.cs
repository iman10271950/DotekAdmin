using Application.Common.BaseEntities;
using MediatR;

namespace Application.Business.PaymentServices.Command;

public class TransferMoneyUserWalletCommand:IRequest<BaseResult_VM<bool>>
{
    
}