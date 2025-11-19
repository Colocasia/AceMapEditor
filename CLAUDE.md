# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

---

## 项目概述

**AceMapEditor** 是一个类似星际2银河编辑器的游戏地图编辑器，以 Unity Editor Extension（插件）形式开发。

**核心特点**:
- Unity 编辑器扩展插件
- C# 技术栈
- URP 渲染管线（Universal Render Pipeline）
- 架构设计支持未来移植到 Godot（引擎抽象层）
- 地形编辑、单位放置、触发器系统、数据编辑等完整功能

---

## 架构原则

### 分层架构（关键）
项目采用**分层架构**，核心逻辑与 Unity 引擎解耦，确保可移植性：

```
UI Layer (Unity EditorWindow)         ← Unity 特定
    ↓
Presentation Layer (ViewModel)        ← 可移植
    ↓
Business Logic Layer                  ← 可移植
    ↓
Engine Abstraction Layer (接口)       ← 抽象层
    ↓
Unity Implementation                  ← Unity 特定
```

### 代码组织规则

1. **核心逻辑 (`Core/src/`)**
   - **编译为DLL**：.NET Standard 2.1项目，编译后供各引擎引用
   - **零引擎依赖**：不得直接引用 `UnityEngine`、`Godot` 等任何引擎命名空间
   - **接口驱动**：必须通过接口与引擎交互
   - **使用System.Numerics**：使用 `System.Numerics.Vector3` 而非 `UnityEngine.Vector3`
   - **小型数据结构**：使用 struct 而非 class（高性能）

2. **Unity 实现 (`Packages/com.acemapeditor.unity/Editor/`)**
   - **引用Core DLL**：通过 Plugins/ 引用预编译的 Core.dll
   - **适配器实现**：实现Core定义的接口（如 `ISceneViewAdapter`）
   - **Unity特定代码**：EditorWindow、SceneView、Gizmos、Handles等
   - **命名空间**：`AceMapEditor.Unity.Editor`

3. **数据层 (`Core/src/Data/`)**
   - **标准序列化**：使用JSON格式（System.Text.Json + 源生成器）
   - **避免引擎类型**：不使用Unity/Godot特定类型
   - **使用System.Numerics**：Vector3、Vector4、Quaternion、Matrix4x4

4. **高性能代码**
   - **struct优先**：频繁使用的小型数据用 struct
   - **System.Numerics**：利用SIMD优化的数学类型
   - **批量处理**：使用 `NativeArray<T>`（Unity）或 `Span<T>`
   - **临时缓冲区**：`Span<T>` + `stackalloc`（栈分配）
   - **unsafe代码**：关键路径考虑 `unsafe` 指针

---

## 项目结构

**重要**：本项目采用 **DLL + UPM Package** 架构，Core编译为独立DLL供引擎层引用。

