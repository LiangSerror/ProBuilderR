// #define GENERATE_DESATURATED_ICONS
#if UNITY_2019_1_OR_NEWER
#define SHORTCUT_MANAGER
#endif

using UnityEngine;
using UnityEngine.ProBuilder;

namespace UnityEditor.ProBuilder
{
    /// <summary>
    /// Base class from which any action that is represented in the ProBuilder toolbar inherits.
    /// </summary>
    public abstract class MenuAction
    {
        /// <summary>
        /// A flag indicating the state of a menu action. This determines whether the menu item is visible, and if visible, enabled.
        /// </summary>
        [System.Flags]
        public enum MenuActionState
        {
            /// <summary>
            /// The button is not visible in the toolbar.
            /// </summary>
            Hidden = 0x0,
            /// <summary>
            /// The button is visible in the toolbar.
            /// </summary>
            Visible = 0x1,
            /// <summary>
            /// The button (and by proxy, the action it performs) are valid given the current selection.
            /// </summary>
            Enabled = 0x2,
            /// <summary>
            /// Button and action are both visible in the toolbar and valid given the current selection.
            /// </summary>
            VisibleAndEnabled = 0x3
        };

        /// <value>
        /// Path to the ProBuilder menu category.
        /// </value>
        /// <remarks>
        /// Use this where you wish to add a top level menu item.
        /// </remarks>
        internal const string probuilderMenuPath = "Tools/ProBuilder/";

        /// <value>
        /// The unicode character for the control key symbol on Windows, or command key on macOS.
        /// </value>
        internal const char keyCommandSuper = PreferenceKeys.CMD_SUPER;

        /// <value>
        /// The unicode character for the shift key symbol.
        /// </value>
        internal const char keyCommandShift = PreferenceKeys.CMD_SHIFT;

        /// <value>
        /// The unicode character for the option key symbol on macOS.
        /// </value>
        /// <seealso cref="keyCommandAlt"/>
        internal const char keyCommandOption = PreferenceKeys.CMD_OPTION;

        /// <value>
        /// The unicode character for the alt key symbol on Windows.
        /// </value>
        internal const char keyCommandAlt = PreferenceKeys.CMD_ALT;

        /// <value>
        /// The unicode character for the delete key symbol.
        /// </value>
        internal const char keyCommandDelete = PreferenceKeys.CMD_DELETE;

        static readonly GUIContent AltButtonContent = new GUIContent("+", "");

        static readonly Vector2 AltButtonSize = new Vector2(21, 0);

        Vector2 m_LastCalculatedSize = Vector2.zero;

        //<<<<<<<<<<<<<<<<<<<<<<<<<
        static readonly Vector2 TextModeIconSize = new Vector2(32, 22);

        static readonly System.Collections.Generic.Dictionary<string, string> menuTitleDic
        = new System.Collections.Generic.Dictionary<string, string>
        {
            {"New Bezier Shape", "创建贝塞尔模型"},
            {"New Poly Shape", "创建多边形模型"},
            {"Lightmap UV Editor", "光照UV编辑"},
            {"Material Editor", "材质编辑"},
            {"New Shape", "创建模型"},
            {"Smoothing", "平滑"},
            {"UV Editor", "UV编辑"},
            {"Vertex Colors", "顶点颜色"},
            {"Vertex Editor", "顶点编辑"},
            {"Export", "导出"},
            {"Export Asset", "导出 Asset"},
            {"Export Obj", "导出 Obj"},
            {"Export Ply", "导出 Ply"},
            {"Export Stl", "导出 Stl"},
            {"Bevel", "倒角"},
            {"Bridge Edges", "桥接边"},
            {"Collapse Vertices", "折叠点"},
            {"Conform Normals", "统一法线"},
            {"Connect Edges", "连接边"},
            {"Connect Vertices", "连接点"},
            {"Delete Faces", "删除面"},
            {"Detach Faces", "分离面"},
            {"Duplicate Faces", "复制面"},
            {"Extrude", "挤出"},
            {"Extrude Edges", "挤出边"},
            {"Extrude Faces", "挤出面"},
            {"Fill Hole", "补洞"},
            {"Flip Face Edge", "翻转面边"},
            {"Flip Face Normals", "翻转法线"},
            {"Insert Edge Loop", "插入循环边"},
            {"Merge Faces", "合并面"},
            {"Offset Edges", "偏移边"},
            {"Offset Vertices", "偏移点"},
            {"Set Pivot", "设置枢轴"},
            {"Smart Connect", "智能连接"},
            {"Smart Subdivide", "智能细分"},
            {"Split Vertices", "分离点"},
            {"Subdivide Edges", "细分边"},
            {"Subdivide Faces", "细分面"},
            {"Triangulate Faces", "三角面"},
            {"Weld Vertices", "焊接点"},
            {"Shift: Add", "Shift：添加"},
            {"Shift: Subtract", "Shift：减去"},
            {"Shift: Difference", "Shift：差分"},
            {"Orientation: Global", "Orientation：全局"},
            {"Orientation: Local", "Orientation：本地"},
            {"Orientation: Normal", "Orientation：常规"},
            {"Select Hidden: Off", "选择隐藏：关"},
            {"Select Hidden: On", "选择隐藏：开"},
            {"Center Pivot", "居中枢轴"},
            {"ProBuilderize", "网格转ProB..."},
            {"Flip Normals", "翻转法线"},
            {"Freeze Transform", "冻结变换"},
            {"Lightmap UVs", "光照UVs"},
            {"Merge Objects", "合并对象"},
            {"Mirror Objects", "镜像对象"},
            {"Set Collider", "设置碰撞"},
            {"Set Trigger", "设置触发器"},
            {"Subdivide Object", "细分对象"},
            {"Triangulate", "三角化"},
            {"Grow Selection", "扩展选择区域"},
            {"Invert Selection", "反相选取"},
            {"Select Edge Loop", "选择循环边"},
            {"Select Edge Ring", "选择环边"},
            {"Select Face Loop", "选择循环面"},
            {"Select Face Ring", "选择环面"},
            {"Select Holes", "选择洞"},
            {"Select Loop", "选择循环"},
            {"Select by Material", "用材质选择"},
            {"Select Ring", "选择环"},
            {"Select by Smooth", "用平滑选择"},
            {"Select by Colors", "用颜色选择"},
            {"Shrink Selection", "收缩选择区域"}
        };

