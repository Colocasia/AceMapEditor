// AceMapEditor/Core/src/Interfaces/ISceneViewAdapter.cs
// 场景视图适配器接口
// Author: AceMapEditor Team
// Date: 2025-01-19

using System;
using System.Numerics;

namespace AceMapEditor.Core.Interfaces
{
    /// <summary>
    /// 场景视图适配器接口
    /// 抽象不同引擎的场景视图交互
    /// </summary>
    public interface ISceneViewAdapter
    {
        /// <summary>
        /// 获取鼠标在场景中的射线
        /// </summary>
        /// <param name="mousePosition">鼠标屏幕坐标</param>
        /// <returns>射线（起点和方向）</returns>
        (Vector3 origin, Vector3 direction) GetMouseRay(Vector2 mousePosition);

        /// <summary>
        /// 射线与地形相交测试
        /// </summary>
        /// <param name="origin">射线起点</param>
        /// <param name="direction">射线方向</param>
        /// <param name="hitPoint">相交点</param>
        /// <returns>是否相交</returns>
        bool RaycastTerrain(Vector3 origin, Vector3 direction, out Vector3 hitPoint);

        /// <summary>
        /// 射线与平面相交测试
        /// </summary>
        /// <param name="origin">射线起点</param>
        /// <param name="direction">射线方向</param>
        /// <param name="planeNormal">平面法线</param>
        /// <param name="planePoint">平面上的点</param>
        /// <param name="hitPoint">相交点</param>
        /// <returns>是否相交</returns>
        bool RaycastPlane(Vector3 origin, Vector3 direction, Vector3 planeNormal, Vector3 planePoint, out Vector3 hitPoint);

        /// <summary>
        /// 重绘场景视图
        /// </summary>
        void RepaintSceneView();

        /// <summary>
        /// 获取场景视图摄像机位置
        /// </summary>
        Vector3 GetCameraPosition();

        /// <summary>
        /// 获取场景视图摄像机方向
        /// </summary>
        Vector3 GetCameraForward();
    }

    /// <summary>
    /// Gizmos绘制适配器接口
    /// </summary>
    public interface IGizmosAdapter
    {
        /// <summary>
        /// 设置绘制颜色
        /// </summary>
        void SetColor(Vector4 color);

        /// <summary>
        /// 绘制线段
        /// </summary>
        void DrawLine(Vector3 start, Vector3 end);

        /// <summary>
        /// 绘制线段列表
        /// </summary>
        void DrawLines(Vector3[] points);

        /// <summary>
        /// 绘制圆形（3D空间）
        /// </summary>
        void DrawCircle(Vector3 center, Vector3 normal, float radius, int segments = 32);

        /// <summary>
        /// 绘制球体线框
        /// </summary>
        void DrawWireSphere(Vector3 center, float radius);

        /// <summary>
        /// 绘制立方体线框
        /// </summary>
        void DrawWireCube(Vector3 center, Vector3 size);

        /// <summary>
        /// 绘制实心圆盘（用于笔刷预览）
        /// </summary>
        void DrawSolidDisc(Vector3 center, Vector3 normal, float radius);

        /// <summary>
        /// 绘制文字标签
        /// </summary>
        void DrawLabel(Vector3 position, string text);

        /// <summary>
        /// 绘制箭头
        /// </summary>
        void DrawArrow(Vector3 start, Vector3 end, float arrowHeadLength = 0.25f);
    }

    /// <summary>
    /// 输入适配器接口
    /// </summary>
    public interface IInputAdapter
    {
        /// <summary>
        /// 获取当前鼠标位置（屏幕坐标）
        /// </summary>
        Vector2 GetMousePosition();

        /// <summary>
        /// 检查鼠标按键是否按下
        /// </summary>
        bool IsMouseButtonDown(int button);

        /// <summary>
        /// 检查鼠标按键是否持续按住
        /// </summary>
        bool IsMouseButton(int button);

        /// <summary>
        /// 检查鼠标按键是否释放
        /// </summary>
        bool IsMouseButtonUp(int button);

