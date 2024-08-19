using AutoMapper;
using IndWalks.API.Models.Domain;
using IndWalks.API.Models.DTO;
using IndWalks.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.InteropServices;

namespace IndWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        //private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository)//, IMapper mapper
        {
            this.regionRepository = regionRepository;
            //this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await regionRepository.GetAllAsync();

            //return Dto Regions
            var regionsDTO = new List<Models.DTO.Region>();
            regions.ToList().ForEach(region =>
            {

                var regionDTO = new Models.DTO.Region()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    Area = region.Area,
                    Lat = region.Lat,
                    Long = region.Long,
                    Population = region.Population,
                };

                //var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

                regionsDTO.Add(regionDTO);
            });

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("id:Guid")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            var regionDto = new Models.DTO.Region()
            {
                Id=region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,

            };
           
            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddResionAsync(AddRegionRequest addRegionRequest)
        {
            //Validate the request
            if (!ValidateAddResionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            }

            //reques(DTO)t to Domain model
            var regionDomain = new Models.Domain.Region()
            {
                Name = addRegionRequest.Name,
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };

            //Pass details to repository
            regionDomain = await regionRepository.AddAsync(regionDomain);

            //convert back to DTO
            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = new Models.DTO.Region()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                Area = regionDomain.Area,
                Lat = regionDomain.Lat,
                Long = regionDomain.Long,
                Population = regionDomain.Population
            };

            //return Ok(regionDto);
            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDto.Id }, regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get region from Database
            var region = await regionRepository.DeleteAsync(id);

            //if null Notfound
            if( region == null )
            {
                return NotFound();
            }

            //Convert response back to DTO
            var regionDto = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            //return Ok response
            return Ok(regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] UpdateRegionRequest updateRegionRequest)
        {
            //Validate Update region request
            if(!ValidateUpdateResionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }

            //Convert DTO to Domain model
            var regionDomain = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population
            };

            //Update Region using repository

            regionDomain =  await regionRepository.UpdateAsync(id, regionDomain);

            //If null then Notfound
            if(regionDomain==null)
            {
                return NotFound();
            }

            //convert Domain to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                Area = regionDomain.Area,
                Lat = regionDomain.Lat,
                Long = regionDomain.Long,
                Population = regionDomain.Population
            };

            //Return Ok response
            return Ok(regionDTO);
        }

        #region Private method

        private bool ValidateAddResionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {

            if(addRegionRequest==null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), $"Add Region Data is required");
                return false;
            }

            if(string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),$"{addRegionRequest.Code} can not be  null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{addRegionRequest.Name} can not be  null or whitespace");
            }

            if (addRegionRequest.Area<=0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{addRegionRequest.Area} can not be  less then or equal to zero");
            }

            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{addRegionRequest.Population} can not be  less then or equal to zero");
            }

            if(ModelState.ErrorCount>0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateResionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {

            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest), $"Update Region Data is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{updateRegionRequest.Code} can not be  null or whitespace");
            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{updateRegionRequest.Name} can not be  null or whitespace");
            }

            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{updateRegionRequest.Area} can not be  less then or equal to zero");
            }

            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{updateRegionRequest.Population} can not be  less then or equal to zero");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
