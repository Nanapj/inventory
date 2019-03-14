using Inventory.Domain.Interface.UnitOfWork;
using Inventory.Domain.ViewModels;
using Inventory.Models;
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

    public class WarehouseController : ODataController
    {
        // GET: Api/Warehouse
        private readonly WarehouseService _warehouse;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public WarehouseController(WarehouseService warehouse, IUnitOfWorkManager unitOfWorkManager)
        {
            _warehouse = warehouse;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [EnableQuery]
        public IEnumerable<WarehouseViewModel> Get([FromUri] SearchingViewModel model)
        {

            return _warehouse.QueryableViewModel(model).ToList();
        }

        public IHttpActionResult Put(Guid key, WarehouseViewModel delta)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {


                var result = _warehouse.UpdateWarehouse(delta);
                unitOfWork.Commit();
                return StatusCode(HttpStatusCode.OK);

            }


        }

        // POST: odata/Warehouse
        public IHttpActionResult Post(WarehouseViewModel delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.
            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                var result = _warehouse.AddWarehouse(delta);
                unitOfWork.Commit();
                var resultObject = new WarehouseViewModel()
                {
                    Id = result.Id,
                    Name = result.Name,
                    Address = result.Address,
                    PhoneNumber = result.PhoneNumber
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
                _warehouse.DeleteWarehouse(key);
                unitOfWork.Commit();
            }
            return StatusCode(HttpStatusCode.OK);
            //return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}