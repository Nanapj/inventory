using Inventory.Domain.Interface.UnitOfWork;
using Inventory.Domain.ViewModels;
using Inventory.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Inventory.Areas.Api.Controllers
{
    public class NoteController : ODataController
    {
        private readonly NoteService _note;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public NoteController(NoteService note, IUnitOfWorkManager unitOfWorkManager)
        {
            _note = note;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [EnableQuery]
        public IEnumerable<NoteViewModel> Get([FromUri] SearchingViewModel model)
        {

            return _note.QueryableViewModel(model).ToList();
        }

        [EnableQuery]
        public NoteViewModel Get(Guid key)
        {
            return _note.SpecifiedNote(key);
        }

        public IHttpActionResult Put(Guid key, NoteViewModel delta)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                _note.UpdateNote(delta);
                unitOfWork.Commit();
                return StatusCode(HttpStatusCode.OK);
            }
        }

        // POST: odata/Warehouse
        public IHttpActionResult Post(NoteViewModel delta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.
            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                var result = _note.AddNote(delta);
                unitOfWork.Commit();
                if(result.Source == null)
                {
                    var resultObject = new NoteViewModel()
                    {
                        Id = result.Id,
                        DestinationId = result.Destination.Id,
                        SourceId = null,
                        CreatedDate = result.CreatedDate,
                        LastModifiedDate = result.LastModifiedDate,
                        Noted = result.Noted,
                        Code = result.Code,
                        ProductType = result.ProductType,
                        //Details = result.Details.Select(y => new NoteDetailViewModel()
                        //{
                        //    Id = y.Id,
                        //    Price = y.Price,
                        //    Quantity = y.Quantity
                        //}).ToList(),
                        Total = result.Total
                    };
                    return Created(resultObject);
                }else if(result.Destination == null)
                {
                    var resultObject = new NoteViewModel()
                    {
                        Id = result.Id,
                        DestinationId = null,
                        SourceId = result.Source.Id,
                        CreatedDate = result.CreatedDate,
                        LastModifiedDate = result.LastModifiedDate,
                        Noted = result.Noted,
                        Code = result.Code,
                        ProductType = result.ProductType,
                        Details = result.Details.Select(y => new NoteDetailViewModel()
                        {
                            Id = y.Id,
                            Price = y.Price,
                            Quantity = y.Quantity
                        }).ToList(),
                        Total = result.Total
                    };
                    return Created(resultObject);
                }
                else
                {
                    var resultObject = new NoteViewModel()
                    {
                        Id = result.Id,
                        DestinationId = result.Destination.Id,
                        SourceId = result.Source.Id,
                        CreatedDate = result.CreatedDate,
                        LastModifiedDate = result.LastModifiedDate,
                        Noted = result.Noted,
                        Code = result.Code,
                        ProductType = result.ProductType,
                        Details = result.Details.Select(y => new NoteDetailViewModel()
                        {
                            Id = y.Id,
                            Price = y.Price,
                            Quantity = y.Quantity
                        }).ToList(),
                        Total = result.Total
                    };
                    return Created(resultObject);
                }                           
            }           
        }
        // DELETE: odata/Warehouse(5)
        public IHttpActionResult Delete(Guid key)
        {
            // TODO: Add delete logic here.
            using (var unitOfWork = _unitOfWorkManager.NewUnitOfWork())
            {
                _note.DeleteNote(key);
                unitOfWork.Commit();
            }
            return StatusCode(HttpStatusCode.OK);
            //return StatusCode(HttpStatusCode.NotImplemented);
        }

        public IEnumerable<StockGroupViewModel> GetProductInStock([FromUri]SearchingStockViewModel model)
        {
            return _note.GetProductInStock(model);
        }

    }
}