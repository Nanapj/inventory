using Inventory.Core;
using Inventory.Data;
using Inventory.Data.Repository;
using Inventory.Domain.ViewModels;
using Inventory.Models;
using Inventory.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory.Controllers
{
    public class CongNoItem
    {
        public string WarehouseName { get; set; }
        public double NoDauKy { get; set; }
        public double NoCuoiKy { get; set; }
        
    }
    public class JsonController : Controller
    {
        private readonly NoteService _note;
        private readonly NoteDetailService _notedetail;
        private readonly WarehouseService _warehouse;
        private readonly DebtNoteService _debtnote;
        public readonly InventoryContext db;
        private readonly ProductService _product;
        protected UserManager<ApplicationUser> userManager;
        private readonly IRepository<ApplicationUser> _userRepository;
        public JsonController(
            NoteService note,
            NoteDetailService noteDetail,
            DebtNoteService debtNote,
            ProductService product,
            WarehouseService warehouse,
            IRepository<ApplicationUser> userRepository)
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            _userRepository = userRepository;
            _note = note;
            _notedetail = noteDetail;
            _warehouse = warehouse;
            _debtnote = debtNote;
            _product = product;
            db = new InventoryContext();
        }
        //To get all current products in warehouse
        public ActionResult ProductInStock(SearchingStockViewModel model)
        {
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            var DeNote = _note.Queryable().Include(x => x.Details.Select(t => t.Product)).Where(x => ((!model.WarehouseId.HasValue) ||  x.Source.Id == model.WarehouseId)
            && x.Delete.Equals(false)
            && ((!model.Start.HasValue) || x.CreatedDate >= model.Start)
            && ((!model.End.HasValue) || x.CreatedDate <= model.End)
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
            });

            var ReNote = _note.Queryable().Include(x => x.Details.Select(t => t.Product)).Where(x => ((!model.WarehouseId.HasValue) || x.Destination.Id == model.WarehouseId)
            && x.Delete.Equals(false)
            && ((!model.Start.HasValue) || x.CreatedDate >= model.Start)
            && ((!model.End.HasValue) || x.CreatedDate <= model.End)
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
            });
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
            }
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
            }
            var filterDeNote = DeDetails.Where(x => x.Warehouse != null).GroupBy(x => x.Warehouse).Select(x => new
            {
                warehouse = x.Key,
                listItem = x.GroupBy(y => y.ProductName).Select(y => new {
                    productname = y.Key,
                    quantity = y.Sum(t => t.Quantity)
                }).ToList()
            });
            var filterReNote = ReDetails.Where(x => x.Warehouse != null).GroupBy(x => x.Warehouse).Select(x => new
            {
                warehouse = x.Key,
                listItem = x.GroupBy(y => y.ProductName).Select(y => new {
                    productname = y.Key,
                    quantity = y.Sum(t => t.Quantity)
                }).ToList()
            });
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
            return Json(currentStorage, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDebtInStock(SearchingStockViewModel model)
        {
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            var ownedNote = _debtnote.Queryable().Include(t => t.PayWarehouse).Include(t => t.PaymentType).Where(x => x.IsOwed == true 
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
            var ownNote = _debtnote.Queryable().Include(t => t.PayWarehouse).Include(t => t.PaymentType).Where(x => x.IsOwed == false
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
                StockList = group.GroupBy(y => new { y.PayMethod }, (Key2,group2) => new DebtStockViewModel()
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
            var groupOwnNote = ownNote.GroupBy(x => new { x.PayWarehouse }, (Key, group) => new DebtStockGroupViewModel()
            {
                Warehouse = Key.PayWarehouse,
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
            var filterOwnedNote = new List<DebtStockViewModel>();
            foreach(var item in groupOwedNote)
            {
                if(groupOwnNote.Where(x => x.Warehouse == item.Warehouse).Any())
                {

                }
            }
            return Json(groupOwnNote, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CheckProductInStock(NoteViewModel model)
        {
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            var DeNote = _note.Queryable().Include(x => x.Details.Select(t => t.Product)).Where(x => (x.Source.Id != null) && (x.Source.Id == model.SourceId)
             && x.Delete.Equals(false)).Select(x => new NoteViewModel()
           {
               Id = x.Id,
               SourceWarehouse = x.Source.Name,
               Details = x.Details.Select(y => new NoteDetailViewModel()
               {
                   Id = y.Id,
                   ProductName = y.Product.ProductName,
                   Quantity = y.Quantity
               }).ToList()
           });

            var ReNote = _note.Queryable().Include(x => x.Details.Select(t => t.Product)).Where(x => (x.Destination.Id != null) && x.Delete.Equals(false))
            .Select(x => new NoteViewModel()
            {
                Id = x.Id,
                DestinationWarehouse = x.Destination.Name,
                Details = x.Details.Select(y => new NoteDetailViewModel()
                {
                    Id = y.Id,
                    ProductName = y.Product.ProductName,
                    Quantity = y.Quantity
                }).ToList()
            });
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
            }
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
            }
            var filterDeNote = DeDetails.Where(x => x.Warehouse != null).GroupBy(x => x.Warehouse).Select(x => new
            {
                warehouse = x.Key,
                listItem = x.GroupBy(y => y.ProductName).Select(y => new {
                    productname = y.Key,
                    quantity = y.Sum(t => t.Quantity)
                }).ToList()
            });
            var filterReNote = ReDetails.Where(x => x.Warehouse != null).GroupBy(x => x.Warehouse).Select(x => new
            {
                warehouse = x.Key,
                listItem = x.GroupBy(y => y.ProductName).Select(y => new {
                    productname = y.Key,
                    quantity = y.Sum(t => t.Quantity)
                }).ToList()
            });
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
            var flagWrong = false;
            var WrongName="";
            var requestWarehouse = _warehouse.Find((Guid)model.SourceId).Name;
            var matchWarehouse = currentStorage.Where(x => x.Warehouse == requestWarehouse).FirstOrDefault();
            if(matchWarehouse != null)
            {
                foreach (var item in model.Details)
                {
                    var productName = _product.Find((Guid)item.ProductId).ProductName;
                    if (!matchWarehouse.StockList.Where(x => x.ProductName == productName).Any())
                    {
                        flagWrong = true;
                        WrongName = productName;
                        break;
                    }
                    else
                    {
                        var quantity = matchWarehouse.StockList.Where(x => x.ProductName == productName).FirstOrDefault().Quantity;
                        if (item.Quantity > quantity)
                        {
                            flagWrong = true;
                            WrongName = productName;
                            break;
                        }
                    }
                }
            }
            else
            {
                flagWrong = true;
                WrongName = requestWarehouse;
            }
            
            if(flagWrong == true)
            {
                var objects = new
                {
                    result = "Kho không đủ số lượng sản phẩm " + WrongName
                };
                return Json(objects, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var objects = new
                {
                    result = "Ok"
                };
                return Json(objects, JsonRequestBehavior.AllowGet);
            }
                   
        }
        
        public ActionResult getSLNhap(SearchingStockViewModel model)
        {
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            var DsXuatNhap = new List<XuatNhapItem>();
            
            var tonDauModel = new SearchingStockViewModel();
            if(model.Start != null && model.End != null)
            {
                tonDauModel.End = model.Start.Value.AddDays(-1);
            }
            else if (model.Start == null && model.End == null)
            {
                tonDauModel.End = new DateTime(1,1,1);
                model.End = DateTime.Now.Date;
                model.Start = new DateTime(1, 1, 1);
            }
            DsXuatNhap = _note.GetSlNhapXuat(model).ToList();
            tonDauModel.WarehouseId = model.WarehouseId;
            var DsTonDau = new List<StockGroupViewModel>();
            DsTonDau = _note.GetProductInStock(tonDauModel).ToList();
            if (DsTonDau.Count() == 0 && DsXuatNhap.Count() != 0)
            {
                var item = DsXuatNhap.FirstOrDefault();
                var list1 = new StockGroupViewModel();
                var list = new StockDetailViewModel();
                list1.Warehouse = _warehouse.Queryable().Where(x => x.Id == model.WarehouseId).Select(x => x.Name).FirstOrDefault();
                list.ProductName = item.ProductName;
                list.Quantity = 0;
                list.OpeningStock = 0;
                list.ClosingStock = item.ImQuantity - item.ExQuantity;
                list.ExportQuantity = item.ExQuantity;
                list.ImportQuantity = item.ImQuantity;
                list1.StockList.Add(list);
                DsTonDau.Add(list1);
            }
            if (DsTonDau.Count() != 0 && DsXuatNhap.Count() != 0)
            {
                foreach (var item in DsTonDau.FirstOrDefault().StockList.ToList())
                {
                    var check = DsXuatNhap.FirstOrDefault(x => x.ProductName == item.ProductName);
                    if (check == null)
                    {
                        
                        var list = new XuatNhapItem();
                        list.ProductName = item.ProductName;
                        list.ImQuantity = 0;
                        list.ExQuantity = 0;
                        DsXuatNhap.Add(list);
                    }
                }

                foreach (var item in DsTonDau.FirstOrDefault().StockList.ToList())
                {
                    foreach (var item1 in DsXuatNhap)
                    {
                        if(item1.ProductName == item.ProductName)
                        {
                            item.OpeningStock = item.Quantity;
                            item.ExportQuantity = item1.ExQuantity;
                            item.ImportQuantity = item1.ImQuantity;
                            item.ClosingStock = item.OpeningStock + item1.ImQuantity - item1.ExQuantity;
                        }
                        var check = DsTonDau.FirstOrDefault().StockList.FirstOrDefault(x => x.ProductName == item1.ProductName);
                        if (check == null)
                        {
                            var list = new StockDetailViewModel();
                            list.ProductName = item1.ProductName;
                            list.OpeningStock = 0;
                            list.ClosingStock = item1.ImQuantity - item1.ExQuantity;
                            list.ExportQuantity = item1.ExQuantity;
                            list.ImportQuantity = item1.ImQuantity;
                            list.Quantity = 0;
                            DsTonDau.FirstOrDefault().StockList.Add(list);
                        }
                    }
                }
                
            }
            else if (DsTonDau.Count() != 0 && DsXuatNhap.Count() == 0)
            {
                foreach (var item in DsTonDau.FirstOrDefault().StockList)
                {
                    item.Quantity = item.Quantity;
                    item.ClosingStock = item.Quantity;
                    item.ImportQuantity = 0;
                    item.ExportQuantity = 0;
                    item.OpeningStock = item.Quantity;
                }
            }


            return Json(DsTonDau, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getCongNo(SearchingStockViewModel model)
        {
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            var NoDauKy = new List<DebtStockGroupViewModel>();
            var NoCuoiKy = new List<DebtStockGroupViewModel>();
            var tonDauModel = new SearchingStockViewModel();
            tonDauModel.End = model.Start;
            model.Start = null; 
            if(model.End == null)
            {
                model.End = DateTime.Now.Date;
            }
           
            NoDauKy = _debtnote.GetDebtInStock(tonDauModel).ToList();
            NoCuoiKy = _debtnote.GetDebtInStock(model).ToList();
            var test = new List<CongNoItem>();
            var test1 = new CongNoItem();

            if (NoDauKy.Count() != 0 && NoCuoiKy.Count() != 0)
            {
                
                foreach(var item in NoDauKy)
                {
                    test1 = new CongNoItem();
                    test1.NoDauKy = item.Sum;
                    test1.WarehouseName = item.Warehouse;
                    test.Add(test1);
                }
                foreach (var item in NoCuoiKy)
                {
                    foreach (var item1 in test)
                    {
                        if(item.Warehouse == item1.WarehouseName)
                        { 
                            item1.NoCuoiKy = item.Sum;
                        }
                    }
                    var check = NoDauKy.FirstOrDefault(x => x.Warehouse == item.Warehouse);
                    if(check == null)
                    {
                        test1 = new CongNoItem();
                        test1.NoDauKy = 0;
                        test1.WarehouseName = item.Warehouse;
                        test1.NoCuoiKy = item.Sum;
                        test.Add(test1);

                    }
                   
                }

            }
            else if(NoDauKy.Count() == 0 && NoCuoiKy.Count() != 0)
            {
                foreach(var item in NoCuoiKy)
                {
                    test1 = new CongNoItem();
                    test1.NoCuoiKy = item.Sum;
                    test1.NoDauKy = 0;
                    test1.WarehouseName = item.Warehouse;
                    test.Add(test1);
                }

            }
            else if (NoCuoiKy.Count() == 0 && NoDauKy.Count() != 0)
            {
                foreach (var item in NoDauKy)
                {
                    test1 = new CongNoItem();
                    test1.NoCuoiKy = 0;
                    test1.NoDauKy = item.Sum;
                    test1.WarehouseName = item.Warehouse;
                    test.Add(test1);
                }

            }
            return Json(test, JsonRequestBehavior.AllowGet);



        }
        public ActionResult getTongCongNo(SearchingStockViewModel model)
        {
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            model.Start = null;
            if (model.End == null)
            {
                model.End = DateTime.Now.Date;
            }
            var TongNo = new List<DebtStockGroupViewModel>();
            TongNo = _debtnote.GetDebtInStock(model).ToList();
            var result = TongNo.Select(x => x.Sum).Sum();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getRole()
        {
            var user = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId()) ?? _userRepository.GetAll().FirstOrDefault();
            Debug.WriteLine("This is the User Role: " + userManager.GetRoles(user.Id)[0].Trim());
            var Role = userManager.GetRoles(user.Id)[0].Trim();
            return Json(new { role = Role }, JsonRequestBehavior.AllowGet);
        }
    }
   
}