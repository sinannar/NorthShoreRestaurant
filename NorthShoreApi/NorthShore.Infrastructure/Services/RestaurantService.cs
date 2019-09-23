using NorthShore.Domain.Entities;
using NorthShore.Domain.Repositories;
using NorthShore.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthShore.Infrastructure.Services
{
    public class RestaurantService : IRestaurantService
    {
        private IFoodRepository _foodRepository { get; set; }
        private IMenuRepository _menuRepository { get; set; }
        private IFoodMenuMappingRepository _foodMenuMappingRepository { get; set; }
        private IUnitOfWork _unitOfWork { get; set; }

        public RestaurantService(
            IFoodRepository foodRepository,
            IMenuRepository menuRepository,
            IFoodMenuMappingRepository foodMenuMappingRepository,
            IUnitOfWork unitOfWork
            )
        {
            _foodRepository = foodRepository;
            _menuRepository = menuRepository;
            _foodMenuMappingRepository = foodMenuMappingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateFood(Food food)
        {
            await _foodRepository.InsertAsync(food);
            await _unitOfWork.Commit();
        }

        public async Task CreateMenu(Menu menu)
        {
            await _menuRepository.InsertAsync(menu);
            await _unitOfWork.Commit();
        }

        public async Task EditFood(Food food)
        {
            await _foodRepository.UpdateAsync(food);
            await _unitOfWork.Commit();
        }

        public async Task DeleteFood(Food food)
        {
            await _foodRepository.DeleteAsync(food);
            await _unitOfWork.Commit();
        }

        public async Task DeleteMenu(Menu menu)
        {
            await _menuRepository.DeleteAsync(menu);
            await _unitOfWork.Commit();
        }

        public IQueryable<Food> ListFood()
        {
            return _foodRepository.GetAllIncluding(m => m.MenuMappings);
        }

        public IQueryable<Menu> ListMenu()
        {
            return _menuRepository.GetAllIncluding(m => m.FoodMappings);
        }

        public Menu GetMenuWithMappings(long menuId)
        {
            return this.ListMenu()
                .Where(m => m.Id == menuId)
                .FirstOrDefault();
        }


        public IQueryable<Food> ListMenuFoods(IEnumerable<FoodMenuMapping> mapping)
        {
            if (mapping == null)
                return new List<Food>().AsQueryable();
            else
                return _foodRepository
                    .GetAll()
                    .Where(food => mapping.Where(map => food.Id == map.FoodId).Any());
        }
        public IQueryable<Food> ListNonMenuFoods(IEnumerable<FoodMenuMapping> mapping)
        {
            if (mapping == null)
                return _foodRepository.GetAll();
            else
                return _foodRepository
                    .GetAll()
                    .Where(food => !mapping.Where(map => food.Id == map.FoodId).Any());
        }

        public IQueryable<Menu> ListFoodMenus(IEnumerable<FoodMenuMapping> mapping)
        {
            if (mapping == null)
                return new List<Menu>().AsQueryable();
            else
                return _menuRepository
                    .GetAll()
                    .Where(menu => mapping.Where(map => menu.Id == map.MenuId).Any());
        }

        public async Task DeleteFoodMenuMapping(long foodId, long menuId)
        {
            var existingMapping = _foodMenuMappingRepository
                .GetAll()
                .Where(m => m.FoodId == foodId && m.MenuId == menuId)
                .FirstOrDefault();
            if (existingMapping == null)
            {
                throw new Exception($"Food with ID:{foodId} and Menu with ID:{menuId} is not exist");
            }
            else
            {
                await DeleteFoodMenuMapping(existingMapping);
                await _unitOfWork.Commit();
            }
        }
        public async Task DeleteFoodMenuMapping(FoodMenuMapping mapping)
        {
            await _foodMenuMappingRepository.DeleteAsync(mapping);
            await _unitOfWork.Commit();
        }

        public async Task CreateFoodMenuMapping(long foodId, long menuId)
        {
            var existingMapping = _foodMenuMappingRepository.GetAll().Where(m => m.FoodId == foodId && m.MenuId == menuId).FirstOrDefault();
            if (existingMapping != null)
            {
                throw new Exception($"Food with ID:{foodId} and Menu with ID:{menuId} already has mapping");
            }
            else
            {
                await _foodMenuMappingRepository.InsertAsync(new FoodMenuMapping
                {
                    MenuId = menuId,
                    FoodId = foodId
                });
                await _unitOfWork.Commit();
            }
        }
        public async Task CreateFoodMenuMapping(long foodId, List<long> menuIds)
        {
            foreach (var menuId in menuIds)
            {
                await CreateFoodMenuMapping(foodId, menuId);
            }
            await _unitOfWork.Commit();
        }

        public async Task CreateFoodMenuMapping(List<long> foodIds, long menuId)
        {
            foreach (var foodId in foodIds)
            {
                await CreateFoodMenuMapping(foodId, menuId);
            }
            await _unitOfWork.Commit();
        }

        public async Task UpdateMenuValues(long menuId)
        {
            var menu = GetMenuWithMappings(menuId);
            var foods = ListMenuFoods(menu.FoodMappings);
            menu.TotalPrice = !foods.Any() ? 0 : foods.Sum(food => food.Price);
            menu.TotalCalorie = !foods.Any() ? 0 : foods.Sum(food => food.Calorie);
            menu.DiscountedPrice = !foods.Any() ? 0 : menu.TotalPrice * (1 - (menu.DiscountRate / 100));
            await _menuRepository.UpdateAsync(menu);
            await _unitOfWork.Commit();
        }
    }
}
