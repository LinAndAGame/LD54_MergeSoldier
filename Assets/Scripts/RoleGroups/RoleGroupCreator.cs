using System;
using System.Collections;
using System.Collections.Generic;
using Fight;
using MyGameExpand;
using MyGameUtility;
using Role;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RoleGroups {
    public class RoleGroupCreator : MonoBehaviour {
        public CustomAction OnCachedRoleGroupsChanged = new CustomAction();
        
        public float           CD             = 2;
        public List<RoleGroup> AllRolePrefabs;

        private Queue<RoleGroup> _CachedRoleGroups = new Queue<RoleGroup>();
        public  int              CurCachedRoleGroupCount => _CachedRoleGroups.Count;

        public RoleGroup CurRoleGroup { get; set; }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Mouse0) && FightCtrl.I.PlayerCtrlRef.IsTouchingCanPlacedMapLocator) {
                PlaceRoleGroup();
            }
        }

        public void Init() {
            genRoleGroup();
            TryPopAndPreviewRoleGroup();
            StartCoroutine(genRoleGroupProcess());

            IEnumerator genRoleGroupProcess() {
                while (true) {
                    genRoleGroup();
                    TryPopAndPreviewRoleGroup();
                    yield return new WaitForSeconds(CD);
                }
            }

            void genRoleGroup() {
                RoleGroup roleGroup = Instantiate(AllRolePrefabs.GetRandomElement(), this.transform);
                roleGroup.Init();
                _CachedRoleGroups.Enqueue(roleGroup);
                OnCachedRoleGroupsChanged.Invoke();
            }
        }

        public void PreviewRoleGroupToCurTouchedMapLocator() {
            if (CurRoleGroup != null) {
                CurRoleGroup.Show();
                FightCtrl.I.MapCtrlRef.PreviewPlaceRoleGroup(CurRoleGroup, FightCtrl.I.PlayerCtrlRef.LastTouchedCanPlacedMapLocator);
            }
        }

        private void TryPopAndPreviewRoleGroup() {
            if (_CachedRoleGroups.Count > 0 && CurRoleGroup == null) {
                CurRoleGroup = _CachedRoleGroups.Dequeue();
                PreviewRoleGroupToCurTouchedMapLocator();
                OnCachedRoleGroupsChanged.Invoke();
            }
        }

        private void PlaceRoleGroup() {
            if (CurRoleGroup != null) {
                FightCtrl.I.MapCtrlRef.PlaceRoleGroup(CurRoleGroup, FightCtrl.I.PlayerCtrlRef.LastTouchedCanPlacedMapLocator);
                CurRoleGroup = null;
            }

            TryPopAndPreviewRoleGroup();
        }
    }
}