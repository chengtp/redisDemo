using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RedisDemo.Common
{
    public class ExecptionAttribute : HandleErrorAttribute
    {
        public static IRedisClient redisClient = RedisDemo.Redis.RedisManager.GetClient();
        public override void OnException(ExceptionContext filterContext)
        {
            //将错误信息入队
            redisClient.EnqueueItemOnList("errorExecption", filterContext.Exception.ToString());
            filterContext.HttpContext.Response.Redirect("/error.html");
            base.OnException(filterContext);
        }
    }
}