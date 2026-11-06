using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000216 RID: 534
	[AddComponentMenu("Modern UI Pack/Layout Group/Radial Layout Group")]
	public class RadialLayoutGroup : LayoutGroup
	{
		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x00064614 File Offset: 0x00062814
		// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x0006461C File Offset: 0x0006281C
		public RadialLayoutGroup.Direction layoutDir
		{
			get
			{
				return this.refLayoutDir;
			}
			set
			{
				base.SetProperty<RadialLayoutGroup.Direction>(ref this.refLayoutDir, value);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0006462B File Offset: 0x0006282B
		// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x00064633 File Offset: 0x00062833
		public float radiusStart
		{
			get
			{
				return this.refRadiusStart;
			}
			set
			{
				base.SetProperty<float>(ref this.refRadiusStart, value);
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x00064642 File Offset: 0x00062842
		// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x0006464A File Offset: 0x0006284A
		public float radiusDelta
		{
			get
			{
				return this.refRadiusDelta;
			}
			set
			{
				base.SetProperty<float>(ref this.refRadiusDelta, value);
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x00064659 File Offset: 0x00062859
		// (set) Token: 0x06000EF8 RID: 3832 RVA: 0x00064661 File Offset: 0x00062861
		public float radiusRange
		{
			get
			{
				return this.refRadiusRange;
			}
			set
			{
				base.SetProperty<float>(ref this.refRadiusRange, value);
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x00064670 File Offset: 0x00062870
		// (set) Token: 0x06000EFA RID: 3834 RVA: 0x00064678 File Offset: 0x00062878
		public float angleDelta
		{
			get
			{
				return this.refAngleDelta;
			}
			set
			{
				base.SetProperty<float>(ref this.refAngleDelta, value);
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x00064687 File Offset: 0x00062887
		// (set) Token: 0x06000EFC RID: 3836 RVA: 0x0006468F File Offset: 0x0006288F
		public float angleStart
		{
			get
			{
				return this.refAngleStart;
			}
			set
			{
				base.SetProperty<float>(ref this.refAngleStart, value);
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x0006469E File Offset: 0x0006289E
		// (set) Token: 0x06000EFE RID: 3838 RVA: 0x000646A6 File Offset: 0x000628A6
		public float angleCenter
		{
			get
			{
				return this.refAngleCenter;
			}
			set
			{
				base.SetProperty<float>(ref this.refAngleCenter, value);
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x000646B5 File Offset: 0x000628B5
		// (set) Token: 0x06000F00 RID: 3840 RVA: 0x000646BD File Offset: 0x000628BD
		public float angleRange
		{
			get
			{
				return this.refAngleRange;
			}
			set
			{
				base.SetProperty<float>(ref this.refAngleRange, value);
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x000646CC File Offset: 0x000628CC
		// (set) Token: 0x06000F02 RID: 3842 RVA: 0x000646D4 File Offset: 0x000628D4
		public bool childRotate
		{
			get
			{
				return this.refChildRotate;
			}
			set
			{
				base.SetProperty<bool>(ref this.refChildRotate, value);
			}
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00004F31 File Offset: 0x00003131
		public override void CalculateLayoutInputVertical()
		{
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00004F31 File Offset: 0x00003131
		public override void CalculateLayoutInputHorizontal()
		{
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x000646E3 File Offset: 0x000628E3
		public override void SetLayoutHorizontal()
		{
			this.CalculateChildrenPositions();
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x000646E3 File Offset: 0x000628E3
		public override void SetLayoutVertical()
		{
			this.CalculateChildrenPositions();
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x000646EC File Offset: 0x000628EC
		private void CalculateChildrenPositions()
		{
			this.m_Tracker.Clear();
			this.childList.Clear();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				RectTransform rectTransform = base.transform.GetChild(i) as RectTransform;
				if (rectTransform.gameObject.activeSelf)
				{
					this.ignoreList.Clear();
					rectTransform.GetComponents<ILayoutIgnorer>(this.ignoreList);
					if (this.ignoreList.Count == 0)
					{
						this.childList.Add(rectTransform);
					}
					else
					{
						for (int j = 0; j < this.ignoreList.Count; j++)
						{
							if (!this.ignoreList[j].ignoreLayout)
							{
								this.childList.Add(rectTransform);
								break;
							}
						}
						this.ignoreList.Clear();
					}
				}
			}
			this.EnsureParameters(this.childList.Count);
			for (int k = 0; k < this.childList.Count; k++)
			{
				RectTransform rectTransform2 = this.childList[k];
				float num = (float)k * this.angleDelta;
				float num2 = ((this.layoutDir == RadialLayoutGroup.Direction.Clockwise) ? (this.angleStart - num) : (this.angleStart + num));
				this.ProcessOneChild(rectTransform2, num2, this.radiusStart + (float)k * this.radiusDelta);
			}
			this.childList.Clear();
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00064841 File Offset: 0x00062A41
		private void EnsureParameters(int childCount)
		{
			this.EnsureAngleParameters(childCount);
			this.EnsureRadiusParameters(childCount);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x00064854 File Offset: 0x00062A54
		private void EnsureAngleParameters(int childCount)
		{
			int num = childCount - 1;
			switch (this.layoutDir)
			{
			case RadialLayoutGroup.Direction.Clockwise:
				if (num > 0)
				{
					this.angleDelta = this.angleRange / (float)num;
					return;
				}
				this.angleDelta = 0f;
				return;
			case RadialLayoutGroup.Direction.Counterclockwise:
				if (num > 0)
				{
					this.angleDelta = this.angleRange / (float)num;
					return;
				}
				this.angleDelta = 0f;
				return;
			case RadialLayoutGroup.Direction.Bidirectional:
				if (num > 0)
				{
					this.angleDelta = this.angleRange / (float)num;
				}
				else
				{
					this.angleDelta = 0f;
				}
				this.angleStart = this.angleCenter - this.angleRange * 0.5f;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x000648F8 File Offset: 0x00062AF8
		private void EnsureRadiusParameters(int childCount)
		{
			int num = childCount - 1;
			RadialLayoutGroup.Direction layoutDir = this.layoutDir;
			if (layoutDir != RadialLayoutGroup.Direction.Clockwise)
			{
				if (layoutDir - RadialLayoutGroup.Direction.Counterclockwise > 1)
				{
					return;
				}
				if (num > 0)
				{
					this.radiusDelta = this.radiusRange / (float)num;
					return;
				}
				this.radiusDelta = 0f;
				return;
			}
			else
			{
				if (num > 0)
				{
					this.radiusDelta = this.radiusRange / (float)num;
					return;
				}
				this.radiusDelta = 0f;
				return;
			}
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0006495C File Offset: 0x00062B5C
		private void ProcessOneChild(RectTransform child, float angle, float radius)
		{
			Vector3 vector = new Vector3(Mathf.Cos(angle * 0.017453292f), Mathf.Sin(angle * 0.017453292f), 0f);
			child.localPosition = vector * radius;
			DrivenTransformProperties drivenTransformProperties = DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.Rotation | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.PivotX | DrivenTransformProperties.PivotY;
			this.m_Tracker.Add(this, child, drivenTransformProperties);
			child.anchorMin = RadialLayoutGroup.center;
			child.anchorMax = RadialLayoutGroup.center;
			child.pivot = RadialLayoutGroup.center;
			if (this.childRotate)
			{
				child.localEulerAngles = new Vector3(0f, 0f, angle);
				return;
			}
			child.localEulerAngles = Vector3.zero;
		}

		// Token: 0x04000FA3 RID: 4003
		[SerializeField]
		private RadialLayoutGroup.Direction refLayoutDir;

		// Token: 0x04000FA4 RID: 4004
		[SerializeField]
		private float refRadiusStart = 200f;

		// Token: 0x04000FA5 RID: 4005
		[SerializeField]
		private float refRadiusDelta;

		// Token: 0x04000FA6 RID: 4006
		[SerializeField]
		private float refRadiusRange;

		// Token: 0x04000FA7 RID: 4007
		[SerializeField]
		private float refAngleDelta;

		// Token: 0x04000FA8 RID: 4008
		[SerializeField]
		private float refAngleStart;

		// Token: 0x04000FA9 RID: 4009
		[SerializeField]
		private float refAngleCenter;

		// Token: 0x04000FAA RID: 4010
		[SerializeField]
		private float refAngleRange = 200f;

		// Token: 0x04000FAB RID: 4011
		[SerializeField]
		private bool refChildRotate;

		// Token: 0x04000FAC RID: 4012
		private List<RectTransform> childList = new List<RectTransform>();

		// Token: 0x04000FAD RID: 4013
		private List<ILayoutIgnorer> ignoreList = new List<ILayoutIgnorer>();

		// Token: 0x04000FAE RID: 4014
		private static readonly Vector2 center = new Vector2(0.5f, 0.5f);

		// Token: 0x02000217 RID: 535
		public enum Direction
		{
			// Token: 0x04000FB0 RID: 4016
			Clockwise,
			// Token: 0x04000FB1 RID: 4017
			Counterclockwise,
			// Token: 0x04000FB2 RID: 4018
			Bidirectional
		}

		// Token: 0x02000218 RID: 536
		public enum ConstraintMode
		{
			// Token: 0x04000FB4 RID: 4020
			Interval,
			// Token: 0x04000FB5 RID: 4021
			Range
		}
	}
}
