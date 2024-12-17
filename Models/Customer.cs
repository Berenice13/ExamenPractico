public class Customer
{
    public string? CustomerId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneMobile { get; set; }
    public List<Address> Direcciones { get; set; }
    public string? Birthday { get; set; }
}