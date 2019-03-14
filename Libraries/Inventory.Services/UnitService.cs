using Inventory.Core;
using Inventory.Data;
using Inventory.Data.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public class UnitService
    {
        //public class UnitService : Service<Unit>
        //{
        //    private readonly IRepository<ApplicationUser> _userRepository;
        //    protected readonly InventoryContext db;
        //    protected UserManager<ApplicationUser> userManager;
        //    public UnitService(IRepository<Unit> repository,
        //    IRepository<ApplicationUser> userRepository)
        //    : base(repository)
        //    {
        //        db = new InventoryContext();
        //        userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        //        _userRepository = userRepository;
        //    }
        //    public IQueryable<ProductViewModel> QueryableViewModel()
        //    {
        //        return Queryable().Where(x => x.Delete == false).Select(x => new ProductViewModel
        //        {
        //            Id = x.Id,
        //            ProductName = x.ProductName,
        //            Code = x.Code,
        //            Price = x.Price,
        //            Note = x.Note
        //        });
        //    }
        //    public Product AddProduct(ProductViewModel model)
        //    {
        //        if (Queryable().Any(x => x.ProductName.ToLower().Contains(model.ProductName.ToLower()) && x.Delete == false))
        //        {
        //            throw new Exception("Đã tồn tại tên sản phẩm này");
        //        }
        //        else
        //        {
        //            var result = new Product();
        //            result.ProductName = model.ProductName;
        //            result.CreatedDate = DateTime.Now;
        //            result.LastModifiedDate = DateTime.Now;
        //            result.Code = model.Code;
        //            result.Price = model.Price;
        //            result.Note = model.Note;
        //            Insert(result);
        //            return result;
        //        }
        //    }
        //    public bool UpdateProduct(ProductViewModel model)
        //    {
        //        var Product = Find(model.Id.Value);
        //        if (Product != null)
        //        {
        //            Product.ProductName = model.ProductName;
        //            Product.Code = model.Code;
        //            Product.Note = model.Note;
        //            Product.User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId());
        //            Product.LastModifiedDate = DateTime.Now;
        //            Update(Product);
        //            return true;

        //        }
        //        else
        //        {
        //            throw new Exception("Không tìm thấy sản phẩm");

        //        }
        //    }
        //    public bool DeleteProduct(Guid Id)
        //    {
        //        var Product = Find(Id);
        //        if (Product != null)
        //        {
        //            Product.Delete = true;
        //            Update(Product);
        //            return true;
        //        }
        //        else
        //        {
        //            throw new Exception("Không tìm tháy sản phẩm cần xóa");
        //        }
        //    }
        //}
    }
}
