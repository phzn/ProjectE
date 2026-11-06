using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine.VFX
{
	// Token: 0x0200001D RID: 29
	[NativeHeader("Modules/VFX/Public/ScriptBindings/VisualEffectBindings.h")]
	[RequireComponent(typeof(Transform))]
	[NativeHeader("Modules/VFX/Public/VisualEffect.h")]
	public class VisualEffect : Behaviour
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000BB RID: 187
		// (set) Token: 0x060000BC RID: 188
		public extern bool pause
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000BD RID: 189
		// (set) Token: 0x060000BE RID: 190
		public extern float playRate
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000BF RID: 191
		// (set) Token: 0x060000C0 RID: 192
		public extern uint startSeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000C1 RID: 193
		// (set) Token: 0x060000C2 RID: 194
		public extern bool resetSeedOnPlay
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000C3 RID: 195
		// (set) Token: 0x060000C4 RID: 196
		public extern int initialEventID
		{
			[FreeFunction(Name = "VisualEffectBindings::GetInitialEventID", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction(Name = "VisualEffectBindings::SetInitialEventID", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000C5 RID: 197
		// (set) Token: 0x060000C6 RID: 198
		public extern string initialEventName
		{
			[FreeFunction(Name = "VisualEffectBindings::GetInitialEventName", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction(Name = "VisualEffectBindings::SetInitialEventName", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000C7 RID: 199
		public extern bool culled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000C8 RID: 200
		// (set) Token: 0x060000C9 RID: 201
		public extern VisualEffectAsset visualEffectAsset
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00002AAC File Offset: 0x00000CAC
		public VFXEventAttribute CreateVFXEventAttribute()
		{
			bool flag = this.visualEffectAsset == null;
			VFXEventAttribute vfxeventAttribute;
			if (flag)
			{
				vfxeventAttribute = null;
			}
			else
			{
				VFXEventAttribute vfxeventAttribute2 = VFXEventAttribute.Internal_InstanciateVFXEventAttribute(this.visualEffectAsset);
				vfxeventAttribute = vfxeventAttribute2;
			}
			return vfxeventAttribute;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00002AE0 File Offset: 0x00000CE0
		private void CheckValidVFXEventAttribute(VFXEventAttribute eventAttribute)
		{
			bool flag = eventAttribute != null && eventAttribute.vfxAsset != this.visualEffectAsset;
			if (flag)
			{
				throw new InvalidOperationException("Invalid VFXEventAttribute provided to VisualEffect. It has been created with another VisualEffectAsset. Use CreateVFXEventAttribute.");
			}
		}

		// Token: 0x060000CC RID: 204
		[FreeFunction(Name = "VisualEffectBindings::SendEventFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SendEventFromScript(int eventNameID, VFXEventAttribute eventAttribute);

		// Token: 0x060000CD RID: 205 RVA: 0x00002B15 File Offset: 0x00000D15
		public void SendEvent(int eventNameID, VFXEventAttribute eventAttribute)
		{
			this.CheckValidVFXEventAttribute(eventAttribute);
			this.SendEventFromScript(eventNameID, eventAttribute);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00002B29 File Offset: 0x00000D29
		public void SendEvent(string eventName, VFXEventAttribute eventAttribute)
		{
			this.SendEvent(Shader.PropertyToID(eventName), eventAttribute);
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00002B3A File Offset: 0x00000D3A
		public void SendEvent(int eventNameID)
		{
			this.SendEventFromScript(eventNameID, null);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00002B46 File Offset: 0x00000D46
		public void SendEvent(string eventName)
		{
			this.SendEvent(Shader.PropertyToID(eventName), null);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00002B57 File Offset: 0x00000D57
		public void Play(VFXEventAttribute eventAttribute)
		{
			this.SendEvent(VisualEffectAsset.PlayEventID, eventAttribute);
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00002B67 File Offset: 0x00000D67
		public void Play()
		{
			this.SendEvent(VisualEffectAsset.PlayEventID);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00002B76 File Offset: 0x00000D76
		public void Stop(VFXEventAttribute eventAttribute)
		{
			this.SendEvent(VisualEffectAsset.StopEventID, eventAttribute);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00002B86 File Offset: 0x00000D86
		public void Stop()
		{
			this.SendEvent(VisualEffectAsset.StopEventID);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00002B95 File Offset: 0x00000D95
		public void Reinit()
		{
			this.Reinit(true);
		}

		// Token: 0x060000D6 RID: 214
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Reinit(bool sendInitialEventAndPrewarm = true);

		// Token: 0x060000D7 RID: 215
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AdvanceOneFrame();

		// Token: 0x060000D8 RID: 216
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RecreateData();

		// Token: 0x060000D9 RID: 217
		[FreeFunction(Name = "VisualEffectBindings::ResetOverrideFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetOverride(int nameID);

		// Token: 0x060000DA RID: 218
		[FreeFunction(Name = "VisualEffectBindings::GetTextureDimensionFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern TextureDimension GetTextureDimension(int nameID);

		// Token: 0x060000DB RID: 219
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<bool>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasBool(int nameID);

		// Token: 0x060000DC RID: 220
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<int>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasInt(int nameID);

		// Token: 0x060000DD RID: 221
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<UInt32>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasUInt(int nameID);

		// Token: 0x060000DE RID: 222
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<float>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasFloat(int nameID);

		// Token: 0x060000DF RID: 223
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Vector2f>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasVector2(int nameID);

		// Token: 0x060000E0 RID: 224
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Vector3f>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasVector3(int nameID);

		// Token: 0x060000E1 RID: 225
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Vector4f>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasVector4(int nameID);

		// Token: 0x060000E2 RID: 226
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Matrix4x4f>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasMatrix4x4(int nameID);

		// Token: 0x060000E3 RID: 227
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Texture*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasTexture(int nameID);

		// Token: 0x060000E4 RID: 228
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<AnimationCurve*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasAnimationCurve(int nameID);

		// Token: 0x060000E5 RID: 229
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Gradient*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasGradient(int nameID);

		// Token: 0x060000E6 RID: 230
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<Mesh*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasMesh(int nameID);

		// Token: 0x060000E7 RID: 231
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<SkinnedMeshRenderer*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasSkinnedMeshRenderer(int nameID);

		// Token: 0x060000E8 RID: 232
		[FreeFunction(Name = "VisualEffectBindings::HasValueFromScript<GraphicsBuffer*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasGraphicsBuffer(int nameID);

		// Token: 0x060000E9 RID: 233
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<bool>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBool(int nameID, bool b);

		// Token: 0x060000EA RID: 234
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<int>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetInt(int nameID, int i);

		// Token: 0x060000EB RID: 235
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<UInt32>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetUInt(int nameID, uint i);

		// Token: 0x060000EC RID: 236
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<float>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetFloat(int nameID, float f);

		// Token: 0x060000ED RID: 237 RVA: 0x00002BA0 File Offset: 0x00000DA0
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Vector2f>", HasExplicitThis = true)]
		public void SetVector2(int nameID, Vector2 v)
		{
			this.SetVector2_Injected(nameID, ref v);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00002BAB File Offset: 0x00000DAB
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Vector3f>", HasExplicitThis = true)]
		public void SetVector3(int nameID, Vector3 v)
		{
			this.SetVector3_Injected(nameID, ref v);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00002BB6 File Offset: 0x00000DB6
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Vector4f>", HasExplicitThis = true)]
		public void SetVector4(int nameID, Vector4 v)
		{
			this.SetVector4_Injected(nameID, ref v);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00002BC1 File Offset: 0x00000DC1
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Matrix4x4f>", HasExplicitThis = true)]
		public void SetMatrix4x4(int nameID, Matrix4x4 v)
		{
			this.SetMatrix4x4_Injected(nameID, ref v);
		}

		// Token: 0x060000F1 RID: 241
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Texture*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTexture(int nameID, [NotNull("ArgumentNullException")] Texture t);

		// Token: 0x060000F2 RID: 242
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<AnimationCurve*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAnimationCurve(int nameID, [NotNull("ArgumentNullException")] AnimationCurve c);

		// Token: 0x060000F3 RID: 243
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Gradient*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGradient(int nameID, [NotNull("ArgumentNullException")] Gradient g);

		// Token: 0x060000F4 RID: 244
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<Mesh*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetMesh(int nameID, [NotNull("ArgumentNullException")] Mesh m);

		// Token: 0x060000F5 RID: 245
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<SkinnedMeshRenderer*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetSkinnedMeshRenderer(int nameID, SkinnedMeshRenderer m);

		// Token: 0x060000F6 RID: 246
		[FreeFunction(Name = "VisualEffectBindings::SetValueFromScript<GraphicsBuffer*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetGraphicsBuffer(int nameID, GraphicsBuffer g);

		// Token: 0x060000F7 RID: 247
		[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<bool>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetBool(int nameID);

		// Token: 0x060000F8 RID: 248
		[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<int>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetInt(int nameID);

		// Token: 0x060000F9 RID: 249
		[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<UInt32>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern uint GetUInt(int nameID);

		// Token: 0x060000FA RID: 250
		[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<float>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetFloat(int nameID);

		// Token: 0x060000FB RID: 251 RVA: 0x00002BCC File Offset: 0x00000DCC
		[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<Vector2f>", HasExplicitThis = true)]
		public Vector2 GetVector2(int nameID)
		{
			Vector2 vector;
			this.GetVector2_Injected(nameID, out vector);
			return vector;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00002BE4 File Offset: 0x00000DE4
		[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<Vector3f>", HasExplicitThis = true)]
		public Vector3 GetVector3(int nameID)
		{
			Vector3 vector;
			this.GetVector3_Injected(nameID, out vector);
			return vector;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00002BFC File Offset: 0x00000DFC
		[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<Vector4f>", HasExplicitThis = true)]
		public Vector4 GetVector4(int nameID)
		{
			Vector4 vector;
			this.GetVector4_Injected(nameID, out vector);
			return vector;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00002C14 File Offset: 0x00000E14
		[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<Matrix4x4f>", HasExplicitThis = true)]
		public Matrix4x4 GetMatrix4x4(int nameID)
		{
			Matrix4x4 matrix4x;
			this.GetMatrix4x4_Injected(nameID, out matrix4x);
			return matrix4x;
		}

		// Token: 0x060000FF RID: 255
		[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<Texture*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Texture GetTexture(int nameID);

		// Token: 0x06000100 RID: 256
		[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<Mesh*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Mesh GetMesh(int nameID);

		// Token: 0x06000101 RID: 257
		[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<SkinnedMeshRenderer*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern SkinnedMeshRenderer GetSkinnedMeshRenderer(int nameID);

		// Token: 0x06000102 RID: 258
		[FreeFunction(Name = "VisualEffectBindings::GetValueFromScript<GraphicsBuffer*>", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern GraphicsBuffer GetGraphicsBuffer(int nameID);

		// Token: 0x06000103 RID: 259 RVA: 0x00002C2C File Offset: 0x00000E2C
		public Gradient GetGradient(int nameID)
		{
			Gradient gradient = new Gradient();
			this.Internal_GetGradient(nameID, gradient);
			return gradient;
		}

		// Token: 0x06000104 RID: 260
		[FreeFunction(Name = "VisualEffectBindings::Internal_GetGradientFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_GetGradient(int nameID, Gradient gradient);

		// Token: 0x06000105 RID: 261 RVA: 0x00002C50 File Offset: 0x00000E50
		public AnimationCurve GetAnimationCurve(int nameID)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			this.Internal_GetAnimationCurve(nameID, animationCurve);
			return animationCurve;
		}

		// Token: 0x06000106 RID: 262
		[FreeFunction(Name = "VisualEffectBindings::Internal_GetAnimationCurveFromScript", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_GetAnimationCurve(int nameID, AnimationCurve curve);

		// Token: 0x06000107 RID: 263 RVA: 0x00002C74 File Offset: 0x00000E74
		[FreeFunction(Name = "VisualEffectBindings::GetParticleSystemInfo", HasExplicitThis = true, ThrowsException = true)]
		public VFXParticleSystemInfo GetParticleSystemInfo(int nameID)
		{
			VFXParticleSystemInfo vfxparticleSystemInfo;
			this.GetParticleSystemInfo_Injected(nameID, out vfxparticleSystemInfo);
			return vfxparticleSystemInfo;
		}

		// Token: 0x06000108 RID: 264
		[FreeFunction(Name = "VisualEffectBindings::GetSpawnSystemInfo", HasExplicitThis = true, ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetSpawnSystemInfo(int nameID, IntPtr spawnerState);

		// Token: 0x06000109 RID: 265
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasAnySystemAwake();

		// Token: 0x0600010A RID: 266 RVA: 0x00002C8C File Offset: 0x00000E8C
		[FreeFunction(Name = "VisualEffectBindings::GetComputedBounds", HasExplicitThis = true)]
		internal Bounds GetComputedBounds(int nameID)
		{
			Bounds bounds;
			this.GetComputedBounds_Injected(nameID, out bounds);
			return bounds;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00002CA4 File Offset: 0x00000EA4
		[FreeFunction(Name = "VisualEffectBindings::GetCurrentBoundsPadding", HasExplicitThis = true)]
		internal Vector3 GetCurrentBoundsPadding(int nameID)
		{
			Vector3 vector;
			this.GetCurrentBoundsPadding_Injected(nameID, out vector);
			return vector;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00002CBC File Offset: 0x00000EBC
		public void GetSpawnSystemInfo(int nameID, VFXSpawnerState spawnState)
		{
			bool flag = spawnState == null;
			if (flag)
			{
				throw new NullReferenceException("GetSpawnSystemInfo expects a non null VFXSpawnerState.");
			}
			IntPtr ptr = spawnState.GetPtr();
			bool flag2 = ptr == IntPtr.Zero;
			if (flag2)
			{
				throw new NullReferenceException("GetSpawnSystemInfo use an unexpected not owned VFXSpawnerState.");
			}
			this.GetSpawnSystemInfo(nameID, ptr);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00002D08 File Offset: 0x00000F08
		public VFXSpawnerState GetSpawnSystemInfo(int nameID)
		{
			VFXSpawnerState vfxspawnerState = new VFXSpawnerState();
			this.GetSpawnSystemInfo(nameID, vfxspawnerState);
			return vfxspawnerState;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00002D2C File Offset: 0x00000F2C
		public bool HasSystem(int nameID)
		{
			VisualEffectAsset visualEffectAsset = this.visualEffectAsset;
			return visualEffectAsset != null && visualEffectAsset.HasSystem(nameID);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00002D58 File Offset: 0x00000F58
		public void GetSystemNames(List<string> names)
		{
			bool flag = names == null;
			if (flag)
			{
				throw new ArgumentNullException("names");
			}
			VisualEffectAsset visualEffectAsset = this.visualEffectAsset;
			bool flag2 = visualEffectAsset;
			if (flag2)
			{
				visualEffectAsset.GetSystemNames(names);
			}
			else
			{
				names.Clear();
			}
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00002D9C File Offset: 0x00000F9C
		public void GetParticleSystemNames(List<string> names)
		{
			bool flag = names == null;
			if (flag)
			{
				throw new ArgumentNullException("names");
			}
			VisualEffectAsset visualEffectAsset = this.visualEffectAsset;
			bool flag2 = visualEffectAsset;
			if (flag2)
			{
				visualEffectAsset.GetParticleSystemNames(names);
			}
			else
			{
				names.Clear();
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00002DE0 File Offset: 0x00000FE0
		public void GetOutputEventNames(List<string> names)
		{
			bool flag = names == null;
			if (flag)
			{
				throw new ArgumentNullException("names");
			}
			VisualEffectAsset visualEffectAsset = this.visualEffectAsset;
			bool flag2 = visualEffectAsset;
			if (flag2)
			{
				visualEffectAsset.GetOutputEventNames(names);
			}
			else
			{
				names.Clear();
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00002E24 File Offset: 0x00001024
		public void GetSpawnSystemNames(List<string> names)
		{
			bool flag = names == null;
			if (flag)
			{
				throw new ArgumentNullException("names");
			}
			VisualEffectAsset visualEffectAsset = this.visualEffectAsset;
			bool flag2 = visualEffectAsset;
			if (flag2)
			{
				visualEffectAsset.GetSpawnSystemNames(names);
			}
			else
			{
				names.Clear();
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00002E67 File Offset: 0x00001067
		public void ResetOverride(string name)
		{
			this.ResetOverride(Shader.PropertyToID(name));
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00002E78 File Offset: 0x00001078
		public bool HasInt(string name)
		{
			return this.HasInt(Shader.PropertyToID(name));
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00002E98 File Offset: 0x00001098
		public bool HasUInt(string name)
		{
			return this.HasUInt(Shader.PropertyToID(name));
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00002EB8 File Offset: 0x000010B8
		public bool HasFloat(string name)
		{
			return this.HasFloat(Shader.PropertyToID(name));
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00002ED8 File Offset: 0x000010D8
		public bool HasVector2(string name)
		{
			return this.HasVector2(Shader.PropertyToID(name));
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00002EF8 File Offset: 0x000010F8
		public bool HasVector3(string name)
		{
			return this.HasVector3(Shader.PropertyToID(name));
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00002F18 File Offset: 0x00001118
		public bool HasVector4(string name)
		{
			return this.HasVector4(Shader.PropertyToID(name));
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00002F38 File Offset: 0x00001138
		public bool HasMatrix4x4(string name)
		{
			return this.HasMatrix4x4(Shader.PropertyToID(name));
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00002F58 File Offset: 0x00001158
		public bool HasTexture(string name)
		{
			return this.HasTexture(Shader.PropertyToID(name));
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00002F78 File Offset: 0x00001178
		public TextureDimension GetTextureDimension(string name)
		{
			return this.GetTextureDimension(Shader.PropertyToID(name));
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00002F98 File Offset: 0x00001198
		public bool HasAnimationCurve(string name)
		{
			return this.HasAnimationCurve(Shader.PropertyToID(name));
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00002FB8 File Offset: 0x000011B8
		public bool HasGradient(string name)
		{
			return this.HasGradient(Shader.PropertyToID(name));
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00002FD8 File Offset: 0x000011D8
		public bool HasMesh(string name)
		{
			return this.HasMesh(Shader.PropertyToID(name));
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00002FF8 File Offset: 0x000011F8
		public bool HasSkinnedMeshRenderer(string name)
		{
			return this.HasSkinnedMeshRenderer(Shader.PropertyToID(name));
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00003018 File Offset: 0x00001218
		public bool HasGraphicsBuffer(string name)
		{
			return this.HasGraphicsBuffer(Shader.PropertyToID(name));
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00003038 File Offset: 0x00001238
		public bool HasBool(string name)
		{
			return this.HasBool(Shader.PropertyToID(name));
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00003056 File Offset: 0x00001256
		public void SetInt(string name, int i)
		{
			this.SetInt(Shader.PropertyToID(name), i);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00003067 File Offset: 0x00001267
		public void SetUInt(string name, uint i)
		{
			this.SetUInt(Shader.PropertyToID(name), i);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00003078 File Offset: 0x00001278
		public void SetFloat(string name, float f)
		{
			this.SetFloat(Shader.PropertyToID(name), f);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00003089 File Offset: 0x00001289
		public void SetVector2(string name, Vector2 v)
		{
			this.SetVector2(Shader.PropertyToID(name), v);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000309A File Offset: 0x0000129A
		public void SetVector3(string name, Vector3 v)
		{
			this.SetVector3(Shader.PropertyToID(name), v);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x000030AB File Offset: 0x000012AB
		public void SetVector4(string name, Vector4 v)
		{
			this.SetVector4(Shader.PropertyToID(name), v);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000030BC File Offset: 0x000012BC
		public void SetMatrix4x4(string name, Matrix4x4 v)
		{
			this.SetMatrix4x4(Shader.PropertyToID(name), v);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000030CD File Offset: 0x000012CD
		public void SetTexture(string name, Texture t)
		{
			this.SetTexture(Shader.PropertyToID(name), t);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x000030DE File Offset: 0x000012DE
		public void SetAnimationCurve(string name, AnimationCurve c)
		{
			this.SetAnimationCurve(Shader.PropertyToID(name), c);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000030EF File Offset: 0x000012EF
		public void SetGradient(string name, Gradient g)
		{
			this.SetGradient(Shader.PropertyToID(name), g);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00003100 File Offset: 0x00001300
		public void SetMesh(string name, Mesh m)
		{
			this.SetMesh(Shader.PropertyToID(name), m);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00003111 File Offset: 0x00001311
		public void SetSkinnedMeshRenderer(string name, SkinnedMeshRenderer m)
		{
			this.SetSkinnedMeshRenderer(Shader.PropertyToID(name), m);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00003122 File Offset: 0x00001322
		public void SetGraphicsBuffer(string name, GraphicsBuffer g)
		{
			this.SetGraphicsBuffer(Shader.PropertyToID(name), g);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00003133 File Offset: 0x00001333
		public void SetBool(string name, bool b)
		{
			this.SetBool(Shader.PropertyToID(name), b);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00003144 File Offset: 0x00001344
		public int GetInt(string name)
		{
			return this.GetInt(Shader.PropertyToID(name));
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00003164 File Offset: 0x00001364
		public uint GetUInt(string name)
		{
			return this.GetUInt(Shader.PropertyToID(name));
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00003184 File Offset: 0x00001384
		public float GetFloat(string name)
		{
			return this.GetFloat(Shader.PropertyToID(name));
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000031A4 File Offset: 0x000013A4
		public Vector2 GetVector2(string name)
		{
			return this.GetVector2(Shader.PropertyToID(name));
		}

		// Token: 0x06000135 RID: 309 RVA: 0x000031C4 File Offset: 0x000013C4
		public Vector3 GetVector3(string name)
		{
			return this.GetVector3(Shader.PropertyToID(name));
		}

		// Token: 0x06000136 RID: 310 RVA: 0x000031E4 File Offset: 0x000013E4
		public Vector4 GetVector4(string name)
		{
			return this.GetVector4(Shader.PropertyToID(name));
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00003204 File Offset: 0x00001404
		public Matrix4x4 GetMatrix4x4(string name)
		{
			return this.GetMatrix4x4(Shader.PropertyToID(name));
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00003224 File Offset: 0x00001424
		public Texture GetTexture(string name)
		{
			return this.GetTexture(Shader.PropertyToID(name));
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00003244 File Offset: 0x00001444
		public Mesh GetMesh(string name)
		{
			return this.GetMesh(Shader.PropertyToID(name));
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00003264 File Offset: 0x00001464
		public SkinnedMeshRenderer GetSkinnedMeshRenderer(string name)
		{
			return this.GetSkinnedMeshRenderer(Shader.PropertyToID(name));
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00003284 File Offset: 0x00001484
		internal GraphicsBuffer GetGraphicsBuffer(string name)
		{
			return this.GetGraphicsBuffer(Shader.PropertyToID(name));
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000032A4 File Offset: 0x000014A4
		public bool GetBool(string name)
		{
			return this.GetBool(Shader.PropertyToID(name));
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000032C4 File Offset: 0x000014C4
		public AnimationCurve GetAnimationCurve(string name)
		{
			return this.GetAnimationCurve(Shader.PropertyToID(name));
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000032E4 File Offset: 0x000014E4
		public Gradient GetGradient(string name)
		{
			return this.GetGradient(Shader.PropertyToID(name));
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00003304 File Offset: 0x00001504
		public bool HasSystem(string name)
		{
			return this.HasSystem(Shader.PropertyToID(name));
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00003324 File Offset: 0x00001524
		public VFXParticleSystemInfo GetParticleSystemInfo(string name)
		{
			return this.GetParticleSystemInfo(Shader.PropertyToID(name));
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00003344 File Offset: 0x00001544
		public VFXSpawnerState GetSpawnSystemInfo(string name)
		{
			return this.GetSpawnSystemInfo(Shader.PropertyToID(name));
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00003364 File Offset: 0x00001564
		internal Bounds GetComputedBounds(string name)
		{
			return this.GetComputedBounds(Shader.PropertyToID(name));
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00003384 File Offset: 0x00001584
		internal Vector3 GetCurrentBoundsPadding(string name)
		{
			return this.GetCurrentBoundsPadding(Shader.PropertyToID(name));
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000144 RID: 324
		public extern int aliveParticleCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000145 RID: 325
		internal extern float time
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000146 RID: 326
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Simulate(float stepDeltaTime, uint stepCount = 1U);

		// Token: 0x06000147 RID: 327 RVA: 0x000033A4 File Offset: 0x000015A4
		[RequiredByNativeCode]
		private static VFXEventAttribute InvokeGetCachedEventAttributeForOutputEvent_Internal(VisualEffect source)
		{
			bool flag = source.outputEventReceived == null;
			VFXEventAttribute vfxeventAttribute;
			if (flag)
			{
				vfxeventAttribute = null;
			}
			else
			{
				bool flag2 = source.m_cachedEventAttribute == null;
				if (flag2)
				{
					source.m_cachedEventAttribute = source.CreateVFXEventAttribute();
				}
				vfxeventAttribute = source.m_cachedEventAttribute;
			}
			return vfxeventAttribute;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000033E8 File Offset: 0x000015E8
		[RequiredByNativeCode]
		private static void InvokeOutputEventReceived_Internal(VisualEffect source, int eventNameId)
		{
			VFXOutputEventArgs e = new VFXOutputEventArgs(eventNameId, source.m_cachedEventAttribute);
			source.outputEventReceived(e);
		}

		// Token: 0x0600014A RID: 330
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetVector2_Injected(int nameID, ref Vector2 v);

		// Token: 0x0600014B RID: 331
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetVector3_Injected(int nameID, ref Vector3 v);

		// Token: 0x0600014C RID: 332
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetVector4_Injected(int nameID, ref Vector4 v);

		// Token: 0x0600014D RID: 333
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetMatrix4x4_Injected(int nameID, ref Matrix4x4 v);

		// Token: 0x0600014E RID: 334
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetVector2_Injected(int nameID, out Vector2 ret);

		// Token: 0x0600014F RID: 335
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetVector3_Injected(int nameID, out Vector3 ret);

		// Token: 0x06000150 RID: 336
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetVector4_Injected(int nameID, out Vector4 ret);

		// Token: 0x06000151 RID: 337
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetMatrix4x4_Injected(int nameID, out Matrix4x4 ret);

		// Token: 0x06000152 RID: 338
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetParticleSystemInfo_Injected(int nameID, out VFXParticleSystemInfo ret);

		// Token: 0x06000153 RID: 339
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetComputedBounds_Injected(int nameID, out Bounds ret);

		// Token: 0x06000154 RID: 340
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetCurrentBoundsPadding_Injected(int nameID, out Vector3 ret);

		// Token: 0x04000126 RID: 294
		private VFXEventAttribute m_cachedEventAttribute;

		// Token: 0x04000127 RID: 295
		public Action<VFXOutputEventArgs> outputEventReceived;
	}
}
