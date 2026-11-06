using System;
using System.Collections.Generic;
using System.Linq;
using Invector;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020000F0 RID: 240
[vClassHeader("Detection Target", "This is a target to be detected from vDetectionController", useHelpBox = true, openClose = false)]
public class vDetectionTarget : vMonoBehaviour
{
	// Token: 0x17000208 RID: 520
	// (get) Token: 0x0600094D RID: 2381 RVA: 0x00043866 File Offset: 0x00041A66
	public Transform center
	{
		get
		{
			if (!this.centerPoint)
			{
				return base.transform;
			}
			return this.centerPoint;
		}
	}

	// Token: 0x17000209 RID: 521
	// (get) Token: 0x0600094E RID: 2382 RVA: 0x00043882 File Offset: 0x00041A82
	// (set) Token: 0x0600094F RID: 2383 RVA: 0x0004388A File Offset: 0x00041A8A
	public virtual float DetectionReductionValue
	{
		get
		{
			return this.detectionReduction;
		}
		set
		{
			this.detectionReduction = value;
		}
	}

	// Token: 0x1700020A RID: 522
	// (get) Token: 0x06000950 RID: 2384 RVA: 0x00043893 File Offset: 0x00041A93
	public virtual List<vDetectionTarget.DetectionPoint> DetectionPoints
	{
		get
		{
			return this.detectionPoints;
		}
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0004389B File Offset: 0x00041A9B
	[ContextMenu("Order Points by Value")]
	protected virtual void OrdePointByValue()
	{
		this.detectionPoints = this.detectionPoints.OrderByDescending<vDetectionTarget.DetectionPoint, float>((vDetectionTarget.DetectionPoint p) => p.value).ToList<vDetectionTarget.DetectionPoint>();
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x000438D4 File Offset: 0x00041AD4
	[ContextMenu("Find Points")]
	protected virtual void AutoFindDetectionPoints()
	{
		vIDamageReceiver[] componentsInChildren = base.GetComponentsInChildren<vIDamageReceiver>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			vIDamageReceiver p = componentsInChildren[i];
			if (!this.detectionPoints.Exists((vDetectionTarget.DetectionPoint d) => d.center.gameObject.Equals(p.gameObject)))
			{
				this.detectionPoints.Add(new vDetectionTarget.DetectionPoint
				{
					name = p.gameObject.name,
					center = p.transform,
					value = 1f,
					targetPriority = vDetectionTarget.TargetPriority.Minimal
				});
			}
		}
		if (this.detectionPoints.Count > 0)
		{
			this.detectionPoints = this.detectionPoints.FindAll((vDetectionTarget.DetectionPoint d) => d.center != null);
		}
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x000439A5 File Offset: 0x00041BA5
	protected virtual void Start()
	{
		this.healthController = base.GetComponent<vIHealthController>();
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06000954 RID: 2388 RVA: 0x000439B3 File Offset: 0x00041BB3
	public virtual bool IsValid
	{
		get
		{
			return this.healthController != null && !this.healthController.isDead;
		}
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x000439CD File Offset: 0x00041BCD
	public virtual void AddDetectionReduction(float value)
	{
		this.detectionReduction += value;
	}

	// Token: 0x04000884 RID: 2180
	public vIHealthController healthController;

	// Token: 0x04000885 RID: 2181
	[FormerlySerializedAs("center")]
	[SerializeField]
	[Tooltip("The center of Target used to detect")]
	protected Transform centerPoint;

	// Token: 0x04000886 RID: 2182
	[SerializeField]
	protected List<vDetectionTarget.DetectionPoint> detectionPoints;

	// Token: 0x04000887 RID: 2183
	[SerializeField]
	protected float detectionReduction;

	// Token: 0x020000F1 RID: 241
	public enum TargetPriority
	{
		// Token: 0x04000889 RID: 2185
		None,
		// Token: 0x0400088A RID: 2186
		Minimal,
		// Token: 0x0400088B RID: 2187
		Medium,
		// Token: 0x0400088C RID: 2188
		High,
		// Token: 0x0400088D RID: 2189
		Maximum
	}

	// Token: 0x020000F2 RID: 242
	[Serializable]
	public class DetectionPoint
	{
		// Token: 0x0400088E RID: 2190
		[Tooltip("The name of detection Point, this is just for list order")]
		public string name;

		// Token: 0x0400088F RID: 2191
		[Tooltip("The detection point reference, Used to check if the point is seen for someone")]
		public Transform center;

		// Token: 0x04000890 RID: 2192
		[Tooltip("The score of this point used to compare with required detection value of observer")]
		public float value;

		// Token: 0x04000891 RID: 2193
		[Tooltip("Priority value of this detection point")]
		public vDetectionTarget.TargetPriority targetPriority = vDetectionTarget.TargetPriority.Minimal;
	}
}
