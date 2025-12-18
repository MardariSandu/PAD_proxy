using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyncNode.Services;

namespace SyncNode.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly SyncWorkJobService _workjobService;

        public SyncController(SyncWorkJobService workJobService)
        {
            _workjobService = workJobService;
        }

        [HttpPost]
        public IActionResult Sync(SyncEntity entity)
        {
            _workjobService.AddItem(entity);

            return Ok();
        }
    }
}
