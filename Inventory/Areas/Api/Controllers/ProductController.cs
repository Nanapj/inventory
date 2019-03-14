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
    public class ProductController : ODataController
    {
        private readonly ProductService _product;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ProductController(ProductService product, IUnitOfWorkManager unitOfWorkManager)
        {
            _product = product;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [EnableQuery]
        public IEnumerable<ProductViewModel> Get()
        {

            return _product.QueryableViewModel().ToList();
        }

        public IHttpActionResult Put(Guid key, ProductViewModel delta)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {


                _product.UpdateProduct(delta);
                unitOfWork.Commit();
                return StatusCode(HttpStatusCode.OK);
            }
        }

        // POST: odata/Warehouse
        public IHttpActionResult Post(ProductViewModel delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.
            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                var result = _product.AddProduct(delta);
                unitOfWork.Commit();
                var resultObject = new ProductViewModel()
                {
                    Id = result.Id,
                    ProductName = result.ProductName,
                    Code = result.Code,
                    Price = result.Price,
                    Note = result.Note
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
                _product.DeleteProduct(key);
                unitOfWork.Commit();
            }
            return StatusCode(HttpStatusCode.OK);
            //return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}