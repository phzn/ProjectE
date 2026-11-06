using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000741 RID: 1857
	public class MudFootprintDecalPoolService : MonoBehaviour
	{
		// Token: 0x06003009 RID: 12297 RVA: 0x0010FE90 File Offset: 0x0010E090
		private void Awake()
		{
			if (this.decalMaterial == null)
			{
				Debug.LogError("[MudFootprintDecalPoolService] Decal material is missing. Assign a URP Decal material.", this);
				base.enabled = false;
				return;
			}
			for (int i = 0; i < this.prewarmCount; i++)
			{
				this.available.Enqueue(this.CreateDecalRuntime(i));
			}
		}

		// Token: 0x0600300A RID: 12298 RVA: 0x0010FEE4 File Offset: 0x0010E0E4
		private void Update()
		{
			float time = Time.time;
			for (int i = this.active.Count - 1; i >= 0; i--)
			{
				MudFootprintDecalPoolService.DecalRuntime decalRuntime = this.active[i];
				float num = time - decalRuntime.SpawnTime;
				if (num >= decalRuntime.Lifetime)
				{
					this.Release(i);
				}
				else if (decalRuntime.FadeDuration > 0f)
				{
					float num2 = decalRuntime.Lifetime - decalRuntime.FadeDuration;
					if (num >= num2)
					{
						float num3 = Mathf.Clamp01((num - num2) / decalRuntime.FadeDuration);
						float num4 = Mathf.Lerp(decalRuntime.StartAlpha, 0f, num3);
						decalRuntime.Projector.fadeFactor = num4;
					}
				}
			}
		}

		// Token: 0x0600300B RID: 12299 RVA: 0x0010FF94 File Offset: 0x0010E194
		public void Spawn(Vector3 worldPosition, Vector3 worldNormal, Vector3 forwardHint, float size, float lifetime, float fadeDuration, float alpha, bool isLeft)
		{
			if (!base.enabled)
			{
				return;
			}
			if (size <= 0f || lifetime <= 0f || alpha <= 0f)
			{
				return;
			}
			if (this.active.Count >= this.maxActiveDecals)
			{
				this.Release(0);
			}
			MudFootprintDecalPoolService.DecalRuntime decalRuntime = ((this.available.Count > 0) ? this.available.Dequeue() : this.CreateDecalRuntime(this.active.Count + this.available.Count));
			Transform transform = decalRuntime.Transform;
			Vector3 vector = ((worldNormal.sqrMagnitude > 0.0001f) ? worldNormal.normalized : Vector3.up);
			Vector3 vector2 = Vector3.ProjectOnPlane(forwardHint, vector);
			if (vector2.sqrMagnitude < 0.0001f)
			{
				vector2 = Vector3.ProjectOnPlane(Vector3.forward, vector);
			}
			vector2.Normalize();
			transform.position = worldPosition + vector * 0.05f;
			transform.rotation = Quaternion.LookRotation(-vector, vector2);
			transform.localScale = new Vector3(isLeft ? (-1f) : 1f, 1f, 1f);
			float num = size * this.footprintAspectRatio;
			decalRuntime.Projector.size = new Vector3(num, size, this.projectionDepth);
			decalRuntime.Projector.pivot = Vector3.zero;
			decalRuntime.SpawnTime = Time.time;
			decalRuntime.Lifetime = lifetime;
			decalRuntime.FadeDuration = Mathf.Clamp(fadeDuration, 0f, lifetime);
			decalRuntime.StartAlpha = Mathf.Clamp01(alpha);
			decalRuntime.Projector.fadeFactor = decalRuntime.StartAlpha;
			transform.gameObject.SetActive(true);
			this.active.Add(decalRuntime);
		}

		// Token: 0x0600300C RID: 12300 RVA: 0x00110148 File Offset: 0x0010E348
		public void ClearAll()
		{
			for (int i = this.active.Count - 1; i >= 0; i--)
			{
				this.Release(i);
			}
		}

		// Token: 0x0600300D RID: 12301 RVA: 0x00110174 File Offset: 0x0010E374
		private void Release(int activeIndex)
		{
			MudFootprintDecalPoolService.DecalRuntime decalRuntime = this.active[activeIndex];
			this.active.RemoveAt(activeIndex);
			decalRuntime.Transform.gameObject.SetActive(false);
			decalRuntime.Projector.fadeFactor = 0f;
			this.available.Enqueue(decalRuntime);
		}

		// Token: 0x0600300E RID: 12302 RVA: 0x001101C8 File Offset: 0x0010E3C8
		private MudFootprintDecalPoolService.DecalRuntime CreateDecalRuntime(int index)
		{
			GameObject gameObject = new GameObject(string.Format("MudFootprintDecal_{0}", index));
			gameObject.transform.SetParent(base.transform, false);
			gameObject.SetActive(false);
			DecalProjector decalProjector = gameObject.AddComponent<DecalProjector>();
			decalProjector.material = this.decalMaterial;
			decalProjector.size = new Vector3(0.25f, 0.25f, this.projectionDepth);
			decalProjector.pivot = Vector3.zero;
			decalProjector.fadeFactor = 0f;
			decalProjector.scaleMode = DecalScaleMode.InheritFromHierarchy;
			return new MudFootprintDecalPoolService.DecalRuntime
			{
				Transform = gameObject.transform,
				Projector = decalProjector
			};
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x00110267 File Offset: 0x0010E467
		private void OnValidate()
		{
			this.prewarmCount = Mathf.Max(0, this.prewarmCount);
			this.maxActiveDecals = Mathf.Max(1, this.maxActiveDecals);
			this.projectionDepth = Mathf.Max(0.01f, this.projectionDepth);
		}

		// Token: 0x04003093 RID: 12435
		[Header("Decal Visual")]
		[Tooltip("URP Decal material (Shader Graphs/Decal). Must use Decal shader.")]
		[SerializeField]
		private Material decalMaterial;

		// Token: 0x04003094 RID: 12436
		[Tooltip("Projection depth along surface normal (how deep the decal projects into geometry).")]
		[SerializeField]
		private float projectionDepth = 0.5f;

		// Token: 0x04003095 RID: 12437
		[Tooltip("Aspect ratio of the footprint (width/height).")]
		[SerializeField]
		private float footprintAspectRatio = 0.5f;

		// Token: 0x04003096 RID: 12438
		[Header("Pool")]
		[SerializeField]
		private int prewarmCount = 48;

		// Token: 0x04003097 RID: 12439
		[SerializeField]
		private int maxActiveDecals = 200;

		// Token: 0x04003098 RID: 12440
		private readonly Queue<MudFootprintDecalPoolService.DecalRuntime> available = new Queue<MudFootprintDecalPoolService.DecalRuntime>();

		// Token: 0x04003099 RID: 12441
		private readonly List<MudFootprintDecalPoolService.DecalRuntime> active = new List<MudFootprintDecalPoolService.DecalRuntime>(256);

		// Token: 0x02000742 RID: 1858
		private sealed class DecalRuntime
		{
			// Token: 0x0400309A RID: 12442
			public Transform Transform;

			// Token: 0x0400309B RID: 12443
			public DecalProjector Projector;

			// Token: 0x0400309C RID: 12444
			public float SpawnTime;

			// Token: 0x0400309D RID: 12445
			public float Lifetime;

			// Token: 0x0400309E RID: 12446
			public float FadeDuration;

			// Token: 0x0400309F RID: 12447
			public float StartAlpha;
		}
	}
}
