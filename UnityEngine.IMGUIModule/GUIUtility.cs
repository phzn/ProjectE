using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000030 RID: 48
	[NativeHeader("Runtime/Camera/RenderLayers/GUITexture.h")]
	[NativeHeader("Runtime/Utilities/CopyPaste.h")]
	[NativeHeader("Modules/IMGUI/GUIUtility.h")]
	[NativeHeader("Runtime/Input/InputManager.h")]
	[NativeHeader("Modules/IMGUI/GUIManager.h")]
	[NativeHeader("Runtime/Input/InputBindings.h")]
	public class GUIUtility
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000382 RID: 898
		public static extern bool hasModalWindow
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000383 RID: 899
		[NativeProperty("GetGUIState().m_PixelsPerPoint", true, TargetType.Field)]
		internal static extern float pixelsPerPoint
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000384 RID: 900
		[NativeProperty("GetGUIState().m_OnGUIDepth", true, TargetType.Field)]
		internal static extern int guiDepth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000385 RID: 901 RVA: 0x0000C658 File Offset: 0x0000A858
		// (set) Token: 0x06000386 RID: 902 RVA: 0x0000C66D File Offset: 0x0000A86D
		internal static Vector2 s_EditorScreenPointOffset
		{
			[NativeMethod("GetGUIState().GetGUIPixelOffset", true)]
			get
			{
				Vector2 vector;
				GUIUtility.get_s_EditorScreenPointOffset_Injected(out vector);
				return vector;
			}
			[NativeMethod("GetGUIState().SetGUIPixelOffset", true)]
			set
			{
				GUIUtility.set_s_EditorScreenPointOffset_Injected(ref value);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000387 RID: 903
		// (set) Token: 0x06000388 RID: 904
		[NativeProperty("GetGUIState().m_CanvasGUIState.m_IsMouseUsed", true, TargetType.Field)]
		internal static extern bool mouseUsed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000389 RID: 905
		// (set) Token: 0x0600038A RID: 906
		[StaticAccessor("GetInputManager()", StaticAccessorType.Dot)]
		internal static extern bool textFieldInput
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600038B RID: 907
		// (set) Token: 0x0600038C RID: 908
		internal static extern bool manualTex2SRGBEnabled
		{
			[FreeFunction("GUITexture::IsManualTex2SRGBEnabled")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("GUITexture::SetManualTex2SRGBEnabled")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600038D RID: 909
		// (set) Token: 0x0600038E RID: 910
		public static extern string systemCopyBuffer
		{
			[FreeFunction("GetCopyBuffer")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("SetCopyBuffer")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000C676 File Offset: 0x0000A876
		[FreeFunction("GetGUIState().GetControlID")]
		private static int Internal_GetControlID(int hint, FocusType focusType, Rect rect)
		{
			return GUIUtility.Internal_GetControlID_Injected(hint, focusType, ref rect);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000C684 File Offset: 0x0000A884
		public static int GetControlID(int hint, FocusType focusType, Rect rect)
		{
			GUIUtility.s_ControlCount++;
			return GUIUtility.Internal_GetControlID(hint, focusType, rect);
		}

		// Token: 0x06000391 RID: 913
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void BeginContainerFromOwner(ScriptableObject owner);

		// Token: 0x06000392 RID: 914
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void BeginContainer(ObjectGUIState objectGUIState);

		// Token: 0x06000393 RID: 915
		[NativeMethod("EndContainer")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_EndContainer();

		// Token: 0x06000394 RID: 916
		[FreeFunction("GetSpecificGUIState(0).m_EternalGUIState->GetNextUniqueID")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetPermanentControlID();

		// Token: 0x06000395 RID: 917
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int CheckForTabEvent(Event evt);

		// Token: 0x06000396 RID: 918
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetKeyboardControlToFirstControlId();

		// Token: 0x06000397 RID: 919
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetKeyboardControlToLastControlId();

		// Token: 0x06000398 RID: 920
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool HasFocusableControls();

		// Token: 0x06000399 RID: 921
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool OwnsId(int id);

		// Token: 0x0600039A RID: 922 RVA: 0x0000C6AC File Offset: 0x0000A8AC
		public static Rect AlignRectToDevice(Rect rect, out int widthInPixels, out int heightInPixels)
		{
			Rect rect2;
			GUIUtility.AlignRectToDevice_Injected(ref rect, out widthInPixels, out heightInPixels, out rect2);
			return rect2;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600039B RID: 923
		[StaticAccessor("InputBindings", StaticAccessorType.DoubleColon)]
		internal static extern string compositionString
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600039C RID: 924
		// (set) Token: 0x0600039D RID: 925
		[StaticAccessor("InputBindings", StaticAccessorType.DoubleColon)]
		internal static extern IMECompositionMode imeCompositionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0000C6C8 File Offset: 0x0000A8C8
		// (set) Token: 0x0600039F RID: 927 RVA: 0x0000C6DD File Offset: 0x0000A8DD
		[StaticAccessor("InputBindings", StaticAccessorType.DoubleColon)]
		internal static Vector2 compositionCursorPos
		{
			get
			{
				Vector2 vector;
				GUIUtility.get_compositionCursorPos_Injected(out vector);
				return vector;
			}
			set
			{
				GUIUtility.set_compositionCursorPos_Injected(ref value);
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000C6E8 File Offset: 0x0000A8E8
		internal static Vector3 Internal_MultiplyPoint(Vector3 point, Matrix4x4 transform)
		{
			Vector3 vector;
			GUIUtility.Internal_MultiplyPoint_Injected(ref point, ref transform, out vector);
			return vector;
		}

		// Token: 0x060003A1 RID: 929
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetChanged();

		// Token: 0x060003A2 RID: 930
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetChanged(bool changed);

		// Token: 0x060003A3 RID: 931
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetDidGUIWindowsEatLastEvent(bool value);

		// Token: 0x060003A4 RID: 932
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetHotControl();

		// Token: 0x060003A5 RID: 933
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetKeyboardControl();

		// Token: 0x060003A6 RID: 934
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetHotControl(int value);

		// Token: 0x060003A7 RID: 935
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetKeyboardControl(int value);

		// Token: 0x060003A8 RID: 936
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object Internal_GetDefaultSkin(int skinMode);

		// Token: 0x060003A9 RID: 937
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object Internal_GetBuiltinSkin(int skin);

		// Token: 0x060003AA RID: 938
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_ExitGUI();

		// Token: 0x060003AB RID: 939 RVA: 0x0000C704 File Offset: 0x0000A904
		private static Vector2 InternalWindowToScreenPoint(Vector2 windowPoint)
		{
			Vector2 vector;
			GUIUtility.InternalWindowToScreenPoint_Injected(ref windowPoint, out vector);
			return vector;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000C71C File Offset: 0x0000A91C
		private static Vector2 InternalScreenToWindowPoint(Vector2 screenPoint)
		{
			Vector2 vector;
			GUIUtility.InternalScreenToWindowPoint_Injected(ref screenPoint, out vector);
			return vector;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000C733 File Offset: 0x0000A933
		[RequiredByNativeCode]
		private static void MarkGUIChanged()
		{
			Action action = GUIUtility.guiChanged;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000C748 File Offset: 0x0000A948
		public static int GetControlID(FocusType focus)
		{
			return GUIUtility.GetControlID(0, focus);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000C764 File Offset: 0x0000A964
		public static int GetControlID(GUIContent contents, FocusType focus)
		{
			return GUIUtility.GetControlID(contents.hash, focus);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000C784 File Offset: 0x0000A984
		public static int GetControlID(FocusType focus, Rect position)
		{
			return GUIUtility.GetControlID(0, focus, position);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000C7A0 File Offset: 0x0000A9A0
		public static int GetControlID(GUIContent contents, FocusType focus, Rect position)
		{
			return GUIUtility.GetControlID(contents.hash, focus, position);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000C7C0 File Offset: 0x0000A9C0
		public static int GetControlID(int hint, FocusType focus)
		{
			return GUIUtility.GetControlID(hint, focus, Rect.zero);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000C7E0 File Offset: 0x0000A9E0
		public static object GetStateObject(Type t, int controlID)
		{
			return GUIStateObjects.GetStateObject(t, controlID);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000C7FC File Offset: 0x0000A9FC
		public static object QueryStateObject(Type t, int controlID)
		{
			return GUIStateObjects.QueryStateObject(t, controlID);
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000C815 File Offset: 0x0000AA15
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x0000C81C File Offset: 0x0000AA1C
		internal static bool guiIsExiting { get; set; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000C824 File Offset: 0x0000AA24
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x0000C83B File Offset: 0x0000AA3B
		public static int hotControl
		{
			get
			{
				return GUIUtility.Internal_GetHotControl();
			}
			set
			{
				GUIUtility.Internal_SetHotControl(value);
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000C845 File Offset: 0x0000AA45
		[RequiredByNativeCode]
		internal static void TakeCapture()
		{
			Action action = GUIUtility.takeCapture;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000C859 File Offset: 0x0000AA59
		[RequiredByNativeCode]
		internal static void RemoveCapture()
		{
			Action action = GUIUtility.releaseCapture;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0000C870 File Offset: 0x0000AA70
		// (set) Token: 0x060003BC RID: 956 RVA: 0x0000C887 File Offset: 0x0000AA87
		public static int keyboardControl
		{
			get
			{
				return GUIUtility.Internal_GetKeyboardControl();
			}
			set
			{
				GUIUtility.Internal_SetKeyboardControl(value);
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000C894 File Offset: 0x0000AA94
		internal static bool HasKeyFocus(int controlID)
		{
			return controlID == GUIUtility.keyboardControl && (GUIUtility.s_HasCurrentWindowKeyFocusFunc == null || GUIUtility.s_HasCurrentWindowKeyFocusFunc());
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000C8C5 File Offset: 0x0000AAC5
		public static void ExitGUI()
		{
			throw new ExitGUIException();
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
		internal static GUISkin GetDefaultSkin(int skinMode)
		{
			return GUIUtility.Internal_GetDefaultSkin(skinMode) as GUISkin;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000C8F0 File Offset: 0x0000AAF0
		internal static GUISkin GetDefaultSkin()
		{
			return GUIUtility.Internal_GetDefaultSkin(GUIUtility.s_SkinMode) as GUISkin;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000C914 File Offset: 0x0000AB14
		internal static GUISkin GetBuiltinSkin(int skin)
		{
			return GUIUtility.Internal_GetBuiltinSkin(skin) as GUISkin;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000C934 File Offset: 0x0000AB34
		[RequiredByNativeCode]
		internal static void ProcessEvent(int instanceID, IntPtr nativeEventPtr, out bool result)
		{
			bool flag = GUIUtility.beforeEventProcessed != null;
			if (flag)
			{
				GUIUtility.m_Event.CopyFromPtr(nativeEventPtr);
				GUIUtility.beforeEventProcessed(GUIUtility.m_Event.type, GUIUtility.m_Event.keyCode);
			}
			result = false;
			bool flag2 = GUIUtility.processEvent != null;
			if (flag2)
			{
				result = GUIUtility.processEvent(instanceID, nativeEventPtr);
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000C998 File Offset: 0x0000AB98
		internal static void EndContainer()
		{
			GUIUtility.Internal_EndContainer();
			GUIUtility.Internal_ExitGUI();
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000C9A7 File Offset: 0x0000ABA7
		internal static void CleanupRoots()
		{
			Action action = GUIUtility.cleanupRoots;
			if (action != null)
			{
				action();
			}
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000C9BC File Offset: 0x0000ABBC
		[RequiredByNativeCode]
		internal static void BeginGUI(int skinMode, int instanceID, int useGUILayout)
		{
			GUIUtility.s_SkinMode = skinMode;
			GUIUtility.s_OriginalID = instanceID;
			GUIUtility.ResetGlobalState();
			bool flag = useGUILayout != 0;
			if (flag)
			{
				GUILayoutUtility.Begin(instanceID);
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000C9ED File Offset: 0x0000ABED
		[RequiredByNativeCode]
		internal static void DestroyGUI(int instanceID)
		{
			GUILayoutUtility.RemoveSelectedIdList(instanceID, false);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000C9F8 File Offset: 0x0000ABF8
		[RequiredByNativeCode]
		internal static void EndGUI(int layoutType)
		{
			try
			{
				bool flag = Event.current.type == EventType.Layout;
				if (flag)
				{
					switch (layoutType)
					{
					case 1:
						GUILayoutUtility.Layout();
						break;
					case 2:
						GUILayoutUtility.LayoutFromEditorWindow();
						break;
					}
				}
				GUILayoutUtility.SelectIDList(GUIUtility.s_OriginalID, false);
				GUIContent.ClearStaticCache();
			}
			finally
			{
				GUIUtility.Internal_ExitGUI();
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000CA74 File Offset: 0x0000AC74
		[RequiredByNativeCode]
		internal static bool EndGUIFromException(Exception exception)
		{
			GUIUtility.Internal_ExitGUI();
			return GUIUtility.ShouldRethrowException(exception);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000CA94 File Offset: 0x0000AC94
		[RequiredByNativeCode]
		internal static bool EndContainerGUIFromException(Exception exception)
		{
			bool flag = GUIUtility.endContainerGUIFromException != null;
			return flag && GUIUtility.endContainerGUIFromException(exception);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000CAC1 File Offset: 0x0000ACC1
		internal static void ResetGlobalState()
		{
			GUI.skin = null;
			GUIUtility.guiIsExiting = false;
			GUI.changed = false;
			GUI.scrollViewStates.Clear();
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000CAE4 File Offset: 0x0000ACE4
		internal static bool IsExitGUIException(Exception exception)
		{
			while (exception is TargetInvocationException && exception.InnerException != null)
			{
				exception = exception.InnerException;
			}
			return exception is ExitGUIException;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000CB24 File Offset: 0x0000AD24
		internal static bool ShouldRethrowException(Exception exception)
		{
			return GUIUtility.IsExitGUIException(exception);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000CB3C File Offset: 0x0000AD3C
		internal static void CheckOnGUI()
		{
			bool flag = GUIUtility.guiDepth <= 0;
			if (flag)
			{
				throw new ArgumentException("You can only call GUI functions from inside OnGUI.");
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000CB64 File Offset: 0x0000AD64
		internal static float RoundToPixelGrid(float v)
		{
			return Mathf.Floor(v * GUIUtility.pixelsPerPoint + 0.48f) / GUIUtility.pixelsPerPoint;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000CB90 File Offset: 0x0000AD90
		internal static float RoundToPixelGrid(float v, float scale)
		{
			return Mathf.Floor(v * scale + 0.48f) / scale;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000CBB4 File Offset: 0x0000ADB4
		public static Vector2 GUIToScreenPoint(Vector2 guiPoint)
		{
			return GUIUtility.InternalWindowToScreenPoint(GUIClip.UnclipToWindow(guiPoint));
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000CBD4 File Offset: 0x0000ADD4
		public static Rect GUIToScreenRect(Rect guiRect)
		{
			Vector2 vector = GUIUtility.GUIToScreenPoint(new Vector2(guiRect.x, guiRect.y));
			guiRect.x = vector.x;
			guiRect.y = vector.y;
			return guiRect;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000CC1C File Offset: 0x0000AE1C
		public static Vector2 ScreenToGUIPoint(Vector2 screenPoint)
		{
			return GUIClip.ClipToWindow(GUIUtility.InternalScreenToWindowPoint(screenPoint));
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000CC3C File Offset: 0x0000AE3C
		public static Rect ScreenToGUIRect(Rect screenRect)
		{
			Vector2 vector = GUIUtility.ScreenToGUIPoint(new Vector2(screenRect.x, screenRect.y));
			screenRect.x = vector.x;
			screenRect.y = vector.y;
			return screenRect;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000CC84 File Offset: 0x0000AE84
		public static void RotateAroundPivot(float angle, Vector2 pivotPoint)
		{
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			Vector2 vector = GUIClip.Unclip(pivotPoint);
			Matrix4x4 matrix4x = Matrix4x4.TRS(vector, Quaternion.Euler(0f, 0f, angle), Vector3.one) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			GUI.matrix = matrix4x * matrix;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000CCF8 File Offset: 0x0000AEF8
		public static void ScaleAroundPivot(Vector2 scale, Vector2 pivotPoint)
		{
			Matrix4x4 matrix = GUI.matrix;
			Vector2 vector = GUIClip.Unclip(pivotPoint);
			Matrix4x4 matrix4x = Matrix4x4.TRS(vector, Quaternion.identity, new Vector3(scale.x, scale.y, 1f)) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			GUI.matrix = matrix4x * matrix;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000CD68 File Offset: 0x0000AF68
		public static Rect AlignRectToDevice(Rect rect)
		{
			int num;
			int num2;
			return GUIUtility.AlignRectToDevice(rect, out num, out num2);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x0000CD84 File Offset: 0x0000AF84
		internal static bool HitTest(Rect rect, Vector2 point, int offset)
		{
			return point.x >= rect.xMin - (float)offset && point.x < rect.xMax + (float)offset && point.y >= rect.yMin - (float)offset && point.y < rect.yMax + (float)offset;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000CDE4 File Offset: 0x0000AFE4
		internal static bool HitTest(Rect rect, Vector2 point, bool isDirectManipulationDevice)
		{
			int num = 0;
			return GUIUtility.HitTest(rect, point, num);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000CE00 File Offset: 0x0000B000
		internal static bool HitTest(Rect rect, Event evt)
		{
			return GUIUtility.HitTest(rect, evt.mousePosition, evt.isDirectManipulationDevice);
		}

		// Token: 0x060003DC RID: 988
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_s_EditorScreenPointOffset_Injected(out Vector2 ret);

		// Token: 0x060003DD RID: 989
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_s_EditorScreenPointOffset_Injected(ref Vector2 value);

		// Token: 0x060003DE RID: 990
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int Internal_GetControlID_Injected(int hint, FocusType focusType, ref Rect rect);

		// Token: 0x060003DF RID: 991
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void AlignRectToDevice_Injected(ref Rect rect, out int widthInPixels, out int heightInPixels, out Rect ret);

		// Token: 0x060003E0 RID: 992
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_compositionCursorPos_Injected(out Vector2 ret);

		// Token: 0x060003E1 RID: 993
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_compositionCursorPos_Injected(ref Vector2 value);

		// Token: 0x060003E2 RID: 994
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_MultiplyPoint_Injected(ref Vector3 point, ref Matrix4x4 transform, out Vector3 ret);

		// Token: 0x060003E3 RID: 995
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalWindowToScreenPoint_Injected(ref Vector2 windowPoint, out Vector2 ret);

		// Token: 0x060003E4 RID: 996
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void InternalScreenToWindowPoint_Injected(ref Vector2 screenPoint, out Vector2 ret);

		// Token: 0x040000E5 RID: 229
		internal static int s_ControlCount = 0;

		// Token: 0x040000E6 RID: 230
		internal static int s_SkinMode;

		// Token: 0x040000E7 RID: 231
		internal static int s_OriginalID;

		// Token: 0x040000E8 RID: 232
		internal static Action takeCapture;

		// Token: 0x040000E9 RID: 233
		internal static Action releaseCapture;

		// Token: 0x040000EA RID: 234
		internal static Func<int, IntPtr, bool> processEvent;

		// Token: 0x040000EB RID: 235
		internal static Action cleanupRoots;

		// Token: 0x040000EC RID: 236
		internal static Func<Exception, bool> endContainerGUIFromException;

		// Token: 0x040000ED RID: 237
		internal static Action guiChanged;

		// Token: 0x040000EE RID: 238
		internal static Action<EventType, KeyCode> beforeEventProcessed;

		// Token: 0x040000EF RID: 239
		private static Event m_Event = new Event();

		// Token: 0x040000F1 RID: 241
		internal static Func<bool> s_HasCurrentWindowKeyFocusFunc;
	}
}
