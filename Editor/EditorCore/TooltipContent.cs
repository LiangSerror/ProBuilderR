using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;
using UnityEngine.ProBuilder;
using UnityEditor.ProBuilder.UI;
using System;

namespace UnityEditor.ProBuilder
{
    /// <inheritdoc />
    /// <summary>
    /// An extended tooltip for use in MenuAction.
    /// </summary>
    [Serializable]
    public sealed class TooltipContent : IEquatable<TooltipContent>
    {
        static GUIStyle TitleStyle { get { if (_titleStyle == null) InitStyles(); return _titleStyle; } }
        static GUIStyle ShortcutStyle { get { if (_shortcutStyle == null) InitStyles(); return _shortcutStyle; } }
        static GUIStyle _titleStyle = null;
        static GUIStyle _shortcutStyle = null;

        const float k_MinWidth = 128;
        const float k_MaxWidth = 330;
        const float k_MinHeight = 0;

        static void InitStyles()
        {
            _titleStyle = new GUIStyle();
            _titleStyle.margin = new RectOffset(4, 4, 4, 4);
            _titleStyle.padding = new RectOffset(4, 4, 4, 4);
            _titleStyle.fontSize = 14;
            _titleStyle.fontStyle = FontStyle.Bold;
            _titleStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            _titleStyle.richText = true;

            _shortcutStyle = new GUIStyle(_titleStyle);
            _shortcutStyle.fontSize = 14;
            _shortcutStyle.fontStyle = FontStyle.Normal;
            _shortcutStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(.5f, .5f, .5f, 1f) : new Color(.3f, .3f, .3f, 1f);

            EditorStyles.wordWrappedLabel.richText = true;
        }

        static readonly Color separatorColor = new Color(.65f, .65f, .65f, .5f);

        /// <summary>
        /// The title to show in the tooltip window.
        /// </summary>
        /// <value>
        /// The header text for this tooltip.
        /// </value>
        public string title { get; set; }

        /// <summary>
        /// A brief summary of what this menu action does.
        /// </summary>
        /// <value>
        /// The body of the summary text.
        /// </value>
        //public string summary { get; set; }//=================================
        //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        public string summary { get { return GetSummary(); } set { summaryOir = value; } }
        public string summaryOir;

        internal string GetSummary()
        {
            if (!ProBuilderEditor.s_EnableTextModeZH) return summaryOir;
            return summaryDic.ContainsKey(title) ? summaryDic[title] : summaryOir;
        }

