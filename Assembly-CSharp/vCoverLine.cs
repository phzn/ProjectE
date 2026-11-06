using System;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class vCoverLine : MonoBehaviour
{
	// Token: 0x06000942 RID: 2370 RVA: 0x00043600 File Offset: 0x00041800
	private void Reset()
	{
		this.close = false;
		while (base.transform.childCount > 0)
		{
			Object.DestroyImmediate(base.transform.GetChild(0).gameObject);
		}
		this.CtrlPTransform = new GameObject("ControlPoints").transform;
		this.CPTransform = new GameObject("CoverPoints").transform;
		this.CtrlPTransform.parent = base.transform;
		this.CPTransform.parent = base.transform;
		this.CtrlPTransform.localPosition = Vector3.zero;
		this.CtrlPTransform.localEulerAngles = Vector3.zero;
		this.CPTransform.localPosition = Vector3.zero;
		this.CPTransform.localEulerAngles = Vector3.zero;
		vCoverLinePoint vCoverLinePoint = new GameObject("P1").AddComponent<vCoverLinePoint>();
		vCoverLinePoint vCoverLinePoint2 = new GameObject("P2").AddComponent<vCoverLinePoint>();
		vCoverLinePoint.transform.parent = this.CtrlPTransform;
		vCoverLinePoint2.transform.parent = this.CtrlPTransform;
		vCoverLinePoint.transform.localPosition = Vector3.right * -1f;
		vCoverLinePoint2.transform.localPosition = Vector3.right;
		vCoverLinePoint.curveType = vCoverLine.CurveType.Linear;
		vCoverLinePoint2.curveType = vCoverLine.CurveType.Linear;
		vCoverLinePoint.conectedRight = vCoverLinePoint2;
		vCoverLinePoint2.conectedLeft = vCoverLinePoint;
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x00043750 File Offset: 0x00041950
	public vCoverLinePoint NewPoint(Vector3 localPosition)
	{
		vCoverLinePoint vCoverLinePoint = new GameObject("P" + (this.CtrlPTransform.childCount + 1).ToString()).AddComponent<vCoverLinePoint>();
		vCoverLinePoint.transform.parent = this.CtrlPTransform;
		vCoverLinePoint.transform.localPosition = localPosition;
		vCoverLinePoint.curveType = vCoverLine.CurveType.Linear;
		return vCoverLinePoint;
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x000437AA File Offset: 0x000419AA
	public vCoverLinePoint NewPoint(Vector3 localPosition, int siblingIndex)
	{
		vCoverLinePoint vCoverLinePoint = this.NewPoint(localPosition);
		vCoverLinePoint.transform.SetSiblingIndex(siblingIndex);
		return vCoverLinePoint;
	}

	// Token: 0x0400086E RID: 2158
	public bool close;

	// Token: 0x0400086F RID: 2159
	[Tooltip("Approximate Width of Cover Points")]
	[Range(0.1f, 1f)]
	public float width = 1f;

	// Token: 0x04000870 RID: 2160
	[Range(0.5f, 2f)]
	public float height = 1f;

	// Token: 0x04000871 RID: 2161
	[Tooltip("The Z size of Cover Points")]
	[Range(0.2f, 1f)]
	public float depht = 0.5f;

	// Token: 0x04000872 RID: 2162
	[Tooltip("The Z position of Cover Points")]
	[Range(0f, 1f)]
	public float offsetZ;

	// Token: 0x04000873 RID: 2163
	[vTagSelector]
	public string coverPointTag = "CoverPoint";

	// Token: 0x04000874 RID: 2164
	[vLayerSelector]
	public string coverPointLayer = "CoverPoint";

	// Token: 0x04000875 RID: 2165
	public Transform CtrlPTransform;

	// Token: 0x04000876 RID: 2166
	public Transform CPTransform;

	// Token: 0x04000877 RID: 2167
	private int pointCount;

	// Token: 0x020000EB RID: 235
	public enum CurveType
	{
		// Token: 0x04000879 RID: 2169
		Linear,
		// Token: 0x0400087A RID: 2170
		LinearRight,
		// Token: 0x0400087B RID: 2171
		LinearLeft,
		// Token: 0x0400087C RID: 2172
		Align,
		// Token: 0x0400087D RID: 2173
		Mirror,
		// Token: 0x0400087E RID: 2174
		Free
	}
}
