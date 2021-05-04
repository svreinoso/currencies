using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Currencies.Data;
using Currencies.Data.Entities;
using Currencies.Services;
using Currencies.Data.Dtos;

namespace Currencies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPurchaseServices _purchaseServices;

        public PurchasesController(ApplicationDbContext context, IPurchaseServices purchaseServices)
        {
            _context = context;
            _purchaseServices = purchaseServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchases(int userId)
        {
            return await _purchaseServices.GetPurchaseByUserId(userId);
        }

        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(PurchaseDto purchase)
        {
            return await _purchaseServices.PostPurchase(purchase);
        }

    }
}
