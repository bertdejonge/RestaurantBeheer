using RestaurantProject.Domain.Exceptions;
using RestaurantProject.Domain.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Models
{
    public class User {
        // Base 
        public User(string name, string email, string phoneNumber, int zipCode, string municipality) {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            ZipCode = zipCode;
            Municipality = municipality;
        }

        // + streetname
        public User(string name, string email, string phoneNumber, int zipCode, string municipality, string streetName) 
            :this(name, email, phoneNumber, zipCode, municipality) {
            StreetName = streetName;
        }

        // + houseNumberLabel
        public User(string name, string email, string phoneNumber, int zipCode, string municipality, string streetName, string houseNumberLabel) 
            : this(name, email, phoneNumber, zipCode, municipality, streetName) {
            HouseNumberLabel = houseNumberLabel;
        }

        private string _name;
		public string Name {
			get { return _name; }
			set {
				if (!string.IsNullOrWhiteSpace(value)) {
					_name = value;
				} else {
					throw new UserException("Invalid name. Please enter a name that contains characters other than whitespace.");
				}
			}
		}

        // LOCATION
        private int _zipCode;
        public int ZipCode {
            get { return _zipCode; }
            set {
                if (value > 0 && value < 9999) {
                    _zipCode = value;
                } else {
                    throw new UserException("Invalid zip code. Please insert a valid zipcode between 0 and 9999.");
                }
            }
        }


        private string _municipality;
        public string Municipality {
            get { return _municipality; }
            set {
                if (!string.IsNullOrWhiteSpace(value)) {
                    _municipality = value;
                } else {
                    throw new UserException("Invalid municipality. Please insert a valid municipality name.");
                }
            }
        }

        private string _streetName;
        public string StreetName {
            get { return _streetName; }
            set {
                if (!string.IsNullOrWhiteSpace(value)) {
                    _streetName = value;
                } else {
                    throw new UserException("Invalid street name");
                }
            }
        }

        private string _houseNumberLabel;

        public string HouseNumberLabel {
            get { return _houseNumberLabel; }
            set {
                if (string.IsNullOrWhiteSpace(value)) { }
                _houseNumberLabel = value;
            }
        }

        private string _email;
        public string Email {
            get { return _email; }
            set {
                if (value.Contains('@')) {
                    if (CheckProperty.CheckEmail(value)) {
                        _email = value;
                    } else {
                        throw new UserException("Invalid email. \"The email address is not in the correct format \\\"john@example.com\\\"\\\"\"");
                    }
                } else {
                    throw new UserException("Invalid email. The email address has to contain an '@'");
                }
            }
        }

        private string _phoneNumber;

        public string PhoneNumber {
            get { return _phoneNumber; }
            set {
                if (CheckProperty.CheckPhoneNumber(value)) {
                    _phoneNumber = value;
                } else {
                    throw new UserException("Invalid phone number. Please enter a phone number that only contains numbers.");
                }
            }
        }


        public int UserID { get; private set; }

		public void SetUserID(int userId) {
			if(userId > 0) {
                UserID = userId;
            }			
		}

        public int GetUserID() {
            return UserID;
        }
		        
        public override string? ToString() {
            bool isValidStreet = string.IsNullOrWhiteSpace(StreetName);
            bool isValidHouseNumber = string.IsNullOrWhiteSpace(HouseNumberLabel);

            if (!isValidStreet && !isValidHouseNumber) {
                return $"User {Name}, in {Municipality}({ZipCode}). \n" +
                       $"ContactInfo: {Email}, {PhoneNumber}";
            } else if (isValidStreet) {
                return $"User {Name}, at {StreetName} ({Municipality}, {ZipCode}). \n" +
                       $"ContactInfo: {Email}, {PhoneNumber}";
            } else if (isValidStreet && isValidHouseNumber) {
                return $"User {Name}, at {StreetName} {HouseNumberLabel} ({Municipality}, {ZipCode}). \n" +
                       $"ContactInfo: {Email}, {PhoneNumber}";
            } else {
                return $"{Name}, in {Municipality}, {ZipCode}). \n" +
                       $"ContactInfo: {Email}, {PhoneNumber}";
            }
        }
    }
}
