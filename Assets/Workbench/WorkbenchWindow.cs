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

        private int mInstance;
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
                    if (item.mInfo != workbenchInfo) continue;
                    window = item;
                    break;
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
            window.mInstance = instanceID;
            window.minSize = new Vector2(600, 400);
            window.Show();
            window.Focus();
            
            return true;
        }

        private void OnDestroy()
        {
            sCacheWindows.Remove(mInstance);
        }

        private float mToolbarHeight = 21;
        private float mLastListWidth;
        [SerializeField]
        private float mDirectoriesAreaWidth = 115f;
        private float mMinDirectoriesAreaWidth = 110f;
        [NonSerialized]
        private Rect mListAreaRect;
        [NonSerialized]
        private Rect mTreeViewRect;

        private void OnGUI()
        {
            if (mInfo==null)
            {
                Close();
            }
            this.ResizeHandling(base.position.width, base.position.height - mToolbarHeight);
            // if (this.m_SearchFilter.IsSearching())
            //     this.SearchAreaBar();
            // else
            //     this.BreadCrumbBar();
            //this.m_FolderTree.OnGUI(this.m_TreeViewRect, this.m_TreeViewKeyboardControlID);
            //this.m_ListArea.OnGUI(this.m_ListAreaRect, this.m_ListKeyboardControlID);
            UtilEditorGUI.DrawHorizontalSplitter(new Rect(this.mListAreaRect.x + 1f,  mToolbarHeight, 1f, base.position.height - mToolbarHeight));
            // if (this.m_SearchFilter.GetState() == SearchFilter.State.FolderBrowsing && this.m_ListArea.numItemsDisplayed == 0)
            // {
            //     Vector2 vector2 = EditorStyles.label.CalcSize(ProjectBrowser.s_Styles.m_EmptyFolderText);
            //     Rect position3 = new Rect(this.m_ListAreaRect.x + 2f + Mathf.Max(0.0f, (float) (((double) this.m_ListAreaRect.width - (double) vector2.x) * 0.5)), this.m_ListAreaRect.y + 10f, vector2.x, 20f);
            //     using (new EditorGUI.DisabledScope(true))
            //         GUI.Label(position3, ProjectBrowser.s_Styles.m_EmptyFolderText, EditorStyles.label);
            // }
          
        }


        private void ResizeHandling(float width, float height)
        {
            Rect dragRect = new Rect(this.mDirectoriesAreaWidth, mToolbarHeight, 5f, height);
            dragRect = UtilEditorGUI.HandleHorizontalSplitter(dragRect, base.position.width, this.mMinDirectoriesAreaWidth, 230f - this.mMinDirectoriesAreaWidth);
            this.mDirectoriesAreaWidth = dragRect.x;
            float num = base.position.width - this.mDirectoriesAreaWidth;
            bool flag2 = num != this.mLastListWidth;
            if (flag2)
            {
                //this.RefreshSplittedSelectedPath();
            }
            this.mLastListWidth = num;
        }

    }
}
