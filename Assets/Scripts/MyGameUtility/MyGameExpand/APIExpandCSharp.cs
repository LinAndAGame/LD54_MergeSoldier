using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MyGameExpand {
    public static class APIExpandCSharp {
#region List
        
        /// <summary>
        /// 克隆一个新的列表
        /// </summary>
        /// <param name="original"></param>
        /// <typeparam name="T"></typeparam>
        public static List<T> Clone<T>(this List<T> original) {
            return new List<T>(original);
        }
        
        /// <summary>
        /// 对List排序
        /// </summary>
        /// <param name="original"></param>
        /// <typeparam name="T"></typeparam>
        public static List<T> SortList<T>(this List<T> original, Comparison<T> comparison) {
            original.Sort(comparison);
            return original;
        }

        /// <summary>
        /// 对List类拓展乱序
        /// </summary>
        /// <param name="original"></param>
        /// <typeparam name="T"></typeparam>
        public static List<T> RandomList<T>(this List<T> original) {
            // 产生随机数的时候同时生成随机种子
            System.Random random = new System.Random();
            List<T>       temp   = new List<T>();
            foreach (T item in original) {
                temp.Insert(random.Next(temp.Count + 1), item);
            }

            original.Clear();
            original.AddRange(temp);

            return original;
        }
        
        /// <summary>
        /// 获取一个列表中的随机元素
        /// </summary>
        /// <param name="original"></param>
        /// <typeparam name="T"></typeparam>
        public static T GetRandomElement<T>(this IList<T> original)
        {
            if (original.IsNullOrEmpty())
            {
                return default;
            }

            // 产生随机数的时候同时生成随机种子
            System.Random random      = new System.Random();
            int           randomValue = random.Next(original.Count);
            return original[randomValue];
        }
        
        /// <summary>
        /// 获取一个列表中的随机元素
        /// </summary>
        /// <param name="original"></param>
        /// <typeparam name="T"></typeparam>
        public static List<T> GetRandomList<T>(this IList<T> original, int count)
        {
            if (original.IsNullOrEmpty())
            {
                return default;
            }

            List<T> result = new List<T>(original);
            result.RandomList();
            return result.GetRange(0, Mathf.Min(result.Count, count));
        }


        /// <summary>
        /// 对List类进行拓展，判断List是否为空或数量为0
        /// </summary>
        /// <param name="original"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IList<T> original) {
            return original == null || original.Count == 0;
        }
        
        /// <summary>
        /// 对List类进行拓展，添加时排除Null数据
        /// </summary>
        /// <param name="original"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void AddExceptNull<T>(this IList<T> original, T target) {
            if (original == null) {
                throw new ArgumentNullException();
            }
            
            if (target == null) {
                return;
            }

            original.Add(target);
        }
        
        /// <summary>
        /// 对List类拓展，唯一性添加，添加前会检查数组中是否已经含有了指定的元素
        /// </summary>
        /// <param name="original"></param>
        /// <param name="target"></param>
        /// <typeparam name="T"></typeparam>
        public static void OnlyAdd<T>(this List<T> original, T target, Func<T, bool> additionalAddFunc = null)
        {
            if (original == null) {
                throw new ArgumentNullException();
            }
            
            // IList 拓展方法，属于IList的方法，直接调用即可，确保唯一性
            if (original.Contains(target) == false)
            {
                if (additionalAddFunc != null) {
                    if (additionalAddFunc.Invoke(target)) {
                        original.Add(target);
                    }
                }
                else {
                    original.Add(target);
                }
            }
        }
        /// <summary>
        /// 对List类拓展，唯一性添加，添加前会检查数组中是否已经含有了指定的元素
        /// </summary>
        /// <param name="original"></param>
        /// <param name="target"></param>
        /// <typeparam name="T"></typeparam>
        public static void OnlyAddRange<T>(this List<T> original, List<T> targets, Func<T, bool> additionalAddFunc = null)
        {
            if (original == null) {
                throw new ArgumentNullException();
            }

            if (targets.IsNullOrEmpty()) {
                return;
            }
            
            foreach (var target in targets) {
                original.OnlyAdd(target, additionalAddFunc);
            }
        }

        /// <summary>
        /// 对List类拓展，唯一性添加，添加前会检查数组中是否已经含有了指定的元素,添加时排除空
        /// </summary>
        /// <param name="original"></param>
        /// <param name="target"></param>
        /// <typeparam name="T"></typeparam>
        public static void OnlyAddExceptNull<T>(this List<T> original, T target, Func<T, bool> checkNullFunc = null)
        {
            original.OnlyAdd(target, checkNullFunc ?? (arg => arg != null));
        }

        /// <summary>
        /// 对List类进行拓展，添加时排除Null数据
        /// </summary>
        /// <param name="original"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static void CleanNull<T>(this IList<T> original) {
            if (original.IsNullOrEmpty()) {
                return;
            }

            for (int i = original.Count - 1; i >= 0; i--) {
                if (original[i] == null) {
                    original.RemoveAt(i);
                }
            }
        }

#endregion

#region String

        

        /// <summary>
        /// 对字符串进行缩进
        /// </summary>
        /// <param name="original"></param>
        /// <param name="count">缩进数量</param>
        /// <returns></returns>
        public static string Indent(this string original, int count)
        {
            string        indent = "\u3000";
            StringBuilder sb     = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append(indent);
            }

            sb.Append(original);
            return sb.ToString();
        }

#endregion

#region Array

        public static List<T> ToList<T>(this T[,] original) {
            if (original == null) {
                throw new ArgumentNullException();
            }

            List<T> result = new List<T>();
            foreach (var data in original) {
                result.Add(data);
            }

            return result;
        }

#endregion
    }
}