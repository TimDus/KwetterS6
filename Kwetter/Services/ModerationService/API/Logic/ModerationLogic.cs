using MediatR;
using ModerationService.API.Models.DTO;
using ModerationService.API.Models.Entity;
using ModerationService.API.Repositories;
using System.Text;
using OpenAI_API;
using OpenAI_API.Completions;

namespace ModerationService.API.Logic
{
    public class ModerationLogic : IModerationLogic
    {
        //private readonly IMediator _mediator;
        private readonly IModerationRepository _repository;
        private readonly IConfiguration _configuration;
        private const string Gpt3Endpoint = "https://api.openai.com/v1/completions";

        public ModerationLogic(IMediator mediator, IModerationRepository repository, IConfiguration configuration)
        {
            //_mediator = mediator;
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<string> CheckKweet(KweetDTO kweetDTO)
        {
            string ApiKey = "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";
            string response = string.Empty;

            var openai = new OpenAIAPI(ApiKey);

            CompletionRequest completion = new();

            return "no";

            completion.Prompt = "Determine if the text after sentence carries negative sentiment and respond with yes or no. " + kweetDTO.Text;
            completion.Model = "gpt-3.5-turbo";
            completion.MaxTokens = 200;

            CompletionResult result = await openai.Completions.CreateCompletionsAsync(completion);

            foreach(var item in result.Completions) 
            {
                response = item.Text;
            }
        }

        public async Task<List<KweetDTO>> GetPendingList()
        {
            List<KweetEntity> kweetEntities = await _repository.GetPendingList();

            List<KweetDTO> kweets = new();

            foreach(KweetEntity kweet in kweetEntities)
            {
                kweets.Add(new(kweet.KweetServiceId, kweet.Customer.Id, kweet.Customer.CustomerName, kweet.Text, kweet.CreatedDate));
            }

            return kweets;
        }
    }
}
