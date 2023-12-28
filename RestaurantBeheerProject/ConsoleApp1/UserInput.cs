using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumeRest {
    public class UserInput {
        public UserInput(string name, string email, string phoneNumber, string municipality, int zipCode, string streetName, string housenumberLabel) {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Municipality = municipality;
            ZipCode = zipCode;
            StreetName = streetName;
            HousenumberLabel = housenumberLabel;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Municipality { get; set; }
        public int ZipCode { get; set; }
        public string StreetName { get; set; }
        public string HousenumberLabel { get; set; }
    }
}
