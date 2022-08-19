using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using DocumentDealWithCommand.Logic.Models;

namespace DocumentDealWithCommand.Logic.Implementation
{
    /// <summary>
    /// 文件信息相关扩展操作方法
    /// </summary>
    public static class ArrayDataExtend
    {
        /// <summary>
        /// 用户输入目标位置标识转为程序列表规范位置标识
        /// </summary>
        /// <param name="userInputIndex">用户输入目标位置标识</param>
        /// <param name="dataListLength">程序列表总长度</param>
        /// <returns>程序列表规范位置标识</returns>
        public static uint UserInputToProgramTargetIndex(uint userInputIndex, int dataListLength)
        {
            uint index = userInputIndex;
            index = index > 0 ? index - 1 : 0;
            index = Math.Max(0, index);
            index = Math.Min(index, (uint)dataListLength - 1);
            return index;
        }

        /// <summary>
        /// 用户输入目标位置标识队列转为程序列表规范位置标识队列
        /// </summary>
        /// <param name="userInputIndexs">用户输入目标位置标识队列</param>
        /// <param name="dataListLength">程序列表总长度</param>
        /// <returns>程序列表规范位置标识队列</returns>
        public static uint[] UserInputToProgramTargetIndexs(uint[] userInputIndexs, int dataListLength)
        {
            HashSet<uint> hash = new HashSet<uint>();
            for (int i = 0; i < userInputIndexs.Length; i++)
            {
                hash.Add(UserInputToProgramTargetIndex(userInputIndexs[i], dataListLength));
            }
            return hash.ToArray();
        }

        /// <summary>
        /// 移动数组各项的位置
        /// </summary>
        /// <param name="list">数组队列</param>
        /// <param name="targetIndex">移动到的目标位置(从0开始)</param>
        /// <param name="positionIndexs">需要移动的项位置标识(从0开始)</param>
        /// <returns></returns>
        public static IList<T> MoveArray<T>(this IList<T> list, uint targetIndex, uint[] positionIndexs)
        {
            targetIndex = Math.Min(targetIndex, (uint)list.Count - (uint)positionIndexs.Length);
            T[] rlist = new T[list.Count];
            for (int i = 0; i < positionIndexs.Length; i++)
            {
                rlist[(int)targetIndex + i] = list[(int)positionIndexs[i]];
            }
            int i_list = 0;
            int i_rlist = 0;
            int i_position = 0;
            while (i_list < list.Count && i_rlist < rlist.Length)
            {
                if (targetIndex <= i_rlist && i_rlist <= (targetIndex + positionIndexs.Length - 1))
                {
                    i_rlist++;
                    continue;
                }
                if (i_position < positionIndexs.Length && i_list == positionIndexs[i_position])
                {
                    i_list++;
                    i_position++;
                    continue;
                }
                rlist[i_rlist] = list[i_list];
                i_rlist++;
                i_list++;
            }
            return rlist;
        }

        /// <summary>
        /// 连接数组对象
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="arr">原数组</param>
        /// <param name="objs">目标数组</param>
        /// <returns>结果数组</returns>
        public static IList<T> ConcatCanNull<T>(this IList<T> arr, IList<T> objs)
        {
            return (arr ?? new List<T>()).Concat(objs ?? new List<T>()).ToArray();
        }
    }
}
