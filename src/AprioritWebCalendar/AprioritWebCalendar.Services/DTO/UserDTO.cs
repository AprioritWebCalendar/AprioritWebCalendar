using System.Collections.Generic;

namespace AprioritWebCalendar.Services.DTO
{
    /// <summary>
    /// API sends this models to front-end after successfull authentication
    /// together with an access token.
    /// </summary>
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
