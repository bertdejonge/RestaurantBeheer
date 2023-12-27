using Microsoft.AspNetCore.Mvc;
using RestaurantProject.API.Mappers;
using RestaurantProject.API.Models.Input;
using RestaurantProject.API.Models.Output;
using RestaurantProject.Datalayer.Data;
using RestaurantProject.Domain.Interfaces;
using RestaurantProject.Domain.Models;

namespace RestaurantProject.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AdminRestService : Controller {
        private readonly IUserService _userService;
        private readonly IRestaurantService _restaurantService;
        private readonly IReservationService _reservationService;
        private readonly RestaurantDbContext _context = new();

        public AdminRestService(IUserService userService, IRestaurantService restaurantService, IReservationService reservationService) {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _restaurantService = restaurantService ?? throw new ArgumentNullException(nameof(restaurantService));
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        }


    [HttpGet("reservations/{restaurantID}")]
    public async Task<ActionResult<List<ReservationOutputDTO>>> GetReservationsRestaurantForDateOrRangeAsync(int restaurantID, DateOnly date, DateOnly? optionalDate) {
        var reservations = await _reservationService.GetReservationsForRestaurantForDateOrRangeAsync(restaurantID, date, optionalDate);

        if(reservations.Count == 0) {
            return BadRequest($"No reservations found for given date or range {date} - {optionalDate} ");
        }

        var reservationsOutput = reservations.Select(r => MapFromDomain.MapFromReservationDomain(r)).ToList();
        return Ok(reservationsOutput);
    }


    [HttpPost("restaurants/addrestaurant")]
    public async Task<ActionResult<RestaurantOutputDTO>> CreateRestaurantAsync([FromBody] RestaurantInputDTO input) {
        try {
            if(input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            Restaurant domainRestaurant = MapToDomain.MapToRestaurantDomain(input);
            Restaurant domainRestaurantOutput = await _restaurantService.CreateRestaurantAsync(domainRestaurant);
            RestaurantOutputDTO restaurantOutput = MapFromDomain.MapFromRestaurantDomain(domainRestaurantOutput);
            return Created("test", restaurantOutput);
        } catch (Exception) {

            throw;
        }
    }

    [HttpPut("restaurants/update/{restaurantID}")]
    public async Task<ActionResult<RestaurantOutputDTO>> UpdateRestaurantAsync(int restaurantID, [FromBody] RestaurantInputDTO input) {
        try {
            if (input == null) {
                throw new ArgumentNullException(nameof(input));
            }

            Restaurant domainRestaurant = MapToDomain.MapToRestaurantDomain(input);
            
            if(await _restaurantService.ExistingRestaurantAsync(domainRestaurant)) {

            }

        } catch (Exception) {
            throw;
        }
    }
}
