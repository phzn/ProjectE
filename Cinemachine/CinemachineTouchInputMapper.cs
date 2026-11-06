using System;
using Cinemachine;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class CinemachineTouchInputMapper : MonoBehaviour
{
	// Token: 0x06000008 RID: 8 RVA: 0x000022B6 File Offset: 0x000004B6
	private void Start()
	{
		CinemachineCore.GetInputAxis = new CinemachineCore.AxisInputDelegate(this.GetInputAxis);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000022CC File Offset: 0x000004CC
	private float GetInputAxis(string axisName)
	{
		if (Input.touchCount > 0)
		{
			if (axisName == this.TouchXInputMapTo)
			{
				return Input.touches[0].deltaPosition.x / this.TouchSensitivityX;
			}
			if (axisName == this.TouchYInputMapTo)
			{
				return Input.touches[0].deltaPosition.y / this.TouchSensitivityY;
			}
		}
		return Input.GetAxis(axisName);
	}

	// Token: 0x0400000B RID: 11
	[Tooltip("Sensitivity multiplier for x-axis")]
	public float TouchSensitivityX = 10f;

	// Token: 0x0400000C RID: 12
	[Tooltip("Sensitivity multiplier for y-axis")]
	public float TouchSensitivityY = 10f;

	// Token: 0x0400000D RID: 13
	[Tooltip("Input channel to spoof for X axis")]
	public string TouchXInputMapTo = "Mouse X";

	// Token: 0x0400000E RID: 14
	[Tooltip("Input channel to spoof for Y axis")]
	public string TouchYInputMapTo = "Mouse Y";
}
