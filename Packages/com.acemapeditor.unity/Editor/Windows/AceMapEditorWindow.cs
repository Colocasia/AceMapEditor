// AceMapEditorWindow.cs
// 主编辑器窗口
// Author: AceMapEditor Team
// Date: 2025-01-19

using UnityEditor;
using UnityEngine;
using AceMapEditor.Core.Editor;
using AceMapEditor.Core.Data;
using AceMapEditor.Unity.Editor.Adapters;

namespace AceMapEditor.Unity.Editor.Windows
{
    /// <summary>
    /// AceMapEditor主编辑器窗口
    /// </summary>
    public class AceMapEditorWindow : EditorWindow
    {
        private MapEditor? mapEditor;
        private UnitySceneViewAdapter? sceneViewAdapter;
        private UnityGizmosAdapter? gizmosAdapter;
        private UnityInputAdapter? inputAdapter;
        private UnityEditorUIAdapter? uiAdapter;

        // UI状态
        private Vector2 scrollPosition;
        private bool showTerrainSettings = true;
        private bool showBrushSettings = true;
        private bool showMapInfo = true;

        /// <summary>
        /// 打开编辑器窗口的菜单项
        /// </summary>
        [MenuItem("Window/Ace Map Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<AceMapEditorWindow>("Ace Map Editor");
            window.minSize = new Vector2(300, 400);
            window.Show();
        }

        private void OnEnable()
        {
            // 初始化适配器
            uiAdapter = new UnityEditorUIAdapter();
            gizmosAdapter = new UnityGizmosAdapter();
            inputAdapter = new UnityInputAdapter();

            // 注册SceneView回调
            SceneView.duringSceneGui += OnSceneGUI;

            uiAdapter.LogInfo("AceMapEditor window opened");
        }

        private void OnDisable()
        {
            // 取消注册
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            DrawMenuBar();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            DrawMapInfo();
            DrawToolbar();
            DrawBrushSettings();
            DrawTerrainSettings();

            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// 绘制菜单栏
        /// </summary>
        private void DrawMenuBar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            // 文件菜单
            if (GUILayout.Button("File", EditorStyles.toolbarDropDown, GUILayout.Width(50)))
            {
                ShowFileMenu();
            }

            // 编辑菜单
            if (GUILayout.Button("Edit", EditorStyles.toolbarDropDown, GUILayout.Width(50)))
            {
                ShowEditMenu();
            }

            // 视图菜单
            if (GUILayout.Button("View", EditorStyles.toolbarDropDown, GUILayout.Width(50)))
            {
                ShowViewMenu();
            }

            // 工具菜单
            if (GUILayout.Button("Tools", EditorStyles.toolbarDropDown, GUILayout.Width(50)))
            {
                ShowToolsMenu();
            }

            GUILayout.FlexibleSpace();

            // 当前地图状态
            if (mapEditor?.State.CurrentMap != null)
            {
                string dirtyMark = mapEditor.State.IsDirty ? "*" : "";
                GUILayout.Label($"{mapEditor.State.CurrentMap.Name}{dirtyMark}", EditorStyles.toolbarButton);
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 显示文件菜单
        /// </summary>
        private void ShowFileMenu()
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("New Map"), false, OnNewMap);
            menu.AddItem(new GUIContent("Open Map"), false, OnOpenMap);
            menu.AddSeparator("");

            bool hasMap = mapEditor?.State.CurrentMap != null;
            if (hasMap)
            {
                menu.AddItem(new GUIContent("Save"), false, OnSaveMap);
                menu.AddItem(new GUIContent("Save As..."), false, OnSaveMapAs);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Save"));
                menu.AddDisabledItem(new GUIContent("Save As..."));
            }

            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Exit"), false, () => Close());

            menu.ShowAsContext();
        }

        /// <summary>
        /// 显示编辑菜单
        /// </summary>
        private void ShowEditMenu()
        {
            GenericMenu menu = new GenericMenu();

            menu.AddDisabledItem(new GUIContent("Undo (Ctrl+Z)"));
            menu.AddDisabledItem(new GUIContent("Redo (Ctrl+Y)"));
            menu.AddSeparator("");
            menu.AddDisabledItem(new GUIContent("Copy"));
            menu.AddDisabledItem(new GUIContent("Paste"));
            menu.AddDisabledItem(new GUIContent("Delete"));

            menu.ShowAsContext();
        }

        /// <summary>
        /// 显示视图菜单
        /// </summary>
        private void ShowViewMenu()
        {
            GenericMenu menu = new GenericMenu();

            bool showGrid = mapEditor?.State.ShowGrid ?? true;
            bool showRegions = mapEditor?.State.ShowRegions ?? true;
            bool showUnits = mapEditor?.State.ShowUnits ?? true;

            menu.AddItem(new GUIContent("Show Grid"), showGrid, () =>
            {
                if (mapEditor != null) mapEditor.State.ShowGrid = !mapEditor.State.ShowGrid;
            });

            menu.AddItem(new GUIContent("Show Regions"), showRegions, () =>
            {
                if (mapEditor != null) mapEditor.State.ShowRegions = !mapEditor.State.ShowRegions;
            });

            menu.AddItem(new GUIContent("Show Units"), showUnits, () =>
            {
                if (mapEditor != null) mapEditor.State.ShowUnits = !mapEditor.State.ShowUnits;
            });

            menu.ShowAsContext();
        }

        /// <summary>
        /// 显示工具菜单
        /// </summary>
        private void ShowToolsMenu()
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Terrain Editor"), false, () => { });
            menu.AddItem(new GUIContent("Unit Editor"), false, () => { });
            menu.AddItem(new GUIContent("Trigger Editor"), false, () => { });
            menu.AddItem(new GUIContent("Data Editor"), false, () => { });

            menu.ShowAsContext();
        }

