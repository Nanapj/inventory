using ClosedXML.Excel;
using Inventory.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using WebGrease.Css.Extensions;
using Microsoft.OData.Edm;
using Inventory.Domain.ViewModels;
using Inventory.Models;
using Inventory.Core;
using Inventory.Domain;
using Inventory.Data;
using Microsoft.AspNet.Identity;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using Inventory.Areas.Api.Controllers;

namespace Inventory.Controllers
{
    public class ExcelController : Controller
    {
        private readonly NoteService _note;
        private readonly NoteDetailService _noteDetail;
        private readonly WarehouseService _warehouse;
        private readonly ProductService _product;
        public readonly InventoryContext db;
        public ExcelController(
            NoteService note,
            NoteDetailService noteDetail,
            ProductService product,
            WarehouseService warehouse) 
        {
            _warehouse = warehouse;
            _noteDetail = noteDetail;
            _note = note;
            _product = product;
            db = new InventoryContext();
        }
     
        public String NumberToTextVN(double total)
        {
            try
            {
                string rs = "";
                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "ngàn", "", "", "triệu", "", "", "tỷ", "", "", "ngàn", "", "", "triệu" };
                string nstr = total.ToString();

                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }

                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số 
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                                continue;
                            }
                        }
                    }

                    rs += (rs == "" ? " " : ", ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }
                if (rs[rs.Length - 1] != ' ')
                    rs += " đồng";
                else
                    rs += "đồng";

                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                Debug.WriteLine(rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười"));
                return rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười");
            }
            catch
            {
                return "";
            }

        }
        [HttpGet]
        public ActionResult ReNoteExcel(Guid? Id)
        {
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "*");
            if (Id != null)
            {
                string filename = Server.MapPath("~/Content/ExcelFile/PhieuNhapKho.xlsx");
                using (XLWorkbook wb = new XLWorkbook(filename))
                {
                    var Note = _note.Queryable().Include(x => x.Details.Select(y => y.Product)).Include(x => x.Source).Include(x => x.Source).Where(x => x.Id == Id).FirstOrDefault();
                    var sheet = wb.Worksheet(1);
                    var dateRow = sheet.Row(2);
                    dateRow.Cell("E").Value = DateTime.Now.ToString("dd") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.Year;
                    var codeRow = sheet.Row(4);
                    codeRow.Cell("E").Value = Note.Code;
                    var deliver = sheet.Row(5);
                    var WarehouseName = _warehouse.Queryable().Where(t => t.Id == Note.Source.Id).Select(t => t.Name).FirstOrDefault();
                    deliver.Cell("B").Value = WarehouseName;
                    var address = sheet.Row(6);
                    address.Cell("B").Value = _warehouse.Queryable().Where(t => t.Id == Note.Source.Id).Select(t => t.Address).FirstOrDefault();
                    var number = sheet.Row(7);
                    number.Cell("b").Value = _warehouse.Queryable().Where(t => t.Id == Note.Source.Id).Select(t => t.PhoneNumber).FirstOrDefault();
                    var typeProduct = sheet.Row(8);
                    typeProduct.Cell("B").Value = "Hàng mới";

                    var detailRows = sheet.Row(11);
                    detailRows.InsertRowsBelow(1);
                    var currentRow = 11;
                    var index = 1;
                    foreach(var item in Note.Details)
                    {
                        var detailrow = sheet.Row(currentRow);
                        detailrow.InsertRowsBelow(1);
                        detailrow.Cell("A").Value = index;
                        detailrow.Cell("B").Value = item.Product.ProductName;
                        detailrow.Cell("C").Value = item.Unit;
                        detailrow.Cell("D").Value = item.Quantity;
                        detailrow.Cell("E").Value = Note.Noted;
                        if(currentRow%2 == 0)
                        {

                            detailrow.Cell("A").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                            detailrow.Cell("B").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                            detailrow.Cell("C").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                            detailrow.Cell("D").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                            detailrow.Cell("E").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                            detailrow.Cell("F").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                        }
                        index++;
                        currentRow++;
                    }
                    sheet.Range(sheet.Cell(currentRow+2,"A"), sheet.Cell(currentRow+2,"E")).Merge();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PhieuNhapKho.xlsx");
                    }
                }
            }
            else
            {
                string errorfilename = Server.MapPath("~/Content/ExcelFile/UndefinedNote.xlsx");
                using (XLWorkbook wb = new XLWorkbook(errorfilename))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PhieuNhapKho.xlsx");
                    }
                }
            }           
        }
        [HttpGet]
        public ActionResult DeNoteExcel(Guid? Id)
        {
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "*");
            if (Id != null)
            {
                string filename = Server.MapPath("~/Content/ExcelFile/PhieuXuatKho.xlsx");
                using (XLWorkbook wb = new XLWorkbook(filename))
                {
                    var Note = _note.Queryable().Include(x => x.Details.Select(y => y.Product)).Include(x => x.Destination).Include(x => x.Source).Where(x => x.Id == Id).FirstOrDefault();
                    var sheet = wb.Worksheet(1);
                    var dateRow = sheet.Row(2);
                    dateRow.Cell("H").Value = DateTime.Now.ToString("dd") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.Year;
                    var codeRow = sheet.Row(4);
                    codeRow.Cell("H").Value = Note.Code;
                    var receiver = sheet.Row(5);
                    receiver.Cell("D").Value = _warehouse.Queryable().Where(t => t.Id == Note.Destination.Id).Select(t => t.Name).FirstOrDefault();
                    var address = sheet.Row(6);
                    address.Cell("D").Value = _warehouse.Queryable().Where(t => t.Id == Note.Destination.Id).Select(t => t.Address).FirstOrDefault();
                    var number = sheet.Row(7);
                    number.Cell("D").Value = _warehouse.Queryable().Where(t => t.Id == Note.Destination.Id).Select(t => t.PhoneNumber).FirstOrDefault();
                    var typeProduct = sheet.Row(8);
                    typeProduct.Cell("D").Value = "Hàng mới";
                    var Noted = sheet.Row(9);
                    Noted.Cell("D").Value = Note.Noted;
                    var detailRows = sheet.Row(13);
                    detailRows.InsertRowsBelow(1);
                    var currentRow = 13;
                    var index = 1;
                    foreach (var item in Note.Details)
                    {
                        var detailrow = sheet.Row(currentRow);
                        detailrow.InsertRowsBelow(1);
                        detailrow.Cell("A").Value = index;
                        detailrow.Cell("B").Value = item.Product.ProductName;
                        sheet.Range("B" + currentRow + ":" + "D" + currentRow).Row(1).Merge();
                        detailrow.Cell("E").Value = item.Unit;
                        detailrow.Cell("F").Value = item.Quantity;
                        detailrow.Cell("G").Value = item.Price;
                        detailrow.Cell("H").Value = item.Quantity * item.Price;
                        if (currentRow % 2 == 0)
                        {
                            detailrow.Cell("A").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                            detailrow.Cell("B").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                            detailrow.Cell("E").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                            detailrow.Cell("F").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                            detailrow.Cell("G").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                            detailrow.Cell("H").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                        }
                        index++;
                        currentRow++;
                    }
                    var finalRow = sheet.Row(currentRow);
                    finalRow.Cell("B").Value = "Cộng";
                    finalRow.Cell("F").Value = Note.Details.Sum(x => x.Quantity);
                    finalRow.Cell("H").Value = Note.Total;
                    if (currentRow % 2 == 0)
                    {
                        finalRow.Cell("A").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                        finalRow.Cell("B").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                        finalRow.Cell("E").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                        finalRow.Cell("F").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                        finalRow.Cell("G").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                        finalRow.Cell("H").Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
                    }
                    var sumRow = sheet.Row(currentRow + 2);
                    sumRow.Cell("A").Value += NumberToTextVN(Note.Total).Replace(",","");
                    sheet.Range(sheet.Cell(currentRow + 2, "A"), sheet.Cell(currentRow + 2, "H")).Merge();
                    sheet.Range(sheet.Cell(currentRow + 3, "A"), sheet.Cell(currentRow + 3, "H")).Merge();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PhieuXuatKho.xlsx");
                    }
                }
            }
            else
            {
                string errorfilename = Server.MapPath("~/Content/ExcelFile/UndefinedNote.xlsx");
                using (XLWorkbook wb = new XLWorkbook(errorfilename))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PhieuXuatKho.xlsx");
                    }
                }
            }
        }
        [HttpPost]
        public ActionResult UploadNote()
        {
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            Request.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Headers", "*");
            if (Request.Files.Count > 0)
            {
                try
                {
                    var NoteList = new List<NoteViewModel>();
                    //  Get all files from Request object  
                    string firstfname = "";
                    string fname;
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];             

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }
                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        firstfname = fname;
                        file.SaveAs(fname);
                    }
                    using (XLWorkbook wb = new XLWorkbook(firstfname))
                    {
                        var ws = wb.Worksheet(1);
                        var sheet = wb.Worksheet(1).RowsUsed();
                        var usedColumn = wb.Worksheet(1).ColumnsUsed();
                        var ExceptRow = 1;
                        foreach (var dataRow in sheet)
                        {
                            var Note = new NoteViewModel();
                            if (ExceptRow == 1)
                            {
                                ExceptRow++;
                            }
                            else
                            {
                                var NoteType = ws.Cell(1, 1).Value.ToString();
                                switch (NoteType)
                                {
                                    case "NK":
                                        {
                                            Note.Code = "NK" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + (_note.Queryable().Where(mi => mi.Source.Id == null && mi.CreatedDate.Month == DateTime.Now.Month
                                            && mi.CreatedDate.Year == DateTime.Now.Year).Count() + 1).ToString("D3");
                                            Note.Source = null;
                                            var destCode = dataRow.Cell("A").Value.ToString();
                                            Note.DestinationId = _warehouse.Queryable().Where(x => x.WarehouseCode.Equals(destCode)).Select(x => x.Id).FirstOrDefault();
                                            Note.DestinationWarehouse = _warehouse.Queryable().Where(x => x.WarehouseCode.Equals(destCode)).Select(x => x.Name).FirstOrDefault();
                                            Note.Details = new List<NoteDetailViewModel>();
                                            for (int i = 2; i < usedColumn.Count(); i++)
                                            {
                                                if (!(dataRow.Cell(i).Value.ToString() == "" ))
                                                {
                                                   var detail = new NoteDetailViewModel();
                                                    var NumberProduct = (double)dataRow.Cell(i).Value;                                                
                                                    var detailCode = ws.Cell(1, i).Value.ToString();
                                                    var price = _product.Queryable().Where(p => p.ProductName.Equals(detailCode)).Select(p => p.Price).FirstOrDefault();
                                                    detail.Price = price;
                                                    detail.Quantity = (int)NumberProduct;
                                                    detail.ProductId = _product.Queryable().Where(p => p.ProductName.Equals(detailCode)).Select(p => p.Id).FirstOrDefault();
                                                    detail.ProductName = _product.Queryable().Where(p => p.ProductName.Equals(detailCode)).Select(p => p.ProductName).FirstOrDefault();
                                                    detail.Unit = _product.Queryable().Where(p => p.ProductName.Equals(detailCode)).Select(p => p.Unit).FirstOrDefault();
                                                    Note.Details.Add(detail);
                                                }                                              
                                            }
                                            Note.Noted = ws.Cell(ExceptRow, usedColumn.Count()).Value.ToString();
                                            Note.Total = Note.Details.Sum(x => x.Price * x.Quantity);
                                            NoteList.Add(Note);
                                            ExceptRow++;
                                            break;
                                        }
                                    case "XK":
                                        {
                                            Note.Code = "XK" + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + (_note.Queryable().Where(mi => mi.Destination.Id == null && mi.CreatedDate.Month == DateTime.Now.Month
                                            && mi.CreatedDate.Year == DateTime.Now.Year).Count() + 1).ToString("D3");
                                            var sourceCode = dataRow.Cell("A").Value.ToString();
                                            Note.SourceId = _warehouse.Queryable().Where(s => s.WarehouseCode.Equals(sourceCode)).Select(s => s.Id).FirstOrDefault();
                                            Note.Destination = null;
                                            Note.SourceWarehouse = _warehouse.Queryable().Where(s => s.WarehouseCode.Equals(sourceCode)).Select(s => s.Name).FirstOrDefault();
                                            Note.Details = new List<NoteDetailViewModel>();
                                            for (int i = 2; i < usedColumn.Count(); i++)
                                            {
                                                if (!(dataRow.Cell(i).Value.ToString() == ""))
                                                {
                                                    var detail = new NoteDetailViewModel();
                                                    var NumberProduct = (double)dataRow.Cell(i).Value;
                                                    var detailCode = ws.Cell(1, i).Value.ToString();
                                                    var price = _product.Queryable().Where(p => p.ProductName.Equals(detailCode)).Select(p => p.Price).FirstOrDefault();
                                                    detail.Price = price;
                                                    detail.Quantity = (int)NumberProduct;
                                                    detail.ProductId = _product.Queryable().Where(p => p.ProductName.Equals(detailCode)).Select(p => p.Id).FirstOrDefault();
                                                    detail.ProductName = _product.Queryable().Where(p => p.ProductName.Equals(detailCode)).Select(p => p.ProductName).FirstOrDefault();
                                                    detail.Unit = _product.Queryable().Where(p => p.ProductName.Equals(detailCode)).Select(p => p.Unit).FirstOrDefault();
                                                    Note.Details.Add(detail);
                                                }                                              
                                            }
                                            Note.Noted = ws.Cell(ExceptRow, usedColumn.Count()).Value.ToString();
                                            Note.Total = Note.Details.Sum(x => x.Price * x.Quantity);
                                            NoteList.Add(Note);
                                            ExceptRow++;
                                            break;
                                        }
                                    case "CK":
                                        {
                                            break;
                                        }
                                }
                               
                            }
                           
                        }

                        //using (MemoryStream stream = new MemoryStream())
                        //{
                        //    wb.SaveAs(stream);
                        //    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PhieuNhapKho.xlsx");
                        //}
                    }
                    // Returns message that successfully uploaded                
                    return Json(NoteList,JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
    }
}



























//[HttpGet]
//public ActionResult DNoteExcel(Guid Id)
//{
//    var money = NumberToTextVN(1752222687);
//    var workbook = new XLWorkbook();
//    var sheet = workbook.Worksheets.Add("Sheet1");
//    var imagePath = Path.Combine(Server.MapPath("~/Content/Images/logo.png"));
//    var image = sheet.AddPicture(imagePath)
//        .MoveTo(sheet.Cell("B3").Address)
//    .Scale(1);
//    sheet.Cell("A1").Value = money;
//    string myName = Server.UrlEncode("Test" + ".xlsx");
//    string handle = Guid.NewGuid().ToString();
//    using (MemoryStream memoryStream = new MemoryStream())
//    {
//        workbook.SaveAs(memoryStream);
//        memoryStream.Position = 0;
//        TempData[handle] = memoryStream.ToArray();
//    }
//    var myfile = new
//    {
//        FileGuid = handle,
//        FileName = myName
//    };
//    // Note we are returning a filename as well as the handle
//    return Json(myfile, JsonRequestBehavior.AllowGet);
//}