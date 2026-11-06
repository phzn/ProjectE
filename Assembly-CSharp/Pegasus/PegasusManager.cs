using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pegasus
{
	// Token: 0x020001B7 RID: 439
	public class PegasusManager : MonoBehaviour
	{
		// Token: 0x06000CEB RID: 3307 RVA: 0x00058B08 File Offset: 0x00056D08
		private void Start()
		{
			this.m_currentState = PegasusConstants.FlythroughState.Stopped;
			if (this.m_target == null)
			{
				if (Camera.main == null)
				{
					Debug.LogWarning("Can not start Pegasus - no target has been assigned.");
					return;
				}
				if (this.m_displayDebug)
				{
					Debug.Log("Assigning main camera to target : " + Camera.main.name);
				}
				this.m_target = Camera.main.gameObject;
			}
			if (this.m_defaults == null)
			{
				this.SetDefaults();
			}
			this.ChangeFramerate(this.m_targetFramerateType);
			this.InitialiseFlythrough();
			if (this.m_autoStartAtRuntime)
			{
				this.StartFlythrough(false);
			}
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x00058BA8 File Offset: 0x00056DA8
		public void SetDefaults()
		{
			this.m_defaults = null;
			this.m_defaults = ScriptableObject.CreateInstance<PegasusDefaults>();
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00058BBC File Offset: 0x00056DBC
		public void InitialiseFlythrough()
		{
			if (this.m_displayDebug)
			{
				Debug.Log("Initialising flythrough...");
			}
			this.m_currentState = PegasusConstants.FlythroughState.Initialising;
			this.m_poiList.Clear();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				this.m_poiList.Add(base.transform.GetChild(i).GetComponent<PegasusPoi>());
			}
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				PegasusPoi pegasusPoi = this.m_poiList[i];
				pegasusPoi.m_segmentIndex = i;
				if (i == 0)
				{
					pegasusPoi.m_isFirstPOI = true;
				}
				else
				{
					pegasusPoi.m_isFirstPOI = false;
				}
				if (i == this.m_poiList.Count - 1)
				{
					pegasusPoi.m_isLastPOI = true;
				}
				else
				{
					pegasusPoi.m_isLastPOI = false;
				}
				if (this.m_flythroughType == PegasusConstants.FlythroughType.SingleShot)
				{
					if (pegasusPoi.m_isFirstPOI)
					{
						if (this.m_poiList.Count > 1)
						{
							pegasusPoi.m_prevPoi = this.m_poiList[1];
						}
						else
						{
							pegasusPoi.m_prevPoi = pegasusPoi;
						}
					}
					else
					{
						pegasusPoi.m_prevPoi = this.m_poiList[i - 1];
					}
					if (pegasusPoi.m_isLastPOI)
					{
						if (this.m_poiList.Count > 1)
						{
							pegasusPoi.m_nextPoi = this.m_poiList[i - 1];
						}
						else
						{
							pegasusPoi.m_nextPoi = pegasusPoi;
						}
					}
					else
					{
						pegasusPoi.m_nextPoi = this.m_poiList[i + 1];
					}
				}
				else
				{
					if (i == 0)
					{
						pegasusPoi.m_prevPoi = this.m_poiList[this.m_poiList.Count - 1];
					}
					else
					{
						pegasusPoi.m_prevPoi = this.m_poiList[i - 1];
					}
					if (i == this.m_poiList.Count - 1)
					{
						pegasusPoi.m_nextPoi = this.m_poiList[0];
					}
					else
					{
						pegasusPoi.m_nextPoi = this.m_poiList[i + 1];
					}
				}
				pegasusPoi.m_alwaysShowGizmos = this.m_alwaysShowGizmos;
			}
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				this.m_poiList[i].Initialise(false);
			}
			this.m_totalDuration = TimeSpan.Zero;
			this.m_totalDistanceTravelledPct = 0f;
			this.m_totalDistanceTravelled = 0f;
			this.m_totalDistance = 0f;
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				this.m_poiList[i].Initialise(true);
				this.m_poiList[i].m_segmentStartTime = new TimeSpan(this.m_totalDuration.Ticks);
				this.m_totalDistance += this.m_poiList[i].m_segmentDistance;
				this.m_totalDuration += this.m_poiList[i].m_segmentDuration;
			}
			this.m_currentSegmentIdx = 0;
			if (this.m_poiList.Count > 0)
			{
				this.m_currentSegment = this.m_poiList[this.m_currentSegmentIdx];
			}
			else
			{
				this.m_currentSegment = null;
			}
			this.m_currentSegmentDistanceTravelled = 0f;
			this.m_lastUpdateTime = DateTime.Now;
			this.m_canUpdateNow = true;
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x00058ED4 File Offset: 0x000570D4
		private void RestartFlythrough()
		{
			if (this.m_displayDebug)
			{
				Debug.Log("Restarting flythrough...");
			}
			this.m_currentState = PegasusConstants.FlythroughState.Initialising;
			this.m_totalDistanceTravelledPct = 0f;
			this.m_totalDistanceTravelled = 0f;
			this.m_currentSegmentIdx = 0;
			if (this.m_poiList.Count > 0)
			{
				this.m_currentSegment = this.m_poiList[this.m_currentSegmentIdx];
			}
			else
			{
				this.m_currentSegment = null;
			}
			this.m_currentSegmentDistanceTravelled = 0f;
			this.m_lastUpdateTime = DateTime.Now;
			this.m_canUpdateNow = true;
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00058F64 File Offset: 0x00057164
		public void UpdateFlythroughMetaData()
		{
			this.m_totalDuration = TimeSpan.Zero;
			this.m_totalDistance = 0f;
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				this.m_poiList[i].m_segmentStartTime = new TimeSpan(this.m_totalDuration.Ticks);
				this.m_totalDuration += this.m_poiList[i].m_segmentDuration;
				this.m_totalDistance += this.m_poiList[i].m_segmentDistance;
			}
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x00059000 File Offset: 0x00057200
		public void UpdateSegmentWithDependencies(PegasusPoi segment)
		{
			if (segment == null)
			{
				Debug.LogError("Attempting to update null segment!");
				return;
			}
			if (this.m_flythroughType == PegasusConstants.FlythroughType.SingleShot)
			{
				int segmentIndex = segment.m_segmentIndex;
				this.SafeInitialise(segmentIndex - 2, false, true);
				this.SafeInitialise(segmentIndex - 1, false, true);
				this.SafeInitialise(segmentIndex, false, true);
				this.SafeInitialise(segmentIndex + 1, false, true);
			}
			else
			{
				int segmentIndex2 = segment.m_segmentIndex;
				this.SafeInitialise(segmentIndex2 - 2, true, true);
				this.SafeInitialise(segmentIndex2 - 1, true, true);
				this.SafeInitialise(segmentIndex2, true, true);
				this.SafeInitialise(segmentIndex2 + 1, true, true);
			}
			this.UpdateFlythroughMetaData();
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00059094 File Offset: 0x00057294
		private void SafeInitialise(int idx, bool wrap, bool updateSegments)
		{
			if (!wrap)
			{
				if (idx >= 0 && idx < this.m_poiList.Count)
				{
					this.m_poiList[idx].Initialise(updateSegments);
					return;
				}
			}
			else
			{
				idx %= this.m_poiList.Count;
				if (idx < 0)
				{
					idx += this.m_poiList.Count;
				}
				this.m_poiList[idx].Initialise(updateSegments);
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00059100 File Offset: 0x00057300
		public void StartFlythrough(bool fullInitialise = false)
		{
			Application.runInBackground = true;
			if (fullInitialise)
			{
				this.InitialiseFlythrough();
			}
			else
			{
				this.RestartFlythrough();
			}
			if (this.m_displayDebug)
			{
				Debug.Log("Starting flythrough..");
			}
			if (this.m_target != null)
			{
				this.m_currentSegment.CalculateProgress(0f, out this.m_currentVelocity, out this.m_currentPosition, out this.m_currentRotation);
				this.m_target.transform.rotation = this.m_currentRotation;
				this.m_target.transform.position = this.m_currentPosition;
				this.m_currentSegment.OnStartTriggers();
				this.m_currentState = PegasusConstants.FlythroughState.Started;
				return;
			}
			Debug.LogWarning("Cannot start Pegasus - no target has been assigned!");
			this.m_currentState = PegasusConstants.FlythroughState.Stopped;
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x000591B8 File Offset: 0x000573B8
		public void ResumeFlythrough()
		{
			if (this.m_displayDebug)
			{
				Debug.Log("Resuming flythrough");
			}
			if (this.m_currentState != PegasusConstants.FlythroughState.Paused)
			{
				Debug.LogWarning("Can not resume flythrough - it was not paused.");
				return;
			}
			this.m_currentState = PegasusConstants.FlythroughState.Started;
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x000591E7 File Offset: 0x000573E7
		public void PauseFlythrough()
		{
			if (this.m_displayDebug)
			{
				Debug.Log("Pausing flythrough");
			}
			this.m_currentState = PegasusConstants.FlythroughState.Paused;
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x00059202 File Offset: 0x00057402
		public void StopFlythrough()
		{
			if (this.m_displayDebug)
			{
				Debug.Log("Stopping flythrough");
			}
			this.m_currentState = PegasusConstants.FlythroughState.Stopped;
			this.m_canUpdateNow = false;
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00059224 File Offset: 0x00057424
		public void ChangeFramerate(PegasusConstants.TargetFrameRate newRate)
		{
			this.m_targetFramerateType = newRate;
			if (this.m_targetFramerateType == PegasusConstants.TargetFrameRate.LeaveAlone)
			{
				return;
			}
			if (Application.isPlaying)
			{
				switch (this.m_targetFramerateType)
				{
				case PegasusConstants.TargetFrameRate.NineFps:
					Application.targetFrameRate = 9;
					this.m_frameUpdateTime = 0.11111111f;
					return;
				case PegasusConstants.TargetFrameRate.FifteenFps:
					Application.targetFrameRate = 15;
					this.m_frameUpdateTime = 0.06666667f;
					return;
				case PegasusConstants.TargetFrameRate.TwentyFourFps:
					Application.targetFrameRate = 24;
					this.m_frameUpdateTime = 0.041666668f;
					return;
				case PegasusConstants.TargetFrameRate.TwentyFiveFps:
					Application.targetFrameRate = 25;
					this.m_frameUpdateTime = 0.04f;
					return;
				case PegasusConstants.TargetFrameRate.ThirtyFps:
					Application.targetFrameRate = 30;
					this.m_frameUpdateTime = 0.033333335f;
					return;
				case PegasusConstants.TargetFrameRate.SixtyFps:
					Application.targetFrameRate = 60;
					this.m_frameUpdateTime = 0.016666668f;
					return;
				case PegasusConstants.TargetFrameRate.NinetyFps:
					Application.targetFrameRate = 90;
					this.m_frameUpdateTime = 0.011111111f;
					return;
				case PegasusConstants.TargetFrameRate.MaxFps:
					Application.targetFrameRate = -1;
					this.m_frameUpdateTime = 0f;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x00059310 File Offset: 0x00057510
		public void SetSpeed(float speed)
		{
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				this.m_poiList[i].m_startSpeed = speed;
				this.m_poiList[i].m_startSpeedType = PegasusConstants.SpeedType.Custom;
			}
			this.InitialiseFlythrough();
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x00059360 File Offset: 0x00057560
		public void SetAutoRoll(bool autoRoll)
		{
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				this.m_poiList[i].m_autoRollOn = autoRoll;
				if (!autoRoll)
				{
					this.m_poiList[i].transform.localEulerAngles = Vector3.zero;
				}
			}
			this.InitialiseFlythrough();
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000593BC File Offset: 0x000575BC
		public void SelectPoi(PegasusPoi poi)
		{
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				this.m_poiList[i].m_isSelected = false;
			}
			if (poi != null)
			{
				poi.m_isSelected = true;
			}
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00059404 File Offset: 0x00057604
		public void MovePoi(PegasusPoi poi, Vector3 movement)
		{
			poi.transform.position = this.GetValidatedPoiPosition(poi.transform.position + movement, poi.m_heightCheckType);
			poi.GetRelativeOffsets(poi.transform.position, poi.m_lookatLocation, out poi.m_lookAtDistance, out poi.m_lookAtHeight, out poi.m_lookAtAngle);
			this.UpdateSegmentWithDependencies(poi);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0005946C File Offset: 0x0005766C
		public void MovePoiLookat(PegasusPoi poi, Vector3 movement)
		{
			poi.m_lookatType = PegasusConstants.LookatType.Target;
			Vector3 validatedLookatPosition = this.GetValidatedLookatPosition(poi.m_lookatLocation + movement, poi.m_heightCheckType);
			if (validatedLookatPosition != poi.m_lookatLocation)
			{
				poi.m_lookatLocation = validatedLookatPosition;
				poi.m_lookatType = PegasusConstants.LookatType.Target;
				poi.GetRelativeOffsets(poi.transform.position, poi.m_lookatLocation, out poi.m_lookAtDistance, out poi.m_lookAtHeight, out poi.m_lookAtAngle);
				this.UpdateSegmentWithDependencies(poi);
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x000594E5 File Offset: 0x000576E5
		public void MoveTargetNow()
		{
			this.MoveTargetTo(this.m_totalDistanceTravelled / this.m_totalDistance);
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x000594FC File Offset: 0x000576FC
		public void MoveTargetTo(float percent)
		{
			Debug.Log(string.Format("Moving to {0} percent", percent));
			if (this.m_target == null)
			{
				Debug.LogWarning("Can not move target as none has been set");
				return;
			}
			float num = percent * this.m_totalDistance;
			float num2 = 0f;
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				PegasusPoi pegasusPoi = this.m_poiList[i];
				float num3 = num2 + pegasusPoi.m_segmentDistance;
				if (num >= num2 && num <= num3)
				{
					this.m_totalDistanceTravelled = num;
					this.m_totalDistanceTravelledPct = this.m_totalDistanceTravelled / this.m_totalDistance;
					this.m_currentPosition = pegasusPoi.CalculatePositionLinear((num - num2) / pegasusPoi.m_segmentDistance);
					this.m_currentRotation = pegasusPoi.CalculateRotation((num - num2) / pegasusPoi.m_segmentDistance);
					this.m_currentVelocity = pegasusPoi.CalculateVelocity((num - num2) / pegasusPoi.m_segmentDistance);
					this.m_target.transform.position = this.m_currentPosition;
					this.m_target.transform.rotation = this.m_currentRotation;
					return;
				}
				num2 += pegasusPoi.m_segmentDistance;
			}
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00059624 File Offset: 0x00057824
		public void CalculateTargetAtDistance(float distance, out Vector3 position, out Quaternion rotation, out float velocity)
		{
			if (this.m_target == null || this.m_poiList.Count == 0)
			{
				position = Vector3.zero;
				rotation = Quaternion.identity;
				velocity = 0f;
				return;
			}
			float num;
			if (this.m_flythroughType == PegasusConstants.FlythroughType.SingleShot)
			{
				num = Mathf.Clamp(distance, 0f, this.m_totalDistance);
			}
			else
			{
				num = distance % this.m_totalDistance;
				if (num < 0f)
				{
					num += this.m_totalDistance;
				}
			}
			float num2 = 0f;
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				PegasusPoi pegasusPoi = this.m_poiList[i];
				float num3 = num2 + pegasusPoi.m_segmentDistance;
				if (num >= num2 && num <= num3)
				{
					position = pegasusPoi.CalculatePositionLinear((num - num2) / pegasusPoi.m_segmentDistance);
					rotation = pegasusPoi.CalculateRotation((num - num2) / pegasusPoi.m_segmentDistance);
					velocity = pegasusPoi.CalculateVelocity((num - num2) / pegasusPoi.m_segmentDistance);
					return;
				}
				num2 += pegasusPoi.m_segmentDistance;
			}
			position = this.m_poiList[this.m_poiList.Count - 1].transform.position;
			rotation = this.m_poiList[this.m_poiList.Count - 1].transform.localRotation;
			velocity = this.m_poiList[this.m_poiList.Count - 1].m_startSpeed;
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x000597A4 File Offset: 0x000579A4
		public void CalculateTargetAtPercent(float percent, out Vector3 position, out Quaternion rotation, out float velocity)
		{
			float num = Mathf.Clamp(percent, 0f, 100f) * this.m_totalDistance;
			this.CalculateTargetAtDistance(num, out position, out rotation, out velocity);
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x000597D4 File Offset: 0x000579D4
		public void MoveTargetToPoi(PegasusPoi targetPoi)
		{
			if (this.m_target == null)
			{
				Debug.LogWarning("Can not move target as none has been set");
				return;
			}
			this.m_totalDistanceTravelled = 0f;
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				if (this.m_poiList[i].GetInstanceID() == targetPoi.GetInstanceID())
				{
					this.m_totalDistanceTravelledPct = this.m_totalDistanceTravelled / this.m_totalDistance;
					this.m_currentPosition = targetPoi.CalculatePositionLinear(0f);
					this.m_currentRotation = targetPoi.CalculateRotation(0f);
					this.m_currentVelocity = targetPoi.CalculateVelocity(0f);
					this.m_target.transform.position = this.m_currentPosition;
					this.m_target.transform.rotation = this.m_currentRotation;
				}
				else
				{
					this.m_totalDistanceTravelled += this.m_poiList[i].m_segmentDistance;
				}
			}
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x000598CC File Offset: 0x00057ACC
		public void StepTargetBackward(float distMeters)
		{
			this.m_totalDistanceTravelled -= distMeters;
			if (this.m_totalDistanceTravelled < 0f)
			{
				this.m_totalDistanceTravelled = 0f;
			}
			this.MoveTargetTo(this.m_totalDistanceTravelled / this.m_totalDistance);
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x00059907 File Offset: 0x00057B07
		public void StepTargetForward(float distMeters)
		{
			this.m_totalDistanceTravelled += distMeters;
			if (this.m_totalDistanceTravelled > this.m_totalDistance)
			{
				this.m_totalDistanceTravelled = this.m_totalDistance;
			}
			this.MoveTargetTo(this.m_totalDistanceTravelled / this.m_totalDistance);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x00059944 File Offset: 0x00057B44
		public void CreateDebugObjects()
		{
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				PegasusPoi pegasusPoi = this.m_poiList[i];
				if (pegasusPoi.transform.childCount == 0)
				{
					GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
					Object.DestroyImmediate(gameObject.GetComponent<BoxCollider>());
					gameObject.transform.position = pegasusPoi.transform.position;
					gameObject.transform.localScale = new Vector3(0.05f, 10f, 0.05f);
					gameObject.transform.parent = pegasusPoi.transform;
					GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
					Object.DestroyImmediate(gameObject2.GetComponent<BoxCollider>());
					gameObject2.transform.position = pegasusPoi.transform.position;
					gameObject2.transform.localScale = new Vector3(0.05f, 0.05f, 5f);
					gameObject2.transform.parent = pegasusPoi.transform;
					GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
					Object.DestroyImmediate(gameObject3.GetComponent<BoxCollider>());
					gameObject3.transform.position = pegasusPoi.transform.position;
					gameObject3.transform.localScale = new Vector3(5f, 0.05f, 0.05f);
					gameObject3.transform.parent = pegasusPoi.transform;
				}
			}
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00059A8C File Offset: 0x00057C8C
		public void DeleteDebugObjects()
		{
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				PegasusPoi pegasusPoi = this.m_poiList[i];
				while (pegasusPoi.transform.childCount > 0)
				{
					Object.DestroyImmediate(pegasusPoi.transform.GetChild(0).gameObject);
				}
			}
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00059AE4 File Offset: 0x00057CE4
		public Vector3 GetValidatedPoiPosition(Vector3 source, PegasusConstants.PoiHeightCheckType heightCheckOverride = PegasusConstants.PoiHeightCheckType.ManagerSettings)
		{
			PegasusConstants.HeightCheckType heightCheckType = this.m_heightCheckType;
			if (heightCheckOverride != PegasusConstants.PoiHeightCheckType.ManagerSettings)
			{
				if (heightCheckOverride == PegasusConstants.PoiHeightCheckType.Collision)
				{
					heightCheckType = PegasusConstants.HeightCheckType.Collision;
				}
				else if (heightCheckOverride == PegasusConstants.PoiHeightCheckType.Terrain)
				{
					heightCheckType = PegasusConstants.HeightCheckType.Terrain;
				}
				else
				{
					heightCheckType = PegasusConstants.HeightCheckType.None;
				}
			}
			if (heightCheckType == PegasusConstants.HeightCheckType.None)
			{
				return source;
			}
			if (heightCheckType == PegasusConstants.HeightCheckType.Collision)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(new Vector3(source.x, source.y + this.m_collisionHeightOffset, source.z), Vector3.down, out raycastHit, 10000f) && raycastHit.point.y + this.m_minHeightAboveTerrain > source.y)
				{
					source.y = raycastHit.point.y + this.m_minHeightAboveTerrain;
				}
				return source;
			}
			Terrain terrain = this.GetTerrain(source);
			if (terrain != null)
			{
				float num = terrain.SampleHeight(source);
				if (num + this.m_minHeightAboveTerrain > source.y)
				{
					source.y = num + this.m_minHeightAboveTerrain;
				}
			}
			return source;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00059BB8 File Offset: 0x00057DB8
		public Vector3 GetLowestPoiPosition(Vector3 source, PegasusConstants.PoiHeightCheckType heightCheckOverride = PegasusConstants.PoiHeightCheckType.ManagerSettings)
		{
			PegasusConstants.HeightCheckType heightCheckType = this.m_heightCheckType;
			if (heightCheckOverride != PegasusConstants.PoiHeightCheckType.ManagerSettings)
			{
				if (heightCheckOverride == PegasusConstants.PoiHeightCheckType.Collision)
				{
					heightCheckType = PegasusConstants.HeightCheckType.Collision;
				}
				else if (heightCheckOverride == PegasusConstants.PoiHeightCheckType.Terrain)
				{
					heightCheckType = PegasusConstants.HeightCheckType.Terrain;
				}
				else
				{
					heightCheckType = PegasusConstants.HeightCheckType.None;
				}
			}
			if (heightCheckType == PegasusConstants.HeightCheckType.None)
			{
				return source;
			}
			if (heightCheckType == PegasusConstants.HeightCheckType.Collision)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(new Vector3(source.x, source.y + this.m_collisionHeightOffset, source.z), Vector3.down, out raycastHit, 10000f))
				{
					source.y = raycastHit.point.y + this.m_minHeightAboveTerrain;
				}
				return source;
			}
			Terrain terrain = this.GetTerrain(source);
			if (terrain != null)
			{
				source.y = terrain.SampleHeight(source) + this.m_minHeightAboveTerrain;
			}
			return source;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00059C60 File Offset: 0x00057E60
		public Vector3 GetValidatedLookatPosition(Vector3 source, PegasusConstants.PoiHeightCheckType heightCheckOverride = PegasusConstants.PoiHeightCheckType.ManagerSettings)
		{
			PegasusConstants.HeightCheckType heightCheckType = this.m_heightCheckType;
			if (heightCheckOverride != PegasusConstants.PoiHeightCheckType.ManagerSettings)
			{
				if (heightCheckOverride == PegasusConstants.PoiHeightCheckType.Collision)
				{
					heightCheckType = PegasusConstants.HeightCheckType.Collision;
				}
				else if (heightCheckOverride == PegasusConstants.PoiHeightCheckType.Terrain)
				{
					heightCheckType = PegasusConstants.HeightCheckType.Terrain;
				}
				else
				{
					heightCheckType = PegasusConstants.HeightCheckType.None;
				}
			}
			if (heightCheckType == PegasusConstants.HeightCheckType.None)
			{
				return source;
			}
			if (heightCheckType == PegasusConstants.HeightCheckType.Collision)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(new Vector3(source.x, source.y + this.m_collisionHeightOffset, source.z), Vector3.down, out raycastHit, 2000f) && raycastHit.point.y > source.y)
				{
					source.y = raycastHit.point.y;
				}
				return source;
			}
			Terrain terrain = this.GetTerrain(source);
			if (terrain != null)
			{
				float num = terrain.SampleHeight(source);
				if (num > source.y)
				{
					source.y = num;
				}
			}
			return source;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00059D18 File Offset: 0x00057F18
		public Vector3 GetLowestLookatPosition(Vector3 source, PegasusConstants.PoiHeightCheckType heightCheckOverride = PegasusConstants.PoiHeightCheckType.ManagerSettings)
		{
			PegasusConstants.HeightCheckType heightCheckType = this.m_heightCheckType;
			if (heightCheckOverride != PegasusConstants.PoiHeightCheckType.ManagerSettings)
			{
				if (heightCheckOverride == PegasusConstants.PoiHeightCheckType.Collision)
				{
					heightCheckType = PegasusConstants.HeightCheckType.Collision;
				}
				else if (heightCheckOverride == PegasusConstants.PoiHeightCheckType.Terrain)
				{
					heightCheckType = PegasusConstants.HeightCheckType.Terrain;
				}
				else
				{
					heightCheckType = PegasusConstants.HeightCheckType.None;
				}
			}
			if (heightCheckType == PegasusConstants.HeightCheckType.None)
			{
				return source;
			}
			if (heightCheckType == PegasusConstants.HeightCheckType.Collision)
			{
				RaycastHit raycastHit;
				if (Physics.Raycast(new Vector3(source.x, source.y + this.m_collisionHeightOffset, source.z), Vector3.down, out raycastHit, 2000f))
				{
					source.y = raycastHit.point.y;
				}
				return source;
			}
			Terrain terrain = this.GetTerrain(source);
			if (terrain != null)
			{
				source.y = terrain.SampleHeight(source);
			}
			return source;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00059DB0 File Offset: 0x00057FB0
		public float GetValidatedLookatHeightRelativeToMinimum(Vector3 source, PegasusConstants.PoiHeightCheckType heightCheckOverride = PegasusConstants.PoiHeightCheckType.ManagerSettings)
		{
			Vector3 lowestLookatPosition = this.GetLowestLookatPosition(source, heightCheckOverride);
			return source.y - lowestLookatPosition.y;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00059DD4 File Offset: 0x00057FD4
		public Terrain GetTerrain(Vector3 location)
		{
			Vector3 vector = default(Vector3);
			Vector3 vector2 = default(Vector3);
			Terrain terrain = Terrain.activeTerrain;
			if (terrain != null)
			{
				vector = terrain.GetPosition();
				vector2 = vector + terrain.terrainData.size;
				if (location.x >= vector.x && location.x <= vector2.x && location.z >= vector.z && location.z <= vector2.z)
				{
					return terrain;
				}
			}
			for (int i = 0; i < Terrain.activeTerrains.Length; i++)
			{
				terrain = Terrain.activeTerrains[i];
				vector = terrain.GetPosition();
				vector2 = vector + terrain.terrainData.size;
				if (location.x >= vector.x && location.x <= vector2.x && location.z >= vector.z && location.z <= vector2.z)
				{
					return terrain;
				}
			}
			return null;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00059EC4 File Offset: 0x000580C4
		private void LateUpdate()
		{
			if (this.m_currentState == PegasusConstants.FlythroughState.Paused)
			{
				this.m_lastUpdateTime = DateTime.Now;
			}
			if (this.m_currentState != PegasusConstants.FlythroughState.Started)
			{
				return;
			}
			this.CalculateFlythroughUpdates();
			if (this.m_canUpdateNow && this.m_target != null)
			{
				if (this.m_rotationDamping > 0f)
				{
					this.m_target.transform.rotation = Quaternion.Slerp(this.m_target.transform.rotation, this.m_currentRotation, Time.deltaTime * (1f / this.m_rotationDamping));
				}
				else
				{
					this.m_target.transform.rotation = this.m_currentRotation;
				}
				if (this.m_positionDamping > 0f && Vector3.Distance(this.m_target.transform.position, this.m_currentPosition) > 0.01f)
				{
					this.m_target.transform.position = Vector3.Slerp(this.m_target.transform.position, this.m_currentPosition, Time.deltaTime * (1f / this.m_positionDamping));
				}
				else
				{
					this.m_target.transform.position = this.m_currentPosition;
				}
				this.m_canUpdateNow = false;
			}
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0005A000 File Offset: 0x00058200
		private void CalculateFlythroughUpdates()
		{
			if (this.m_currentSegment != null)
			{
				if (this.m_currentSegment.m_segmentDistance > 0f)
				{
					this.m_currentSegment.CalculateProgress(this.m_currentSegmentDistanceTravelled / this.m_currentSegment.m_segmentDistance, out this.m_currentVelocity, out this.m_currentPosition, out this.m_currentRotation);
					if (this.m_targetFramerateType == PegasusConstants.TargetFrameRate.MaxFps || this.m_targetFramerateType == PegasusConstants.TargetFrameRate.LeaveAlone)
					{
						this.m_frameUpdateTime = (float)(DateTime.Now - this.m_lastUpdateTime).Milliseconds / 1000f;
						this.m_lastUpdateTime = DateTime.Now;
					}
					this.m_frameUpdateDistance = this.m_frameUpdateTime * this.m_currentVelocity;
					this.m_currentSegmentDistanceTravelled += this.m_frameUpdateDistance;
					this.m_totalDistanceTravelled += this.m_frameUpdateDistance;
					this.m_totalDistanceTravelledPct = this.m_totalDistanceTravelled / this.m_totalDistance;
				}
				if (this.m_currentSegmentDistanceTravelled >= this.m_currentSegment.m_segmentDistance || this.m_currentSegment.m_segmentDistance == 0f)
				{
					this.m_currentSegment.OnEndTriggers();
					this.m_currentSegmentIdx++;
					if (this.m_currentSegmentIdx >= this.m_poiList.Count)
					{
						if (this.m_flythroughType == PegasusConstants.FlythroughType.Looped)
						{
							this.m_currentSegmentIdx = 0;
							this.m_currentSegmentDistanceTravelled -= this.m_currentSegment.m_segmentDistance;
							this.m_totalDistanceTravelled = this.m_currentSegmentDistanceTravelled;
						}
						else
						{
							this.m_currentSegmentIdx--;
							this.m_currentSegmentDistanceTravelled = this.m_currentSegment.m_segmentDistance;
							this.m_totalDistanceTravelled = this.m_totalDistance;
							this.m_totalDistanceTravelledPct = 1f;
							this.m_currentPosition = this.m_poiList[this.m_currentSegmentIdx].transform.position;
							if (this.m_target.transform.position == this.m_currentPosition)
							{
								if (this.m_flythroughEndAction == PegasusConstants.FlythroughEndAction.StopFlythrough)
								{
									this.StopFlythrough();
									return;
								}
								if (this.m_flythroughEndAction == PegasusConstants.FlythroughEndAction.QuitApplication)
								{
									this.StopFlythrough();
									Application.Quit();
									return;
								}
								this.StopFlythrough();
								if (this.m_nextPegasus != null)
								{
									this.m_nextPegasus.StartFlythrough(false);
									return;
								}
								Debug.Log("Next Pegasus has not been configured. Can not start.");
								return;
							}
						}
					}
					else
					{
						this.m_currentSegmentDistanceTravelled -= this.m_currentSegment.m_segmentDistance;
					}
					this.m_totalDistanceTravelledPct = this.m_totalDistanceTravelled / this.m_totalDistance;
					this.m_currentSegment = this.m_poiList[this.m_currentSegmentIdx];
					this.m_currentSegment.OnStartTriggers();
					if (this.m_currentState != PegasusConstants.FlythroughState.Started)
					{
						this.m_canUpdateNow = false;
						return;
					}
				}
				this.m_currentSegment.OnUpdateTriggers(this.m_currentSegmentDistanceTravelled / this.m_currentSegment.m_segmentDistance);
				this.m_canUpdateNow = true;
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0005A2C8 File Offset: 0x000584C8
		public void AddPOI(Vector3 targetLocation, Vector3 lookatLocation)
		{
			PegasusPoi pegasusPoi = new GameObject("POI " + this.m_poiList.Count.ToString())
			{
				transform = 
				{
					parent = base.transform,
					position = targetLocation
				}
			}.AddComponent<PegasusPoi>();
			pegasusPoi.m_manager = this;
			pegasusPoi.m_lookatLocation = lookatLocation;
			if (targetLocation != lookatLocation)
			{
				pegasusPoi.m_lookatType = PegasusConstants.LookatType.Target;
			}
			pegasusPoi.m_startSpeedType = this.m_defaults.m_flyThroughSpeed;
			this.m_poiList.Add(pegasusPoi);
			this.InitialiseFlythrough();
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0005A35C File Offset: 0x0005855C
		public PegasusPoi AddPoiAfter(PegasusPoi currentPoi)
		{
			GameObject gameObject = new GameObject("POI " + this.m_poiList.Count.ToString());
			PegasusPoi pegasusPoi = gameObject.AddComponent<PegasusPoi>();
			pegasusPoi.m_manager = this;
			pegasusPoi.m_startSpeedType = this.m_defaults.m_flyThroughSpeed;
			this.m_poiList.Insert(this.m_poiList.IndexOf(currentPoi) + 1, pegasusPoi);
			Vector3 validatedPoiPosition = this.GetValidatedPoiPosition(currentPoi.CalculatePositionLinear(0.5f), pegasusPoi.m_heightCheckType);
			gameObject.transform.position = validatedPoiPosition;
			gameObject.transform.parent = base.transform;
			gameObject.transform.SetSiblingIndex(currentPoi.m_segmentIndex + 1);
			this.InitialiseFlythrough();
			return pegasusPoi;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0005A412 File Offset: 0x00058612
		public PegasusPoi AddPoiBefore(PegasusPoi currentPoi)
		{
			return this.AddPoiAfter(this.GetPrevPOI(currentPoi, true));
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x0005A422 File Offset: 0x00058622
		public PegasusPoi GetFirstPOI()
		{
			if (this.m_poiList.Count < 1)
			{
				return null;
			}
			return this.m_poiList[0];
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0005A440 File Offset: 0x00058640
		public PegasusPoi GetPOI(int poiIndex)
		{
			if (this.m_poiList.Count == 0 || poiIndex < 0 || poiIndex >= this.m_poiList.Count)
			{
				return null;
			}
			return this.m_poiList[poiIndex];
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0005A470 File Offset: 0x00058670
		public PegasusPoi GetPrevPOI(PegasusPoi currentPoi, bool wrap = true)
		{
			if (!(currentPoi != null))
			{
				return null;
			}
			if (currentPoi.m_segmentIndex > 0)
			{
				return this.m_poiList[currentPoi.m_segmentIndex - 1];
			}
			if (wrap)
			{
				return this.m_poiList[this.m_poiList.Count - 1];
			}
			return null;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0005A4C4 File Offset: 0x000586C4
		public PegasusPoi GetNextPOI(PegasusPoi currentPoi, bool wrap = true)
		{
			if (!(currentPoi != null))
			{
				return null;
			}
			if (currentPoi.m_segmentIndex < this.m_poiList.Count - 1)
			{
				return this.m_poiList[currentPoi.m_segmentIndex + 1];
			}
			if (wrap)
			{
				return this.m_poiList[0];
			}
			return null;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0005A518 File Offset: 0x00058718
		public void SetPoiToMinHeight()
		{
			for (int i = 0; i < this.m_poiList.Count; i++)
			{
				PegasusPoi pegasusPoi = this.m_poiList[i];
				pegasusPoi.transform.position = pegasusPoi.m_manager.GetLowestPoiPosition(pegasusPoi.transform.position, pegasusPoi.m_heightCheckType);
			}
			this.InitialiseFlythrough();
		}

		// Token: 0x04000D4C RID: 3404
		public GameObject m_target;

		// Token: 0x04000D4D RID: 3405
		public PegasusConstants.FlythroughType m_flythroughType = PegasusConstants.FlythroughType.Looped;

		// Token: 0x04000D4E RID: 3406
		public PegasusConstants.FlythroughEndAction m_flythroughEndAction;

		// Token: 0x04000D4F RID: 3407
		public PegasusConstants.TargetFrameRate m_targetFramerateType = PegasusConstants.TargetFrameRate.LeaveAlone;

		// Token: 0x04000D50 RID: 3408
		public PegasusConstants.HeightCheckType m_heightCheckType = PegasusConstants.HeightCheckType.Terrain;

		// Token: 0x04000D51 RID: 3409
		public bool m_autoStartAtRuntime = true;

		// Token: 0x04000D52 RID: 3410
		public List<PegasusPoi> m_poiList = new List<PegasusPoi>();

		// Token: 0x04000D53 RID: 3411
		public float m_minHeightAboveTerrain = 1.8f;

		// Token: 0x04000D54 RID: 3412
		public bool m_displayDebug;

		// Token: 0x04000D55 RID: 3413
		public bool m_alwaysShowGizmos = true;

		// Token: 0x04000D56 RID: 3414
		public PegasusConstants.FlythroughState m_currentState;

		// Token: 0x04000D57 RID: 3415
		public int m_currentSegmentIdx;

		// Token: 0x04000D58 RID: 3416
		public PegasusPoi m_currentSegment;

		// Token: 0x04000D59 RID: 3417
		public float m_currentSegmentDistanceTravelled;

		// Token: 0x04000D5A RID: 3418
		public float m_totalDistanceTravelled;

		// Token: 0x04000D5B RID: 3419
		public float m_totalDistanceTravelledPct;

		// Token: 0x04000D5C RID: 3420
		public float m_totalDistance;

		// Token: 0x04000D5D RID: 3421
		public TimeSpan m_totalDuration = TimeSpan.Zero;

		// Token: 0x04000D5E RID: 3422
		public float m_currentVelocity;

		// Token: 0x04000D5F RID: 3423
		public Vector3 m_currentPosition = Vector3.zero;

		// Token: 0x04000D60 RID: 3424
		public Quaternion m_currentRotation = Quaternion.identity;

		// Token: 0x04000D61 RID: 3425
		public bool m_canUpdateNow;

		// Token: 0x04000D62 RID: 3426
		public DateTime m_lastUpdateTime = DateTime.MinValue;

		// Token: 0x04000D63 RID: 3427
		public float m_frameUpdateTime = 0.016666668f;

		// Token: 0x04000D64 RID: 3428
		public float m_frameUpdateDistance;

		// Token: 0x04000D65 RID: 3429
		public float m_rotationDamping = 0.75f;

		// Token: 0x04000D66 RID: 3430
		public float m_positionDamping = 0.3f;

		// Token: 0x04000D67 RID: 3431
		public PegasusManager m_nextPegasus;

		// Token: 0x04000D68 RID: 3432
		public bool m_alwaysShowPath;

		// Token: 0x04000D69 RID: 3433
		public bool m_showScrubber;

		// Token: 0x04000D6A RID: 3434
		public bool m_showPOIHelpers;

		// Token: 0x04000D6B RID: 3435
		public float m_poiGizmoSize = 0.75f;

		// Token: 0x04000D6C RID: 3436
		public bool m_showAdvanced;

		// Token: 0x04000D6D RID: 3437
		public float m_collisionHeightOffset = 1000f;

		// Token: 0x04000D6E RID: 3438
		public float m_managerSpeed = 8f;

		// Token: 0x04000D6F RID: 3439
		public PegasusDefaults m_defaults;

		// Token: 0x04000D70 RID: 3440
		public float m_autoRollMaxSpeed = 25f;

		// Token: 0x04000D71 RID: 3441
		public float m_autoRollMaxAngle = 15f;
	}
}