```
AceMapEditor/                              # 仓库根目录
│
├── Core/                                  # 核心逻辑层（.NET Standard 2.1 DLL）
│   ├── AceMapEditor.Core.csproj           # .NET 项目文件
│   ├── README.md                          # Core 层说明
│   ├── src/                               # 源代码（引擎无关）
│   │   ├── Data/                          # 数据模型
│   │   │   ├── MapData.cs                 # 地图数据
│   │   │   └── EditorState.cs             # 编辑器状态
│   │   ├── Editor/                        # 编辑器逻辑（纯C#）
│   │   │   └── MapEditor.cs               # 地图编辑器核心类
│   │   ├── Interfaces/                    # 引擎抽象接口
│   │   │   ├── ISceneViewAdapter.cs       # 场景视图接口
│   │   │   ├── IGizmosAdapter.cs          # Gizmos绘制接口
│   │   │   ├── IInputAdapter.cs           # 输入处理接口
│   │   │   └── IEditorUIAdapter.cs        # UI适配器接口
│   │   ├── Serialization/                 # 序列化（JSON）
│   │   └── Utilities/                     # 工具类
│   └── bin/Release/netstandard2.1/        # 编译输出
│       └── AceMapEditor.Core.dll          # 核心DLL（24KB）
│
├── Packages/com.acemapeditor.unity/       # Unity Package（UPM）
│   ├── package.json                       # UPM 配置文件
│   ├── Editor/                            # Unity 编辑器扩展
│   │   ├── AceMapEditor.Editor.asmdef     # Assembly Definition
│   │   ├── Adapters/                      # Unity 适配器实现
│   │   │   ├── UnitySceneViewAdapter.cs
│   │   │   ├── UnityGizmosAdapter.cs
│   │   │   ├── UnityInputAdapter.cs
│   │   │   └── UnityEditorUIAdapter.cs
│   │   ├── Windows/                       # 编辑器窗口
│   │   │   └── AceMapEditorWindow.cs      # 主编辑器窗口
│   │   ├── Tools/                         # SceneView 工具
│   │   ├── Inspectors/                    # 自定义 Inspector
│   │   └── SceneView/                     # SceneView 扩展
│   ├── Runtime/                           # Unity 运行时（可选）
│   │   ├── AceMapEditor.Runtime.asmdef
│   │   └── Components/                    # MonoBehaviour 组件
│   ├── Plugins/                           # 预编译DLL存放目录
│   │   ├── AceMapEditor.Core.dll          ← Core编译后复制到这里
│   │   └── AceMapEditor.Core.pdb          # 调试符号
│   ├── Shaders/                           # URP Shader
│   │   └── TerrainBlend.shader
│   ├── Resources/                         # Unity 资源
│   │   ├── Definitions/                   # 游戏数据定义（JSON）
│   │   └── Textures/
│   └── Tests/                             # 单元测试
│
├── Build/                                 # 构建脚本
│   └── build-core.sh                      # Core DLL 自动构建脚本
│
├── Releases/                              # DLL 发布目录
│   └── netstandard2.1/
│       ├── AceMapEditor.Core.dll
│       └── AceMapEditor.Core.pdb
│
├── Docs/                                  # 项目文档
│   ├── 需求文档.md                        # 功能需求和开发阶段
│   ├── 项目文档.md                        # 技术方案和架构
│   └── 代码规范.md                        # 代码规范和性能优化
│
└── [未来] Godot/                          # Godot项目（未来添加）
    ├── project.godot                      # Godot 项目文件
    ├── addons/acemapeditor/               # Godot 插件
    │   ├── plugin.cfg                     # Godot 插件配置
    │   ├── Libs/
    │   │   └── AceMapEditor.Core.dll      ← 复用同一个Core DLL
    │   ├── Adapters/                      # Godot版本的适配器
    │   │   ├── GodotSceneViewAdapter.cs   # 实现ISceneViewAdapter
    │   │   └── GodotGizmosAdapter.cs      # 实现IGizmosAdapter
    │   └── Editor/
    │       └── MapEditorDock.cs           # Godot编辑器Dock
    └── Scenes/                            # Godot 测试场景
```

---

## 开发指南

### 添加新功能时

1. **先定义接口**（`Core/Interfaces/`）
   ```csharp
   public interface INewFeatureAdapter
   {
       void DoSomething();
   }
   ```

2. **实现核心逻辑**（`Core/Editor/`）
   ```csharp
   public class NewFeatureEditor
   {
       private INewFeatureAdapter adapter;
       public NewFeatureEditor(INewFeatureAdapter adapter) 
       {
           this.adapter = adapter;
       }
   }
   ```

3. **Unity 适配器实现**（`Editor/Adapters/`）
   ```csharp
   public class UnityNewFeatureAdapter : INewFeatureAdapter
   {
       // Unity 特定实现
   }
   ```

4. **编辑器 UI**（`Editor/Windows/`）
   ```csharp
   public class NewFeatureWindow : EditorWindow
   {
       // IMGUI 或 UIElements
   }
   ```

### 数据持久化

- **地图文件格式**: `.acemap`（JSON 格式）
- **配置数据**: ScriptableObject（`Resources/Definitions/`）
- **序列化**: 使用 `Core/Serialization/MapSerializer.cs`

示例：
```csharp
var mapData = new MapData();
var serializer = new MapSerializer();
serializer.SaveMap("path/to/map.acemap", mapData);
```

### Unity 编辑器扩展常用 API

