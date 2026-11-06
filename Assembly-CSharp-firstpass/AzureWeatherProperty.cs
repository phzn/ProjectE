using System;
using System.Reflection;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000024 RID: 36
	[Serializable]
	public sealed class AzureWeatherProperty
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000792A File Offset: 0x00005B2A
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x00007932 File Offset: 0x00005B32
		public string name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x0000793B File Offset: 0x00005B3B
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x00007943 File Offset: 0x00005B43
		public AzureWeatherPropertyType propertyType
		{
			get
			{
				return this.m_propertyType;
			}
			set
			{
				this.m_propertyType = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x0000794C File Offset: 0x00005B4C
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x00007954 File Offset: 0x00005B54
		public float minValue
		{
			get
			{
				return this.m_minValue;
			}
			set
			{
				this.m_minValue = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000795D File Offset: 0x00005B5D
		// (set) Token: 0x060001DB RID: 475 RVA: 0x00007965 File Offset: 0x00005B65
		public float maxValue
		{
			get
			{
				return this.m_maxValue;
			}
			set
			{
				this.m_maxValue = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0000796E File Offset: 0x00005B6E
		// (set) Token: 0x060001DD RID: 477 RVA: 0x00007976 File Offset: 0x00005B76
		public float defaultFloatValue
		{
			get
			{
				return this.m_defaultFloatValue;
			}
			set
			{
				this.m_defaultFloatValue = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000797F File Offset: 0x00005B7F
		// (set) Token: 0x060001DF RID: 479 RVA: 0x00007987 File Offset: 0x00005B87
		public Color defaultColorValue
		{
			get
			{
				return this.m_defaultColorValue;
			}
			set
			{
				this.m_defaultColorValue = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00007990 File Offset: 0x00005B90
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00007998 File Offset: 0x00005B98
		public AnimationCurve defaultCurveValue
		{
			get
			{
				return this.m_defaultCurveValue;
			}
			set
			{
				this.m_defaultCurveValue = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x000079A1 File Offset: 0x00005BA1
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x000079A9 File Offset: 0x00005BA9
		public Gradient defaultGradientValue
		{
			get
			{
				return this.m_defaultGradientValue;
			}
			set
			{
				this.m_defaultGradientValue = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x000079B2 File Offset: 0x00005BB2
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x000079BA File Offset: 0x00005BBA
		public Vector3 defaultVector3Value
		{
			get
			{
				return this.m_defaultVector3Value;
			}
			set
			{
				this.m_defaultVector3Value = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x000079C3 File Offset: 0x00005BC3
		// (set) Token: 0x060001E7 RID: 487 RVA: 0x000079CB File Offset: 0x00005BCB
		public AzureWeatherPropertyOverrideMode overrideMode
		{
			get
			{
				return this.m_overrideMode;
			}
			set
			{
				this.m_overrideMode = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x000079D4 File Offset: 0x00005BD4
		// (set) Token: 0x060001E9 RID: 489 RVA: 0x000079DC File Offset: 0x00005BDC
		public AzureWeatherPropertyTargetType targetType
		{
			get
			{
				return this.m_targetType;
			}
			set
			{
				this.m_targetType = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060001EA RID: 490 RVA: 0x000079E5 File Offset: 0x00005BE5
		// (set) Token: 0x060001EB RID: 491 RVA: 0x000079ED File Offset: 0x00005BED
		public Type targetGlobalType
		{
			get
			{
				return this.m_targetGlobalType;
			}
			set
			{
				this.m_targetGlobalType = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000079F6 File Offset: 0x00005BF6
		// (set) Token: 0x060001ED RID: 493 RVA: 0x000079FE File Offset: 0x00005BFE
		public GameObject targetObject
		{
			get
			{
				return this.m_targetObject;
			}
			set
			{
				this.m_targetObject = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060001EE RID: 494 RVA: 0x00007A07 File Offset: 0x00005C07
		// (set) Token: 0x060001EF RID: 495 RVA: 0x00007A0F File Offset: 0x00005C0F
		public Component targetComponent
		{
			get
			{
				return this.m_targetComponent;
			}
			set
			{
				this.m_targetComponent = value;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x00007A18 File Offset: 0x00005C18
		// (set) Token: 0x060001F1 RID: 497 RVA: 0x00007A20 File Offset: 0x00005C20
		public Material targetMaterial
		{
			get
			{
				return this.m_targetMaterial;
			}
			set
			{
				this.m_targetMaterial = value;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00007A29 File Offset: 0x00005C29
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x00007A31 File Offset: 0x00005C31
		public string targetComponentName
		{
			get
			{
				return this.m_targetComponentName;
			}
			set
			{
				this.m_targetComponentName = value;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00007A3A File Offset: 0x00005C3A
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x00007A42 File Offset: 0x00005C42
		public string targetPropertyName
		{
			get
			{
				return this.m_targetPropertyName;
			}
			set
			{
				this.m_targetPropertyName = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00007A4B File Offset: 0x00005C4B
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x00007A53 File Offset: 0x00005C53
		public FieldInfo fieldInfo
		{
			get
			{
				return this.m_fieldInfo;
			}
			set
			{
				this.m_fieldInfo = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00007A5C File Offset: 0x00005C5C
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x00007A64 File Offset: 0x00005C64
		public PropertyInfo propertyInfo
		{
			get
			{
				return this.m_propertyInfo;
			}
			set
			{
				this.m_propertyInfo = value;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060001FA RID: 506 RVA: 0x00007A6D File Offset: 0x00005C6D
		// (set) Token: 0x060001FB RID: 507 RVA: 0x00007A75 File Offset: 0x00005C75
		public float floatOutput
		{
			get
			{
				return this.m_floatOutput;
			}
			set
			{
				this.m_floatOutput = value;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060001FC RID: 508 RVA: 0x00007A7E File Offset: 0x00005C7E
		// (set) Token: 0x060001FD RID: 509 RVA: 0x00007A86 File Offset: 0x00005C86
		public Color colorOutput
		{
			get
			{
				return this.m_colorOutput;
			}
			set
			{
				this.m_colorOutput = value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00007A8F File Offset: 0x00005C8F
		// (set) Token: 0x060001FF RID: 511 RVA: 0x00007A97 File Offset: 0x00005C97
		public Vector3 vector3Output
		{
			get
			{
				return this.m_vector3Output;
			}
			set
			{
				this.m_vector3Output = value;
			}
		}

		// Token: 0x04000166 RID: 358
		[SerializeField]
		private string m_name = "MyPropertyName";

		// Token: 0x04000167 RID: 359
		[SerializeField]
		private AzureWeatherPropertyType m_propertyType;

		// Token: 0x04000168 RID: 360
		[SerializeField]
		private float m_minValue;

		// Token: 0x04000169 RID: 361
		[SerializeField]
		private float m_maxValue = 1f;

		// Token: 0x0400016A RID: 362
		[SerializeField]
		private float m_defaultFloatValue;

		// Token: 0x0400016B RID: 363
		[SerializeField]
		private Color m_defaultColorValue = Color.white;

		// Token: 0x0400016C RID: 364
		[SerializeField]
		private AnimationCurve m_defaultCurveValue = AnimationCurve.Linear(0f, 0.5f, 24f, 0.5f);

		// Token: 0x0400016D RID: 365
		[SerializeField]
		private Gradient m_defaultGradientValue = new Gradient();

		// Token: 0x0400016E RID: 366
		[SerializeField]
		private Vector3 m_defaultVector3Value = Vector3.zero;

		// Token: 0x0400016F RID: 367
		[SerializeField]
		private AzureWeatherPropertyOverrideMode m_overrideMode;

		// Token: 0x04000170 RID: 368
		[SerializeField]
		private AzureWeatherPropertyTargetType m_targetType;

		// Token: 0x04000171 RID: 369
		[SerializeField]
		private Type m_targetGlobalType;

		// Token: 0x04000172 RID: 370
		[SerializeField]
		private GameObject m_targetObject;

		// Token: 0x04000173 RID: 371
		[SerializeField]
		private Component m_targetComponent;

		// Token: 0x04000174 RID: 372
		[SerializeField]
		private Material m_targetMaterial;

		// Token: 0x04000175 RID: 373
		[SerializeField]
		private string m_targetComponentName;

		// Token: 0x04000176 RID: 374
		[SerializeField]
		private string m_targetPropertyName;

		// Token: 0x04000177 RID: 375
		private FieldInfo m_fieldInfo;

		// Token: 0x04000178 RID: 376
		private PropertyInfo m_propertyInfo;

		// Token: 0x04000179 RID: 377
		private float m_floatOutput;

		// Token: 0x0400017A RID: 378
		private Color m_colorOutput;

		// Token: 0x0400017B RID: 379
		private Vector3 m_vector3Output;
	}
}
