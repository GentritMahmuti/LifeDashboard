using System.Linq.Expressions;

namespace lifeEcommerce.Helpers;
public static class HelperMethods
{
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool searchCondition, Expression<Func<T, bool>> predicate)
    {
        return searchCondition ? query.Where(predicate) : query;
    }

    public static IQueryable<T> PageBy<T,Type>(this IQueryable<T> query, Expression<Func<T, Type>> orderBy, int page, int pageSize, bool orderByDescending = true) //add Tkey into PageBy if needed
    {
        const int defaultPageNumber = 1;

        if (query == null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        // Check if the page number is greater then zero - otherwise use default page number
        if (page <= 0)
        {
            page = defaultPageNumber;
        }

        // It is necessary sort items before it
        query = orderByDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public static double GetPriceByQuantity(int quantity, double price, double price50, double price100)
    {
        if(quantity <= 50) return price;
        else
        {
            if (quantity <= 100) return price50;
            else
                return price100;
        }
    }

    public static string GetIpAddress(this IHttpContextAccessor accessor)
    {

        // CF-Connecting-IP header provides the client IP address, connecting to Cloudflare
        if (!string.IsNullOrEmpty(accessor.HttpContext.Request.Headers["CF-Connecting-IP"]))
            return accessor.HttpContext.Request.Headers["CF-Connecting-IP"];

        //X-Forwarded-For header field is a method for identifying the IP address of a client connecting to a web server through an HTTP proxy or load balancer.
        var ipAddress = accessor.HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR");

        if (!string.IsNullOrEmpty(ipAddress))
        {
            var addresses = ipAddress.Split(',');
            if (addresses.Length != 0)
                return addresses.Last();
        }

        // Get Remote IP Address if any of two methotds above does not work 
        return accessor.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "N/A";
    }
}
