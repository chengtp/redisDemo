using RedisDemo.Common;
using System.Web;
using System.Web.Mvc;

namespace RedisDemo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new ExecptionAttribute());
        }
    }
}