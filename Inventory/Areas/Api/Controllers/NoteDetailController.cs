using Inventory.Domain.Interface.UnitOfWork;
using Inventory.Domain.ViewModels;
using Inventory.Services;
using Microsoft.AspNet.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Inventory.Areas.Api.Controllers
{
    public class NoteDetailController : ODataController
    {
        private readonly NoteDetailService _noteDetail;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public NoteDetailController(NoteDetailService noteDetail, IUnitOfWorkManager unitOfWorkManager)
        {
            _noteDetail = noteDetail;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [EnableQuery]
        public IEnumerable<NoteDetailViewModel> Get()
        {

            return _noteDetail.QueryableViewModel().ToList();
        }

        public IHttpActionResult Put(Guid key, NoteDetailViewModel delta)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                _noteDetail.UpdateNoteDetail(delta);
                unitOfWork.Commit();
                return StatusCode(HttpStatusCode.OK);
            }
        }

        // POST: odata/Warehouse
        public IHttpActionResult Post(NoteDetailViewModel delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.
            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                var result = _noteDetail.AddNoteDetail(delta);
                unitOfWork.Commit();
                var resultObject = new NoteDetailViewModel()
                {
                    Id = result.Id,
                    Price = result.Price,
                    ProductName = result.Product.ProductName,
                    Quantity = result.Quantity
                };
                return Created(resultObject);
            }

            
        }
        // DELETE: odata/Warehouse(5)
        public IHttpActionResult Delete(Guid key)
        {
            // TODO: Add delete logic here.
            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                _noteDetail.DeleteDetail(key);
                unitOfWork.Commit();
            }
            return StatusCode(HttpStatusCode.OK);
            //return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}