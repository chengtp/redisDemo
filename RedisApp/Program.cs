using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //连接服务器，6379 是redis默认的端口
            var client = new RedisClient("127.0.0.1", 6379);
            //client.Password = ""; //设置密码，没有可以注释掉


            #region 字符串类型

            //赋值
            client.Set<string>("name", "zhangsan");
            //取值
            string userName = client.Get<string>("name");
            //打印 值
            Console.WriteLine(userName);
            Console.ReadKey();

            #endregion

            #region 哈希

            client.SetEntryInHash("userinfoId", "name", "张三");
            client.SetEntryInHash("userinfoId", "password", "123456");
            List<string> keys = client.GetHashKeys("userinfoId");
            List<string> values = client.GetHashValues("userinfoId");

            foreach (var item in keys)
            {
                Console.WriteLine(item);
            }

            foreach (var item in values)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
            Console.ReadKey();

            #endregion


            #region 队列

            //入队
            client.EnqueueItemOnList("name2", "zhangsan");
            client.EnqueueItemOnList("name2","lisi");
            client.EnqueueItemOnList("name2","wangwu");

            int length = (int)client.GetListCount("name2");
            for (int i = 0; i < length; i++)
            {
                //出队
                Console.WriteLine(client.DequeueItemFromList("name2"));
            }
            Console.WriteLine();
            Console.ReadKey();


            //入栈
            client.PushItemToList("name4","zhangsan1");
            client.PushItemToList("name4", "lisi1");
            client.PushItemToList("name4", "wangwu1");

            int lengthPush = (int)client.GetListCount("name4");
            for (int i = 0; i < lengthPush; i++)
            {
                //出栈
                Console.WriteLine(client.PopItemFromList("name4"));
            }
            Console.WriteLine();
            Console.ReadKey();
            #endregion


        }
    }
}
