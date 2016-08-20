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
    public class ListController : Controller
    {
        #region 初始化 数据 Index

        public ActionResult Index()
        {  //删除所有key=userInfo1 的所有数据
            RedisBase.List_RemoveAll<Models.UserInfo>("userInfo1");

            //创建假的 实体数据
            Models.UserInfo zhangsan = new Models.UserInfo() { Id = RedisBase.GetNextSequence<Models.UserInfo>("userInfo1"), Name = "张三1", Desc = "喜欢白开水" };
            Models.UserInfo lisi = new Models.UserInfo() { Id = RedisBase.GetNextSequence<Models.UserInfo>("userInfo1"), Name = "李四1", Desc = "名字李四" };
            Models.UserInfo wangwu = new Models.UserInfo() { Id = RedisBase.GetNextSequence<Models.UserInfo>("userInfo1"), Name = "王五1", Desc = "喜欢水果" };
            Models.UserInfo zhaoliu = new Models.UserInfo() { Id = RedisBase.GetNextSequence<Models.UserInfo>("userInfo1"), Name = "赵六1", Desc = "喜欢香蕉" };
            Models.UserInfo zhangliu = new Models.UserInfo() { Id = RedisBase.GetNextSequence<Models.UserInfo>("userInfo1"), Name = "张六1", Desc = "喜欢香蕉1" };

            //添加到redis中
            RedisBase.List_Add("userInfo1", zhangsan);
            RedisBase.List_Add("userInfo1", lisi);
            RedisBase.List_Add("userInfo1", wangwu);
            RedisBase.List_Add("userInfo1", zhaoliu);
            RedisBase.List_Add("userInfo1", zhangliu);

            //从redis中查询userInfo1的所有数据
            List<Models.UserInfo> models = RedisBase.List_GetList<Models.UserInfo>("userInfo1");
            //得到集合
            ViewBag.UserInfoList = models;

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
            //得到所有的数据
            List<Models.UserInfo> modelsAll = RedisBase.List_GetList<Models.UserInfo>("userInfo1");
            //通过条件得到部分数据
            List<Models.UserInfo> models = modelsAll.Where(m => m.Id.ToString().Contains(Name) || m.Name.Contains(Name) || m.Desc.Contains(Name)).ToList();
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
            //实体对象
            Models.UserInfo model = new Models.UserInfo()
            {
                Id = RedisBase.GetNextSequence<Models.UserInfo>("userInfo1"),
                Name = userinfo.Name,
                Desc = userinfo.Desc
            };

            //添加一条记录到集合中
            RedisBase.List_Add<Models.UserInfo>("userInfo1", model);
            //获取列表 集合数据
            List<Models.UserInfo> models = RedisBase.List_GetList<Models.UserInfo>("userInfo1");
            //转化成json数据传递给前端
            return Json(new { UserModels = models });
        }

        #endregion

        #region 编辑修改操作

        public ActionResult Edit(long Id)
        {
            //获取列表 集合数据
            List<Models.UserInfo> models = RedisBase.List_GetList<Models.UserInfo>("userInfo1");
            //从集合中取出单条数据
            Models.UserInfo model = models.FirstOrDefault(m => m.Id == Id);
            //转化成json数据传递给前端
            return Json(new { UserModel = model }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 修改保存事件

        [HttpPost]
        public ActionResult Edit(Models.UserInfo userinfo)
        {
            //获取列表 集合数据
            List<Models.UserInfo> models = RedisBase.List_GetList<Models.UserInfo>("userInfo1");
            //从集合中取出单条数据
            Models.UserInfo model = models.FirstOrDefault(m => m.Id == userinfo.Id);
            //赋值操作
            model.Name = userinfo.Name;
            model.Desc = userinfo.Desc;

            //先删除
            RedisBase.List_Remove<Models.UserInfo>("userInfo1", model);
            //添加数据
            RedisBase.List_Add("userInfo1", model);

            //获取列表 集合数据
            models = RedisBase.List_GetList<Models.UserInfo>("userInfo1");
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
            //获取列表 集合数据
            List<Models.UserInfo> modelsAll = RedisBase.List_GetList<Models.UserInfo>("userInfo1");
            //从集合中取出单条数据
            Models.UserInfo model = modelsAll.FirstOrDefault(m => m.Id == Id);
            //移除 数据
            RedisBase.List_Remove<Models.UserInfo>("userInfo1", model);
            //获取列表 集合数据
            List<Models.UserInfo> models = RedisBase.List_GetList<Models.UserInfo>("userInfo1");
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

        private void test()
        {
            string ss = "fsf";
            string a = "12";
            string b = "23";
            string c = "45";
             c = "45";
             c = "45"; 
            c = "45";
        }
    }
}
