using System;
using UnityEngine;

namespace ProjectExhibitionist.PoiTravel
{
	// Token: 0x0200095B RID: 2395
	[DefaultExecutionOrder(10000)]
	[DisallowMultipleComponent]
	public sealed class PoiDisplayCameraBudget : MonoBehaviour
	{
		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x0600414C RID: 16716 RVA: 0x0018380C File Offset: 0x00181A0C
		public bool IsFeedPaused
		{
			get
			{
				return this.pausedByUs;
			}
		}

		// Token: 0x0600414D RID: 16717 RVA: 0x00183814 File Offset: 0x00181A14
		private void OnEnable()
		{
			this.authoredEnabled = this.feedCamera != null && this.feedCamera.enabled;
			this.lastVisibleTime = Time.unscaledTime;
			this.pausedByUs = false;
		}

		// Token: 0x0600414E RID: 16718 RVA: 0x0018384A File Offset: 0x00181A4A
		private void LateUpdate()
		{
			if (this.viewer == null || !this.viewer.isActiveAndEnabled)
			{
				this.viewer = Camera.main;
			}
			this.Evaluate(this.viewer, Time.unscaledTime);
		}

		// Token: 0x0600414F RID: 16719 RVA: 0x00183884 File Offset: 0x00181A84
		public void Evaluate(Camera source, float now)
		{
			if (this.feedCamera == null || this.feedCamera.targetTexture == null || !this.authoredEnabled)
			{
				return;
			}
			bool flag = source == null || this.displays.Length == 0;
			if (source != null)
			{
				GeometryUtility.CalculateFrustumPlanes(source, this.planes);
				foreach (Renderer renderer in this.displays)
				{
					if (renderer == null)
					{
						flag = true;
						break;
					}
					if (renderer.enabled && !renderer.forceRenderingOff && renderer.gameObject.activeInHierarchy && (source.cullingMask & (1 << renderer.gameObject.layer)) != 0)
					{
						Bounds bounds = renderer.bounds;
						bounds.Expand(2f);
						if (GeometryUtility.TestPlanesAABB(this.planes, bounds))
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (flag)
			{
				this.lastVisibleTime = now;
			}
			bool flag2 = !flag && now - this.lastVisibleTime >= this.hideDelay;
			if (flag2 == this.pausedByUs)
			{
				return;
			}
			this.pausedByUs = flag2;
			this.feedCamera.enabled = !flag2;
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x001839B8 File Offset: 0x00181BB8
		private void OnDisable()
		{
			if (this.pausedByUs && this.feedCamera != null)
			{
				this.feedCamera.enabled = this.authoredEnabled;
			}
			this.pausedByUs = false;
		}

		// Token: 0x04004134 RID: 16692
		[SerializeField]
		private Camera feedCamera;

		// Token: 0x04004135 RID: 16693
		[SerializeField]
		private Renderer[] displays = Array.Empty<Renderer>();

		// Token: 0x04004136 RID: 16694
		[SerializeField]
		[Min(0f)]
		private float hideDelay = 0.5f;

		// Token: 0x04004137 RID: 16695
		private readonly Plane[] planes = new Plane[6];

		// Token: 0x04004138 RID: 16696
		private Camera viewer;

		// Token: 0x04004139 RID: 16697
		private bool authoredEnabled;

		// Token: 0x0400413A RID: 16698
		private bool pausedByUs;

		// Token: 0x0400413B RID: 16699
		private float lastVisibleTime;
	}
}
