using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200013A RID: 314
public class vSendScore : MonoBehaviour
{
	// Token: 0x06000A3D RID: 2621 RVA: 0x0004680C File Offset: 0x00044A0C
	public void SendScore(float value)
	{
		if (this.shooterScore == null)
		{
			this.shooterScore = Object.FindObjectOfType<vShooterScore>();
		}
		if (this.shooterScore != null)
		{
			this.shooterScore.AddScore(new vShooterScore.ScorePoint(this.displayID, value));
		}
		this.onAdd.Invoke();
	}

	// Token: 0x04000973 RID: 2419
	public int displayID;

	// Token: 0x04000974 RID: 2420
	public vShooterScore shooterScore;

	// Token: 0x04000975 RID: 2421
	public UnityEvent onAdd;
}
