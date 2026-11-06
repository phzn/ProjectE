using System;
using UnityEngine;

// Token: 0x0200011E RID: 286
public class vPingPongRotation : MonoBehaviour
{
	// Token: 0x060009CA RID: 2506 RVA: 0x0004503F File Offset: 0x0004323F
	private void Start()
	{
		if (this.targetTransform == null)
		{
			this.targetTransform = base.transform;
		}
		this.defaultLocalForward = this.targetTransform.parent.InverseTransformDirection(this.targetTransform.forward);
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x0004507C File Offset: 0x0004327C
	private void OnEnable()
	{
		this.evaluateToDirection = 0f;
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x0004507C File Offset: 0x0004327C
	public void Reset()
	{
		this.evaluateToDirection = 0f;
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x0004508C File Offset: 0x0004328C
	private void Update()
	{
		Vector3 vector = this.targetTransform.parent.TransformDirection(this.defaultLocalForward);
		if (this.angleX.magnitude > 0f)
		{
			this.pingPongTime.x = Time.time * this.speed.x;
		}
		if (this.angleY.magnitude > 0f)
		{
			this.pingPongTime.y = Time.time * this.speed.y;
		}
		if (this.angleZ.magnitude > 0f)
		{
			this.pingPongTime.z = Time.time * this.speed.z;
		}
		if (this.evaluateToDirection < 1f)
		{
			this.evaluateToDirection += Time.deltaTime * this.speed.magnitude;
		}
		else
		{
			this.evaluateToDirection = 1f;
		}
		if (this.angleX.magnitude > 0f)
		{
			this.evaluate.x = Mathf.PingPong(this.pingPongTime.x, 1f);
		}
		if (this.angleY.magnitude > 0f)
		{
			this.evaluate.y = Mathf.PingPong(this.pingPongTime.y, 1f);
		}
		if (this.angleZ.magnitude > 0f)
		{
			this.pingPongTime.z = Time.time * this.speed.z;
		}
		this.evaluate.z = Mathf.PingPong(this.pingPongTime.z, 1f);
		if (this.angleX.magnitude > 0f)
		{
			this.euler.x = Mathf.Lerp(this.angleX.x, this.angleX.y, this.evaluate.x);
		}
		if (this.angleY.magnitude > 0f)
		{
			this.euler.y = Mathf.Lerp(this.angleY.x, this.angleY.y, this.evaluate.y);
		}
		if (this.angleZ.magnitude > 0f)
		{
			this.pingPongTime.z = Time.time * this.speed.z;
		}
		this.euler.z = Mathf.Lerp(this.angleZ.x, this.angleZ.y, this.evaluate.z);
		this.targetTransform.forward = Vector3.Lerp(vector, Quaternion.Euler(this.euler) * vector, this.evaluateToDirection);
	}

	// Token: 0x04000921 RID: 2337
	[vHelpBox("This Component needs to be child of a root transform", vHelpBoxAttribute.MessageType.None)]
	[vMinMax(-180f, 180f)]
	public Vector2 angleX;

	// Token: 0x04000922 RID: 2338
	[vMinMax(-180f, 180f)]
	public Vector2 angleY;

	// Token: 0x04000923 RID: 2339
	[vMinMax(-180f, 180f)]
	public Vector2 angleZ;

	// Token: 0x04000924 RID: 2340
	public Vector3 speed = Vector3.one;

	// Token: 0x04000925 RID: 2341
	private Vector3 pingPongTime;

	// Token: 0x04000926 RID: 2342
	private Vector3 euler;

	// Token: 0x04000927 RID: 2343
	private float evaluateToDirection;

	// Token: 0x04000928 RID: 2344
	private Vector3 defaultLocalForward;

	// Token: 0x04000929 RID: 2345
	public Transform targetTransform;

	// Token: 0x0400092A RID: 2346
	private Vector3 evaluate;
}
