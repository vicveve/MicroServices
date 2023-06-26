using Aforo255.Cross.Cache.Src;
using Carter;
using MS.AFORO255.History.Features.DTOs;
using MS.AFORO255.History.Features.Services;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace MS.AFORO255.History.Features;

public class HistoryModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/history/{accountId}", GetAccount)
           .Produces<HistoryResponse>()
           .Produces(StatusCodes.Status404NotFound);

        app.MapGet("ping", Ping);
    }

    private static async Task<IResult> GetAccount(int accountId, IHistoryService service, IExtensionCache extensionCache)
    {
        //var movements = await service.GetById(accountId);
        //if (movements is null)  return Results.NotFound();
        //return Results.Ok(movements);

        string keyHistory = $"keyHistory-{accountId}";
        IEnumerable<HistoryResponse> model = extensionCache.GetData<IEnumerable<HistoryResponse>>(keyHistory);
        if (model == null)
        {
            model = await service.GetById(accountId);
            extensionCache.SetData(model, keyHistory, 1);
        }
        return Results.Ok(model);
    }

    private static IResult Ping()
    {
        return Results.Ok();
    }
}

 
 

