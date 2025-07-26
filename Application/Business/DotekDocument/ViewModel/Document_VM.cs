namespace Application.Business.DotekDocument.ViewModel;

public class Document_VM
{
    public long Id { get; set; }
    public long DocumentTypeId { get; set; }
    public string DocumentTypeName { get; set; }
    public string Name { get; set; }
    public string Base64File { get; set; }
    public string ContentType { get; set; }
    public long Size { get; set; }
    public DateTime UploadDate { get; set; }
    public int Status { get; set; }
    public string? StatusDesc { get; set; }
    public byte[]? File { get; set; }
}