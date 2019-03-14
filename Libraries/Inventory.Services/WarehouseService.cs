using Inventory.Core;
using Inventory.Data;
using Inventory.Data.Repository;
using Inventory.Domain.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Inventory.Services
{
    public class WarehouseService : Service<Warehouse>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        protected readonly InventoryContext db;
        protected UserManager<ApplicationUser> userManager;
        public WarehouseService(IRepository<Warehouse> repository,
        IRepository<ApplicationUser> userRepository)
        : base(repository)
        {
            db = new InventoryContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            _userRepository = userRepository;
        }
        public IQueryable<WarehouseViewModel> QueryableViewModel(SearchingViewModel model)
        {
            return Queryable().Where(x => x.Delete == false 
            && ((!model.Id.HasValue) || x.Id == model.Id)
            ).Select(x => new WarehouseViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Code = x.WarehouseCode,
                PhoneNumber = x.PhoneNumber,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate,
            });
        }
        public Warehouse AddWarehouse(WarehouseViewModel model)
        {
            if (Queryable().Any(x => x.Name.ToLower().Contains(model.Name.ToLower()) && x.Delete == false))
            {
                throw new Exception("Đã tồn tại kho");
            }
            else
            {
                var result = new Warehouse();
                result.Name = model.Name;
                result.Address = model.Address;
                result.PhoneNumber = model.PhoneNumber;
                result.WarehouseCode = model.Code;
                result.User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId());
                result.CreatedDate = DateTime.Now;
                result.LastModifiedDate = DateTime.Now;
                Insert(result);
                return result;
            }
        }
        public bool UpdateWarehouse(WarehouseViewModel model)
        {
            var Warehouse = Find(model.Id.Value);
            if (Warehouse != null)
            {

                Warehouse.Name = model.Name;
                Warehouse.Address = model.Address;
                Warehouse.PhoneNumber = model.PhoneNumber;
                Warehouse.User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId());
                Warehouse.WarehouseCode = model.Code;
                Warehouse.LastModifiedDate = DateTime.Now;
                Update(Warehouse);
                return true;

            }
            else
            {
                throw new Exception("Không tìm thấy kho");

            }
        }
        public bool DeleteWarehouse (Guid Id)
        {
            var Warehouse = Find(Id);
            if (Warehouse != null)
            {
                Warehouse.Delete = true;
                Update(Warehouse);
                return true;
            }
            else
            {
                throw new Exception("Không tìm tháy kênh cần xóa");
            }
        }
    }
}
