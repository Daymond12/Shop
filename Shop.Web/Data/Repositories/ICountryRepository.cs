

namespace Shop.Web.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
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

        //Llenar combo de paises
        IEnumerable<SelectListItem> GetComboCountries();

        //llenar combo de ciudades que pertenecen al pais de parametro 
        IEnumerable<SelectListItem> GetComboCities(int conuntryId);

        //método que le pasamos una ciudad y nos deculeve el pais
        //al que pertenece
        Task<Country> GetCountryAsync(City city);
    }

}
