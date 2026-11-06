using System;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using UnityEngine;

namespace Features.NipplePiercings
{
	// Token: 0x02000F66 RID: 3942
	[DisallowMultipleComponent]
	public sealed class NipplePiercingErection : MonoBehaviour
	{
		// Token: 0x060076D3 RID: 30419 RVA: 0x003052E0 File Offset: 0x003034E0
		public void ApplyToBody(SkinnedMeshRenderer body)
		{
			if (!base.enabled || (Application.isPlaying && !base.gameObject.activeInHierarchy) || body == null || body.sharedMesh == null)
			{
				this.Release();
				return;
			}
			BlendShapeManager blendShapeManager = BlendShapeManager.ResolveForRig(body);
			Object @object = ((blendShapeManager != null) ? blendShapeManager : body);
			if (this._lease == null || this._lease.Released || this._key != @object)
			{
				this.Release();
				if (!NipplePiercingErection.Leases.TryGetValue(@object, out this._lease))
				{
					this._lease = new NipplePiercingErection.Lease(blendShapeManager, body);
					NipplePiercingErection.Leases.Add(@object, this._lease);
				}
				this._key = @object;
				this._lease.Owners++;
			}
			this._lease.Apply(body);
		}

		// Token: 0x060076D4 RID: 30420 RVA: 0x003053BC File Offset: 0x003035BC
		public void Release()
		{
			NipplePiercingErection.Lease lease = this._lease;
			Object key = this._key;
			this._lease = null;
			this._key = null;
			if (lease == null || lease.Released)
			{
				return;
			}
			NipplePiercingErection.Lease lease2 = lease;
			int num = lease2.Owners - 1;
			lease2.Owners = num;
			if (num > 0)
			{
				return;
			}
			lease.Restore();
			NipplePiercingErection.Leases.Remove(key);
		}

		// Token: 0x060076D5 RID: 30421 RVA: 0x00305418 File Offset: 0x00303618
		private void OnDisable()
		{
			this.Release();
		}

		// Token: 0x060076D6 RID: 30422 RVA: 0x00305418 File Offset: 0x00303618
		private void OnDestroy()
		{
			this.Release();
		}

		// Token: 0x060076D7 RID: 30423 RVA: 0x00305418 File Offset: 0x00303618
		private void OnTransformParentChanged()
		{
			this.Release();
		}

		// Token: 0x060076D8 RID: 30424 RVA: 0x00305420 File Offset: 0x00303620
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetLeases()
		{
			foreach (NipplePiercingErection.Lease lease in NipplePiercingErection.Leases.Values)
			{
				lease.Restore();
			}
			NipplePiercingErection.Leases.Clear();
		}