- **EditorWindow**: 主编辑器窗口
- **ScriptableObject**: 数据配置资源
- **Handles**: 场景视图 3D 绘制
- **Gizmos**: 场景视图辅助绘制
- **Undo API**: 撤销/重做系统
- **SceneView.duringSceneGui**: 场景视图事件回调

---

## 核心模块说明

### 1. 地形编辑器（Terrain Editor）
- **位置**: `Core/Editor/TerrainEditor.cs`
- **接口**: `ITerrainAdapter`, `ITextureBlendAdapter`
- **Unity 实现**: `Editor/Adapters/UnityTerrainAdapter.cs`
- **Shader**: `Shaders/TerrainBlend.shader`（URP自定义Shader）
- **功能**: 地形升降、多图层纹理混合、水面设置
- **技术方案**: 自定义Mesh + Splatmap权重图系统（不使用Unity Terrain）

### 2. 单位系统（Unit System）
- **位置**: `Core/Editor/UnitManager.cs`
- **数据**: `Core/Data/UnitData.cs`
- **定义**: `Resources/Definitions/Units/` (ScriptableObject)
- **功能**: 单位放置、属性编辑、分组管理

### 3. 触发器系统（Trigger System）
- **位置**: `Core/Editor/TriggerSystem.cs`
- **组件**: `TriggerEvent`, `TriggerCondition`, `TriggerAction`
- **编辑器**: `Editor/Windows/TriggerEditorWindow.cs`
- **功能**: 事件-条件-动作逻辑编程

### 4. 区域系统（Region System）
- **位置**: `Core/Editor/RegionManager.cs`
- **数据**: `Core/Data/RegionData.cs`
- **功能**: 矩形、圆形、多边形区域绘制

### 5. 数据编辑器（Data Editor）
- **位置**: `Editor/Windows/DataEditorWindow.cs`
- **定义**: ScriptableObject (单位、技能、装饰物等)
- **功能**: 游戏数据配置

---

## 代码规范

### 命名约定
- **类名**: PascalCase (`TerrainEditor`)
- **接口**: `I` 前缀 (`ITerrainAdapter`)
- **私有字段**: camelCase (`terrainData`)
- **属性**: PascalCase (`BrushSize`)
- **常量**: UPPER_SNAKE_CASE (`MAX_MAP_SIZE`)

### 高性能编程示例

```csharp
// ✅ 推荐：使用 readonly struct
public readonly struct Vector3f
{
    public readonly float x, y, z;

    public Vector3f(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

// ✅ 推荐：使用 in 参数避免复制
public void Transform(in Matrix4x4f matrix, in Vector3f position)
{
    // 零复制
}

// ✅ 推荐：使用 NativeArray 避免 GC
using var vertices = new NativeArray<Vector3>(count, Allocator.TempJob);

// ✅ 推荐：使用 Span<T> + stackalloc
Span<float> tempBuffer = stackalloc float[256];
```

### 注释规范
```csharp
/// <summary>
/// 地形编辑器核心类，负责地形高度和纹理编辑
/// </summary>
public class TerrainEditor
{
    /// <summary>
    /// 升高地形
    /// </summary>
    /// <param name="position">中心位置</param>
    /// <param name="radius">笔刷半径</param>
    /// <param name="strength">笔刷强度</param>
    public void RaiseTerrain(Vector2Int position, float radius, float strength)
    {
        // 实现...
    }
}
```

### 文件头注释
```csharp
// AceMapEditor/Core/Editor/TerrainEditor.cs
// 地形编辑器核心逻辑
// Author: [Your Name]
// Date: 2025-01-XX
```

**详细规范**: 参见 `Docs/代码规范.md`

---

## 测试

### 单元测试
- 使用 **Unity Test Framework**
- 测试目录: `Tests/Editor/` 和 `Tests/Runtime/`
- 运行测试: Window > General > Test Runner

示例：
```csharp
[Test]
public void TerrainEditor_RaiseTerrain_IncreasesHeight()
{
    var adapter = new MockTerrainAdapter();
    var editor = new TerrainEditor(adapter);
    
    editor.RaiseTerrain(new Vector2Int(10, 10), 5f, 1f);
    
    Assert.Greater(adapter.GetHeight(10, 10), 0);
}
```

---

## 常用开发任务

### 打开编辑器窗口
```
Unity 菜单: Window > AceMap Editor
```

