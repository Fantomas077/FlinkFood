using FlinkFood.Data;
using FlinkFood.Repository.IRepository;
using FlinkFood.Services.Extensions;

namespace FlinkFood.Services
{
    public class ProductService
    {
        private readonly IProductRepository _cat;
        public ProductService(IProductRepository cat)
        {
            _cat = cat;

        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _cat.GetAllAsync();
        }
        public async Task<Product?> GetById(int id)
        {
            var obj = await _cat.FindById(id);
            if (obj == null)
            {
                throw new NotFoundException("Id not foound");
            }
            return obj;
        }
        public async Task AddProductAsync(Product obj)
        {

            obj.Name = obj.Name.Trim().ToLowerInvariant();


            if (await _cat.ExistsByNameAsync(obj.Name))
            {
                throw new ProductAlreadyExisted($"{obj.Name} already exists");
            }

            await _cat.AddAsync(obj);
        }

        public async Task UpdateProductAsync(Product obj)
        {

            obj.Name = obj.Name.Trim().ToLowerInvariant();


            var existing = await _cat.FindById(obj.Id);
            if (existing == null)
            {
                throw new NotFoundException("Id not found");
            }


            if (await _cat.ExistsByNameAsync(obj.Name, excludeId: obj.Id))
            {
                throw new ProductAlreadyExisted($"{obj.Name} already exists");
            }


            existing.Name = obj.Name;
            await _cat.UpdateAsync(existing);
        }
        public async Task DeleteProductAsync(Product obj)
        {

            var existing = await _cat.FindById(obj.Id);
            if (existing == null)
            {
                throw new NotFoundException("Id not found");
            }

            await _cat.DeleteAsync(existing);
        }
    }
}

