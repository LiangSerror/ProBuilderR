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
            {"New Bezier Shape", "����һ��ͨ�����ű������������߼�ѹ����������״��"},
            {"New Polygon Shape", "ͨ�����һ���ܳ��ͼ�ѹ����һ������״��"},
            {"Material Editor", "�򿪲��ʱ༭�����ڡ�\n\n���ʱ༭�����ڽ�����Ӧ�õ�ѡ�����������ϡ�"},
            {"New Shape Tool", "����״�༭�����ڡ�\n\n��״�༭����һ�����ڣ�����������ʽ�ش����µ�3d�����塣"},
            {"Smoothing Groups Editor", "��ƽ����༭����\n\nƽ���������ƽ��Ķ��㷨�߽���ƽ����������⻬�ı�Եʱ��������ƹ��Ը���ʵ�ķ�ʽ���֡�\n\nProBuilderͨ�������ͬһ���е���������������Щ��Ӧ�ñ�ƽ�����������Ӳ���飬Ӳ����ʹ������ı�Ե��Ӳ��"},
            {"UV Editor", "��UV�༭�����ڡ�\n\nuv�༭��������ı��������������Ⱦ����ķ�ʽ��"},
            {"Vertex Colors Editor", "�򿪶�����ɫ��ɫ�塣\n\nʹ���沿ģʽӦ��Ӳ����ɫ��ʹ�ñ�Ե�򶥵�ģʽӦ������͵Ļ����ɫ��"},
            {"Vertex Position Editor", "�򿪶���λ�ñ༭�����ڡ�"},
            {"Export", "��ѡ����ProBuilder���󵼳�Ϊģ���ļ���"},
            {"Export Asset", "���� Unity ������Դ�ļ���"},
            {"Export Obj", "���� Wavefront OBJ �ļ���"},
            {"Export Ply", "���� Stanford PLY �ļ���"},
            {"Export Stl", "���� Stl �����ļ���"},
            {"Bevel", "ͨ����������������������б����ƽ��ѡ���ıߡ�"},
            {"Bridge Edges", "���һ�����������ߵ����档"},
            {"Collapse Vertices", "�ϲ�����ѡ��Ķ��㵽һ����һ�Ķ��㣬������ѡ����ƽ��ֵΪ���ġ�"},
            {"Conform Face Normals", "����ѡ����泯����ͬ�ķ���"},
            {"Connect Edges", "����һ���µı���������ѡ��ߵ����ĵ㡣�μ���ϸ�֡���"},
            {"Connect Vertices", "�����������ѡ������ıߡ�"},
            {"Delete Faces", "ɾ������ѡ���档"},
            {"Detach Faces", "��ѡ�е��洴��һ���¶���(��������)��"},
            {"Duplicate Faces", "����ѡ�е��棬���߽�������ӵ���������У����ߴ���һ���µ���Ϸ����"},
            {"Extrude Edges", "�ӵ�ǰѡ��ı�Ե��չһ�����档��Ե�����п��ŵ�һ����ܱ���ѹ��"},
            {"Extrude Faces", "��ѹѡ�����棬������Ϊһ���鼷ѹ��Ҳ���Ե�����ѹ��\n\nAlt + �����˰�ť����ʾ������ѹѡ�"},
            {"Fill Hole", "����һ���µ�����������ѡ��Ķ��㡣"},
            {"Flip Face Edge", "���ı����з�ת�м��Ե�ķ���ʹ������̶���������ͷ�벻ͬ�ĸ߶Ƚǡ�"},
            {"Flip Face Normals", "��ѡ������ת������ķ���"},
            {"Insert Edge Loop", "��������Χ�����б����ӳ�һ��Ȧ��"},
            {"Merge Faces", "����ProBuilder��ѡ�е��浱����һ����������ע�ⲻҪ��û�����ӵ�����ʹ�����!"},
            {"Offset Vertices", "���趨�������ƶ�ѡ����Ԫ�ء�"},
            {"Set Pivot to Center of Selection", "��ÿ�������������ƶ�������ѡ��Ԫ��λ�õ�ƽ��ֵ������ζ��֧���ƶ����ֱ���ǰ���ڵ�λ�á�"},
            {"Split Vertices", "�Ͽ��ڿռ��й�����ͬλ�õĶ�������ӣ��Ա����ǿ����໥�������ƶ���"},
            {"Subdivide Edges", "��������ȵ��¶���׷�ӵ�ѡ���ıߡ�"},
            {"Subdivide Faces", "��ÿ��ѡ��������Ĳ���һ���¶��㣬����ÿ���ܳ��ߵ����ĵ����Ķ��㴴��һ���±ߡ�"},
            {"Triangulate Faces", "������ѡ�����ֽ�������Ρ�"},
            {"Weld Vertices", "�ڵ�ǰѡ��Ķ�������������һ�������ָ�������ڵĶ��㣬�������Ǻϲ�Ϊ�������㡣"},
            {"Set Drag Rect Mode", "�����Ƿ���Ҫͨ���϶�������Ԫ��(�߻���)��ȫ��Χ��\n\nĬ��ֵΪIntersect��������϶����δ�����Ԫ�ص��κβ��֣����Ԫ�ؽ���ѡ�С�"},
            {"Set Drag Selection Mode", "���϶�ѡ��Ԫ��ʱ��shift��\n\n -[��]�Ƿ�������ӵ�ѡ����\n -[��]�Ƿ����Ǵ�ѡ���м�ȥѡ����\n -[��]����ѡ�淭תѡ��(Ĭ��)\n"},
            {"Local", "ת�������������ת���롣"},
            {"Set Hidden Element Selection", "��������Ԫ��ѡ������Ϊ������������ѡ�񱻼������ڵ����棬���߱��Գ�����ͷ����(������)��\n\nĬ��ֵ��On��\n"},
            {"Center Pivot", "���ö�������ĵ�����Ϊ��߽�����ġ�"},
            {"Conform Object Normals", "���������Ƿ�����������������෴����ת���棬Ȼ�󽫳�������淭ת��"},
            {"Flip Object Normals", "����ѡ����������淽��ת��"},
            {"Freeze Transform", "�������ĵ�Ϊ��������(0,0,0)��������еı任ֵ��ͬʱ�����������ʵ���λ�á�"},
            {"Lightmap UVs", "Ϊ���ų�����ȱ�ٵ��κ��������ɹ�����ͼuv��"},
            {"Merge Objects", "������ѡ���ProBuilder����ϲ���һ�������С�"},
            {"Mirror Objects", "������󽫸��ƺͷ�תָ�����ϵĶ���"},
            {"ProBuilderize", "�������д��� ProBuilder-modifiable ����"},
            {"Set Collider", "Ӧ����ײ�����ʲ����һ��������ײ��(���û����ײ��)��MeshRenderer���ڽ��벥��ģʽʱ�Զ��رա�"},
            {"Set Trigger", "Ӧ�ô������ʲ���ӱ��Ϊ����������ײ����MeshRenderer���ڽ��벥��ģʽʱ�Զ��رա�"},
            {"Subdivide Object", "ͨ����ÿ�����ϴ���4���µ��ı�����������������ϵıߺͶ����������"},
            {"Triangulate Objects", "ɾ�������ϵ������ı��κ�n�Σ������������δ��档ʹ�������һ��Ӳƽ������ʵ��һ���Ͷ���ε��滯��ۡ�"},
            {"Grow Selection", "�����ڵ�Ԫ����ӵ���ǰ��ѡ�����У�����ѡ����������Ƿ���ָ���ĽǶ��ڡ�\n\n���Ƕ�������ͨ��ѡ��+��������ѡ��ť���á�"},
            {"Invert Selection", "ѡ��ǰ��ѡ���ݵķ��档���磬����δѡ�е�Ԫ�ؽ���ѡ�У���ǰѡ���Ԫ�ؽ���ȡ��ѡ�С�"},
            {"Select Edge Loop", "ѡ����ͨ�ߵ�ѭ����\n\n��ݷ�ʽ:˫����Ե"},
            {"Select Edge Ring", "ѡ��һȦ�ߡ����α߶�ѡ��ߡ�\n\n��ݷ�ʽ:Shift +˫����Ե"},
            {"Select Face Loop", "ѡ��һ��������ѭ����\n\n��ݷ�ʽ:Shift +˫���档"},
            {"Select Face Ring", "ѡ��һ�������滷��\n\n��ݷ�ʽ:ctrl +˫���档"},
            {"Select Holes", "ѡ�������ϵĿס�\n\nʹ�õ�ǰѡ���Ԫ�أ����û��ѡ��߻򶥵㣬�������������"},
            {"Select by Material", "ѡ������ѡ����ƥ��������档"},
            {"Select by Smooth", "ѡ������ѡƽ����ƥ��������档"},
            {"Select by Colors", "ѡ������ѡ������ɫƥ��������档"},
            {"Shrink Selection", "ɾ����ǰѡ�����ݱ�Ե�ϵ�Ԫ�ء�"}
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
