using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Workbench
{
    public static class UtilEditorGUI
    {
        private static int sMouseDeltaReaderHash = "MouseDeltaReader".GetHashCode();
        private static Vector2 sMouseDeltaReaderLastPos;

        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
        private static Func<Vector2,Vector2> sUnclip = Type.GetType("UnityEngine.GUIClip,UnityEngine").GetMethod("Unclip",FLAGS,null,new Type[]{typeof(Vector2)},null).CreateDelegate(typeof(Func<Vector2,Vector2>)) as Func<Vector2,Vector2>;
        private static Func<Rect, Event, bool> sHitTest = typeof(GUIUtility).GetMethod("HitTest", FLAGS,null,new Type[]{typeof(Rect),typeof(Event)},null).CreateDelegate(typeof(Func<Rect, Event, bool>)) as Func<Rect, Event, bool>;
        
            
            
        public static Rect HandleHorizontalSplitter(Rect dragRect, float width, float minLeftSide, float minRightSide)
        {
            bool flag = Event.current.type == EventType.Repaint;
            if (flag)
            {
                EditorGUIUtility.AddCursorRect(dragRect, MouseCursor.SplitResizeLeftRight);
            }
            float num = 0f;
            float x = MouseDeltaReader(dragRect, true).x;
            bool flag2 = x != 0f;
            if (flag2)
            {
                dragRect.x += x;
                num = Mathf.Clamp(dragRect.x, minLeftSide, width - minRightSide);
            }
            bool flag3 = dragRect.x > width - minRightSide;
            if (flag3)
            {
                num = width - minRightSide;
            }
            bool flag4 = num > 0f;
            if (flag4)
            {
                dragRect.x = num;
            }
            return dragRect;
        }
        
        public static Vector2 MouseDeltaReader(Rect position, bool activated)
        {
            int controlID = GUIUtility.GetControlID(sMouseDeltaReaderHash, FocusType.Passive, position);
            Event current = Event.current;
            switch (current.GetTypeForControl(controlID))
            {
                case EventType.MouseDown:
                {
                    bool flag = activated && GUIUtility.hotControl == 0 && sHitTest.Invoke(position, current) && current.button == 0;
                    if (flag)
                    {
                        GUIUtility.hotControl = controlID;
                        GUIUtility.keyboardControl = 0;
                        sMouseDeltaReaderLastPos = sUnclip.Invoke(current.mousePosition);
                        current.Use();
                    }
                    break;
                }
                case EventType.MouseUp:
                {
                    bool flag2 = GUIUtility.hotControl == controlID && current.button == 0;
                    if (flag2)
                    {
                        GUIUtility.hotControl = 0;
                        current.Use();
                    }
                    break;
                }
                case EventType.MouseDrag:
                {
                    bool flag3 = GUIUtility.hotControl == controlID;
                    if (flag3)
                    {
                        Vector2 a = sUnclip.Invoke(current.mousePosition);
                        Vector2 result = a - sMouseDeltaReaderLastPos;
                        sMouseDeltaReaderLastPos = a;
                        current.Use();
                        return result;
                    }
                    break;
                }
            }
            return Vector2.zero;
        }
        
        public static void DrawHorizontalSplitter(Rect dragRect)
        {
            if (Event.current.type != UnityEngine.EventType.Repaint)
                return;
            Color color = GUI.color;
            GUI.color *= EditorGUIUtility.isProSkin ? new Color(0.12f, 0.12f, 0.12f, 1.333f) : new Color(0.6f, 0.6f, 0.6f, 1.333f);
            GUI.DrawTexture(new Rect(dragRect.x - 1f, dragRect.y, 1f, dragRect.height), (Texture) EditorGUIUtility.whiteTexture);
            GUI.color = color;
        }
    }
}