        /// <summary>
        /// 获取鼠标滚轮增量
        /// </summary>
        float GetMouseScrollDelta();

        /// <summary>
        /// 检查键盘按键是否按下
        /// </summary>
        bool IsKeyDown(KeyCode key);

        /// <summary>
        /// 检查键盘按键是否持续按住
        /// </summary>
        bool IsKey(KeyCode key);

        /// <summary>
        /// 检查Shift键是否按住
        /// </summary>
        bool IsShiftHeld();

        /// <summary>
        /// 检查Ctrl键是否按住
        /// </summary>
        bool IsControlHeld();

        /// <summary>
        /// 检查Alt键是否按住
        /// </summary>
        bool IsAltHeld();
    }

    /// <summary>
    /// 键码枚举（引擎无关）
    /// </summary>
    public enum KeyCode
    {
        /// <summary>无按键</summary>
        None = 0,
        /// <summary>退格键</summary>
        Backspace = 8,
        /// <summary>Tab键</summary>
        Tab = 9,
        /// <summary>回车键</summary>
        Return = 13,
        /// <summary>Esc键</summary>
        Escape = 27,
        /// <summary>空格键</summary>
        Space = 32,
        /// <summary>Delete键</summary>
        Delete = 127,

        /// <summary>A键</summary>
        A = 65,
        /// <summary>B键</summary>
        B,
        /// <summary>C键</summary>
        C,
        /// <summary>D键</summary>
        D,
        /// <summary>E键</summary>
        E,
        /// <summary>F键</summary>
        F,
        /// <summary>G键</summary>
        G,
        /// <summary>H键</summary>
        H,
        /// <summary>I键</summary>
        I,
        /// <summary>J键</summary>
        J,
        /// <summary>K键</summary>
        K,
        /// <summary>L键</summary>
        L,
        /// <summary>M键</summary>
        M,
        /// <summary>N键</summary>
        N,
        /// <summary>O键</summary>
        O,
        /// <summary>P键</summary>
        P,
        /// <summary>Q键</summary>
        Q,
        /// <summary>R键</summary>
        R,
        /// <summary>S键</summary>
        S,
        /// <summary>T键</summary>
        T,
        /// <summary>U键</summary>
        U,
        /// <summary>V键</summary>
        V,
        /// <summary>W键</summary>
        W,
        /// <summary>X键</summary>
        X,
        /// <summary>Y键</summary>
        Y,
        /// <summary>Z键</summary>
        Z,

        /// <summary>数字0键</summary>
        Alpha0 = 48,
        /// <summary>数字1键</summary>
        Alpha1,
        /// <summary>数字2键</summary>
        Alpha2,
        /// <summary>数字3键</summary>
        Alpha3,
        /// <summary>数字4键</summary>
        Alpha4,
        /// <summary>数字5键</summary>
        Alpha5,
        /// <summary>数字6键</summary>
        Alpha6,
        /// <summary>数字7键</summary>
        Alpha7,
        /// <summary>数字8键</summary>
        Alpha8,
        /// <summary>数字9键</summary>
        Alpha9,

        /// <summary>左方向键</summary>
        LeftArrow = 37,
        /// <summary>上方向键</summary>
        UpArrow = 38,
        /// <summary>右方向键</summary>
        RightArrow = 39,
        /// <summary>下方向键</summary>
        DownArrow = 40,

        /// <summary>F1键</summary>
        F1 = 112,
        /// <summary>F2键</summary>
        F2,
        /// <summary>F3键</summary>
        F3,
        /// <summary>F4键</summary>
        F4,
        /// <summary>F5键</summary>
        F5,
        /// <summary>F6键</summary>
        F6,
        /// <summary>F7键</summary>
        F7,
        /// <summary>F8键</summary>
        F8,
        /// <summary>F9键</summary>
        F9,
        /// <summary>F10键</summary>
        F10,
        /// <summary>F11键</summary>
        F11,
        /// <summary>F12键</summary>
        F12
    }
}
