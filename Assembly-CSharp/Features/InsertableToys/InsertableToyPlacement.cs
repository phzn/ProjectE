using System;
using UnityEngine;

namespace Features.InsertableToys
{
	// Token: 0x02001176 RID: 4470
	[Serializable]
	public sealed class InsertableToyPlacement
	{
		// Token: 0x170015A4 RID: 5540
		// (get) Token: 0x0600887A RID: 34938 RVA: 0x0037E31E File Offset: 0x0037C51E
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}

		// Token: 0x170015A5 RID: 5541
		// (get) Token: 0x0600887B RID: 34939 RVA: 0x0037E326 File Offset: 0x0037C526
		public InsertionSite Site
		{
			get
			{
				return this.site;
			}
		}

		// Token: 0x170015A6 RID: 5542
		// (get) Token: 0x0600887C RID: 34940 RVA: 0x0037E32E File Offset: 0x0037C52E
		public string ActionName
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.actionName))
				{
					return this.actionName;
				}
				return "Insert";
			}
		}

		// Token: 0x170015A7 RID: 5543
		// (get) Token: 0x0600887D RID: 34941 RVA: 0x0037E349 File Offset: 0x0037C549
		public KeyCode InputKey
		{
			get
			{
				return this.inputKey;
			}
		}

		// Token: 0x170015A8 RID: 5544
		// (get) Token: 0x0600887E RID: 34942 RVA: 0x0037E351 File Offset: 0x0037C551
		public string KeyDisplayLabel
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(this.keyDisplayLabel))
				{
					return this.keyDisplayLabel;
				}
				return this.inputKey.ToString();
			}
		}

		// Token: 0x170015A9 RID: 5545
		// (get) Token: 0x0600887F RID: 34943 RVA: 0x0037E378 File Offset: 0x0037C578
		public string SocketTransformName
		{
			get
			{
				return this.socketTransformName;
			}
		}

		// Token: 0x170015AA RID: 5546
		// (get) Token: 0x06008880 RID: 34944 RVA: 0x0037E380 File Offset: 0x0037C580
		public HumanBodyBones SocketBone
		{
			get
			{
				return this.socketBone;
			}
		}

		// Token: 0x170015AB RID: 5547
		// (get) Token: 0x06008881 RID: 34945 RVA: 0x0037E388 File Offset: 0x0037C588
		public Vector3 LocalPosition
		{
			get
			{
				return this.localPosition;
			}
		}

		// Token: 0x170015AC RID: 5548
		// (get) Token: 0x06008882 RID: 34946 RVA: 0x0037E390 File Offset: 0x0037C590
		public Vector3 LocalEulerAngles
		{
			get
			{
				return this.localEulerAngles;
			}
		}

		// Token: 0x170015AD RID: 5549
		// (get) Token: 0x06008883 RID: 34947 RVA: 0x0037E398 File Offset: 0x0037C598
		public Vector3 LocalScale
		{
			get
			{
				if (!(this.localScale == Vector3.zero))
				{
					return this.localScale;
				}
				return Vector3.one;
			}
		}

		// Token: 0x170015AE RID: 5550
		// (get) Token: 0x06008884 RID: 34948 RVA: 0x0037E3B8 File Offset: 0x0037C5B8
		public string BlendShapeName
		{
			get
			{
				return this.blendShapeName;
			}
		}

		// Token: 0x170015AF RID: 5551
		// (get) Token: 0x06008885 RID: 34949 RVA: 0x0037E3C0 File Offset: 0x0037C5C0
		public float BlendShapeWeight
		{
			get
			{
				return this.blendShapeWeight;
			}
		}

		// Token: 0x06008886 RID: 34950 RVA: 0x0037E3C8 File Offset: 0x0037C5C8
		public InsertableToyPlacement(InsertionSite insertionSite, string label, KeyCode key, string shapeName, Vector3 position, Vector3 eulerAngles, Vector3 scale)
		{
			this.site = insertionSite;
			this.actionName = label;
			this.inputKey = key;
			this.keyDisplayLabel = key.ToString();
			this.blendShapeName = shapeName;
			this.localPosition = position;
			this.localEulerAngles = eulerAngles;
			this.localScale = scale;
		}

		// Token: 0x04008B30 RID: 35632
		[SerializeField]
		private bool enabled = true;

		// Token: 0x04008B31 RID: 35633
		[SerializeField]
		private InsertionSite site;

		// Token: 0x04008B32 RID: 35634
		[Header("Held action")]
		[SerializeField]
		private string actionName = "Insert";

		// Token: 0x04008B33 RID: 35635
		[SerializeField]
		private KeyCode inputKey;

		// Token: 0x04008B34 RID: 35636
		[SerializeField]
		private string keyDisplayLabel = "";

		// Token: 0x04008B35 RID: 35637
		[Header("Body socket")]
		[Tooltip("Optional exact transform name under the character rig (for example PussyAnchor). When empty or not found, Socket Bone is used.")]
		[SerializeField]
		private string socketTransformName;

		// Token: 0x04008B36 RID: 35638
		[SerializeField]
		private HumanBodyBones socketBone;

		// Token: 0x04008B37 RID: 35639
		[SerializeField]
		private Vector3 localPosition;

		// Token: 0x04008B38 RID: 35640
		[SerializeField]
		private Vector3 localEulerAngles;

		// Token: 0x04008B39 RID: 35641
		[SerializeField]
		private Vector3 localScale = Vector3.one;

		// Token: 0x04008B3A RID: 35642
		[Header("Body deformation")]
		[SerializeField]
		private string blendShapeName;

		// Token: 0x04008B3B RID: 35643
		[SerializeField]
		[Range(0f, 100f)]
		private float blendShapeWeight = 100f;
	}
}
