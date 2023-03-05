/********************************************************************
 Copyright (C) 2020 STUPID DOG STUDIO 
 类    名：UnityIconWindow.cs
 创建时间：2021-04-01 17:51:52
 作    者：Birth.Fat 
 描    述：
 版    本：2.0
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.IO;

public class UnityIconsWindow : EditorWindow
{
    GUIStyle m_textStyle;
    GUIStyle m_saveBtnStyle;
    GUIStyle m_iconNameStyle;
    string m_search = "";
    Vector2 m_scrollPosition = new Vector2(0, 0);
    List<UnityEngine.Object> m_Icons;
    TextEditor m_te = new TextEditor();
    
    [MenuItem("Tools/UnityIcon")]
    static void AddWindow()
    {
        Rect wr = new Rect(0, 0, 1600, 1000);
        UnityIconsWindow window = (UnityIconsWindow)EditorWindow.GetWindowWithRect(typeof(UnityIconsWindow), wr, true, "Unity系统图标");
        window.Show();
    }
    //------------------------------------------------------
    void OnGUI()
    {
        SystemIconViewer();
    }
    //------------------------------------------------------
    private void SystemIconViewer()
    {
        InitGUIStyle();
        
        GUILayout.BeginHorizontal("HelpBox");
        GUILayout.Label("结果如下：" + m_search, m_textStyle);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("SHIT!", m_saveBtnStyle, GUILayout.Width(80)))
        {
            InitIconList();
        }
        GUILayout.EndHorizontal();
        
        m_scrollPosition = GUILayout.BeginScrollView(m_scrollPosition);
        if (m_Icons != null)
        {
            for (int i = 0; i < m_Icons.Count; i++)
            {
                if (i % 15 == 0)
                {
                    GUILayout.BeginHorizontal("PopupCurveSwatchBackground");
                    for (int j = 0; j < 15; j++)
                    {
                        var icon = m_Icons[i];

                        GUILayout.BeginVertical();

                        if (GUILayout.Button((Texture)icon, GUILayout.Width(100), GUILayout.Height(100)))
                        {
                            m_te.text = icon.name;
                            m_te.SelectAll();
                            m_te.Copy();
                            this.ShowNotification(new GUIContent("Unity 体统不表：" + icon.name + "已经复制到剪切板！"));
                        }
                        GUILayout.Label(icon.name, m_iconNameStyle, GUILayout.Width(100));
                        GUILayout.EndVertical();
                        i++;

                        if (i == m_Icons.Count)
                            break;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(6);
                }
            }
        }
        GUILayout.EndScrollView();
    }
    //------------------------------------------------------
    private void InitIconList()
    {
        if (m_Icons == null || m_Icons.Count == 0)
        {
            m_Icons = new List<UnityEngine.Object>(Resources.FindObjectsOfTypeAll(typeof(Texture)));
            m_Icons.Sort((pA, pB) => System.String.Compare(pA.name, pB.name, System.StringComparison.OrdinalIgnoreCase));
            m_search = m_Icons.Count.ToString(); // GUILayout.Label(m_search)
        }
    }

    //------------------------------------------------------
    private void InitGUIStyle()
    {
        if (m_textStyle == null)
        {
            m_textStyle = new GUIStyle("HeaderLabel");
            m_textStyle.fontSize = 20;
            //color = textStyle.normal.textColor ;
        }
        if (m_iconNameStyle == null)
        {
            m_iconNameStyle = new GUIStyle("WarningOverlay");
            m_iconNameStyle.fontSize = 12;
        }
        if (m_saveBtnStyle == null)
        {
            m_saveBtnStyle = new GUIStyle("flow node 1");
            m_saveBtnStyle.fontSize = 16;
            m_saveBtnStyle.fixedHeight = 38;
            m_saveBtnStyle.alignment = TextAnchor.MiddleCenter;
            m_saveBtnStyle.fontStyle = m_textStyle.fontStyle;
        }
    }

    //------------------------------------------------------
    private void Reset()
    {
        if (m_Icons == null)
            return;
        if (m_Icons.Count > 0)
            m_Icons.Clear();
    }
}