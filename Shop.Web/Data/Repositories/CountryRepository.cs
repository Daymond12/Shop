
namespace Shop.Web.Data.Entities
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Linq;
    using System.Threading.Tasks;

    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext context;

        public CountryRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        //agregue una ciudad a un país
        public async Task AddCityAsync(CityViewModel model)
        {
            //busqueme el país
            var country = await this.GetCountryWithCitiesAsync(model.CountryId);
            if (country == null)
            {
                //si no lo encuentra , !pa fuera
                return;
            }

            //lo encuentra, agregue una nueva ciudad
            country.Cities.Add(new City { Name = model.Name });
            this.context.Countries.Update(country);
            await this.context.SaveChangesAsync();
        }

        public async Task<int> DeleteCityAsync(City city)
        {
            var country = await this.context.Countries.Where(c => c.Cities.Any(ci => ci.Id == city.Id)).FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            this.context.Cities.Remove(city);
            await this.context.SaveChangesAsync();
            return country.Id;
        }

        public IQueryable GetCountriesWithCities()
        {
            return this.context.Countries
                .Include(c => c.Cities)
                .OrderBy(c => c.Name);
        }

        public async Task<Country> GetCountryWithCitiesAsync(int id)
        {
            //deme toda la colecion de paises
            return await this.context.Countries
                //incluyendo la colección de ciudades, e.d inner join
                .Include(c => c.Cities)
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateCityAsync(City city)
        {
            var country = await this.context.Countries.Where(c => c.Cities.Any(ci => ci.Id == city.Id)).FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            this.context.Cities.Update(city);
            await this.context.SaveChangesAsync();
            return country.Id;
        }

        public async Task<City> GetCityAsync(int id)
        {
            return await this.context.Cities.FindAsync(id);
        }
    }



}
