using log4net;
using RedisDemo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RedisDemo
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure(); //获取Log4Net配置信息(配置信息定义在Web.config文件中)
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters); //错误信息类 添加
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //开启一个线程，然后不停的从队列中添加数据
            string filePath = Server.MapPath("/Log/");
            ThreadPool.QueueUserWorkItem(m =>
            {
                while (true)
                {
                    try
                    {
                        if (ExecptionAttribute.redisClient.GetListCount("errorExecption") > 0)
                        {
                            //从Redis队列中取出异常数据
                            string errorMsg = ExecptionAttribute.redisClient.DequeueItemFromList("errorExecption");
                            if (!string.IsNullOrEmpty(errorMsg))
                            {
                                ILog logger = LogManager.GetLogger("error");
                                //将异常信息写入到Log4Net中
                                logger.Error(errorMsg);
                            }
                            else
                            {
                                Thread.Sleep(30);
                            }
                        }
                        else
                        {
                            Thread.Sleep(30); //避免CPU空转
                        }
                    }
                    catch (Exception ex)
                    {
                        //将异常信息写入到队列中
                        ExecptionAttribute.redisClient.EnqueueItemOnList("errorExecption", ex.ToString());
                    }
                }
            }, filePath);
        }
    }
}