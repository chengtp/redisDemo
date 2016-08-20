using RedisDemo.Redis;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Redis.Generic;

namespace RedisDemo.Controllers
{
    public class HomeController : Controller
    {

        #region 初始化 数据 Index

        public ActionResult Index()
        {
            //   var client = new RedisClient("127.0.0.1", 6379);
            // client.Set<string>("name", "张三");
            //  ViewBag.name = client.Get<string>("name");

            IRedisClient redisClient = RedisManager.GetClient();
            //转化类型
            var user = redisClient.As<Models.UserInfo>();
            if (user.GetAll().Count > 0)
            {
                user.DeleteAll();
            }
            //创建数据
            Models.UserInfo zhangsan = new Models.UserInfo() { Id = user.GetNextSequence(), Name = "张三", Desc = "喜欢白开水" };
            Models.UserInfo lisi = new Models.UserInfo() { Id = user.GetNextSequence(), Name = "李四", Desc = "名字李四" };
            Models.UserInfo wangwu = new Models.UserInfo() { Id = user.GetNextSequence(), Name = "王五", Desc = "喜欢水果" };
            Models.UserInfo zhaoliu = new Models.UserInfo() { Id = user.GetNextSequence(), Name = "赵六", Desc = "喜欢香蕉" };
            Models.UserInfo zhangliu = new Models.UserInfo() { Id = user.GetNextSequence(), Name = "张六", Desc = "喜欢香蕉1" };
            System.Collections.Generic.List<Models.UserInfo> models = new List<Models.UserInfo> { zhangsan, lisi, wangwu, zhaoliu, zhangliu };

            //填充数据到redis中
            user.StoreAll(models);
            //得到集合
            ViewBag.UserInfoList = user.GetAll();
            return View();
        }

        #endregion

        #region 通过条件查询

        /// <summary>
        /// 通过条件查询
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public ActionResult Detail(string Name)
        {
            IRedisClient redisClient = RedisManager.GetClient();
            var user = redisClient.As<Models.UserInfo>();
            //从集合中取出多条数据
            List<Models.UserInfo> models = user.GetAll().Where(m => m.Id.ToString().Contains(Name) || m.Name.Contains(Name) || m.Desc.Contains(Name)).ToList();
            //转化成json数据传递给前端
            return Json(new { UserModels = models }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 添加操作

        /// <summary>
        /// 添加操作
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Models.UserInfo userinfo)
        {
            IRedisClient redisClient = RedisManager.GetClient();
            var user = redisClient.As<Models.UserInfo>();
            Models.UserInfo model = new Models.UserInfo()
            {
                Id = user.GetNextSequence(),
                Name = userinfo.Name,
                Desc = userinfo.Desc
            };
            var userinfolist = new List<Models.UserInfo> { model };
            //添加一条记录到集合中
            user.StoreAll(userinfolist);
            //获取集合
            var models = user.GetAll();
            //转化成json数据传递给前端
            return Json(new { UserModels = models });
        }

        #endregion

        #region 编辑修改操作

        public ActionResult Edit(long Id)
        {
            IRedisClient redisClient = RedisManager.GetClient();
            var user = redisClient.As<Models.UserInfo>();
            //从集合中取出单条数据
            Models.UserInfo model = user.GetAll().FirstOrDefault(m => m.Id == Id);
            //转化成json数据传递给前端
            return Json(new { UserModel = model }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 修改保存事件

        [HttpPost]
        public ActionResult Edit(Models.UserInfo userinfo)
        {
            IRedisClient redisClient = RedisManager.GetClient();
            var user = redisClient.As<Models.UserInfo>();
            //从集合中取出单条数据
            Models.UserInfo model = user.GetAll().FirstOrDefault(m => m.Id == userinfo.Id);
            //赋值操作
            model.Name = userinfo.Name;
            model.Desc = userinfo.Desc;

            var users = new List<Models.UserInfo> { model };
            user.StoreAll(users);

            //获取集合
            var models = user.GetAll();
            //转化成json数据传递给前端
            return Json(new { UserModels = models });
        }

        #endregion

        #region 删除操作

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(long Id)
        {
            IRedisClient redisClient = RedisManager.GetClient();
            var user = redisClient.As<Models.UserInfo>();
            //删除操作
            user.DeleteById(Id);
            //获取集合
            var models = user.GetAll();
            //转化成json数据传递给前端
            return Json(new { UserModels = models });
        }

        #endregion

        #region 队列 异常日志

        public ActionResult Queue()
        {
            int a = 2;
            int b = 0;
            double c = a / b;

            return View();
        }
        #endregion

    }
}
