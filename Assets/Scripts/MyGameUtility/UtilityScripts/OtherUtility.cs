using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace MyGameUtility {
    public static class OtherUtility {
        public static string GetScenePath(GameObject obj) {
            if (obj == null || obj.transform == null) {
                Debug.LogWarning("输入的对象为空，不能找到路径！");
                return string.Empty;
            }

            List<Transform> parents = new List<Transform>();
            parents.Add(obj.transform);
            Transform targetObj = obj.transform;

            while (targetObj.parent != null) {
                parents.Insert(0, targetObj.parent);
                targetObj = targetObj.parent;
            }

            return StringUtility.Connect("/", parents.Select(data => data.name).ToArray());
        }

        public static void RotateTo2D(Transform target, Vector2 dirTo) {
            Vector3 axis = Vector3.forward;
            target.rotation = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.right, dirTo, axis), axis);
        }

        public static float OutputTimeCost(System.Action act,string otherInfo = null) {
            Stopwatch sw = Stopwatch.StartNew();
            act?.Invoke();
            sw.Stop();
            Debug.Log($"{otherInfo}耗时【{sw.Elapsed.TotalMilliseconds}】");

            return (float)sw.Elapsed.TotalMilliseconds;
        }
    }
}