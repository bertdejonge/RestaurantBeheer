using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumeRest {
    public class UserOutput {
        public UserOutput(int id, string name, string email, string phoneNumber, string municipality, int zipCode, string? streetName, string? houseNumberLabel) {
            Id = id;
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Municipality = municipality;
            ZipCode = zipCode;
            TrySetStreetAndHouseNumber(streetName, houseNumberLabel);
        }

        private void TrySetStreetAndHouseNumber(string streetName, string houseNumberLabel) {
            bool isValidStreet = string.IsNullOrWhiteSpace(StreetName);
            bool isValidHouseNumber = string.IsNullOrWhiteSpace(HouseNumberLabel);

            if (isValidStreet && isValidHouseNumber) {
                StreetName = streetName;
                HouseNumberLabel = houseNumberLabel;
            } else if (isValidStreet) {
                StreetName = streetName;
            }
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Municipality { get; set; }
        public int ZipCode { get; set; }
        public string? StreetName { get; set; }
        public string? HouseNumberLabel { get; set; }

        public override string? ToString() {
            bool isValidStreet = string.IsNullOrWhiteSpace(StreetName);
            bool isValidHouseNumber = string.IsNullOrWhiteSpace(HouseNumberLabel);

            if (!isValidStreet && !isValidHouseNumber) {
                return $"({Id}) User {Name}, in {Municipality}({ZipCode}). " +
                       $"ContactInfo: {Email}, {PhoneNumber}";
            } else if (isValidStreet) {
                return $"({Id}) User {Name}, at {StreetName} ({Municipality}, {ZipCode}). " +
                       $"ContactInfo: {Email}, {PhoneNumber}";
            } else if (isValidStreet && isValidHouseNumber) {
                return $"({Id}) User {Name}, at {StreetName} {HouseNumberLabel} ({Municipality}, {ZipCode}). " +
                       $"ContactInfo: {Email}, {PhoneNumber}";
            } else {
                return $"({Id}) User {Name}, in {Municipality}, {ZipCode}). " +
                       $"ContactInfo: {Email}, {PhoneNumber}";
            }
        }
    }
}
