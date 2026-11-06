using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Internal
{
	// Token: 0x02000016 RID: 22
	[NativeHeader("Runtime/Input/InputBindings.h")]
	internal static class InputUnsafeUtility
	{
		// Token: 0x060000B3 RID: 179
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetKeyString(string name);

		// Token: 0x060000B4 RID: 180
		[NativeThrows]
		[RequiredMember]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern bool GetKeyString__Unmanaged(byte* name, int nameLen);

		// Token: 0x060000B5 RID: 181
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetKeyUpString(string name);

		// Token: 0x060000B6 RID: 182
		[NativeThrows]
		[RequiredMember]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern bool GetKeyUpString__Unmanaged(byte* name, int nameLen);

		// Token: 0x060000B7 RID: 183
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetKeyDownString(string name);

		// Token: 0x060000B8 RID: 184
		[NativeThrows]
		[RequiredMember]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern bool GetKeyDownString__Unmanaged(byte* name, int nameLen);

		// Token: 0x060000B9 RID: 185
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetAxis(string axisName);

		// Token: 0x060000BA RID: 186
		[NativeThrows]
		[RequiredMember]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern float GetAxis__Unmanaged(byte* axisName, int axisNameLen);

		// Token: 0x060000BB RID: 187
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern float GetAxisRaw(string axisName);

		// Token: 0x060000BC RID: 188
		[RequiredMember]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern float GetAxisRaw__Unmanaged(byte* axisName, int axisNameLen);

		// Token: 0x060000BD RID: 189
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetButton(string buttonName);

		// Token: 0x060000BE RID: 190
		[NativeThrows]
		[RequiredMember]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern bool GetButton__Unmanaged(byte* buttonName, int buttonNameLen);

		// Token: 0x060000BF RID: 191
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetButtonDown(string buttonName);

		// Token: 0x060000C0 RID: 192
		[NativeThrows]
		[RequiredMember]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern byte GetButtonDown__Unmanaged(byte* buttonName, int buttonNameLen);

		// Token: 0x060000C1 RID: 193
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool GetButtonUp(string buttonName);

		// Token: 0x060000C2 RID: 194
		[RequiredMember]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern bool GetButtonUp__Unmanaged(byte* buttonName, int buttonNameLen);
	}
}
