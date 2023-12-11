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

        public User(string name, ContactInfo contactInfo, Location location) {
            Name = name;
            ContactInfo = contactInfo;
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

		private ContactInfo _contactInfo;
		public ContactInfo ContactInfo {
			get { return _contactInfo; }
			set { _contactInfo = value; }
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
    }
}
