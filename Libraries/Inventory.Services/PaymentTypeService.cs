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
    public class PaymentTypeService : Service<PaymentType>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        protected readonly InventoryContext db;
        protected UserManager<ApplicationUser> userManager;
        public PaymentTypeService(IRepository<PaymentType> repository,
        IRepository<ApplicationUser> userRepository)
        : base(repository)
        {
            db = new InventoryContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            _userRepository = userRepository;
        }
        public IQueryable<PaymentTypeViewModel> QueryableViewModel()
        {
            return Queryable().Where(x => x.Delete == false).Select(x => new PaymentTypeViewModel
            {
               Id = x.Id,
               Name = x.Name
            });
        }
        public PaymentType AddPaymentType(PaymentTypeViewModel model)
        {
            if (Queryable().Any(x => x.Name.ToLower().Contains(model.Name.ToLower()) && x.Delete == false))
            {
                throw new Exception("Đã tồn tại hình thức thanh toán này");
            }
            else
            {
                var result = new PaymentType();
                result.Name = model.Name;
                result.CreatedDate = DateTime.Now;
                result.LastModifiedDate = DateTime.Now;
                result.User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId()) ?? _userRepository.GetAll().FirstOrDefault();
                Insert(result);
                return result;
            }
        }
        public bool UpdatePaymentType(PaymentTypeViewModel model)
        {
            var PaymentType = Find(model.Id.Value);
            if (PaymentType != null)
            {
                PaymentType.Name = model.Name;
                PaymentType.LastModifiedDate = DateTime.Now;
                PaymentType.User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId());
                Update(PaymentType);
                return true;

            }
            else
            {
                throw new Exception("Không tìm thấy hình thức thanh toán này");

            }
        }
        public bool DeletePaymentType(Guid Id)
        {
            var paymentType = Find(Id);
            if (paymentType != null)
            {
                paymentType.Delete = true;
                Update(paymentType);
                return true;
            }
            else
            {
                throw new Exception("Không tìm tháy sản phẩm cần xóa");
            }
        }
    }
}
