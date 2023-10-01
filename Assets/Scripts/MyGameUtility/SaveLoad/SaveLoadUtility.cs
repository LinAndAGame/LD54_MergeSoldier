/*
 * Es3存储结构 ： 文件 -> 键 -> 数据
 */

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ES3Internal;
using MyGameExpand;
using Debug = UnityEngine.Debug;
using ThreadPriority = System.Threading.ThreadPriority;

namespace MyGameUtility.SaveLoad {
    public static class SaveLoadUtility {
        private static List<TaskData>    _AllCurTaskData     = new List<TaskData>();
        private static List<Es3TaskData> _AllCurEs3TaskDatas = new List<Es3TaskData>();

        static SaveLoadUtility() {
            ES3.Init();
            ES3IO.GetExtension(string.Empty);
        }

        public static void AsyncSaveByEs3<T>(string key, T needSaveData, ES3Settings es3Settings) {
            // 对相同路径文件进行存储时会使用Task进行链接
            // 如果对一个相同的saveData进行保存
            string pathKey       = es3Settings.path;
            var    aliveTaskData = _AllCurEs3TaskDatas.Find(data => data.PathKey == pathKey);
            if (aliveTaskData == null) {
                aliveTaskData = new Es3TaskData(pathKey);
                _AllCurEs3TaskDatas.Add(aliveTaskData);
            }

            aliveTaskData.SaveAsync(key, needSaveData, es3Settings);
        }

        public static void ForceSyncSaveByEs3<T>(string key, T needSaveData, ES3Settings es3Settings) {
            string pathKey       = es3Settings.path;
            var    aliveTaskData = _AllCurEs3TaskDatas.Find(data => data.PathKey == pathKey);
            if (aliveTaskData == null) {
                aliveTaskData = new Es3TaskData(pathKey);
            }

            aliveTaskData.SaveSync(key, needSaveData, es3Settings);
        }

        public static async void AsyncLoadByEs3<T>(string key) {
            Task<T> task = Task<T>.Factory.StartNew(() => {
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                return ES3.Load<T>(key);
            });

            await task;
            Debug.Log("异步读取已完成");
        }

        private class Es3TaskData {
            public string PathKey;

            public  Task                     CurRunningTask2;
            private List<Es3TaskElementData> AllEs3TaskElementDatas = new List<Es3TaskElementData>();
            private Es3TaskElementData       CurRunningElementData;

            public Es3TaskData(string pathKey) {
                PathKey = pathKey;
            }
            
            public void SaveAsync<T>(string key, T needSaveData, ES3Settings es3Settings) {
                Es3TaskElementData aliveElementData = AllEs3TaskElementDatas.Find(data => data.SaveDataKey == key);
                if (aliveElementData == null) {
                    aliveElementData = new Es3TaskElementData(key);
                }
                aliveElementData.NextTaskSaveDataAct2 = () => ES3.Save(key, needSaveData, es3Settings);
                AllEs3TaskElementDatas.Remove(aliveElementData);
                AllEs3TaskElementDatas.Add(aliveElementData);
                
                if (CurRunningTask2 == null) {
                    runeSaveAction();
                }

                async void runeSaveAction() {
                    CurRunningElementData = AllEs3TaskElementDatas[0];
                    AllEs3TaskElementDatas.Remove(CurRunningElementData);
                    
                    CurRunningTask2 = Task.Factory.StartNew(() => {
                        Debug.Log($"开始一个任务，向【{es3Settings.path}】中修改数据，键 = {CurRunningElementData.SaveDataKey}");
                        CurRunningElementData.NextTaskSaveDataAct2.Invoke();
                    });

                    await CurRunningTask2;
                    Debug.Log($"一个Save任务结束了！向【{es3Settings.path}】中修改数据，键 = {CurRunningElementData.SaveDataKey}");
                    CurRunningTask2       = null;
                    CurRunningElementData = null;
                    if (AllEs3TaskElementDatas.IsNullOrEmpty() == false) {
                        runeSaveAction();
                    }
                }
            }

            public void SaveSync<T>(string key, T needSaveData, ES3Settings es3Settings) {
                Debug.Log("终止当前正在进行的保存任务，开始！");
                if (CurRunningTask2 != null) {
                    CurRunningTask2.Wait();
                }
                foreach (var es3TaskElementData in AllEs3TaskElementDatas) {
                    es3TaskElementData.NextTaskSaveDataAct2.Invoke();
                }
                AllEs3TaskElementDatas.Clear();
                Debug.Log("终止当前正在进行的保存任务，结束！");

                Debug.Log("强制同步保存开始！");
                ES3.Save(key, needSaveData, es3Settings);
                Debug.Log("强制同步保存结束！");
            }
        }

        private class Es3TaskElementData {
            public readonly string SaveDataKey;

            public Action NextTaskSaveDataAct2;
            public Action OverrideActionWhileTasking;

            public Es3TaskElementData(string saveDataKey) {
                SaveDataKey = saveDataKey;
            }
        }

        private class TaskData {
            public string                        Key;
            public List<Task>                    TaskSources = new List<Task>();
            public List<CancellationTokenSource> CTSs        = new List<CancellationTokenSource>();

            public void CancelAllTasks() {
                Debug.Log($"当前存在的【{TaskSources.Count}】个保存任务，将全部取消！");
                for (var i = CTSs.Count - 1; i >= 0; i--) {
                    var cts = CTSs[i];
                    cts.Cancel();
                }

                TaskSources.Clear();
                CTSs.Clear();
            }
        }
    }
}