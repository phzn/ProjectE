using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Collections;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem
{
	// Token: 0x02000034 RID: 52
	public static class InputControlPath
	{
		// Token: 0x06000449 RID: 1097 RVA: 0x0001285B File Offset: 0x00010A5B
		internal static string CleanSlashes(this string pathComponent)
		{
			return pathComponent.Replace('/', ' ');
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00012868 File Offset: 0x00010A68
		public static string Combine(InputControl parent, string path)
		{
			if (parent == null)
			{
				if (string.IsNullOrEmpty(path))
				{
					return string.Empty;
				}
				if (path[0] != '/')
				{
					return "/" + path;
				}
				return path;
			}
			else
			{
				if (string.IsNullOrEmpty(path))
				{
					return parent.path;
				}
				return parent.path + "/" + path;
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x000128C0 File Offset: 0x00010AC0
		public static string ToHumanReadableString(string path, InputControlPath.HumanReadableStringOptions options = InputControlPath.HumanReadableStringOptions.None, InputControl control = null)
		{
			string text;
			string text2;
			return InputControlPath.ToHumanReadableString(path, out text, out text2, options, control);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000128DC File Offset: 0x00010ADC
		public static string ToHumanReadableString(string path, out string deviceLayoutName, out string controlPath, InputControlPath.HumanReadableStringOptions options = InputControlPath.HumanReadableStringOptions.None, InputControl control = null)
		{
			deviceLayoutName = null;
			controlPath = null;
			if (string.IsNullOrEmpty(path))
			{
				return string.Empty;
			}
			if (control != null)
			{
				InputControl inputControl = InputControlPath.TryFindControl(control, path, 0) ?? (InputControlPath.Matches(path, control) ? control : null);
				if (inputControl != null)
				{
					string text = (((options & InputControlPath.HumanReadableStringOptions.UseShortNames) != InputControlPath.HumanReadableStringOptions.None && !string.IsNullOrEmpty(inputControl.shortDisplayName)) ? inputControl.shortDisplayName : inputControl.displayName);
					if ((options & InputControlPath.HumanReadableStringOptions.OmitDevice) == InputControlPath.HumanReadableStringOptions.None)
					{
						text = text + " [" + inputControl.device.displayName + "]";
					}
					deviceLayoutName = inputControl.device.layout;
					if (!(inputControl is InputDevice))
					{
						controlPath = inputControl.path.Substring(inputControl.device.path.Length + 1);
					}
					return text;
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			InputControlPath.PathParser pathParser = new InputControlPath.PathParser(path);
			string text4;
			using (InputControlLayout.CacheRef())
			{
				if (pathParser.MoveToNextComponent())
				{
					string text3;
					string text2 = pathParser.current.ToHumanReadableString(null, null, out text3, out text4, options);
					deviceLayoutName = text3;
					bool flag = true;
					while (pathParser.MoveToNextComponent())
					{
						if (!flag)
						{
							stringBuilder.Append('/');
						}
						stringBuilder.Append(pathParser.current.ToHumanReadableString(text3, controlPath, out text3, out controlPath, options));
						flag = false;
					}
					if ((options & InputControlPath.HumanReadableStringOptions.OmitDevice) == InputControlPath.HumanReadableStringOptions.None && !string.IsNullOrEmpty(text2))
					{
						stringBuilder.Append(" [");
						stringBuilder.Append(text2);
						stringBuilder.Append(']');
					}
				}
				if (stringBuilder.Length == 0)
				{
					text4 = path;
				}
				else
				{
					text4 = stringBuilder.ToString();
				}
			}
			return text4;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00012A70 File Offset: 0x00010C70
		public static string[] TryGetDeviceUsages(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			InputControlPath.PathParser pathParser = new InputControlPath.PathParser(path);
			if (!pathParser.MoveToNextComponent())
			{
				return null;
			}
			if (pathParser.current.m_Usages.length > 0)
			{
				return pathParser.current.m_Usages.ToArray<string>((Substring x) => x.ToString());
			}
			return null;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00012AE4 File Offset: 0x00010CE4
		public static string TryGetDeviceLayout(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			InputControlPath.PathParser pathParser = new InputControlPath.PathParser(path);
			if (!pathParser.MoveToNextComponent())
			{
				return null;
			}
			if (pathParser.current.m_Layout.length > 0)
			{
				return pathParser.current.m_Layout.ToString().Unescape("ntr\\\"", "\n\t\r\\\"");
			}
			if (pathParser.current.isWildcard)
			{
				return "*";
			}
			return null;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x00012B64 File Offset: 0x00010D64
		public static string TryGetControlLayout(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			int length = path.Length;
			int num = path.LastIndexOf('/');
			if (num == -1 || num == 0)
			{
				return null;
			}
			if (length > num + 2 && path[num + 1] == '<' && path[length - 1] == '>')
			{
				int num2 = num + 2;
				int num3 = length - num2 - 1;
				return path.Substring(num2, num3);
			}
			InputControlPath.PathParser pathParser = new InputControlPath.PathParser(path);
			if (!pathParser.MoveToNextComponent())
			{
				return null;
			}
			if (pathParser.current.isWildcard)
			{
				throw new NotImplementedException();
			}
			if (pathParser.current.m_Layout.length == 0)
			{
				return null;
			}
			string text = pathParser.current.m_Layout.ToString();
			if (!pathParser.MoveToNextComponent())
			{
				return null;
			}
			if (pathParser.current.isWildcard)
			{
				return "*";
			}
			return InputControlPath.FindControlLayoutRecursive(ref pathParser, text.Unescape("ntr\\\"", "\n\t\r\\\""));
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x00012C5C File Offset: 0x00010E5C
		private static string FindControlLayoutRecursive(ref InputControlPath.PathParser parser, string layoutName)
		{
			string text;
			using (InputControlLayout.CacheRef())
			{
				InputControlLayout inputControlLayout = InputControlLayout.cache.FindOrLoadLayout(new InternedString(layoutName), false);
				if (inputControlLayout == null)
				{
					text = null;
				}
				else
				{
					text = InputControlPath.FindControlLayoutRecursive(ref parser, inputControlLayout);
				}
			}
			return text;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00012CB8 File Offset: 0x00010EB8
		private static string FindControlLayoutRecursive(ref InputControlPath.PathParser parser, InputControlLayout layout)
		{
			string text = null;
			int count = layout.controls.Count;
			for (int i = 0; i < count; i++)
			{
				if (InputControlPath.ControlLayoutMatchesPathComponent(ref layout.m_Controls[i], ref parser))
				{
					InternedString layout2 = layout.m_Controls[i].layout;
					if (!parser.isAtEnd)
					{
						InputControlPath.PathParser pathParser = parser;
						if (pathParser.MoveToNextComponent())
						{
							string text2 = InputControlPath.FindControlLayoutRecursive(ref pathParser, layout2);
							if (text2 != null)
							{
								if (text != null && text2 != text)
								{
									return null;
								}
								text = text2;
							}
						}
					}
					else
					{
						if (text != null && layout2 != text)
						{
							return null;
						}
						text = layout2.ToString();
					}
				}
			}
			return text;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00012D70 File Offset: 0x00010F70
		private static bool ControlLayoutMatchesPathComponent(ref InputControlLayout.ControlItem controlItem, ref InputControlPath.PathParser parser)
		{
			Substring layout = parser.current.m_Layout;
			if (layout.length > 0 && !InputControlPath.StringMatches(layout, controlItem.layout))
			{
				return false;
			}
			if (parser.current.m_Usages.length > 0)
			{
				for (int i = 0; i < parser.current.m_Usages.length; i++)
				{
					Substring substring = parser.current.m_Usages[i];
					if (substring.length > 0)
					{
						int count = controlItem.usages.Count;
						bool flag = false;
						for (int j = 0; j < count; j++)
						{
							if (InputControlPath.StringMatches(substring, controlItem.usages[j]))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							return false;
						}
					}
				}
			}
			Substring name = parser.current.m_Name;
			return name.length <= 0 || InputControlPath.StringMatches(name, controlItem.name);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00012E60 File Offset: 0x00011060
		private static bool StringMatches(Substring str, InternedString matchTo)
		{
			int length = str.length;
			int length2 = matchTo.length;
			string text = matchTo.ToLower();
			int num = 0;
			int num2 = 0;
			while (num2 < length && num < length2)
			{
				char c = str[num2];
				if (c == '\\' && num2 + 1 < length)
				{
					c = str[++num2];
				}
				if (c == '*')
				{
					if (num2 == length - 1)
					{
						return true;
					}
					num2++;
					c = char.ToLowerInvariant(str[num2]);
					while (num < length2 && text[num] != c)
					{
						num++;
					}
					if (num == length2)
					{
						return false;
					}
				}
				else if (char.ToLowerInvariant(c) != text[num])
				{
					return false;
				}
				num++;
				num2++;
			}
			return num == length2 && num2 == length;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00012F2A File Offset: 0x0001112A
		public static InputControl TryFindControl(InputControl control, string path, int indexInPath = 0)
		{
			return InputControlPath.TryFindControl<InputControl>(control, path, indexInPath);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00012F34 File Offset: 0x00011134
		public static InputControl[] TryFindControls(InputControl control, string path, int indexInPath = 0)
		{
			InputControlList<InputControl> inputControlList = new InputControlList<InputControl>(Allocator.Temp, 0);
			InputControl[] array;
			try
			{
				InputControlPath.TryFindControls<InputControl>(control, path, indexInPath, ref inputControlList);
				array = inputControlList.ToArray(false);
			}
			finally
			{
				inputControlList.Dispose();
			}
			return array;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00012F7C File Offset: 0x0001117C
		public static int TryFindControls(InputControl control, string path, ref InputControlList<InputControl> matches, int indexInPath = 0)
		{
			return InputControlPath.TryFindControls<InputControl>(control, path, indexInPath, ref matches);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00012F88 File Offset: 0x00011188
		public static TControl TryFindControl<TControl>(InputControl control, string path, int indexInPath = 0) where TControl : InputControl
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (string.IsNullOrEmpty(path))
			{
				return default(TControl);
			}
			if (indexInPath == 0 && path[0] == '/')
			{
				indexInPath++;
			}
			InputControlList<TControl> inputControlList = default(InputControlList<TControl>);
			return InputControlPath.MatchControlsRecursive<TControl>(control, path, indexInPath, ref inputControlList, false);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00012FDC File Offset: 0x000111DC
		public static int TryFindControls<TControl>(InputControl control, string path, int indexInPath, ref InputControlList<TControl> matches) where TControl : InputControl
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (indexInPath == 0 && path[0] == '/')
			{
				indexInPath++;
			}
			int count = matches.Count;
			InputControlPath.MatchControlsRecursive<TControl>(control, path, indexInPath, ref matches, true);
			return matches.Count - count;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00013032 File Offset: 0x00011232
		public static InputControl TryFindChild(InputControl control, string path, int indexInPath = 0)
		{
			return InputControlPath.TryFindChild<InputControl>(control, path, indexInPath);
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001303C File Offset: 0x0001123C
		public static TControl TryFindChild<TControl>(InputControl control, string path, int indexInPath = 0) where TControl : InputControl
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			ReadOnlyArray<InputControl> children = control.children;
			int count = children.Count;
			for (int i = 0; i < count; i++)
			{
				TControl tcontrol = InputControlPath.TryFindControl<TControl>(children[i], path, indexInPath);
				if (tcontrol != null)
				{
					return tcontrol;
				}
			}
			return default(TControl);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000130A4 File Offset: 0x000112A4
		public static bool Matches(string expected, InputControl control)
		{
			if (string.IsNullOrEmpty(expected))
			{
				throw new ArgumentNullException("expected");
			}
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			InputControlPath.PathParser pathParser = new InputControlPath.PathParser(expected);
			return InputControlPath.MatchesRecursive(ref pathParser, control, false);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000130E4 File Offset: 0x000112E4
		internal static bool MatchControlComponent(ref InputControlPath.ParsedPathComponent expectedControlComponent, ref InputControlLayout.ControlItem controlItem, bool matchAlias = false)
		{
			bool flag = false;
			bool flag2 = false;
			if (!expectedControlComponent.m_Name.isEmpty)
			{
				if (InputControlPath.StringMatches(expectedControlComponent.m_Name, controlItem.name))
				{
					flag = true;
				}
				else
				{
					if (!matchAlias)
					{
						return false;
					}
					ReadOnlyArray<InternedString> aliases = controlItem.aliases;
					for (int i = 0; i < aliases.Count; i++)
					{
						if (InputControlPath.StringMatches(expectedControlComponent.m_Name, aliases[i]))
						{
							flag = true;
							break;
						}
					}
				}
			}
			foreach (Substring substring in expectedControlComponent.m_Usages)
			{
				if (!substring.isEmpty)
				{
					int count = controlItem.usages.Count;
					for (int j = 0; j < count; j++)
					{
						if (InputControlPath.StringMatches(substring, controlItem.usages[j]))
						{
							flag2 = true;
							break;
						}
					}
				}
			}
			return flag || flag2;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x000131E0 File Offset: 0x000113E0
		public static bool MatchesPrefix(string expected, InputControl control)
		{
			if (string.IsNullOrEmpty(expected))
			{
				throw new ArgumentNullException("expected");
			}
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			InputControlPath.PathParser pathParser = new InputControlPath.PathParser(expected);
			return InputControlPath.MatchesRecursive(ref pathParser, control, true) && pathParser.isAtEnd;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00013230 File Offset: 0x00011430
		private static bool MatchesRecursive(ref InputControlPath.PathParser parser, InputControl currentControl, bool prefixOnly = false)
		{
			InputControl parent = currentControl.parent;
			if (parent != null && !InputControlPath.MatchesRecursive(ref parser, parent, prefixOnly))
			{
				return false;
			}
			if (!parser.MoveToNextComponent())
			{
				return prefixOnly;
			}
			return parser.current.Matches(currentControl);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001326C File Offset: 0x0001146C
		private static TControl MatchControlsRecursive<TControl>(InputControl control, string path, int indexInPath, ref InputControlList<TControl> matches, bool matchMultiple) where TControl : InputControl
		{
			int length = path.Length;
			bool flag = true;
			if (path[indexInPath] == '<')
			{
				indexInPath++;
				flag = InputControlPath.MatchPathComponent(control.layout, path, ref indexInPath, InputControlPath.PathComponentType.Layout, 0);
				if (!flag)
				{
					InternedString layout = control.m_Layout;
					while (InputControlLayout.s_Layouts.baseLayoutTable.TryGetValue(layout, out layout))
					{
						flag = InputControlPath.MatchPathComponent(layout, path, ref indexInPath, InputControlPath.PathComponentType.Layout, 0);
						if (flag)
						{
							break;
						}
					}
				}
			}
			while (indexInPath < length && path[indexInPath] == '{' && flag)
			{
				indexInPath++;
				for (int i = 0; i < control.usages.Count; i++)
				{
					flag = InputControlPath.MatchPathComponent(control.usages[i], path, ref indexInPath, InputControlPath.PathComponentType.Usage, 0);
					if (flag)
					{
						break;
					}
				}
			}
			if (indexInPath < length - 1 && flag && path[indexInPath] == '#' && path[indexInPath + 1] == '(')
			{
				indexInPath += 2;
				flag = InputControlPath.MatchPathComponent(control.displayName, path, ref indexInPath, InputControlPath.PathComponentType.DisplayName, 0);
			}
			if (indexInPath < length && flag && path[indexInPath] != '/')
			{
				flag = InputControlPath.MatchPathComponent(control.name, path, ref indexInPath, InputControlPath.PathComponentType.Name, 0);
				if (!flag)
				{
					int num = 0;
					while (num < control.aliases.Count && !flag)
					{
						flag = InputControlPath.MatchPathComponent(control.aliases[num], path, ref indexInPath, InputControlPath.PathComponentType.Name, 0);
						num++;
					}
				}
			}
			if (flag)
			{
				if (indexInPath < length && path[indexInPath] == '*')
				{
					indexInPath++;
				}
				if (indexInPath == length)
				{
					TControl tcontrol = control as TControl;
					if (tcontrol == null)
					{
						return default(TControl);
					}
					if (matchMultiple)
					{
						matches.Add(tcontrol);
					}
					return tcontrol;
				}
				else if (path[indexInPath] == '/')
				{
					indexInPath++;
					if (indexInPath != length)
					{
						TControl tcontrol2;
						if (path[indexInPath] == '{')
						{
							tcontrol2 = InputControlPath.MatchByUsageAtDeviceRootRecursive<TControl>(control.device, path, indexInPath, ref matches, matchMultiple);
						}
						else
						{
							tcontrol2 = InputControlPath.MatchChildrenRecursive<TControl>(control, path, indexInPath, ref matches, matchMultiple);
						}
						return tcontrol2;
					}
					TControl tcontrol3 = control as TControl;
					if (tcontrol3 == null)
					{
						return default(TControl);
					}
					if (matchMultiple)
					{
						matches.Add(tcontrol3);
					}
					return tcontrol3;
				}
			}
			return default(TControl);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000134A0 File Offset: 0x000116A0
		private static TControl MatchByUsageAtDeviceRootRecursive<TControl>(InputDevice device, string path, int indexInPath, ref InputControlList<TControl> matches, bool matchMultiple) where TControl : InputControl
		{
			InternedString[] usagesForEachControl = device.m_UsagesForEachControl;
			if (usagesForEachControl == null)
			{
				return default(TControl);
			}
			int num = device.m_UsageToControl.LengthSafe<InputControl>();
			int num2 = indexInPath + 1;
			bool flag = InputControlPath.PathComponentCanYieldMultipleMatches(path, indexInPath);
			int length = path.Length;
			indexInPath++;
			if (indexInPath == length)
			{
				throw new ArgumentException("Invalid path spec '" + path + "'; trailing '{'", "path");
			}
			TControl tcontrol = default(TControl);
			for (int i = 0; i < num; i++)
			{
				if (!InputControlPath.MatchPathComponent(usagesForEachControl[i], path, ref indexInPath, InputControlPath.PathComponentType.Usage, 0))
				{
					indexInPath = num2;
				}
				else
				{
					InputControl inputControl = device.m_UsageToControl[i];
					if (indexInPath < length && path[indexInPath] == '/')
					{
						tcontrol = InputControlPath.MatchChildrenRecursive<TControl>(inputControl, path, indexInPath + 1, ref matches, matchMultiple);
						if (tcontrol != null && !flag)
						{
							break;
						}
						if (tcontrol != null && !matchMultiple)
						{
							break;
						}
					}
					else
					{
						tcontrol = inputControl as TControl;
						if (tcontrol != null)
						{
							if (!matchMultiple)
							{
								break;
							}
							matches.Add(tcontrol);
						}
					}
				}
			}
			return tcontrol;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000135B0 File Offset: 0x000117B0
		private static TControl MatchChildrenRecursive<TControl>(InputControl control, string path, int indexInPath, ref InputControlList<TControl> matches, bool matchMultiple) where TControl : InputControl
		{
			ReadOnlyArray<InputControl> children = control.children;
			int count = children.Count;
			TControl tcontrol = default(TControl);
			bool flag = InputControlPath.PathComponentCanYieldMultipleMatches(path, indexInPath);
			for (int i = 0; i < count; i++)
			{
				TControl tcontrol2 = InputControlPath.MatchControlsRecursive<TControl>(children[i], path, indexInPath, ref matches, matchMultiple);
				if (tcontrol2 != null)
				{
					if (!flag)
					{
						return tcontrol2;
					}
					if (!matchMultiple)
					{
						return tcontrol2;
					}
					tcontrol = tcontrol2;
				}
			}
			return tcontrol;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001361C File Offset: 0x0001181C
		private static bool MatchPathComponent(string component, string path, ref int indexInPath, InputControlPath.PathComponentType componentType, int startIndexInComponent = 0)
		{
			int length = component.Length;
			int length2 = path.Length;
			int num = indexInPath;
			int num2 = startIndexInComponent;
			while (indexInPath < length2)
			{
				char c = path[indexInPath];
				if (c == '\\' && indexInPath + 1 < length2)
				{
					indexInPath++;
					c = path[indexInPath];
				}
				else
				{
					if (c == '/' && componentType == InputControlPath.PathComponentType.Name)
					{
						break;
					}
					if ((c == '>' && componentType == InputControlPath.PathComponentType.Layout) || (c == '}' && componentType == InputControlPath.PathComponentType.Usage) || (c == ')' && componentType == InputControlPath.PathComponentType.DisplayName))
					{
						indexInPath++;
						break;
					}
					if (c == '*')
					{
						int num3 = indexInPath + 1;
						if (indexInPath < length2 - 1 && num2 < length && InputControlPath.MatchPathComponent(component, path, ref num3, componentType, num2))
						{
							indexInPath = num3;
							return true;
						}
						if (num2 < length)
						{
							num2++;
							continue;
						}
						return true;
					}
				}
				if (num2 == length)
				{
					indexInPath = num;
					return false;
				}
				char c2 = component[num2];
				if (c2 != c && char.ToLowerInvariant(c2) != char.ToLowerInvariant(c))
				{
					indexInPath = num;
					return false;
				}
				num2++;
				indexInPath++;
			}
			if (num2 == length)
			{
				return true;
			}
			indexInPath = num;
			return false;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00013720 File Offset: 0x00011920
		private static bool PathComponentCanYieldMultipleMatches(string path, int indexInPath)
		{
			int num = path.IndexOf('/', indexInPath);
			if (num == -1)
			{
				return path.IndexOf('*', indexInPath) != -1 || path.IndexOf('<', indexInPath) != -1;
			}
			int num2 = num - indexInPath;
			return path.IndexOf('*', indexInPath, num2) != -1 || path.IndexOf('<', indexInPath, num2) != -1;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001377C File Offset: 0x0001197C
		public static IEnumerable<InputControlPath.ParsedPathComponent> Parse(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			InputControlPath.PathParser parser = new InputControlPath.PathParser(path);
			while (parser.MoveToNextComponent())
			{
				yield return parser.current;
			}
			yield break;
		}

		// Token: 0x04000149 RID: 329
		public const string Wildcard = "*";

		// Token: 0x0400014A RID: 330
		public const string DoubleWildcard = "**";

		// Token: 0x0400014B RID: 331
		public const char Separator = '/';

		// Token: 0x0400014C RID: 332
		internal const char SeparatorReplacement = ' ';

		// Token: 0x02000195 RID: 405
		[Flags]
		public enum HumanReadableStringOptions
		{
			// Token: 0x040008D7 RID: 2263
			None = 0,
			// Token: 0x040008D8 RID: 2264
			OmitDevice = 2,
			// Token: 0x040008D9 RID: 2265
			UseShortNames = 4
		}

		// Token: 0x02000196 RID: 406
		private enum PathComponentType
		{
			// Token: 0x040008DB RID: 2267
			Name,
			// Token: 0x040008DC RID: 2268
			DisplayName,
			// Token: 0x040008DD RID: 2269
			Usage,
			// Token: 0x040008DE RID: 2270
			Layout
		}

		// Token: 0x02000197 RID: 407
		public struct ParsedPathComponent
		{
			// Token: 0x1700056A RID: 1386
			// (get) Token: 0x06001411 RID: 5137 RVA: 0x0005D02C File Offset: 0x0005B22C
			public string layout
			{
				get
				{
					return this.m_Layout.ToString();
				}
			}

			// Token: 0x1700056B RID: 1387
			// (get) Token: 0x06001412 RID: 5138 RVA: 0x0005D03F File Offset: 0x0005B23F
			public IEnumerable<string> usages
			{
				get
				{
					return this.m_Usages.Select<Substring, string>((Substring x) => x.ToString());
				}
			}

			// Token: 0x1700056C RID: 1388
			// (get) Token: 0x06001413 RID: 5139 RVA: 0x0005D070 File Offset: 0x0005B270
			public string name
			{
				get
				{
					return this.m_Name.ToString();
				}
			}

			// Token: 0x1700056D RID: 1389
			// (get) Token: 0x06001414 RID: 5140 RVA: 0x0005D083 File Offset: 0x0005B283
			public string displayName
			{
				get
				{
					return this.m_DisplayName.ToString();
				}
			}

			// Token: 0x1700056E RID: 1390
			// (get) Token: 0x06001415 RID: 5141 RVA: 0x0005D096 File Offset: 0x0005B296
			internal bool isWildcard
			{
				get
				{
					return this.m_Name == "*";
				}
			}

			// Token: 0x1700056F RID: 1391
			// (get) Token: 0x06001416 RID: 5142 RVA: 0x0005D0AD File Offset: 0x0005B2AD
			internal bool isDoubleWildcard
			{
				get
				{
					return this.m_Name == "**";
				}
			}

			// Token: 0x06001417 RID: 5143 RVA: 0x0005D0C4 File Offset: 0x0005B2C4
			internal string ToHumanReadableString(string parentLayoutName, string parentControlPath, out string referencedLayoutName, out string controlPath, InputControlPath.HumanReadableStringOptions options)
			{
				referencedLayoutName = null;
				controlPath = null;
				string text = string.Empty;
				if (this.isWildcard)
				{
					text += "Any";
				}
				if (this.m_Usages.length > 0)
				{
					string text2 = string.Empty;
					for (int i = 0; i < this.m_Usages.length; i++)
					{
						if (!this.m_Usages[i].isEmpty)
						{
							if (text2 != string.Empty)
							{
								text2 = text2 + " & " + InputControlPath.ParsedPathComponent.ToHumanReadableString(this.m_Usages[i]);
							}
							else
							{
								text2 = InputControlPath.ParsedPathComponent.ToHumanReadableString(this.m_Usages[i]);
							}
						}
					}
					if (text2 != string.Empty)
					{
						if (text != string.Empty)
						{
							text = text + " " + text2;
						}
						else
						{
							text += text2;
						}
					}
				}
				if (!this.m_Layout.isEmpty)
				{
					referencedLayoutName = this.m_Layout.ToString();
					InputControlLayout inputControlLayout = InputControlLayout.cache.FindOrLoadLayout(referencedLayoutName, false);
					string text3;
					if (inputControlLayout != null && !string.IsNullOrEmpty(inputControlLayout.m_DisplayName))
					{
						text3 = inputControlLayout.m_DisplayName;
					}
					else
					{
						text3 = InputControlPath.ParsedPathComponent.ToHumanReadableString(this.m_Layout);
					}
					if (!string.IsNullOrEmpty(text))
					{
						text = text + " " + text3;
					}
					else
					{
						text += text3;
					}
				}
				if (!this.m_Name.isEmpty && !this.isWildcard)
				{
					string text4 = null;
					if (!string.IsNullOrEmpty(parentLayoutName))
					{
						InputControlLayout inputControlLayout2 = InputControlLayout.cache.FindOrLoadLayout(new InternedString(parentLayoutName), false);
						if (inputControlLayout2 != null)
						{
							InternedString internedString = new InternedString(this.m_Name.ToString());
							int num;
							InputControlLayout.ControlItem? controlItem = inputControlLayout2.FindControlIncludingArrayElements(internedString, out num);
							if (controlItem != null)
							{
								if (string.IsNullOrEmpty(parentControlPath))
								{
									if (num != -1)
									{
										controlPath = string.Format("{0}{1}", controlItem.Value.name, num);
									}
									else
									{
										controlPath = controlItem.Value.name;
									}
								}
								else if (num != -1)
								{
									controlPath = string.Format("{0}/{1}{2}", parentControlPath, controlItem.Value.name, num);
								}
								else
								{
									controlPath = string.Format("{0}/{1}", parentControlPath, controlItem.Value.name);
								}
								string text5 = (((options & InputControlPath.HumanReadableStringOptions.UseShortNames) != InputControlPath.HumanReadableStringOptions.None) ? controlItem.Value.shortDisplayName : null);
								string text6 = ((!string.IsNullOrEmpty(text5)) ? text5 : controlItem.Value.displayName);
								if (!string.IsNullOrEmpty(text6))
								{
									if (num != -1)
									{
										text4 = string.Format("{0} #{1}", text6, num);
									}
									else
									{
										text4 = text6;
									}
								}
								if (string.IsNullOrEmpty(referencedLayoutName))
								{
									referencedLayoutName = controlItem.Value.layout;
								}
							}
						}
					}
					if (text4 == null)
					{
						text4 = InputControlPath.ParsedPathComponent.ToHumanReadableString(this.m_Name);
					}
					if (!string.IsNullOrEmpty(text))
					{
						text = text + " " + text4;
					}
					else
					{
						text += text4;
					}
				}
				if (!this.m_DisplayName.isEmpty)
				{
					string text7 = "\"" + InputControlPath.ParsedPathComponent.ToHumanReadableString(this.m_DisplayName) + "\"";
					if (!string.IsNullOrEmpty(text))
					{
						text = text + " " + text7;
					}
					else
					{
						text += text7;
					}
				}
				return text;
			}

			// Token: 0x06001418 RID: 5144 RVA: 0x0005D442 File Offset: 0x0005B642
			private static string ToHumanReadableString(Substring substring)
			{
				return substring.ToString().Unescape("/*{<", "/*{<");
			}

			// Token: 0x06001419 RID: 5145 RVA: 0x0005D460 File Offset: 0x0005B660
			public bool Matches(InputControl control)
			{
				if (!this.m_Layout.isEmpty)
				{
					bool flag = InputControlPath.ParsedPathComponent.ComparePathElementToString(this.m_Layout, control.layout);
					if (!flag)
					{
						InternedString layout = control.m_Layout;
						while (InputControlLayout.s_Layouts.baseLayoutTable.TryGetValue(layout, out layout) && !flag)
						{
							flag = InputControlPath.ParsedPathComponent.ComparePathElementToString(this.m_Layout, layout.ToString());
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (this.m_Usages.length > 0)
				{
					for (int i = 0; i < this.m_Usages.length; i++)
					{
						if (!this.m_Usages[i].isEmpty)
						{
							ReadOnlyArray<InternedString> usages = control.usages;
							bool flag2 = false;
							for (int j = 0; j < usages.Count; j++)
							{
								if (InputControlPath.ParsedPathComponent.ComparePathElementToString(this.m_Usages[i], usages[j]))
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								return false;
							}
						}
					}
				}
				return (this.m_Name.isEmpty || this.isWildcard || InputControlPath.ParsedPathComponent.ComparePathElementToString(this.m_Name, control.name)) && (this.m_DisplayName.isEmpty || InputControlPath.ParsedPathComponent.ComparePathElementToString(this.m_DisplayName, control.displayName));
			}

			// Token: 0x0600141A RID: 5146 RVA: 0x0005D5A4 File Offset: 0x0005B7A4
			private static bool ComparePathElementToString(Substring pathElement, string element)
			{
				int length = pathElement.length;
				int length2 = element.Length;
				int num = 0;
				int num2 = 0;
				bool flag;
				bool flag2;
				for (;;)
				{
					flag = num == length;
					flag2 = num2 == length2;
					if (flag || flag2)
					{
						break;
					}
					char c = pathElement[num];
					if (c == '\\' && num + 1 < length)
					{
						c = pathElement[++num];
					}
					if (char.ToLowerInvariant(c) != char.ToLowerInvariant(element[num2]))
					{
						return false;
					}
					num++;
					num2++;
				}
				return flag == flag2;
			}

			// Token: 0x040008DF RID: 2271
			internal Substring m_Layout;

			// Token: 0x040008E0 RID: 2272
			internal InlinedArray<Substring> m_Usages;

			// Token: 0x040008E1 RID: 2273
			internal Substring m_Name;

			// Token: 0x040008E2 RID: 2274
			internal Substring m_DisplayName;
		}

		// Token: 0x02000198 RID: 408
		private struct PathParser
		{
			// Token: 0x17000570 RID: 1392
			// (get) Token: 0x0600141B RID: 5147 RVA: 0x0005D622 File Offset: 0x0005B822
			public bool isAtEnd
			{
				get
				{
					return this.rightIndexInPath == this.length;
				}
			}

			// Token: 0x0600141C RID: 5148 RVA: 0x0005D632 File Offset: 0x0005B832
			public PathParser(string path)
			{
				this.path = path;
				this.length = path.Length;
				this.leftIndexInPath = 0;
				this.rightIndexInPath = 0;
				this.current = default(InputControlPath.ParsedPathComponent);
			}

			// Token: 0x0600141D RID: 5149 RVA: 0x0005D664 File Offset: 0x0005B864
			public bool MoveToNextComponent()
			{
				if (this.rightIndexInPath == this.length)
				{
					return false;
				}
				this.leftIndexInPath = this.rightIndexInPath;
				if (this.path[this.leftIndexInPath] == '/')
				{
					this.leftIndexInPath++;
					this.rightIndexInPath = this.leftIndexInPath;
					if (this.leftIndexInPath == this.length)
					{
						return false;
					}
				}
				Substring substring = default(Substring);
				if (this.rightIndexInPath < this.length && this.path[this.rightIndexInPath] == '<')
				{
					substring = this.ParseComponentPart('>');
				}
				InlinedArray<Substring> inlinedArray = default(InlinedArray<Substring>);
				while (this.rightIndexInPath < this.length && this.path[this.rightIndexInPath] == '{')
				{
					inlinedArray.AppendWithCapacity(this.ParseComponentPart('}'), 10);
				}
				Substring substring2 = default(Substring);
				if (this.rightIndexInPath < this.length - 1 && this.path[this.rightIndexInPath] == '#' && this.path[this.rightIndexInPath + 1] == '(')
				{
					this.rightIndexInPath++;
					substring2 = this.ParseComponentPart(')');
				}
				Substring substring3 = default(Substring);
				if (this.rightIndexInPath < this.length && this.path[this.rightIndexInPath] != '/')
				{
					substring3 = this.ParseComponentPart('/');
				}
				this.current = new InputControlPath.ParsedPathComponent
				{
					m_Layout = substring,
					m_Usages = inlinedArray,
					m_Name = substring3,
					m_DisplayName = substring2
				};
				return this.leftIndexInPath != this.rightIndexInPath;
			}

			// Token: 0x0600141E RID: 5150 RVA: 0x0005D810 File Offset: 0x0005BA10
			private Substring ParseComponentPart(char terminator)
			{
				if (terminator != '/')
				{
					this.rightIndexInPath++;
				}
				int num = this.rightIndexInPath;
				while (this.rightIndexInPath < this.length && this.path[this.rightIndexInPath] != terminator)
				{
					if (this.path[this.rightIndexInPath] == '\\' && this.rightIndexInPath + 1 < this.length)
					{
						this.rightIndexInPath++;
					}
					this.rightIndexInPath++;
				}
				int num2 = this.rightIndexInPath - num;
				if (this.rightIndexInPath < this.length && terminator != '/')
				{
					this.rightIndexInPath++;
				}
				return new Substring(this.path, num, num2);
			}

			// Token: 0x040008E3 RID: 2275
			private string path;

			// Token: 0x040008E4 RID: 2276
			private int length;

			// Token: 0x040008E5 RID: 2277
			private int leftIndexInPath;

			// Token: 0x040008E6 RID: 2278
			private int rightIndexInPath;

			// Token: 0x040008E7 RID: 2279
			public InputControlPath.ParsedPathComponent current;
		}
	}
}