        internal string TranslateMT(string menuTitle)
        {
            if (!textModeZH) return menuTitle;
            return menuTitleDic.ContainsKey(menuTitle) ? menuTitleDic[menuTitle] : menuTitle;
        }

        //>>>>>>>>>>>>>>>>>>>>>>>>>

        protected MenuAction()
        {
            iconMode = ProBuilderEditor.s_IsIconGui;
            //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            textModeIcon = ProBuilderEditor.s_EnableTextModeIcon;
            textModeZH = ProBuilderEditor.s_EnableTextModeZH;
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        }

        /// <summary>
        /// Compare two menu items precedence by their category and priority modifier.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        internal static int CompareActionsByGroupAndPriority(MenuAction left, MenuAction right)
        {
            if (left == null)
            {
                if (right == null)
                    return 0;
                else
                    return -1;
            }
            else
            {
                if (right == null)
                {
                    return 1;
                }
                else
                {
                    int l = (int)left.group, r = (int)right.group;

                    if (l < r)
                        return -1;
                    else if (l > r)
                        return 1;
                    else
                    {
                        int lp = left.toolbarPriority < 0 ? int.MaxValue : left.toolbarPriority,
                            rp = right.toolbarPriority < 0 ? int.MaxValue : right.toolbarPriority;

                        return lp.CompareTo(rp);
                    }
                }
            }
        }

        Texture2D m_DesaturatedIcon = null;

        /// <summary>
        /// By default this function will look for an image named `${icon}_disabled`. If your disabled icon is somewhere else override this function.
        /// </summary>
        protected virtual Texture2D disabledIcon
        {
            get
            {
                if (m_DesaturatedIcon == null)
                {
                    if (icon == null)
                        return null;

                    m_DesaturatedIcon = IconUtility.GetIcon(string.Format("Toolbar/{0}_disabled", icon.name));

#if GENERATE_DESATURATED_ICONS
                    if (!m_DesaturatedIcon)
                        m_DesaturatedIcon = ProBuilder2.EditorCommon.DebugUtilities.pb_GenerateDesaturatedImage.CreateDesaturedImage(icon);
#endif
                }

                return m_DesaturatedIcon;
            }
        }

        /// <value>
        /// What category this action belongs in.
        /// </value>
        public abstract ToolbarGroup group { get; }

        /// <value>
        /// Optional value influences where in the toolbar this menu item will be placed.
        /// <remarks>
        /// 0 is first, 1 is second, -1 is no preference.
        /// </remarks>
        /// </value>
        public virtual int toolbarPriority { get { return -1; } }

        /// <value>
        /// The icon to be displayed for this action.
        /// </value>
        /// <remarks>
        /// Not used when toolbar is in text mode.
        /// </remarks>
        public abstract Texture2D icon { get; }

        /// <value>
        /// The contents to display for this menu action's tooltip.
        /// </value>
        public abstract TooltipContent tooltip { get; }

