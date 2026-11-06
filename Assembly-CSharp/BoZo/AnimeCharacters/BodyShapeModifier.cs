using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000818 RID: 2072
	public class BodyShapeModifier : MonoBehaviour
	{
		// Token: 0x06003746 RID: 14150 RVA: 0x0013F997 File Offset: 0x0013DB97
		private void Awake()
		{
			this.CaptureDefaults();
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x0013F99F File Offset: 0x0013DB9F
		private void Start()
		{
			this.Init();
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x0013F9A8 File Offset: 0x0013DBA8
		private void Init()
		{
			this.system = base.GetComponentInParent<OutfitSystem>();
			this.initalPosition = base.transform.localPosition;
			if (!this.system)
			{
				return;
			}
			if (this.MirrorTransform)
			{
				string text = base.name;
				text = text.Replace("_l", "_r");
				Dictionary<string, Transform> bones = this.system.GetBones();
				this.mirror = bones[text];
				this.initalMirrorPosition = this.mirror.localPosition;
			}
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x0013FA2C File Offset: 0x0013DC2C
		private void CaptureDefaults()
		{
			if (this.defaultsCaptured)
			{
				return;
			}
			this.defaultScaleValue = this.scaleValue;
			this.defaultXScaleValue = this.xScaleValue;
			this.defaultYScaleValue = this.yScaleValue;
			this.defaultZScaleValue = this.zScaleValue;
			this.defaultXPosValue = this.xPosValue;
			this.defaultYPosValue = this.yPosValue;
			this.defaultZPosValue = this.zPosValue;
			this.defaultRotation = this.rotation;
			this.defaultsCaptured = true;
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x0600374A RID: 14154 RVA: 0x0013FAA9 File Offset: 0x0013DCA9
		public float DefaultScaleValue
		{
			get
			{
				return this.defaultScaleValue;
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x0600374B RID: 14155 RVA: 0x0013FAB1 File Offset: 0x0013DCB1
		public float DefaultXScaleValue
		{
			get
			{
				return this.defaultXScaleValue;
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x0600374C RID: 14156 RVA: 0x0013FAB9 File Offset: 0x0013DCB9
		public float DefaultYScaleValue
		{
			get
			{
				return this.defaultYScaleValue;
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x0600374D RID: 14157 RVA: 0x0013FAC1 File Offset: 0x0013DCC1
		public float DefaultZScaleValue
		{
			get
			{
				return this.defaultZScaleValue;
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x0600374E RID: 14158 RVA: 0x0013FAC9 File Offset: 0x0013DCC9
		public float DefaultXPosValue
		{
			get
			{
				return this.defaultXPosValue;
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x0600374F RID: 14159 RVA: 0x0013FAD1 File Offset: 0x0013DCD1
		public float DefaultYPosValue
		{
			get
			{
				return this.defaultYPosValue;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06003750 RID: 14160 RVA: 0x0013FAD9 File Offset: 0x0013DCD9
		public float DefaultZPosValue
		{
			get
			{
				return this.defaultZPosValue;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06003751 RID: 14161 RVA: 0x0013FAE1 File Offset: 0x0013DCE1
		public float DefaultRotation
		{
			get
			{
				return this.defaultRotation;
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06003752 RID: 14162 RVA: 0x0013FAEC File Offset: 0x0013DCEC
		public bool IsAtDefault
		{
			get
			{
				return Mathf.Approximately(this.scaleValue, this.defaultScaleValue) && Mathf.Approximately(this.xScaleValue, this.defaultXScaleValue) && Mathf.Approximately(this.yScaleValue, this.defaultYScaleValue) && Mathf.Approximately(this.zScaleValue, this.defaultZScaleValue) && Mathf.Approximately(this.xPosValue, this.defaultXPosValue) && Mathf.Approximately(this.yPosValue, this.defaultYPosValue) && Mathf.Approximately(this.zPosValue, this.defaultZPosValue) && Mathf.Approximately(this.rotation, this.defaultRotation);
			}
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x0013FB94 File Offset: 0x0013DD94
		public void ResetToDefault()
		{
			this.CaptureDefaults();
			this.SetScale(this.defaultXScaleValue, this.defaultYScaleValue, this.defaultZScaleValue, this.defaultScaleValue);
			this.SetPosition(this.defaultXPosValue, this.defaultYPosValue, this.defaultZPosValue);
			this.SetRotation(this.defaultRotation);
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x0013FBE9 File Offset: 0x0013DDE9
		public void SetScale(float x, float y, float z, float v)
		{
			this.xScaleValue = x;
			this.yScaleValue = y;
			this.zScaleValue = z;
			this.scaleValue = v;
			this.ApplyScale();
		}

		// Token: 0x06003755 RID: 14165 RVA: 0x0013FC0E File Offset: 0x0013DE0E
		public void SetScale(float value)
		{
			this.scaleValue = value;
			this.ApplyScale();
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x0013FC1D File Offset: 0x0013DE1D
		public void SetPosition(float x, float y, float z)
		{
			this.xPosValue = x;
			this.yPosValue = y;
			this.zPosValue = z;
			this.ApplyPosition();
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x0013FC3A File Offset: 0x0013DE3A
		public void SetRotation(float value)
		{
			this.rotation = value;
			this.ApplyRotation();
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x0013FC49 File Offset: 0x0013DE49
		public void Apply()
		{
			if (this.usePosition)
			{
				this.ApplyPosition();
			}
			if (this.useRotation)
			{
				this.ApplyRotation();
			}
			if (this.useScale)
			{
				this.ApplyScale();
			}
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x0013FC78 File Offset: 0x0013DE78
		public void ApplyScale()
		{
			this.scaleValue = Mathf.Clamp(this.scaleValue, this.scaleRange.x, this.scaleRange.y);
			this.xScaleValue = Mathf.Clamp(this.xScaleValue, this.scaleRange.x, this.scaleRange.y);
			this.yScaleValue = Mathf.Clamp(this.yScaleValue, this.scaleRange.x, this.scaleRange.y);
			this.zScaleValue = Mathf.Clamp(this.zScaleValue, this.scaleRange.x, this.scaleRange.y);
			float num = 1f;
			float num2 = 1f;
			float num3 = 1f;
			if (this.linkScaleAxis)
			{
				if (this.useXScale)
				{
					num = this.scaleValue;
				}
				if (this.useYScale)
				{
					num2 = this.scaleValue;
				}
				if (this.useZScale)
				{
					num3 = this.scaleValue;
				}
			}
			else
			{
				if (this.useXScale)
				{
					num = this.xScaleValue;
				}
				if (this.useYScale)
				{
					num2 = this.yScaleValue;
				}
				if (this.useZScale)
				{
					num3 = this.zScaleValue;
				}
			}
			Vector3 vector = new Vector3(num, num2, num3);
			Vector3 vector2 = new Vector3(num - 1f, num2 - 1f, num3 - 1f);
			this.counterValue = Vector3.zero;
			foreach (Vector3 vector3 in this.counterSources.Values)
			{
				this.counterValue += vector3;
			}
			base.transform.localScale = vector - this.counterValue;
			if (this.mirror)
			{
				this.mirror.localScale = vector - this.counterValue;
			}
			foreach (BodyShapeModifier bodyShapeModifier in this.counterScale)
			{
				if (bodyShapeModifier)
				{
					bodyShapeModifier.counterSources[base.name] = vector2;
					bodyShapeModifier.ApplyScale();
				}
			}
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x0013FEA4 File Offset: 0x0013E0A4
		public void ApplyPosition()
		{
			this.xPosValue = Mathf.Clamp(this.xPosValue, this.posRange.x, this.posRange.y);
			this.yPosValue = Mathf.Clamp(this.yPosValue, this.posRange.x, this.posRange.y);
			this.zPosValue = Mathf.Clamp(this.zPosValue, this.posRange.x, this.posRange.y);
			if (this.system == null)
			{
				return;
			}
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			if (this.useXPos)
			{
				num = this.xPosValue;
			}
			if (this.useYPos)
			{
				num2 = this.yPosValue;
			}
			if (this.useZPos)
			{
				num3 = this.zPosValue;
			}
			Vector3 vector = new Vector3(num, num2, num3);
			Vector3 vector2 = new Vector3(-num, num2, num3);
			base.transform.localPosition = vector + this.initalPosition;
			if (this.mirror)
			{
				this.mirror.localPosition = vector2 + this.initalMirrorPosition;
			}
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x0013FFC8 File Offset: 0x0013E1C8
		public void ApplyRotation()
		{
			this.rotation = Mathf.Clamp(this.rotation, this.rotRange.x, this.rotRange.y);
			Vector3 vector = new Vector3(this.rotation * this.rotationAxis.x, this.rotation * this.rotationAxis.y, this.rotation * this.rotationAxis.z);
			base.transform.localRotation = Quaternion.Euler(vector);
			if (this.mirror)
			{
				this.mirror.localRotation = Quaternion.Euler(-vector);
			}
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x00140070 File Offset: 0x0013E270
		public BodyModData GetData()
		{
			return new BodyModData
			{
				scaleValue = this.scaleValue,
				scale = new Vector3(this.xScaleValue, this.yScaleValue, this.zScaleValue),
				posValue = this.posValue,
				position = new Vector3(this.xPosValue, this.yPosValue, this.zPosValue),
				rotation = this.rotation
			};
		}

		// Token: 0x0600375D RID: 14173 RVA: 0x001400E0 File Offset: 0x0013E2E0
		public void SetData(BodyModData data)
		{
			this.scaleValue = data.scaleValue;
			this.xScaleValue = data.scale.x;
			this.yScaleValue = data.scale.y;
			this.zScaleValue = data.scale.z;
			this.posValue = data.posValue;
			this.xPosValue = data.position.x;
			this.yPosValue = data.position.y;
			this.zPosValue = data.position.z;
			this.rotation = data.rotation;
			this.Apply();
		}

		// Token: 0x04003758 RID: 14168
		private OutfitSystem system;

		// Token: 0x04003759 RID: 14169
		private float defaultScaleValue;

		// Token: 0x0400375A RID: 14170
		private float defaultXScaleValue;

		// Token: 0x0400375B RID: 14171
		private float defaultYScaleValue;

		// Token: 0x0400375C RID: 14172
		private float defaultZScaleValue;

		// Token: 0x0400375D RID: 14173
		private float defaultXPosValue;

		// Token: 0x0400375E RID: 14174
		private float defaultYPosValue;

		// Token: 0x0400375F RID: 14175
		private float defaultZPosValue;

		// Token: 0x04003760 RID: 14176
		private float defaultRotation;

		// Token: 0x04003761 RID: 14177
		private bool defaultsCaptured;

		// Token: 0x04003762 RID: 14178
		public string shapeName;

		// Token: 0x04003763 RID: 14179
		public string category = "Body";

		// Token: 0x04003764 RID: 14180
		public string subcategory = "General";

		// Token: 0x04003765 RID: 14181
		public string part;

		// Token: 0x04003766 RID: 14182
		public string sorting;

		// Token: 0x04003767 RID: 14183
		public bool useScale;

		// Token: 0x04003768 RID: 14184
		public float scaleValue = 1f;

		// Token: 0x04003769 RID: 14185
		public float xScaleValue = 1f;

		// Token: 0x0400376A RID: 14186
		public float yScaleValue = 1f;

		// Token: 0x0400376B RID: 14187
		public float zScaleValue = 1f;

		// Token: 0x0400376C RID: 14188
		public BodyShapeModifier[] counterScale;

		// Token: 0x0400376D RID: 14189
		private Vector3 counterValue;

		// Token: 0x0400376E RID: 14190
		public Dictionary<string, Vector3> counterSources = new Dictionary<string, Vector3>();

		// Token: 0x0400376F RID: 14191
		public bool useXScale;

		// Token: 0x04003770 RID: 14192
		public bool useYScale;

		// Token: 0x04003771 RID: 14193
		public bool useZScale;

		// Token: 0x04003772 RID: 14194
		public bool linkScaleAxis;

		// Token: 0x04003773 RID: 14195
		public Vector2 scaleRange = new Vector2(0.5f, 2f);

		// Token: 0x04003774 RID: 14196
		public bool usePosition;

		// Token: 0x04003775 RID: 14197
		private Vector3 initalPosition;

		// Token: 0x04003776 RID: 14198
		private Vector3 initalMirrorPosition;

		// Token: 0x04003777 RID: 14199
		public float posValue;

		// Token: 0x04003778 RID: 14200
		public float xPosValue;

		// Token: 0x04003779 RID: 14201
		public float yPosValue;

		// Token: 0x0400377A RID: 14202
		public float zPosValue;

		// Token: 0x0400377B RID: 14203
		public bool useXPos;

		// Token: 0x0400377C RID: 14204
		public bool useYPos;

		// Token: 0x0400377D RID: 14205
		public bool useZPos;

		// Token: 0x0400377E RID: 14206
		public Vector2 posRange = new Vector2(-0.02f, 0.02f);

		// Token: 0x0400377F RID: 14207
		public bool useRotation;

		// Token: 0x04003780 RID: 14208
		public float rotation;

		// Token: 0x04003781 RID: 14209
		public Vector3 rotationAxis;

		// Token: 0x04003782 RID: 14210
		public Vector2 rotRange = new Vector2(-90f, 90f);

		// Token: 0x04003783 RID: 14211
		[SerializeField]
		private bool MirrorTransform;

		// Token: 0x04003784 RID: 14212
		private Transform mirror;
	}
}
