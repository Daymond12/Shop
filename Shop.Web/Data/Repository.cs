

namespace Shop.Web.Data
{
    using Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public class Repository : IRepository
    {
        //El campo se inicializa, ya que si no se hace 
        //el context se inyectará solo en el contructor
        //en cambio al inicializar queda dispoible la conexión para toda la clase
        private readonly DataContext context;
        public Repository(DataContext context)
        {
            this.context = context;

        }

        //IEnumerable es una lista no instanciada
        //se puede convertir en un list, ObservableCollection,- 
        //genericCollesction, cualquier tipo de lista
        public IEnumerable<Product> GetProducts()
        {
            return this.context.Products.OrderBy(p => p.Name);
        }

        public Product GetProduct(int id)
        {
            return this.context.Products.Find(id);
        }

        public void AddProduct(Product product)
        {
            this.context.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            this.context.Update(product);
        }

        public void RemoveProduct(Product product)
        {
            this.context.Products.Remove(product);
        }

        public async Task<bool> SaveAllAsync()
        {
            //espera que los grabe
            //si es mayor a cero devuelve true
            return await this.context.SaveChangesAsync() > 0;
        }

        public bool ProductExists(int id)
        {
            //devuelve si existe o no existe
            //deme cualquier producto en la BD si el id Producto es igual al id del parámetro
            return this.context.Products.Any(p => p.Id == id);
        }


    }
}
