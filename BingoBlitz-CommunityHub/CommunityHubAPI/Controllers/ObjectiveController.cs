using BingoBlitz_CommunityHub.Data;
using BingoBlitz_CommunityHub.Data.Interfaces;
using BingoBlitz_CommunityHub.Models;
using Microsoft.AspNetCore.Mvc;

namespace BingoBlitz_CommunityHub.Controllers
{
    [ApiController]
    [Route("api/objectives/")]
    public class ObjectiveController : ControllerBase
    {
        private readonly ILogger<ObjectiveController> _logger;
        private readonly IObjectiveData _objectiveData;

        public ObjectiveController(ILogger<ObjectiveController> logger, IObjectiveData objectiveData)
        {
            _logger = logger;
            _objectiveData = objectiveData;
        }

        [Obsolete("Method currently not working as intended. Use 'collections/get' instead")]
        [HttpGet]
        [Route("collections/getqueryable")]
        public async Task<ActionResult<IterableObjectiveCollectionData>> GetObjectiveCollectionsQueryable(
            string? continuationToken = null,
            int start = 0,
            int count = 10,
            string? filter = null)
        {
            var result = await _objectiveData.QueryCollectionsByPage(start, count, continuationToken, filter);
            return result;
        }

        [HttpGet]
        [Route("collections/get")]
        public async Task<ActionResult<List<ObjectiveCollection>>> GetObjectiveCollections()
        {
            return await _objectiveData.GetAllObjectiveCollections();
        }

        [HttpGet]
        [Route("collections/getbyid")]
        public async Task<ActionResult<ObjectiveCollection>> GetObjectiveCollectionById(string id) 
        {
            ObjectiveCollection objectiveCollection = await _objectiveData.GetObjectiveCollectionById(id);

            return objectiveCollection;
        }

        [HttpPost]
        [Route("collections/save")]
        public async Task<ActionResult<string>> SaveObjectiveCollection(ObjectiveCollection objectiveCollection)
        {
            if (objectiveCollection == null) return StatusCode(StatusCodes.Status400BadRequest, "'objectiveCollection' can not be null.");

            return await _objectiveData.SaveObjectiveCollection(objectiveCollection);
        }
    }
}