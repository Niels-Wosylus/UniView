using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Wosylus.UniView.Tools.Editor
{
    public static class HierarchyObserver
    {
        [InitializeOnLoadMethod]
        private static void Init()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }
        
        private static void OnHierarchyChanged()
        {
            var openScene = SceneManager.GetActiveScene();
            var roots = openScene.GetRootGameObjects();
            var views = new List<ViewElementBase>();
            foreach (var root in roots)
            {
                var children = root.GetComponentsInChildren<ViewElementBase>();
                views.AddRange(children);
            }

            foreach (var view in views)
            {
                view.OnValidate();
            }
        }
    }
}