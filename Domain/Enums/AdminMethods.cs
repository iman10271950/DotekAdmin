namespace Domain.Enums;

public enum AdminMethods
{
    #region  Request
    Dotek_GetAllUserForAdmin=100001,
    Dotek_GetAllRoles=100002,
    Dotek_GetUserInformationWithIdInAdmin=100003,
    Dotek_UpdateUser=100004,
    Dotek_UpdateRolleInAdmin=100005,
    Dotek_DeleteDotekRolle=100006,
    Dotek_InactiveDotekUser=100007,
    Dotek_InsertProductInAdmin=100008,
    Dotek_GetProductWithId=100009,
    Dotek_DeleteProduct=100010,
    Dotek_GetAllProductListWithFilter=100011,
    Dotek_GetlAllDotekRequest=100012,
    Dotek_GUpdateDotekRequestStatus=100013,
    

    #endregion
    
    #region  User 

    User_ExistUser=200001,
    User_CreateAccount=200002,
    User_Login=200003,
    User_UpdateUserCommand=200004,
    #endregion

    #region  Payment

    Payment_GetBaseWalletInformation=300001,   
    Payment_ConfrimUserWithdrawRequest=300002,
    #endregion
 
}