using System;
using UnityEngine;

namespace NGS.AdvancedCullingSystem.Static
{
	// Token: 0x02000027 RID: 39
	public class LightStaticCullingSourceStrategy : IStaticCullingSourceStrategy
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00005AFB File Offset: 0x00003CFB
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00005B03 File Offset: 0x00003D03
		public Bounds LocalBounds
		{
			get
			{
				return this._localBounds;
			}
			set
			{
				this._localBounds = value;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005B0C File Offset: 0x00003D0C
		public LightStaticCullingSourceStrategy(GameObject context)
		{
			this._context = context;
			this._light = this._context.GetComponent<Light>();
			LightCullingTarget component = this._context.GetComponent<LightCullingTarget>();
			if (component != null)
			{
				this._localBounds = new Bounds(component.Bounds.center - component.transform.position, component.Bounds.size);
				return;
			}
			if (this._light != null)
			{
				this._localBounds = new Bounds
				{
					center = Vector3.zero,
					size = Vector3.one
				};
				if (this._light.type == LightType.Point)
				{
					this._localBounds.size = this._light.range * Vector3.one;
				}
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005BE7 File Offset: 0x00003DE7
		public bool Validate(out string errorMessage)
		{
			this._light = this._context.GetComponent<Light>();
			if (this._light == null)
			{
				errorMessage = "Light component not found";
				return false;
			}
			errorMessage = "";
			return true;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00005C19 File Offset: 0x00003E19
		public bool TryGetBounds(out Bounds bounds)
		{
			bounds = this._localBounds;
			bounds.center += this._context.transform.position;
			return true;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005C4C File Offset: 0x00003E4C
		public CullingTarget CreateCullingTarget()
		{
			LightCullingTarget lightCullingTarget = this._context.gameObject.AddComponent<LightCullingTarget>();
			Bounds bounds;
			this.TryGetBounds(out bounds);
			lightCullingTarget.Bounds = bounds;
			return lightCullingTarget;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005C79 File Offset: 0x00003E79
		public void PrepareForBaking()
		{
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00005C79 File Offset: 0x00003E79
		public void ClearAfterBaking()
		{
		}

		// Token: 0x040000BC RID: 188
		[SerializeField]
		private GameObject _context;

		// Token: 0x040000BD RID: 189
		[SerializeField]
		private Light _light;

		// Token: 0x040000BE RID: 190
		[SerializeField]
		private Bounds _localBounds;
	}
}
