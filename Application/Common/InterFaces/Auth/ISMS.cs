using Application.Common.BaseEntities;

namespace Application.Common.Interfaces.Auth
{
    public interface ISMS
    {
      public Task<BaseResult_VM<int>> Send(smsInputSingle input);
    }
}
