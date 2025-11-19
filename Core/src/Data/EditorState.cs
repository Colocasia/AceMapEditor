// AceMapEditor/Core/src/Data/EditorState.cs
// 编辑器状态数据
// Author: AceMapEditor Team
// Date: 2025-01-19

using System.Collections.Generic;
using System.Numerics;

namespace AceMapEditor.Core.Data
{
    /// <summary>
    /// 编辑器状态，管理当前编辑器的工作状态
    /// </summary>
    public class EditorState
    {
        /// <summary>
        /// 当前打开的地图数据
        /// </summary>
        public MapData? CurrentMap { get; set; }

        /// <summary>
        /// 当前使用的工具
        /// </summary>
        public EditorTool CurrentTool { get; set; } = EditorTool.None;

        /// <summary>
        /// 当前编辑模式
        /// </summary>
        public EditorMode CurrentMode { get; set; } = EditorMode.Terrain;

        /// <summary>
        /// 选中的对象ID列表
        /// </summary>
        public List<string> SelectedObjectIds { get; set; } = new List<string>();

        /// <summary>
        /// 地形笔刷设置
        /// </summary>
        public BrushSettings BrushSettings { get; set; } = new BrushSettings();

        /// <summary>
        /// 当前选中的纹理层索引
        /// </summary>
        public int CurrentTextureLayer { get; set; } = 0;

        /// <summary>
        /// 网格是否可见
        /// </summary>
        public bool ShowGrid { get; set; } = true;

        /// <summary>
        /// 是否显示区域
        /// </summary>
        public bool ShowRegions { get; set; } = true;

        /// <summary>
        /// 是否显示单位
        /// </summary>
        public bool ShowUnits { get; set; } = true;

        /// <summary>
        /// 地图文件路径
        /// </summary>
        public string? CurrentFilePath { get; set; }

        /// <summary>
        /// 地图是否有未保存的修改
        /// </summary>
        public bool IsDirty { get; set; } = false;
    }

    /// <summary>
    /// 编辑器工具枚举
    /// </summary>
    public enum EditorTool
    {
        /// <summary>无工具</summary>
        None,
        /// <summary>选择工具</summary>
        Select,
        /// <summary>移动工具</summary>
        Move,
        /// <summary>旋转工具</summary>
        Rotate,
        /// <summary>缩放工具</summary>
        Scale,
        /// <summary>地形抬升</summary>
        TerrainRaise,
        /// <summary>地形降低</summary>
        TerrainLower,
        /// <summary>地形平滑</summary>
        TerrainSmooth,
        /// <summary>地形平整</summary>
        TerrainFlatten,
        /// <summary>纹理绘制</summary>
        TexturePaint,
        /// <summary>放置单位</summary>
        PlaceUnit,
        /// <summary>放置装饰物</summary>
        PlaceDoodad,
        /// <summary>绘制区域</summary>
        DrawRegion
    }

    /// <summary>
    /// 编辑器模式
    /// </summary>
    public enum EditorMode
    {
        /// <summary>地形编辑模式</summary>
        Terrain,
        /// <summary>单位编辑模式</summary>
        Units,
        /// <summary>装饰物编辑模式</summary>
        Doodads,
        /// <summary>区域编辑模式</summary>
        Regions,
        /// <summary>触发器编辑模式</summary>
        Triggers
    }

    /// <summary>
    /// 笔刷设置
    /// </summary>
    public class BrushSettings
    {
        /// <summary>
        /// 笔刷半径（世界单位）
        /// </summary>
        public float Radius { get; set; } = 5.0f;

        /// <summary>
        /// 笔刷强度（0-1）
        /// </summary>
        public float Strength { get; set; } = 0.5f;

        /// <summary>
        /// 笔刷衰减类型
        /// </summary>
        public BrushFalloff Falloff { get; set; } = BrushFalloff.Smooth;

        /// <summary>
        /// 笔刷形状
        /// </summary>
        public BrushShape Shape { get; set; } = BrushShape.Circle;
    }

    /// <summary>
    /// 笔刷衰减类型
    /// </summary>
    public enum BrushFalloff
    {
        /// <summary>线性衰减</summary>
        Linear,
        /// <summary>平滑衰减</summary>
        Smooth,
        /// <summary>球形衰减</summary>
        Spherical,
        /// <summary>锐利边缘</summary>
        Sharp
    }

    /// <summary>
    /// 笔刷形状
    /// </summary>
    public enum BrushShape
    {
        /// <summary>圆形</summary>
        Circle,
        /// <summary>方形</summary>
        Square
    }
}
