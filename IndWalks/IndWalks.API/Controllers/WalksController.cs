using IndWalks.API.Models.DTO;
using IndWalks.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IndWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;

        public WalksController(IWalkRepository walkRepository)
        {
            this.walkRepository = walkRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkAsync()
        {
            //Featch data from database
            var walkDomain = await walkRepository.GetAllAsync();

            if (walkDomain == null)
            {
                return NotFound();
            }

            //convert domain walks to DTO
            var walksDto = new List<Models.DTO.WalkDto>();
            walkDomain.ToList().ForEach(walk =>
            {
                var walkDto = new Models.DTO.WalkDto()
                {
                    Id = walk.Id,
                    Name = walk.Name,
                    Length = walk.Length,
                    RegionId=walk.RegionId,
                    WalkDifficultyId = walk.WalkDifficultyId

                };

                //var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

                walksDto.Add(walkDto);
            });

            return Ok(walksDto);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get walk Domain object from database
            var walkDomain = await walkRepository.GetAsync(id);

            //Convert Domain object to DTO
            var walkDto = new Models.DTO.WalkDto()
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            //REturn Response
            return Ok(walkDto);

        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AddWalkRequest addWalkRequest)
        {
            //convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId=addWalkRequest.WalkDifficultyId
            };

            //Pass Domain object to Repository to persisit this
            walkDomain=await walkRepository.AddAsync(walkDomain);

            //Convert the Domain object to DTO
            var walkDTO = new Models.DTO.WalkDto
            {
                Id=walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId=walkDomain.WalkDifficultyId
            };

            //Send DtO response back to client
            return CreatedAtAction(nameof(GetWalkAsync),new { id=walkDTO.Id}, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length=updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            //pass deails to Repository - Get Domai object response (or null)
            walkDomain=await walkRepository.UpdateAsync(id,walkDomain);


            //Handle Null (Not found)
            if (walkDomain == null)
            {
                return NotFound();
            }
            else
            {
                //Convert back doamin to Dto
                var walkDTO = new Models.DTO.WalkDto
                {
                    Id = walkDomain.Id,
                    Name = walkDomain.Name,
                    Length = walkDomain.Length,
                    RegionId = walkDomain.RegionId,
                    WalkDifficultyId = walkDomain.WalkDifficultyId
                };

                //Retunr Response
                return Ok(walkDTO);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //call repository to delete walk
            var walkDomain = await walkRepository.DeleteAsync(id);
            if(walkDomain == null)
            {
                return NotFound();
            }

            var walkDTO = new Models.DTO.WalkDto
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            return Ok(walkDTO);

        }
    }
}
