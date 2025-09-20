using Microsoft.AspNetCore.Routing;

namespace Framework;

public interface IEndPoint
{
    void MapEndPoint(IEndpointRouteBuilder app);
}