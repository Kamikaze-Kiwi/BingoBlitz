using BingoBlitz_CommunityHub.Data;
using BingoBlitz_CommunityHub.Data.Interfaces;
using BingoBlitz_CommunityHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

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

        [HttpGet]
        [Route("collections/getqueryable")]
        public async Task<ActionResult<IterableObjectiveCollectionData>> GetObjectiveCollectionsQueryable(
            string? continuationToken = null,
            int count = 10,
            string? filter = null)
        {
            var result = await _objectiveData.QueryCollectionsByPage(count, continuationToken, filter ?? "");
            return result;
        }

        [HttpGet]
        [Route("collections/getqueryablebyuser")]
        public async Task<ActionResult<IterableObjectiveCollectionData>> GetObjectiveCollectionsQueryableByUser(
            string userId,
            string? continuationToken = null,
            int count = 10,
            string? filter = null)
        {
            var result = await _objectiveData.QueryCollectionsByPage(count, continuationToken, filter ?? "", userId);
            return result;
        }

        [HttpGet]
        [Route("collections/getbyid")]
        public async Task<ActionResult<ObjectiveCollection>> GetObjectiveCollectionById(string id) 
        {
            ObjectiveCollection objectiveCollection = await _objectiveData.GetObjectiveCollectionById(id);

            return objectiveCollection;
        }

        [HttpPost]
        [Authorize]
        [Route("collections/save")]
        public async Task<ActionResult<string>> SaveObjectiveCollection(ObjectiveCollection objectiveCollection)
        {
            string? userId = User.FindFirst("sub")?.Value;
            if (userId == null || userId != objectiveCollection.UserId) return StatusCode(StatusCodes.Status401Unauthorized, "User not authorized to make request.");

            if (objectiveCollection == null) return StatusCode(StatusCodes.Status400BadRequest, "'objectiveCollection' can not be null.");

            return await _objectiveData.SaveObjectiveCollection(objectiveCollection);
        }
    }
}