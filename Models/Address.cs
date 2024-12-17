public class Address
{
    public string? AddressId { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public int PostalCode { get; set; }
    public string? StateCode { get; set; }
    public string? Colonia { get; set; }
    public string? StreetNumber { get; set; }
    public bool Preferred { get; set; }
    public DateTimeOffset? creationDate { get; set; }
}

