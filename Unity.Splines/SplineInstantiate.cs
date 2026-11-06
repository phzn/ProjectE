using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine.Serialization;

namespace UnityEngine.Splines
{
	// Token: 0x0200002A RID: 42
	[ExecuteInEditMode]
	[AddComponentMenu("Splines/Spline Instantiate")]
	public class SplineInstantiate : SplineComponent
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000204 RID: 516 RVA: 0x000096ED File Offset: 0x000078ED
		[Obsolete("Use Container instead.", false)]
		public SplineContainer container
		{
			get
			{
				return this.Container;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000205 RID: 517 RVA: 0x000096F5 File Offset: 0x000078F5
		// (set) Token: 0x06000206 RID: 518 RVA: 0x000096FD File Offset: 0x000078FD
		public SplineContainer Container
		{
			get
			{
				return this.m_Container;
			}
			set
			{
				this.m_Container = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00009706 File Offset: 0x00007906
		// (set) Token: 0x06000208 RID: 520 RVA: 0x00009713 File Offset: 0x00007913
		public SplineInstantiate.InstantiableItem[] itemsToInstantiate
		{
			get
			{
				return this.m_ItemsToInstantiate.ToArray();
			}
			set
			{
				this.m_ItemsToInstantiate.Clear();
				this.m_ItemsToInstantiate.AddRange(value);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000972C File Offset: 0x0000792C
		[Obsolete("Use InstantiateMethod instead.", false)]
		public SplineInstantiate.Method method
		{
			get
			{
				return this.InstantiateMethod;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00009734 File Offset: 0x00007934
		// (set) Token: 0x0600020B RID: 523 RVA: 0x0000973C File Offset: 0x0000793C
		public SplineInstantiate.Method InstantiateMethod
		{
			get
			{
				return this.m_Method;
			}
			set
			{
				this.m_Method = value;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600020C RID: 524 RVA: 0x00009745 File Offset: 0x00007945
		[Obsolete("Use CoordinateSpace instead.", false)]
		public SplineInstantiate.Space space
		{
			get
			{
				return this.CoordinateSpace;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000974D File Offset: 0x0000794D
		// (set) Token: 0x0600020E RID: 526 RVA: 0x00009755 File Offset: 0x00007955
		public SplineInstantiate.Space CoordinateSpace
		{
			get
			{
				return this.m_Space;
			}
			set
			{
				this.m_Space = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000975E File Offset: 0x0000795E
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0000976B File Offset: 0x0000796B
		public float MinSpacing
		{
			get
			{
				return this.m_Spacing.x;
			}
			set
			{
				this.m_Spacing = new Vector2(value, this.m_Spacing.y);
				this.ValidateSpacing();
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000978A File Offset: 0x0000798A
		// (set) Token: 0x06000212 RID: 530 RVA: 0x00009797 File Offset: 0x00007997
		public float MaxSpacing
		{
			get
			{
				return this.m_Spacing.y;
			}
			set
			{
				this.m_Spacing = new Vector2(this.m_Spacing.x, value);
				this.ValidateSpacing();
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000213 RID: 531 RVA: 0x000097B6 File Offset: 0x000079B6
		[Obsolete("Use UpAxis instead.", false)]
		public SplineComponent.AlignAxis upAxis
		{
			get
			{
				return this.UpAxis;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000214 RID: 532 RVA: 0x000097BE File Offset: 0x000079BE
		// (set) Token: 0x06000215 RID: 533 RVA: 0x000097C6 File Offset: 0x000079C6
		public SplineComponent.AlignAxis UpAxis
		{
			get
			{
				return this.m_Up;
			}
			set
			{
				this.m_Up = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000216 RID: 534 RVA: 0x000097CF File Offset: 0x000079CF
		[Obsolete("Use ForwardAxis instead.", false)]
		public SplineComponent.AlignAxis forwardAxis
		{
			get
			{
				return this.ForwardAxis;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000217 RID: 535 RVA: 0x000097D7 File Offset: 0x000079D7
		// (set) Token: 0x06000218 RID: 536 RVA: 0x000097DF File Offset: 0x000079DF
		public SplineComponent.AlignAxis ForwardAxis
		{
			get
			{
				return this.m_Forward;
			}
			set
			{
				this.m_Forward = value;
				this.ValidateAxis();
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000219 RID: 537 RVA: 0x000097EE File Offset: 0x000079EE
		[Obsolete("Use MinPositionOffset instead.", false)]
		public Vector3 minPositionOffset
		{
			get
			{
				return this.MinPositionOffset;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600021A RID: 538 RVA: 0x000097F6 File Offset: 0x000079F6
		// (set) Token: 0x0600021B RID: 539 RVA: 0x00009803 File Offset: 0x00007A03
		public Vector3 MinPositionOffset
		{
			get
			{
				return this.m_PositionOffset.min;
			}
			set
			{
				this.m_PositionOffset.min = value;
				this.m_PositionOffset.CheckMinMax();
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000981C File Offset: 0x00007A1C
		[Obsolete("Use MaxPositionOffset instead.", false)]
		public Vector3 maxPositionOffset
		{
			get
			{
				return this.MaxPositionOffset;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00009824 File Offset: 0x00007A24
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00009831 File Offset: 0x00007A31
		public Vector3 MaxPositionOffset
		{
			get
			{
				return this.m_PositionOffset.max;
			}
			set
			{
				this.m_PositionOffset.max = value;
				this.m_PositionOffset.CheckMinMax();
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000984A File Offset: 0x00007A4A
		[Obsolete("Use PositionSpace instead.", false)]
		public SplineInstantiate.OffsetSpace positionSpace
		{
			get
			{
				return this.PositionSpace;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00009852 File Offset: 0x00007A52
		// (set) Token: 0x06000221 RID: 545 RVA: 0x0000985F File Offset: 0x00007A5F
		public SplineInstantiate.OffsetSpace PositionSpace
		{
			get
			{
				return this.m_PositionOffset.space;
			}
			set
			{
				this.m_PositionOffset.space = value;
				this.m_PositionOffset.CheckCustomSpace(this.m_Space);
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000987E File Offset: 0x00007A7E
		[Obsolete("Use MinRotationOffset instead.", false)]
		public Vector3 minRotationOffset
		{
			get
			{
				return this.MinRotationOffset;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00009886 File Offset: 0x00007A86
		// (set) Token: 0x06000224 RID: 548 RVA: 0x00009893 File Offset: 0x00007A93
		public Vector3 MinRotationOffset
		{
			get
			{
				return this.m_RotationOffset.min;
			}
			set
			{
				this.m_RotationOffset.min = value;
				this.m_RotationOffset.CheckMinMax();
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000225 RID: 549 RVA: 0x000098AC File Offset: 0x00007AAC
		[Obsolete("Use MaxRotationOffset instead.", false)]
		public Vector3 maxRotationOffset
		{
			get
			{
				return this.MaxRotationOffset;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000226 RID: 550 RVA: 0x000098B4 File Offset: 0x00007AB4
		// (set) Token: 0x06000227 RID: 551 RVA: 0x000098C1 File Offset: 0x00007AC1
		public Vector3 MaxRotationOffset
		{
			get
			{
				return this.m_RotationOffset.max;
			}
			set
			{
				this.m_RotationOffset.max = value;
				this.m_RotationOffset.CheckMinMax();
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000228 RID: 552 RVA: 0x000098DA File Offset: 0x00007ADA
		[Obsolete("Use RotationSpace instead.", false)]
		public SplineInstantiate.OffsetSpace rotationSpace
		{
			get
			{
				return this.RotationSpace;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000098E2 File Offset: 0x00007AE2
		// (set) Token: 0x0600022A RID: 554 RVA: 0x000098EF File Offset: 0x00007AEF
		public SplineInstantiate.OffsetSpace RotationSpace
		{
			get
			{
				return this.m_RotationOffset.space;
			}
			set
			{
				this.m_RotationOffset.space = value;
				this.m_RotationOffset.CheckCustomSpace(this.m_Space);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000990E File Offset: 0x00007B0E
		[Obsolete("Use MinScaleOffset instead.", false)]
		public Vector3 minScaleOffset
		{
			get
			{
				return this.MinScaleOffset;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00009916 File Offset: 0x00007B16
		// (set) Token: 0x0600022D RID: 557 RVA: 0x00009923 File Offset: 0x00007B23
		public Vector3 MinScaleOffset
		{
			get
			{
				return this.m_ScaleOffset.min;
			}
			set
			{
				this.m_ScaleOffset.min = value;
				this.m_ScaleOffset.CheckMinMax();
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000993C File Offset: 0x00007B3C
		[Obsolete("Use MaxScaleOffset instead.", false)]
		public Vector3 maxScaleOffset
		{
			get
			{
				return this.MaxScaleOffset;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00009944 File Offset: 0x00007B44
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00009951 File Offset: 0x00007B51
		public Vector3 MaxScaleOffset
		{
			get
			{
				return this.m_ScaleOffset.max;
			}
			set
			{
				this.m_ScaleOffset.max = value;
				this.m_ScaleOffset.CheckMinMax();
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000996A File Offset: 0x00007B6A
		[Obsolete("Use ScaleSpace instead.", false)]
		public SplineInstantiate.OffsetSpace scaleSpace
		{
			get
			{
				return this.ScaleSpace;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00009972 File Offset: 0x00007B72
		// (set) Token: 0x06000233 RID: 563 RVA: 0x0000997F File Offset: 0x00007B7F
		public SplineInstantiate.OffsetSpace ScaleSpace
		{
			get
			{
				return this.m_ScaleOffset.space;
			}
			set
			{
				this.m_ScaleOffset.space = value;
				this.m_ScaleOffset.CheckCustomSpace(this.m_Space);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000999E File Offset: 0x00007B9E
		internal GameObject InstancesRoot
		{
			get
			{
				return this.m_InstancesRoot;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000235 RID: 565 RVA: 0x000099A8 File Offset: 0x00007BA8
		private Transform instancesRootTransform
		{
			get
			{
				if (this.m_InstancesRoot == null)
				{
					this.m_InstancesRoot = new GameObject("root-" + base.GetInstanceID().ToString());
					this.m_InstancesRoot.hideFlags |= HideFlags.HideAndDontSave;
					this.m_InstancesRoot.transform.parent = base.transform;
					this.m_InstancesRoot.transform.localPosition = Vector3.zero;
					this.m_InstancesRoot.transform.localRotation = Quaternion.identity;
				}
				return this.m_InstancesRoot.transform;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00009A45 File Offset: 0x00007C45
		internal List<GameObject> instances
		{
			get
			{
				return this.m_Instances;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00009A4D File Offset: 0x00007C4D
		// (set) Token: 0x06000238 RID: 568 RVA: 0x00009A55 File Offset: 0x00007C55
		private float maxProbability
		{
			get
			{
				return this.m_MaxProbability;
			}
			set
			{
				if (this.m_MaxProbability != value)
				{
					this.m_MaxProbability = value;
					this.m_InstancesCacheDirty = true;
				}
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00009A6E File Offset: 0x00007C6E
		// (set) Token: 0x0600023A RID: 570 RVA: 0x00009A76 File Offset: 0x00007C76
		public int Seed
		{
			get
			{
				return this.m_Seed;
			}
			set
			{
				this.m_Seed = value;
				this.m_InstancesCacheDirty = true;
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00009A86 File Offset: 0x00007C86
		private void OnEnable()
		{
			if (this.m_Seed == 0)
			{
				this.m_Seed = base.GetInstanceID();
			}
			this.CheckChildrenValidity();
			Spline.Changed += this.OnSplineChanged;
			this.UpdateInstances();
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00009AB9 File Offset: 0x00007CB9
		private void OnDisable()
		{
			Spline.Changed -= this.OnSplineChanged;
			this.Clear();
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00009AD2 File Offset: 0x00007CD2
		private void UndoRedoPerformed()
		{
			this.m_InstancesCacheDirty = true;
			this.m_SplineDirty = true;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00009AE2 File Offset: 0x00007CE2
		private void OnValidate()
		{
			this.ValidateSpacing();
			this.m_SplineDirty = this.m_AutoRefresh;
			this.EnsureItemsValidity();
			this.m_PositionOffset.CheckMinMaxValidity();
			this.m_RotationOffset.CheckMinMaxValidity();
			this.m_ScaleOffset.CheckMinMaxValidity();
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00009B20 File Offset: 0x00007D20
		private void EnsureItemsValidity()
		{
			float num = 0f;
			for (int i = 0; i < this.m_ItemsToInstantiate.Count; i++)
			{
				SplineInstantiate.InstantiableItem instantiableItem = this.m_ItemsToInstantiate[i];
				if (instantiableItem.Prefab != null)
				{
					if (base.transform.IsChildOf(instantiableItem.Prefab.transform))
					{
						Debug.LogWarning(string.Concat(new string[]
						{
							"Instantiating a parent of the SplineInstantiate object itself is not permitted (",
							instantiableItem.Prefab.name,
							" is a parent of ",
							base.transform.gameObject.name,
							")."
						}), this);
						instantiableItem.Prefab = null;
						this.m_ItemsToInstantiate[i] = instantiableItem;
					}
					else
					{
						num += instantiableItem.Probability;
					}
				}
			}
			this.maxProbability = num;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00009BF8 File Offset: 0x00007DF8
		private void CheckChildrenValidity()
		{
			List<int> list = (from sInstantiate in base.GetComponents<SplineInstantiate>()
				select sInstantiate.GetInstanceID()).ToList<int>();
			for (int i = base.transform.childCount - 1; i >= 0; i--)
			{
				GameObject gameObject = base.transform.GetChild(i).gameObject;
				if (gameObject.name.StartsWith("root-"))
				{
					bool flag = true;
					foreach (int num in list)
					{
						if (gameObject.name.Equals("root-" + num.ToString()))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						Object.Destroy(gameObject);
					}
				}
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00009CE0 File Offset: 0x00007EE0
		private void ValidateSpacing()
		{
			float num = Mathf.Max(0.1f, this.m_Spacing.x);
			if (this.m_Method != SplineInstantiate.Method.LinearDistance)
			{
				float num2 = (float.IsNaN(this.m_Spacing.y) ? num : Mathf.Max(0.1f, this.m_Spacing.y));
				this.m_Spacing = new Vector2(num, Mathf.Max(num, num2));
				return;
			}
			if (this.m_Method == SplineInstantiate.Method.LinearDistance)
			{
				float num3 = (float.IsNaN(this.m_Spacing.y) ? this.m_Spacing.y : num);
				this.m_Spacing = new Vector2(num, num3);
			}
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00009D82 File Offset: 0x00007F82
		private void ValidateAxis()
		{
			if (this.m_Forward == this.m_Up || this.m_Forward == (this.m_Up + 3) % (SplineComponent.AlignAxis)6)
			{
				this.m_Forward = (this.m_Forward + 1) % (SplineComponent.AlignAxis)6;
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00009DB4 File Offset: 0x00007FB4
		internal void SetSplineDirty(Spline spline)
		{
			if (this.m_Container != null && this.m_Container.Splines.Contains(spline) && this.m_AutoRefresh)
			{
				this.UpdateInstances();
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00009DE5 File Offset: 0x00007FE5
		private void InitContainer()
		{
			if (this.m_Container == null)
			{
				this.m_Container = base.GetComponent<SplineContainer>();
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00009E01 File Offset: 0x00008001
		public void Clear()
		{
			this.SetDirty();
			this.TryClearCache();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00009E0F File Offset: 0x0000800F
		public void SetDirty()
		{
			this.m_InstancesCacheDirty = true;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00009E18 File Offset: 0x00008018
		private void TryClearCache()
		{
			if (!this.m_InstancesCacheDirty)
			{
				for (int i = 0; i < this.m_Instances.Count; i++)
				{
					if (this.m_Instances[i] == null)
					{
						this.m_InstancesCacheDirty = true;
						break;
					}
				}
			}
			if (this.m_InstancesCacheDirty)
			{
				for (int j = this.m_Instances.Count - 1; j >= 0; j--)
				{
					Object.Destroy(this.m_Instances[j]);
				}
				Object.Destroy(this.m_InstancesRoot);
				this.m_Instances.Clear();
				this.m_InstancesCacheDirty = false;
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00009EB0 File Offset: 0x000080B0
		private void ClearDeprecatedInstances()
		{
			foreach (GameObject gameObject in this.m_DeprecatedInstances)
			{
				Object.Destroy(gameObject);
			}
			this.m_DeprecatedInstances.Clear();
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00009F0C File Offset: 0x0000810C
		public void Randomize()
		{
			this.Seed = Random.Range(int.MinValue, int.MaxValue);
			this.m_SplineDirty = true;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00009F2A File Offset: 0x0000812A
		private void Update()
		{
			if (this.m_SplineDirty)
			{
				this.UpdateInstances();
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00009F3C File Offset: 0x0000813C
		public void UpdateInstances()
		{
			this.ClearDeprecatedInstances();
			this.TryClearCache();
			if (this.m_Container == null)
			{
				this.InitContainer();
			}
			if (this.m_Container == null || this.m_Container.Splines.Count == 0 || this.m_ItemsToInstantiate.Count == 0)
			{
				return;
			}
			Random.State state = Random.state;
			Random.InitState(this.m_Seed);
			int num = 0;
			int num2 = 0;
			this.m_LengthsCache.Clear();
			float num3 = 0f;
			for (int i = 0; i < this.m_Container.Splines.Count; i++)
			{
				float num4 = this.m_Container.CalculateLength(i);
				this.m_LengthsCache.Add(num4);
				num3 += num4;
			}
			float num5 = Random.Range(this.m_Spacing.x, this.m_Spacing.y);
			float num6 = 0f;
			float num7 = 0f;
			if (this.m_Method == SplineInstantiate.Method.InstanceCount)
			{
				if (num5 == 1f)
				{
					num6 = num3 / 2f;
				}
				else if (num5 < 1f)
				{
					num6 = num3 + 1f;
				}
				if (this.m_Container.Splines.Count == 1)
				{
					num7 = num3 / (float)(this.m_Container.Splines[0].Closed ? ((int)num5) : ((int)num5 - 1));
				}
				else
				{
					num7 = num3 / (float)((int)num5 - 1);
				}
			}
			this.EnsureItemsValidity();
			for (int j = 0; j < this.m_Container.Splines.Count; j++)
			{
				Spline spline = this.m_Container.Splines[j];
				using (NativeSpline nativeSpline = new NativeSpline(spline, this.m_Container.transform.localToWorldMatrix, Allocator.TempJob))
				{
					float num8 = this.m_LengthsCache[j];
					bool flag = false;
					if (this.m_Method == SplineInstantiate.Method.InstanceCount)
					{
						if (num6 > num8 + 0.001f && num6 <= num3 + 0.001f)
						{
							num6 -= num8;
							flag = true;
						}
					}
					else
					{
						num6 = 0f;
					}
					this.m_TimesCache.Clear();
					int num9 = 0;
					while (num6 <= num8 + 0.001f && !flag && this.SpawnPrefab(num))
					{
						this.m_TimesCache.Add(num6 / num8);
						if (this.m_Method == SplineInstantiate.Method.SpacingDistance)
						{
							num5 = Random.Range(this.m_Spacing.x, this.m_Spacing.y);
							num6 += num5;
						}
						else if (this.m_Method == SplineInstantiate.Method.InstanceCount)
						{
							if (num5 > 1f)
							{
								float num10 = num6;
								num6 += num7;
								if (num10 < num8 && num6 > num8 + 0.001f)
								{
									num6 -= num8;
									flag = true;
								}
							}
							else
							{
								num6 += num3;
							}
						}
						else if (this.m_Method == SplineInstantiate.Method.LinearDistance)
						{
							if (float.IsNaN(this.m_Spacing.y))
							{
								MeshFilter meshFilter = this.m_Instances[num].GetComponent<MeshFilter>();
								Vector3 vector = Vector3.right;
								if (this.m_Forward == SplineComponent.AlignAxis.ZAxis || this.m_Forward == SplineComponent.AlignAxis.NegativeZAxis)
								{
									vector = Vector3.forward;
								}
								if (this.m_Forward == SplineComponent.AlignAxis.YAxis || this.m_Forward == SplineComponent.AlignAxis.NegativeYAxis)
								{
									vector = Vector3.up;
								}
								if (meshFilter == null)
								{
									meshFilter = this.m_Instances[num].GetComponentInChildren<MeshFilter>();
									if (meshFilter != null)
									{
										vector = Vector3.Scale(meshFilter.transform.InverseTransformDirection(this.m_Instances[num].transform.TransformDirection(vector)), meshFilter.transform.lossyScale);
									}
								}
								if (meshFilter != null)
								{
									Bounds bounds = meshFilter.sharedMesh.bounds;
									MeshFilter[] componentsInChildren = meshFilter.GetComponentsInChildren<MeshFilter>();
									for (int k = 0; k < componentsInChildren.Length; k++)
									{
										Bounds bounds2 = componentsInChildren[k].sharedMesh.bounds;
										bounds.size = new Vector3(Mathf.Max(bounds.size.x, bounds2.size.x), Mathf.Max(bounds.size.z, bounds2.size.z), Mathf.Max(bounds.size.z, bounds2.size.z));
									}
									num5 = Vector3.Scale(bounds.size, vector).magnitude;
								}
							}
							else
							{
								num5 = Random.Range(this.m_Spacing.x, this.m_Spacing.y);
							}
							float num11;
							nativeSpline.GetPointAtLinearDistance(this.m_TimesCache[num9], num5, out num11);
							num6 = ((num11 >= 1f) ? (num8 + 1f) : (num11 * num8));
						}
						num++;
						num9++;
					}
					for (int l = this.m_Instances.Count - 1; l >= num; l--)
					{
						if (this.m_Instances[l] != null)
						{
							Object.Destroy(this.m_Instances[l]);
							this.m_Instances.RemoveAt(l);
						}
					}
					for (int m = num2; m < num; m++)
					{
						GameObject gameObject = this.m_Instances[m];
						float num12 = this.m_TimesCache[m - num2];
						float3 @float;
						float3 float2;
						float3 float3;
						nativeSpline.Evaluate(num12, out @float, out float2, out float3);
						gameObject.transform.position = @float;
						if (this.m_Method == SplineInstantiate.Method.LinearDistance)
						{
							float2 = nativeSpline.EvaluatePosition((m + 1 < num) ? this.m_TimesCache[m + 1 - num2] : 1f) - @float;
						}
						float3 float4 = math.normalizesafe(float3, default(float3));
						float3 float5 = math.normalizesafe(float2, default(float3));
						if (this.m_Space == SplineInstantiate.Space.World)
						{
							float4 = Vector3.up;
							float5 = Vector3.forward;
						}
						else if (this.m_Space == SplineInstantiate.Space.Local)
						{
							float4 = base.transform.TransformDirection(Vector3.up);
							float5 = base.transform.TransformDirection(Vector3.forward);
						}
						float3 float6 = math.normalizesafe(base.GetAxis(this.m_Forward), default(float3));
						float3 float7 = math.normalizesafe(base.GetAxis(this.m_Up), default(float3));
						Quaternion quaternion = Quaternion.Inverse(quaternion.LookRotationSafe(float6, float7));
						gameObject.transform.rotation = quaternion.LookRotationSafe(float5, float4) * quaternion;
						float3 float8 = float4;
						float3 float9 = float5;
						if (this.m_PositionOffset.hasOffset)
						{
							if (this.m_PositionOffset.hasCustomSpace)
							{
								this.GetCustomSpaceAxis(this.m_PositionOffset.space, float3, float2, gameObject.transform, out float8, out float9);
							}
							Vector3 nextOffset = this.m_PositionOffset.GetNextOffset();
							Vector3 normalized = Vector3.Cross(float8, float9).normalized;
							gameObject.transform.position += nextOffset.x * normalized + nextOffset.y * float8 + nextOffset.z * float9;
						}
						if (this.m_ScaleOffset.hasOffset)
						{
							float8 = float4;
							float9 = float5;
							if (this.m_ScaleOffset.hasCustomSpace)
							{
								this.GetCustomSpaceAxis(this.m_ScaleOffset.space, float3, float2, gameObject.transform, out float8, out float9);
							}
							float8 = gameObject.transform.InverseTransformDirection(float8).normalized;
							float9 = gameObject.transform.InverseTransformDirection(float9).normalized;
							Vector3 nextOffset2 = this.m_ScaleOffset.GetNextOffset();
							Vector3 normalized2 = Vector3.Cross(float8, float9).normalized;
							gameObject.transform.localScale += nextOffset2.x * normalized2 + nextOffset2.y * float8 + nextOffset2.z * float9;
						}
						if (this.m_RotationOffset.hasOffset)
						{
							float8 = float4;
							float9 = float5;
							if (this.m_RotationOffset.hasCustomSpace)
							{
								this.GetCustomSpaceAxis(this.m_RotationOffset.space, float3, float2, gameObject.transform, out float8, out float9);
								if (this.m_RotationOffset.space == SplineInstantiate.OffsetSpace.Object)
								{
									quaternion = quaternion.identity;
								}
							}
							Vector3 nextOffset3 = this.m_RotationOffset.GetNextOffset();
							Vector3 normalized3 = Vector3.Cross(float8, float9).normalized;
							float9 = Quaternion.AngleAxis(nextOffset3.y, float8) * Quaternion.AngleAxis(nextOffset3.x, normalized3) * float9;
							float8 = Quaternion.AngleAxis(nextOffset3.x, normalized3) * Quaternion.AngleAxis(nextOffset3.z, float9) * float8;
							gameObject.transform.rotation = quaternion.LookRotationSafe(float9, float8) * quaternion;
						}
					}
					num2 = num;
				}
			}
			this.m_SplineDirty = false;
			Random.state = state;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000A8E4 File Offset: 0x00008AE4
		private bool SpawnPrefab(int index)
		{
			int num = ((this.m_ItemsToInstantiate.Count == 1) ? 0 : this.GetPrefabIndex());
			this.m_CurrentItem = this.m_ItemsToInstantiate[num];
			if (this.m_CurrentItem.Prefab == null)
			{
				return false;
			}
			if (index >= this.m_Instances.Count)
			{
				this.m_Instances.Add(Object.Instantiate<GameObject>(this.m_CurrentItem.Prefab, this.instancesRootTransform));
			}
			this.m_Instances[index].transform.localPosition = this.m_CurrentItem.Prefab.transform.localPosition;
			this.m_Instances[index].transform.localRotation = this.m_CurrentItem.Prefab.transform.localRotation;
			this.m_Instances[index].transform.localScale = this.m_CurrentItem.Prefab.transform.localScale;
			return true;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000A9E4 File Offset: 0x00008BE4
		private void GetCustomSpaceAxis(SplineInstantiate.OffsetSpace space, float3 splineUp, float3 direction, Transform instanceTransform, out float3 customUp, out float3 customForward)
		{
			customUp = Vector3.up;
			customForward = Vector3.forward;
			if (space == SplineInstantiate.OffsetSpace.Local)
			{
				customUp = base.transform.TransformDirection(Vector3.up);
				customForward = base.transform.TransformDirection(Vector3.forward);
				return;
			}
			if (space == SplineInstantiate.OffsetSpace.Spline)
			{
				customUp = splineUp;
				customForward = direction;
				return;
			}
			if (space == SplineInstantiate.OffsetSpace.Object)
			{
				customUp = instanceTransform.TransformDirection(Vector3.up);
				customForward = instanceTransform.TransformDirection(Vector3.forward);
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000AA98 File Offset: 0x00008C98
		private int GetPrefabIndex()
		{
			float num = Random.Range(0f, this.m_MaxProbability);
			float num2 = 0f;
			for (int i = 0; i < this.m_ItemsToInstantiate.Count; i++)
			{
				if (!(this.m_ItemsToInstantiate[i].Prefab == null))
				{
					float probability = this.m_ItemsToInstantiate[i].Probability;
					if (num < num2 + probability)
					{
						return i;
					}
					num2 += probability;
				}
			}
			return 0;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000AB0A File Offset: 0x00008D0A
		private void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
		{
			if (this.m_Container != null && this.m_Container.Spline == spline)
			{
				this.m_SplineDirty = this.m_AutoRefresh;
			}
		}

		// Token: 0x0400009C RID: 156
		[SerializeField]
		private SplineContainer m_Container;

		// Token: 0x0400009D RID: 157
		[SerializeField]
		private List<SplineInstantiate.InstantiableItem> m_ItemsToInstantiate = new List<SplineInstantiate.InstantiableItem>();

		// Token: 0x0400009E RID: 158
		[SerializeField]
		private SplineInstantiate.Method m_Method = SplineInstantiate.Method.SpacingDistance;

		// Token: 0x0400009F RID: 159
		[SerializeField]
		private SplineInstantiate.Space m_Space;

		// Token: 0x040000A0 RID: 160
		[SerializeField]
		private Vector2 m_Spacing = new Vector2(1f, 1f);

		// Token: 0x040000A1 RID: 161
		[SerializeField]
		private SplineComponent.AlignAxis m_Up = SplineComponent.AlignAxis.YAxis;

		// Token: 0x040000A2 RID: 162
		[SerializeField]
		private SplineComponent.AlignAxis m_Forward = SplineComponent.AlignAxis.ZAxis;

		// Token: 0x040000A3 RID: 163
		[SerializeField]
		private SplineInstantiate.Vector3Offset m_PositionOffset;

		// Token: 0x040000A4 RID: 164
		[SerializeField]
		private SplineInstantiate.Vector3Offset m_RotationOffset;

		// Token: 0x040000A5 RID: 165
		[SerializeField]
		private SplineInstantiate.Vector3Offset m_ScaleOffset;

		// Token: 0x040000A6 RID: 166
		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("m_Instances")]
		private List<GameObject> m_DeprecatedInstances = new List<GameObject>();

		// Token: 0x040000A7 RID: 167
		private const string k_InstancesRootName = "root-";

		// Token: 0x040000A8 RID: 168
		private GameObject m_InstancesRoot;

		// Token: 0x040000A9 RID: 169
		private readonly List<GameObject> m_Instances = new List<GameObject>();

		// Token: 0x040000AA RID: 170
		private bool m_InstancesCacheDirty;

		// Token: 0x040000AB RID: 171
		[SerializeField]
		private bool m_AutoRefresh = true;

		// Token: 0x040000AC RID: 172
		private SplineInstantiate.InstantiableItem m_CurrentItem;

		// Token: 0x040000AD RID: 173
		private bool m_SplineDirty;

		// Token: 0x040000AE RID: 174
		private float m_MaxProbability = 1f;

		// Token: 0x040000AF RID: 175
		[SerializeField]
		private int m_Seed;

		// Token: 0x040000B0 RID: 176
		private List<float> m_TimesCache = new List<float>();

		// Token: 0x040000B1 RID: 177
		private List<float> m_LengthsCache = new List<float>();

		// Token: 0x02000064 RID: 100
		public enum OffsetSpace
		{
			// Token: 0x0400014A RID: 330
			[InspectorName("Spline Element")]
			Spline,
			// Token: 0x0400014B RID: 331
			[InspectorName("Spline Object")]
			Local,
			// Token: 0x0400014C RID: 332
			[InspectorName("World Space")]
			World,
			// Token: 0x0400014D RID: 333
			[InspectorName("Instantiated Object")]
			Object
		}

		// Token: 0x02000065 RID: 101
		[Serializable]
		internal struct Vector3Offset
		{
			// Token: 0x170000BF RID: 191
			// (get) Token: 0x06000364 RID: 868 RVA: 0x00010B5A File Offset: 0x0000ED5A
			public bool hasOffset
			{
				get
				{
					return (this.setup & SplineInstantiate.Vector3Offset.Setup.HasOffset) > SplineInstantiate.Vector3Offset.Setup.None;
				}
			}

			// Token: 0x170000C0 RID: 192
			// (get) Token: 0x06000365 RID: 869 RVA: 0x00010B67 File Offset: 0x0000ED67
			public bool hasCustomSpace
			{
				get
				{
					return (this.setup & SplineInstantiate.Vector3Offset.Setup.HasCustomSpace) > SplineInstantiate.Vector3Offset.Setup.None;
				}
			}

			// Token: 0x06000366 RID: 870 RVA: 0x00010B74 File Offset: 0x0000ED74
			internal Vector3 GetNextOffset()
			{
				if ((this.setup & SplineInstantiate.Vector3Offset.Setup.HasOffset) != SplineInstantiate.Vector3Offset.Setup.None)
				{
					return new Vector3(this.randomX ? Random.Range(this.min.x, this.max.x) : this.min.x, this.randomY ? Random.Range(this.min.y, this.max.y) : this.min.y, this.randomZ ? Random.Range(this.min.z, this.max.z) : this.min.z);
				}
				return Vector3.zero;
			}

			// Token: 0x06000367 RID: 871 RVA: 0x00010C2C File Offset: 0x0000EE2C
			internal void CheckMinMaxValidity()
			{
				this.max.x = Mathf.Max(this.min.x, this.max.x);
				this.max.y = Mathf.Max(this.min.y, this.max.y);
				this.max.z = Mathf.Max(this.min.z, this.max.z);
			}

			// Token: 0x06000368 RID: 872 RVA: 0x00010CAB File Offset: 0x0000EEAB
			internal void CheckMinMax()
			{
				this.CheckMinMaxValidity();
				if (this.max.magnitude > 0f)
				{
					this.setup |= SplineInstantiate.Vector3Offset.Setup.HasOffset;
					return;
				}
				this.setup &= ~SplineInstantiate.Vector3Offset.Setup.HasOffset;
			}

			// Token: 0x06000369 RID: 873 RVA: 0x00010CE3 File Offset: 0x0000EEE3
			internal void CheckCustomSpace(SplineInstantiate.Space instanceSpace)
			{
				if (this.space == (SplineInstantiate.OffsetSpace)instanceSpace)
				{
					this.setup &= ~SplineInstantiate.Vector3Offset.Setup.HasCustomSpace;
					return;
				}
				this.setup |= SplineInstantiate.Vector3Offset.Setup.HasCustomSpace;
			}

			// Token: 0x0400014E RID: 334
			public SplineInstantiate.Vector3Offset.Setup setup;

			// Token: 0x0400014F RID: 335
			public Vector3 min;

			// Token: 0x04000150 RID: 336
			public Vector3 max;

			// Token: 0x04000151 RID: 337
			public bool randomX;

			// Token: 0x04000152 RID: 338
			public bool randomY;

			// Token: 0x04000153 RID: 339
			public bool randomZ;

			// Token: 0x04000154 RID: 340
			public SplineInstantiate.OffsetSpace space;

			// Token: 0x02000075 RID: 117
			[Flags]
			public enum Setup
			{
				// Token: 0x04000181 RID: 385
				None = 0,
				// Token: 0x04000182 RID: 386
				HasOffset = 1,
				// Token: 0x04000183 RID: 387
				HasCustomSpace = 2
			}
		}

		// Token: 0x02000066 RID: 102
		[Serializable]
		public struct InstantiableItem
		{
			// Token: 0x04000155 RID: 341
			[HideInInspector]
			[Obsolete("Use Prefab instead.", false)]
			public GameObject prefab;

			// Token: 0x04000156 RID: 342
			[FormerlySerializedAs("prefab")]
			public GameObject Prefab;

			// Token: 0x04000157 RID: 343
			[HideInInspector]
			[Obsolete("Use Probability instead.", false)]
			public float probability;

			// Token: 0x04000158 RID: 344
			[FormerlySerializedAs("probability")]
			public float Probability;
		}

		// Token: 0x02000067 RID: 103
		public enum Method
		{
			// Token: 0x0400015A RID: 346
			[InspectorName("Instance Count")]
			InstanceCount,
			// Token: 0x0400015B RID: 347
			[InspectorName("Spline Distance")]
			SpacingDistance,
			// Token: 0x0400015C RID: 348
			[InspectorName("Linear Distance")]
			LinearDistance
		}

		// Token: 0x02000068 RID: 104
		public enum Space
		{
			// Token: 0x0400015E RID: 350
			[InspectorName("Spline Element")]
			Spline,
			// Token: 0x0400015F RID: 351
			[InspectorName("Spline Object")]
			Local,
			// Token: 0x04000160 RID: 352
			[InspectorName("World Space")]
			World
		}
	}
}