		// Token: 0x060076D9 RID: 30425 RVA: 0x00305480 File Offset: 0x00303680
		private static int FindShape(Mesh mesh, string name)
		{
			if (mesh == null)
			{
				return -1;
			}
			int blendShapeIndex = mesh.GetBlendShapeIndex(name);
			if (blendShapeIndex >= 0)
			{
				return blendShapeIndex;
			}
			for (int i = 0; i < mesh.blendShapeCount; i++)
			{
				string blendShapeName = mesh.GetBlendShapeName(i);
				int num = blendShapeName.LastIndexOf(':');
				if (num >= 0 && blendShapeName.Substring(num + 1) == name)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060076DA RID: 30426 RVA: 0x003054E0 File Offset: 0x003036E0
		private static void SetWeight(SkinnedMeshRenderer body, string name, float value)
		{
			if (body == null)
			{
				return;
			}
			int num = NipplePiercingErection.FindShape(body.sharedMesh, name);
			if (num >= 0)
			{
				body.SetBlendShapeWeight(num, value);
			}
		}

		// Token: 0x04007A17 RID: 31255
		public const string NippleShape = "Nipple Depth";

		// Token: 0x04007A18 RID: 31256
		public const string AreolaShape = "Areola Depth";

		// Token: 0x04007A19 RID: 31257
		public const float NippleWeight = 30f;

		// Token: 0x04007A1A RID: 31258
		public const float AreolaWeight = 35f;

		// Token: 0x04007A1B RID: 31259
		private const string Source = "NipplePiercingErection";

		// Token: 0x04007A1C RID: 31260
		private const string BaselineSource = "NipplePiercingErection.Baseline";

		// Token: 0x04007A1D RID: 31261
		private static readonly Dictionary<Object, NipplePiercingErection.Lease> Leases = new Dictionary<Object, NipplePiercingErection.Lease>();

		// Token: 0x04007A1E RID: 31262
		private Object _key;

		// Token: 0x04007A1F RID: 31263
		private NipplePiercingErection.Lease _lease;

		// Token: 0x02000F67 RID: 3943
		private sealed class Lease
		{
			// Token: 0x1700137A RID: 4986
			// (get) Token: 0x060076DD RID: 30429 RVA: 0x0030551C File Offset: 0x0030371C
			// (set) Token: 0x060076DE RID: 30430 RVA: 0x00305524 File Offset: 0x00303724
			public bool Released { get; private set; }

			// Token: 0x060076DF RID: 30431 RVA: 0x00305530 File Offset: 0x00303730
			public Lease(BlendShapeManager manager, SkinnedMeshRenderer body)
			{
				this._manager = manager;
				this._fallbackBody = body;
				if (manager != null)
				{
					manager.Initialize();
				}
				this._shapes = new NipplePiercingErection.Shape[]
				{
					this.Capture(body, "Nipple Depth", 30f),
					this.Capture(body, "Areola Depth", 35f)
				};
			}

			// Token: 0x060076E0 RID: 30432 RVA: 0x003055A0 File Offset: 0x003037A0
			private NipplePiercingErection.Shape Capture(SkinnedMeshRenderer body, string name, float weight)
			{
				int num = NipplePiercingErection.FindShape(body.sharedMesh, name);
				NipplePiercingErection.Shape shape = new NipplePiercingErection.Shape
				{
					Name = name,
					Weight = weight,
					Exists = (num >= 0)
				};
				if (!shape.Exists)
				{
					return shape;
				}
				shape.Baseline = body.GetBlendShapeWeight(num);
				if (this._manager != null)
				{
					float num2;
					if (this._manager.TryGetAuthoredBlendShapeValue(name, out num2))
					{
						shape.Baseline = num2;
					}
					else
					{
						float num3;
						if (this._manager.TryGetEffectiveOverride(name, out num3, null))
						{
							shape.Baseline = 0f;
						}
						shape.OwnsBaseline = true;
					}
				}
				return shape;
			}

			// Token: 0x060076E1 RID: 30433 RVA: 0x00305640 File Offset: 0x00303840
			public void Apply(SkinnedMeshRenderer body)
			{
				Mesh mesh;
				if (this._manager != null && (!this._boundMeshes.TryGetValue(body, out mesh) || mesh != body.sharedMesh))
				{
					this._boundMeshes[body] = body.sharedMesh;
					this._manager.Rebuild();
				}
				foreach (NipplePiercingErection.Shape shape in this._shapes)
				{
					if (shape.Exists)
					{
						if (this._manager != null)
						{
							if (shape.OwnsBaseline)
							{
								this._manager.SetOverride(shape.Name, shape.Baseline, "NipplePiercingErection.Baseline", BlendShapePriority.Default, true);
							}
							this._manager.SetOverride(shape.Name, shape.Weight, "NipplePiercingErection", BlendShapePriority.Piercing, true);
						}
						NipplePiercingErection.SetWeight(body, shape.Name, shape.Weight);
					}
				}
			}

			// Token: 0x060076E2 RID: 30434 RVA: 0x0030571C File Offset: 0x0030391C
			public void Restore()
			{
				if (this.Released)
				{
					return;
				}
				this.Released = true;
				foreach (NipplePiercingErection.Shape shape in this._shapes)
				{
					if (shape.Exists)
					{
						if (this._manager != null)
						{
							this._manager.RemoveOverride(shape.Name, "NipplePiercingErection", true, true, shape.Baseline);
							if (shape.OwnsBaseline)
							{
								this._manager.RemoveOverride(shape.Name, "NipplePiercingErection.Baseline", true, true, shape.Baseline);
							}
						}
						else
						{
							NipplePiercingErection.SetWeight(this._fallbackBody, shape.Name, shape.Baseline);
						}
					}
				}
			}

			// Token: 0x04007A20 RID: 31264
			private readonly BlendShapeManager _manager;

			// Token: 0x04007A21 RID: 31265
			private readonly SkinnedMeshRenderer _fallbackBody;

			// Token: 0x04007A22 RID: 31266
			private readonly Dictionary<SkinnedMeshRenderer, Mesh> _boundMeshes = new Dictionary<SkinnedMeshRenderer, Mesh>();

			// Token: 0x04007A23 RID: 31267
			private readonly NipplePiercingErection.Shape[] _shapes;

			// Token: 0x04007A24 RID: 31268
			public int Owners;
		}

		// Token: 0x02000F68 RID: 3944
		private sealed class Shape
		{
			// Token: 0x04007A26 RID: 31270
			public string Name;

			// Token: 0x04007A27 RID: 31271
			public float Weight;

			// Token: 0x04007A28 RID: 31272
			public float Baseline;

			// Token: 0x04007A29 RID: 31273
			public bool Exists;

			// Token: 0x04007A2A RID: 31274
			public bool OwnsBaseline;
		}
	}
}