        /// <value>
        /// Optional override for the action title displayed in the toolbar button.
        /// </value>
        /// <remarks>
        /// If unimplemented the tooltip title is used.
        /// </remarks>
        public virtual string menuTitle { get { return tooltip.title; } }

        /// <value>
        /// True if this class should have an entry built into the hardware menu. This is not implemented for custom actions.
        /// </value>
        protected virtual bool hasFileMenuEntry { get { return true; } }

        /// <summary>
        /// Is the current mode and selection valid for this action?
        /// </summary>
        /// <value>A flag indicating both the visibility and enabled state for an action.</value>
        public MenuActionState menuActionState
        {
            get
            {
                if (hidden)
                    return MenuActionState.Hidden;
                if (enabled)
                    return MenuActionState.VisibleAndEnabled;
                return MenuActionState.Visible;
            }
        }

        /// <summary>
        /// In which SelectMode states is this action applicable. Drives the `virtual bool hidden { get; }` property unless overridden.
        /// </summary>
        public virtual SelectMode validSelectModes
        {
            get { return SelectMode.Any; }
        }

        /// <summary>
        /// A check for whether or not the action is valid given the current selection.
        /// </summary>
        /// <seealso cref="hidden"/>
        /// <value>True if this action is valid with current selection and mode.</value>
        public virtual bool enabled
        {
            get
            {
                return ProBuilderEditor.instance != null
                    && ProBuilderEditor.selectMode.ContainsFlag(validSelectModes)
                    && !ProBuilderEditor.selectMode.ContainsFlag(SelectMode.InputTool);
            }
        }

        /// <summary>
        /// Is this action visible in the ProBuilder toolbar?
        /// </summary>
        /// <remarks>This returns false by default.</remarks>
        /// <seealso cref="enabled"/>
        /// <value>True if this action should be shown in the toolbar with the current mode and settings, false otherwise.</value>
        public virtual bool hidden
        {
            get { return !ProBuilderEditor.selectMode.ContainsFlag(validSelectModes); }
        }

        /// <summary>
        /// Get a flag indicating the visibility and enabled state of an extra options menu modifier for this action.
        /// </summary>
        /// <value>A flag specifying whether an options icon should be displayed for this action button. If your action implements some etra options, you must also implement OnSettingsGUI.</value>
        protected virtual MenuActionState optionsMenuState
        {
            get { return MenuActionState.Hidden; }
        }

        /// <summary>
        /// Perform whatever action this menu item is supposed to do. You are responsible for implementing Undo.
        /// </summary>
        /// <returns>A new ActionResult with a summary of the state of the action's success.</returns>
        public abstract ActionResult DoAction();

        protected virtual void DoAlternateAction()
        {
            MenuOption.Show(OnSettingsGUI, OnSettingsEnable, OnSettingsDisable);
        }

        /// <summary>
        /// Implement the extra settings GUI for your action in this method.
        /// </summary>
        protected virtual void OnSettingsGUI() { }

        /// <summary>
        /// Called when the settings window is opened.
        /// </summary>
        protected virtual void OnSettingsEnable() { }

        /// <summary>
        /// Called when the settings window is closed.
        /// </summary>
        protected virtual void OnSettingsDisable() { }

