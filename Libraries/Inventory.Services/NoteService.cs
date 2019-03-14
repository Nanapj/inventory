using Inventory.Core;
using Inventory.Data;
using Inventory.Data.Repository;
using Inventory.Domain.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Inventory.Services
{
    public class XuatNhapItem
    {
        public string ProductName { get; set; }
        public long ImQuantity { get; set; }
        public long ExQuantity { get; set; }
    }

    public class NoteService : Service<Note>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        protected readonly InventoryContext db;
        public readonly IRepository<Warehouse> _warehouse;
        public readonly IRepository<NoteDetail> _notedetail;
        protected UserManager<ApplicationUser> userManager;
        public readonly IRepository<Product> _product;
        public readonly IRepository<DebtNote> _debtnote;
        public readonly IRepository<PaymentType> _paymenttype;
        public NoteService(IRepository<Note> repository,
        IRepository<Warehouse> warehouse,
        IRepository<NoteDetail> notedetail,
        IRepository<Product> product,
        IRepository<DebtNote> debtnote,
        IRepository<PaymentType> paymenttype,
        IRepository<ApplicationUser> userRepository)
        : base(repository)
        {
            db = new InventoryContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            _userRepository = userRepository;
            _warehouse = warehouse;
            _notedetail = notedetail;
            _product = product;
            _debtnote = debtnote;
            _paymenttype = paymenttype;
        }
        public IQueryable<NoteViewModel> QueryableViewModel(SearchingViewModel model)
        {
            if (model.to != null)
            {
                model.to = model.to.Value.AddDays(1);
            }
            var result =  Queryable().Where(x => ((x.Delete == false)
           && (!model.DestinationId.HasValue || model.DestinationId == x.Destination.Id)
           && ((!model.SourceId.HasValue) || (model.SourceId == x.Source.Id))
           && ((!model.from.HasValue) || (DbFunctions.TruncateTime(x.CreatedDate) >= DbFunctions.TruncateTime(model.from)))
           && ((!model.to.HasValue) || (DbFunctions.TruncateTime(x.CreatedDate) <= DbFunctions.TruncateTime(model.to)))
           && ((!model.Id.HasValue) || x.Id == model.Id))
           && ((!(model.Code == "")) || (model.Code == x.Code))
           )
           .Select(x => new NoteViewModel
            {
                Id = x.Id,
                Code = x.Code,
                DestinationId = x.Destination.Id,
                SourceId = x.Source.Id,
                DestinationWarehouse = x.Destination.Name,
                SourceWarehouse = x.Source.Name,
                Noted = x.Noted,
                Total = x.Total,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate,
                ProductType = x.ProductType,
                UserName =x.User.UserName,
                Details = x.Details.Where(t => t.Delete == false).Select(t => new NoteDetailViewModel()
                {
                    Id = t.Id,
                    ProductId = t.Product.Id,
                    Price = t.Price,
                    Unit = t.Unit,
                    ProductName = t.Product.ProductName,
                    Quantity = t.Quantity,
                    Noted  = t.Noted
                }).ToList()        
            });
            return result;
        }
        public NoteViewModel SpecifiedNote(Guid key)
        {
            var objects = Queryable().Include(x => x.Details.Select(t => t.Product))
                .Include(x => x.Destination)
                .Include(x => x.Source).FirstOrDefault(x => x.Id == key);
            var result = new NoteViewModel()
            {
                Id = objects.Id,
                Code = objects.Code,
                DestinationId = objects.Destination?.Id,
                SourceId = objects.Source?.Id,
                DestinationWarehouse = objects.Destination?.Name,
                SourceWarehouse = objects.Source?.Name,
                Noted = objects.Noted,
                Total = objects.Total,
                CreatedDate = objects.CreatedDate,
                ProductType = objects.ProductType,
                LastModifiedDate = objects.LastModifiedDate,
                Details = objects.Details.Select(y => new NoteDetailViewModel() {
                    Id = y.Id,
                    ProductId = y.Product?.Id,
                    Price = y.Price,
                    Unit = y.Unit,
                    ProductName = y.Product?.ProductName,
                    Quantity = y.Quantity,
                    Noted = y.Noted
                }).ToList()
            };
            return result;
        }
        public Note AddNote(NoteViewModel model)
        {
            if (Queryable().Any(x => x.Code.ToLower().Contains(model.Code.ToLower()) && x.Delete == false))
            {
                throw new Exception("Đã tồn tại mã phiếu");
            }
            else
            {
                var result = new Note();
                var User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId()) ?? _userRepository.GetAll().FirstOrDefault();
                result.User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId());
                result.CreatedDate = DateTime.Now;
                result.LastModifiedDate = DateTime.Now;
                result.Noted = model.Noted;
                result.ProductType = model.ProductType;
                result.Details = new List<NoteDetail>();
                foreach (var item in model.Details)
                {
                    result.Details.Add(new NoteDetail()
                    {
                        Product = _product.Queryable().Where(p => p.Id == item.ProductId).FirstOrDefault(),
                        Price = item.Price,
                        Quantity = item.Quantity,         
                        Unit = _product.Queryable().Where(p => p.Id == item.ProductId).FirstOrDefault().Unit,
                        User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId()),
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Noted = item.Noted
                    });
                }
                result.Total = model.Details.Sum(x => x.Quantity * x.Price);
                if(model.SourceId == null)
                {
                    result.Code = "NK" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + (Queryable().Where(mi =>  mi.Source.Id == null && mi.CreatedDate.Month == DateTime.Now.Month
                            && mi.CreatedDate.Year == DateTime.Now.Year).Count() + 1).ToString("D3"); 
                    result.Source = null;
                    result.Destination = _warehouse.Queryable().Where(d => d.Id == model.DestinationId).FirstOrDefault();
                    result.Index = Queryable().Where(de => de.Source.Id == null).Count() + 1;
                    result.MonthIndex = Queryable().Where(mi => mi.Source.Id == null
                    && mi.CreatedDate.Month == DateTime.Now.Month
                    && mi.CreatedDate.Year == DateTime.Now.Year).Count() + 1;

                }
                else if(model.DestinationId == null )
                {
                    result.Code = "XK" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") +  (Queryable().Where(mi => mi.Destination.Id == null && mi.CreatedDate.Month == DateTime.Now.Month
                           && mi.CreatedDate.Year == DateTime.Now.Year).Count() + 1).ToString("D3");
                    result.Source = _warehouse.Queryable().Where(s => s.Id == model.SourceId).FirstOrDefault();
                    result.Destination = null;
                    result.Index = Queryable().Where(de => de.Destination.Id == null).Count() + 1;
                    result.MonthIndex = Queryable().Where(mi => mi.Destination.Id == null 
                    && mi.CreatedDate.Month == DateTime.Now.Month
                    && mi.CreatedDate.Year == DateTime.Now.Year).Count() + 1;
                }
                else
                {
                    result.Code = "CK" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + (Queryable().Where(mi => mi.Destination.Id != null && mi.Source.Id != null && mi.CreatedDate.Month == DateTime.Now.Month
                          && mi.CreatedDate.Year == DateTime.Now.Year).Count() + 1).ToString("D3");
                    result.Source = _warehouse.Queryable().Where(s => s.Id == model.SourceId).FirstOrDefault();
                    result.Destination = _warehouse.Queryable().Where(d => d.Id == model.DestinationId).FirstOrDefault();
                    result.Index = Queryable().Where(de => de.Destination.Id != null && de.Source.Id != null).Count() + 1;
                    result.MonthIndex = Queryable().Where(mi => mi.Destination.Id != null && mi.Source.Id != null && mi.CreatedDate.Month == DateTime.Now.Month
                    && mi.CreatedDate.Year == DateTime.Now.Year).Count() + 1;
                    if(model.ProductType == Inventory.Core.Enum.ProductType.New)
                    {
                        var debtNote = new DebtNote();
                        debtNote.Code = "OWED" + DateTime.Now.ToString("yy") +  DateTime.Now.ToString("MM") + (_debtnote.Queryable().Where(x => x.IsOwed == true
                           && x.CreatedDate.Month == DateTime.Now.Month
                           && x.CreatedDate.Year == DateTime.Now.Year).Count() + 1).ToString("D3");
                        debtNote.Index = _debtnote.Queryable().Where(x => x.IsOwed == true).Count() + 1;
                        debtNote.MonthIndex = _debtnote.Queryable().Where(x => x.IsOwed == true
                        && x.CreatedDate.Month == DateTime.Now.Month
                        && x.CreatedDate.Year == DateTime.Now.Year).Count() + 1;
                        debtNote.User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId()) ?? _userRepository.GetAll().FirstOrDefault();
                        debtNote.CreatedDate = DateTime.Now;
                        debtNote.LastModifiedDate = DateTime.Now;
                        debtNote.Total = model.Details.Sum(x => x.Quantity * x.Price);
                        debtNote.IsOwed = true;
                        debtNote.PaymentType = _paymenttype.Queryable().Where(t => t.Name.Equals("Tiền mặt")).FirstOrDefault();
                        debtNote.PayWarehouse = _warehouse.Find(model.DestinationId);
                        _debtnote.Insert(debtNote);
                        _debtnote.SaveChanges();
                    }
                }              
                Insert(result);
                return result;
            }
        }
        public bool UpdateNote(NoteViewModel model)
        {
            var Note = Find(model.Id.Value);
           
            if (Note != null)
            {
                
                foreach(var item in model.Details)
                {
                    if(item.Id != null)
                    {
                        var details = _notedetail.Find(item.Id.Value);
                        if (details != null)
                        {
                            details.Price = item.Price;
                            details.Quantity = item.Quantity;
                            details.Product = _product.Find(item.ProductId);
                            details.Unit = _product.Queryable().Where(p => p.Id == item.ProductId).FirstOrDefault().Unit;
                            details.LastModifiedDate = DateTime.Now;
                            details.Noted = item.Noted;
                        }                  

                    }                  
                }
                Note.Noted = model.Noted;
                Note.LastModifiedDate = DateTime.Now;
                Note.User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId());
                Note.Total = model.Details.Sum(s => s.Quantity * s.Price);
                Update(Note);
                return true;
            }
            else
            {
                throw new Exception("Phiếu không tồn tại");

            }
        }
        public bool DeleteNote(Guid Id)
        {
            var Note = Find(Id);
            var ListDetail = _notedetail.Queryable().Where(x => x.Note.Id == Note.Id).ToList();
            if (Note != null)
            {
                
                Note.Delete = true;
                foreach(var item in ListDetail)
                {
                    item.Delete = true;
                    _notedetail.Update(item);
                }
                Update(Note);
                return true;
            }
            else
            {
                throw new Exception("Không tìm tháy phiếu cần xóa");
            }
        }
        public IEnumerable<XuatNhapItem> GetSlNhapXuat(SearchingStockViewModel model)
        {
            if (model.End != null)
            {
                model.End = model.End.Value.AddDays(1);
            }
            var DsXuatNhap = new List<XuatNhapItem>();
            var DsXuatNhapKho = new XuatNhapItem();
            var DsNhap = Queryable().Include(x => x.Details.Select(y => y.Product)).Where(x => (x.Destination != null)
                    && (x.Destination.Id == model.WarehouseId)
                    && ((!model.Start.HasValue) || (x.CreatedDate >= model.Start))
                    && ((!model.End.HasValue) || (x.CreatedDate <= model.End))
                && (x.Delete != true)).SelectMany(x => x.Details).GroupBy(x => x.Product.ProductName)
                .Select(x => new XuatNhapItem()
                {
                    ProductName = x.Key,
                    ImQuantity = x.Sum(i => i.Quantity),
                    ExQuantity = 0,
                   
                }).ToList();

            var DsXuat = Queryable().Include(x => x.Details.Select(y => y.Product)).Where(x => (x.Source != null)
                    && (x.Source.Id == model.WarehouseId)
                    && ((!model.Start.HasValue) || (x.CreatedDate >= model.Start))
                    && ((!model.End.HasValue) || (x.CreatedDate <= model.End))
                && (x.Delete != true)).SelectMany(x => x.Details).GroupBy(x => x.Product.ProductName)
                .Select(x => new XuatNhapItem()
                {
                    ProductName = x.Key,
                    ExQuantity = x.Sum(i => i.Quantity),
                    ImQuantity = 0,
                    
                }).ToList();

            if(DsNhap.Count() != 0 && DsXuat.Count() != 0)
            {
                foreach (var item in DsNhap)
                {
                    DsXuatNhapKho = new XuatNhapItem();
                    DsXuatNhapKho.ImQuantity = item.ImQuantity;
                    DsXuatNhapKho.ProductName = item.ProductName;
                    DsXuatNhap.Add(DsXuatNhapKho);
                }
                foreach( var item in DsXuat)
                {
                    foreach (var item1 in DsXuatNhap)
                    {
                        if(item.ProductName == item1.ProductName)
                        {
                            item1.ExQuantity = item.ExQuantity;
                        }
                    }
                    var check = DsNhap.FirstOrDefault(x => x.ProductName == item.ProductName);
                    if(check == null)
                    {
                        DsXuatNhapKho = new XuatNhapItem();
                        DsXuatNhapKho.ImQuantity = 0;
                        DsXuatNhapKho.ProductName = item.ProductName;
                        DsXuatNhapKho.ExQuantity = item.ExQuantity;
                        DsXuatNhap.Add(DsXuatNhapKho);
                    }
                }

            }
            else if (DsNhap.Count() == 0 && DsXuat.Count() != 0)
            {
                foreach(var item in DsXuat)
                {
                    DsXuatNhapKho = new XuatNhapItem();
                    DsXuatNhapKho.ImQuantity = 0;
                    DsXuatNhapKho.ProductName = item.ProductName;
                    DsXuatNhapKho.ExQuantity = item.ExQuantity;
                    DsXuatNhap.Add(DsXuatNhapKho);
                }
            }
            else if (DsNhap.Count() != 0 && DsXuat.Count() == 0)
            {
                foreach (var item in DsNhap)
                {
                    DsXuatNhapKho = new XuatNhapItem();
                    DsXuatNhapKho.ImQuantity = item.ImQuantity;
                    DsXuatNhapKho.ProductName = item.ProductName;
                    DsXuatNhapKho.ExQuantity = 0;
                    DsXuatNhap.Add(DsXuatNhapKho);
                }
            }
            return DsXuatNhap;
        }
        public IEnumerable<StockGroupViewModel> GetProductInStock(SearchingStockViewModel model)
        {
            if (model.End != null)
            {
                model.End = model.End.Value.AddDays(1);
            }
            var DeNote = Queryable().Include(x => x.Details.Select(t => t.Product)).Where(x => ((!model.WarehouseId.HasValue) || x.Source.Id == model.WarehouseId)
            && ((!model.Start.HasValue) || x.CreatedDate >= model.Start)
            && ((!model.End.HasValue) || x.CreatedDate <= model.End)
            && x.Delete.Equals(false)
            ).Select(x => new NoteViewModel()
            {
                Id = x.Id,
                SourceWarehouse = x.Source.Name,
                Details = x.Details.Select(y => new NoteDetailViewModel()
                {
                    Id = y.Id,
                    ProductName = y.Product.ProductName,
                    Quantity = y.Quantity
                }).ToList()
            }); // Danh sách phiếu xuất 

            var ReNote =Queryable().Include(x => x.Details.Select(t => t.Product)).Where(x => ((!model.WarehouseId.HasValue) || x.Destination.Id == model.WarehouseId)
            && ((!model.Start.HasValue) || x.CreatedDate >= model.Start)
            && ((!model.End.HasValue) || x.CreatedDate <= model.End)
            && x.Delete.Equals(false)
            ).Select(x => new NoteViewModel()
            {
                Id = x.Id,
                DestinationWarehouse = x.Destination.Name,
                Details = x.Details.Select(y => new NoteDetailViewModel()
                {
                    Id = y.Id,
                    ProductName = y.Product.ProductName,
                    Quantity = y.Quantity
                }).ToList()
            }); // Danh sách phiếu nhập
            var ReDetails = new List<StockViewModel>();
            var DeDetails = new List<StockViewModel>();
            foreach (var item in DeNote)
            {
                foreach (var smallitem in item.Details)
                {
                    DeDetails.Add(new StockViewModel()
                    {
                        ProductName = smallitem.ProductName,
                        Warehouse = item.SourceWarehouse,
                        Quantity = smallitem.Quantity
                    });
                }
            } // List chi tiết phiếu xuất
            foreach (var item in ReNote)
            {
                foreach (var smallitem in item.Details)
                {
                    ReDetails.Add(new StockViewModel()
                    {
                        ProductName = smallitem.ProductName,
                        Warehouse = item.DestinationWarehouse,
                        Quantity = smallitem.Quantity
                    });
                }
            } // List chi tiết phiếu nhập
            var filterDeNote = DeDetails.Where(x => x.Warehouse != null).GroupBy(x => x.Warehouse).Select(x => new
            {
                warehouse = x.Key,
                listItem = x.GroupBy(y => y.ProductName).Select(y => new {
                    productname = y.Key,
                    quantity = y.Sum(t => t.Quantity)
                }).ToList()
            }); // list số lượng theo sản phảm xuất
            var filterReNote = ReDetails.Where(x => x.Warehouse != null).GroupBy(x => x.Warehouse).Select(x => new
            {
                warehouse = x.Key,
                listItem = x.GroupBy(y => y.ProductName).Select(y => new {
                    productname = y.Key,
                    quantity = y.Sum(t => t.Quantity)
                }).ToList()
            });// list số lượng theo sản phảm nhập
            var currentStorage = new List<StockGroupViewModel>();
            foreach (var item in filterReNote)
            {
                var currentStorageItem = new StockGroupViewModel();

                currentStorageItem.Warehouse = item.warehouse;

                if (filterDeNote.Where(x => x.warehouse == item.warehouse).Any())
                {
                    var gooditem = filterDeNote.Where(x => x.warehouse == item.warehouse).FirstOrDefault();
                    foreach (var smallitem in item.listItem)
                    {
                        var detailItem = new StockDetailViewModel();
                        if (gooditem.listItem.Where(t => t.productname == smallitem.productname).Any())
                        {
                            var caughtDetail = gooditem.listItem.Where(t => t.productname == smallitem.productname).FirstOrDefault();
                            detailItem.ProductName = smallitem.productname;
                            detailItem.Quantity = smallitem.quantity - caughtDetail.quantity;
                            currentStorageItem.StockList.Add(detailItem);
                        }
                        else
                        {
                            detailItem.ProductName = smallitem.productname;
                            detailItem.Quantity = smallitem.quantity;
                            currentStorageItem.StockList.Add(detailItem);
                        }
                    }
                    currentStorage.Add(currentStorageItem);

                }
                else
                {
                    foreach (var smallitem in item.listItem)
                    {
                        var detailItem = new StockDetailViewModel();
                        detailItem.ProductName = smallitem.productname;
                        detailItem.Quantity = smallitem.quantity;
                        currentStorageItem.StockList.Add(detailItem);
                    }
                    currentStorage.Add(currentStorageItem);
                }
            }
            return currentStorage;
        }
        

    }
}