        static readonly System.Collections.Generic.Dictionary<string, string> summaryDic
        = new System.Collections.Generic.Dictionary<string, string>
        {
            {"New Bezier Shape", "创建一个通过沿着贝塞尔样条曲线挤压创建的新形状。"},
            {"New Polygon Shape", "通过点击一个周长和挤压创建一个新形状。"},
            {"Material Editor", "打开材质编辑器窗口。\n\n材质编辑器窗口将材质应用到选定的面或对象上。"},
            {"New Shape Tool", "打开形状编辑器窗口。\n\n形状编辑器是一个窗口，允许您交互式地创建新的3d基本体。"},
            {"Smoothing Groups Editor", "打开平滑组编辑器。\n\n平滑组对相邻平面的顶点法线进行平均。当处理光滑的边缘时，这允许灯光以更真实的方式表现。\n\nProBuilder通过检查在同一组中的相邻面来决定哪些边应该被平滑。它还检查硬分组，硬分组使相邻面的边缘变硬。"},
            {"UV Editor", "打开UV编辑器窗口。\n\nuv编辑器允许你改变在这个网格上渲染纹理的方式。"},
            {"Vertex Colors Editor", "打开顶点颜色调色板。\n\n使用面部模式应用硬边颜色。使用边缘或顶点模式应用于柔和的混合颜色。"},
            {"Vertex Position Editor", "打开顶点位置编辑器窗口。"},
            {"Export", "将选定的ProBuilder对象导出为模型文件。"},
            {"Export Asset", "导出 Unity 网格资源文件。"},
            {"Export Obj", "导出 Wavefront OBJ 文件。"},
            {"Export Ply", "导出 Stanford PLY 文件。"},
            {"Export Stl", "导出 Stl 网格文件。"},
            {"Bevel", "通过添加连接两个相邻面的倾斜面来平滑选定的边。"},
            {"Bridge Edges", "添加一个连接两条边的新面。"},
            {"Collapse Vertices", "合并所有选择的顶点到一个单一的顶点，以所有选择点的平均值为中心。"},
            {"Conform Face Normals", "所有选择的面朝向相同的方向。"},
            {"Connect Edges", "插入一个新的边连接所有选择边的中心点。参见“细分”。"},
            {"Connect Vertices", "添加连接所有选定顶点的边。"},
            {"Delete Faces", "删除所有选定面。"},
            {"Detach Faces", "从选中的面创建一个新对象(或子网格)。"},
            {"Duplicate Faces", "复制选中的面，或者将它们添加到这个网格中，或者创建一个新的游戏对象。"},
            {"Extrude Edges", "从当前选择的边缘扩展一个新面。边缘必须有开放的一面才能被挤压。"},
            {"Extrude Faces", "挤压选定的面，可以作为一个组挤压，也可以单独挤压。\n\nAlt + 单击此按钮以显示其他挤压选项。"},
            {"Fill Hole", "创建一个新的面连接所有选择的顶点。"},
            {"Flip Face Edge", "在四边形中反转中间边缘的方向。使用这个固定中线在四头与不同的高度角。"},
            {"Flip Face Normals", "在选择中逆转所有面的方向。"},
            {"Insert Edge Loop", "将物体周围的所有边连接成一个圈。"},
            {"Merge Faces", "告诉ProBuilder把选中的面当作单一的面来处理。注意不要在没有连接的面上使用这个!"},
            {"Offset Vertices", "按设定的数量移动选定的元素。"},
            {"Set Pivot to Center of Selection", "将每个网格的枢轴点移动到所有选定元素位置的平均值。这意味着支点移动到手柄当前所在的位置。"},
            {"Split Vertices", "断开在空间中共享相同位置的顶点的连接，以便它们可以相互独立地移动。"},
            {"Subdivide Edges", "将间隔均匀的新顶点追加到选定的边。"},
            {"Subdivide Faces", "在每个选定面的中心插入一个新顶点，并从每个周长边的中心到中心顶点创建一个新边。"},
            {"Triangulate Faces", "将所有选择的面分解成三角形。"},
            {"Weld Vertices", "在当前选择的顶点中搜索与另一个顶点的指定距离内的顶点，并将它们合并为单个顶点。"},
            {"Set Drag Rect Mode", "设置是否需要通过拖动将网格元素(边或面)完全包围。\n\n默认值为Intersect，即如果拖动矩形触及该元素的任何部分，则该元素将被选中。"},
            {"Set Drag Selection Mode", "当拖动选择元素时，shift键\n\n -[加]是否总是添加到选区中\n -[减]是否总是从选区中减去选区中\n -[差]按所选面翻转选区(默认)\n"},
            {"Local", "转换句柄与活动对象旋转对齐。"},
            {"Set Hidden Element Selection", "将“隐藏元素选择”设置为“开启”可以选择被几何体遮挡的面，或者背对场景镜头的面(背对面)。\n\n默认值是On。\n"},
            {"Center Pivot", "将该对象的轴心点设置为其边界的中心。"},
            {"Conform Object Normals", "检查该物体是否有与其他大多数面相反方向翻转的面，然后将持异议的面翻转。"},
            {"Flip Object Normals", "将所选对象的所有面方向翻转。"},
            {"Freeze Transform", "设置轴心点为世界坐标(0,0,0)，清除所有的变换值，同时保持网格在适当的位置。"},
            {"Lightmap UVs", "为开放场景中缺少的任何网格生成光照贴图uv。"},
            {"Merge Objects", "将所有选择的ProBuilder对象合并到一个网格中。"},
            {"Mirror Objects", "镜像对象将复制和翻转指定轴上的对象。"},
            {"ProBuilderize", "从网格中创建 ProBuilder-modifiable 对象。"},
            {"Set Collider", "应用碰撞器材质并添加一个网格碰撞器(如果没有碰撞器)。MeshRenderer将在进入播放模式时自动关闭。"},
            {"Set Trigger", "应用触发材质并添加标记为触发器的碰撞器。MeshRenderer将在进入播放模式时自动关闭。"},
            {"Subdivide Object", "通过在每个面上创建4个新的四边形来增加这个对象上的边和顶点的数量。"},
            {"Triangulate Objects", "删除网格上的所有四边形和n形，并插入三角形代替。使用这个和一个硬平滑组来实现一个低多边形的面化外观。"},
            {"Grow Selection", "将相邻的元素添加到当前所选内容中，可以选择测试它们是否处于指定的角度内。\n\n按角度增长可通过选项+单击增长选择按钮启用。"},
            {"Invert Selection", "选择当前所选内容的反面。例如，所有未选中的元素将被选中，当前选择的元素将被取消选中。"},
            {"Select Edge Loop", "选择连通边的循环。\n\n快捷方式:双击边缘"},
            {"Select Edge Ring", "选择一圈边。环形边对选择边。\n\n快捷方式:Shift +双击边缘"},
            {"Select Face Loop", "选择一个连接面循环。\n\n快捷方式:Shift +双击面。"},
            {"Select Face Ring", "选择一个连接面环。\n\n快捷方式:ctrl +双击面。"},
            {"Select Holes", "选择网格上的孔。\n\n使用当前选择的元素，如果没有选择边或顶点，则测试整个网格。"},
            {"Select by Material", "选择与所选材质匹配的所有面。"},
            {"Select by Smooth", "选择与所选平滑组匹配的所有面。"},
            {"Select by Colors", "选择与所选顶点颜色匹配的所有面。"},
            {"Shrink Selection", "删除当前选定内容边缘上的元素。"}
        };
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        /// <summary>
        /// The shortcut assigned to this menu item.
        /// </summary>
        /// <value>
        /// A text representation of the optional shortcut.
        /// </value>
        public string shortcut { get; set; }

