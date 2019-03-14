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
    public class PaymentTypeController : ODataController
    {
        // GET: Api/Payment
        private readonly PaymentTypeService _paymentType;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PaymentTypeController(PaymentTypeService paymentType, IUnitOfWorkManager unitOfWorkManager)
        {
            _paymentType = paymentType;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [EnableQuery]
        public IEnumerable<PaymentTypeViewModel> Get()
        {

            return _paymentType.QueryableViewModel().ToList();
        }

        public IHttpActionResult Put(Guid key, PaymentTypeViewModel delta)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                _paymentType.UpdatePaymentType(delta);
                unitOfWork.Commit();
                return StatusCode(HttpStatusCode.OK);
            }
        }

        // POST: odata/Warehouse
        public IHttpActionResult Post(PaymentTypeViewModel delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.
            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                var result = _paymentType.AddPaymentType(delta);
                unitOfWork.Commit();
                var resultObject = new PaymentTypeViewModel()
                {
                    Id = result.Id,
                    Name = result.Name
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
                _paymentType.DeletePaymentType(key);
                unitOfWork.Commit();
            }
            return StatusCode(HttpStatusCode.OK);
            //return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}