﻿using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedisDemo.Redis
{
    public class RedisManager
    {
        /// <summary>
        /// redis配置文件信息
        /// </summary>
        private static RedisConfigInfo redisConfigInfo = RedisConfigInfo.GetConfig();

        private static PooledRedisClientManager prcm;

        /// <summary>
        /// 静态构造方法，初始化连接池管理对象
        /// </summary>
        static RedisManager()
        {
            CreateManager();
        }

        /// <summary>
        /// 创建连接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            string[] writeServerList = SplitString(redisConfigInfo.WriteServerList, ",");
            string[] readServerList = SplitString(redisConfigInfo.ReadServerList, ",");

            prcm = new PooledRedisClientManager(readServerList, writeServerList,
                new RedisClientManagerConfig()
                {
                    MaxWritePoolSize = redisConfigInfo.MaxWritePoolSize,
                    MaxReadPoolSize = redisConfigInfo.MaxReadPoolSize,
                    AutoStart = redisConfigInfo.AutoStart,
                });

        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        /// <returns></returns>
        public static IRedisClient GetClient()
        {
            if (prcm == null)
            {
                CreateManager();
            }

            return prcm.GetClient();

        }
    }
}