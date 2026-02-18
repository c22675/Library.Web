using System.Net.Http.Json;

namespace Library.Web.Services;

public class MlServiceClient
{
    private readonly HttpClient _http;

    public MlServiceClient(IHttpClientFactory factory)
    {
        _http = factory.CreateClient("MlService");
    }

    public record BookPriceRequest(string Title, string BookCategory, int StarRating, int Quantity);
    public record BookPriceResponse(float PredictedPrice);

    public async Task<BookPriceResponse?> PredictBookPriceAsync(BookPriceRequest req, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync("/api/bookprice/predict", req, ct);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<BookPriceResponse>(cancellationToken: ct);
    }
}