        protected bool iconMode { get; set; }

        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        protected bool textModeIcon { get; set; }
        protected bool textModeZH { get; set; }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        /// <summary>
        /// Draw a menu button.  Returns true if the button is active and settings are enabled, false if settings are not enabled.
        /// </summary>
        /// <param name="isHorizontal"></param>
        /// <param name="showOptions"></param>
        /// <param name="optionsRect"></param>
        /// <param name="layoutOptions"></param>
        /// <returns></returns>
        internal bool DoButton(bool isHorizontal, bool showOptions, ref Rect optionsRect, params GUILayoutOption[] layoutOptions)
        {
            bool wasEnabled = GUI.enabled;
            bool buttonEnabled = (menuActionState & MenuActionState.Enabled) == MenuActionState.Enabled;

            GUI.enabled = buttonEnabled;

            GUI.backgroundColor = Color.white;

            if (iconMode)
            {
                if (GUILayout.Button(buttonEnabled || !disabledIcon ? icon : disabledIcon, ToolbarGroupUtility.GetStyle(group, isHorizontal), layoutOptions))
                {
                    if (showOptions && (optionsMenuState & MenuActionState.VisibleAndEnabled) == MenuActionState.VisibleAndEnabled)
                    {
                        DoAlternateAction();
                    }
                    else
                    {
                        ActionResult result = DoAction();
                        EditorUtility.ShowNotification(result.notification);
                    }
                }

                if ((optionsMenuState & MenuActionState.VisibleAndEnabled) == MenuActionState.VisibleAndEnabled)
                {
                    Rect r = GUILayoutUtility.GetLastRect();
                    r.x = r.x + r.width - 16;
                    r.y += 0;
                    r.width = 14;
                    r.height = 14;
                    GUI.Label(r, IconUtility.GetIcon("Toolbar/Options", IconSkin.Pro), GUIStyle.none);
                    optionsRect = r;
                    GUI.enabled = wasEnabled;
                    return buttonEnabled;
                }
                else
                {
                    GUI.enabled = wasEnabled;
                    return false;
                }
            }
            else
            {
                // in text mode always use the vertical layout.
                isHorizontal = false;
                GUILayout.BeginHorizontal(MenuActionStyles.rowStyleVertical, layoutOptions);

                GUI.backgroundColor = ToolbarGroupUtility.GetColor(group);

                //if (GUILayout.Button(menuTitle, MenuActionStyles.buttonStyleVertical))//=======================
                //<<<<<<<<<<<<<<<<<<<<<<<<
                GUIContent guiContent = new GUIContent(TranslateMT(menuTitle), textModeIcon ? (buttonEnabled || !disabledIcon ? icon : disabledIcon) : null);
                using (new EditorGUIUtility.IconSizeScope(TextModeIconSize))
                {
                    if (GUILayout.Button(guiContent, MenuActionStyles.buttonStyleVertical))
                    //>>>>>>>>>>>>>>>>>>>>>>>>
                    {
                        ActionResult res = DoAction();
                        EditorUtility.ShowNotification(res.notification);
                    }
                }//<<<<<<<<<<<<<>>>>>>>>>>>>>
                MenuActionState altState = optionsMenuState;

                if ((altState & MenuActionState.Visible) == MenuActionState.Visible)
                {
                    GUI.enabled = GUI.enabled && (altState & MenuActionState.Enabled) == MenuActionState.Enabled;

                    //if (DoAltButton(GUILayout.MaxWidth(21), GUILayout.MaxHeight(16))) //==========================
                    //if (DoAltButton(GUILayout.MaxWidth(21), GUILayout.MaxHeight(textModeIcon ? TextModeIconSize.y + 4 : 16)))   //<<<<<<<<<<<<<>>>>>>>>>>>>>
                    if (DoAltButton(GUILayout.MaxWidth(21), GUILayout.MaxHeight(textModeIcon ? Mathf.Max(16, guiContent.image.height + 4) : 16)))   //<<<<<<<<<<<<<>>>>>>>>>>>>>
                    DoAlternateAction();
                }
                GUILayout.EndHorizontal();

                GUI.backgroundColor = Color.white;

                GUI.enabled = wasEnabled;

                return false;
            }
        }

        bool DoAltButton(params GUILayoutOption[] options)
        {
            return GUILayout.Button(AltButtonContent, MenuActionStyles.altButtonStyle, options);
        }

        /// <summary>
        /// Get the rendered width of this GUI item.
        /// </summary>
        /// <param name="isHorizontal"></param>
        /// <returns></returns>
        internal Vector2 GetSize(bool isHorizontal)
        {
            if (iconMode)
            {
                m_LastCalculatedSize = ToolbarGroupUtility.GetStyle(ToolbarGroup.Object, isHorizontal).CalcSize(UI.EditorGUIUtility.TempContent(null, null, icon));
            }
            else
            {
                // in text mode always use the vertical layout.
                isHorizontal = false;

                //m_LastCalculatedSize = MenuActionStyles.buttonStyleVertical.CalcSize(UI.EditorGUIUtility.TempContent(menuTitle)) + AltButtonSize;//=================

                //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                m_LastCalculatedSize = MenuActionStyles.buttonStyleVertical.CalcSize(UI.EditorGUIUtility.TempContent(TranslateMT(menuTitle))) + AltButtonSize;

                if (textModeIcon)
                {
                    bool buttonEnabled = (menuActionState & MenuActionState.Enabled) == MenuActionState.Enabled;
                    Texture2D t = (buttonEnabled || !disabledIcon ? icon : disabledIcon);
                    if (t)
                    {
                        m_LastCalculatedSize.x += t.width;
                        m_LastCalculatedSize.y = System.Math.Max(m_LastCalculatedSize.y, t.height);
                    }

                    //m_LastCalculatedSize.x += TextModeIconSize.x;
                    //m_LastCalculatedSize.y = System.Math.Max(m_LastCalculatedSize.y, TextModeIconSize.y);
                }
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            }
            return m_LastCalculatedSize;
        }
    }
}
