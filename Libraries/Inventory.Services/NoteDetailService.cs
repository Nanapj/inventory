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
    public class NoteDetailService :Service<NoteDetail>
    {
        private readonly IRepository<ApplicationUser> _userRepository;
        protected readonly InventoryContext db;
        public readonly IRepository<Warehouse> _warehouse;
        protected UserManager<ApplicationUser> userManager;
        public readonly IRepository<Product> _product;
        public readonly IRepository<Note> _note;
        public NoteDetailService(IRepository<NoteDetail> repository,
        IRepository<Warehouse> warehouse,
        IRepository<Product> product,
        IRepository<Note> note,
        IRepository<ApplicationUser> userRepository)
        : base(repository)
        {
            db = new InventoryContext();
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            _userRepository = userRepository;
            _warehouse = warehouse;
            _product = product;
            _note = note;
        }
        public IQueryable<NoteDetailViewModel> QueryableViewModel()
        {
            return Queryable().Where(x => x.Delete == false).Select(x => new NoteDetailViewModel
            {
                Id = x.Id,
                Price = x.Price,
                Quantity = x.Quantity,
                CreatedDate = x.CreatedDate,
                ProductName = x.Product.ProductName,
                LastModifiedDate = x.LastModifiedDate,
                NoteId = x.Note.Id
            });
        }
        public NoteDetail AddNoteDetail(NoteDetailViewModel model)
        {
            if (Queryable().Any(x => x.Id == model.Id))
            {
                throw new Exception("Đã tồn tại Id");
            }
            else
            {
                var result = new NoteDetail();           
                result.Price = model.Price;
                result.Quantity = model.Quantity;
                result.User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId());
                result.Note = _note.Find(model.NoteId);
                result.Product = _product.Find(model.ProductId);
                result.CreatedDate = DateTime.Now;
                result.LastModifiedDate = DateTime.Now;
                var note = _note.Find(model.NoteId);
                note.Total = Queryable().Where(x => x.Note.Id == note.Id && x.Delete == false).Sum(x => x.Quantity * x.Price) + (model.Quantity*model.Price);
                _note.Update(note);
                _note.SaveChanges();
                return result;
            }
        }
        public bool UpdateNoteDetail(NoteDetailViewModel model)
        {
            var NoteDetail = Find(model.Id.Value);
            if (NoteDetail != null)
            {
                NoteDetail.Price = model.Price;
                NoteDetail.Quantity = model.Quantity;
                NoteDetail.User = _userRepository.Find(HttpContext.Current.User.Identity.GetUserId());
                NoteDetail.Product = _product.Find(model.ProductId);
                NoteDetail.LastModifiedDate = DateTime.Now;
                Update(NoteDetail);
                return true;
            }
            else
            {
                throw new Exception("Không tìm thấy chi tiết phiếu");

            }
        }
        public bool DeleteDetail(Guid Id)
        {
            var Detail = Find(Id);
            if (Detail != null)
            {
                Detail.Delete = true;
                Update(Detail);
                return true;
            }
            else
            {
                throw new Exception("Không tìm tháy chi tiết phiếu");
            }
        }
    }
}
