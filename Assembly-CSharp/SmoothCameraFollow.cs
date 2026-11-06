using System;
using UnityEngine;

// Token: 0x0200016F RID: 367
public class SmoothCameraFollow : MonoBehaviour
{
	// Token: 0x06000BB9 RID: 3001 RVA: 0x0005102E File Offset: 0x0004F22E
	private void Awake()
	{
		this._offset = base.transform.position - this.target.position;
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x00051054 File Offset: 0x0004F254
	private void LateUpdate()
	{
		Vector3 vector = this.target.position + this._offset;
		base.transform.position = Vector3.SmoothDamp(base.transform.position, vector, ref this._currentVelocity, this.smoothTime);
	}

	// Token: 0x04000B81 RID: 2945
	private Vector3 _offset;

	// Token: 0x04000B82 RID: 2946
	[SerializeField]
	private Transform target;

	// Token: 0x04000B83 RID: 2947
	[SerializeField]
	private float smoothTime;

	// Token: 0x04000B84 RID: 2948
	private Vector3 _currentVelocity = Vector3.zero;
}
