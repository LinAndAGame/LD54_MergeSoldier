using System;
using UnityEngine;
using UnityEngine.AI;

namespace MyGameExpand {
    public static class APIExpandUnity {
#region RectTransform

        /// <summary>
        /// 寻找父对象中最远的Canvas
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Canvas FindCanvas(this RectTransform original) {
            Transform parentTrans = original.parent;
            Canvas    result      = null;

            while (parentTrans != null) {
                Canvas canvasSelf = parentTrans.GetComponent<Canvas>();
                if (canvasSelf != null) {
                    result = canvasSelf;
                }

                parentTrans = parentTrans.parent;
            }

            return result;
        }

#endregion

#region Vector3

        /// <summary>
        /// 使UI根据父对象自动填充
        /// </summary>
        /// <param name="original"></param>
        public static bool Approximately(this Vector3 original, Vector3 target, float errorRange = 0.1f) {
            return Mathf.Abs(original.x - target.x) <= errorRange &&
                   Mathf.Abs(original.y - target.y) <= errorRange &&
                   Mathf.Abs(original.z - target.z) <= errorRange;
        }

        /// <summary>
        /// 单独设置X
        /// </summary>
        /// <param name="original"></param>
        public static Vector3 SetX(this Vector3 original, float newX) {
            return new Vector3(newX, original.y, original.z);
        }

        /// <summary>
        /// 单独设置Y
        /// </summary>
        /// <param name="original"></param>
        public static Vector3 SetY(this Vector3 original, float newY) {
            return new Vector3(original.x, newY, original.z);
        }

        /// <summary>
        /// 单独设置Z
        /// </summary>
        /// <param name="original"></param>
        public static Vector3 SetZ(this Vector3 original, float newZ) {
            return new Vector3(original.x, original.y, newZ);
        }

#endregion

#region Transform
        
        /// <summary>
        /// 对Transform类拓展，立即销毁所有子物体
        /// </summary>
        /// <param name="original"></param>
        public static void DestroyAllChildrenImmediate(this Transform original)
        {
            if (original == null)
            {
                Debug.LogErrorFormat("父对象已经为空，不能销毁其子物体！");
                return;
            }

            for (int i = original.childCount - 1; i >= 0; i--)
            {
                if (original.GetChild(i) == null || original.GetChild(i).gameObject == null)
                {
                    Debug.LogErrorFormat("父对象【{0}】的第【{1}】个子物体已经为空，不能进行销毁！", original.name, i);
                    continue;
                }

                UnityEngine.Object.DestroyImmediate(original.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 对Transform类拓展，正常销毁所有子物体
        /// </summary>
        /// <param name="original"></param>
        public static void DestroyAllChildren(this Transform original)
        {
            if (original == null)
            {
                Debug.LogError("传入的值为空，不能销毁所有子物体！");
                return;
            }

            for (int i = original.childCount - 1; i >= 0; i--)
            {
                original.GetChild(i).gameObject.SetActive(false);
                UnityEngine.Object.Destroy(original.GetChild(i).gameObject);
            }
        }
        
        /// <summary>
        /// 重置LocalTransform
        /// </summary>
        /// <param name="original"></param>
        public static void ResetLocalTrans(this Transform original)
        {
            original.localRotation = Quaternion.identity;
            original.localPosition = Vector3.zero;
            original.localScale    = Vector3.one;
        }

#endregion

#region Color

        /// <summary>
        /// 设置颜色的R值
        /// </summary>
        public static Color SetR(this Color original, float r)
        {
            r = Mathf.Clamp01(r);
            Color result = new Color(r, original.g, original.b, original.a);
            return result;
        }

        /// <summary>
        /// 设置颜色的R值
        /// </summary>
        public static Color SetG(this Color original, float g)
        {
            g = Mathf.Clamp01(g);
            Color result = new Color(original.r, g, original.b, original.a);
            return result;
        }

        /// <summary>
        /// 设置颜色的R值
        /// </summary>
        public static Color SetB(this Color original, float b)
        {
            b = Mathf.Clamp01(b);
            Color result = new Color(original.r, original.g, b, original.a);
            return result;
        }

        /// <summary>
        /// 设置颜色的R值
        /// </summary>
        public static Color SetA(this Color original, float a)
        {
            a = Mathf.Clamp01(a);
            Color result = new Color(original.r, original.g, original.b, a);
            return result;
        }

#endregion

#region NavMesh
        
        /// <summary>
        /// 在NavMesh地图上寻找随机点
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Vector3 GetNavMeshRandomPos(this GameObject obj)
        {
            NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

            int t = new System.Random().Next(0, navMeshData.indices.Length - 3);

            // 利用插值取三个点构成的三角形内的随机点
            Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]],
                navMeshData.vertices[navMeshData.indices[t + 1]], UnityEngine.Random.value);
            point = Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], UnityEngine.Random.value);

            return point;
        }
#endregion

#region GameObject

        /// <summary>
        /// 设置自身和所有子物体的Layer
        /// </summary>
        /// <param name="original"></param>
        /// <param name="layer"></param>
        public static void SetAllChildrenLayer(this GameObject original, int layer)
        {
            foreach (var o in original.GetComponentsInChildren<Transform>())
            {
                o.gameObject.layer = layer;
            }
        }

        public static void DestroyImmediateAllChildren(this Component original) {
            if (original == null) {
                throw new ArgumentNullException();
            }

            int childCount = original.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--) {
                UnityEngine.Object.DestroyImmediate(original.transform.GetChild(i).gameObject);
            }
        }


#endregion
    }
}