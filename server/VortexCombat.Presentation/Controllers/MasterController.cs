using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VortexCombat.Domain.Entities;
using VortexCombat.Domain.Interfaces;

namespace VortexCombat.Presentation.Controllers
{
    [Route("api/masters")]
    [ApiController]
    [Authorize(Roles = "PrimaryMaster")]
    public class MasterController : ControllerBase
    {
        private readonly IMasterRepository _masterRepository;

        public MasterController(IMasterRepository masterRepository)
        {
            _masterRepository = masterRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Master>>> GetMasters()
        {
            var masters = await _masterRepository.GetAllWithUserAsync();
            return Ok(masters);
        }
    }
}