### 创建新的单位定义
```
右键 Project 窗口 > Create > AceMap > Unit Definition
```

### 运行测试
```
Window > General > Test Runner > Run All
```

### 调试编辑器代码
1. 打开 Visual Studio
2. 附加到 Unity 进程（Attach to Unity）
3. 设置断点

---

## 依赖管理

### Unity 官方包（通过 Package Manager）
- `com.unity.mathematics` - 高性能数学库
- `com.unity.collections` - 高性能集合
- `com.unity.test-framework` - 单元测试

### 第三方库（可选）
- **Newtonsoft.Json**: 高级 JSON 序列化
- **Odin Inspector**: 强大的 Inspector 扩展（付费）

---

## 未来扩展：添加Godot支持

当需要添加Godot支持时，按以下步骤操作：

### 1. 创建Godot项目目录

在仓库根目录创建 `Godot/` 目录：

```bash
mkdir -p Godot/addons/acemapeditor/{Libs,Adapters,Editor}
```

### 2. 初始化Godot项目

```bash
cd Godot
# 创建Godot项目文件（或用Godot编辑器创建）
touch project.godot
```

### 3. 配置Godot插件

创建 `Godot/addons/acemapeditor/plugin.cfg`：

```ini
[plugin]
name="Ace Map Editor"
description="地图编辑器插件"
author="AceMapEditor Team"
version="0.1.0"
script="Editor/MapEditorPlugin.cs"
```

### 4. 复制Core DLL

**选项A：手动复制**
```bash
cp Releases/netstandard2.1/AceMapEditor.Core.dll Godot/addons/acemapeditor/Libs/
```

**选项B：修改构建脚本**
编辑 `Build/build-core.sh`，添加Godot目标：
```bash
# 复制到Godot插件
if [ -d "../Godot/addons/acemapeditor/Libs" ]; then
    cp $OUTPUT_DLL ../Godot/addons/acemapeditor/Libs/
    echo "✓ Copied to Godot addons"
fi
```

### 5. 实现Godot适配器

在 `Godot/addons/acemapeditor/Adapters/` 创建适配器：

**GodotSceneViewAdapter.cs**：
```csharp
using Godot;
using AceMapEditor.Core.Interfaces;
using System.Numerics;

public class GodotSceneViewAdapter : ISceneViewAdapter
{
    private readonly Node3D viewport;

    public GodotSceneViewAdapter(Node3D viewport)
    {
        this.viewport = viewport;
    }

    public (Vector3 origin, Vector3 direction) GetMouseRay(Vector2 mousePosition)
    {
        // 使用Godot Camera3D.ProjectRayOrigin/ProjectRayNormal
        // 转换Godot.Vector3 到 System.Numerics.Vector3
    }

    // 实现其他接口方法...
}
```

**GodotGizmosAdapter.cs**：
```csharp
public class GodotGizmosAdapter : IGizmosAdapter
{
    public void DrawLine(Vector3 start, Vector3 end)
    {
        // 使用Godot的DebugDraw或ImmediateMesh
    }

    // 实现其他接口方法...
}
```

### 6. 创建Godot编辑器Dock

**MapEditorDock.cs**：
```csharp
using Godot;
using AceMapEditor.Core.Editor;

[Tool]
public partial class MapEditorDock : Control
{
    private MapEditor mapEditor;

    public override void _Ready()
    {
        // 初始化适配器
        var sceneAdapter = new GodotSceneViewAdapter(...);
        var gizmosAdapter = new GodotGizmosAdapter(...);
        // ...

        // 创建MapEditor实例
        mapEditor = new MapEditor(sceneAdapter, gizmosAdapter, ...);
    }
}
```

### 7. 创建插件入口

**MapEditorPlugin.cs**：
```csharp
using Godot;

[Tool]
public partial class MapEditorPlugin : EditorPlugin
{
    private MapEditorDock dock;

    public override void _EnterTree()
    {
        dock = new MapEditorDock();
        AddControlToDock(DockSlot.LeftUl, dock);
    }

    public override void _ExitTree()
    {
        RemoveDock(dock);
        dock.QueueFree();
    }
}
```

### 8. 关键点

