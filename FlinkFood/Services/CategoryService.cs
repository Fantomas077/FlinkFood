using FlinkFood.Data;
using FlinkFood.Repository.IRepository;
using FlinkFood.Services.Extensions;

namespace FlinkFood.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _cat;
        public CategoryService(ICategoryRepository cat)
        {
            _cat = cat;

        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _cat.GetAllAsync();
        }
        public async Task<Category?> GetById(int id)
        {
            var obj = await _cat.FindById(id);
            if (obj == null)
            {
                throw new NotFoundException("Id not foound");
            }
            return obj;
        }
        public async Task AddCategoryAsync(Category category)
        {
           
            category.Name = category.Name.Trim().ToLowerInvariant();

            
            if (await _cat.ExistsByNameAsync(category.Name))
            {
                throw new CategoryAlreadyExisted($"{category.Name} already exists");
            }

            await _cat.AddAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            
            category.Name = category.Name.Trim().ToLowerInvariant();

            
            var existing = await _cat.FindById(category.Id);
            if (existing == null)
            {
                throw new NotFoundException("Id not found");
            }

            
            if (await _cat.ExistsByNameAsync(category.Name, excludeId: category.Id))
            {
                throw new CategoryAlreadyExisted($"{category.Name} already exists");
            }

            
            existing.Name = category.Name;
            await _cat.UpdateAsync(existing);
        }
        public async Task DeleteCategoryAsync(Category category)
        {

            var existing = await _cat.FindById(category.Id);
            if (existing == null)
            {
                throw new NotFoundException("Id not found");
            }

            await _cat.DeleteAsync(existing);
        }
        


    }
}
