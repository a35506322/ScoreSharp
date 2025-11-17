namespace ScoreSharp.Common.Helpers.Address;

public class AddressInfoDto
{
    public string City { get; set; }
    public string Area { get; set; }
    public string Road { get; set; }
    public string ZipCode { get; set; }
    public string Scope { get; set; }
    public string[] ScopeArray
    {
        get
        {
            try
            {
                return JsonHelper.反序列化物件不分大小寫<string[]>(Scope);
            }
            catch
            {
                return Array.Empty<string>();
            }
        }
    }
}