**DLL引用**：
- Godot 4.x支持.NET Standard 2.1
- 确保Godot项目配置为C#项目（.NET 6+）
- DLL放在Godot可识别的路径（如 `addons/*/Libs/`）

**类型转换**：
- `System.Numerics.Vector3` ↔ `Godot.Vector3`
- 需要手动转换（Godot不会自动转换）

**命名空间**：
- Godot适配器使用 `AceMapEditor.Godot.Editor` 命名空间
- 与Unity的命名空间区分开

### 9. 测试

```bash
# 在Godot编辑器中启用插件
# 项目 > 项目设置 > 插件 > Ace Map Editor > 启用
```

### 10. 优势

- ✅ **Core逻辑复用**：MapEditor、数据模型完全共享
- ✅ **独立开发**：Unity和Godot互不影响
- ✅ **同步更新**：修改Core DLL后两个引擎同时受益
- ✅ **代码量减少**：只需实现适配器层（约500-1000行）

---

## 重要注意事项

### 可移植性检查清单
在编写核心逻辑代码时，确保：
- [ ] 不直接使用 `UnityEngine.Vector3`（使用 `System.Numerics.Vector3`）
- [ ] 不直接调用 Unity/Godot API（通过接口）
- [ ] 数据结构可序列化为标准格式（JSON）
- [ ] 业务逻辑与 UI 逻辑分离

### 高性能编程原则

**数据结构选择**：
- 小型数据（<64字节）：使用 `readonly struct`
- 大型数据：使用 class
- 示例：`Vector3f`、`TerrainVertex` 用 struct

**内存优化**：
- 避免频繁分配：使用 `NativeArray<T>`（零GC）
- 临时数据：使用 `Span<T>` + `stackalloc`
- 对象池：频繁创建的对象使用对象池

**参数传递**：
- 大型 struct 用 `in` 参数（避免复制）
- 需要修改用 `ref` 参数
- 输出用 `out` 参数

**循环优化**：
- 优先使用 `for` 而非 `foreach`（避免GC）
- `Span<T>` 的 foreach 无GC

**Unsafe 代码**：
- 大批量数组操作考虑使用 `unsafe` 和指针
- 纹理处理使用 `fixed` 和指针

**详见**: `Docs/代码规范.md` 的"高性能编程"章节

### 地形系统性能
- 大型地形编辑使用 **Unity.Jobs** 多线程
- 延迟更新（批量修改后一次性应用）
- 地形分块处理（Chunk）
- Splatmap 操作使用 NativeArray

### 撤销/重做
- 使用 `Undo.RecordObject()` 记录对象修改
- 对于自定义数据，使用命令模式（Command Pattern）

---

## 参考文档

### 项目文档
- **需求文档**: `Docs/需求文档.md`
  - 功能需求和业务逻辑
  - 6个开发阶段规划
  - 功能优先级（P0-P3）

- **项目文档**: `Docs/项目文档.md`
  - 技术栈和架构设计
  - 核心系统技术方案
  - 文件格式和性能优化

- **代码规范**: `Docs/代码规范.md`
  - 命名约定和代码格式
  - **高性能编程**（Struct、指针、NativeArray、Span）
  - Unity特定规范
  - 可移植性规则

### 外部文档
- **Unity 官方文档**: https://docs.unity3d.com/Manual/ExtendingTheEditor.html
- **URP Shader文档**: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest
- **Unity.Collections**: https://docs.unity3d.com/Packages/com.unity.collections@latest
- **星际2银河编辑器**: 功能参考

---

## 开发阶段（当前状态）

**当前阶段**: 项目初始化  
**下一步**: 
1. 创建 Unity 项目
2. 搭建插件基础框架
3. 实现主编辑器窗口

---

## 联系和支持

如有问题，请查阅：
- `Docs/需求文档.md` - 功能需求和开发阶段规划
- `Docs/项目文档.md` - 技术实现和架构设计
- `Docs/代码规范.md` - 代码规范和高性能编程指南
- Unity 论坛社区
- 项目 Issue Tracker（如有）

---

## 文档管理规则

- Docs的目录结构需要保持简单
- 只在明确需要时创建新文档
- 新增内容前先询问应该添加到哪个现有文档
- 添加新文档需要找我确认