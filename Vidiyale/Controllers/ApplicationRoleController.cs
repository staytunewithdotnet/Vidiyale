using Vidiyale.Data;
using Vidiyale.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vidiyale.Models.UserRole;
using Microsoft.EntityFrameworkCore;

namespace Vidiyale.Controllers
{
    public class ApplicationRoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> roleManager;

        public ApplicationRoleController(RoleManager<ApplicationRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await roleManager.Roles.Select(r => new UserRoleListViewModel
            {
                RoleName = r.Name,
                Id = r.Id,
                Description = r.Description//,
                //NumberOfUsers = r.Users.Count
            }).ToListAsync();

            return View(model);
        }

        public IActionResult Create()
        {
            var model = new UserRoleViewModel();
            return View();
        }

        // POST: UserRole/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRoleViewModel userrole)
        {

            if (ModelState.IsValid)
            {
                ApplicationRole applicationRole = new ApplicationRole { CreatedDate = DateTime.UtcNow };
                applicationRole.Name = userrole.RoleName;
                applicationRole.Description = userrole.Description;
                applicationRole.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();

                IdentityResult roleRuslt = await roleManager.CreateAsync(applicationRole);
                if (roleRuslt.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(userrole);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            UserRoleViewModel model = new UserRoleViewModel();
            if (!String.IsNullOrEmpty(Id))
            {
                ApplicationRole applicationRole = await roleManager.FindByIdAsync(Id);
                if (applicationRole != null)
                {
                    model.Id = applicationRole.Id;
                    model.RoleName = applicationRole.Name;
                    model.Description = applicationRole.Description;
                }
            }
            return View(model);
        }

        // POST: UserRole/Update
        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isExist = !String.IsNullOrEmpty(model.Id);
                ApplicationRole applicationRole = isExist ? await roleManager.FindByIdAsync(model.Id) :
               new ApplicationRole
               {
                   CreatedDate = DateTime.UtcNow
               };
                applicationRole.Name = model.RoleName;
                applicationRole.Description = model.Description;
                applicationRole.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                IdentityResult roleRuslt = isExist ? await roleManager.UpdateAsync(applicationRole)
                                                    : await roleManager.CreateAsync(applicationRole);
                if (roleRuslt.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        
        // GET: ApplicationRole/Delete/5
        public async Task<IActionResult> Delete(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var model = await roleManager.FindByIdAsync(Id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: ApplicationRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            ApplicationRole applicationRole = await roleManager.FindByIdAsync(Id);
            if (applicationRole != null)
            {
                IdentityResult roleRuslt = roleManager.DeleteAsync(applicationRole).Result;
                if (roleRuslt.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return NotFound();
        }

    }
}