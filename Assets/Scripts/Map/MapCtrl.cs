using System.Collections.Generic;
using System.Linq;
using Fight;
using MyGameExpand;
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
        public List<MapLocator> AllCanAttackMapLocators;

        public bool IsLocatorAtAttackArea(MapLocator original) {
            return AllCanAttackMapLocators.Contains(original);
        }
        public bool IsLocatorAtPreviewArea(MapLocator original) {
            return AllPlayerPreviewMapLocators.Contains(original);
        }
        public MapLocator GetUpAroundLocator(MapLocator original) {
            return GetAroundLocator(original, Vector2Int.up);
        }
        public MapLocator GetDownAroundLocator(MapLocator original) {
            return GetAroundLocator(original, Vector2Int.down);
        }
        public MapLocator GetLeftAroundLocator(MapLocator original) {
            return GetAroundLocator(original, Vector2Int.left);
        }
        public MapLocator GetRightAroundLocator(MapLocator original) {
            return GetAroundLocator(original, Vector2Int.right);
        }
        public MapLocator GetAroundLocator(MapLocator original, Vector2Int dir) {
            return AllMapLocators.Find(data => data.Pos == original.Pos + dir);
        }

        public List<BaseRole_Player> AllAttackAreaPlayers => AllCanAttackMapLocators.FindAll(data => data.HasRoleData).Select(data => data.CurPlacedPlayerRole).ToList();
        
        public List<BaseRole_Player> GetLinkedRolePlayers(BaseRole_Player rolePlayer) {
            List<BaseRole_Player>  result      = new List<BaseRole_Player>();
            List<MapLocator>   checkedList = new List<MapLocator>();
            Queue<BaseRole_Player> tempQueue   = new Queue<BaseRole_Player>();
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

                    var aroundPlayer = aroundLocator.CurPlacedPlayerRole;
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
                BaseRole_Player readyPlacedRoleCtrl = roleGroup.AllReadyPlacedRoleCtrls[i];
                readyPlacedRoleCtrl.PlaceToLocator(canPlacedMapLocator);
                readyPlacedRoleCtrl.MoveToBelongToMapLocator();
                canPlacedMapLocator = canPlacedMapLocator.RightLocator;
            }
        }

        public void PlaceRoleGroup(RoleGroup roleGroup, MapLocator mapLocator) {
            PreviewPlaceRoleGroup(roleGroup, mapLocator);
            initRoleGroupRoles();
            moveUp(out List<BaseRole_Player> abandonedRoles);
            abandonedRolesHandle(abandonedRoles);

            List<BaseRole_Player> aliveLinkedList = new List<BaseRole_Player>();

            while (checkLinkList(out aliveLinkedList)) {
                var              curRoleType        = aliveLinkedList[0].RoleType;
                List<MapLocator> linkedLocatorsList = new List<MapLocator>(aliveLinkedList.Select(data=>data.BelongToLocator));
                removeLinkedList(aliveLinkedList);
                var upgradeRolePlacedLocator = getUpgradeRolePlacedLocator(linkedLocatorsList);
                addUpgradedRole(upgradeRolePlacedLocator,curRoleType);
                moveDown();
            }

            void initRoleGroupRoles() {
                foreach (var role in roleGroup.AllReadyPlacedRoleCtrls) {
                    role.Init();
                }
            }
            
            void moveUp(out List<BaseRole_Player> abandonedRoles) {
                // 将当前角色组放置到攻击区域中
                // 如果攻击区域纵列已经满了，则纵列的首个角色将被抛弃
                abandonedRoles = new List<BaseRole_Player>();
                foreach (BaseRole_Player readyPlacedRoleCtrl in roleGroup.AllReadyPlacedRoleCtrls) {
                    MapLocator        fromLocator       = readyPlacedRoleCtrl.BelongToLocator;
                    Stack<MapLocator> columnMapLocators = new Stack<MapLocator>();
                    do {
                        columnMapLocators.Push(fromLocator);
                        fromLocator = fromLocator.UpLocator;
                    } while (fromLocator != null && fromLocator.HasRoleData);

                    while (columnMapLocators.Count > 0) {
                        var topLocator = columnMapLocators.Pop();
                        var topRole    = topLocator.CurPlacedPlayerRole;
                        if (topLocator.UpLocator == null) {
                            abandonedRoles.Add(topRole);
                            topRole.SetNotPlacedOnLocator();
                        }
                        else {
                            topRole.MoveToLocator(topLocator.UpLocator);
                        }
                    }
                }
            }

            void abandonedRolesHandle(List<BaseRole_Player> abandonedRoles) {
                foreach (var abandonedRole in abandonedRoles) {
                    abandonedRole.DestroySelf();
                }
            }

            bool checkLinkList(out List<BaseRole_Player> linkedList) {
                linkedList = new List<BaseRole_Player>();
                for (var i = 0; i < AllCanAttackMapLocators.Count; i++) {
                    var curLocator           = AllCanAttackMapLocators[i];
                    if (curLocator.HasRoleData == false) {
                        continue;
                    }
                    
                    var curPlayer            = curLocator.CurPlacedPlayerRole;
                    linkedList.Clear();
                    linkedList.AddRange(GetLinkedRolePlayers(curPlayer));
                    if (linkedList.Count >= curPlayer.MergeCount) {
                        return true;
                    }
                }
                
                return false;
            }

            void removeLinkedList(List<BaseRole_Player> linkedList) {
                foreach (var roleCtrl in linkedList) {
                    roleCtrl.DestroySelf();
                }
            }

            MapLocator getUpgradeRolePlacedLocator(List<MapLocator> linkedLocatorsList) {
                List<MapLocator> tempLinkedLocatorsLink = new List<MapLocator>(linkedLocatorsList);
                tempLinkedLocatorsLink.Sort((a, b) => {
                    Vector2Int aPos = a.Pos;
                    Vector2Int bPos = b.Pos;
                    if (aPos.x < bPos.x) {
                        return -1;
                    }
                    else if (aPos.x == bPos.x) {
                        if (aPos.y < bPos.y) {
                            return -1;
                        }
                    }

                    return 1;
                });
                return tempLinkedLocatorsLink[0];
            }

            void addUpgradedRole(MapLocator upgradeMapLocator, RoleTypeEnum curRoleType) {
                int          nextRoleTypeNum = (int) curRoleType + 1;
                if (System.Enum.IsDefined(typeof(RoleTypeEnum), nextRoleTypeNum)) {
                    RoleTypeEnum nextRoleType = (RoleTypeEnum)nextRoleTypeNum;
                    var          nextRole     = Instantiate(FightCtrl.I.RoleCreatorCtrlRef.GetRoleByType(nextRoleType) as BaseRole_Player);
                    nextRole.InitOnRoleGroup();
                    nextRole.MoveToLocator(upgradeMapLocator);
                    nextRole.Init();

                    var allowedRoleAbilityDatas = ScriptableAssetsCollection.I.AllRoleAbilityDatas.FindAll(data => data.AllowedRoleTypes.Contains(nextRole.RoleType)).GetRandomList(3);
                    if (allowedRoleAbilityDatas.IsNullOrEmpty() == false) {
                        FightCtrl.I.FightUIRef.OpenPlayerAbilityChoosePanel(nextRole, allowedRoleAbilityDatas);   
                    }
                }
            }

            void moveDown() {
                bool needRecheckMoveDown = false;
                do {
                    needRecheckMoveDown = false;
                    foreach (var canAttackMapLocator in AllCanAttackMapLocators) {
                        if (canAttackMapLocator.HasRoleData && canAttackMapLocator.DownLocator.IsCanAttackLocator && canAttackMapLocator.DownLocator.HasRoleData == false) {
                            Queue<MapLocator> columnMapLocators = new Queue<MapLocator>();
                            var               downLocator       = canAttackMapLocator;
                            do {
                                columnMapLocators.Enqueue(downLocator);
                                downLocator = downLocator.UpLocator;
                            } while (downLocator != null && downLocator.HasRoleData);

                            while (columnMapLocators.Count > 0) {
                                var tempMapLocator = columnMapLocators.Dequeue();
                                var player         = tempMapLocator.CurPlacedPlayerRole;
                                player.MoveToLocator(tempMapLocator.DownLocator);
                            }

                            needRecheckMoveDown = true;
                        }
                    }
                } while (needRecheckMoveDown);
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
            AllCanAttackMapLocators = new List<MapLocator>();
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
                        AllCanAttackMapLocators.Add(mapLocator);
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