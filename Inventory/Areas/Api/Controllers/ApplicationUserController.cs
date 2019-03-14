using System.Web;
using System.Web.Http;
using Inventory.Core;
using Inventory.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.OData;

namespace Inventory.Areas.Api.Controllers
{
    public class ApplicationUserController : ODataController
    {
        private ApplicationUserManager _userManager;

        public ApplicationUserController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // POST: odata/ApplicationUser
        public IHttpActionResult Post([FromUri]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
            var result = UserManager.Create(user, model.Password);
            if (result.Succeeded)
            {
                return Content(System.Net.HttpStatusCode.Created, "Succeed");
            }
            else
            {
                return Content(System.Net.HttpStatusCode.BadRequest,result.Errors);
            }
        }
    }
}
