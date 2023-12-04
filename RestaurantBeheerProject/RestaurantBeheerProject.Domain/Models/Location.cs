using RestaurantProject.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantProject.Domain.Models {
	public class Location {

        public Location(string municipality) {
            Municipality = municipality;
        }

        public Location(int zipCode, string municipality) : this(municipality){
            ZipCode = zipCode;
        }

        public Location(int zipCode, string municipality, string streetName, string houseNumberLabel) : this(zipCode, municipality) {
            StreetName = streetName;
            HouseNumberLabel = houseNumberLabel;
        }

        private int _zipCode;
		public int ZipCode {
			get { return _zipCode; }
			set {
				if (value > 0 && value < 9999) {
					_zipCode = value;
				} else {
					throw new LocationException("Invalid zip code. Please insert a valid zipcode between 0 and 9999.");
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
					throw new LocationException("Invalid municipality. Please insert a valid municipality name.");
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
					throw new LocationException("Invalid street name");
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

	}
}
