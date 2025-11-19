// AceMapEditor/Core/src/Editor/MapEditor.cs
// 地图编辑器核心类
// Author: AceMapEditor Team
// Date: 2025-01-19

using System;
using System.Numerics;
using AceMapEditor.Core.Data;
using AceMapEditor.Core.Interfaces;

namespace AceMapEditor.Core.Editor
{
    /// <summary>
    /// 地图编辑器核心类（引擎无关）
    /// </summary>
    public class MapEditor
    {
        private readonly ISceneViewAdapter sceneViewAdapter;
        private readonly IGizmosAdapter gizmosAdapter;
        private readonly IInputAdapter inputAdapter;
        private readonly IEditorUIAdapter uiAdapter;

        /// <summary>
        /// 编辑器状态
        /// </summary>
        public EditorState State { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MapEditor(
            ISceneViewAdapter sceneViewAdapter,
            IGizmosAdapter gizmosAdapter,
            IInputAdapter inputAdapter,
            IEditorUIAdapter uiAdapter)
        {
            this.sceneViewAdapter = sceneViewAdapter ?? throw new ArgumentNullException(nameof(sceneViewAdapter));
            this.gizmosAdapter = gizmosAdapter ?? throw new ArgumentNullException(nameof(gizmosAdapter));
            this.inputAdapter = inputAdapter ?? throw new ArgumentNullException(nameof(inputAdapter));
            this.uiAdapter = uiAdapter ?? throw new ArgumentNullException(nameof(uiAdapter));

            State = new EditorState();
        }

        /// <summary>
        /// 创建新地图
        /// </summary>
        public void NewMap(int width, int height)
        {
            var map = new MapData
            {
                Name = "New Map",
                Width = width,
                Height = height,
                CreatedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                ModifiedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            // 初始化地形数据
            map.Terrain = new TerrainData
            {
                Resolution = width + 1,
                HeightMap = new float[(width + 1) * (height + 1)]
            };

            State.CurrentMap = map;
            State.CurrentFilePath = null;
            State.IsDirty = false;

            uiAdapter.LogInfo($"Created new map: {width}x{height}");
        }

        /// <summary>
        /// 更新编辑器（每帧调用）
        /// </summary>
        public void Update()
        {
            if (State.CurrentMap == null)
                return;

            HandleInput();
        }

        /// <summary>
        /// 绘制Gizmos（场景视图）
        /// </summary>
        public void OnDrawGizmos()
        {
            if (State.CurrentMap == null)
                return;

            // 绘制网格
            if (State.ShowGrid)
            {
                DrawGrid();
            }

            // 绘制笔刷预览
            if (IsTerrainEditTool(State.CurrentTool))
            {
                DrawBrushPreview();
            }

            // 绘制区域
            if (State.ShowRegions)
            {
                DrawRegions();
            }
        }

        /// <summary>
        /// 处理输入
        /// </summary>
        private void HandleInput()
        {
            // 获取鼠标射线
            var mousePos = inputAdapter.GetMousePosition();
            var (rayOrigin, rayDir) = sceneViewAdapter.GetMouseRay(mousePos);

            // 鼠标左键按下
            if (inputAdapter.IsMouseButton(0))
            {
                HandleMouseDrag(rayOrigin, rayDir);
            }

            // 快捷键处理
            HandleHotkeys();
        }

        /// <summary>
        /// 处理鼠标拖拽
        /// </summary>
        private void HandleMouseDrag(Vector3 rayOrigin, Vector3 rayDir)
        {
            // 与地形相交
            if (!sceneViewAdapter.RaycastTerrain(rayOrigin, rayDir, out Vector3 hitPoint))
                return;

            switch (State.CurrentTool)
            {
                case EditorTool.TerrainRaise:
                    // 地形抬升逻辑（第二阶段实现）
                    break;

                case EditorTool.TerrainLower:
                    // 地形降低逻辑（第二阶段实现）
                    break;

                case EditorTool.TexturePaint:
                    // 纹理绘制逻辑（第二阶段实现）
                    break;
            }
        }

        /// <summary>
        /// 处理快捷键
        /// </summary>
        private void HandleHotkeys()
        {
            // Ctrl+N - 新建地图
            if (inputAdapter.IsControlHeld() && inputAdapter.IsKeyDown(KeyCode.N))
            {
                // 由UI层处理
            }

            // Ctrl+S - 保存
            if (inputAdapter.IsControlHeld() && inputAdapter.IsKeyDown(KeyCode.S))
            {
                // 由UI层处理
            }

            // 工具切换快捷键
            if (inputAdapter.IsKeyDown(KeyCode.Q))
                State.CurrentTool = EditorTool.Select;
            else if (inputAdapter.IsKeyDown(KeyCode.W))
                State.CurrentTool = EditorTool.Move;
            else if (inputAdapter.IsKeyDown(KeyCode.E))
                State.CurrentTool = EditorTool.Rotate;
            else if (inputAdapter.IsKeyDown(KeyCode.R))
                State.CurrentTool = EditorTool.Scale;
        }

        /// <summary>
        /// 绘制网格
        /// </summary>
        private void DrawGrid()
        {
            if (State.CurrentMap?.Terrain == null)
                return;

            gizmosAdapter.SetColor(new Vector4(0.5f, 0.5f, 0.5f, 0.3f));

            var terrain = State.CurrentMap.Terrain;
            int width = State.CurrentMap.Width;
            int height = State.CurrentMap.Height;
            float cellSize = terrain.CellSize;

            // 绘制横线（每隔5格绘制一条）
            for (int z = 0; z <= height; z += 5)
            {
                Vector3 start = new Vector3(0, 0, z * cellSize);
                Vector3 end = new Vector3(width * cellSize, 0, z * cellSize);
                gizmosAdapter.DrawLine(start, end);
            }

            // 绘制竖线
            for (int x = 0; x <= width; x += 5)
            {
                Vector3 start = new Vector3(x * cellSize, 0, 0);
                Vector3 end = new Vector3(x * cellSize, 0, height * cellSize);
                gizmosAdapter.DrawLine(start, end);
            }
        }

        /// <summary>
        /// 绘制笔刷预览
        /// </summary>
        private void DrawBrushPreview()
        {
            var mousePos = inputAdapter.GetMousePosition();
            var (rayOrigin, rayDir) = sceneViewAdapter.GetMouseRay(mousePos);

            if (!sceneViewAdapter.RaycastTerrain(rayOrigin, rayDir, out Vector3 hitPoint))
                return;

            var brush = State.BrushSettings;
            gizmosAdapter.SetColor(new Vector4(1, 1, 0, 0.5f)); // 半透明黄色

            if (brush.Shape == BrushShape.Circle)
            {
                gizmosAdapter.DrawCircle(hitPoint, Vector3.UnitY, brush.Radius);
                gizmosAdapter.DrawSolidDisc(hitPoint, Vector3.UnitY, brush.Radius);
            }
            else if (brush.Shape == BrushShape.Square)
            {
                Vector3 size = new Vector3(brush.Radius * 2, 0.1f, brush.Radius * 2);
                gizmosAdapter.DrawWireCube(hitPoint, size);
            }
        }

        /// <summary>
        /// 绘制区域
        /// </summary>
        private void DrawRegions()
        {
            if (State.CurrentMap == null)
                return;

            foreach (var region in State.CurrentMap.Regions)
            {
                gizmosAdapter.SetColor(region.Color);

                switch (region.Type)
                {
                    case RegionType.Rectangle:
                        DrawRectangleRegion(region);
                        break;

                    case RegionType.Circle:
                        DrawCircleRegion(region);
                        break;

                    case RegionType.Polygon:
                        DrawPolygonRegion(region);
                        break;
                }

                // 绘制区域名称
                gizmosAdapter.DrawLabel(region.Center, region.Name);
            }
        }

        private void DrawRectangleRegion(RegionData region)
        {
            Vector3 size = new Vector3(region.Size.X, 1, region.Size.Y);
            gizmosAdapter.DrawWireCube(region.Center, size);
        }

        private void DrawCircleRegion(RegionData region)
        {
            gizmosAdapter.DrawCircle(region.Center, Vector3.UnitY, region.Size.X);
        }

        private void DrawPolygonRegion(RegionData region)
        {
            if (region.PolygonPoints.Count < 3)
                return;

            for (int i = 0; i < region.PolygonPoints.Count; i++)
            {
                int next = (i + 1) % region.PolygonPoints.Count;
                gizmosAdapter.DrawLine(region.PolygonPoints[i], region.PolygonPoints[next]);
            }
        }

        /// <summary>
        /// 检查是否为地形编辑工具
        /// </summary>
        private bool IsTerrainEditTool(EditorTool tool)
        {
            return tool == EditorTool.TerrainRaise ||
                   tool == EditorTool.TerrainLower ||
                   tool == EditorTool.TerrainSmooth ||
                   tool == EditorTool.TerrainFlatten ||
                   tool == EditorTool.TexturePaint;
        }

        /// <summary>
        /// 标记地图已修改
        /// </summary>
        public void MarkDirty()
        {
            State.IsDirty = true;
            State.CurrentMap!.ModifiedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }
}
