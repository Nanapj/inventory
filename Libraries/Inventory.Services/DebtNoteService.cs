using Inventory.Core;
using Inventory.Data;
using Inventory.Data.Repository;
using Inventory.Domain.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Inventory.Services
{
    public class DebtNoteitem
    {
        public string ProductName { get; set; }
        public long Quantity { get; set; }
        public long ImQuantity { get; set; }
        public long ExQuantity { get; set; }
        public bool IsExport { get; set; }
    }
    public class DebtNoteService : Service<DebtNote>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        protected readonly InventoryContext db;
        public readonly IRepository<Warehouse> _warehouse;
        protected UserManager<ApplicationUser> userManager;
        public readonly IRepository<Product> _product;
        public readonly IRepository<PaymentType> _paymentType;
        public DebtNoteService(IRepository<DebtNote> repository,
        IRepository<Warehouse> warehouse,
        IRepository<NoteDetail> notedetail,
        IRepository<Product> product,
        IRepository<PaymentType> paymentType,
        IRepository<ApplicationUser> userRepository)
        : base(repository)
        {
            db = new InventoryContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            _userRepository = userRepository;
            _warehouse = warehouse;
            _product = product;
            _paymentType = paymentType;
        }
        public IQueryable<DebtNoteViewModel> QueryableViewModel(DebtSearchingViewModel model)
        {
            if (model.to != null)
            {
                model.to = model.to.Value.AddDays(1);
            }
            return Queryable().Where(x => ((x.Delete == false)
           && ((!model.WarehouseId.HasValue) || (model.WarehouseId == x.PayWarehouse.Id))
           && ((!model.from.HasValue) || (DbFunctions.TruncateTime(x.CreatedDate) >= DbFunctions.TruncateTime(model.from)))
           && ((!model.to.HasValue) || (DbFunctions.TruncateTime(x.CreatedDate) <= DbFunctions.TruncateTime(model.to)))))
           .Select(x => new DebtNoteViewModel
           {
               Id = x.Id,
               Code = x.Code,
               CreatedDate = x.CreatedDate,
               CreatedUser = x.User.UserName,
               PayMethod = x.PaymentType.Name,
               PayWarehouse = x.PayWarehouse.Name,
               LastModifiedDate = x.LastModifiedDate,
               PaymentTypeId = x.PaymentType.Id,
               WarehouseId = x.PayWarehouse.Id,
               isOwed = x.IsOwed,
               Noted = x.Noted,
               Total = x.Total
           });
        }
        public DebtNote AddDebtNote(DebtNoteViewModel model)
        {
            if (Queryable().Any(x => x.Code.ToLower().Contains(model.Code.ToLower()) && x.Delete == false))
            {
                throw new Exception("Đã tồn tại mã phiếu");
            }
            else
            {
                var result = new DebtNote();
                var User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId()) ?? _userRepository.GetAll().FirstOrDefault();
                if (model.isOwed == false) {
                    result.Code = "OWN" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM")+ (Queryable().Where(x => x.IsOwed == false 
                    && x.CreatedDate.Month == DateTime.Now.Month
                    && x.CreatedDate.Year == DateTime.Now.Year).Count() + 1).ToString("D4");
                    result.Index = Queryable().Where(x => x.IsOwed == false).Count() + 1;
                    result.MonthIndex = Queryable().Where(x => x.IsOwed == false
                    && x.CreatedDate.Month == DateTime.Now.Month
                    && x.CreatedDate.Year == DateTime.Now.Year).Count() + 1;
                }
                else
                {
                    result.Code = "OWED" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM")+(Queryable().Where(x => x.IsOwed == true 
                    && x.CreatedDate.Month == DateTime.Now.Month
                    && x.CreatedDate.Year == DateTime.Now.Year).Count() + 1).ToString("D4");
                    result.Index = Queryable().Where(x => x.IsOwed == true).Count() + 1;
                    result.MonthIndex = Queryable().Where(x => x.IsOwed == true
                    && x.CreatedDate.Month == DateTime.Now.Month
                    && x.CreatedDate.Year == DateTime.Now.Year).Count() + 1;         
                }
                result.User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId()) ?? _userRepository.GetAll().FirstOrDefault();
                result.CreatedDate = DateTime.Now;
                result.LastModifiedDate = DateTime.Now;
                result.Total = model.Total;
                result.IsOwed = model.isOwed;
                result.PaymentType =_paymentType.Find( model.PaymentTypeId);
                result.PayWarehouse = _warehouse.Find(model.WarehouseId);
                result.Noted = model.Noted;
                Insert(result);
                return result;
            }
        }
        public bool UpdateDebtNote(DebtNoteViewModel model)
        {
            var DebtNote = Find(model.Id.Value);

            if (DebtNote != null)
            {

                DebtNote.Total = model.Total;
                DebtNote.PaymentType = _paymentType.Find(model.PaymentTypeId);
                DebtNote.PayWarehouse = _warehouse.Find(model.WarehouseId);
                DebtNote.IsOwed = model.isOwed;
                DebtNote.LastModifiedDate = DateTime.Now;
                DebtNote.Noted = model.Noted;
                Update(DebtNote);
                return true;
            }
            else
            {
                throw new Exception("Phiếu không tồn tại");

            }
        }
        public bool DeleteDebtNote(Guid Id)
        {
            var DebtNote = Find(Id);
            if (DebtNote != null)
            {
                DebtNote.Delete = true;
                Update(DebtNote);
                return true;
            }
            else
            {
                throw new Exception("Không tìm tháy phiếu cần xóa");
            }
        }
        public IEnumerable<DebtStockGroupViewModel> GetDebtInStock(SearchingStockViewModel model)
        {
            if (model.End != null)
            {
                model.End = model.End.Value.AddDays(1);
            }
            var ownedNote =Queryable().Include(t => t.PayWarehouse).Include(t => t.PaymentType).Where(x => x.IsOwed == true
             && x.Delete == false
             && ((!model.WarehouseId.HasValue) || x.PayWarehouse.Id == model.WarehouseId)
             && ((!model.Start.HasValue) || x.CreatedDate >= model.Start)
             && ((!model.End.HasValue) || x.CreatedDate <= model.End)).Select(x => new DebtNoteViewModel
             {
                 Id = x.Id,
                 PayWarehouse = x.PayWarehouse.Name,
                 Total = x.Total,
                 CreatedDate = x.CreatedDate,
                 LastModifiedDate = x.LastModifiedDate,
                 PayMethod = x.PaymentType.Name,
                 Noted = x.Noted,
                 Code = x.Code
             });
            var ownNote =Queryable().Include(t => t.PayWarehouse).Include(t => t.PaymentType).Where(x => x.IsOwed == false
            && x.Delete == false
            && ((!model.WarehouseId.HasValue) || x.PayWarehouse.Id == model.WarehouseId)
            && ((!model.Start.HasValue) || x.CreatedDate >= model.Start)
            && ((!model.End.HasValue) || x.CreatedDate <= model.End)
            ).Select(x => new DebtNoteViewModel
            {
                Id = x.Id,
                PayWarehouse = x.PayWarehouse.Name,
                Total = x.Total,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate,
                PayMethod = x.PaymentType.Name,
                Noted = x.Noted,
                Code = x.Code
            });
            var groupOwedNote = ownedNote.GroupBy(x => new { x.PayWarehouse }, (Key, group) => new DebtStockGroupViewModel()
            {
                Warehouse = Key.PayWarehouse,
                Sum = group.Sum(y => y.Total),
                StockList = group.GroupBy(y => new { y.PayMethod }, (Key2, group2) => new DebtStockViewModel()
                {
                    PayMethod = Key2.PayMethod,
                    Details = group2.Select(z => new DebtStockDetailViewModel()
                    {
                        Code = z.Code,
                        Total = z.Total,
                        Noted = z.Noted
                    }).ToList()
                }).ToList()
            });
            var tst = groupOwedNote.Count();
            var groupOwnNote = ownNote.GroupBy(x => new { x.PayWarehouse }, (Key, group) => new DebtStockGroupViewModel()
            {
                Warehouse = Key.PayWarehouse,
                Sum = group.Sum(y => y.Total),
                StockList = group.GroupBy(y => new { y.PayMethod }, (Key2, group2) => new DebtStockViewModel()
                {
                    PayMethod = Key2.PayMethod,
                    Details = group2.Select(z => new DebtStockDetailViewModel()
                    {
                        Code = z.Code,
                        Total = z.Total,
                        Noted = z.Noted
                    }).ToList()
                }).ToList()
            });
            var test = groupOwnNote.Count();
            var filterOwnedNote = new List<DebtStockGroupViewModel>();
            foreach (var item in groupOwedNote)
            {
                if(test != 0)
                {
                    if (groupOwnNote.Where(x => x.Warehouse == item.Warehouse).Any())
                    {
                        var matchOwnNote = groupOwnNote.Where(x => x.Warehouse == item.Warehouse).FirstOrDefault();
                        item.Sum = item.Sum - matchOwnNote.Sum;
                        foreach (var smallitem in item.StockList)
                        {
                            if (matchOwnNote.StockList.Where(y => y.PayMethod == smallitem.PayMethod).Any())
                            {
                                var matchPayMethod = (matchOwnNote.StockList.Where(y => y.PayMethod == smallitem.PayMethod).FirstOrDefault());
                                foreach (var details in matchPayMethod.Details)
                                {
                                    smallitem.Details.Add(details);
                                }
                            }
                        }

                    }
                }
                
                filterOwnedNote.Add(item);
            }
            return filterOwnedNote;
        }
    }
}