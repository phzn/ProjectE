using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Invector.vCover
{
	// Token: 0x02000680 RID: 1664
	[vClassHeader("Cover Controller", true, "icon_v2", false, "")]
	public class vCoverController : vMonoBehaviour
	{
		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060026DB RID: 9947 RVA: 0x000D9E98 File Offset: 0x000D8098
		public float rotationSpeed
		{
			get
			{
				return Mathf.Lerp(this.rotationSpeedIdle, this.rotationSpeedMoving, Mathf.Clamp(this.shooterInput.cc.input.magnitude, 0f, 1f));
			}
		}

		// Token: 0x060026DC RID: 9948 RVA: 0x000D9EE0 File Offset: 0x000D80E0
		protected virtual void OnDrawGizmos()
		{
			if (this.wayPath.Count > 0)
			{
				Gizmos.DrawWireSphere(this.wayPath[this.wayPath.Count - 1], this.shooterInput.cc._capsuleCollider.radius);
			}
			if (this.inCover)
			{
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(this.helper.position, 0.1f);
				Gizmos.color = Color.blue;
				Gizmos.DrawRay(this.helper.position, this.helper.forward * 0.5f);
				Gizmos.color = Color.yellow;
				Gizmos.DrawRay(this.helper.position, this.helper.right * 0.5f);
				Gizmos.color = Color.red;
				Gizmos.DrawRay(this.helper.position, -this.helper.right * 0.5f);
				if (this.currentCoverPoint)
				{
					BoxCollider boxCollider = this.currentCoverPoint.boxCollider;
					Gizmos.color = Color.yellow * 0.2f;
					Vector3 vector = boxCollider.transform.TransformPoint(boxCollider.center);
					Vector3 size = boxCollider.size;
					Matrix4x4 matrix = Gizmos.matrix;
					size.Scale(boxCollider.transform.lossyScale);
					Gizmos.matrix = Matrix4x4.TRS(vector, boxCollider.transform.rotation, size);
					Gizmos.DrawCube(Vector3.zero, Vector3.one);
					Gizmos.color = Color.yellow * 0.5f;
					Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
					Gizmos.matrix = matrix;
				}
			}
			if ((this.inCover || this.goingToCoverPoint) && this.showActionGizmos && this.genericAction.triggerAction && this.genericAction.triggerAction._collider is BoxCollider)
			{
				BoxCollider boxCollider2 = this.genericAction.triggerAction._collider as BoxCollider;
				if (boxCollider2)
				{
					bool flag = boxCollider2.IsClosed(base.transform.position, this.checkActionMargin, this.checkActionOffset);
					Matrix4x4 matrix2 = Gizmos.matrix;
					boxCollider2.transform.TransformPoint(boxCollider2.center);
					Vector3 vector2 = boxCollider2.BoxSize();
					vector2 = new Vector3(vector2.x + this.checkActionMargin.x, vector2.y + this.checkActionMargin.y, vector2.z + this.checkActionMargin.z);
					Gizmos.matrix = Matrix4x4.TRS(boxCollider2.transform.TransformPoint(boxCollider2.center + this.checkActionOffset), boxCollider2.transform.rotation, vector2);
					Gizmos.color = (flag ? (Color.green * 0.2f) : (Color.yellow * 0.2f));
					Gizmos.DrawCube(Vector3.zero, Vector3.one);
					Gizmos.color = (flag ? (Color.green * 0.8f) : (Color.yellow * 0.8f));
					Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
					Gizmos.matrix = matrix2;
					Gizmos.color = Color.yellow * 0.2f;
					Vector3 vector3 = boxCollider2.transform.TransformPoint(boxCollider2.center);
					vector2 = boxCollider2.BoxSize();
					Gizmos.matrix = Matrix4x4.TRS(vector3, boxCollider2.transform.rotation, vector2);
					Gizmos.DrawCube(Vector3.zero, Vector3.one);
					Gizmos.color = Color.yellow * 0.5f;
					Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
					Gizmos.matrix = matrix2;
				}
			}
		}

		// Token: 0x060026DD RID: 9949 RVA: 0x000DA2A7 File Offset: 0x000D84A7
		protected IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			this.path = new NavMeshPath();
			this.shooterInput = base.GetComponent<vShooterMeleeInput>();
			this.originalWalkByDefault = this.shooterInput.cc.freeSpeed.walkByDefault;
			this.shooterInput.onFixedUpdate += this.UpdateCover;
			this.shooterInput.onUpdate += this.UpdateInput;
			vShooterMeleeInput vShooterMeleeInput = this.shooterInput;
			vShooterMeleeInput.onUpdateCheckAimPointsEvent = (vShooterMeleeInput.UpdateCheckAimPointsEvent)Delegate.Combine(vShooterMeleeInput.onUpdateCheckAimPointsEvent, new vShooterMeleeInput.UpdateCheckAimPointsEvent(this.UpdateCheckAimPoints));
			this.genericAction = base.GetComponent<vGenericAction>();
			this.helper = new GameObject("Helper").transform;
			this.helper.gameObject.hideFlags = HideFlags.HideInHierarchy;
			this.moveSpeed = new vThirdPersonMotor.vMovementSpeed
			{
				walkByDefault = true
			};
			this.lastMoveToDirectionInFree = this.shooterInput.cc.moveToDirectionInFree;
			this.animatorCoverCornerLayer = this.shooterInput.animator.GetLayerIndex("CoverCorner");
			this.animatorCoverOverBarrierLayer = this.shooterInput.animator.GetLayerIndex("AimingOverBarrier");
			this.onEnterCover.AddListener(delegate
			{
				this.shooterInput.cc.colliderRadius = this.capsuleRadiusInCover;
			});
			this.onExitCover.AddListener(delegate
			{
				this.shooterInput.cc.colliderRadius = this.shooterInput.cc.colliderRadiusDefault;
			});
			this.lastHipFireTimer = this.shooterInput.shooterManager.HipfireAimTime;
			this.lastUseHipFire = this.shooterInput.shooterManager.hipfireShot;
			yield break;
		}

		// Token: 0x060026DE RID: 9950 RVA: 0x000DA2B6 File Offset: 0x000D84B6
		protected virtual void UpdateCover()
		{
			this.CheckForHelperTransform();
			this.HandlePossibleCoverPoint();
			this.HandlePossibleCornerPoint();
			this.HandleCoverRouteLine();
			this.HandleCoverIndicator();
			this.HandleCoverMovement();
		}

		// Token: 0x060026DF RID: 9951 RVA: 0x000DA2DC File Offset: 0x000D84DC
		protected virtual void CheckForHelperTransform()
		{
			if (this.helper == null)
			{
				this.helper = new GameObject("Helper").transform;
				this.helper.gameObject.hideFlags = HideFlags.HideInHierarchy;
			}
		}

		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060026E0 RID: 9952 RVA: 0x000DA312 File Offset: 0x000D8512
		protected virtual bool isRolling
		{
			get
			{
				return this.shooterInput.cc.isRolling;
			}
		}

		// Token: 0x060026E1 RID: 9953 RVA: 0x000DA324 File Offset: 0x000D8524
		protected virtual void UpdateInput()
		{
			this.HandleEnterExitCover();
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x000DA32C File Offset: 0x000D852C
		protected virtual void HandleEnterExitCover()
		{
			if (this.isInCornerInputAction || this.goingToCornerPoint)
			{
				if (this.inCover && this.shooterInput.cc.IsAnimatorTag("ExitCover"))
				{
					this.ExitCover(true);
				}
				return;
			}
			bool flag = false;
			float num = 0f;
			bool buttonTimer = this.enterExitInput.GetButtonTimer(ref num, ref flag, 0.2f);
			if (!this.shooterInput.animator.IsInTransition(0) && this.possibleCoverPoint && !this.goingToCoverPoint && !this.shooterInput.cc.customAction && !this.isRolling && !this.shooterInput.cc.isJumping && !this.shooterInput.cc.ragdolled && !this.shooterInput.cc.isDead && (((this.enterExitInput.GetButtonDown() || this.inAutoEnterTimer) && !this.inCover) || (buttonTimer && this.inCover && this.wasInCover)))
			{
				this.timeToExit = Time.time + 0.5f;
				this.currentCoverRoutine = (((this.autoEnterCover && !this.inCover) || !this.autoTravelToNextCover) ? base.StartCoroutine(this.EnterCoverPointRoutine(this.possibleCoverPoint)) : base.StartCoroutine(this.GoToCoverPointRoutine(this.possibleCoverPoint, this.wayPath.vCopy<Vector3>())));
				this.possibleCoverPoint = null;
			}
			bool flag2 = this.inCover && this.wasInCover && flag && this.autoTravelToNextCover && !this.goingToCornerPoint && num > 0f && this.timeToExit < Time.time;
			bool flag3 = !this.inCover && (!this.autoEnterCover || this.wasInCover) && num == 0f && this.autoTravelToNextCover && !this.enterExitInput.GetButton() && this.goingToCoverPoint;
			bool flag4 = this.inCover && this.wasInCover && !this.goingToCornerPoint && !this.shooterInput.animator.IsInTransition(0) && (this.shooterInput.cc.customAction || this.isRolling || this.shooterInput.cc.isJumping || this.shooterInput.cc.ragdolled || this.shooterInput.cc.isDead);
			bool flag5 = this.inCover && !this.autoTravelToNextCover && !this.goingToCornerPoint && !this.shooterInput.animator.IsInTransition(0) && num > 0f && flag;
			bool flag6 = this.shooterInput.cc.IsAnimatorTag("ExitCover");
			if ((flag2 || flag3 || flag4 || flag5 || flag6) && (this.inCover || (this.goingToCoverPoint && (this.remainingDistanceToNextCover > this.minDistanceToCancelTravel || flag6))))
			{
				if (this.debug)
				{
					Debug.Log(string.Format("EXIT Condition   A :{0}    B:{1}  / C:{2}  / D:{3} / E:{4}", new object[]
					{
						flag2.ToStringColor(),
						flag3.ToStringColor(),
						flag4.ToStringColor(),
						flag5.ToStringColor(),
						this.remainingDistanceToNextCover
					}));
				}
				if (this.goingToCoverPoint)
				{
					this.onFinishGoToCoverPoint.Invoke();
				}
				if (this.inCover)
				{
					this.onExitCover.Invoke();
				}
				this.ExitCover(flag6);
			}
			if ((this.inCover || this.wasInCover) && this.ExitByActions())
			{
				this.ExitCover(false);
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060026E3 RID: 9955 RVA: 0x000DA6E0 File Offset: 0x000D88E0
		protected virtual bool inAutoEnterTimer
		{
			get
			{
				if (!this.autoEnterCover)
				{
					return false;
				}
				if (this.timeToEnterByDirection >= this.enterDirectionTimer)
				{
					return true;
				}
				if (this.possibleCoverPoint)
				{
					this.timeToEnterByDirection += Time.deltaTime;
				}
				else
				{
					this.timeToEnterByDirection = 0f;
				}
				return false;
			}
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x000DA734 File Offset: 0x000D8934
		protected virtual void HandlePossibleCoverPoint()
		{
			if (this.goingToCoverPoint)
			{
				return;
			}
			if (this.autoEnterCover && !this.inCover && !this.wasInCover)
			{
				vCoverPoint vCoverPoint = null;
				if (this.shooterInput.cc.inputSmooth.magnitude > 0.5f && this.EnterCoverConditions())
				{
					vCoverPoint nearCoverPoint = this.GetNearCoverPoint();
					if (nearCoverPoint)
					{
						this.ControlInputDirectionInCover();
						Vector3 vector = this.inputDirection;
						vector.y = 0f;
						if ((double)vector.magnitude > 0.5 && Mathf.Abs((-nearCoverPoint.transform.forward).AngleFormOtherDirection(vector.normalized).y) < this.enterInputDirectionAngle)
						{
							vCoverPoint = nearCoverPoint;
						}
					}
				}
				this.possibleCoverPoint = vCoverPoint;
				return;
			}
			if (this.inCover && !this.autoTravelToNextCover)
			{
				return;
			}
			if (!this.inCover && this.autoEnterCover)
			{
				return;
			}
			if (!this.inCover && !this.autoTravelToNextCover)
			{
				vCoverPoint nearCoverPoint2 = this.GetNearCoverPoint();
				if (nearCoverPoint2)
				{
					this.coverIndicatorPosition = nearCoverPoint2.posePosition;
				}
				if (nearCoverPoint2 != this.possibleCoverPoint)
				{
					if (this.debug && nearCoverPoint2)
					{
						Debug.Log("Select possible cover point ", nearCoverPoint2);
					}
					this.possibleCoverPoint = nearCoverPoint2;
				}
				return;
			}
			vCoverPoint coverPoint = this.GetCoverPoint();
			if (coverPoint != null && coverPoint != this.possibleCornerPoint && this.CalculatePath(coverPoint.posePosition))
			{
				this.possibleCoverPoint = coverPoint;
				this.wayPath = this.path.corners.vToList<Vector3>();
				Transform transform = this.possibleCoverPoint.transform;
				Vector3 vector2 = transform.InverseTransformPoint(this.wayPath[this.wayPath.Count - 1]);
				vector2.z = Mathf.Max(this.shooterInput.cc._capsuleCollider.radius + this.positionOffsetZ, this.shooterInput.cc._capsuleCollider.radius);
				this.wayPath[this.wayPath.Count - 1] = transform.TransformPoint(vector2);
			}
			else
			{
				this.drawRouteTimer = 0f;
				this.possibleCoverPoint = null;
				if (this.wayPath.Count > 0)
				{
					this.wayPath.Clear();
				}
			}
			this.wayPathToDraw = this.wayPath;
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000DA9A4 File Offset: 0x000D8BA4
		protected virtual vCoverPoint GetCoverPoint()
		{
			vCoverPoint vCoverPoint = null;
			Vector3 forward = this.shooterInput.cameraMain.transform.forward;
			Ray ray = new Ray(this.shooterInput.cameraMain.transform.position, this.shooterInput.cameraMain.transform.forward);
			forward.y = 0f;
			Ray ray2 = new Ray(base.transform.position + Vector3.up * this.shooterInput.cc._capsuleCollider.radius + this.shooterInput.cameraMain.transform.right * this.shooterInput.tpCamera.lerpState.right - forward * this.shooterInput.cc._capsuleCollider.radius, forward.normalized);
			Vector3 position = base.transform.position;
			if (this.inCover)
			{
				this.FindCoverPoint(ray, ref vCoverPoint);
			}
			else
			{
				this.FindCoverPoint(ray2, ref vCoverPoint);
				if (!vCoverPoint)
				{
					vCoverPoint = this.GetNearCoverPoint();
				}
			}
			return vCoverPoint;
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x000DAAD4 File Offset: 0x000D8CD4
		protected virtual bool RayCastCoverPoint(Ray ray, ref vCoverPoint coverPoint)
		{
			RaycastHit[] array = Physics.RaycastAll(ray, this.inCover ? this.nextCoverDistance.y : this.maxEnterCoverDistance, this.coverLayer);
			for (int i = 0; i < array.Length; i++)
			{
				if (this.debug)
				{
					Debug.DrawLine(ray.origin, array[i].point, Color.yellow);
				}
				vCoverPoint component = array[i].collider.gameObject.GetComponent<vCoverPoint>();
				if (!(component == null) && Mathf.Abs(ray.direction.AngleFormOtherDirection(-component.transform.forward).y) <= this.minAngleOfValidCover && !Physics.Linecast(component.boxCollider.bounds.center, ray.origin, this.obstaclesLayer) && (!this.inCover || array[i].distance >= this.nextCoverDistance.x || !this.CoverIsConnectedTo(this.currentCoverPoint, component)))
				{
					coverPoint = component;
					this.coverIndicatorPosition = array[i].point;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x000DAC0C File Offset: 0x000D8E0C
		protected virtual bool CoverIsConnectedTo(vCoverPoint currentCoverPoint, vCoverPoint coverPointToCheck)
		{
			if (currentCoverPoint == null)
			{
				return false;
			}
			bool flag = false;
			if (currentCoverPoint.Equals(coverPointToCheck))
			{
				return true;
			}
			vCoverPoint vCoverPoint = currentCoverPoint.left;
			vCoverPoint vCoverPoint2 = currentCoverPoint.right;
			int num = (int)(this.nextCoverDistance.x / currentCoverPoint.boxCollider.size.x) + 1;
			for (int i = 0; i < num; i++)
			{
				if (coverPointToCheck.Equals(vCoverPoint) || coverPointToCheck.Equals(vCoverPoint2))
				{
					flag = true;
					break;
				}
				if (vCoverPoint == null && vCoverPoint2 == null)
				{
					flag = false;
					break;
				}
				if (vCoverPoint)
				{
					vCoverPoint = vCoverPoint.left;
				}
				if (vCoverPoint2)
				{
					vCoverPoint2 = vCoverPoint2.right;
				}
			}
			return flag;
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x000DACBC File Offset: 0x000D8EBC
		protected virtual bool FindCoverPoint(Ray ray, ref vCoverPoint coverPoint)
		{
			bool flag = false;
			Vector3 direction = ray.direction;
			Vector3 vector = Quaternion.AngleAxis(90f, Vector3.up) * direction;
			vector.y = 0f;
			vector.Normalize();
			float num = (float)Mathf.Max(2 * this.rayCastAnglePass, 1) * 0.5f;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < Mathf.Max(2 * this.rayCastAnglePass, 1); i++)
			{
				if ((float)i >= num)
				{
					flag2 = true;
				}
				for (int j = 0; j < Mathf.Max(2 * this.rayCastAnglePass, 1); j++)
				{
					if ((float)j >= num)
					{
						flag3 = true;
					}
					float num2 = (flag2 ? (-(num - ((float)i - num))) : (num - (float)i));
					Vector3 vector2 = Quaternion.AngleAxis((flag3 ? (-(num - ((float)j - num))) : (num - (float)j)) * this.rayCastAngleMultiplier, Vector3.up) * Quaternion.AngleAxis(num2 * this.rayCastAngleMultiplier, vector) * direction;
					ray.direction = vector2;
					if (this.debug)
					{
						Debug.DrawLine(ray.origin, ray.GetPoint(this.inCover ? this.nextCoverDistance.y : this.maxEnterCoverDistance), Color.white * 0.5f);
					}
					if (this.RayCastCoverPoint(ray, ref coverPoint) && coverPoint)
					{
						if (this.debug)
						{
							Debug.DrawLine(ray.origin, this.coverIndicatorPosition, Color.green);
						}
						flag = true;
						break;
					}
				}
				flag3 = false;
				if (flag)
				{
					break;
				}
			}
			return flag;
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x000DAE50 File Offset: 0x000D9050
		protected virtual bool CalculatePath(Vector3 position)
		{
			return NavMesh.CalculatePath(base.transform.position, position, -1, this.path) && this.path.GetLenght() < (this.inCover ? this.nextCoverDistance.y : this.maxEnterCoverDistance) && this.path.status == NavMeshPathStatus.PathComplete;
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x000DAEB0 File Offset: 0x000D90B0
		protected virtual void HandleCoverIndicator()
		{
			if (this.coverUI)
			{
				bool flag = this.possibleCoverPoint != null && !this.goingToCornerPoint && !this.isInCornerInputAction;
				if (flag)
				{
					Vector3 vector = this.possibleCoverPoint.transform.InverseTransformPoint(this.coverIndicatorPosition);
					if (this.IsCoverCorner(this.possibleCoverPoint))
					{
						vector.x = 0f;
					}
					vector.z = 0f;
					vector.y = this.coverIndicaterHeight;
					Vector3 vector2 = this.possibleCoverPoint.transform.TransformPoint(vector);
					float num = Vector3.Distance(vector2, this.shooterInput.cameraMain.transform.position);
					this.coverUI.transform.position = vector2;
					this.coverUI.transform.forward = this.possibleCoverPoint.transform.forward;
					this.coverUI.transform.localScale = Vector3.one * Mathf.Max(1f, num / this.coverIndicatorDistance);
				}
				if (this.coverUI.activeSelf != flag)
				{
					this.coverUI.SetActive(flag);
				}
			}
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x000DAFE8 File Offset: 0x000D91E8
		protected virtual void HandleCoverRouteLine()
		{
			if (!this.routeLine || this.goingToCoverPoint)
			{
				return;
			}
			this.UpdateDrawRouteDelayTimer();
			if (!this.goingToCornerPoint && !this.isInCornerInputAction && this.drawRouteTimer >= this.drawRouteDelayTime)
			{
				this.pathDrawPoints.Clear();
				this.wayPath = this.wayPathToDraw;
				Vector3 vector = base.transform.position + Vector3.up * 0.5f;
				this.pathDrawPoints.Add(vector);
				for (int i = 1; i < this.wayPath.Count; i++)
				{
					Vector3 vector2 = this.wayPath[i] + Vector3.up * ((i == this.wayPath.Count - 1) ? 0.2f : 0.5f);
					if (vector2 != vector)
					{
						Vector3 vector3 = vector2 - vector;
						vector3.y = 0f;
						vector3.Normalize();
						bool flag = false;
						RaycastHit raycastHit;
						if (Physics.Linecast(vector, vector2, out raycastHit, this.obstaclesLayer))
						{
							if (this.debug)
							{
								Debug.DrawLine(vector2, vector, Color.cyan, 1f);
							}
							Vector3 vector4 = raycastHit.point - vector3 * 0.05f;
							for (int j = 0; j < 20; j++)
							{
								float num = 0.1f * (float)j;
								vector4 += Vector3.up * num;
								if (!Physics.Raycast(vector4, vector3, out raycastHit, 0.1f, this.obstaclesLayer))
								{
									if (j > 0)
									{
										this.pathDrawPoints.Add(vector4);
									}
									flag = true;
									break;
								}
							}
						}
						if (Physics.Linecast(vector2, vector, out raycastHit, this.obstaclesLayer))
						{
							Vector3 vector5 = raycastHit.point + vector3 * 0.05f;
							for (int k = 0; k < 20; k++)
							{
								float num2 = 0.1f * (float)k;
								vector5 += Vector3.up * num2;
								if (!Physics.Raycast(vector5, -vector3, out raycastHit, 0.1f, this.obstaclesLayer))
								{
									if (k > 0)
									{
										this.pathDrawPoints.Add(vector5);
									}
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							this.pathDrawPoints.Add(vector2);
							vector = vector2;
						}
						else
						{
							i--;
							vector = this.pathDrawPoints[this.pathDrawPoints.Count - 1];
						}
					}
				}
				Vector3[] array = this.pathDrawPoints.MakeSmoothCurveArray(this.routeLineSmoothFactor);
				for (int l = 0; l < this.routeLineSmoothPass; l++)
				{
					array = array.MakeSmoothCurve(this.routeLineSmoothFactor);
				}
				this.routeLine.positionCount = array.Length;
				this.routeLine.SetPositions(array);
				return;
			}
			if (this.routeLine.positionCount > 0)
			{
				this.routeLine.positionCount = 0;
			}
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x000DB2EC File Offset: 0x000D94EC
		protected virtual void UpdateDrawRouteDelayTimer()
		{
			if (!this.inCover)
			{
				this.drawRouteTimer = 0f;
			}
			if (this.wayPathToDraw.Count > 1)
			{
				if (this.drawRouteTimer < this.drawRouteDelayTime + 1f)
				{
					this.drawRouteTimer += Time.deltaTime;
				}
			}
			else if (this.drawRouteTimer > 0f)
			{
				this.drawRouteTimer = 0f;
			}
			this.routeLine.material.SetColor(this.routeLineColorName, this.routeLineColor * ((this.drawRouteTimer > this.drawRouteDelayTime) ? Mathf.Clamp(this.drawRouteTimer - this.drawRouteDelayTime, 0f, 1f) : 0f));
		}

		// Token: 0x060026ED RID: 9965 RVA: 0x000DB3AC File Offset: 0x000D95AC
		protected virtual IEnumerator EnterCoverPointRoutine(vCoverPoint target)
		{
			this.onStartGoToCoverPoint.Invoke();
			this.inCover = false;
			this.goingToCoverPoint = true;
			bool flag = this.CheckCoverHeight(target, false);
			this.enterCoverFromLeftSide = this.EnterCoverFromLeftSide(target);
			Vector3 coverDirection = (this.enterCoverFromLeftSide ? target.transform.right : (-target.transform.right));
			this.side = (this.enterCoverFromLeftSide ? vCoverController.Side.left : vCoverController.Side.right);
			this.currentTimeToGetUp = this.timeToStandUp - 0.01f;
			if (this.shooterInput.cc.isCrouching != flag)
			{
				this.shooterInput.cc.isCrouching = flag;
			}
			this.EnterCoverLocomotionState(true);
			float t = 0f;
			Vector3 p = base.transform.position;
			Vector3 p2 = target.transform.InverseTransformPoint(p);
			p2.z = this.PositionOffsetZ;
			p2 = target.transform.TransformPoint(p2);
			Quaternion r = base.transform.rotation;
			Quaternion r2 = Quaternion.LookRotation(coverDirection);
			this.shooterInput.cc.lockRotation = true;
			this.shooterInput.cc.lockMovement = true;
			this.shooterInput.cc.inputMagnitude = 0f;
			this.shooterInput.cc.input = Vector3.zero;
			this.shooterInput.cc.inputSmooth = Vector3.zero;
			this.shooterInput.cc.moveDirection = Vector3.zero;
			this.shooterInput.cc._rigidbody.velocity = Vector3.zero;
			this.shooterInput.cc.ResetInputAnimatorParameters();
			this.shooterInput.cc.moveToDirectionInFree = true;
			float diference = Mathf.Abs(base.transform.forward.AngleFormOtherDirection(coverDirection).y);
			target.EnterCover(base.gameObject);
			while (diference > 5f)
			{
				t += Time.deltaTime * 5f;
				p2.y = base.transform.position.y;
				p.y = base.transform.position.y;
				this.shooterInput.cc.transform.position = Vector3.Lerp(p, p2, Mathf.Clamp(t, 0f, 1f));
				this.shooterInput.cc.transform.rotation = Quaternion.Lerp(r, r2, Mathf.Clamp(t, 0f, 1f));
				diference = Mathf.Abs(base.transform.forward.AngleFormOtherDirection(coverDirection).y);
				yield return null;
			}
			this.shooterInput.cc.lockMovement = false;
			this.currentCoverPoint = target;
			this.goingToCoverPoint = false;
			this.inCover = true;
			this.onFinishGoToCoverPoint.Invoke();
			this.onEnterCover.Invoke();
			yield return new WaitForEndOfFrame();
			this.wasInCover = true;
			yield return null;
			yield break;
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x000DB3C2 File Offset: 0x000D95C2
		protected virtual IEnumerator GoToCoverPointRoutine(vCoverPoint target, List<Vector3> movePath)
		{
			this.shooterInput.cc.lockInStrafe = true;
			float num = this.PathLenght(movePath);
			this.remainingDistanceToNextCover = num;
			if (this.inCover)
			{
				this.onExitCover.Invoke();
			}
			this.onStartGoToCoverPoint.Invoke();
			this.inCover = false;
			this.goingToCoverPoint = true;
			Vector3 coverDirection = (this.enterCoverFromLeftSide ? target.transform.right : (-target.transform.right));
			bool needToCrouch = this.CheckCoverHeight(target, false);
			float stoppingDistance = 0.5f;
			Vector3 vector = base.transform.forward;
			Vector3 targetPosition = base.transform.position;
			bool slideToCover = false;
			if (movePath.Count > 0)
			{
				this.SetCameraState(this.goToCoverCameraState);
				if (movePath != null && movePath.Count > 1)
				{
					movePath.RemoveAt(0);
				}
				this.enterCoverFromLeftSide = this.EnterCoverFromLeftSide(target);
				coverDirection = (this.enterCoverFromLeftSide ? target.transform.right : (-target.transform.right));
				this.side = (this.enterCoverFromLeftSide ? vCoverController.Side.left : vCoverController.Side.right);
				Vector3 position = base.transform.position;
				slideToCover = this.useSlideToCover && num > this.minPathLenghToSlide.y;
				bool isSliding = false;
				this.currentCornerWeight = 0f;
				this.PrepareToMoveOnPath(num, false);
				vector = base.transform.forward;
				targetPosition = ((movePath.Count > 0) ? movePath[movePath.Count - 1] : base.transform.position);
				Vector3 vector2 = target.transform.InverseTransformPoint(targetPosition);
				vector2.z = this.PositionOffsetZ;
				targetPosition = target.transform.TransformPoint(vector2);
				Debug.DrawLine(targetPosition, targetPosition + Vector3.up, Color.red);
				if (movePath.Count > 0)
				{
					movePath[movePath.Count - 1] = targetPosition;
				}
				else
				{
					movePath.Add(targetPosition);
				}
				this.PlayGoToCoverPointState();
				while (movePath.Count > 1 || (targetPosition - base.transform.position).magnitude > 0.5f)
				{
					yield return new WaitForFixedUpdate();
					this.remainingDistanceToNextCover = this.PathLenght(movePath);
					Vector3 vector3 = movePath[0];
					vector3.y = base.transform.position.y;
					vector = vector3 - base.transform.position;
					vector.y = 0f;
					this.shooterInput.cc.moveDirection = vector.normalized;
					this.shooterInput.cc.MoveToPosition(vector3);
					if (!this.wasInCover && !this.shooterInput.cc.isSprinting && this.shooterInput.cc.isStrafing)
					{
						this.shooterInput.cc.lockRotation = true;
						this.shooterInput.cc.isStrafing = true;
						this.shooterInput.cc.RotateToDirection(this.enterCoverFromLeftSide ? target.transform.right : (-target.transform.right), 5f);
					}
					this.shooterInput.cc.inputSmooth = this.shooterInput.cc.input;
					if (Vector3.Distance(base.transform.position, vector3) < 0.2f)
					{
						if (movePath.Count > 1)
						{
							movePath.RemoveAt(0);
						}
					}
					else
					{
						if (this.genericAction.triggerAction && this.CheckActionsOnWay(vector3, true) && !this.shooterInput.cc.customAction)
						{
							if (this.possibleActions.Contains(this.genericAction.triggerAction.actionName))
							{
								this.genericAction.TriggerActionEvents();
								this.genericAction.TriggerAnimation();
								if (movePath.Count > 1)
								{
									movePath.RemoveAt(0);
									continue;
								}
								continue;
							}
							else
							{
								this.ExitCover(false);
								this.onFinishGoToCoverPoint.Invoke();
								base.StopCoroutine(this.currentCoverRoutine);
							}
						}
						if (!this.shooterInput.cc.IsAnimatorTag("SlideToCover") && !this.shooterInput.cc.IsAnimatorTag("CoverLocomotion") && !this.shooterInput.cc.customAction)
						{
							this.PlayGoToCoverPointState();
						}
						if (this.remainingDistanceToNextCover < (isSliding ? (stoppingDistance * 2f) : stoppingDistance))
						{
							if (isSliding)
							{
								this.shooterInput.cc.input = Vector3.zero;
								this.shooterInput.animator.CrossFadeInFixedTime(this.finishSlideToCoverPoint, this.finishSlideToCoverAnimCrossfade);
								yield return new WaitForSeconds(this.finishSlideToCoverWaitTime);
							}
							IL_06C1:
							while (this.shooterInput.cc.customAction || this.shooterInput.animator.IsInTransition(0))
							{
								yield return null;
							}
							goto IL_06E6;
						}
						if (slideToCover && movePath.Count == 1 && !this.shooterInput.cc.IsAnimatorTag("SlideToCover") && this.remainingDistanceToNextCover < Mathf.Max(this.minPathLenghToSlide.x + 0.5f, this.minPathLenghToSlide.y - 1f) && this.remainingDistanceToNextCover > this.minPathLenghToSlide.x && !this.shooterInput.cc.customAction)
						{
							this.shooterInput.animator.CrossFadeInFixedTime(this.startSlideToCoverPoint, this.startSlideToCoverAnimCrossfade);
							isSliding = true;
						}
					}
				}
				goto IL_06C1;
			}
			IL_06E6:
			this.currentTimeToGetUp = this.timeToStandUp - 0.01f;
			this.goingToCoverPoint = false;
			if (this.shooterInput.cc.isCrouching != needToCrouch)
			{
				this.shooterInput.cc.isCrouching = needToCrouch;
			}
			this.EnterCoverLocomotionState(true);
			this.shooterInput.cc.lockInStrafe = false;
			float t = 0f;
			Vector3 p = base.transform.position;
			Vector3 p2 = targetPosition;
			Quaternion r = base.transform.rotation;
			Quaternion r2 = Quaternion.LookRotation(coverDirection);
			this.shooterInput.cc.lockRotation = true;
			this.shooterInput.cc.lockMovement = true;
			this.shooterInput.cc.lockSetMoveSpeed = true;
			this.shooterInput.cc.inputMagnitude = 0f;
			this.shooterInput.cc.input = Vector3.zero;
			this.shooterInput.cc.inputSmooth = Vector3.zero;
			this.shooterInput.cc.moveDirection = Vector3.zero;
			this.shooterInput.cc._rigidbody.velocity = Vector3.zero;
			this.shooterInput.cc.moveToDirectionInFree = true;
			this.shooterInput.cc.ResetInputAnimatorParameters();
			float diference = Mathf.Abs(base.transform.forward.AngleFormOtherDirection(coverDirection).y);
			target.EnterCover(base.gameObject);
			while (diference > 5f)
			{
				t += Time.deltaTime * 5f;
				p2.y = base.transform.position.y;
				p.y = base.transform.position.y;
				this.shooterInput.cc.transform.position = Vector3.Lerp(p, p2, Mathf.Clamp(t, 0f, 1f));
				this.shooterInput.cc.transform.rotation = Quaternion.Lerp(r, r2, Mathf.Clamp(t, 0f, 1f));
				diference = Mathf.Abs(base.transform.forward.AngleFormOtherDirection(coverDirection).y);
				yield return null;
			}
			this.shooterInput.cc.lockMovement = false;
			this.currentCoverPoint = target;
			this.inCover = true;
			this.onFinishGoToCoverPoint.Invoke();
			this.onEnterCover.Invoke();
			yield return new WaitForEndOfFrame();
			this.wasInCover = true;
			yield break;
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x000DB3E0 File Offset: 0x000D95E0
		protected virtual bool CheckActionsOnWay(Vector3 nextWayPoint, bool checkAngle = true)
		{
			if (!this.genericAction.triggerAction)
			{
				return false;
			}
			Vector3 vector = this.genericAction.triggerAction._collider.ClosestPoint(nextWayPoint);
			float num = Vector3.Distance(vector, nextWayPoint);
			vector = this.genericAction.triggerAction._collider.ClosestPoint(base.transform.position);
			vector.y = base.transform.position.y;
			float num2 = Vector3.Distance(vector, base.transform.position);
			bool flag = true;
			if (this.genericAction.triggerAction._collider is BoxCollider)
			{
				flag = (this.genericAction.triggerAction._collider as BoxCollider).IsClosed(base.transform.position, this.checkActionMargin, this.checkActionOffset);
			}
			return flag && num > 0.2f && num2 < this.shooterInput.cc._capsuleCollider.radius * 0.5f && (!checkAngle || this.CheckAngleOfDirections(this.genericAction.triggerAction.transform.forward, this.shooterInput.cc.moveDirection.normalized, 45f)) && !this.shooterInput.cc.customAction;
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x000DB534 File Offset: 0x000D9734
		protected virtual void EnterCoverLocomotionState(bool playCoverLocomotion = true)
		{
			if (this.debug)
			{
				Debug.Log("Enter Cover");
			}
			this.shooterInput.SetWalkByDefault(this.originalWalkByDefault);
			this.shooterInput.lockCameraInput = false;
			this.shooterInput.cc.lockRotation = true;
			this.shooterInput.cc.isStrafing = false;
			this.shooterInput.cc.lockInStrafe = false;
			this.shooterInput.cc.isSprinting = false;
			this.shooterInput.crouchInput.useInput = false;
			this.shooterInput.SetLockAllInput(false);
			this.shooterInput.lockMoveInput = false;
			this.shooterInput.SetLockUpdateMoveDirection(true);
			if (playCoverLocomotion)
			{
				this.ControlSideInCover();
				this.PlayCoverLocomotionState(this.enterCoverFromLeftSide);
			}
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x000DB600 File Offset: 0x000D9800
		protected virtual void PlayGoToCoverPointState()
		{
			string text = this.goToCoverPoint;
			this.shooterInput.animator.CrossFadeInFixedTime(text, this.goToCoverAnimCrossfade);
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x000DB62C File Offset: 0x000D982C
		protected virtual void PlayCoverLocomotionState(bool enterCoverFromLeftSide)
		{
			if (this.debug)
			{
				Debug.Log("Play Cover Anim");
			}
			this.side = (enterCoverFromLeftSide ? vCoverController.Side.left : vCoverController.Side.right);
			this.shooterInput.cc.animator.SetFloat("CoverSide", (float)this.side, 0.2f, Time.deltaTime);
			string text = (enterCoverFromLeftSide ? (this.shooterInput.cc.isCrouching ? this.coverCrouchedLeft : this.coverStandingLeft) : (this.shooterInput.cc.isCrouching ? this.coverCrouchedRight : this.coverStandingRight));
			this.shooterInput.animator.CrossFadeInFixedTime(text, this.enterCoverAnimCrossfade);
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x000DB6E0 File Offset: 0x000D98E0
		protected virtual bool EnterCoverFromLeftSide(vCoverPoint coverPoint)
		{
			if (coverPoint == null)
			{
				return false;
			}
			this.isLeftCorner = this.IsLeftCoverCorner(coverPoint);
			this.isRightCorner = this.IsRightCoverCorner(coverPoint);
			if (this.isRightCorner)
			{
				return false;
			}
			if (this.isLeftCorner)
			{
				return true;
			}
			Vector3 vector = coverPoint.transform.position - this.shooterInput.cameraMain.transform.position;
			vector.y = 0f;
			return (Quaternion.LookRotation(vector.normalized).eulerAngles - Quaternion.LookRotation(-coverPoint.transform.forward).eulerAngles).NormalizeAngle().y < -15f;
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x000DB7A0 File Offset: 0x000D99A0
		protected virtual void PlayDefaultLocomotionState()
		{
			string text = (this.shooterInput.cc.isStrafing ? (this.shooterInput.cc.isCrouching ? this.defaultStrafeCrouched : this.defaultStrafeStanding) : (this.shooterInput.cc.isCrouching ? this.defaultFreeCrouched : this.defaultFreeStanding));
			this.shooterInput.animator.CrossFadeInFixedTime(text, this.exitCoverAnimCrossfade);
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x000DB81C File Offset: 0x000D9A1C
		protected virtual void ExitCover(bool forceExit = false)
		{
			if (this.debug)
			{
				Debug.Log("Exit Cover");
			}
			base.StopAllCoroutines();
			this.ResetCoverCornerUI();
			this.currentCornerWeight = 0f;
			this.shooterInput.animator.SetLayerWeight(this.animatorCoverCornerLayer, 0f);
			this.shooterInput.animator.SetLayerWeight(this.animatorCoverOverBarrierLayer, 0f);
			this.shooterInput.tpCamera.switchRight = 1f;
			this.shooterInput.ResetCameraState();
			this.shooterInput.SetLockUpdateMoveDirection(false);
			this.shooterInput.SetLockAllInput(false);
			this.shooterInput.lockCameraInput = false;
			this.shooterInput.lockUpdateMoveDirection = false;
			this.shooterInput.lockMoveInput = false;
			this.shooterInput.crouchInput.useInput = true;
			this.shooterInput.cc.ignoreAnimatorMovement = false;
			this.shooterInput.cc.lockRotation = false;
			this.shooterInput.cc.isSprinting = false;
			this.shooterInput.cc.lockSetMoveSpeed = false;
			this.shooterInput.cc.ResetCapsule();
			this.shooterInput.cc.lockInStrafe = false;
			this.shooterInput.shooterManager.hipfireAimTime = this.lastHipFireTimer;
			this.shooterInput.shooterManager.hipfireShot = this.lastUseHipFire;
			this.shooterInput.cc.moveToDirectionInFree = this.lastMoveToDirectionInFree;
			if (this.alwaysStandUpOnExit)
			{
				this.shooterInput.cc.isCrouching = false;
			}
			this.currentCoverPoint = null;
			this.possibleCoverPoint = null;
			this.possibleCornerPoint = null;
			this.goingToCoverPoint = false;
			this.goingToCornerPoint = false;
			this.isAimingInCorner = false;
			this.isRightCorner = false;
			this.isLeftCorner = false;
			this.isInRightCornerPositionRange = false;
			this.isInLeftCornerPositionRange = false;
			this.inCornerAngle = false;
			this.canMove = false;
			this.shooterInput.ResetCameraState();
			this.shooterInput.ResetCustomIKAdjustState();
			this.shooterInput.tpCamera.switchRight = 1f;
			if (this.shooterInput.cc.IsAnimatorTag("CoverLocomotion") && ((!this.shooterInput.cc.customAction && !this.isRolling && !this.shooterInput.cc.isDead) || forceExit))
			{
				this.PlayDefaultLocomotionState();
			}
			this.wasInCover = false;
			this.inCover = false;
			this.goingToCoverPoint = false;
			this.shooterInput.animator.speed = 1f;
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x000DBAB0 File Offset: 0x000D9CB0
		protected virtual void HandlePossibleCornerPoint()
		{
			if (!this.inCover || this.goingToCoverPoint || !this.currentCoverPoint || this.goingToCornerPoint || this.shooterInput.IsAiming)
			{
				this.ResetCoverCornerUI();
				return;
			}
			this.holdingCornerInput = this.enterUsingGetCoverInput && this.enterExitInput.GetButton();
			bool flag = this.IsLookingToCorner();
			this.possibleCornerPoint = ((this.side == vCoverController.Side.left) ? this.currentCoverPoint.leftCorner : this.currentCoverPoint.rightCorner);
			this.isInCornerInputAction = flag && this.possibleCornerPoint && this.enterUsingGetCoverInput;
			if (this.cornerInputUI)
			{
				bool flag2 = this.isInCornerInputAction;
				if (this.cornerInputUI.activeSelf != flag2)
				{
					this.cornerInputUI.SetActive(flag2);
				}
				if (this.possibleCornerPoint)
				{
					this.cornerInputUI.transform.SetPositionAndRotation(this.possibleCornerPoint.transform.position + this.possibleCornerPoint.transform.forward * 0.1f + Vector3.up * this.CornerUIHeight * 0.25f, Quaternion.LookRotation(this.possibleCornerPoint.transform.forward));
				}
			}
			if (this.cornerUICenter)
			{
				this.cornerUICenter.transform.localPosition = Vector3.up * this.CornerUIHeight;
			}
			if (flag && this.possibleCornerPoint && (!this.enterUsingGetCoverInput || this.holdingCornerInput))
			{
				this.currentTimeToEnterCorner += Time.deltaTime;
				if (this.cornerRightUI && this.cornerRightUI.gameObject.activeSelf != (this.side == vCoverController.Side.right && !this.enterUsingGetCoverInput))
				{
					this.cornerRightUI.gameObject.SetActive(this.side == vCoverController.Side.right && !this.enterUsingGetCoverInput);
				}
				if (this.cornerLeftUI && this.cornerLeftUI.gameObject.activeSelf != (this.side == vCoverController.Side.left && !this.enterUsingGetCoverInput))
				{
					this.cornerLeftUI.gameObject.SetActive(this.side == vCoverController.Side.left && !this.enterUsingGetCoverInput);
				}
				this.onUpdateCornerInput.Invoke(this.currentTimeToEnterCorner / this.timeToEnterCorner);
				if (this.currentTimeToEnterCorner > this.timeToEnterCorner && this.CalculatePath(this.possibleCornerPoint.posePosition) && this.path.status == NavMeshPathStatus.PathComplete)
				{
					this.StartGoToCornerPoint(this.possibleCornerPoint);
					return;
				}
			}
			else if (this.currentTimeToEnterCorner > 0f)
			{
				this.onUpdateCornerInput.Invoke(0f);
				this.currentTimeToEnterCorner = 0f;
				if (this.cornerRightUI && this.cornerRightUI.gameObject.activeSelf)
				{
					this.cornerRightUI.gameObject.SetActive(false);
				}
				if (this.cornerLeftUI && this.cornerLeftUI.gameObject.activeSelf)
				{
					this.cornerLeftUI.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x000DBE10 File Offset: 0x000DA010
		protected virtual void StartGoToCornerPoint(vCoverPoint newCp)
		{
			this.goingToCornerPoint = true;
			this.shooterInput.cc.lockSetMoveSpeed = true;
			if (this.cornerInputUI && this.cornerInputUI.gameObject.activeSelf)
			{
				this.cornerInputUI.SetActive(false);
			}
			this.currentTimeToEnterCorner = 0f;
			this.onUpdateCornerInput.Invoke(0f);
			Vector3 vector = ((this.side == vCoverController.Side.left) ? this.currentCoverPoint.leftCornerP : this.currentCoverPoint.rightCornerP);
			vector = this.currentCoverPoint.transform.InverseTransformPoint(vector);
			vector.z = this.localCharPosition.z;
			vector = this.currentCoverPoint.transform.TransformPoint(vector);
			vector = newCp.transform.InverseTransformPoint(vector);
			vector.z = this.localCharPosition.z;
			vector = newCp.transform.TransformPoint(vector);
			Vector3 vector2 = newCp.transform.InverseTransformPoint(newCp.posePosition);
			vector2.z = this.localCharPosition.z;
			vector2 = newCp.transform.TransformPoint(vector2);
			Vector3[] array = new Vector3[]
			{
				base.transform.position,
				vector,
				vector2
			}.MakeSmoothCurve(0.25f);
			array = array.MakeSmoothCurve(0.25f);
			if (this.cornerRightUI && this.cornerRightUI.gameObject.activeSelf)
			{
				this.cornerRightUI.gameObject.SetActive(false);
			}
			if (this.cornerLeftUI && this.cornerLeftUI.gameObject.activeSelf)
			{
				this.cornerLeftUI.gameObject.SetActive(false);
			}
			this.currentCoverPoint = newCp;
			this.currentCoverRoutine = base.StartCoroutine(this.GoToCornerPointRoutine(array));
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x000DBFF1 File Offset: 0x000DA1F1
		protected virtual IEnumerator GoToCornerPointRoutine(Vector3[] cornerPath)
		{
			int pathIndex = 0;
			this.PrepareToMoveOnPath(this.PathLenght(cornerPath.vToList<Vector3>()), this.shooterInput.cc.isCrouching);
			Vector3 vector = cornerPath[pathIndex];
			vector.y = base.transform.position.y;
			Vector3 vector2 = base.transform.position;
			(vector - vector2).y = 0f;
			while (pathIndex < cornerPath.Length)
			{
				yield return null;
				this.ControlCoverHelperTransform();
				vector2 = base.transform.position;
				vector = cornerPath[pathIndex];
				Vector3 vector3 = vector - vector2;
				vector.y = base.transform.position.y;
				if (vector3.magnitude > 0.25f)
				{
					vector3.y = 0f;
					this.shooterInput.cc.MoveToPosition(base.transform.position + vector3.normalized);
					this.shooterInput.cc.inputMagnitude = 1f;
					this.shooterInput.cc.inputSmooth = this.shooterInput.cc.input;
				}
				else
				{
					if (pathIndex + 1 > cornerPath.Length - 1)
					{
						break;
					}
					int num = pathIndex;
					pathIndex = num + 1;
				}
			}
			this.shooterInput.lockCameraInput = false;
			this.shooterInput.cc.lockRotation = true;
			this.shooterInput.cc.isStrafing = false;
			this.shooterInput.cc.lockInStrafe = false;
			this.shooterInput.crouchInput.useInput = false;
			this.shooterInput.SetLockAllInput(false);
			this.shooterInput.lockMoveInput = false;
			this.goingToCornerPoint = false;
			this.isInCornerInputAction = false;
			yield break;
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x000DC008 File Offset: 0x000DA208
		protected virtual void HandleCoverMovement()
		{
			if (this.inCover || this.goingToCornerPoint || this.goingToCoverPoint)
			{
				this.ControlSpeedInCover();
			}
			if (!this.inCover)
			{
				return;
			}
			this.ControlNearCoverPoint();
			if (!this.currentCoverPoint)
			{
				return;
			}
			this.ControlCoverHelperTransform();
			this.ControlCrouchStateInCover();
			this.ControlInputDirectionInCover();
			this.ControlMovementDirectionInCover();
			this.ControlCoverAngles();
			this.ControlSideInCover();
			this.ControlCoverLocomotionState();
			this.ControlRotationInCover();
			this.ControlMovementInCover();
			this.CheckOverBarrier();
			this.ControlCameraState();
			this.ControlIKState();
			this.ControlExitCoverByAngle();
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x000DC0A0 File Offset: 0x000DA2A0
		protected virtual void ControlCoverHelperTransform()
		{
			if (!this.currentCoverPoint)
			{
				return;
			}
			Vector3 vector = this.currentCoverPoint.transform.forward;
			Vector3 vector2 = this.currentCoverPoint.posePosition;
			int num = 1;
			for (int i = 0; i < this.coverPoints.Count; i++)
			{
				if (this.coverPoints[i] != this.currentCoverPoint && this.currentCoverPoint.leftCorner != this.coverPoints[i] && this.currentCoverPoint.rightCorner != this.coverPoints[i])
				{
					num++;
					if (this.currentCoverPoint.corner == (vCoverPoint.Corner)0)
					{
						vector2 += this.coverPoints[i].posePosition;
					}
				}
			}
			if (num > 1 && this.currentCoverPoint.corner == (vCoverPoint.Corner)0)
			{
				vector2 /= (float)this.coverPoints.Count;
			}
			vCoverPoint right = this.currentCoverPoint.right;
			vCoverPoint left = this.currentCoverPoint.left;
			Vector3 vector3 = this.currentCoverPoint.transform.InverseTransformPoint(vector2);
			if (this.isLeftCorner || this.isRightCorner)
			{
				if (this.isLeftCorner)
				{
					if (vector3.x >= 0f)
					{
						vector3.x = 0f;
					}
					vector2 = this.currentCoverPoint.transform.TransformPoint(vector3);
				}
				else if (this.isRightCorner)
				{
					if (vector3.x <= 0f)
					{
						vector3.x = 0f;
					}
					vector2 = this.currentCoverPoint.transform.TransformPoint(vector3);
				}
				vector.y = 0f;
				vector.Normalize();
				vector2 -= vector * this.currentCoverPoint.posePositionZ;
				if (vector.magnitude > 0f)
				{
					this.helper.transform.position = vector2;
					this.helper.forward = vector;
					this.coverNormal = vector;
					this.positionHelper = vector2;
					return;
				}
			}
			else
			{
				if (this.side == vCoverController.Side.right && right)
				{
					Vector3 posePosition = this.currentCoverPoint.posePosition;
					Vector3 posePosition2 = right.posePosition;
					Vector3 forward = right.transform.forward;
					forward.y = 0f;
					float num2 = Vector3.Distance(posePosition, posePosition2);
					float num3 = Mathf.Clamp(Vector3.Distance(base.transform.position, posePosition2) - 0.2f, 0f, num2);
					float num4 = 1f - Mathf.Clamp(num3 / num2, 0f, 1f);
					vector = Vector3.Lerp(vector, forward.normalized, num4);
					vector2 = Vector3.Lerp(posePosition, posePosition2, num4);
				}
				if (this.side == vCoverController.Side.left && left)
				{
					Vector3 posePosition3 = this.currentCoverPoint.posePosition;
					Vector3 posePosition4 = left.posePosition;
					Vector3 forward2 = left.transform.forward;
					forward2.y = 0f;
					float num5 = Vector3.Distance(posePosition3, posePosition4);
					float num6 = Mathf.Clamp(Vector3.Distance(base.transform.position, posePosition4) - 0.2f, 0f, num5);
					float num7 = 1f - Mathf.Clamp(num6 / num5, 0f, 1f);
					vector = Vector3.Lerp(vector, forward2.normalized, num7);
					vector2 = Vector3.Lerp(posePosition3, posePosition4, num7);
				}
				vector2 -= vector * this.currentCoverPoint.posePositionZ;
				vector.y = 0f;
				vector.Normalize();
				if (vector.magnitude > 0f)
				{
					Debug.DrawLine(vector2 + vector * this.PositionOffsetZ + Vector3.up * 0.2f, vector2 + Vector3.up * 0.2f, Color.red);
					RaycastHit raycastHit;
					if (Physics.Linecast(vector2 + vector * this.PositionOffsetZ + Vector3.up * 0.2f, vector2 + Vector3.up * 0.2f, out raycastHit, this.obstaclesLayer))
					{
						vector2 = raycastHit.point - Vector3.up * 0.2f;
					}
					this.helper.transform.position = vector2;
					this.helper.forward = vector;
					this.coverNormal = vector;
					this.positionHelper = vector2;
				}
			}
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x000DC51F File Offset: 0x000DA71F
		protected virtual void ControlCrouchStateInCover()
		{
			this.shooterInput.cc.isCrouching = this.CheckCoverHeight(this.currentCoverPoint, this.shooterInput.isAimingByInput);
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x000DC548 File Offset: 0x000DA748
		protected virtual void ControlInputDirectionInCover()
		{
			this.cameraRightDir = this.shooterInput.cameraMain.transform.right;
			this.cameraRightDir.y = 0f;
			this.cameraRightDir.Normalize();
			this.cameraForwardDir = Quaternion.AngleAxis(-90f, Vector3.up) * this.cameraRightDir;
			this.inputDirection = this.shooterInput.cc.inputSmooth.x * this.cameraRightDir + this.shooterInput.cc.inputSmooth.z * this.cameraForwardDir;
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x000DC5F6 File Offset: 0x000DA7F6
		protected virtual void ControlMovementDirectionInCover()
		{
			if (!this.inCover)
			{
				return;
			}
			this.coverMoveDirection = this.helper.InverseTransformDirection(this.inputDirection);
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x000DC618 File Offset: 0x000DA818
		protected virtual void ControlCoverAngles()
		{
			Vector3 eulerAngles = Quaternion.LookRotation(-this.helper.forward).eulerAngles;
			Vector3 eulerAngles2 = Quaternion.LookRotation(this.cameraForwardDir).eulerAngles;
			this.cameraAngle = (float)((int)(eulerAngles2 - eulerAngles).NormalizeAngle().y);
			if (this.inputDirection == Vector3.zero)
			{
				this.movementAngle = 0f;
				return;
			}
			Vector3 eulerAngles3 = Quaternion.LookRotation(this.inputDirection.normalized).eulerAngles;
			this.movementAngle = (float)((int)(eulerAngles3 - eulerAngles).NormalizeAngle().y);
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x000DC6C0 File Offset: 0x000DA8C0
		protected virtual void ControlSideInCover()
		{
			if (this.goingToCornerPoint)
			{
				return;
			}
			Vector3 vector = this.coverMoveDirection;
			float magnitude = vector.magnitude;
			if (Mathf.Abs(vector.y) > 0f)
			{
				vector.y = 0f;
			}
			float num = (float)Math.Round((double)this.localCharPosition.x, 1);
			if (!this.shooterInput.IsAiming)
			{
				float num2 = (((double)this.inputDirection.magnitude > 0.1 && (double)this.coverNormal.magnitude > 0.1) ? Mathf.Abs(this.inputDirection.AngleFormOtherDirection(this.coverNormal).y) : 0f);
				if (vector.magnitude > 0.1f && num2.IsInSideRange(20f, 160f))
				{
					if ((double)vector.x > 0.1)
					{
						this.side = vCoverController.Side.left;
					}
					else if ((double)vector.x < -0.1)
					{
						this.side = vCoverController.Side.right;
					}
				}
				else if (vector.magnitude < 0.1f && this.changeSide && Mathf.Abs(this.cameraAngle) > this.angleToChangeSide.x)
				{
					if (this.side == vCoverController.Side.right)
					{
						if (this.cameraAngle < -this.angleToChangeSide.x && this.cameraAngle > -this.angleToChangeSide.y)
						{
							this.side = vCoverController.Side.left;
						}
					}
					else if (this.side == vCoverController.Side.left && this.cameraAngle > this.angleToChangeSide.x && this.cameraAngle < this.angleToChangeSide.y)
					{
						this.side = vCoverController.Side.right;
					}
				}
			}
			else if (this.changeSideAiming)
			{
				if (this.cameraAngle.IsInSideRange(-this.angleToChangeSideOnAiming.x, this.angleToChangeSideOnAiming.x))
				{
					if (this.isLeftCorner && this.side == vCoverController.Side.right && num >= 0f && vector.x > 0.9f)
					{
						this.side = vCoverController.Side.left;
					}
					else if (this.isRightCorner && this.side == vCoverController.Side.left && num <= 0f && vector.x < -0.9f)
					{
						this.side = vCoverController.Side.right;
					}
				}
				else if (this.side == vCoverController.Side.left)
				{
					if (this.cameraAngle.IsInSideRange(this.angleToChangeSideOnAiming.x, this.angleToChangeSideOnAiming.y))
					{
						this.side = vCoverController.Side.right;
					}
				}
				else if (this.side == vCoverController.Side.right && this.cameraAngle.IsInSideRange(-this.angleToChangeSideOnAiming.y, -this.angleToChangeSideOnAiming.x))
				{
					this.side = vCoverController.Side.left;
				}
			}
			this.enterCoverFromLeftSide = this.side == vCoverController.Side.left;
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x000DC9A8 File Offset: 0x000DABA8
		protected virtual void ControlCoverLocomotionState()
		{
			this.shooterInput.cc.animator.SetFloat("CoverSide", (float)this.side, 0.2f, Time.deltaTime);
			this.shooterInput.animator.SetLayerWeight(this.animatorCoverCornerLayer, this.currentCornerWeight);
			if (!this.shooterInput.IsAiming)
			{
				if (!this.shooterInput.cc.IsAnimatorTag("CoverLocomotion") && this.EnterCoverConditions())
				{
					this.shooterInput.cc.isCrouching = this.CheckCoverHeight(this.currentCoverPoint, false);
					this.onEnterCover.Invoke();
					this.PlayCoverLocomotionState(this.enterCoverFromLeftSide);
					return;
				}
			}
			else if (this.shooterInput.IsAiming && this.shooterInput.cc.IsAnimatorTag("CoverLocomotion") && !this.shooterInput.animator.IsInTransition(0))
			{
				this.onExitCover.Invoke();
				string text = (this.shooterInput.cc.isCrouching ? "Strafing Crouch" : "Strafing Movement");
				this.shooterInput.animator.CrossFadeInFixedTime(text, this.exitCoverAnimCrossfade);
			}
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000DCADC File Offset: 0x000DACDC
		protected virtual void ControlSpeedInCover()
		{
			this.shooterInput.cc.lockSetMoveSpeed = !this.shooterInput.IsAiming;
			if (!this.shooterInput.IsAiming)
			{
				float num;
				if (this.goingToCoverPoint)
				{
					this.moveSpeed.walkByDefault = false;
					num = (this.shooterInput.cc.isSprinting ? this.goToCoverPointSprintingAnimatorSpeed : this.goToCoverPointRunningAnimatorSpeed);
				}
				else if (this.goingToCornerPoint)
				{
					this.moveSpeed.walkByDefault = true;
					num = this.goToCornerPointAnimatorSpeed;
				}
				else if (this.shooterInput.cc.isCrouching)
				{
					this.moveSpeed.walkByDefault = true;
					num = this.crouchAnimatorSpeed;
				}
				else
				{
					this.moveSpeed.walkByDefault = true;
					num = this.standAnimatorSpeed;
				}
				this.shooterInput.animator.speed = Mathf.Lerp(1f, num, Mathf.Min(this.shooterInput.cc.inputSmooth.magnitude, 1f));
				this.moveSpeed.crouchSpeed = (this.goingToCornerPoint ? this.goToCornerCrouchedMoveSpeed : this.crouchedMoveSpeed);
				this.moveSpeed.walkSpeed = (this.goingToCornerPoint ? this.goToCornerStandingMoveSpeed : this.standingMoveSpeed);
				this.moveSpeed.runningSpeed = this.goToCoverPointRunningMoveSpeed;
				this.moveSpeed.sprintSpeed = this.goToCoverPointSprintingMoveSpeed;
				this.shooterInput.cc.SetControllerMoveSpeed(this.moveSpeed);
				this.shooterInput.cc.SetAnimatorMoveSpeed(this.moveSpeed);
				return;
			}
			this.shooterInput.animator.speed = 1f;
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000DCC90 File Offset: 0x000DAE90
		protected virtual void ControlRotationInCover()
		{
			if (this.goingToCornerPoint || !this.inCover)
			{
				return;
			}
			this.shooterInput.cc.lockRotation = !this.shooterInput.IsAiming || (this.shooterInput.isAimingByInput && this.currentCornerWeight > 0.1f);
			if (this.shooterInput.IsAiming)
			{
				if (this.shooterInput.cc.lockRotation && this.currentCornerWeight > 0.1f)
				{
					Vector3 vector = -this.currentCoverPoint.transform.forward;
					vector.y = 0f;
					base.transform.rotation = Quaternion.LerpUnclamped(base.transform.rotation, Quaternion.LookRotation(vector), this.currentCornerWeight);
				}
				return;
			}
			if (this.shooterInput.cc.lockRotation)
			{
				Vector3 vector2 = Quaternion.AngleAxis((float)((vCoverController.Side)(-90) * this.side), Vector3.up) * this.coverNormal.normalized;
				Vector3 vector3 = Quaternion.AngleAxis((float)((vCoverController.Side)(-70) * this.side), Vector3.up) * this.coverNormal.normalized;
				float num = ((Vector3.Angle(base.transform.forward, vector3) > 25f) ? Quaternion.LookRotation(vector3).eulerAngles.y : Quaternion.LookRotation(vector2).eulerAngles.y);
				Vector3 eulerAngles = base.transform.rotation.eulerAngles;
				eulerAngles.y = Mathf.LerpAngle(eulerAngles.y, num, this.rotationSpeed * Time.deltaTime);
				base.transform.rotation = Quaternion.Euler(eulerAngles);
			}
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x000DCE4C File Offset: 0x000DB04C
		protected virtual void ControlMovementInCover()
		{
			if (this.goingToCornerPoint)
			{
				return;
			}
			Vector3 vector = this.coverMoveDirection;
			float magnitude = vector.magnitude;
			if (Mathf.Abs(vector.y) >= 0f)
			{
				vector.y = 0f;
			}
			if (Mathf.Abs(vector.x) <= 0.1f)
			{
				vector.x = 0f;
			}
			this.shooterInput.cc.ignoreAnimatorMovement = this.shooterInput.cc.inputSmooth.magnitude < 0.1f;
			this.isLeftCorner = this.IsLeftCoverCorner(this.currentCoverPoint);
			this.isRightCorner = this.IsRightCoverCorner(this.currentCoverPoint);
			this.localCharPosition = this.helper.InverseTransformPoint(base.transform.position);
			this.localCharPosition.z = this.PositionOffsetZ;
			this.inCornerAngle = !this.shooterInput.isAimingByInput || Mathf.Abs(this.cameraAngle) < 60f;
			float x = vector.x;
			float num = this.XLocalClampedInCorner;
			if (num < 0f && this.isLeftCorner && this.isRightCorner)
			{
				num = 0f;
			}
			float num2 = (float)Math.Round((double)this.localCharPosition.x, 2);
			if (this.isLeftCorner)
			{
				if (num2 >= num)
				{
					this.localCharPosition.x = num;
					if (x > -0.1f)
					{
						vector = Vector3.zero;
					}
				}
				this.isInLeftCornerPositionRange = this.side == vCoverController.Side.left && this.localCharPosition.x >= num - (this.wasAiming ? 0.1f : (this.shooterInput.cc.colliderRadiusDefault + 0.1f));
			}
			else
			{
				this.isInLeftCornerPositionRange = false;
			}
			if (this.isRightCorner)
			{
				if (num2 <= -num)
				{
					this.localCharPosition.x = -num;
					if (x < 0.1f)
					{
						vector = Vector3.zero;
					}
				}
				this.isInRightCornerPositionRange = this.side == vCoverController.Side.right && this.localCharPosition.x <= -(num - (this.wasAiming ? 0.1f : (this.shooterInput.cc.colliderRadiusDefault + 0.1f)));
			}
			else
			{
				this.isInRightCornerPositionRange = false;
			}
			if (this.shooterInput.aimInput.GetButtonDown())
			{
				if (this.side == vCoverController.Side.right)
				{
					this.isInRightCorner = this.isInRightCornerPositionRange && this.inCornerAngle;
					this.isInLeftCorner = false;
				}
				else if (this.side == vCoverController.Side.left)
				{
					this.isInLeftCorner = this.isInLeftCornerPositionRange && this.inCornerAngle;
					this.isInRightCorner = false;
				}
				this.isAimingInCorner = this.isInRightCorner || this.isInLeftCorner;
			}
			else if (this.shooterInput.isAimingByInput && this.inCornerAngle)
			{
				if (this.isInRightCorner && (double)x > 0.1)
				{
					this.isAimingInCorner = false;
					this.isInRightCorner = false;
				}
				else if (!this.isInRightCorner && this.isInRightCornerPositionRange && (x < -0.1f || this.isAimingInCorner))
				{
					this.isAimingInCorner = true;
					this.isInRightCorner = true;
				}
				if (this.isInLeftCorner && (double)x < -0.1)
				{
					this.isAimingInCorner = false;
					this.isInLeftCorner = false;
				}
				else if (!this.isInLeftCorner && this.isInLeftCornerPositionRange && (x > 0.1f || this.isAimingInCorner))
				{
					this.isAimingInCorner = true;
					this.isInLeftCorner = true;
				}
			}
			else
			{
				if (!this.shooterInput.isAimingByInput || (!this.inCornerAngle && this.isInLeftCornerPositionRange && !this.isInRightCornerPositionRange))
				{
					this.isAimingInCorner = false;
				}
				this.isInRightCorner = false;
				this.isInLeftCorner = false;
			}
			float num3;
			if ((this.isInRightCorner || this.isInLeftCorner) && this.shooterInput.isAimingByInput)
			{
				num3 = 1f;
			}
			else
			{
				num3 = 0f;
			}
			this.currentCornerWeight = Mathf.Lerp(this.currentCornerWeight, num3, this.cornerWeightSmooth * Time.deltaTime);
			if ((this.currentCornerWeight >= 0.9f || (this.shooterInput.isAimingByInput && !this.isInRightCornerPositionRange && !this.isInLeftCornerPositionRange)) && !this.wasAiming)
			{
				this.wasAiming = true;
			}
			else if (this.currentCornerWeight < 0.9f && this.wasAiming && !this.shooterInput.isAimingByInput)
			{
				this.wasAiming = false;
			}
			float num4 = this.localCharPosition.x;
			if (this.currentCornerWeight > 0.1f)
			{
				num4 = Mathf.Lerp(num4, this.helper.InverseTransformPoint(this.CornerAimingPosition).x, this.currentCornerWeight);
				this.localCharPosition = this.helper.InverseTransformPoint(base.transform.position);
				this.localCharPosition.x = num4;
				base.transform.position = this.helper.TransformPoint(this.localCharPosition);
			}
			base.transform.position = Vector3.Lerp(base.transform.position, this.helper.TransformPoint(this.localCharPosition), ((this.localCharPosition.z > this.PositionOffsetZ) ? 20f : 5f) * Time.deltaTime);
			float num5 = Mathf.Abs(vector.x) * 0.25f;
			vector.z = Mathf.Clamp(vector.z, -num5, num5);
			this.canMove = this.shooterInput.IsAiming || (vector.magnitude >= num5 * 4f && vector.x > 0f && this.side == vCoverController.Side.left) || (vector.x < 0f && this.side == vCoverController.Side.right);
			this.shooterInput.cc.moveDirection = this.helper.TransformDirection(vector) * (this.canMove ? 1f : 0f);
			Debug.DrawRay(base.transform.position + Vector3.up * 0.2f, this.shooterInput.cc.moveDirection, Color.green);
			if (!this.canMove || vector.magnitude == 0f)
			{
				this.shooterInput.cc.inputMagnitude = 0f;
			}
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000DD4AC File Offset: 0x000DB6AC
		protected virtual void ControlCameraState()
		{
			float num = (float)this.side;
			if (this.isInRightCornerPositionRange)
			{
				this.cameraSwtichLeft = 0f;
				this.cameraSwtichRight = Mathf.Clamp(this.cameraSwtichRight + Time.deltaTime * 10f, 0f, 1f);
				num += Mathf.Lerp(0f, this.isAimingInCorner ? this.cornerRightAiming_OffsetCameraRight : this.cornerRight_OffsetCameraRight, this.cameraSwtichRight);
			}
			else if (this.isInLeftCornerPositionRange)
			{
				this.cameraSwtichRight = 0f;
				this.cameraSwtichLeft = Mathf.Clamp(this.cameraSwtichLeft + Time.deltaTime * 10f, 0f, 1f);
				num -= Mathf.Lerp(0f, this.isAimingInCorner ? this.cornerLeftAiming_OffsetCameraRight : this.cornerLeft_OffsetCameraRight, this.cameraSwtichLeft);
			}
			else
			{
				this.cameraSwtichLeft = 0f;
				this.cameraSwtichRight = 0f;
			}
			this.shooterInput.tpCamera.switchRight = num;
			if (this.shooterInput.cc.isCrouching)
			{
				this.SetCameraState(this.shooterInput.IsAiming ? (this.aimingOverBarrier ? this.aimingOverBarrierCameraState : this.crouchAimingCameraState) : this.crouchCameraState);
				return;
			}
			this.SetCameraState(this.shooterInput.isAimingByInput ? this.standAimingCameraState : this.standCameraState);
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000DD618 File Offset: 0x000DB818
		protected virtual void ControlIKState()
		{
			if (!this.inCover)
			{
				return;
			}
			string targetIKPose = this.TargetIKPose;
			if (string.IsNullOrEmpty(targetIKPose) && !string.IsNullOrEmpty(this.shooterInput.CustomIKAdjustState))
			{
				this.currentIKPose = targetIKPose;
				this.shooterInput.ResetCustomIKAdjustState();
				return;
			}
			if (!string.IsNullOrEmpty(targetIKPose) && targetIKPose != this.currentIKPose)
			{
				this.currentIKPose = targetIKPose;
				this.shooterInput.SetCustomIKAdjustState(this.currentIKPose);
			}
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000DD690 File Offset: 0x000DB890
		protected virtual void ControlExitCoverByAngle()
		{
			if (!this.autoExitCover || this.shooterInput.cc.input.magnitude < 0.1f || this.goingToCornerPoint)
			{
				this.timeToExitByDirection = 0f;
				return;
			}
			if (Vector3.Angle(this.inputDirection, -this.helper.transform.forward) > this.exitInputDirectionAngle)
			{
				this.timeToExitByDirection += Time.deltaTime;
				if (this.timeToExitByDirection > this.exitDirectionTimer)
				{
					if (this.shooterInput.cc.IsAnimatorTag("CoverLocomotion"))
					{
						this.onExitCover.Invoke();
					}
					this.ExitCover(false);
					return;
				}
			}
			else
			{
				this.timeToExitByDirection = 0f;
			}
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000DD755 File Offset: 0x000DB955
		protected virtual void OnTriggerStay(Collider other)
		{
			if (!this.coverTag.Contains(other.gameObject.tag))
			{
				return;
			}
			this.CheckCoverTrigger(other);
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000DD778 File Offset: 0x000DB978
		protected virtual void OnTriggerExit(Collider other)
		{
			if (!this.coverTag.Contains(other.gameObject.tag))
			{
				return;
			}
			vCoverPoint component = other.gameObject.GetComponent<vCoverPoint>();
			if (component && this.coverPoints.Contains(component) && (!this.inCover || this.coverPoints.Count > 0))
			{
				this.coverPoints.Remove(component);
			}
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000DD7E4 File Offset: 0x000DB9E4
		protected virtual void CheckCoverTrigger(Collider other)
		{
			if (this.currentCoverPoint && other.gameObject == this.currentCoverPoint.gameObject)
			{
				return;
			}
			vCoverPoint component = other.gameObject.GetComponent<vCoverPoint>();
			if (component && !this.coverPoints.Contains(component))
			{
				this.coverPoints.Add(component);
			}
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000DD845 File Offset: 0x000DBA45
		protected virtual void SetCameraState(string state)
		{
			if (this.shooterInput.customCameraState != state)
			{
				this.shooterInput.ChangeCameraState(state, true);
			}
		}

		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x0600270B RID: 9995 RVA: 0x000DD868 File Offset: 0x000DBA68
		protected virtual string TargetIKPose
		{
			get
			{
				if (!this.inCover)
				{
					return string.Empty;
				}
				if (this.shooterInput.cc.isCrouching)
				{
					if (!this.shooterInput.IsAiming)
					{
						if (this.side != vCoverController.Side.left)
						{
							return this.ikRightCrouching;
						}
						return this.ikLeftCrouching;
					}
					else if (this.side != vCoverController.Side.left)
					{
						if (this.isAimingInCorner)
						{
							return this.ikRightCrouchingCornerAiming;
						}
						if (!this.aimingOverBarrier)
						{
							return this.ikRightCrouchingAiming;
						}
						return this.ikRightCrouchingBarrierAiming;
					}
					else
					{
						if (this.isAimingInCorner)
						{
							return this.ikLeftCrouchingCornerAiming;
						}
						if (!this.aimingOverBarrier)
						{
							return this.ikLeftCrouchingAiming;
						}
						return this.ikLeftCrouchingBarrierAiming;
					}
				}
				else if (!this.shooterInput.IsAiming)
				{
					if (this.side != vCoverController.Side.left)
					{
						return this.ikRightStanding;
					}
					return this.ikLeftStanding;
				}
				else if (this.side != vCoverController.Side.left)
				{
					if (!this.isAimingInCorner)
					{
						return this.ikRightStandingAiming;
					}
					return this.ikRightStandingCornerAiming;
				}
				else
				{
					if (!this.isAimingInCorner)
					{
						return this.ikLeftStandingAiming;
					}
					return this.ikLeftStandingCornerAiming;
				}
			}
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000DD968 File Offset: 0x000DBB68
		protected virtual void ControlNearCoverPoint()
		{
			if (this.goingToCornerPoint || this.currentCornerWeight > 0.1f)
			{
				return;
			}
			vCoverPoint nearCoverPoint = this.GetNearCoverPoint();
			if (nearCoverPoint)
			{
				this.currentCoverPoint = nearCoverPoint;
				return;
			}
			if (this.currentCoverPoint && Vector3.Distance(base.transform.position, this.currentCoverPoint.boxCollider.ClosestPoint(base.transform.position)) > 0.5f && this.inCover)
			{
				if (this.shooterInput.cc.IsAnimatorTag("CoverLocomotion"))
				{
					this.onExitCover.Invoke();
				}
				this.ExitCover(false);
			}
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x000DDA14 File Offset: 0x000DBC14
		private vCoverPoint GetNearCoverPoint()
		{
			float num = (this.currentCoverPoint ? (this.currentCoverPoint.posePosition - base.transform.position).magnitude : 10f);
			vCoverPoint vCoverPoint = this.currentCoverPoint;
			this.coverPoints = this.coverPoints.FindAll((vCoverPoint c) => (c.posePosition - base.transform.position).magnitude < 10f);
			for (int i = 0; i < this.coverPoints.Count; i++)
			{
				float magnitude = (base.transform.position - this.coverPoints[i].posePosition).magnitude;
				if (magnitude < num)
				{
					vCoverPoint = this.coverPoints[i];
					num = magnitude + 0.1f;
				}
			}
			return vCoverPoint;
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x000DDADC File Offset: 0x000DBCDC
		protected virtual void PrepareToMoveOnPath(float pathLenght, bool crouching = false)
		{
			this.shooterInput.animator.SetLayerWeight(this.animatorCoverCornerLayer, 0f);
			this.shooterInput.cc.lockSetMoveSpeed = false;
			this.shooterInput.SetLockUpdateMoveDirection(true);
			this.shooterInput.SetLockAllInput(true);
			this.shooterInput.SetWalkByDefault(false);
			this.shooterInput.CancelAiming();
			this.shooterInput.cc.isStrafing = false;
			this.shooterInput.lockCameraInput = true;
			this.shooterInput.lockUpdateMoveDirection = true;
			this.shooterInput.lockMoveInput = true;
			this.shooterInput.cc.isSprinting = pathLenght > 3f;
			this.shooterInput.cc.isCrouching = crouching;
			this.shooterInput.cc.lockRotation = false;
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x000DDBB4 File Offset: 0x000DBDB4
		protected virtual void CheckOverBarrier()
		{
			this.enableHipFireOverBarrier = Mathf.Abs(this.cameraAngle) < 90f;
			if (this.enableHipFireOverBarrier)
			{
				this.shooterInput.shooterManager.hipfireShot = this.shooterInput.IsCrouching && !this.isInLeftCornerPositionRange && !this.isInRightCornerPositionRange && this.useHipFireOverBarrier;
				this.shooterInput.shooterManager.hipfireAimTime = this.hipFireTimer;
			}
			else
			{
				this.shooterInput.shooterManager.hipfireAimTime = this.lastHipFireTimer;
				this.shooterInput.shooterManager.hipfireShot = this.lastUseHipFire;
			}
			this.aimingOverBarrier = this.useHipFireOverBarrier && this.enableHipFireOverBarrier && !this.shooterInput.isAimingByInput && this.shooterInput.isAimingByHipFire;
			this.overBarrierWeight = Mathf.Lerp(this.overBarrierWeight, this.aimingOverBarrier ? 1f : 0f, Time.deltaTime * (this.aimingOverBarrier ? 50f : 5f));
			this.shooterInput.animator.SetLayerWeight(this.animatorCoverOverBarrierLayer, this.overBarrierWeight * (1f - this.currentCornerWeight));
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x000DDCF4 File Offset: 0x000DBEF4
		private void UpdateCheckAimPoints(ref float startX, ref float endX, ref float startY, ref float endY)
		{
			if (this.enableHipFireOverBarrier && this.shooterInput.IsCrouching)
			{
				startX += this.checkAimOverBarrierOffsetStartX;
				startY += this.checkAimOverBarrierOffsetStartY;
				endX += this.checkAimOverBarrierOffsetEndX;
				endY += this.checkAimOverBarrierOffsetEndY;
			}
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x000DDD44 File Offset: 0x000DBF44
		protected virtual void ResetCoverCornerUI()
		{
			if (this.currentTimeToEnterCorner > 0f)
			{
				this.currentTimeToEnterCorner = 0f;
			}
			if (this.cornerRightUI && this.cornerRightUI.gameObject.activeSelf)
			{
				this.cornerRightUI.gameObject.SetActive(false);
			}
			if (this.cornerLeftUI && this.cornerLeftUI.gameObject.activeSelf)
			{
				this.cornerLeftUI.gameObject.SetActive(false);
			}
			if (this.cornerInputUI && this.cornerInputUI.gameObject.activeSelf)
			{
				this.cornerInputUI.SetActive(false);
			}
			this.isInCornerInputAction = false;
		}

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06002712 RID: 10002 RVA: 0x000DDDFC File Offset: 0x000DBFFC
		protected virtual Vector3 CornerAimingPosition
		{
			get
			{
				float num = this.PositionOffsetZ;
				float xlocalPositionAimingInCorner = this.XLocalPositionAimingInCorner;
				return this.helper.position + this.helper.forward * num + this.helper.right * -xlocalPositionAimingInCorner * (float)this.side;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06002713 RID: 10003 RVA: 0x000DDE5B File Offset: 0x000DC05B
		protected float XLocalClampedInCorner
		{
			get
			{
				return (float)Math.Round((double)(this.currentCoverPoint.boxCollider.size.x * 0.5f - this.shooterInput.cc.colliderRadiusDefault), 2);
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002714 RID: 10004 RVA: 0x000DDE94 File Offset: 0x000DC094
		protected float XLocalPositionAimingInCorner
		{
			get
			{
				float num = this.XLocalClampedInCorner;
				num += ((this.side == vCoverController.Side.right) ? this.cornerRightAiming_OffsetPosition : this.cornerLeftAiming_OffsetPosition);
				return Mathf.Max(0f, num);
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06002715 RID: 10005 RVA: 0x000DDED0 File Offset: 0x000DC0D0
		public float CornerUIHeight
		{
			get
			{
				float num = Vector3.Distance(base.transform.position, this.shooterInput.cameraMain.transform.position);
				Vector3 vector = this.shooterInput.cameraMain.transform.position + this.shooterInput.cameraMain.transform.forward * num;
				vector = base.transform.InverseTransformPoint(vector);
				vector.x = 0f;
				vector.z = 0f;
				return Mathf.Clamp(vector.y, 0f, this.shooterInput.cc._capsuleCollider.height);
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06002716 RID: 10006 RVA: 0x000DDF83 File Offset: 0x000DC183
		protected virtual float PositionOffsetZ
		{
			get
			{
				return (this.inCover ? this.shooterInput.cc._capsuleCollider.radius : this.capsuleRadiusInCover) + this.positionOffsetZ;
			}
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x000DDFB4 File Offset: 0x000DC1B4
		protected virtual float PathLenght(List<Vector3> movePath)
		{
			float num = 0f;
			Vector3 vector = base.transform.position;
			for (int i = 0; i < movePath.Count; i++)
			{
				float num2 = Vector3.Distance(vector, movePath[i]);
				num += num2;
				if (this.debug)
				{
					Debug.DrawLine(vector, movePath[i], Color.green, 10f);
				}
				vector = movePath[i];
			}
			return num;
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000DE020 File Offset: 0x000DC220
		protected virtual bool CheckCoverHeight(vCoverPoint coverPoint, bool checkAimingHeight)
		{
			bool flag = !this.inCover;
			Vector3 forward = this.shooterInput.cameraMain.transform.forward;
			forward.y = 0f;
			Vector3 vector = (flag ? coverPoint.posePosition : this.helper.position);
			Vector3 forward2 = coverPoint.transform.forward;
			Vector3 vector2 = (flag ? (vector + Vector3.up * this.crouchHeight) : (vector + Vector3.up * this.crouchHeight + forward2 * 0.5f));
			Ray ray = new Ray(vector2, -forward2.normalized);
			int num = 0;
			bool flag2 = Physics.SphereCast(ray, this.crouchRayRadius, out this.checkHeightHit, this.crouchRayDistance, this.obstaclesLayer, QueryTriggerInteraction.Ignore) && !this.CheckIgnoreHeight(this.checkHeightHit.collider);
			bool flag3 = Physics.SphereCast(vector2 + coverPoint.transform.right * (this.shooterInput.cc.colliderRadiusDefault * 0.5f), this.crouchRayRadius, -forward2.normalized, out this.checkHeightHit, this.crouchRayDistance, this.obstaclesLayer, QueryTriggerInteraction.Ignore) && !this.CheckIgnoreHeight(this.checkHeightHit.collider);
			bool flag4 = Physics.SphereCast(vector2 - coverPoint.transform.right * (this.shooterInput.cc.colliderRadiusDefault * 0.5f), this.crouchRayRadius, -forward2.normalized, out this.checkHeightHit, this.crouchRayDistance, this.obstaclesLayer, QueryTriggerInteraction.Ignore) && !this.CheckIgnoreHeight(this.checkHeightHit.collider);
			if (!flag2)
			{
				num++;
				if (this.debug)
				{
					Debug.DrawRay(vector2, -forward2.normalized * this.crouchRayDistance, Color.red);
				}
			}
			else if (this.debug)
			{
				Debug.DrawRay(vector2, -forward2.normalized * this.crouchRayDistance, Color.green);
			}
			if (!flag3)
			{
				num++;
				if (this.debug)
				{
					Debug.DrawRay(vector2 + coverPoint.transform.right * (this.shooterInput.cc.colliderRadiusDefault * 0.5f), -forward2.normalized * this.crouchRayDistance, Color.red);
				}
			}
			else if (this.debug)
			{
				Debug.DrawRay(vector2 + coverPoint.transform.right * (this.shooterInput.cc.colliderRadiusDefault * 0.5f), -forward2.normalized * this.crouchRayDistance, Color.green);
			}
			if (!flag4)
			{
				num++;
				if (this.debug)
				{
					Debug.DrawRay(vector2 - coverPoint.transform.right * (this.shooterInput.cc.colliderRadiusDefault * 0.5f), -forward2.normalized * this.crouchRayDistance, Color.red);
				}
			}
			else if (this.debug)
			{
				Debug.DrawRay(vector2 - coverPoint.transform.right * (this.shooterInput.cc.colliderRadiusDefault * 0.5f), -forward2.normalized * this.crouchRayDistance, Color.green);
			}
			bool flag5 = num > 1;
			if (flag5 && checkAimingHeight)
			{
				Vector3 position = base.transform.position;
				Vector3 vector3 = -this.helper.transform.right * (float)this.side * (this.shooterInput.cc.colliderRadiusDefault + this.crouchRayOffsetInCorner);
				ray = new Ray(position + Vector3.up * this.crouchHeightAiming + vector3, forward.normalized);
				Vector3 vector4 = Quaternion.AngleAxis(90f, Vector3.up) * ray.direction;
				if (Physics.Linecast(position, ray.origin + vector4.normalized * this.crouchRayRadius * 2f, out this.checkHeightHit, this.obstaclesLayer))
				{
					ray.origin = this.checkHeightHit.point - vector4.normalized * this.crouchRayRadius * 2f;
				}
				if (this.debug)
				{
					Debug.DrawRay(ray.origin + vector4.normalized * this.crouchRayRadius, ray.direction * this.crouchRayDistance);
				}
				if (this.debug)
				{
					Debug.DrawRay(ray.origin - vector4.normalized * this.crouchRayRadius, ray.direction * this.crouchRayDistance);
				}
				flag5 = !Physics.SphereCast(ray, this.crouchRayRadius, this.crouchRayDistance, this.obstaclesLayer, QueryTriggerInteraction.Ignore);
			}
			if (!flag)
			{
				if (!this.shooterInput.IsAiming && !flag5 && this.shooterInput.cc.isCrouching)
				{
					if (this.currentTimeToGetUp >= this.timeToStandUp)
					{
						this.currentTimeToGetUp = 0f;
					}
					else
					{
						flag5 = true;
					}
					this.currentTimeToGetUp += Time.deltaTime;
				}
				else
				{
					this.currentTimeToGetUp = 0f;
				}
			}
			return flag5;
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000DE5EA File Offset: 0x000DC7EA
		protected virtual bool CheckIgnoreHeight(Collider target)
		{
			return this.checkHeightIgnoreTags.Contains(target.gameObject.tag) || this.checkHeightIgnoreNames.Contains(target.gameObject.name);
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x000D8DDD File Offset: 0x000D6FDD
		protected virtual bool IsLeftCoverCorner(vCoverPoint coverPoint)
		{
			return (coverPoint.corner & vCoverPoint.Corner.Left) > (vCoverPoint.Corner)0;
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x000D8DEA File Offset: 0x000D6FEA
		protected virtual bool IsRightCoverCorner(vCoverPoint coverPoint)
		{
			return (coverPoint.corner & vCoverPoint.Corner.Right) > (vCoverPoint.Corner)0;
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x000DE61C File Offset: 0x000DC81C
		protected virtual bool IsCoverCorner(vCoverPoint coverPoint)
		{
			return this.IsRightCoverCorner(coverPoint) || this.IsLeftCoverCorner(coverPoint);
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000DE630 File Offset: 0x000DC830
		protected virtual bool CheckAngleOfDirections(Vector3 directionA, Vector3 directionB, float value)
		{
			return Vector3.Angle(directionA, directionB) <= value;
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000DE640 File Offset: 0x000DC840
		protected virtual bool IsLookingToCorner()
		{
			double num = Math.Round((double)(this.helper.InverseTransformDirection(this.inputDirection).x * -1f), 1);
			return Vector3.Angle(-this.currentCoverPoint.transform.right * (float)this.side, base.transform.forward) < this.minAngleToStartMoveToCorner && ((this.isInLeftCornerPositionRange && num < -0.20000000298023224) || (this.isInRightCornerPositionRange && num > 0.20000000298023224)) && this.cameraAngle * (float)this.side < this.angleToEnterCorner;
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x000DE6EC File Offset: 0x000DC8EC
		protected virtual bool EnterCoverConditions()
		{
			return !this.shooterInput.cc.animator.IsInTransition(this.shooterInput.cc.baseLayer) && !this.shooterInput.cc.customAction && !this.shooterInput.cc.isJumping && !this.shooterInput.isAimingByInput && !this.isRolling;
		}

		// Token: 0x06002720 RID: 10016 RVA: 0x000DE75C File Offset: 0x000DC95C
		protected virtual bool ExitByActions()
		{
			return !this.shooterInput.animator.IsInTransition(0) && ((this.shooterInput.cc.customAction && !this.goingToCoverPoint) || this.isRolling || this.shooterInput.cc.isJumping || this.shooterInput.cc.ragdolled || this.shooterInput.cc.isDead);
		}

		// Token: 0x040026EC RID: 9964
		[vEditorToolbar("Cover Settings", false, "", false, false)]
		[vSeparator("Enter/Exit Cover Input", "")]
		public GenericInput enterExitInput = new GenericInput("Space", "A", "A");

		// Token: 0x040026ED RID: 9965
		[Tooltip("Check this option to automatically enter cover mode when close to a cover point without input and within the EnterInputDirectionAngle limit.")]
		public bool autoEnterCover;

		// Token: 0x040026EE RID: 9966
		[vHideInInspector("autoEnterCover", false)]
		[Tooltip("Angle of Input direction to enter cover")]
		public float enterInputDirectionAngle = 60f;

		// Token: 0x040026EF RID: 9967
		[vHideInInspector("autoEnterCover", false)]
		[Tooltip("Time to auto enter when enterDirectionAngle is reached")]
		public float enterDirectionTimer = 0.25f;

		// Token: 0x040026F0 RID: 9968
		[Tooltip("Auto Exit Cover based on a Input Direction")]
		public bool autoExitCover;

		// Token: 0x040026F1 RID: 9969
		[vHideInInspector("autoExitCover", false)]
		[Tooltip("Angle of Input direction to exit cover")]
		public float exitInputDirectionAngle = 135f;

		// Token: 0x040026F2 RID: 9970
		[vHideInInspector("autoExitCover", false)]
		[Tooltip("Timer to exit when Exit direction angle is reached")]
		public float exitDirectionTimer = 0.25f;

		// Token: 0x040026F3 RID: 9971
		[vSeparator("Get Cover Settings", "")]
		[Range(1f, 25f)]
		[Tooltip("Distance to get cover point to enter in cover\nOnly work with Auto Enter disabled")]
		public float maxEnterCoverDistance = 4f;

		// Token: 0x040026F4 RID: 9972
		[Tooltip("The minimum distance to cancel moving to a CoverPoint if you release the input")]
		public float minDistanceToCancelTravel = 2.5f;

		// Token: 0x040026F5 RID: 9973
		[Tooltip("Enable to Detect distante CoverPoints and Travel to it *NAVMESH REQUIRED")]
		public bool autoTravelToNextCover = true;

		// Token: 0x040026F6 RID: 9974
		[vMinMax(1f, 30f)]
		[Tooltip("Min Max Distance to get a new cover point when it's in cover\n *The Min Value is ignored if is in cover and next Cover is not connected to current cover line\nWorks with autoTravelToNextCover enabled")]
		public Vector2 nextCoverDistance = new Vector2(4f, 10f);

		// Token: 0x040026F7 RID: 9975
		[Tooltip("Creates more Raycasts within a open angle, Turn on DEBUG to see it")]
		[Range(0f, 4f)]
		public int rayCastAnglePass = 3;

		// Token: 0x040026F8 RID: 9976
		[Range(1f, 10f)]
		[Tooltip("Multiplies the raycastAnglePass into several casts, Turn on DEBUG to see it")]
		public float rayCastAngleMultiplier = 2f;

		// Token: 0x040026F9 RID: 9977
		[Tooltip("Capsule Collider Radius while in Cover")]
		public float capsuleRadiusInCover = 0.2f;

		// Token: 0x040026FA RID: 9978
		[Tooltip("The minimum angle accepted to be a valid corner")]
		[Range(15f, 90f)]
		public float minAngleOfValidCover = 80f;

		// Token: 0x040026FB RID: 9979
		[Tooltip("Distance OffsetZ of the Controller from the Cover")]
		public float positionOffsetZ;

		// Token: 0x040026FC RID: 9980
		[vSeparator("Slide Settings", "")]
		[Tooltip("Check to use a Slide animation when switching between covers using the AutoTravel")]
		public bool useSlideToCover = true;

		// Token: 0x040026FD RID: 9981
		[Tooltip("Min and Max distance to Slide")]
		[vMinMax(2f, 5f)]
		public Vector2 minPathLenghToSlide = new Vector2(2f, 5f);

		// Token: 0x040026FE RID: 9982
		[vSeparator("Side Settings", "")]
		[vToggleOption("Change Side Method", "Only Input Direction", "Change By Angle and Input")]
		public bool changeSide = true;

		// Token: 0x040026FF RID: 9983
		public Vector2 angleToChangeSide = new Vector3(65f, 180f);

		// Token: 0x04002700 RID: 9984
		[vToggleOption("Change Side Method Aiming", "Only Input Direction", "Change By Angle and Input")]
		public bool changeSideAiming = true;

		// Token: 0x04002701 RID: 9985
		[vMinMax(minLimit = 45f, maxLimit = 180f)]
		public Vector2 angleToChangeSideOnAiming = new Vector3(45f, 135f);

		// Token: 0x04002702 RID: 9986
		[vMinMax(minLimit = -180f, maxLimit = 180f)]
		[Tooltip("Angle to keep in corner state pose when aiming")]
		public Vector2 cornerAimingAngle = new Vector2(-15f, 90f);

		// Token: 0x04002703 RID: 9987
		[vSeparator("Crouch Settings", "")]
		[Tooltip("Standup when exiting a Cover Crouched or Keep it crouched")]
		public bool alwaysStandUpOnExit = true;

		// Token: 0x04002704 RID: 9988
		[Range(0.01f, 0.2f)]
		[Tooltip("Radius to verify if the character will be on crouch, Turn on DEBUG to see it")]
		public float crouchRayRadius = 0.05f;

		// Token: 0x04002705 RID: 9989
		[Tooltip("Distance to verify if the character will be on crouch, Turn on DEBUG to see it")]
		public float crouchRayDistance = 0.5f;

		// Token: 0x04002706 RID: 9990
		[Tooltip("Height to verify if the character will be on crouch, Turn on DEBUG to see it")]
		public float crouchHeight = 1.5f;

		// Token: 0x04002707 RID: 9991
		[Tooltip("Height to verify if the character will be on crouch and aiming, Turn on DEBUG to see it")]
		public float crouchHeightAiming = 1f;

		// Token: 0x04002708 RID: 9992
		public float crouchRayOffsetInCorner = 0.25f;

		// Token: 0x04002709 RID: 9993
		public float timeToStandUp = 0.5f;

		// Token: 0x0400270A RID: 9994
		[vSeparator("Corner Settings", "")]
		[Tooltip("Check to use the GetCoverInput to move on corners, Keep it False to just press the input direction to do corners,  this will also change the Corner UI")]
		public bool enterUsingGetCoverInput;

		// Token: 0x0400270B RID: 9995
		[Tooltip("Angle to Enter a Corner")]
		public float angleToEnterCorner = -10f;

		// Token: 0x0400270C RID: 9996
		[Tooltip("Minimum angle to start moving to a corner")]
		public float minAngleToStartMoveToCorner = 10f;

		// Token: 0x0400270D RID: 9997
		[Tooltip("Delay to do Corner")]
		public float timeToEnterCorner = 0.5f;

		// Token: 0x0400270E RID: 9998
		[Tooltip("Controller Offset Position when Aiming on a Corner")]
		public float cornerLeftAiming_OffsetPosition = 0.6f;

		// Token: 0x0400270F RID: 9999
		[Tooltip("Controller Offset Position when Aiming on a Corner")]
		public float cornerRightAiming_OffsetPosition = 0.4f;

		// Token: 0x04002710 RID: 10000
		[Tooltip("Smooth when performing a Corner")]
		public float cornerWeightSmooth = 20f;

		// Token: 0x04002711 RID: 10001
		[vSeparator("Shot Over Barrier *Experimental*", "")]
		public bool useHipFireOverBarrier;

		// Token: 0x04002712 RID: 10002
		public float hipFireTimer;

		// Token: 0x04002713 RID: 10003
		public float checkAimOverBarrierOffsetStartY = 0.5f;

		// Token: 0x04002714 RID: 10004
		public float checkAimOverBarrierOffsetStartX;

		// Token: 0x04002715 RID: 10005
		public float checkAimOverBarrierOffsetEndY = 0.5f;

		// Token: 0x04002716 RID: 10006
		public float checkAimOverBarrierOffsetEndX;

		// Token: 0x04002717 RID: 10007
		[vSeparator("Generic Actions Settings", "")]
		public List<string> possibleActions = new List<string> { "Action" };

		// Token: 0x04002718 RID: 10008
		public Vector3 checkActionMargin;

		// Token: 0x04002719 RID: 10009
		public Vector3 checkActionOffset;

		// Token: 0x0400271A RID: 10010
		public bool showActionGizmos;

		// Token: 0x0400271B RID: 10011
		[vEditorToolbar("Movement Speed", false, "", false, false)]
		[vSeparator("Movement Speed", "")]
		public float crouchedMoveSpeed = 2f;

		// Token: 0x0400271C RID: 10012
		public float standingMoveSpeed = 2f;

		// Token: 0x0400271D RID: 10013
		public float goToCoverPointRunningMoveSpeed = 2f;

		// Token: 0x0400271E RID: 10014
		public float goToCoverPointSprintingMoveSpeed = 2f;

		// Token: 0x0400271F RID: 10015
		public float goToCornerCrouchedMoveSpeed = 2f;

		// Token: 0x04002720 RID: 10016
		public float goToCornerStandingMoveSpeed = 2f;

		// Token: 0x04002721 RID: 10017
		[Tooltip("Rotation Speed of the Controller while in Cover")]
		public float rotationSpeedMoving = 5f;

		// Token: 0x04002722 RID: 10018
		public float rotationSpeedIdle = 10f;

		// Token: 0x04002723 RID: 10019
		[vSeparator("Animation Speed", "")]
		public float standAnimatorSpeed = 1f;

		// Token: 0x04002724 RID: 10020
		public float crouchAnimatorSpeed = 1f;

		// Token: 0x04002725 RID: 10021
		public float goToCoverPointRunningAnimatorSpeed = 1f;

		// Token: 0x04002726 RID: 10022
		public float goToCoverPointSprintingAnimatorSpeed = 1f;

		// Token: 0x04002727 RID: 10023
		public float goToCornerPointAnimatorSpeed = 1f;

		// Token: 0x04002728 RID: 10024
		private LayerMask oldStopMoveMask;

		// Token: 0x04002729 RID: 10025
		[vEditorToolbar("IK Pose", false, "", false, false)]
		public string ikLeftStanding;

		// Token: 0x0400272A RID: 10026
		public string ikLeftStandingAiming;

		// Token: 0x0400272B RID: 10027
		public string ikLeftStandingCornerAiming;

		// Token: 0x0400272C RID: 10028
		public string ikLeftCrouching;

		// Token: 0x0400272D RID: 10029
		public string ikLeftCrouchingAiming;

		// Token: 0x0400272E RID: 10030
		public string ikLeftCrouchingBarrierAiming;

		// Token: 0x0400272F RID: 10031
		public string ikLeftCrouchingCornerAiming;

		// Token: 0x04002730 RID: 10032
		public string ikRightStanding;

		// Token: 0x04002731 RID: 10033
		public string ikRightStandingAiming;

		// Token: 0x04002732 RID: 10034
		public string ikRightStandingCornerAiming;

		// Token: 0x04002733 RID: 10035
		public string ikRightCrouching;

		// Token: 0x04002734 RID: 10036
		public string ikRightCrouchingAiming;

		// Token: 0x04002735 RID: 10037
		public string ikRightCrouchingBarrierAiming;

		// Token: 0x04002736 RID: 10038
		public string ikRightCrouchingCornerAiming;

		// Token: 0x04002737 RID: 10039
		[vEditorToolbar("Layers and Tags", false, "", false, false)]
		public LayerMask obstaclesLayer;

		// Token: 0x04002738 RID: 10040
		[Tooltip("Tags to be ignored when checking if need to crouch or standup")]
		public vTagMask checkHeightIgnoreTags = new vTagMask(new string[] { "Glass" });

		// Token: 0x04002739 RID: 10041
		[Tooltip("GameObject Names to be ignored when checking if need to crouch or standup")]
		public List<string> checkHeightIgnoreNames = new List<string>();

		// Token: 0x0400273A RID: 10042
		public LayerMask coverLayer;

		// Token: 0x0400273B RID: 10043
		public vTagMask coverTag;

		// Token: 0x0400273C RID: 10044
		[vEditorToolbar("Cover UI", false, "", false, false)]
		public Color routeLineColor = Color.grey;

		// Token: 0x0400273D RID: 10045
		[Range(0f, 2f)]
		public float drawRouteDelayTime;

		// Token: 0x0400273E RID: 10046
		public LineRenderer routeLine;

		// Token: 0x0400273F RID: 10047
		public int routeLineSmoothPass = 2;

		// Token: 0x04002740 RID: 10048
		public float routeLineSmoothFactor = 0.2f;

		// Token: 0x04002741 RID: 10049
		[Range(0.1f, 10f)]
		public float coverIndicatorDistance = 5f;

		// Token: 0x04002742 RID: 10050
		public float coverIndicaterHeight = 1f;

		// Token: 0x04002743 RID: 10051
		public GameObject coverUI;

		// Token: 0x04002744 RID: 10052
		public GameObject cornerUICenter;

		// Token: 0x04002745 RID: 10053
		public GameObject cornerLeftUI;

		// Token: 0x04002746 RID: 10054
		public GameObject cornerRightUI;

		// Token: 0x04002747 RID: 10055
		public GameObject cornerInputUI;

		// Token: 0x04002748 RID: 10056
		public Slider.SliderEvent onUpdateCornerInput;

		// Token: 0x04002749 RID: 10057
		[vEditorToolbar("Animation States", false, "", false, false)]
		[vSeparator("Default Locomotion", "")]
		public string defaultFreeStanding = "Free Movement";

		// Token: 0x0400274A RID: 10058
		public string defaultFreeCrouched = "Free Crouch";

		// Token: 0x0400274B RID: 10059
		public string defaultStrafeStanding = "Strafing Movement";

		// Token: 0x0400274C RID: 10060
		public string defaultStrafeCrouched = "Strafing Crouch";

		// Token: 0x0400274D RID: 10061
		[vSeparator("Cover Locomotion", "")]
		public string goToCoverPoint = "Go To Cover Point";

		// Token: 0x0400274E RID: 10062
		public string startSlideToCoverPoint = "Start Slide To Cover";

		// Token: 0x0400274F RID: 10063
		public string finishSlideToCoverPoint = "Finish Slide To Cover";

		// Token: 0x04002750 RID: 10064
		public string coverStandingLeft = "Cover Stand Left";

		// Token: 0x04002751 RID: 10065
		public string coverStandingRight = "Cover Stand Right";

		// Token: 0x04002752 RID: 10066
		public string coverCrouchedLeft = "Cover Crouch Left";

		// Token: 0x04002753 RID: 10067
		public string coverCrouchedRight = "Cover Crouch Right";

		// Token: 0x04002754 RID: 10068
		[vSeparator("Cross Fade Transition", "")]
		[Range(0.1f, 0.5f)]
		public float enterCoverAnimCrossfade = 0.25f;

		// Token: 0x04002755 RID: 10069
		[Range(0.1f, 0.5f)]
		public float exitCoverAnimCrossfade = 0.25f;

		// Token: 0x04002756 RID: 10070
		[Range(0.1f, 0.5f)]
		public float goToCoverAnimCrossfade = 0.25f;

		// Token: 0x04002757 RID: 10071
		[Range(0.1f, 0.5f)]
		public float startSlideToCoverAnimCrossfade = 0.25f;

		// Token: 0x04002758 RID: 10072
		[Range(0.1f, 0.5f)]
		public float finishSlideToCoverAnimCrossfade = 0.25f;

		// Token: 0x04002759 RID: 10073
		[Range(0.1f, 1f)]
		public float finishSlideToCoverWaitTime = 0.5f;

		// Token: 0x0400275A RID: 10074
		[vEditorToolbar("Camera Settings", false, "", false, false)]
		[vSeparator("Camera Offset Right", "")]
		public float cornerLeft_OffsetCameraRight = 1f;

		// Token: 0x0400275B RID: 10075
		public float cornerRight_OffsetCameraRight = 1f;

		// Token: 0x0400275C RID: 10076
		public float cornerLeftAiming_OffsetCameraRight = 1f;

		// Token: 0x0400275D RID: 10077
		public float cornerRightAiming_OffsetCameraRight = 1f;

		// Token: 0x0400275E RID: 10078
		[vSeparator("Moving to Next Cover CameraState", "")]
		public string goToCoverCameraState = "GoToCover";

		// Token: 0x0400275F RID: 10079
		[vSeparator("Stand Cover CameraState", "")]
		public string standCameraState = "CoverStand";

		// Token: 0x04002760 RID: 10080
		public string standAimingCameraState = "CoverStandAiming";

		// Token: 0x04002761 RID: 10081
		[vSeparator("Crouch Cover CameraState", "")]
		public string crouchCameraState = "CoverCrouch";

		// Token: 0x04002762 RID: 10082
		public string crouchAimingCameraState = "CoverCrouchAiming";

		// Token: 0x04002763 RID: 10083
		[vSeparator("HipFire Cover CameraState", "")]
		public string aimingOverBarrierCameraState;

		// Token: 0x04002764 RID: 10084
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent onEnterCover;

		// Token: 0x04002765 RID: 10085
		public UnityEvent onExitCover;

		// Token: 0x04002766 RID: 10086
		public UnityEvent onStartGoToCoverPoint;

		// Token: 0x04002767 RID: 10087
		public UnityEvent onFinishGoToCoverPoint;

		// Token: 0x04002768 RID: 10088
		[vEditorToolbar("Debug", false, "", false, false)]
		public bool debug;

		// Token: 0x04002769 RID: 10089
		[vReadOnly(true)]
		public bool inCover;

		// Token: 0x0400276A RID: 10090
		[vReadOnly(true)]
		public bool wasInCover;

		// Token: 0x0400276B RID: 10091
		[vReadOnly(true)]
		public bool goingToCoverPoint;

		// Token: 0x0400276C RID: 10092
		[vReadOnly(true)]
		public bool goingToCornerPoint;

		// Token: 0x0400276D RID: 10093
		[vReadOnly(true)]
		[SerializeField]
		public float currentTimeToEnterCorner;

		// Token: 0x0400276E RID: 10094
		[vReadOnly(true)]
		public vCoverPoint currentCoverPoint;

		// Token: 0x0400276F RID: 10095
		[vReadOnly(true)]
		public vCoverPoint possibleCoverPoint;

		// Token: 0x04002770 RID: 10096
		[vReadOnly(true)]
		public vCoverPoint possibleCornerPoint;

		// Token: 0x04002771 RID: 10097
		[vReadOnly(true)]
		[SerializeField]
		protected float drawRouteTimer;

		// Token: 0x04002772 RID: 10098
		[vReadOnly(true)]
		[SerializeField]
		protected vCoverController.Side side = vCoverController.Side.right;

		// Token: 0x04002773 RID: 10099
		protected int animatorCoverCornerLayer;

		// Token: 0x04002774 RID: 10100
		protected int animatorCoverOverBarrierLayer;

		// Token: 0x04002775 RID: 10101
		protected float remainingDistanceToNextCover;

		// Token: 0x04002776 RID: 10102
		protected float movementAngle;

		// Token: 0x04002777 RID: 10103
		protected float cameraAngle;

		// Token: 0x04002778 RID: 10104
		[SerializeField]
		[vReadOnly(true)]
		protected float currentCornerWeight;

		// Token: 0x04002779 RID: 10105
		protected float cameraSwtichRight;

		// Token: 0x0400277A RID: 10106
		protected float cameraSwtichLeft;

		// Token: 0x0400277B RID: 10107
		protected float overBarrierWeight;

		// Token: 0x0400277C RID: 10108
		[SerializeField]
		[vReadOnly(true)]
		protected float currentTimeToGetUp;

		// Token: 0x0400277D RID: 10109
		protected float timeToEnterByDirection;

		// Token: 0x0400277E RID: 10110
		protected float timeToExitByDirection;

		// Token: 0x0400277F RID: 10111
		protected float lastHipFireTimer;

		// Token: 0x04002780 RID: 10112
		protected float timeToExit;

		// Token: 0x04002781 RID: 10113
		protected bool lastUseHipFire;

		// Token: 0x04002782 RID: 10114
		protected bool enterCoverFromLeftSide;

		// Token: 0x04002783 RID: 10115
		protected bool isInCornerInputAction;

		// Token: 0x04002784 RID: 10116
		protected bool inCornerAdjustPosition;

		// Token: 0x04002785 RID: 10117
		protected bool holdingCornerInput;

		// Token: 0x04002786 RID: 10118
		protected bool originalWalkByDefault;

		// Token: 0x04002787 RID: 10119
		protected bool lastMoveToDirectionInFree;

		// Token: 0x04002788 RID: 10120
		[SerializeField]
		[vReadOnly(true)]
		protected bool isInLeftCornerPositionRange;

		// Token: 0x04002789 RID: 10121
		[SerializeField]
		[vReadOnly(true)]
		protected bool isInRightCornerPositionRange;

		// Token: 0x0400278A RID: 10122
		[SerializeField]
		[vReadOnly(true)]
		protected bool isLeftCorner;

		// Token: 0x0400278B RID: 10123
		[SerializeField]
		[vReadOnly(true)]
		protected bool isRightCorner;

		// Token: 0x0400278C RID: 10124
		[SerializeField]
		[vReadOnly(true)]
		protected bool isInLeftCorner;

		// Token: 0x0400278D RID: 10125
		[SerializeField]
		[vReadOnly(true)]
		protected bool isInRightCorner;

		// Token: 0x0400278E RID: 10126
		[SerializeField]
		[vReadOnly(true)]
		protected bool inCornerAngle;

		// Token: 0x0400278F RID: 10127
		[SerializeField]
		[vReadOnly(true)]
		protected bool enableHipFireOverBarrier;

		// Token: 0x04002790 RID: 10128
		[SerializeField]
		[vReadOnly(true)]
		protected bool aimingOverBarrier;

		// Token: 0x04002791 RID: 10129
		[SerializeField]
		[vReadOnly(true)]
		protected bool wasAiming;

		// Token: 0x04002792 RID: 10130
		[SerializeField]
		[vReadOnly(true)]
		protected bool isAimingInCorner;

		// Token: 0x04002793 RID: 10131
		[SerializeField]
		[vReadOnly(true)]
		protected bool canMove;

		// Token: 0x04002794 RID: 10132
		[SerializeField]
		[vReadOnly(true)]
		protected string currentIKPose;

		// Token: 0x04002795 RID: 10133
		protected string routeLineColorName = "_TintColor";

		// Token: 0x04002796 RID: 10134
		protected Vector3 coverIndicatorPosition;

		// Token: 0x04002797 RID: 10135
		protected Vector3 coverMoveDirection;

		// Token: 0x04002798 RID: 10136
		protected Vector3 coverNormal;

		// Token: 0x04002799 RID: 10137
		protected Vector3 positionHelper;

		// Token: 0x0400279A RID: 10138
		protected Vector3 inputDirection;

		// Token: 0x0400279B RID: 10139
		protected Vector3 cameraForwardDir;

		// Token: 0x0400279C RID: 10140
		protected Vector3 cameraRightDir;

		// Token: 0x0400279D RID: 10141
		[SerializeField]
		[vReadOnly(true)]
		protected Vector3 localCharPosition;

		// Token: 0x0400279E RID: 10142
		protected RaycastHit checkHeightHit;

		// Token: 0x0400279F RID: 10143
		protected Transform helper;

		// Token: 0x040027A0 RID: 10144
		protected List<Vector3> wayPath = new List<Vector3>();

		// Token: 0x040027A1 RID: 10145
		protected List<Vector3> wayPathToDraw = new List<Vector3>();

		// Token: 0x040027A2 RID: 10146
		protected List<Vector3> pathDrawPoints = new List<Vector3>();

		// Token: 0x040027A3 RID: 10147
		public List<vCoverPoint> coverPoints = new List<vCoverPoint>();

		// Token: 0x040027A4 RID: 10148
		protected NavMeshPath path;

		// Token: 0x040027A5 RID: 10149
		protected vShooterMeleeInput shooterInput;

		// Token: 0x040027A6 RID: 10150
		protected vGenericAction genericAction;

		// Token: 0x040027A7 RID: 10151
		protected Coroutine currentCoverRoutine;

		// Token: 0x040027A8 RID: 10152
		protected vThirdPersonMotor.vMovementSpeed moveSpeed;

		// Token: 0x02000681 RID: 1665
		protected enum Side
		{
			// Token: 0x040027AA RID: 10154
			left = -1,
			// Token: 0x040027AB RID: 10155
			right = 1
		}
	}
}
