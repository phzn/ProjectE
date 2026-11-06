using System;
using DA_Assets.Singleton;
using UnityEngine;

namespace DA_Assets.DAB
{
	// Token: 0x02000007 RID: 7
	[CreateAssetMenu(menuName = "D.A. Assets/DabConfig")]
	[ResourcePath(new string[] { "" })]
	public class DabConfig : AssetConfig<DabConfig>
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002956 File Offset: 0x00000B56
		public AnimatedProperty<Vector2> DefaultScaleProps
		{
			get
			{
				return this._defaultScaleProps;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000026 RID: 38 RVA: 0x0000295E File Offset: 0x00000B5E
		public AnimatedProperty<Vector2> DefaultPositionProps
		{
			get
			{
				return this._defaultPositionProps;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002966 File Offset: 0x00000B66
		public AnimatedProperty<float> DefaulRotationProps
		{
			get
			{
				return this._defaulRotationProps;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000028 RID: 40 RVA: 0x0000296E File Offset: 0x00000B6E
		public AnimatedProperty<Color> DefaultColorProps
		{
			get
			{
				return this._defaultColorProps;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002976 File Offset: 0x00000B76
		public AnimatedProperty<AnimatedGradient> DefaultGradientProps
		{
			get
			{
				return this._defaultGradientProps;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002A RID: 42 RVA: 0x0000297E File Offset: 0x00000B7E
		public EventAnimations DefaultEventAnimations
		{
			get
			{
				return this._defaultEventAnimations;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002986 File Offset: 0x00000B86
		public Shader SpriteTransitionShader
		{
			get
			{
				return this._spriteTransitionShader;
			}
		}

		// Token: 0x04000013 RID: 19
		public const string DabConfigStr = "DabConfig";

		// Token: 0x04000014 RID: 20
		[SerializeField]
		private AnimatedProperty<Vector2> _defaultScaleProps;

		// Token: 0x04000015 RID: 21
		[SerializeField]
		private AnimatedProperty<Vector2> _defaultPositionProps;

		// Token: 0x04000016 RID: 22
		[SerializeField]
		private AnimatedProperty<float> _defaulRotationProps;

		// Token: 0x04000017 RID: 23
		[SerializeField]
		private AnimatedProperty<Color> _defaultColorProps;

		// Token: 0x04000018 RID: 24
		[SerializeField]
		private AnimatedProperty<AnimatedGradient> _defaultGradientProps;

		// Token: 0x04000019 RID: 25
		[SerializeField]
		private EventAnimations _defaultEventAnimations;

		// Token: 0x0400001A RID: 26
		[Space]
		[SerializeField]
		private Shader _spriteTransitionShader;
	}
}
