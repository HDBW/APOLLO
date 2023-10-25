using Apollo.Api;
using Apollo.Common.Entities;
using Apollo.RestService.Messages;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Apollo.Service.Controllers
{
    /// <summary>
    /// Implements all operations required to deal with trainings.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingController : ControllerBase
    {
        private readonly ApolloApi _api;

        private readonly ILogger<TrainingController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="api"></param>
        public TrainingController(ApolloApi api, ILogger<TrainingController> logger)
        {
            _api = api;
            _logger = logger;
        }

        /// <summary>
        /// Gets the training for the given taining identifier.
        /// </summary>
        /// <param name="id">The id of the training to be returned.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<GetTrainingResponse> GetTraining(string id)
        {
            _logger.LogTrace($"{nameof(GetTraining)} entered.");

            var res = await _api.GetTraining(id);

            return new GetTrainingResponse { Training = res };
        }

        /// <summary>
        /// Looks up the training that mathc the given criteria.
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public Task<IList<QueryTrainingsResponse>> QueryTrainings([FromBody] QueryTrainingsRequest req)
        {
            //_api.QueryTrainings
            // todo. invoke api..
            return Task.FromResult<IList<QueryTrainingsResponse>>(new List<QueryTrainingsResponse> { new QueryTrainingsResponse() { Trainings = new List<Training>() } });
        }


        // POST api/<TrainingController>
        [HttpPut]
        public Task<CreateOrUpdateTrainingResponse> CreateOrUpdateTraining([FromBody] CreateOrUpdateTrainingRequest req)
        {
            // todo. invoke api..
            return Task.FromResult(new CreateOrUpdateTrainingResponse());
        }


        // DELETE api/<TrainingController>/5
        [HttpDelete("{id}")]
        public Task Delete(List<int> iDs)
        {
            // todo. invoke api..
            return Task.CompletedTask;
        }
    }
}
