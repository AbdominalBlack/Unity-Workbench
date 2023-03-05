using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Workbench
{
    public class WorkbenchWindow : EditorWindow
    {
        private static Dictionary<int, WorkbenchWindow> sCacheWindows = new Dictionary<int, WorkbenchWindow>();

        private WorkbenchInfo mInfo;
        
        
        
        
        [OnOpenAsset(0)]
        public static bool Open(int instanceID, int line)
        {
            if (!(EditorUtility.InstanceIDToObject(instanceID) is WorkbenchInfo workbenchInfo)) return false;
             
            if (!sCacheWindows.TryGetValue(instanceID,out WorkbenchWindow window))
            {
                WorkbenchWindow[] windows = Resources.FindObjectsOfTypeAll<WorkbenchWindow>();
                foreach (WorkbenchWindow item in windows)
                {
                    if (item.mInfo==workbenchInfo)
                    {
                        window = item;
                        break;
                    }
                }

                if (window==null)
                {
                    window = CreateWindow<WorkbenchWindow>();
                }
                sCacheWindows.Add(instanceID, window);
            }

            string title = string.IsNullOrEmpty(workbenchInfo.Function) ? "Workbench" : $"Workbench - {workbenchInfo.Function}";
            window.titleContent = new GUIContent(title, EditorGUIUtility.IconContent("d_Favorite Icon").image);
            window.mInfo = workbenchInfo;
            window.minSize = new Vector2(600, 400);
            window.Show();
            window.Focus();
            
            return true;
        }

        private void OnDestroy()
        {
            sCacheWindows.Remove(mInfo.GetInstanceID());
        }
    }
}
