// UnitySceneViewAdapter.cs
// Unity场景视图适配器实现
// Author: AceMapEditor Team
// Date: 2025-01-19

using System.Numerics;
using UnityEditor;
using UnityEngine;
using AceMapEditor.Core.Interfaces;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;

namespace AceMapEditor.Unity.Editor.Adapters
{
    /// <summary>
    /// Unity场景视图适配器
    /// </summary>
    public class UnitySceneViewAdapter : ISceneViewAdapter
    {
        private readonly SceneView sceneView;

        public UnitySceneViewAdapter(SceneView sceneView)
        {
            this.sceneView = sceneView;
        }

        public (Vector3 origin, Vector3 direction) GetMouseRay(Vector2 mousePosition)
        {
            // 转换为Unity坐标系
            UnityEngine.Vector2 unityMousePos = new UnityEngine.Vector2(mousePosition.X, mousePosition.Y);

            // 获取场景视图摄像机
            Camera camera = sceneView.camera;
            if (camera == null)
                return (Vector3.Zero, Vector3.UnitZ);

            // 转换屏幕坐标
            UnityEngine.Ray ray = HandleUtility.GUIPointToWorldRay(unityMousePos);

            // 转换为System.Numerics.Vector3
            Vector3 origin = new Vector3(ray.origin.x, ray.origin.y, ray.origin.z);
            Vector3 direction = new Vector3(ray.direction.x, ray.direction.y, ray.direction.z);

            return (origin, direction);
        }

        public bool RaycastTerrain(Vector3 origin, Vector3 direction, out Vector3 hitPoint)
        {
            hitPoint = Vector3.Zero;

            // 转换为Unity类型
            UnityEngine.Vector3 unityOrigin = new UnityEngine.Vector3(origin.X, origin.Y, origin.Z);
            UnityEngine.Vector3 unityDir = new UnityEngine.Vector3(direction.X, direction.Y, direction.Z);
            UnityEngine.Ray ray = new UnityEngine.Ray(unityOrigin, unityDir);

            // 射线检测
            if (Physics.Raycast(ray, out RaycastHit hit, 10000f))
            {
                hitPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                return true;
            }

            // 如果没有碰撞体，尝试与Y=0平面相交
            return RaycastPlane(origin, direction, Vector3.UnitY, Vector3.Zero, out hitPoint);
        }

        public bool RaycastPlane(Vector3 origin, Vector3 direction, Vector3 planeNormal, Vector3 planePoint, out Vector3 hitPoint)
        {
            hitPoint = Vector3.Zero;

            // 归一化方向
            direction = System.Numerics.Vector3.Normalize(direction);

            // 计算射线与平面的交点
            float denom = System.Numerics.Vector3.Dot(planeNormal, direction);

            if (Math.Abs(denom) < 1e-6f)
                return false; // 射线与平面平行

            Vector3 diff = planePoint - origin;
            float t = System.Numerics.Vector3.Dot(diff, planeNormal) / denom;

            if (t < 0)
                return false; // 交点在射线背后

            hitPoint = origin + direction * t;
            return true;
        }

        public void RepaintSceneView()
        {
            sceneView?.Repaint();
        }

        public Vector3 GetCameraPosition()
        {
            if (sceneView?.camera == null)
                return Vector3.Zero;

            var pos = sceneView.camera.transform.position;
            return new Vector3(pos.x, pos.y, pos.z);
        }

        public Vector3 GetCameraForward()
        {
            if (sceneView?.camera == null)
                return Vector3.UnitZ;

            var forward = sceneView.camera.transform.forward;
            return new Vector3(forward.x, forward.y, forward.z);
        }
    }
}
