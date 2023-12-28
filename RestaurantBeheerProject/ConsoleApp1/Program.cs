using RestaurantProject.Datalayer.Data;
using System.Net.Http.Headers;

namespace ConsumeRest {
    public class Program {
        static HttpClient client = new();

        public static async Task Main(string[] args) {
            //RestaurantDbContext context = new();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            client.BaseAddress = new Uri("http://localhost:5138/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            UserInput input = new("Web4", "Web4@hogent.be", "0444654128", "Gent", 9000, "Henleykaai", "84 Gebouw C");

            Uri createdUri = await CreateUserAsync(input);

            UserOutput user = await GetUserByIDAsync(createdUri.AbsoluteUri);
            Console.WriteLine(user);
        }

        private static async Task<Uri>CreateUserAsync(UserInput input) {
            try {
                HttpResponseMessage response = await client.PostAsJsonAsync("UserRestService/users/adduser/", input);

                response.EnsureSuccessStatusCode();

                return response.Headers.Location;
            } catch (Exception) {

                throw;
            }            
        }

        private static async Task<UserOutput> GetUserByIDAsync(string link) {
            try {
                // Get the generated userID from the POST response 
                string lastCharUrl = link.Substring(link.Length-1);
                int userID = int.Parse(lastCharUrl);


                UserOutput output = null;
                HttpResponseMessage response = await client.GetAsync($"UserRestService/users/{userID}");

                if(response.IsSuccessStatusCode) {
                    output = await response.Content.ReadAsAsync<UserOutput>();
                }

                return output;
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}