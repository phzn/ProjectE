using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000059 RID: 89
	[NativeHeader("ParticleSystemScriptingClasses.h")]
	[NativeHeader("Modules/ParticleSystem/ParticleSystemRenderer.h")]
	[NativeHeader("Modules/ParticleSystem/ScriptBindings/ParticleSystemRendererScriptBindings.h")]
	[RequireComponent(typeof(Transform))]
	public sealed class ParticleSystemRenderer : Renderer
	{
		// Token: 0x060006B3 RID: 1715 RVA: 0x00005DCB File Offset: 0x00003FCB
		[Obsolete("EnableVertexStreams is deprecated. Use SetActiveVertexStreams instead.", false)]
		public void EnableVertexStreams(ParticleSystemVertexStreams streams)
		{
			this.Internal_SetVertexStreams(streams, true);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00005DD7 File Offset: 0x00003FD7
		[Obsolete("DisableVertexStreams is deprecated. Use SetActiveVertexStreams instead.", false)]
		public void DisableVertexStreams(ParticleSystemVertexStreams streams)
		{
			this.Internal_SetVertexStreams(streams, false);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00005DE4 File Offset: 0x00003FE4
		[Obsolete("AreVertexStreamsEnabled is deprecated. Use GetActiveVertexStreams instead.", false)]
		public bool AreVertexStreamsEnabled(ParticleSystemVertexStreams streams)
		{
			return this.Internal_GetEnabledVertexStreams(streams) == streams;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00005E00 File Offset: 0x00004000
		[Obsolete("GetEnabledVertexStreams is deprecated. Use GetActiveVertexStreams instead.", false)]
		public ParticleSystemVertexStreams GetEnabledVertexStreams(ParticleSystemVertexStreams streams)
		{
			return this.Internal_GetEnabledVertexStreams(streams);
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x00005E1C File Offset: 0x0000401C
		[Obsolete("Internal_SetVertexStreams is deprecated. Use SetActiveVertexStreams instead.", false)]
		internal void Internal_SetVertexStreams(ParticleSystemVertexStreams streams, bool enabled)
		{
			List<ParticleSystemVertexStream> list = new List<ParticleSystemVertexStream>(this.activeVertexStreamsCount);
			this.GetActiveVertexStreams(list);
			if (enabled)
			{
				bool flag = (streams & ParticleSystemVertexStreams.Position) > ParticleSystemVertexStreams.None;
				if (flag)
				{
					bool flag2 = !list.Contains(ParticleSystemVertexStream.Position);
					if (flag2)
					{
						list.Add(ParticleSystemVertexStream.Position);
					}
				}
				bool flag3 = (streams & ParticleSystemVertexStreams.Normal) > ParticleSystemVertexStreams.None;
				if (flag3)
				{
					bool flag4 = !list.Contains(ParticleSystemVertexStream.Normal);
					if (flag4)
					{
						list.Add(ParticleSystemVertexStream.Normal);
					}
				}
				bool flag5 = (streams & ParticleSystemVertexStreams.Tangent) > ParticleSystemVertexStreams.None;
				if (flag5)
				{
					bool flag6 = !list.Contains(ParticleSystemVertexStream.Tangent);
					if (flag6)
					{
						list.Add(ParticleSystemVertexStream.Tangent);
					}
				}
				bool flag7 = (streams & ParticleSystemVertexStreams.Color) > ParticleSystemVertexStreams.None;
				if (flag7)
				{
					bool flag8 = !list.Contains(ParticleSystemVertexStream.Color);
					if (flag8)
					{
						list.Add(ParticleSystemVertexStream.Color);
					}
				}
				bool flag9 = (streams & ParticleSystemVertexStreams.UV) > ParticleSystemVertexStreams.None;
				if (flag9)
				{
					bool flag10 = !list.Contains(ParticleSystemVertexStream.UV);
					if (flag10)
					{
						list.Add(ParticleSystemVertexStream.UV);
					}
				}
				bool flag11 = (streams & ParticleSystemVertexStreams.UV2BlendAndFrame) > ParticleSystemVertexStreams.None;
				if (flag11)
				{
					bool flag12 = !list.Contains(ParticleSystemVertexStream.UV2);
					if (flag12)
					{
						list.Add(ParticleSystemVertexStream.UV2);
						list.Add(ParticleSystemVertexStream.AnimBlend);
						list.Add(ParticleSystemVertexStream.AnimFrame);
					}
				}
				bool flag13 = (streams & ParticleSystemVertexStreams.CenterAndVertexID) > ParticleSystemVertexStreams.None;
				if (flag13)
				{
					bool flag14 = !list.Contains(ParticleSystemVertexStream.Center);
					if (flag14)
					{
						list.Add(ParticleSystemVertexStream.Center);
						list.Add(ParticleSystemVertexStream.VertexID);
					}
				}
				bool flag15 = (streams & ParticleSystemVertexStreams.Size) > ParticleSystemVertexStreams.None;
				if (flag15)
				{
					bool flag16 = !list.Contains(ParticleSystemVertexStream.SizeXYZ);
					if (flag16)
					{
						list.Add(ParticleSystemVertexStream.SizeXYZ);
					}
				}
				bool flag17 = (streams & ParticleSystemVertexStreams.Rotation) > ParticleSystemVertexStreams.None;
				if (flag17)
				{
					bool flag18 = !list.Contains(ParticleSystemVertexStream.Rotation3D);
					if (flag18)
					{
						list.Add(ParticleSystemVertexStream.Rotation3D);
					}
				}
				bool flag19 = (streams & ParticleSystemVertexStreams.Velocity) > ParticleSystemVertexStreams.None;
				if (flag19)
				{
					bool flag20 = !list.Contains(ParticleSystemVertexStream.Velocity);
					if (flag20)
					{
						list.Add(ParticleSystemVertexStream.Velocity);
					}
				}
				bool flag21 = (streams & ParticleSystemVertexStreams.Lifetime) > ParticleSystemVertexStreams.None;
				if (flag21)
				{
					bool flag22 = !list.Contains(ParticleSystemVertexStream.AgePercent);
					if (flag22)
					{
						list.Add(ParticleSystemVertexStream.AgePercent);
						list.Add(ParticleSystemVertexStream.InvStartLifetime);
					}
				}
				bool flag23 = (streams & ParticleSystemVertexStreams.Custom1) > ParticleSystemVertexStreams.None;
				if (flag23)
				{
					bool flag24 = !list.Contains(ParticleSystemVertexStream.Custom1XYZW);
					if (flag24)
					{
						list.Add(ParticleSystemVertexStream.Custom1XYZW);
					}
				}
				bool flag25 = (streams & ParticleSystemVertexStreams.Custom2) > ParticleSystemVertexStreams.None;
				if (flag25)
				{
					bool flag26 = !list.Contains(ParticleSystemVertexStream.Custom2XYZW);
					if (flag26)
					{
						list.Add(ParticleSystemVertexStream.Custom2XYZW);
					}
				}
				bool flag27 = (streams & ParticleSystemVertexStreams.Random) > ParticleSystemVertexStreams.None;
				if (flag27)
				{
					bool flag28 = !list.Contains(ParticleSystemVertexStream.StableRandomXYZ);
					if (flag28)
					{
						list.Add(ParticleSystemVertexStream.StableRandomXYZ);
						list.Add(ParticleSystemVertexStream.VaryingRandomX);
					}
				}
			}
			else
			{
				bool flag29 = (streams & ParticleSystemVertexStreams.Position) > ParticleSystemVertexStreams.None;
				if (flag29)
				{
					list.Remove(ParticleSystemVertexStream.Position);
				}
				bool flag30 = (streams & ParticleSystemVertexStreams.Normal) > ParticleSystemVertexStreams.None;
				if (flag30)
				{
					list.Remove(ParticleSystemVertexStream.Normal);
				}
				bool flag31 = (streams & ParticleSystemVertexStreams.Tangent) > ParticleSystemVertexStreams.None;
				if (flag31)
				{
					list.Remove(ParticleSystemVertexStream.Tangent);
				}
				bool flag32 = (streams & ParticleSystemVertexStreams.Color) > ParticleSystemVertexStreams.None;
				if (flag32)
				{
					list.Remove(ParticleSystemVertexStream.Color);
				}
				bool flag33 = (streams & ParticleSystemVertexStreams.UV) > ParticleSystemVertexStreams.None;
				if (flag33)
				{
					list.Remove(ParticleSystemVertexStream.UV);
				}
				bool flag34 = (streams & ParticleSystemVertexStreams.UV2BlendAndFrame) > ParticleSystemVertexStreams.None;
				if (flag34)
				{
					list.Remove(ParticleSystemVertexStream.UV2);
					list.Remove(ParticleSystemVertexStream.AnimBlend);
					list.Remove(ParticleSystemVertexStream.AnimFrame);
				}
				bool flag35 = (streams & ParticleSystemVertexStreams.CenterAndVertexID) > ParticleSystemVertexStreams.None;
				if (flag35)
				{
					list.Remove(ParticleSystemVertexStream.Center);
					list.Remove(ParticleSystemVertexStream.VertexID);
				}
				bool flag36 = (streams & ParticleSystemVertexStreams.Size) > ParticleSystemVertexStreams.None;
				if (flag36)
				{
					list.Remove(ParticleSystemVertexStream.SizeXYZ);
				}
				bool flag37 = (streams & ParticleSystemVertexStreams.Rotation) > ParticleSystemVertexStreams.None;
				if (flag37)
				{
					list.Remove(ParticleSystemVertexStream.Rotation3D);
				}
				bool flag38 = (streams & ParticleSystemVertexStreams.Velocity) > ParticleSystemVertexStreams.None;
				if (flag38)
				{
					list.Remove(ParticleSystemVertexStream.Velocity);
				}
				bool flag39 = (streams & ParticleSystemVertexStreams.Lifetime) > ParticleSystemVertexStreams.None;
				if (flag39)
				{
					list.Remove(ParticleSystemVertexStream.AgePercent);
					list.Remove(ParticleSystemVertexStream.InvStartLifetime);
				}
				bool flag40 = (streams & ParticleSystemVertexStreams.Custom1) > ParticleSystemVertexStreams.None;
				if (flag40)
				{
					list.Remove(ParticleSystemVertexStream.Custom1XYZW);
				}
				bool flag41 = (streams & ParticleSystemVertexStreams.Custom2) > ParticleSystemVertexStreams.None;
				if (flag41)
				{
					list.Remove(ParticleSystemVertexStream.Custom2XYZW);
				}
				bool flag42 = (streams & ParticleSystemVertexStreams.Random) > ParticleSystemVertexStreams.None;
				if (flag42)
				{
					list.Remove(ParticleSystemVertexStream.StableRandomXYZW);
					list.Remove(ParticleSystemVertexStream.VaryingRandomX);
				}
			}
			this.SetActiveVertexStreams(list);
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00006268 File Offset: 0x00004468
		[Obsolete("Internal_GetVertexStreams is deprecated. Use GetActiveVertexStreams instead.", false)]
		internal ParticleSystemVertexStreams Internal_GetEnabledVertexStreams(ParticleSystemVertexStreams streams)
		{
			List<ParticleSystemVertexStream> list = new List<ParticleSystemVertexStream>(this.activeVertexStreamsCount);
			this.GetActiveVertexStreams(list);
			ParticleSystemVertexStreams particleSystemVertexStreams = ParticleSystemVertexStreams.None;
			bool flag = list.Contains(ParticleSystemVertexStream.Position);
			if (flag)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Position;
			}
			bool flag2 = list.Contains(ParticleSystemVertexStream.Normal);
			if (flag2)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Normal;
			}
			bool flag3 = list.Contains(ParticleSystemVertexStream.Tangent);
			if (flag3)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Tangent;
			}
			bool flag4 = list.Contains(ParticleSystemVertexStream.Color);
			if (flag4)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Color;
			}
			bool flag5 = list.Contains(ParticleSystemVertexStream.UV);
			if (flag5)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.UV;
			}
			bool flag6 = list.Contains(ParticleSystemVertexStream.UV2);
			if (flag6)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.UV2BlendAndFrame;
			}
			bool flag7 = list.Contains(ParticleSystemVertexStream.Center);
			if (flag7)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.CenterAndVertexID;
			}
			bool flag8 = list.Contains(ParticleSystemVertexStream.SizeXYZ);
			if (flag8)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Size;
			}
			bool flag9 = list.Contains(ParticleSystemVertexStream.Rotation3D);
			if (flag9)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Rotation;
			}
			bool flag10 = list.Contains(ParticleSystemVertexStream.Velocity);
			if (flag10)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Velocity;
			}
			bool flag11 = list.Contains(ParticleSystemVertexStream.AgePercent);
			if (flag11)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Lifetime;
			}
			bool flag12 = list.Contains(ParticleSystemVertexStream.Custom1XYZW);
			if (flag12)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Custom1;
			}
			bool flag13 = list.Contains(ParticleSystemVertexStream.Custom2XYZW);
			if (flag13)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Custom2;
			}
			bool flag14 = list.Contains(ParticleSystemVertexStream.StableRandomXYZ);
			if (flag14)
			{
				particleSystemVertexStreams |= ParticleSystemVertexStreams.Random;
			}
			return particleSystemVertexStreams & streams;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x000063A6 File Offset: 0x000045A6
		[Obsolete("BakeMesh with useTransform is deprecated. Use BakeMesh with ParticleSystemBakeMeshOptions instead.", false)]
		public void BakeMesh(Mesh mesh, bool useTransform = false)
		{
			this.BakeMesh(mesh, Camera.main, useTransform);
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000063B7 File Offset: 0x000045B7
		[Obsolete("BakeMesh with useTransform is deprecated. Use BakeMesh with ParticleSystemBakeMeshOptions instead.", false)]
		public void BakeMesh(Mesh mesh, Camera camera, bool useTransform = false)
		{
			this.BakeMesh(mesh, camera, useTransform ? ParticleSystemBakeMeshOptions.BakeRotationAndScale : ParticleSystemBakeMeshOptions.Default);
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000063CA File Offset: 0x000045CA
		[Obsolete("BakeTrailsMesh with useTransform is deprecated. Use BakeTrailsMesh with ParticleSystemBakeMeshOptions instead.", false)]
		public void BakeTrailsMesh(Mesh mesh, bool useTransform = false)
		{
			this.BakeTrailsMesh(mesh, Camera.main, useTransform);
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x000063DB File Offset: 0x000045DB
		[Obsolete("BakeTrailsMesh with useTransform is deprecated. Use BakeTrailsMesh with ParticleSystemBakeMeshOptions instead.", false)]
		public void BakeTrailsMesh(Mesh mesh, Camera camera, bool useTransform = false)
		{
			this.BakeTrailsMesh(mesh, camera, useTransform ? ParticleSystemBakeMeshOptions.BakeRotationAndScale : ParticleSystemBakeMeshOptions.Default);
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060006BD RID: 1725
		// (set) Token: 0x060006BE RID: 1726
		[NativeName("RenderAlignment")]
		public extern ParticleSystemRenderSpace alignment
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060006BF RID: 1727
		// (set) Token: 0x060006C0 RID: 1728
		public extern ParticleSystemRenderMode renderMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060006C1 RID: 1729
		// (set) Token: 0x060006C2 RID: 1730
		public extern ParticleSystemMeshDistribution meshDistribution
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060006C3 RID: 1731
		// (set) Token: 0x060006C4 RID: 1732
		public extern ParticleSystemSortMode sortMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060006C5 RID: 1733
		// (set) Token: 0x060006C6 RID: 1734
		public extern float lengthScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060006C7 RID: 1735
		// (set) Token: 0x060006C8 RID: 1736
		public extern float velocityScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060006C9 RID: 1737
		// (set) Token: 0x060006CA RID: 1738
		public extern float cameraVelocityScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060006CB RID: 1739
		// (set) Token: 0x060006CC RID: 1740
		public extern float normalDirection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060006CD RID: 1741
		// (set) Token: 0x060006CE RID: 1742
		public extern float shadowBias
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060006CF RID: 1743
		// (set) Token: 0x060006D0 RID: 1744
		public extern float sortingFudge
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060006D1 RID: 1745
		// (set) Token: 0x060006D2 RID: 1746
		public extern float minParticleSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060006D3 RID: 1747
		// (set) Token: 0x060006D4 RID: 1748
		public extern float maxParticleSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x000063F0 File Offset: 0x000045F0
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00006406 File Offset: 0x00004606
		public Vector3 pivot
		{
			get
			{
				Vector3 vector;
				this.get_pivot_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_pivot_Injected(ref value);
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00006410 File Offset: 0x00004610
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x00006426 File Offset: 0x00004626
		public Vector3 flip
		{
			get
			{
				Vector3 vector;
				this.get_flip_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_flip_Injected(ref value);
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060006D9 RID: 1753
		// (set) Token: 0x060006DA RID: 1754
		public extern SpriteMaskInteraction maskInteraction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060006DB RID: 1755
		// (set) Token: 0x060006DC RID: 1756
		public extern Material trailMaterial
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001CF RID: 463
		// (set) Token: 0x060006DD RID: 1757
		internal extern Material oldTrailMaterial
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060006DE RID: 1758
		// (set) Token: 0x060006DF RID: 1759
		public extern bool enableGPUInstancing
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060006E0 RID: 1760
		// (set) Token: 0x060006E1 RID: 1761
		public extern bool allowRoll
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060006E2 RID: 1762
		// (set) Token: 0x060006E3 RID: 1763
		public extern bool freeformStretching
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060006E4 RID: 1764
		// (set) Token: 0x060006E5 RID: 1765
		public extern bool rotateWithStretchDirection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060006E6 RID: 1766
		// (set) Token: 0x060006E7 RID: 1767
		public extern Mesh mesh
		{
			[FreeFunction(Name = "ParticleSystemRendererScriptBindings::GetMesh", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction(Name = "ParticleSystemRendererScriptBindings::SetMesh", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060006E8 RID: 1768
		[RequiredByNativeCode]
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::GetMeshes", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetMeshes([NotNull("ArgumentNullException")] [Out] Mesh[] meshes);

		// Token: 0x060006E9 RID: 1769
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::SetMeshes", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetMeshes([NotNull("ArgumentNullException")] Mesh[] meshes, int size);

		// Token: 0x060006EA RID: 1770 RVA: 0x00006430 File Offset: 0x00004630
		public void SetMeshes(Mesh[] meshes)
		{
			this.SetMeshes(meshes, meshes.Length);
		}

		// Token: 0x060006EB RID: 1771
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::GetMeshWeightings", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetMeshWeightings([NotNull("ArgumentNullException")] [Out] float[] weightings);

		// Token: 0x060006EC RID: 1772
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::SetMeshWeightings", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetMeshWeightings([NotNull("ArgumentNullException")] float[] weightings, int size);

		// Token: 0x060006ED RID: 1773 RVA: 0x0000643E File Offset: 0x0000463E
		public void SetMeshWeightings(float[] weightings)
		{
			this.SetMeshWeightings(weightings, weightings.Length);
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060006EE RID: 1774
		public extern int meshCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0000644C File Offset: 0x0000464C
		public void BakeMesh(Mesh mesh, ParticleSystemBakeMeshOptions options)
		{
			this.BakeMesh(mesh, Camera.main, options);
		}

		// Token: 0x060006F0 RID: 1776
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void BakeMesh([NotNull("ArgumentNullException")] Mesh mesh, [NotNull("ArgumentNullException")] Camera camera, ParticleSystemBakeMeshOptions options);

		// Token: 0x060006F1 RID: 1777 RVA: 0x0000645D File Offset: 0x0000465D
		public void BakeTrailsMesh(Mesh mesh, ParticleSystemBakeMeshOptions options)
		{
			this.BakeTrailsMesh(mesh, Camera.main, options);
		}

		// Token: 0x060006F2 RID: 1778
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void BakeTrailsMesh([NotNull("ArgumentNullException")] Mesh mesh, [NotNull("ArgumentNullException")] Camera camera, ParticleSystemBakeMeshOptions options);

		// Token: 0x060006F3 RID: 1779 RVA: 0x00006470 File Offset: 0x00004670
		public int BakeTexture(ref Texture2D verticesTexture, ParticleSystemBakeTextureOptions options)
		{
			return this.BakeTexture(ref verticesTexture, Camera.main, options);
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00006490 File Offset: 0x00004690
		public int BakeTexture(ref Texture2D verticesTexture, Camera camera, ParticleSystemBakeTextureOptions options)
		{
			bool flag = this.renderMode == ParticleSystemRenderMode.Mesh;
			if (flag)
			{
				throw new InvalidOperationException("Baking mesh particles to texture requires supplying an indices texture");
			}
			int num;
			verticesTexture = this.BakeTextureNoIndicesInternal(verticesTexture, camera, options, out num);
			return num;
		}

		// Token: 0x060006F5 RID: 1781
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::BakeTextureNoIndices", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Texture2D BakeTextureNoIndicesInternal(Texture2D verticesTexture, [NotNull("ArgumentNullException")] Camera camera, ParticleSystemBakeTextureOptions options, out int indexCount);

		// Token: 0x060006F6 RID: 1782 RVA: 0x000064CC File Offset: 0x000046CC
		public int BakeTexture(ref Texture2D verticesTexture, ref Texture2D indicesTexture, ParticleSystemBakeTextureOptions options)
		{
			return this.BakeTexture(ref verticesTexture, ref indicesTexture, Camera.main, options);
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000064EC File Offset: 0x000046EC
		public int BakeTexture(ref Texture2D verticesTexture, ref Texture2D indicesTexture, Camera camera, ParticleSystemBakeTextureOptions options)
		{
			int num;
			ParticleSystemRenderer.BakeTextureOutput bakeTextureOutput = this.BakeTextureInternal(verticesTexture, indicesTexture, camera, options, out num);
			verticesTexture = bakeTextureOutput.vertices;
			indicesTexture = bakeTextureOutput.indices;
			return num;
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00006520 File Offset: 0x00004720
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::BakeTexture", HasExplicitThis = true)]
		private ParticleSystemRenderer.BakeTextureOutput BakeTextureInternal(Texture2D verticesTexture, Texture2D indicesTexture, [NotNull("ArgumentNullException")] Camera camera, ParticleSystemBakeTextureOptions options, out int indexCount)
		{
			ParticleSystemRenderer.BakeTextureOutput bakeTextureOutput;
			this.BakeTextureInternal_Injected(verticesTexture, indicesTexture, camera, options, out indexCount, out bakeTextureOutput);
			return bakeTextureOutput;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x00006540 File Offset: 0x00004740
		public int BakeTrailsTexture(ref Texture2D verticesTexture, ref Texture2D indicesTexture, ParticleSystemBakeTextureOptions options)
		{
			return this.BakeTrailsTexture(ref verticesTexture, ref indicesTexture, Camera.main, options);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00006560 File Offset: 0x00004760
		public int BakeTrailsTexture(ref Texture2D verticesTexture, ref Texture2D indicesTexture, Camera camera, ParticleSystemBakeTextureOptions options)
		{
			int num;
			ParticleSystemRenderer.BakeTextureOutput bakeTextureOutput = this.BakeTrailsTextureInternal(verticesTexture, indicesTexture, camera, options, out num);
			verticesTexture = bakeTextureOutput.vertices;
			indicesTexture = bakeTextureOutput.indices;
			return num;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x00006594 File Offset: 0x00004794
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::BakeTrailsTexture", HasExplicitThis = true)]
		private ParticleSystemRenderer.BakeTextureOutput BakeTrailsTextureInternal(Texture2D verticesTexture, Texture2D indicesTexture, [NotNull("ArgumentNullException")] Camera camera, ParticleSystemBakeTextureOptions options, out int indexCount)
		{
			ParticleSystemRenderer.BakeTextureOutput bakeTextureOutput;
			this.BakeTrailsTextureInternal_Injected(verticesTexture, indicesTexture, camera, options, out indexCount, out bakeTextureOutput);
			return bakeTextureOutput;
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060006FC RID: 1788
		public extern int activeVertexStreamsCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060006FD RID: 1789
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::SetActiveVertexStreams", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetActiveVertexStreams([NotNull("ArgumentNullException")] List<ParticleSystemVertexStream> streams);

		// Token: 0x060006FE RID: 1790
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::GetActiveVertexStreams", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetActiveVertexStreams([NotNull("ArgumentNullException")] List<ParticleSystemVertexStream> streams);

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060006FF RID: 1791
		public extern int activeTrailVertexStreamsCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000700 RID: 1792
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::SetActiveTrailVertexStreams", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetActiveTrailVertexStreams([NotNull("ArgumentNullException")] List<ParticleSystemVertexStream> streams);

		// Token: 0x06000701 RID: 1793
		[FreeFunction(Name = "ParticleSystemRendererScriptBindings::GetActiveTrailVertexStreams", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void GetActiveTrailVertexStreams([NotNull("ArgumentNullException")] List<ParticleSystemVertexStream> streams);

		// Token: 0x06000703 RID: 1795
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_pivot_Injected(out Vector3 ret);

		// Token: 0x06000704 RID: 1796
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_pivot_Injected(ref Vector3 value);

		// Token: 0x06000705 RID: 1797
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_flip_Injected(out Vector3 ret);

		// Token: 0x06000706 RID: 1798
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_flip_Injected(ref Vector3 value);

		// Token: 0x06000707 RID: 1799
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void BakeTextureInternal_Injected(Texture2D verticesTexture, Texture2D indicesTexture, Camera camera, ParticleSystemBakeTextureOptions options, out int indexCount, out ParticleSystemRenderer.BakeTextureOutput ret);

		// Token: 0x06000708 RID: 1800
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void BakeTrailsTextureInternal_Injected(Texture2D verticesTexture, Texture2D indicesTexture, Camera camera, ParticleSystemBakeTextureOptions options, out int indexCount, out ParticleSystemRenderer.BakeTextureOutput ret);

		// Token: 0x0200005A RID: 90
		internal struct BakeTextureOutput
		{
			// Token: 0x04000188 RID: 392
			[NativeName("first")]
			internal Texture2D vertices;

			// Token: 0x04000189 RID: 393
			[NativeName("second")]
			internal Texture2D indices;
		}
	}
}
