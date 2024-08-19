﻿using IndWalks.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IndWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var walkDifficultyDomain = await walkDifficultyRepository.GetAllAsync();

            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            //var walkDifficultyDTO = new List<Models.Domain.WalkDifficulty>
            //{
            //    Id = walkDifficultyDomain.id,
            //    Code = walkDifficultyDomain.Code,
            //};


            var walkDifficultiesDTO = new List<Models.DTO.WalkDifficultyDto>();

            walkDifficultyDomain.ToList().ForEach(walkdifficulty =>
            {
                var walkDifficultyDTO = new Models.DTO.WalkDifficultyDto()
                {
                    Id = walkdifficulty.Id,
                    Code = walkdifficulty.Code
                };
                walkDifficultiesDTO.Add(walkDifficultyDTO);
            });

            return Ok(walkDifficultiesDTO);
        }


        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetAllWalkDifficultiesBYId")]
        public async Task<IActionResult> GetAllWalkDifficultiesBYId(Guid id)
        {
            var walkDifficultyDomain = await walkDifficultyRepository.GetAsync(id);

            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var walkDifficultyDTO = new Models.Domain.WalkDifficulty
            {
                Id = walkDifficultyDomain.Id,
                Code = walkDifficultyDomain.Code,
            };

            return Ok(walkDifficultyDomain);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkdifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //Convert DTO to Domain object
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };

            //Call repository
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);
            //convert Domain to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficultyDto
            {
                Id = walkDifficultyDomain.Id,
                Code = walkDifficultyDomain.Code,
            };

            //Return
            return CreatedAtAction(nameof(GetAllWalkDifficultiesBYId),new {id= walkDifficultyDTO.Id },walkDifficultyDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkdifficultyAsync([FromRoute] Guid id,[FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //Convert DTO to Domain object
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            //Call repository
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id,walkDifficultyDomain);

            if(walkDifficultyDomain==null)
            {
                return NotFound();
            }

            //convert Domain to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficultyDto
            {
                Id = walkDifficultyDomain.Id,
                Code = walkDifficultyDomain.Code,
            };

            //Return
            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultAsync(Guid id)
        {
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);

            if(walkDifficultyDomain==null)
            {
                return NotFound();
            }

            var walDifficultyDTO = new Models.DTO.WalkDifficultyDto
            {
                Id = walkDifficultyDomain.Id,
                Code = walkDifficultyDomain.Code,
            };

            return Ok(walDifficultyDTO);
        }
    }
}
