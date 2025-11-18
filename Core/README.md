# AceMapEditor.Core

**Engine-agnostic core logic library for Ace Map Editor**

这是AceMapEditor的核心逻辑库，完全引擎无关，使用纯C#实现。

## 特性

- ✅ **引擎无关**：不依赖Unity、Godot或任何游戏引擎
- ✅ **.NET Standard 2.1**：可在任何支持.NET Standard 2.1的平台运行
- ✅ **高性能**：支持unsafe代码、指针、struct优化
- ✅ **可移植**：可被Unity、Godot等多个引擎使用
- ✅ **类型安全**：启用Nullable引用类型

## 项目结构

```
Core/
├── AceMapEditor.Core.csproj    # .NET Standard 2.1项目文件
├── src/
│   ├── Interfaces/             # 引擎抽象接口
│   ├── Data/                   # 数据模型
│   ├── Editor/                 # 编辑器核心逻辑
│   ├── Serialization/          # 序列化
│   └── Utilities/              # 工具类
└── README.md
```

## 编译

### 要求
- .NET SDK 6.0 或更高版本

### 编译命令

```bash
# Debug编译
dotnet build

# Release编译
dotnet build -c Release

# 输出路径
# bin/Debug/netstandard2.1/AceMapEditor.Core.dll
# bin/Release/netstandard2.1/AceMapEditor.Core.dll
```

### 快速编译脚本

```bash
# 使用项目根目录的编译脚本
cd ..
./Build/build-core.sh
```

## 使用

### Unity项目

1. 编译Core.dll
2. 将`bin/Release/netstandard2.1/AceMapEditor.Core.dll`复制到Unity项目的`Plugins/`目录
3. 引用Core的接口实现Unity适配器

### Godot项目

1. 编译Core.dll
2. 将DLL复制到Godot项目的`.mono/assemblies/`目录
3. 在C#脚本中引用并实现适配器

## 架构设计

Core库采用**接口驱动设计**：

```csharp
// Core定义接口
public interface ITerrainAdapter
{
    void UpdateMesh(VertexData[] vertices);
}

// Unity实现
public class UnityTerrainAdapter : ITerrainAdapter
{
    // Unity特定实现
}

// Godot实现
public class GodotTerrainAdapter : ITerrainAdapter
{
    // Godot特定实现
}
```

## 开发指南

- **禁止引用引擎库**：Core中不得添加Unity、Godot等引擎的依赖
- **使用自定义类型**：使用`Vector3f`而非`UnityEngine.Vector3`
- **高性能编程**：优先使用`readonly struct`、指针等高性能特性
- **完整文档**：所有公共API需要XML文档注释

## 许可证

MIT License - 详见根目录的LICENSE.md

## 相关项目

- [AceMapEditor.Unity](../Packages/com.acemapeditor.unity/) - Unity实现
- [AceMapEditor.Godot](../Godot/) - Godot实现（未来）