        /// <summary>
        /// 绘制地图信息
        /// </summary>
        private void DrawMapInfo()
        {
            showMapInfo = EditorGUILayout.BeginFoldoutHeaderGroup(showMapInfo, "Map Info");

            if (showMapInfo)
            {
                if (mapEditor?.State.CurrentMap == null)
                {
                    EditorGUILayout.HelpBox("No map loaded. Create a new map or open an existing one.", MessageType.Info);

                    if (GUILayout.Button("Create New Map", GUILayout.Height(30)))
                    {
                        OnNewMap();
                    }
                }
                else
                {
                    var map = mapEditor.State.CurrentMap;

                    EditorGUILayout.LabelField("Name", map.Name);
                    EditorGUILayout.LabelField("Size", $"{map.Width} x {map.Height}");
                    EditorGUILayout.LabelField("Units", map.Units.Count.ToString());
                    EditorGUILayout.LabelField("Regions", map.Regions.Count.ToString());
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        /// <summary>
        /// 绘制工具栏
        /// </summary>
        private void DrawToolbar()
        {
            if (mapEditor?.State.CurrentMap == null)
                return;

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);

            // 选择工具
            var currentTool = mapEditor.State.CurrentTool;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Toggle(currentTool == EditorTool.Select, "Select (Q)", "Button"))
                mapEditor.State.CurrentTool = EditorTool.Select;

            if (GUILayout.Toggle(currentTool == EditorTool.Move, "Move (W)", "Button"))
                mapEditor.State.CurrentTool = EditorTool.Move;

            if (GUILayout.Toggle(currentTool == EditorTool.Rotate, "Rotate (E)", "Button"))
                mapEditor.State.CurrentTool = EditorTool.Rotate;

            if (GUILayout.Toggle(currentTool == EditorTool.Scale, "Scale (R)", "Button"))
                mapEditor.State.CurrentTool = EditorTool.Scale;

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Terrain Tools", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Toggle(currentTool == EditorTool.TerrainRaise, "Raise", "Button"))
                mapEditor.State.CurrentTool = EditorTool.TerrainRaise;

            if (GUILayout.Toggle(currentTool == EditorTool.TerrainLower, "Lower", "Button"))
                mapEditor.State.CurrentTool = EditorTool.TerrainLower;

            if (GUILayout.Toggle(currentTool == EditorTool.TerrainSmooth, "Smooth", "Button"))
                mapEditor.State.CurrentTool = EditorTool.TerrainSmooth;

            if (GUILayout.Toggle(currentTool == EditorTool.TexturePaint, "Paint", "Button"))
                mapEditor.State.CurrentTool = EditorTool.TexturePaint;

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制笔刷设置
        /// </summary>
        private void DrawBrushSettings()
        {
            if (mapEditor?.State.CurrentMap == null)
                return;

            EditorGUILayout.Space(10);
            showBrushSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showBrushSettings, "Brush Settings");

            if (showBrushSettings)
            {
                var brush = mapEditor.State.BrushSettings;

                brush.Radius = EditorGUILayout.Slider("Radius", brush.Radius, 1f, 50f);
                brush.Strength = EditorGUILayout.Slider("Strength", brush.Strength, 0f, 1f);
                brush.Shape = (BrushShape)EditorGUILayout.EnumPopup("Shape", brush.Shape);
                brush.Falloff = (BrushFalloff)EditorGUILayout.EnumPopup("Falloff", brush.Falloff);
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        /// <summary>
        /// 绘制地形设置
        /// </summary>
        private void DrawTerrainSettings()
        {
            if (mapEditor?.State.CurrentMap?.Terrain == null)
                return;

            EditorGUILayout.Space(10);
            showTerrainSettings = EditorGUILayout.BeginFoldoutHeaderGroup(showTerrainSettings, "Terrain Settings");

            if (showTerrainSettings)
            {
                var terrain = mapEditor.State.CurrentMap.Terrain;

                EditorGUILayout.LabelField("Resolution", terrain.Resolution.ToString());
                terrain.CellSize = EditorGUILayout.FloatField("Cell Size", terrain.CellSize);
                terrain.HeightScale = EditorGUILayout.FloatField("Height Scale", terrain.HeightScale);
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        /// <summary>
        /// SceneView GUI回调
        /// </summary>
        private void OnSceneGUI(SceneView sceneView)
        {
            // 初始化适配器（首次调用）
            if (sceneViewAdapter == null)
            {
                sceneViewAdapter = new UnitySceneViewAdapter(sceneView);
            }

            // 初始化MapEditor
            if (mapEditor == null && sceneViewAdapter != null && gizmosAdapter != null && inputAdapter != null && uiAdapter != null)
            {
                mapEditor = new MapEditor(sceneViewAdapter, gizmosAdapter, inputAdapter, uiAdapter);
            }

            // 更新编辑器
            mapEditor?.Update();

            // 绘制Gizmos
            mapEditor?.OnDrawGizmos();

            // 如果正在编辑，阻止默认的场景选择
            if (mapEditor?.State.CurrentMap != null && mapEditor.State.CurrentTool != EditorTool.None)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }
        }

        // ========== 菜单命令 ==========

        private void OnNewMap()
        {
            // TODO: 显示新建地图对话框
            if (uiAdapter == null || sceneViewAdapter == null || gizmosAdapter == null || inputAdapter == null)
                return;

            if (mapEditor == null)
            {
                mapEditor = new MapEditor(sceneViewAdapter, gizmosAdapter, inputAdapter, uiAdapter);
            }

            mapEditor.NewMap(256, 256);
            Repaint();
        }

        private void OnOpenMap()
        {
            // TODO: 实现打开地图功能（第六阶段）
            uiAdapter?.ShowMessage("Open Map", "Open map functionality will be implemented in Phase 6.", Core.Interfaces.MessageType.Info);
        }

        private void OnSaveMap()
        {
            // TODO: 实现保存地图功能（第六阶段）
            uiAdapter?.ShowMessage("Save Map", "Save map functionality will be implemented in Phase 6.", Core.Interfaces.MessageType.Info);
        }

        private void OnSaveMapAs()
        {
            // TODO: 实现另存为功能（第六阶段）
            uiAdapter?.ShowMessage("Save Map As", "Save As functionality will be implemented in Phase 6.", Core.Interfaces.MessageType.Info);
        }
    }
}
