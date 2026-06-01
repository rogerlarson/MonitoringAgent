public sealed class GatewayMetricsResponse
{
    public bool Reachable
    { get; set; }

    public int HttpStatusCode
    { get; set; }

    public long ResponseMs
    { get; set; }
}