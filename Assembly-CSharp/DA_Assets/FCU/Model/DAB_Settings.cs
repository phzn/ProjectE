using System;
using DA_Assets.DAB;
using DA_Assets.DAI;
using DA_Assets.Singleton;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
	// Token: 0x02000371 RID: 881
	[Serializable]
	public class DAB_Settings : MonoBehaviourLinkerRuntime<FigmaConverterUnity>
	{
		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060016E2 RID: 5858 RVA: 0x00096740 File Offset: 0x00094940
		// (set) Token: 0x060016E3 RID: 5859 RVA: 0x00096748 File Offset: 0x00094948
		[SerializeProperty("_scaleProperties")]
		public AnimatedProperty<Vector2> ScaleProperties
		{
			get
			{
				return this._scaleProperties;
			}
			set
			{
				this._scaleProperties = value;
			}
		}

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060016E4 RID: 5860 RVA: 0x00096751 File Offset: 0x00094951
		// (set) Token: 0x060016E5 RID: 5861 RVA: 0x00096759 File Offset: 0x00094959
		[SerializeProperty("_scaleAnimations")]
		public EventAnimations ScaleAnimations
		{
			get
			{
				return this._scaleAnimations;
			}
			set
			{
				this._scaleAnimations = value;
			}
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x060016E6 RID: 5862 RVA: 0x00096751 File Offset: 0x00094951
		// (set) Token: 0x060016E7 RID: 5863 RVA: 0x00096759 File Offset: 0x00094959
		[SerializeProperty("_colorAnimations")]
		public EventAnimations ColorAnimations
		{
			get
			{
				return this._scaleAnimations;
			}
			set
			{
				this._scaleAnimations = value;
			}
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060016E8 RID: 5864 RVA: 0x00096762 File Offset: 0x00094962
		// (set) Token: 0x060016E9 RID: 5865 RVA: 0x0009676A File Offset: 0x0009496A
		[SerializeProperty("_spriteAnimations")]
		public EventAnimations SpriteAnimations
		{
			get
			{
				return this._spriteAnimations;
			}
			set
			{
				this._spriteAnimations = value;
			}
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x00096774 File Offset: 0x00094974
		public override void Reset()
		{
			EventAnimations defaultEventAnimations = SingletonScriptableObject<DabConfig>.Instance.DefaultEventAnimations;
			this._scaleProperties = SingletonScriptableObject<DabConfig>.Instance.DefaultScaleProps;
			this._scaleAnimations = defaultEventAnimations;
			this._colorAnimations = defaultEventAnimations;
			this._spriteAnimations = defaultEventAnimations;
		}

		// Token: 0x040018B9 RID: 6329
		[SerializeField]
		private AnimatedProperty<Vector2> _scaleProperties;

		// Token: 0x040018BA RID: 6330
		[SerializeField]
		private EventAnimations _scaleAnimations;

		// Token: 0x040018BB RID: 6331
		[SerializeField]
		private EventAnimations _colorAnimations;

		// Token: 0x040018BC RID: 6332
		[SerializeField]
		private EventAnimations _spriteAnimations;
	}
}
