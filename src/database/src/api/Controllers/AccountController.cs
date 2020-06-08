using CQRS.Database.Application;
using CQRS.Database.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CQRS.Database.Controllers
{
    [ApiController, Route("v1/[controller]")]
    public class AccountController : Controller
    {
        private readonly AccountApplication accountApplication;

        public AccountController(AccountApplication accountApplication)
            => this.accountApplication = accountApplication;

        [HttpPost, Route("")]
        public async Task<IActionResult> Save(Account account)
        {
            await accountApplication.Save(account);

            return Ok();
        }

        [HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await accountApplication.Get(id);

            return Ok(result);
        }

        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            accountApplication.Delete(id);

            return Ok();
        }
    }
}
