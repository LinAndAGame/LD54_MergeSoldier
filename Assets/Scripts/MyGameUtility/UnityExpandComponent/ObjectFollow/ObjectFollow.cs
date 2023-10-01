using System;
using UnityEngine;

namespace MyGameUtility {
    public class ObjectFollow : MonoBehaviour {
        public Camera           Camera2D;
        public Camera           Camera3D;
        public RectTransform    Trans2D;
        public Transform        Trans3D;
        public Vector3          Offset;
        public FollowUpdateEnum FollowUpdateType = FollowUpdateEnum.Update;
        public FollowEnum       FollowType       = FollowEnum.Follow2DWith3D;

        private void Update() {
            if (FollowUpdateType == FollowUpdateEnum.Update) {
                Follow();
            }
        }

        private void LateUpdate() {
            if (FollowUpdateType == FollowUpdateEnum.LateUpdate) {
                Follow();
            }
        }

        private void FixedUpdate() {
            if (FollowUpdateType == FollowUpdateEnum.FixedUpdate) {
                Follow();
            }
        }

        private void Follow() {
            switch (FollowType) {
                case FollowEnum.Follow2DWith3D:
                    UIUtility.Move_2DFollow3D(Trans2D, Trans3D, Camera2D, Camera3D,Offset);
                    break;
                case FollowEnum.Follow2DWithMouse:
                    UIUtility.Move_2DFollowMouse(Trans2D, Camera2D, Offset);
                    break;
                case FollowEnum.Follow3DWith2D:
                    UIUtility.Move_3DFollow2D(Trans3D, Trans2D, Camera3D, Offset);
                    break;
                case FollowEnum.Follow3DWithMouse:
                    UIUtility.Move_3DFollowMouse(Trans3D, Camera3D, Offset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum FollowUpdateEnum {
            Update,
            FixedUpdate,
            LateUpdate,
            Manual,
        }
        
        public enum FollowEnum {
            Follow2DWith3D,
            Follow2DWithMouse,
            Follow3DWith2D,
            Follow3DWithMouse,
        }
    }
}