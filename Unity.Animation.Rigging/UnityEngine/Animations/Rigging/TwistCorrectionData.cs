using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000054 RID: 84
	[Serializable]
	public struct TwistCorrectionData : IAnimationJobData, ITwistCorrectionData
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00007632 File Offset: 0x00005832
		// (set) Token: 0x06000208 RID: 520 RVA: 0x0000763A File Offset: 0x0000583A
		public Transform sourceObject
		{
			get
			{
				return this.m_Source;
			}
			set
			{
				this.m_Source = value;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000209 RID: 521 RVA: 0x00007643 File Offset: 0x00005843
		// (set) Token: 0x0600020A RID: 522 RVA: 0x0000764B File Offset: 0x0000584B
		public WeightedTransformArray twistNodes
		{
			get
			{
				return this.m_TwistNodes;
			}
			set
			{
				this.m_TwistNodes = value;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600020B RID: 523 RVA: 0x00007654 File Offset: 0x00005854
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0000765C File Offset: 0x0000585C
		public TwistCorrectionData.Axis twistAxis
		{
			get
			{
				return this.m_TwistAxis;
			}
			set
			{
				this.m_TwistAxis = value;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600020D RID: 525 RVA: 0x00007665 File Offset: 0x00005865
		Transform ITwistCorrectionData.source
		{
			get
			{
				return this.m_Source;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000766D File Offset: 0x0000586D
		Vector3 ITwistCorrectionData.twistAxis
		{
			get
			{
				return TwistCorrectionData.Convert(this.m_TwistAxis);
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000767A File Offset: 0x0000587A
		string ITwistCorrectionData.twistNodesProperty
		{
			get
			{
				return ConstraintsUtils.ConstructConstraintDataPropertyName("m_TwistNodes");
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00007686 File Offset: 0x00005886
		private static Vector3 Convert(TwistCorrectionData.Axis axis)
		{
			if (axis == TwistCorrectionData.Axis.X)
			{
				return Vector3.right;
			}
			if (axis == TwistCorrectionData.Axis.Y)
			{
				return Vector3.up;
			}
			return Vector3.forward;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000076A0 File Offset: 0x000058A0
		bool IAnimationJobData.IsValid()
		{
			if (this.m_Source == null)
			{
				return false;
			}
			for (int i = 0; i < this.m_TwistNodes.Count; i++)
			{
				if (this.m_TwistNodes[i].transform == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000076EF File Offset: 0x000058EF
		void IAnimationJobData.SetDefaultValues()
		{
			this.m_Source = null;
			this.m_TwistAxis = TwistCorrectionData.Axis.Z;
			this.m_TwistNodes.Clear();
		}

		// Token: 0x040000E9 RID: 233
		[SyncSceneToStream]
		[SerializeField]
		private Transform m_Source;

		// Token: 0x040000EA RID: 234
		[NotKeyable]
		[SerializeField]
		private TwistCorrectionData.Axis m_TwistAxis;

		// Token: 0x040000EB RID: 235
		[SyncSceneToStream]
		[SerializeField]
		[WeightRange(-1f, 1f)]
		private WeightedTransformArray m_TwistNodes;

		// Token: 0x02000082 RID: 130
		public enum Axis
		{
			// Token: 0x0400016C RID: 364
			X,
			// Token: 0x0400016D RID: 365
			Y,
			// Token: 0x0400016E RID: 366
			Z
		}
	}
}
