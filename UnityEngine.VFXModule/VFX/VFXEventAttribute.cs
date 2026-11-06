using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.VFX
{
	// Token: 0x02000010 RID: 16
	[RequiredByNativeCode]
	[NativeType(Header = "Modules/VFX/Public/VFXEventAttribute.h")]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class VFXEventAttribute : IDisposable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private VFXEventAttribute(IntPtr ptr, bool owner, VisualEffectAsset vfxAsset)
		{
			this.m_Ptr = ptr;
			this.m_Owner = owner;
			this.m_VfxAsset = vfxAsset;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000206F File Offset: 0x0000026F
		private VFXEventAttribute()
			: this(IntPtr.Zero, false, null)
		{
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002080 File Offset: 0x00000280
		internal static VFXEventAttribute CreateEventAttributeWrapper()
		{
			return new VFXEventAttribute(IntPtr.Zero, false, null);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020A0 File Offset: 0x000002A0
		internal void SetWrapValue(IntPtr ptrToEventAttribute)
		{
			bool owner = this.m_Owner;
			if (owner)
			{
				throw new Exception("VFXSpawnerState : SetWrapValue is reserved to CreateWrapper object");
			}
			this.m_Ptr = ptrToEventAttribute;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020CC File Offset: 0x000002CC
		public VFXEventAttribute(VFXEventAttribute original)
		{
			bool flag = original == null;
			if (flag)
			{
				throw new ArgumentNullException("VFXEventAttribute expect a non null attribute");
			}
			this.m_Ptr = VFXEventAttribute.Internal_Create();
			this.m_VfxAsset = original.m_VfxAsset;
			this.Internal_InitFromEventAttribute(original);
		}

		// Token: 0x06000006 RID: 6
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr Internal_Create();

		// Token: 0x06000007 RID: 7 RVA: 0x00002114 File Offset: 0x00000314
		internal static VFXEventAttribute Internal_InstanciateVFXEventAttribute(VisualEffectAsset vfxAsset)
		{
			VFXEventAttribute vfxeventAttribute = new VFXEventAttribute(VFXEventAttribute.Internal_Create(), true, vfxAsset);
			vfxeventAttribute.Internal_InitFromAsset(vfxAsset);
			return vfxeventAttribute;
		}

		// Token: 0x06000008 RID: 8
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_InitFromAsset(VisualEffectAsset vfxAsset);

		// Token: 0x06000009 RID: 9
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_InitFromEventAttribute(VFXEventAttribute vfxEventAttribute);

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000213C File Offset: 0x0000033C
		internal VisualEffectAsset vfxAsset
		{
			get
			{
				return this.m_VfxAsset;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002154 File Offset: 0x00000354
		private void Release()
		{
			bool flag = this.m_Owner && this.m_Ptr != IntPtr.Zero;
			if (flag)
			{
				VFXEventAttribute.Internal_Destroy(this.m_Ptr);
			}
			this.m_Ptr = IntPtr.Zero;
			this.m_VfxAsset = null;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021A4 File Offset: 0x000003A4
		~VFXEventAttribute()
		{
			this.Release();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021D4 File Offset: 0x000003D4
		public void Dispose()
		{
			this.Release();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600000E RID: 14
		[NativeMethod(IsThreadSafe = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Internal_Destroy(IntPtr ptr);

		// Token: 0x0600000F RID: 15
		[NativeName("HasValueFromScript<bool>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasBool(int nameID);

		// Token: 0x06000010 RID: 16
		[NativeName("HasValueFromScript<int>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasInt(int nameID);

		// Token: 0x06000011 RID: 17
		[NativeName("HasValueFromScript<UInt32>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasUint(int nameID);

		// Token: 0x06000012 RID: 18
		[NativeName("HasValueFromScript<float>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasFloat(int nameID);

		// Token: 0x06000013 RID: 19
		[NativeName("HasValueFromScript<Vector2f>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasVector2(int nameID);

		// Token: 0x06000014 RID: 20
		[NativeName("HasValueFromScript<Vector3f>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasVector3(int nameID);

		// Token: 0x06000015 RID: 21
		[NativeName("HasValueFromScript<Vector4f>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasVector4(int nameID);

		// Token: 0x06000016 RID: 22
		[NativeName("HasValueFromScript<Matrix4x4f>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasMatrix4x4(int nameID);

		// Token: 0x06000017 RID: 23
		[NativeName("SetValueFromScript<bool>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBool(int nameID, bool b);

		// Token: 0x06000018 RID: 24
		[NativeName("SetValueFromScript<int>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetInt(int nameID, int i);

		// Token: 0x06000019 RID: 25
		[NativeName("SetValueFromScript<UInt32>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetUint(int nameID, uint i);

		// Token: 0x0600001A RID: 26
		[NativeName("SetValueFromScript<float>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetFloat(int nameID, float f);

		// Token: 0x0600001B RID: 27 RVA: 0x000021E5 File Offset: 0x000003E5
		[NativeName("SetValueFromScript<Vector2f>")]
		public void SetVector2(int nameID, Vector2 v)
		{
			this.SetVector2_Injected(nameID, ref v);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000021F0 File Offset: 0x000003F0
		[NativeName("SetValueFromScript<Vector3f>")]
		public void SetVector3(int nameID, Vector3 v)
		{
			this.SetVector3_Injected(nameID, ref v);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000021FB File Offset: 0x000003FB
		[NativeName("SetValueFromScript<Vector4f>")]
		public void SetVector4(int nameID, Vector4 v)
		{
			this.SetVector4_Injected(nameID, ref v);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002206 File Offset: 0x00000406
		[NativeName("SetValueFromScript<Matrix4x4f>")]
		public void SetMatrix4x4(int nameID, Matrix4x4 v)
		{
			this.SetMatrix4x4_Injected(nameID, ref v);
		}

		// Token: 0x0600001F RID: 31
		[NativeName("GetValueFromScript<bool>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetBool(int nameID);

		// Token: 0x06000020 RID: 32
		[NativeName("GetValueFromScript<int>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetInt(int nameID);

		// Token: 0x06000021 RID: 33
		[NativeName("GetValueFromScript<UInt32>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern uint GetUint(int nameID);

		// Token: 0x06000022 RID: 34
		[NativeName("GetValueFromScript<float>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetFloat(int nameID);

		// Token: 0x06000023 RID: 35 RVA: 0x00002214 File Offset: 0x00000414
		[NativeName("GetValueFromScript<Vector2f>")]
		public Vector2 GetVector2(int nameID)
		{
			Vector2 vector;
			this.GetVector2_Injected(nameID, out vector);
			return vector;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000222C File Offset: 0x0000042C
		[NativeName("GetValueFromScript<Vector3f>")]
		public Vector3 GetVector3(int nameID)
		{
			Vector3 vector;
			this.GetVector3_Injected(nameID, out vector);
			return vector;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002244 File Offset: 0x00000444
		[NativeName("GetValueFromScript<Vector4f>")]
		public Vector4 GetVector4(int nameID)
		{
			Vector4 vector;
			this.GetVector4_Injected(nameID, out vector);
			return vector;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000225C File Offset: 0x0000045C
		[NativeName("GetValueFromScript<Matrix4x4f>")]
		public Matrix4x4 GetMatrix4x4(int nameID)
		{
			Matrix4x4 matrix4x;
			this.GetMatrix4x4_Injected(nameID, out matrix4x);
			return matrix4x;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002274 File Offset: 0x00000474
		public bool HasBool(string name)
		{
			return this.HasBool(Shader.PropertyToID(name));
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002294 File Offset: 0x00000494
		public bool HasInt(string name)
		{
			return this.HasInt(Shader.PropertyToID(name));
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000022B4 File Offset: 0x000004B4
		public bool HasUint(string name)
		{
			return this.HasUint(Shader.PropertyToID(name));
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000022D4 File Offset: 0x000004D4
		public bool HasFloat(string name)
		{
			return this.HasFloat(Shader.PropertyToID(name));
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000022F4 File Offset: 0x000004F4
		public bool HasVector2(string name)
		{
			return this.HasVector2(Shader.PropertyToID(name));
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002314 File Offset: 0x00000514
		public bool HasVector3(string name)
		{
			return this.HasVector3(Shader.PropertyToID(name));
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002334 File Offset: 0x00000534
		public bool HasVector4(string name)
		{
			return this.HasVector4(Shader.PropertyToID(name));
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002354 File Offset: 0x00000554
		public bool HasMatrix4x4(string name)
		{
			return this.HasMatrix4x4(Shader.PropertyToID(name));
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002372 File Offset: 0x00000572
		public void SetBool(string name, bool b)
		{
			this.SetBool(Shader.PropertyToID(name), b);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002383 File Offset: 0x00000583
		public void SetInt(string name, int i)
		{
			this.SetInt(Shader.PropertyToID(name), i);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002394 File Offset: 0x00000594
		public void SetUint(string name, uint i)
		{
			this.SetUint(Shader.PropertyToID(name), i);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000023A5 File Offset: 0x000005A5
		public void SetFloat(string name, float f)
		{
			this.SetFloat(Shader.PropertyToID(name), f);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000023B6 File Offset: 0x000005B6
		public void SetVector2(string name, Vector2 v)
		{
			this.SetVector2(Shader.PropertyToID(name), v);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000023C7 File Offset: 0x000005C7
		public void SetVector3(string name, Vector3 v)
		{
			this.SetVector3(Shader.PropertyToID(name), v);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000023D8 File Offset: 0x000005D8
		public void SetVector4(string name, Vector4 v)
		{
			this.SetVector4(Shader.PropertyToID(name), v);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000023E9 File Offset: 0x000005E9
		public void SetMatrix4x4(string name, Matrix4x4 v)
		{
			this.SetMatrix4x4(Shader.PropertyToID(name), v);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000023FC File Offset: 0x000005FC
		public bool GetBool(string name)
		{
			return this.GetBool(Shader.PropertyToID(name));
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000241C File Offset: 0x0000061C
		public int GetInt(string name)
		{
			return this.GetInt(Shader.PropertyToID(name));
		}

		// Token: 0x06000039 RID: 57 RVA: 0x0000243C File Offset: 0x0000063C
		public uint GetUint(string name)
		{
			return this.GetUint(Shader.PropertyToID(name));
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000245C File Offset: 0x0000065C
		public float GetFloat(string name)
		{
			return this.GetFloat(Shader.PropertyToID(name));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x0000247C File Offset: 0x0000067C
		public Vector2 GetVector2(string name)
		{
			return this.GetVector2(Shader.PropertyToID(name));
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000249C File Offset: 0x0000069C
		public Vector3 GetVector3(string name)
		{
			return this.GetVector3(Shader.PropertyToID(name));
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000024BC File Offset: 0x000006BC
		public Vector4 GetVector4(string name)
		{
			return this.GetVector4(Shader.PropertyToID(name));
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000024DC File Offset: 0x000006DC
		public Matrix4x4 GetMatrix4x4(string name)
		{
			return this.GetMatrix4x4(Shader.PropertyToID(name));
		}

		// Token: 0x0600003F RID: 63
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CopyValuesFrom([NotNull("ArgumentNullException")] VFXEventAttribute eventAttibute);

		// Token: 0x06000040 RID: 64
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetVector2_Injected(int nameID, ref Vector2 v);

		// Token: 0x06000041 RID: 65
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetVector3_Injected(int nameID, ref Vector3 v);

		// Token: 0x06000042 RID: 66
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetVector4_Injected(int nameID, ref Vector4 v);

		// Token: 0x06000043 RID: 67
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetMatrix4x4_Injected(int nameID, ref Matrix4x4 v);

		// Token: 0x06000044 RID: 68
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetVector2_Injected(int nameID, out Vector2 ret);

		// Token: 0x06000045 RID: 69
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetVector3_Injected(int nameID, out Vector3 ret);

		// Token: 0x06000046 RID: 70
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetVector4_Injected(int nameID, out Vector4 ret);

		// Token: 0x06000047 RID: 71
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetMatrix4x4_Injected(int nameID, out Matrix4x4 ret);

		// Token: 0x04000103 RID: 259
		private IntPtr m_Ptr;

		// Token: 0x04000104 RID: 260
		private bool m_Owner;

		// Token: 0x04000105 RID: 261
		private VisualEffectAsset m_VfxAsset;
	}
}
