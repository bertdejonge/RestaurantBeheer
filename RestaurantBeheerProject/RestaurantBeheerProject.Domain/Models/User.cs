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

        public User(string name, string email, string phoneNumber, Location location) {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Location = location;
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

		private string _email;
		public string Email {
			get { return _email; }
			set {  try {
					if (!string.IsNullOrWhiteSpace(value) && CheckProperty.CheckEmail(value)) {
						_email = value;
					} else {
						throw new UserException("Please enter an email that contains an @-sign.");
					}
				} catch(Exception ex) {
					throw new UserException($"Invalid email. {ex.Message}");
				}
			}
		}

		private string _phoneNumber;
		public string PhoneNumber {
			get { return _phoneNumber; }
			set {
				try {
					if (!string.IsNullOrEmpty(value) && CheckProperty.CheckPhoneNumber(value)) {
						_phoneNumber = value;
					} else {
						throw new UserException("Phone number should only contain numbers");
					}
				}catch(Exception ex) {
					throw new UserException($"Invalid phone number. {ex.Message}");
				}
			}
		}

		private Location _location;
		public Location Location {
			get { return _location; }
			set { _location = value; }
		}		

		public int UserId { get; private set; }

		public void SetUserId(int userId) {
			if(userId > 0) {
                UserId = userId;
            }			
		}
		        

        public List<Reservation> Reservations = new List<Reservation>();


		// Class diagram code
        public Reservation Reservation {
            get => default;
            set {
            }
        }

        public Location Location1 {
            get => default;
            set {
            }
        }
    }
}
