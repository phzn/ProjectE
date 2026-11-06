using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200002A RID: 42
	[NativeHeader("Modules/IMGUI/GUIStyle.bindings.h")]
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class GUIStyleState
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060002E2 RID: 738
		// (set) Token: 0x060002E3 RID: 739
		[NativeProperty("Background", false, TargetType.Function)]
		public extern Texture2D background
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000B918 File Offset: 0x00009B18
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x0000B92E File Offset: 0x00009B2E
		[NativeProperty("textColor", false, TargetType.Field)]
		public Color textColor
		{
			get
			{
				Color color;
				this.get_textColor_Injected(out color);
				return color;
			}
			set
			{
				this.set_textColor_Injected(ref value);
			}
		}

		// Token: 0x060002E6 RID: 742
		[FreeFunction(Name = "GUIStyleState_Bindings::Init", IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr Init();

		// Token: 0x060002E7 RID: 743
		[FreeFunction(Name = "GUIStyleState_Bindings::Cleanup", IsThreadSafe = true, HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Cleanup();

		// Token: 0x060002E8 RID: 744 RVA: 0x0000B938 File Offset: 0x00009B38
		public GUIStyleState()
		{
			this.m_Ptr = GUIStyleState.Init();
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000B94D File Offset: 0x00009B4D
		private GUIStyleState(GUIStyle sourceStyle, IntPtr source)
		{
			this.m_SourceStyle = sourceStyle;
			this.m_Ptr = source;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000B968 File Offset: 0x00009B68
		internal static GUIStyleState ProduceGUIStyleStateFromDeserialization(GUIStyle sourceStyle, IntPtr source)
		{
			return new GUIStyleState(sourceStyle, source);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000B984 File Offset: 0x00009B84
		internal static GUIStyleState GetGUIStyleState(GUIStyle sourceStyle, IntPtr source)
		{
			return new GUIStyleState(sourceStyle, source);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000B9A0 File Offset: 0x00009BA0
		protected override void Finalize()
		{
			try
			{
				bool flag = this.m_SourceStyle == null;
				if (flag)
				{
					this.Cleanup();
					this.m_Ptr = IntPtr.Zero;
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x060002ED RID: 749
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_textColor_Injected(out Color ret);

		// Token: 0x060002EE RID: 750
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_textColor_Injected(ref Color value);

		// Token: 0x040000CA RID: 202
		[NonSerialized]
		internal IntPtr m_Ptr;

		// Token: 0x040000CB RID: 203
		private readonly GUIStyle m_SourceStyle;
	}
}
