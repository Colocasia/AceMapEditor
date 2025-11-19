// UnityGizmosAdapter.cs
// Unity Gizmos适配器实现
// Author: AceMapEditor Team
// Date: 2025-01-19

using System.Numerics;
using UnityEditor;
using UnityEngine;
using AceMapEditor.Core.Interfaces;
using Vector3 = System.Numerics.Vector3;
using Vector4 = System.Numerics.Vector4;

namespace AceMapEditor.Unity.Editor.Adapters
{
    /// <summary>
    /// Unity Gizmos适配器
    /// </summary>
    public class UnityGizmosAdapter : IGizmosAdapter
    {
        private Color currentColor = Color.white;

        public void SetColor(Vector4 color)
        {
            currentColor = new Color(color.X, color.Y, color.Z, color.W);
            Handles.color = currentColor;
        }

        public void DrawLine(Vector3 start, Vector3 end)
        {
            UnityEngine.Vector3 unityStart = ToUnityVector3(start);
            UnityEngine.Vector3 unityEnd = ToUnityVector3(end);
            Handles.DrawLine(unityStart, unityEnd);
        }

        public void DrawLines(Vector3[] points)
        {
            if (points.Length < 2)
                return;

            for (int i = 0; i < points.Length - 1; i++)
            {
                DrawLine(points[i], points[i + 1]);
            }
        }

        public void DrawCircle(Vector3 center, Vector3 normal, float radius, int segments = 32)
        {
            UnityEngine.Vector3 unityCenter = ToUnityVector3(center);
            UnityEngine.Vector3 unityNormal = ToUnityVector3(normal);

            Handles.DrawWireDisc(unityCenter, unityNormal, radius);
        }

        public void DrawWireSphere(Vector3 center, float radius)
        {
            UnityEngine.Vector3 unityCenter = ToUnityVector3(center);
            Handles.DrawWireDisc(unityCenter, UnityEngine.Vector3.up, radius);
            Handles.DrawWireDisc(unityCenter, UnityEngine.Vector3.right, radius);
            Handles.DrawWireDisc(unityCenter, UnityEngine.Vector3.forward, radius);
        }

        public void DrawWireCube(Vector3 center, Vector3 size)
        {
            UnityEngine.Vector3 unityCenter = ToUnityVector3(center);
            UnityEngine.Vector3 unitySize = ToUnityVector3(size);

            // 绘制立方体的12条边
            float x = unitySize.x / 2;
            float y = unitySize.y / 2;
            float z = unitySize.z / 2;

            // 底面
            DrawLine(center + new Vector3(-x, -y, -z), center + new Vector3(x, -y, -z));
            DrawLine(center + new Vector3(x, -y, -z), center + new Vector3(x, -y, z));
            DrawLine(center + new Vector3(x, -y, z), center + new Vector3(-x, -y, z));
            DrawLine(center + new Vector3(-x, -y, z), center + new Vector3(-x, -y, -z));

            // 顶面
            DrawLine(center + new Vector3(-x, y, -z), center + new Vector3(x, y, -z));
            DrawLine(center + new Vector3(x, y, -z), center + new Vector3(x, y, z));
            DrawLine(center + new Vector3(x, y, z), center + new Vector3(-x, y, z));
            DrawLine(center + new Vector3(-x, y, z), center + new Vector3(-x, y, -z));

            // 竖边
            DrawLine(center + new Vector3(-x, -y, -z), center + new Vector3(-x, y, -z));
            DrawLine(center + new Vector3(x, -y, -z), center + new Vector3(x, y, -z));
            DrawLine(center + new Vector3(x, -y, z), center + new Vector3(x, y, z));
            DrawLine(center + new Vector3(-x, -y, z), center + new Vector3(-x, y, z));
        }

        public void DrawSolidDisc(Vector3 center, Vector3 normal, float radius)
        {
            UnityEngine.Vector3 unityCenter = ToUnityVector3(center);
            UnityEngine.Vector3 unityNormal = ToUnityVector3(normal);

            Handles.DrawSolidDisc(unityCenter, unityNormal, radius);
        }

        public void DrawLabel(Vector3 position, string text)
        {
            UnityEngine.Vector3 unityPos = ToUnityVector3(position);
            Handles.Label(unityPos, text);
        }

        public void DrawArrow(Vector3 start, Vector3 end, float arrowHeadLength = 0.25f)
        {
            DrawLine(start, end);

            Vector3 direction = Vector3.Normalize(end - start);
            Vector3 right = Vector3.Cross(Vector3.UnitY, direction);
            if (right.LengthSquared() < 0.001f)
                right = Vector3.Cross(Vector3.UnitX, direction);
            right = Vector3.Normalize(right);
            Vector3 up = Vector3.Cross(direction, right);

            Vector3 arrowTip = end;
            Vector3 arrowBase = end - direction * arrowHeadLength;

            DrawLine(arrowTip, arrowBase + right * arrowHeadLength * 0.5f);
            DrawLine(arrowTip, arrowBase - right * arrowHeadLength * 0.5f);
            DrawLine(arrowTip, arrowBase + up * arrowHeadLength * 0.5f);
            DrawLine(arrowTip, arrowBase - up * arrowHeadLength * 0.5f);
        }

        private static UnityEngine.Vector3 ToUnityVector3(Vector3 v)
        {
            return new UnityEngine.Vector3(v.X, v.Y, v.Z);
        }
    }
}
