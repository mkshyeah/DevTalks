using Framework.FailureResponseExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questions.Application.Features.AddAnswerCommand;
using Questions.Application.Features.CreateQuestionCommand;
using Questions.Application.Features.GetQuestionsWithFiltersQuery;
using Questions.Contracts.Dto;
using Questions.Contracts.Responses;
using Shared.Abstractions;

namespace Questions.Presenters;

[ApiController]
[Route("[controller]")]
public class QuestionsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<Guid, CreateQuestionCommand> handler,
        [FromBody] CreateQuestionDto createQuestionDto, 
        CancellationToken cancellationToken)
    {
        var command = new CreateQuestionCommand(createQuestionDto);
        
        var createResult = await handler.Handle(command, cancellationToken);
        return createResult.IsFailure ? createResult.Error.ToFailureResponse() : Ok(createResult.Value);
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromServices] IQueryHandler<QuestionResponse, GetQuestionsWithFiltersQuery> queryHandler,
        [FromQuery] GetQuestionDto getQuestionDto,
        CancellationToken cancellationToken)
    {
        var query = new GetQuestionsWithFiltersQuery(getQuestionDto);
        
        var response = await queryHandler.Handle(query, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet("questionId:guid")]

    public async Task<IActionResult> GetById(
        [FromRoute] Guid questionId,
        CancellationToken cancellationToken)
    {
        return Ok("Question get");
    }

    [HttpPut("questionId:guid")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid questionId,
        [FromBody] UpdateQuestionDto request,
        CancellationToken cancellationToken)
    {
        return Ok("Question updated");
    }

    [HttpDelete("questionId:guid")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid questionId,
        CancellationToken cancellationToken)
    {
        return Ok("Question deleted");
    }

    [HttpPut("questionId:guid/solution")]
    public async Task<IActionResult> SelectSolution(
        [FromRoute] Guid questionId,
        [FromQuery] Guid answerId,
        CancellationToken cancellationToken)
    {
        return Ok("Solution selected");
    }
    
    [HttpPost("questionId:guid/answer")]
    public async Task<IActionResult> AddAnswer(
        [FromServices] ICommandHandler<Guid, AddAnswerCommand> handler,
        [FromRoute] Guid questionId,
        [FromBody] AddAnswerDto request,
        CancellationToken cancellationToken)
    {
        var command = new AddAnswerCommand(questionId, request);
        
        var addResult = await handler.Handle(command, cancellationToken);
        return addResult.IsFailure ? addResult.Error.ToFailureResponse() : Ok(addResult.Value);
    }
}