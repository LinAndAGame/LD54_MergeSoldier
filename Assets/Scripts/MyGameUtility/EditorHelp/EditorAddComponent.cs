#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace MyGameUtility.EditorHelp {
    [InitializeOnLoad]
    public class EditorAddComponent
    {
        static EditorAddComponent()
        {
            
            // ObjectFactory.componentWasAdded -= HandleComponentAdded;
            // ObjectFactory.componentWasAdded += HandleComponentAdded;
            //
            // EditorApplication.quitting -= OnEditorQuiting;
            // EditorApplication.quitting += OnEditorQuiting;
        }
        private static void HandleComponentAdded(UnityEngine.Component obj)
        {
            Debug.Log(obj.name);
        }

        private static void OnEditorQuiting()
        {
            ObjectFactory.componentWasAdded -= HandleComponentAdded;
            EditorApplication.quitting      -= OnEditorQuiting;
        }
    }
}
#endif