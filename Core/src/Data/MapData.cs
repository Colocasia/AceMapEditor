// AceMapEditor/Core/src/Data/MapData.cs
// 地图数据模型
// Author: AceMapEditor Team
// Date: 2025-01-19

using System.Collections.Generic;
using System.Numerics;

namespace AceMapEditor.Core.Data
{
    /// <summary>
    /// 地图数据，包含地图的所有信息
    /// </summary>
    public class MapData
    {
        /// <summary>
        /// 地图名称
        /// </summary>
        public string Name { get; set; } = "New Map";

        /// <summary>
        /// 地图描述
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// 地图尺寸（宽度，单位：格子数）
        /// </summary>
        public int Width { get; set; } = 256;

        /// <summary>
        /// 地图尺寸（高度，单位：格子数）
        /// </summary>
        public int Height { get; set; } = 256;

        /// <summary>
        /// 地形数据
        /// </summary>
        public TerrainData? Terrain { get; set; }

        /// <summary>
        /// 单位列表
        /// </summary>
        public List<UnitData> Units { get; set; } = new List<UnitData>();

        /// <summary>
        /// 装饰物列表
        /// </summary>
        public List<DoodadData> Doodads { get; set; } = new List<DoodadData>();

        /// <summary>
        /// 区域列表
        /// </summary>
        public List<RegionData> Regions { get; set; } = new List<RegionData>();

        /// <summary>
        /// 玩家列表
        /// </summary>
        public List<PlayerData> Players { get; set; } = new List<PlayerData>();

        /// <summary>
        /// 触发器列表
        /// </summary>
        public List<TriggerData> Triggers { get; set; } = new List<TriggerData>();

        /// <summary>
        /// 地图版本
        /// </summary>
        public int Version { get; set; } = 1;

        /// <summary>
        /// 创建时间戳（Unix时间戳）
        /// </summary>
        public long CreatedTimestamp { get; set; }

        /// <summary>
        /// 最后修改时间戳（Unix时间戳）
        /// </summary>
        public long ModifiedTimestamp { get; set; }
    }

    /// <summary>
    /// 地形数据
    /// </summary>
    public class TerrainData
    {
        /// <summary>
        /// 高度图（一维数组，[y * width + x]）
        /// </summary>
        public float[] HeightMap { get; set; } = System.Array.Empty<float>();

        /// <summary>
        /// 纹理权重图列表（每层一个）
        /// </summary>
        public List<SplatmapData> Splatmaps { get; set; } = new List<SplatmapData>();

        /// <summary>
        /// 地形分辨率（顶点数）
        /// </summary>
        public int Resolution { get; set; } = 257; // 256x256的地形需要257x257个顶点

        /// <summary>
        /// 每个格子的尺寸（世界空间单位）
        /// </summary>
        public float CellSize { get; set; } = 1.0f;

        /// <summary>
        /// 高度缩放系数
        /// </summary>
        public float HeightScale { get; set; } = 100.0f;
    }

    /// <summary>
    /// 纹理权重图数据
    /// </summary>
    public class SplatmapData
    {
        /// <summary>
        /// 纹理层索引
        /// </summary>
        public int LayerIndex { get; set; }

        /// <summary>
        /// 纹理资源路径
        /// </summary>
        public string TexturePath { get; set; } = "";

        /// <summary>
        /// 权重数据（一维数组，值范围0-1）
        /// </summary>
        public float[] Weights { get; set; } = System.Array.Empty<float>();

        /// <summary>
        /// 平铺缩放
        /// </summary>
        public Vector2 TilingScale { get; set; } = Vector2.One;
    }

    /// <summary>
    /// 单位数据
    /// </summary>
    public class UnitData
    {
        /// <summary>
        /// 单位唯一ID
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// 单位定义ID（引用UnitDefinition）
        /// </summary>
        public string DefinitionId { get; set; } = "";

        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// 旋转（欧拉角，度数）
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// 缩放
        /// </summary>
        public Vector3 Scale { get; set; } = Vector3.One;

        /// <summary>
        /// 所属玩家索引
        /// </summary>
        public int PlayerIndex { get; set; } = 0;

        /// <summary>
        /// 自定义属性（键值对）
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 装饰物数据
    /// </summary>
    public class DoodadData
    {
        /// <summary>
        /// 装饰物唯一ID
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// 装饰物定义ID
        /// </summary>
        public string DefinitionId { get; set; } = "";

        /// <summary>
        /// 位置
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// 旋转（欧拉角，度数）
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// 缩放
        /// </summary>
        public Vector3 Scale { get; set; } = Vector3.One;

        /// <summary>
        /// 颜色调制
        /// </summary>
        public Vector4 ColorTint { get; set; } = Vector4.One;
    }

    /// <summary>
    /// 区域数据
    /// </summary>
    public class RegionData
    {
        /// <summary>
        /// 区域名称
        /// </summary>
        public string Name { get; set; } = "Region";

        /// <summary>
        /// 区域类型
        /// </summary>
        public RegionType Type { get; set; } = RegionType.Rectangle;

        /// <summary>
        /// 中心位置
        /// </summary>
        public Vector3 Center { get; set; }

        /// <summary>
        /// 尺寸（对矩形：宽高；对圆形：半径存在x）
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// 多边形顶点（仅用于多边形类型）
        /// </summary>
        public List<Vector3> PolygonPoints { get; set; } = new List<Vector3>();

        /// <summary>
        /// 区域颜色
        /// </summary>
        public Vector4 Color { get; set; } = new Vector4(0, 1, 0, 0.3f); // 半透明绿色
    }

    /// <summary>
    /// 区域类型
    /// </summary>
    public enum RegionType
    {
        /// <summary>矩形区域</summary>
        Rectangle,
        /// <summary>圆形区域</summary>
        Circle,
        /// <summary>多边形区域</summary>
        Polygon
    }

    /// <summary>
    /// 玩家数据
    /// </summary>
    public class PlayerData
    {
        /// <summary>
        /// 玩家索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 玩家名称
        /// </summary>
        public string Name { get; set; } = "Player";

        /// <summary>
        /// 玩家颜色
        /// </summary>
        public Vector4 Color { get; set; } = Vector4.One;

        /// <summary>
        /// 起始位置
        /// </summary>
        public Vector3 StartPosition { get; set; }

        /// <summary>
        /// 队伍
        /// </summary>
        public int Team { get; set; } = 0;

        /// <summary>
        /// 玩家类型
        /// </summary>
        public PlayerType Type { get; set; } = PlayerType.User;
    }

    /// <summary>
    /// 玩家类型
    /// </summary>
    public enum PlayerType
    {
        /// <summary>用户控制</summary>
        User,
        /// <summary>AI</summary>
        Computer,
        /// <summary>中立</summary>
        Neutral
    }

    /// <summary>
    /// 触发器数据（简化版，第四阶段详细实现）
    /// </summary>
    public class TriggerData
    {
        /// <summary>
        /// 触发器名称
        /// </summary>
        public string Name { get; set; } = "Trigger";

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 注释
        /// </summary>
        public string Comment { get; set; } = "";
    }
}
