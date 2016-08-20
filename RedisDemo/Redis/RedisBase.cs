using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedisDemo.Redis
{
    public class RedisBase
    {

        #region base

        /// <summary>
        /// 等到唯一标识的序号
        /// </summary>
        /// <typeparam name="T">实体对象名称</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static long GetNextSequence<T>(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var user = redis.As<T>();
                return user.GetNextSequence();
            }
        }

        #endregion

        #region -- Item --
        /// <summary>
        /// 设置单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static bool Item_Set<T>(string key, T t)
        {
            bool flag = true;
            try
            {
                using (IRedisClient redis = RedisManager.GetClient())
                {
                    return redis.Set<T>(key, t);
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 获取单体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Item_Get<T>(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return redis.Get<T>(key);
            }
        }

        /// <summary>
        /// 移除单体
        /// </summary>
        /// <param name="key"></param>
        public static bool Item_Remove(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return redis.Remove(key);
            }
        }

        #endregion

        #region -- List --
        /// <summary>
        /// 列表 添加
        /// </summary>
        /// <typeparam name="T">对象实体名称</typeparam>
        /// <param name="key">键</param>
        /// <param name="t">对象实体数据</param>
        public static void List_Add<T>(string key, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                redisTypedClient.AddItemToList(redisTypedClient.Lists[key], t);
            }
        }

        /// <summary>
        /// 列表 删除单条数据
        /// </summary>
        /// <typeparam name="T">实体对象名称</typeparam>
        /// <param name="key">键</param>
        /// <param name="t">对象实体数据</param>
        /// <returns></returns>
        public static bool List_Remove<T>(string key, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.RemoveItemFromList(redisTypedClient.Lists[key], t) > 0;
            }
        }

        /// <summary>
        ///列表 删除所有数据 
        /// </summary>
        /// <typeparam name="T">实体对象名称</typeparam>
        /// <param name="key">键</param>
        public static void List_RemoveAll<T>(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                redisTypedClient.Lists[key].RemoveAll();
            }
        }

        /// <summary>
        /// 列表 查出key的总数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int List_Count(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return (int)redis.GetListCount(key);
            }
        }


        public static List<T> List_GetRange<T>(string key, int start, int count)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var c = redis.As<T>();
                return c.Lists[key].GetRange(start, start + count - 1);
            }
        }

        /// <summary>
        /// 列表 查询列表集合
        /// </summary>
        /// <typeparam name="T">实体对象名称</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static List<T> List_GetList<T>(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var c = redis.As<T>();
                return c.Lists[key].GetRange(0, c.Lists[key].Count);
            }
        }

        public static List<T> List_GetList<T>(string key, int pageIndex, int pageSize)
        {
            int start = pageSize * (pageIndex - 1);
            return List_GetRange<T>(key, start, pageSize);
        }

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void List_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }

        public static void List_test<T>(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var c = redis.As<T>();
               
            }
        }

        #endregion

        #region -- Set --
        /// <summary>
        /// 集合  添加数据
        /// </summary>
        /// <typeparam name="T">实体对象名称</typeparam>
        /// <param name="key">键</param>
        /// <param name="t">实体对象数据</param>
        public static void Set_Add<T>(string key, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                redisTypedClient.Sets[key].Add(t);
            }
        }

        /// <summary>
        /// 集合  是否包含该数据
        /// </summary>
        /// <typeparam name="T">实体对象名称</typeparam>
        /// <param name="key">键</param>
        /// <param name="t">实体对象数据</param>
        /// <returns></returns>
        public static bool Set_Contains<T>(string key, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.Sets[key].Contains(t);
            }
        }

        /// <summary>
        /// 集合  移除该条数据
        /// </summary>
        /// <typeparam name="T">实体对象名称</typeparam>
        /// <param name="key">键</param>
        /// <param name="t">实体对象数据</param>
        /// <returns></returns>
        public static bool Set_Remove<T>(string key, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.Sets[key].Remove(t);
            }
        }
        #endregion

        #region -- Hash --
        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool Hash_Exist<T>(string key, string dataKey)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return redis.HashContainsEntry(key, dataKey);
            }
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool Hash_Set<T>(string key, string dataKey, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.SetEntryInHash(key, dataKey, value);
            }
        }
        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool Hash_Remove(string key, string dataKey)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return redis.RemoveEntryFromHash(key, dataKey);
            }
        }
        /// <summary>
        /// 移除整个hash
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool Hash_Remove(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return redis.Remove(key);
            }
        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static T Hash_Get<T>(string key, string dataKey)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                string value = redis.GetValueFromHash(key, dataKey);
                return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
            }
        }
        /// <summary>
        /// 获取整个hash的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> Hash_GetAll<T>(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var list = redis.GetHashValues(key);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(value);
                    }
                    return result;
                }
                return null;
            }
        }
        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void Hash_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }
        #endregion

        #region -- SortedSet --
        /// <summary>
        ///  添加数据到 SortedSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="score"></param>
        public static bool SortedSet_Add<T>(string key, T t, double score)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.AddItemToSortedSet(key, value, score);
            }
        }
        /// <summary>
        /// 移除数据从SortedSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool SortedSet_Remove<T>(string key, T t)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
                return redis.RemoveItemFromSortedSet(key, value);
            }
        }
        /// <summary>
        /// 修剪SortedSet
        /// </summary>
        /// <param name="key"></param>
        /// <param name="size">保留的条数</param>
        /// <returns></returns>
        public static int SortedSet_Trim(string key, int size)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return (int)redis.RemoveRangeFromSortedSet(key, size, 9999999);
            }
        }
        /// <summary>
        /// 获取SortedSet的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int SortedSet_Count(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                return (int)redis.GetSortedSetCount(key);
            }
        }

        /// <summary>
        /// 获取SortedSet的分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> SortedSet_GetList<T>(string key, int pageIndex, int pageSize)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var list = redis.GetRangeFromSortedSet(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(data);
                    }
                    return result;
                }
            }
            return null;
        }


        /// <summary>
        /// 获取SortedSet的全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static List<T> SortedSet_GetListALL<T>(string key)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                var list = redis.GetRangeFromSortedSet(key, 0, 9999999);
                if (list != null && list.Count > 0)
                {
                    List<T> result = new List<T>();
                    foreach (var item in list)
                    {
                        var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
                        result.Add(data);
                    }
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        public static void SortedSet_SetExpire(string key, DateTime datetime)
        {
            using (IRedisClient redis = RedisManager.GetClient())
            {
                redis.ExpireEntryAt(key, datetime);
            }
        }

        //public static double SortedSet_GetItemScore<T>(string key,T t)
        //{
        //    using (IRedisClient redis = RedisManager.GetClient())
        //    {
        //        var data = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
        //        return redis.GetItemScoreInSortedSet(key, data);
        //    }
        //    return 0;
        //}

        #endregion

    }
}