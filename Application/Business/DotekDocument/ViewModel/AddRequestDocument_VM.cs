using Microsoft.AspNetCore.Http;

namespace Application.Business.DotekDocument.ViewModel;

public class AddRequestDocument_VM
{
    public long DocumentTypeId { get; set; }
       
    public IFormFile File { get; set; }
}