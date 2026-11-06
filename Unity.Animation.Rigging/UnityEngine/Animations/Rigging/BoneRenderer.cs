using System;

namespace UnityEngine.Animations.Rigging
{
	// Token: 0x02000075 RID: 117
	[ExecuteInEditMode]
	[AddComponentMenu("Animation Rigging/Setup/Bone Renderer")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.animation.rigging@1.3/manual/RiggingWorkflow.html#bone-renderer-component")]
	public class BoneRenderer : MonoBehaviour
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000A063 File Offset: 0x00008263
		public Transform[] transforms
		{
			get
			{
				return this.m_Transforms;
			}
		}

		// Token: 0x04000138 RID: 312
		public BoneRenderer.BoneShape boneShape = BoneRenderer.BoneShape.Pyramid;

		// Token: 0x04000139 RID: 313
		public bool drawBones = true;

		// Token: 0x0400013A RID: 314
		public bool drawTripods;

		// Token: 0x0400013B RID: 315
		[Range(0.01f, 5f)]
		public float boneSize = 1f;

		// Token: 0x0400013C RID: 316
		[Range(0.01f, 5f)]
		public float tripodSize = 1f;

		// Token: 0x0400013D RID: 317
		public Color boneColor = new Color(0f, 0f, 1f, 0.5f);

		// Token: 0x0400013E RID: 318
		[SerializeField]
		private Transform[] m_Transforms;

		// Token: 0x0200008A RID: 138
		public enum BoneShape
		{
			// Token: 0x0400017F RID: 383
			Line,
			// Token: 0x04000180 RID: 384
			Pyramid,
			// Token: 0x04000181 RID: 385
			Box
		}
	}
}
