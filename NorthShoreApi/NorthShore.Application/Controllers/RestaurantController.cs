using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NorthShore.Domain.Repositories;
using NorthShore.Domain.Services;
using NorthShore.Payload.Adapters;
using NorthShore.Payload.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthShore.Application.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RestaurantController : Controller
    {
        private IFoodRepository _foodRepository { get; set; }
        private IMenuRepository _menuRepository { get; set; }
        private IFoodMenuMappingRepository _foodMenuMappingRepository { get; set; }
        private IRestaurantService _restaurantService { get; set; }
        private IUnitOfWork _unitOfWork { get; set; }

        public RestaurantController(
            IFoodRepository foodRepository,
            IMenuRepository menuRepository,
            IFoodMenuMappingRepository foodMenuMappingRepository,
            IRestaurantService restaurantService,
            IUnitOfWork unitOfWork
            )
        {
            _foodRepository = foodRepository;
            _menuRepository = menuRepository;
            _foodMenuMappingRepository = foodMenuMappingRepository;
            _restaurantService = restaurantService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task CreateFood(CreateFoodDto requestDto)
        {
            var adapter = new CreateFoodAdapter();
            await _restaurantService.CreateFood(adapter.Transform(requestDto));
        }

        [HttpPost]
        public async Task CreateMenu(CreateMenuDto requestDto)
        {
            var adapter = new CreateMenuAdapter();
            await _restaurantService.CreateMenu(adapter.Transform(requestDto));
        }

        [HttpPost]
        public async Task EditFood(EditFoodDto request)
        {
            var entity = _foodRepository.Get(request.Id);
            if (entity != null)
            {
                var adapter = new EditFoodAdapter();
                await _restaurantService.EditFood(adapter.Transform(request, entity));
            }
            else
            {
                throw new Exception("Given food is not found to delete");
            }
        }

        [HttpPost]
        public async Task DeleteFood(long requestId)
        {
            var entity = await _foodRepository.GetAsync(requestId);
            if (entity != null)
            {
                await _restaurantService.DeleteFood(entity);
            }
            else
            {
                throw new Exception("Given food is not found to delete");
            }

        }

        [HttpPost]
        public async Task DeleteMenu(long requestId)
        {
            var entity = await _menuRepository.GetAsync(requestId);
            if (entity != null)
            {
                await _restaurantService.DeleteMenu(entity);
            }
            else
            {
                throw new Exception("Given menu is not found to delete");
            }
        }

        [HttpPost]
        public List<ShowFoodDto> ListFoods()
        {
            var adapter = new ListFoodAdapter();
            var list = _restaurantService.ListFood();
            return adapter.Transform(list);
        }

        [HttpPost]
        public List<ShowMenuDto> ListMenus()
        {
            var adapter = new ListMenuAdapter();
            var list = _restaurantService.ListMenu();
            return adapter.Transform(list);
        }

        [HttpPost]
        public List<ShowFoodDto> ListFoodsInMenu(long menuId)
        {
            var menu = _restaurantService.GetMenuWithMappings(menuId);
            if (menu != null)
            {
                var menuFoodMappings = menu.FoodMappings;
                var mappedFoods = _restaurantService.ListMenuFoods(menuFoodMappings);
                var adapter = new ListFoodAdapter();
                return adapter.Transform(mappedFoods);
            }
            else
            {
                throw new Exception("Given menu is not found");
            }
        }

        [HttpPost]
        public List<ShowFoodDto> ListFoodsNotInMenu(long menuId)
        {
            var menu = _restaurantService.GetMenuWithMappings(menuId);
            if (menu != null)
            {
                var menuFoodMappings = menu.FoodMappings;
                var mappedFoods = _restaurantService.ListNonMenuFoods(menuFoodMappings);
                var adapter = new ListFoodAdapter();
                return adapter.Transform(mappedFoods);
            }
            else
            {
                throw new Exception("Given menu is not found");
            }
        }

        [HttpPost]
        public async Task AddFoodToMenu(AddFoodToMenuDto request)
        {
            await _restaurantService.CreateFoodMenuMapping(request.FoodIds, request.MenuId);
            await _unitOfWork.Commit();
            await _restaurantService.UpdateMenuValues(request.MenuId);
        }

        [HttpPost]
        public async Task RemoveFoodFromMenu(RemoveFoodFromMenuDto request)
        {
            await _restaurantService.DeleteFoodMenuMapping(request.FoodId, request.MenuId);
            await _unitOfWork.Commit();
            await _restaurantService.UpdateMenuValues(request.MenuId);
        }
    }
}
