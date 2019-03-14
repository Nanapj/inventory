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
    public class DebtNoteController : ODataController
    {
        private readonly DebtNoteService _debtNote;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public DebtNoteController(DebtNoteService debtNote, IUnitOfWorkManager unitOfWorkManager)
        {
            _debtNote = debtNote;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [EnableQuery]
        public IEnumerable<DebtNoteViewModel> Get([FromUri] DebtSearchingViewModel model)
        {

            return _debtNote.QueryableViewModel(model).ToList();
        }

        public IHttpActionResult Put(Guid key, DebtNoteViewModel delta)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                _debtNote.UpdateDebtNote(delta);
                unitOfWork.Commit();
                return StatusCode(HttpStatusCode.OK);
            }
        }

        // POST: odata/Warehouse
        public IHttpActionResult Post(DebtNoteViewModel delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.
            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                var result = _debtNote.AddDebtNote(delta);
                unitOfWork.Commit();
                var resultObject = new DebtNoteViewModel()
                {
                    Id = result.Id,
                    isOwed = result.IsOwed,
                    CreatedDate = result.CreatedDate,
                    LastModifiedDate = result.LastModifiedDate,
                    Total = result.Total,
                    Code = result.Code,
                    PaymentTypeId = result.PaymentType.Id,
                    WarehouseId = result.PayWarehouse.Id
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
                _debtNote.DeleteDebtNote(key);
                unitOfWork.Commit();
            }
            return StatusCode(HttpStatusCode.OK);
            //return StatusCode(HttpStatusCode.NotImplemented);
        }
        public IEnumerable<DebtStockGroupViewModel> GetDebtInStock([FromUri] SearchingStockViewModel model)
        {
            return _debtNote.GetDebtInStock(model);
        }
    }
}