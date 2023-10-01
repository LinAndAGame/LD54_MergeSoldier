using MyGameExpand;
using UnityEngine;

namespace MyGameUtility {
    public static class UIUtility {
        public static Rect GetRectFromChildren(RectTransform parent) {
            // 获得最大的包围盒
            float xMin = float.MaxValue;
            float xMax = float.MinValue;
            float yMin = float.MaxValue;
            float yMax = float.MinValue;

            foreach (RectTransform child in parent.GetComponentsInChildren<RectTransform>()) {
                if (child == parent.GetComponent<RectTransform>()) {
                    continue;
                }

                float childPosXMin = child.position.x - child.rect.width * (child.pivot.x);
                float childPosXMax = child.position.x + child.rect.width * (1 - child.pivot.x);
                float childPosYMin = child.position.y - child.rect.height * (child.pivot.y);
                float childPosYMax = child.position.y + child.rect.height * (1 - child.pivot.y);

                if (childPosXMin < xMin) {
                    xMin = childPosXMin;
                }

                if (childPosXMax > xMax) {
                    xMax = childPosXMax;
                }

                if (childPosYMin < yMin) {
                    yMin = childPosYMin;
                }

                if (childPosYMax > yMax) {
                    yMax = childPosYMax;
                }
            }

            return new Rect(new Vector2(xMin, yMin), new Vector2(xMax - xMin, yMax - yMin));
        }

        public static void Move_2DFollowMouse(RectTransform target, Camera renderCamera,Vector3 offset) {
            Canvas canvas = target.GetComponent<Canvas>();
            if (canvas == null) {
                canvas = target.FindCanvas();
            }

            if (target.FindCanvas().renderMode == RenderMode.ScreenSpaceOverlay) {
                target.position = Input.mousePosition + offset;
            }
            else {
                Vector3 dist     = renderCamera.WorldToScreenPoint(target.position);
                Vector3 curPos   = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist.z);
                Vector3 worldPos = renderCamera.ScreenToWorldPoint(curPos);
                target.position = worldPos + offset;
            }
        }

        public static void Move_3DFollow2D(Transform target3D, RectTransform target2D, Camera renderCamera, Vector3 offset) {
            Canvas canvas = target2D.GetComponent<Canvas>();
            if (canvas == null) {
                canvas = target2D.FindCanvas();
            }

            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) {
                float   z        = target3D.position.z - renderCamera.transform.position.z;
                Vector3 mousePos = new Vector3(target2D.position.x, target2D.position.y, z);
                target3D.position = renderCamera.ScreenToWorldPoint(mousePos) + offset;
            }
            else {
                target3D.position = target2D.position.SetZ(target3D.position.z);
            }
        }

        public static void Move_3DFollowMouse(Transform target3D, Camera renderCamera, Vector3 offset) {
            float   z        = target3D.position.z - renderCamera.transform.position.z;
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, z);
            target3D.position = renderCamera.ScreenToWorldPoint(mousePos) + offset;
        }

        public static void Move_2DFollow3D(RectTransform target2D, Transform target3D, Camera rendererCamera2D, Camera rendererCamera3D, Vector3 offset) {
            Canvas canvas = target2D.GetComponent<Canvas>();
            if (canvas == null) {
                canvas = target2D.FindCanvas();
            }

            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) {
                Vector3 screenPos = rendererCamera3D.WorldToScreenPoint(target3D.position);
                target2D.position = screenPos + offset;
            }
            else {
                Vector3 screenPos = rendererCamera3D.WorldToScreenPoint(target3D.position);
                target2D.position = rendererCamera2D.ScreenToWorldPoint(screenPos) + offset;
            }
        }

        public static void UIMustInScreen(RectTransform target) {
            // UI必须在屏幕中，不能超出屏幕
            float halfWidth    = target.rect.size.x;
            float halfHeight   = target.rect.size.y;
            float screenWidth  = Screen.currentResolution.width;
            float screenHeight = Screen.currentResolution.height;

            float minX = halfWidth * target.pivot.x;
            float maxX = screenWidth - halfWidth * (1 - target.pivot.x);
            float minY = halfHeight * target.pivot.y;
            float maxY = screenHeight - halfHeight * (1 - target.pivot.y);

            target.position = new Vector3(Mathf.Clamp(target.position.x, minX, maxX), Mathf.Clamp(target.position.y, minY, maxY), target.position.z);
        }
    }
}