        internal static TooltipContent TempContent = new TooltipContent("", "");

        /// <summary>
        /// Create a new tooltip.
        /// </summary>
        /// <param name="title">The header text for this tooltip.</param>
        /// <param name="summary">The body of the tooltip text. This should be kept brief.</param>
        /// <param name="shortcut">A set of keys to be displayed as the shortcut for this action.</param>
        public TooltipContent(string title, string summary, params char[] shortcut) : this(title, summary, "")
        {
            if (shortcut != null && shortcut.Length > 0)
            {
                this.shortcut = string.Empty;

                for (int i = 0; i < shortcut.Length - 1; i++)
                {
                    if (!EditorUtility.IsUnix())
                        this.shortcut += InternalUtility.ControlKeyString(shortcut[i]) + " + ";
                    else
                        this.shortcut += shortcut[i] + " + ";
                }

                if (!EditorUtility.IsUnix())
                    this.shortcut += InternalUtility.ControlKeyString(shortcut[shortcut.Length - 1]);
                else
                    this.shortcut += shortcut[shortcut.Length - 1];
            }
        }

        /// <summary>
        /// Create a new tooltip.
        /// </summary>
        /// <param name="title">The header text for this tooltip.</param>
        /// <param name="summary">The body of the tooltip text. This should be kept brief.</param>
        /// <param name="shortcut">A set of keys to be displayed as the shortcut for this action.</param>
        public TooltipContent(string title, string summary, string shortcut = "")
        {
            this.title = title;
            this.summary = summary;
            this.shortcut = shortcut;
        }

