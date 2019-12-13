

namespace Shop.Web.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Models;

    public interface ICountryRepository : IGenericRepository<Country>
    {
        //traer paises con ciudades
        IQueryable GetCountriesWithCities();

        //traer pais con sus coleción de ciudades
        Task<Country> GetCountryWithCitiesAsync(int id);

        //devolver la ciudad
        Task<City> GetCityAsync(int id);

        //agregar ciudad
        Task AddCityAsync(CityViewModel model);

        //actualizar Ciudad
        Task<int> UpdateCityAsync(City city);

        //borrrar ciudad
        Task<int> DeleteCityAsync(City city);
    }

}
