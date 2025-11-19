// UnityInputAdapter.cs
// Unity输入适配器实现
// Author: AceMapEditor Team
// Date: 2025-01-19

using System.Numerics;
using UnityEngine;
using AceMapEditor.Core.Interfaces;
using Vector2 = System.Numerics.Vector2;
using KeyCode = AceMapEditor.Core.Interfaces.KeyCode;

namespace AceMapEditor.Unity.Editor.Adapters
{
    /// <summary>
    /// Unity输入适配器
    /// </summary>
    public class UnityInputAdapter : IInputAdapter
    {
        public Vector2 GetMousePosition()
        {
            var mousePos = Event.current?.mousePosition ?? UnityEngine.Vector2.zero;
            return new Vector2(mousePos.x, mousePos.y);
        }

        public bool IsMouseButtonDown(int button)
        {
            return Event.current != null &&
                   Event.current.type == EventType.MouseDown &&
                   Event.current.button == button;
        }

        public bool IsMouseButton(int button)
        {
            return Event.current != null &&
                   (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseDown) &&
                   Event.current.button == button;
        }

        public bool IsMouseButtonUp(int button)
        {
            return Event.current != null &&
                   Event.current.type == EventType.MouseUp &&
                   Event.current.button == button;
        }

        public float GetMouseScrollDelta()
        {
            if (Event.current != null && Event.current.type == EventType.ScrollWheel)
            {
                return Event.current.delta.y;
            }
            return 0f;
        }

        public bool IsKeyDown(KeyCode key)
        {
            return Event.current != null &&
                   Event.current.type == EventType.KeyDown &&
                   ConvertKeyCode(Event.current.keyCode) == key;
        }

        public bool IsKey(KeyCode key)
        {
            return Event.current != null &&
                   (Event.current.type == EventType.KeyDown || Event.current.type == EventType.KeyUp) &&
                   ConvertKeyCode(Event.current.keyCode) == key;
        }

        public bool IsShiftHeld()
        {
            return Event.current != null && Event.current.shift;
        }

        public bool IsControlHeld()
        {
            return Event.current != null && Event.current.control;
        }

        public bool IsAltHeld()
        {
            return Event.current != null && Event.current.alt;
        }

        /// <summary>
        /// 转换Unity KeyCode到引擎无关的KeyCode
        /// </summary>
        private static KeyCode ConvertKeyCode(UnityEngine.KeyCode unityKey)
        {
            return unityKey switch
            {
                UnityEngine.KeyCode.Backspace => KeyCode.Backspace,
                UnityEngine.KeyCode.Tab => KeyCode.Tab,
                UnityEngine.KeyCode.Return => KeyCode.Return,
                UnityEngine.KeyCode.Escape => KeyCode.Escape,
                UnityEngine.KeyCode.Space => KeyCode.Space,
                UnityEngine.KeyCode.Delete => KeyCode.Delete,

                // 字母
                UnityEngine.KeyCode.A => KeyCode.A,
                UnityEngine.KeyCode.B => KeyCode.B,
                UnityEngine.KeyCode.C => KeyCode.C,
                UnityEngine.KeyCode.D => KeyCode.D,
                UnityEngine.KeyCode.E => KeyCode.E,
                UnityEngine.KeyCode.F => KeyCode.F,
                UnityEngine.KeyCode.G => KeyCode.G,
                UnityEngine.KeyCode.H => KeyCode.H,
                UnityEngine.KeyCode.I => KeyCode.I,
                UnityEngine.KeyCode.J => KeyCode.J,
                UnityEngine.KeyCode.K => KeyCode.K,
                UnityEngine.KeyCode.L => KeyCode.L,
                UnityEngine.KeyCode.M => KeyCode.M,
                UnityEngine.KeyCode.N => KeyCode.N,
                UnityEngine.KeyCode.O => KeyCode.O,
                UnityEngine.KeyCode.P => KeyCode.P,
                UnityEngine.KeyCode.Q => KeyCode.Q,
                UnityEngine.KeyCode.R => KeyCode.R,
                UnityEngine.KeyCode.S => KeyCode.S,
                UnityEngine.KeyCode.T => KeyCode.T,
                UnityEngine.KeyCode.U => KeyCode.U,
                UnityEngine.KeyCode.V => KeyCode.V,
                UnityEngine.KeyCode.W => KeyCode.W,
                UnityEngine.KeyCode.X => KeyCode.X,
                UnityEngine.KeyCode.Y => KeyCode.Y,
                UnityEngine.KeyCode.Z => KeyCode.Z,

                // 数字
                UnityEngine.KeyCode.Alpha0 => KeyCode.Alpha0,
                UnityEngine.KeyCode.Alpha1 => KeyCode.Alpha1,
                UnityEngine.KeyCode.Alpha2 => KeyCode.Alpha2,
                UnityEngine.KeyCode.Alpha3 => KeyCode.Alpha3,
                UnityEngine.KeyCode.Alpha4 => KeyCode.Alpha4,
                UnityEngine.KeyCode.Alpha5 => KeyCode.Alpha5,
                UnityEngine.KeyCode.Alpha6 => KeyCode.Alpha6,
                UnityEngine.KeyCode.Alpha7 => KeyCode.Alpha7,
                UnityEngine.KeyCode.Alpha8 => KeyCode.Alpha8,
                UnityEngine.KeyCode.Alpha9 => KeyCode.Alpha9,

                // 方向键
                UnityEngine.KeyCode.LeftArrow => KeyCode.LeftArrow,
                UnityEngine.KeyCode.UpArrow => KeyCode.UpArrow,
                UnityEngine.KeyCode.RightArrow => KeyCode.RightArrow,
                UnityEngine.KeyCode.DownArrow => KeyCode.DownArrow,

                // 功能键
                UnityEngine.KeyCode.F1 => KeyCode.F1,
                UnityEngine.KeyCode.F2 => KeyCode.F2,
                UnityEngine.KeyCode.F3 => KeyCode.F3,
                UnityEngine.KeyCode.F4 => KeyCode.F4,
                UnityEngine.KeyCode.F5 => KeyCode.F5,
                UnityEngine.KeyCode.F6 => KeyCode.F6,
                UnityEngine.KeyCode.F7 => KeyCode.F7,
                UnityEngine.KeyCode.F8 => KeyCode.F8,
                UnityEngine.KeyCode.F9 => KeyCode.F9,
                UnityEngine.KeyCode.F10 => KeyCode.F10,
                UnityEngine.KeyCode.F11 => KeyCode.F11,
                UnityEngine.KeyCode.F12 => KeyCode.F12,

                _ => KeyCode.None
            };
        }
    }
}