        /// <summary>
        /// Get the size required in GUI space to render this tooltip.
        /// </summary>
        /// <returns></returns>
        internal Vector2 CalcSize()
        {
            const float pad = 8;
            Vector2 total = new Vector2(k_MinWidth, k_MinHeight);

            bool hastitle = !string.IsNullOrEmpty(title);
            bool hasSummary = !string.IsNullOrEmpty(summary);
            bool hasShortcut = !string.IsNullOrEmpty(shortcut);

            if (hastitle)
            {
                Vector2 ns = TitleStyle.CalcSize(UI.EditorGUIUtility.TempContent(title));

                if (hasShortcut)
                {
                    ns.x += EditorStyles.boldLabel.CalcSize(UI.EditorGUIUtility.TempContent(shortcut)).x + pad;
                }

                total.x += Mathf.Max(ns.x, 256);
                total.y += ns.y;
            }

            if (hasSummary)
            {
                if (!hastitle)
                {
                    Vector2 sumSize = EditorStyles.wordWrappedLabel.CalcSize(UI.EditorGUIUtility.TempContent(summary));
                    total.x = Mathf.Min(sumSize.x, k_MaxWidth);
                }

                float summaryHeight = EditorStyles.wordWrappedLabel.CalcHeight(UI.EditorGUIUtility.TempContent(summary), total.x);
                total.y += summaryHeight;
            }

            if (hastitle && hasSummary)
                total.y += 16;

            total.x += pad;
            total.y += pad;

            return total;
        }

        internal void Draw()
        {
            if (!string.IsNullOrEmpty(title))
            {
                if (!string.IsNullOrEmpty(shortcut))
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(title, TitleStyle);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(shortcut, ShortcutStyle);
                    GUILayout.EndHorizontal();
                }
                else
                {
                    GUILayout.Label(title, TitleStyle);
                }

                UI.EditorGUIUtility.DrawSeparator(1, separatorColor);
                GUILayout.Space(2);
            }

            if (!string.IsNullOrEmpty(summary))
            {
                GUILayout.Label(summary, EditorStyles.wordWrappedLabel);
            }
        }

        /// <summary>
        /// Equality check is performed by comparing the title property of each tooltip.
        /// </summary>
        /// <param name="other">The ToolTip content to compare.</param>
        /// <returns>True if title is the same, false otherwise.</returns>
        public bool Equals(TooltipContent other)
        {
            return other != null && other.title != null && other.title.Equals(this.title);
        }

        /// <summary>
        /// Equality check is performed by comparing the title property of each tooltip.
        /// </summary>
        /// <param name="obj">The ToolTip content to compare.</param>
        /// <returns>True if title is the same, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is TooltipContent && ((TooltipContent)obj).title.Equals(title);
        }

        public override int GetHashCode()
        {
            return title.GetHashCode();
        }

        /// <summary>
        /// Convert a tooltip to a string.
        /// </summary>
        /// <param name="content">The Tooltip to convert.</param>
        /// <returns>The title of content.</returns>
        /// <exception cref="ArgumentNullException">content is null.</exception>
        public static explicit operator string(TooltipContent content)
        {
            if (content == null)
                throw new ArgumentNullException("content");
            return content.title;
        }

        /// <summary>
        /// Create a Tooltip with a title.
        /// </summary>
        /// <param name="title">The title to apply to the new Tooltip.</param>
        /// <returns>A new Tooltip with title and no content.</returns>
        public static explicit operator TooltipContent(string title)
        {
            return new TooltipContent(title, "");
        }

        /// <summary>
        /// Convert a Tooltip to a string.
        /// </summary>
        /// <returns>The title of the Tooltip.</returns>
        public override string ToString()
        {
            return title;
        }

        /// <summary>
        /// Create a new tooltip with title.
        /// </summary>
        /// <param name="title">The title to apply to the new Tooltip.</param>
        /// <returns>A new Tooltip with title and no content.</returns>
        public static TooltipContent FromString(string title)
        {
            return new TooltipContent(title, "");
        }
    }
}
