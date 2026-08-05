public class BlockIpMiddleware : IMiddleware
{
    static List<string> blockedIP = new List<string>();
    static Dictionary<string, int> requestCount = new Dictionary<string, int>();
    public BlockIpMiddleware()
    {
        //Contruction
    }

    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString();

        if(blockedIP.Contains(clientIp))
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return context.Response.WriteAsync("Access denied");
        }

        if(requestCount.ContainsKey(clientIp))
        {
            requestCount[clientIp]++;
        }
        else
        {
            requestCount[clientIp] = 1;
        }

        if(requestCount[clientIp] > 10000000)
        {
            blockedIP.Add(clientIp);
            requestCount.Remove(clientIp);
        }
        return next(context);
    }
    
}