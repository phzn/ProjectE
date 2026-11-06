using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000141 RID: 321
public class vShooterTrainingController : MonoBehaviour
{
	// Token: 0x06000A51 RID: 2641 RVA: 0x00046CC3 File Offset: 0x00044EC3
	private void Start()
	{
		if (this.shooterScore && this.initOnStart)
		{
			this.StartTraining();
		}
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x00046CE0 File Offset: 0x00044EE0
	public void StartTraining()
	{
		if (!this.shooterScore)
		{
			return;
		}
		this.shooterScore.StartScore();
		this.FinishTraining();
		this.currentRoutine = base.StartCoroutine(this.RunTraining());
	}

	// Token: 0x06000A53 RID: 2643 RVA: 0x00046D13 File Offset: 0x00044F13
	public void CancelTraining()
	{
		if (!this.shooterScore)
		{
			return;
		}
		if (this.currentRoutine != null)
		{
			base.StopCoroutine(this.currentRoutine);
			this.currentRoutine = null;
			this.onCancelTraining.Invoke();
		}
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x00046D4C File Offset: 0x00044F4C
	public void FinishTraining()
	{
		if (!this.shooterScore)
		{
			return;
		}
		if (this.currentRoutine != null)
		{
			base.StopCoroutine(this.currentRoutine);
			this.currentRoutine = null;
			this.shooterScore.FinishScore();
		}
		this.timeDisplay.text = "";
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x00046D9D File Offset: 0x00044F9D
	private IEnumerator RunTraining()
	{
		this.onInit.Invoke();
		yield return new WaitForSeconds(1f);
		this.timeDisplay.text = "";
		float timeToEnd = Time.time + this.timeToStartTraining;
		while (timeToEnd > Time.time)
		{
			this.timeDisplay.text = (timeToEnd - Time.time).ToString("00");
			yield return null;
		}
		yield return new WaitForSeconds(0.2f);
		this.timeDisplay.text = "";
		yield return new WaitForSeconds(0.2f);
		this.timeDisplay.text = "GO!";
		yield return new WaitForSeconds(1f);
		this.timeDisplay.text = "";
		this.onStartCounter.Invoke();
		yield return new WaitForSeconds(0.2f);
		timeToEnd = Time.time + this.timeToFinishTraining;
		while (timeToEnd > Time.time)
		{
			this.timeDisplay.text = (timeToEnd - Time.time).ToString("00");
			yield return null;
		}
		this.onFinishCounter.Invoke();
		this.shooterScore.FinishScore();
		this.timeDisplay.text = "";
		yield break;
	}

	// Token: 0x04000984 RID: 2436
	public vShooterScore shooterScore;

	// Token: 0x04000985 RID: 2437
	public float timeToStartTraining = 3f;

	// Token: 0x04000986 RID: 2438
	public float timeToFinishTraining = 60f;

	// Token: 0x04000987 RID: 2439
	public Text timeDisplay;

	// Token: 0x04000988 RID: 2440
	public UnityEvent onInit;

	// Token: 0x04000989 RID: 2441
	public UnityEvent onStartCounter;

	// Token: 0x0400098A RID: 2442
	public UnityEvent onFinishCounter;

	// Token: 0x0400098B RID: 2443
	public UnityEvent onCancelTraining;

	// Token: 0x0400098C RID: 2444
	private Coroutine currentRoutine;

	// Token: 0x0400098D RID: 2445
	private float currentTime;

	// Token: 0x0400098E RID: 2446
	public bool initOnStart;
}
