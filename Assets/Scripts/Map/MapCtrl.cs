using System.Collections.Generic;
using System.Linq;
using Fight;
using Role;
using RoleGroups;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Map {
    public class MapCtrl : MonoBehaviour {
        public Color            PreviewLocatorColor;
        public List<MapLocator> AllMapLocators;
        public List<MapLocator> AllPlayerMapLocators;
        public List<MapLocator> AllEnemyMapLocators;
        public List<MapLocator> AllPlayerPreviewMapLocators;
        public List<MapLocator> AllPlayerCanAttackMapLocators;

        private Stack<MapLocator> GetHasDataUpMapLocators(MapLocator from) {
            Stack<MapLocator> columnMapLocators = new Stack<MapLocator>();
            do {
                columnMapLocators.Push(from);
                from = from.UpLocator;
            } while (from != null && from.HasRoleData);

            return columnMapLocators;
        }

        private List<Role_Player> getLinkedRolePlayers(Role_Player rolePlayer) {
            List<Role_Player>  result      = new List<Role_Player>();
            List<MapLocator>   checkedList = new List<MapLocator>();
            Queue<Role_Player> tempQueue   = new Queue<Role_Player>();
            tempQueue.Enqueue(rolePlayer);

            while (tempQueue.Count > 0) {
                var curRolePlayer = tempQueue.Dequeue();
                var curMapLocator = curRolePlayer.BelongToLocator;
                foreach (MapLocator aroundLocator in curMapLocator.AroundLocators) {
                    if (FightCtrl.I.MapCtrlRef.AllPlayerPreviewMapLocators.Contains(aroundLocator)) {
                        continue;
                    }

                    if (checkedList.Contains(aroundLocator)) {
                        continue;
                    }

                    var aroundPlayer = aroundLocator.CurPlacedRoleCtrl;
                    checkedList.Add(aroundLocator);
                    if (aroundLocator.HasRoleData && aroundPlayer.RoleType == rolePlayer.RoleType) {
                        result.Add(aroundPlayer);
                        tempQueue.Enqueue(aroundPlayer);
                    }
                }
            }

            return result;
        }

        public MapLocator GetCanPlacedMapLocator(RoleGroup roleGroup, MapLocator mapLocator) {
            if (CanPlaceLocator(roleGroup, mapLocator)) {
                return mapLocator;
            }
            else {
                return AllPlayerPreviewMapLocators[AllPlayerPreviewMapLocators.Count - roleGroup.AllReadyPlacedRoleCtrls.Count];
            }
        }

        private bool CanPlaceLocator(RoleGroup roleGroup, MapLocator placeToLocator) {
            var locatorIndex = placeToLocator.Pos.x;
            return AllPlayerPreviewMapLocators.Count - roleGroup.AllReadyPlacedRoleCtrls.Count >= locatorIndex;
        }

        public void PreviewPlaceRoleGroup(RoleGroup roleGroup, MapLocator mapLocator) {
            MapLocator canPlacedMapLocator = GetCanPlacedMapLocator(roleGroup, mapLocator);

            for (int i = 0; i < roleGroup.AllReadyPlacedRoleCtrls.Count; i++) {
                Role_Player readyPlacedRoleCtrl = roleGroup.AllReadyPlacedRoleCtrls[i];
                readyPlacedRoleCtrl.PlaceToLocator(canPlacedMapLocator);
                readyPlacedRoleCtrl.MoveToBelongToMapLocator();
                canPlacedMapLocator = canPlacedMapLocator.RightLocator;
            }
        }

        public void PlaceRoleGroup(RoleGroup roleGroup, MapLocator mapLocator) {
            bool needReCall = false;
            PreviewPlaceRoleGroup(roleGroup, mapLocator);

            do {
                needReCall = false;
                levelUp();
                moveDown();
            } while (needReCall);
            moveUp();
            initSystemData();

            void initSystemData() {
                foreach (Role_Player readyPlacedRoleCtrl in roleGroup.AllReadyPlacedRoleCtrls) {
                    readyPlacedRoleCtrl.Init();
                }
            }

            void levelUp() {
                for (var i = 0; i < roleGroup.AllReadyPlacedRoleCtrls.Count; i++) {
                    var curPlayer            = roleGroup.AllReadyPlacedRoleCtrls[i];
                    var allLinkedRolePlayers = getLinkedRolePlayers(curPlayer);
                    curPlayer.LevelUp(allLinkedRolePlayers.Sum(data=>data.RoleCommonInfo.Lv));
                    foreach (Role_Player linkedRolePlayer in allLinkedRolePlayers) {
                        linkedRolePlayer.DestroySelf();
                    }
                    if (curPlayer.RoleCommonInfo.Lv >= curPlayer.MergeCount) {
                        RoleTypes curRoleType     = curPlayer.RoleType;
                        int       nextRoleTypeNum = (int) curRoleType + 1;
                        if (System.Enum.IsDefined(typeof(RoleTypes), nextRoleTypeNum)) {
                            var lastLocator = curPlayer.BelongToLocator;
                            curPlayer.DestroySelf();
                            
                            RoleTypes nextRoleType = (RoleTypes)nextRoleTypeNum;
                            var       nextRole = Instantiate(FightCtrl.I.RoleCreatorCtrlRef.GetRoleByType(nextRoleType) as Role_Player);
                            nextRole.InitOnRoleGroup();
                            nextRole.MoveToLocator(lastLocator);
                            roleGroup.AllReadyPlacedRoleCtrls[i] = nextRole;
                            needReCall                           = true;
                        }
                        else {
                            curPlayer.RoleCommonInfo.Lv = curPlayer.MergeCount;
                        }
                    }
                }
            }

            void moveDown() {
                var allHasRoleDataCanAttackMapLocators = AllPlayerCanAttackMapLocators.FindAll(data => data.HasRoleData && data.DownLocator.IsCanAttackLocator && data.DownLocator.HasRoleData == false);
                foreach (MapLocator hasRoleDataCanAttackMapLocator in allHasRoleDataCanAttackMapLocators) {
                    var columnMapLocators = GetHasDataUpMapLocators(hasRoleDataCanAttackMapLocator).ToList();

                    for (int i = columnMapLocators.Count - 1; i >= 0; i--) {
                        var tempMapLocator = columnMapLocators[i];
                        var player         = tempMapLocator.CurPlacedRoleCtrl;
                        player.MoveToLocator(tempMapLocator.DownLocator);
                    }
                }
            }
            
            void moveUp() {
                for (var i = 0; i < roleGroup.AllReadyPlacedRoleCtrls.Count; i++) {
                    var curPlayer  = roleGroup.AllReadyPlacedRoleCtrls[i];
                    var curLocator = curPlayer.BelongToLocator;
                    if (hasEmptyPlayerLocatorAtColumn(curLocator)) {
                        var columnMapLocators = GetHasDataUpMapLocators(curLocator);

                        while (columnMapLocators.Count > 0) {
                            var tempMapLocator = columnMapLocators.Pop();
                            var player         = tempMapLocator.CurPlacedRoleCtrl;
                            player.MoveToLocator(tempMapLocator.UpLocator);
                        }
                    }
                    else {
                        curPlayer.DestroySelf();
                    }
                }
            
                roleGroup.DestroySelf();
            }
            
            bool hasEmptyPlayerLocatorAtColumn(MapLocator mapLocator) {
                foreach (MapLocator playerMapLocator in AllPlayerCanAttackMapLocators) {
                    if (playerMapLocator.Pos.x == mapLocator.Pos.x) {
                        if (playerMapLocator.HasRoleData == false) {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

#if UNITY_EDITOR
        [BoxGroup("EditorCreateMap"), SerializeField]
        private Transform Editor_PlayerLocatorsParentTrans;
        [BoxGroup("EditorCreateMap"), SerializeField]
        private Transform Editor_EnemyLocatorsParentTrans;
        [BoxGroup("EditorCreateMap"), SerializeField]
        private MapLocator Editor_MapLocatorPrefab;
        [BoxGroup("EditorCreateMap"), SerializeField]
        private Vector2Int Editor_MapSize;
        [BoxGroup("EditorCreateMap"), SerializeField]
        private Vector2 Editor_LocatorInterval = new Vector2(0.2f, 0.1f);

        [BoxGroup("EditorCreateMap"), Button]
        private void Editor_CreateMap() {
            for (var i = AllMapLocators.Count - 1; i >= 0; i--) {
                AllMapLocators[i].DestroySelf();
            }
            AllMapLocators.Clear();

            float      prefabWidth         = Editor_MapLocatorPrefab.transform.localScale.x;
            float      prefabHeight        = Editor_MapLocatorPrefab.transform.localScale.y;
            float      width               = Editor_MapSize.x * prefabWidth  + (Editor_MapSize.x - 1) * Editor_LocatorInterval.x;
            float      height              = Editor_MapSize.y * prefabHeight + (Editor_MapSize.y - 1) * Editor_LocatorInterval.y;
            float      startX              = -(width  / 2f)                  + prefabWidth            / 2;
            float      startY              = -(height / 2f)                  + prefabHeight           / 2;

            AllPlayerPreviewMapLocators   = new List<MapLocator>();
            AllPlayerMapLocators          = new List<MapLocator>();
            AllPlayerCanAttackMapLocators = new List<MapLocator>();
            for (int i = 0; i < Editor_MapSize.x; i++) {
                for (int j = 0; j < Editor_MapSize.y; j++) {
                    var mapLocator = PrefabUtility.InstantiatePrefab(Editor_MapLocatorPrefab) as MapLocator;
                    mapLocator.transform.SetParent(this.Editor_PlayerLocatorsParentTrans);
                    mapLocator.Editor_Init(new Vector2Int(i,j));
                    AllMapLocators.Add(mapLocator);
                    AllPlayerMapLocators.Add(mapLocator);
                    if (j == 0) {
                        mapLocator.SR_Self.color = PreviewLocatorColor;
                        AllPlayerPreviewMapLocators.Add(mapLocator);
                    }
                    else {
                        AllPlayerCanAttackMapLocators.Add(mapLocator);
                    }
                    Vector3 localPos = new Vector3(startX + i * (prefabWidth + Editor_LocatorInterval.x), startY + j * (prefabHeight + Editor_LocatorInterval.y), 0);
                    mapLocator.transform.localPosition = localPos;
                }
            }

            AllEnemyMapLocators = new List<MapLocator>();
            for (int i = 0; i < Editor_MapSize.x; i++) {
                var mapLocator = PrefabUtility.InstantiatePrefab(Editor_MapLocatorPrefab) as MapLocator;
                mapLocator.transform.SetParent(this.Editor_EnemyLocatorsParentTrans);
                mapLocator.Editor_Init(new Vector2Int(i,999));
                AllMapLocators.Add(mapLocator);
                AllEnemyMapLocators.Add(mapLocator);
                Vector3 localPos = new Vector3(startX + i * (prefabWidth + Editor_LocatorInterval.x), 0, 0);
                mapLocator.transform.localPosition = localPos;
            }
        }
#endif
    }
}