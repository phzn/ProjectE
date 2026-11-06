using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.VFX
{
	// Token: 0x02000011 RID: 17
	[NativeType(Header = "Modules/VFX/Public/VFXExpressionValues.h")]
	[RequiredByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public class VFXExpressionValues
	{
		// Token: 0x06000048 RID: 72 RVA: 0x000024FA File Offset: 0x000006FA
		private VFXExpressionValues()
		{
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002504 File Offset: 0x00000704
		[RequiredByNativeCode]
		internal static VFXExpressionValues CreateExpressionValuesWrapper(IntPtr ptr)
		{
			return new VFXExpressionValues
			{
				m_Ptr = ptr
			};
		}

		// Token: 0x0600004A RID: 74
		[NativeThrows]
		[NativeName("GetValueFromScript<bool>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetBool(int nameID);

		// Token: 0x0600004B RID: 75
		[NativeName("GetValueFromScript<int>")]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetInt(int nameID);

		// Token: 0x0600004C RID: 76
		[NativeThrows]
		[NativeName("GetValueFromScript<UInt32>")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern uint GetUInt(int nameID);

		// Token: 0x0600004D RID: 77
		[NativeName("GetValueFromScript<float>")]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetFloat(int nameID);

		// Token: 0x0600004E RID: 78 RVA: 0x00002524 File Offset: 0x00000724
		[NativeName("GetValueFromScript<Vector2f>")]
		[NativeThrows]
		public Vector2 GetVector2(int nameID)
		{
			Vector2 vector;
			this.GetVector2_Injected(nameID, out vector);
			return vector;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000253C File Offset: 0x0000073C
		[NativeName("GetValueFromScript<Vector3f>")]
		[NativeThrows]
		public Vector3 GetVector3(int nameID)
		{
			Vector3 vector;
			this.GetVector3_Injected(nameID, out vector);
			return vector;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002554 File Offset: 0x00000754
		[NativeName("GetValueFromScript<Vector4f>")]
		[NativeThrows]
		public Vector4 GetVector4(int nameID)
		{
			Vector4 vector;
			this.GetVector4_Injected(nameID, out vector);
			return vector;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000256C File Offset: 0x0000076C
		[NativeName("GetValueFromScript<Matrix4x4f>")]
		[NativeThrows]
		public Matrix4x4 GetMatrix4x4(int nameID)
		{
			Matrix4x4 matrix4x;
			this.GetMatrix4x4_Injected(nameID, out matrix4x);
			return matrix4x;
		}

		// Token: 0x06000052 RID: 82
		[NativeName("GetValueFromScript<Texture*>")]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Texture GetTexture(int nameID);

		// Token: 0x06000053 RID: 83
		[NativeName("GetValueFromScript<Mesh*>")]
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Mesh GetMesh(int nameID);

		// Token: 0x06000054 RID: 84 RVA: 0x00002584 File Offset: 0x00000784
		public AnimationCurve GetAnimationCurve(int nameID)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			this.Internal_GetAnimationCurveFromScript(nameID, animationCurve);
			return animationCurve;
		}

		// Token: 0x06000055 RID: 85
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_GetAnimationCurveFromScript(int nameID, AnimationCurve curve);

		// Token: 0x06000056 RID: 86 RVA: 0x000025A8 File Offset: 0x000007A8
		public Gradient GetGradient(int nameID)
		{
			Gradient gradient = new Gradient();
			this.Internal_GetGradientFromScript(nameID, gradient);
			return gradient;
		}

		// Token: 0x06000057 RID: 87
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_GetGradientFromScript(int nameID, Gradient gradient);

		// Token: 0x06000058 RID: 88 RVA: 0x000025CC File Offset: 0x000007CC
		public bool GetBool(string name)
		{
			return this.GetBool(Shader.PropertyToID(name));
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000025EC File Offset: 0x000007EC
		public int GetInt(string name)
		{
			return this.GetInt(Shader.PropertyToID(name));
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000260C File Offset: 0x0000080C
		public uint GetUInt(string name)
		{
			return this.GetUInt(Shader.PropertyToID(name));
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000262C File Offset: 0x0000082C
		public float GetFloat(string name)
		{
			return this.GetFloat(Shader.PropertyToID(name));
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000264C File Offset: 0x0000084C
		public Vector2 GetVector2(string name)
		{
			return this.GetVector2(Shader.PropertyToID(name));
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000266C File Offset: 0x0000086C
		public Vector3 GetVector3(string name)
		{
			return this.GetVector3(Shader.PropertyToID(name));
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000268C File Offset: 0x0000088C
		public Vector4 GetVector4(string name)
		{
			return this.GetVector4(Shader.PropertyToID(name));
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000026AC File Offset: 0x000008AC
		public Matrix4x4 GetMatrix4x4(string name)
		{
			return this.GetMatrix4x4(Shader.PropertyToID(name));
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000026CC File Offset: 0x000008CC
		public Texture GetTexture(string name)
		{
			return this.GetTexture(Shader.PropertyToID(name));
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000026EC File Offset: 0x000008EC
		public AnimationCurve GetAnimationCurve(string name)
		{
			return this.GetAnimationCurve(Shader.PropertyToID(name));
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000270C File Offset: 0x0000090C
		public Gradient GetGradient(string name)
		{
			return this.GetGradient(Shader.PropertyToID(name));
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000272C File Offset: 0x0000092C
		public Mesh GetMesh(string name)
		{
			return this.GetMesh(Shader.PropertyToID(name));
		}

		// Token: 0x06000064 RID: 100
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetVector2_Injected(int nameID, out Vector2 ret);

		// Token: 0x06000065 RID: 101
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetVector3_Injected(int nameID, out Vector3 ret);

		// Token: 0x06000066 RID: 102
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetVector4_Injected(int nameID, out Vector4 ret);

		// Token: 0x06000067 RID: 103
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetMatrix4x4_Injected(int nameID, out Matrix4x4 ret);

		// Token: 0x04000106 RID: 262
		internal IntPtr m_Ptr;
	}
}
