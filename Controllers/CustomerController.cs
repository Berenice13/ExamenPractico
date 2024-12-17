using Microsoft.AspNetCore.Mvc;
using CustomerApi.Services;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CustomerApi.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetCustomerInfo()
        {
            var customerData = await _customerService.GetCustomerDataAsync();
            return Ok(customerData);
        }

        [HttpGet("importantInfo")]
        public async Task<IActionResult> GetCustomerInfoImportant()
        {
            var customerData = await _customerService.GetCustomerApiResponseAsync();

            if (customerData == null)
            {
                return StatusCode(500, "Error al obtener datos del cliente");
            }

            try
            {
                var jsonObject = JObject.Parse(customerData);
                 var addresses = jsonObject["addresses"]
                    .Select(a => new Address
                    {
                        AddressId = (string)a["addressId"],
                        Address1 = (string)a["address1"],
                        Address2 = (string)a["address2"],
                        City = (string)a["city"],
                        PostalCode = (int)a["postalCode"],
                        StateCode = (string)a["stateCode"],
                        Colonia = (string)a["colonia"],
                        StreetNumber = (string)a["streetNumber"],
                        creationDate = (DateTimeOffset)a["creationDate"]
                    })
                    .ToList();

                var customer = new Customer
                    {
                        CustomerId = (string)jsonObject["customerId"],
                        FirstName = (string)jsonObject["firstName"],
                        LastName = (string)jsonObject["lastName"],
                        Email = (string)jsonObject["email"],
                        PhoneMobile = (string)jsonObject["phoneMobile"],
                        Direcciones = addresses,
                        Birthday = (string)jsonObject["birthday"],
                    };

                Console.WriteLine($"Customer ID: {customer.CustomerId}");
                Console.WriteLine("Direcciones Preferidas:");
                return Ok(customer);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, $"Error al procesar la respuesta: {e.Message}");
            }
        }

        [HttpGet("sortedAddresses")]
        public async Task<IActionResult> GetSortedAddresses([FromQuery] string orderBy = "Address1", [FromQuery] string sortOrder = "asc")
        {
            var customerData = await _customerService.GetCustomerApiResponseAsync();

            if (customerData == null)
            {
                return StatusCode(500, "Error al obtener datos del cliente");
            }

            try
            {
                var jsonObject = JObject.Parse(customerData);
                var addresses = jsonObject["addresses"]
                    .Select(a => new Address
                    {
                        AddressId = (string)a["addressId"],
                        Address1 = (string)a["address1"],
                        Address2 = (string)a["address2"],
                        City = (string)a["city"],
                        PostalCode = (int)a["postalCode"],
                        StateCode = (string)a["stateCode"],
                        Colonia = (string)a["colonia"],
                        StreetNumber = (string)a["streetNumber"],
                        creationDate = (DateTimeOffset)a["creationDate"]
                    })
                    .ToList();

                IEnumerable<Address> sortedAddresses;


                switch (orderBy.ToLower())
                {
                    case "address1":
                        sortedAddresses = sortOrder.ToLower() == "desc"
                            ? addresses.OrderByDescending(a => a.Address1)
                            : addresses.OrderBy(a => a.Address1);

                            
                        Console.WriteLine(orderBy.ToLower());
                        Console.WriteLine(sortOrder.ToLower());


                        break;

                    case "creationdate":
                        sortedAddresses = sortOrder.ToLower() == "desc"
                            ? addresses.OrderByDescending(a => a.creationDate)
                            : addresses.OrderBy(a => a.creationDate);
                        break;

                    default:
                        return BadRequest("Parámetro 'orderBy' no válido. Usa 'Address1' o 'creationDate'.");
                }

                return Ok(sortedAddresses);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error al procesar la respuesta: {e.Message}");
            }
        }

        [HttpGet("preferredAddress")]
        public async Task<IActionResult> GetPreferredAddress()
        {
            var customerData = await _customerService.GetCustomerApiResponseAsync();

            if (customerData == null)
            {
                return StatusCode(500, "Error al obtener datos del cliente");
            }

            try
            {
                var jsonObject = JObject.Parse(customerData);
                var addresses = jsonObject["addresses"]
                    .Where(a => (bool?)a["preferred"] == true)
                    .Select(a => new Address
                    {
                        Preferred = (bool)a["preferred"],
                        AddressId = (string)a["addressId"],
                        Address1 = (string)a["address1"],
                        Address2 = (string)a["address2"],
                        City = (string)a["city"],
                        PostalCode = (int)a["postalCode"],
                        StateCode = (string)a["stateCode"],
                        Colonia = (string)a["colonia"],
                        StreetNumber = (string)a["streetNumber"],
                        creationDate = (DateTimeOffset)a["creationDate"]
                    })
                    .ToList();


                return Ok(addresses);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error al procesar la respuesta: {e.Message}");
            }
        }

        [HttpGet("addressPostalCode")]
        public async Task<IActionResult> GetAddressPostalCode([FromQuery] int PostalCode)
        {
            var customerData = await _customerService.GetCustomerApiResponseAsync();

            if (customerData == null)
            {
                return StatusCode(500, "Error al obtener datos del cliente");
            }

            try
            {
                var jsonObject = JObject.Parse(customerData);
                var addresses = jsonObject["addresses"]
                    .Where(a => a["postalCode"] != null && (int)a["postalCode"] == PostalCode)
                    .Select(a => new Address
                    {
                        Preferred = (bool)a["preferred"],
                        AddressId = (string)a["addressId"],
                        Address1 = (string)a["address1"],
                        Address2 = (string)a["address2"],
                        City = (string)a["city"],
                        PostalCode = (int)a["postalCode"],
                        StateCode = (string)a["stateCode"],
                        Colonia = (string)a["colonia"],
                        StreetNumber = (string)a["streetNumber"],
                        creationDate = (DateTimeOffset)a["creationDate"]
                    })
                    .ToList();


                return Ok(addresses);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Error al procesar la respuesta: {e.Message}");
            }
        }
    }
}
