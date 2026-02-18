using Library.Grpc;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers;

public class GrpcController : Controller
{
    private readonly StatsService.StatsServiceClient _client;

    public GrpcController(StatsService.StatsServiceClient client)
    {
        _client = client;
    }

    public async Task<IActionResult> Dashboard()
    {
        var reply = await _client.GetDashboardStatsAsync(new StatsRequest());
        return View(reply);
    }
}