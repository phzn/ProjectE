using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Features.CielVoice;
using Features.NPC.Audio;
using UnityEngine;

namespace Features.RunModes
{
	// Token: 0x02000D23 RID: 3363
	[CreateAssetMenu(fileName = "LongWayHomeUniversityConfig", menuName = "Project Exhibitionist/Long Way Home/University Config")]
	public sealed class LongWayHomeUniversityConfigSO : ScriptableObject
	{
		// Token: 0x0600622C RID: 25132 RVA: 0x002717DB File Offset: 0x0026F9DB
		public static LongWayHomeUniversityConfigSO Load()
		{
			return Resources.Load<LongWayHomeUniversityConfigSO>("LongWayHomeUniversityConfig");
		}

		// Token: 0x0600622D RID: 25133 RVA: 0x002717E7 File Offset: 0x0026F9E7
		public AnimationClip StimulationArmsFor(int boobsType)
		{
			return LongWayHomeUniversityConfigSO.ForBreastSize(this.stimulationArms, boobsType);
		}

		// Token: 0x0600622E RID: 25134 RVA: 0x002717F5 File Offset: 0x0026F9F5
		public AnimationClip AdvancedStimulationArmsFor(int boobsType)
		{
			return LongWayHomeUniversityConfigSO.ForBreastSize(this.advancedStimulationArms, boobsType);
		}

		// Token: 0x0600622F RID: 25135 RVA: 0x00271803 File Offset: 0x0026FA03
		public AnimationClip HoldBoobsArmsFor(int boobsType)
		{
			return LongWayHomeUniversityConfigSO.ForBreastSize(this.holdBoobsArms, boobsType);
		}

		// Token: 0x06006230 RID: 25136 RVA: 0x00271811 File Offset: 0x0026FA11
		public AnimationClip ShyPoseArmsFor(int boobsType)
		{
			return LongWayHomeUniversityConfigSO.ForBreastSize(this.shyPoseArms, boobsType);
		}

		// Token: 0x06006231 RID: 25137 RVA: 0x0027181F File Offset: 0x0026FA1F
		public AnimationClip CoverArmsFor(int boobsType)
		{
			return LongWayHomeUniversityConfigSO.ForBreastSize(this.coverArms, boobsType);
		}

		// Token: 0x06006232 RID: 25138 RVA: 0x0027182D File Offset: 0x0026FA2D
		public AnimationClip CoverLowerArmsFor(int boobsType)
		{
			return LongWayHomeUniversityConfigSO.ForBreastSize(this.coverLowerArms, boobsType);
		}

		// Token: 0x06006233 RID: 25139 RVA: 0x0027183C File Offset: 0x0026FA3C
		private static AnimationClip ForBreastSize(AnimationClip[] clips, int boobsType)
		{
			if (clips == null || clips.Length == 0)
			{
				return null;
			}
			int num = Mathf.Clamp(boobsType, 0, clips.Length - 1);
			if (clips[num] != null)
			{
				return clips[num];
			}
			if (clips.Length > 3 && clips[3] != null)
			{
				return clips[3];
			}
			foreach (AnimationClip animationClip in clips)
			{
				if (animationClip != null)
				{
					return animationClip;
				}
			}
			return null;
		}

		// Token: 0x06006234 RID: 25140 RVA: 0x002718A4 File Offset: 0x0026FAA4
		public bool HasRequiredAssets(out string missing)
		{
			string text;
			if (!(this.corridorIdle == null))
			{
				if (!(this.corridorWalk == null))
				{
					if (!(this.floorRoutine == null))
					{
						if (!(this.escapeIdle == null))
						{
							if (!(this.escapeRun == null))
							{
								if (!(this.StimulationArmsFor(3) == null))
								{
									if (!(this.AdvancedStimulationArmsFor(3) == null))
									{
										if (!(this.HoldBoobsArmsFor(3) == null))
										{
											if (!(this.ShyPoseArmsFor(3) == null))
											{
												if (!(this.CoverArmsFor(3) == null))
												{
													if (!(this.CoverLowerArmsFor(3) == null))
													{
														if (!(this.stimulationMask == null))
														{
															if (!(this.leftArmMask == null))
															{
																if (LongWayHomeUniversityConfigSO.HasRuntimeFace(this.corridorFaceAnimation))
																{
																	if (LongWayHomeUniversityConfigSO.HasRuntimeFace(this.pleasureFaceAnimation))
																	{
																		if (LongWayHomeUniversityConfigSO.HasRuntimeFace(this.floorFaceAnimation))
																		{
																			if (LongWayHomeUniversityConfigSO.HasRuntimeFace(this.sleepFaceAnimation))
																			{
																				if (LongWayHomeUniversityConfigSO.HasRuntimeFace(this.wakeFaceAnimation))
																				{
																					if (LongWayHomeUniversityConfigSO.HasRuntimeFace(this.embarrassedFaceAnimation))
																					{
																						if (!(this.stimulationVoice == null))
																						{
																							if (!(this.nightAmbience == null))
																							{
																								if (this.footstepClips != null)
																								{
																									if (!Array.TrueForAll<AudioClip>(this.footstepClips, (AudioClip clip) => clip == null))
																									{
																										string text2;
																										text = ((this.npcIdle == null) ? "npcIdle" : ((this.npcWalk == null) ? "npcWalk" : ((this.npcRun == null) ? "npcRun" : ((this.npcPhoto == null) ? "npcPhoto" : ((this.bystanderIdleMale == null) ? "bystanderIdleMale" : ((this.bystanderIdleFemale == null) ? "bystanderIdleFemale" : ((this.bystanderWalkMale == null) ? "bystanderWalkMale" : ((this.bystanderWalkFemale == null) ? "bystanderWalkFemale" : ((this.bystanderPhoto == null) ? "bystanderPhoto" : ((this.bystanderVideo == null) ? "bystanderVideo" : ((this.corridorPath == null || this.corridorPath.Length < 2) ? "corridorPath" : ((this.corridorShots == null || this.corridorShots.Length == 0) ? "corridorShots" : ((this.hallShots == null || this.hallShots.Length == 0) ? "hallShots" : ((this.hallEscapePath == null || this.hallEscapePath.Length < 2) ? "hallEscapePath" : ((this.restroomPath == null || this.restroomPath.Length < 2) ? "restroomPath" : ((this.restroomEntryPath == null || this.restroomEntryPath.Length < 2) ? "restroomEntryPath" : ((this.crowdMarks == null || this.crowdMarks.Length == 0) ? "crowdMarks" : ((!this.HasValidRoutineTimes(out text2)) ? text2 : null))))))))))))))))));
																										goto IL_043C;
																									}
																								}
																								text = "footstepClips";
																							}
																							else
																							{
																								text = "nightAmbience";
																							}
																						}
																						else
																						{
																							text = "stimulationVoice";
																						}
																					}
																					else
																					{
																						text = "embarrassedFaceAnimation";
																					}
																				}
																				else
																				{
																					text = "wakeFaceAnimation";
																				}
																			}
																			else
																			{
																				text = "sleepFaceAnimation";
																			}
																		}
																		else
																		{
																			text = "floorFaceAnimation";
																		}
																	}
																	else
																	{
																		text = "pleasureFaceAnimation";
																	}
																}
																else
																{
																	text = "corridorFaceAnimation";
																}
															}
															else
															{
																text = "leftArmMask";
															}
														}
														else
														{
															text = "stimulationMask";
														}
													}
													else
													{
														text = "coverLowerArms";
													}
												}
												else
												{
													text = "coverArms";
												}
											}
											else
											{
												text = "shyPoseArms";
											}
										}
										else
										{
											text = "holdBoobsArms";
										}
									}
									else
									{
										text = "advancedStimulationArms";
									}
								}
								else
								{
									text = "stimulationArms";
								}
							}
							else
							{
								text = "escapeRun";
							}
						}
						else
						{
							text = "escapeIdle";
						}
					}
					else
					{
						text = "floorRoutine";
					}
				}
				else
				{
					text = "corridorWalk";
				}
			}
			else
			{
				text = "corridorIdle";
			}
			IL_043C:
			missing = text;
			return missing == null;
		}

		// Token: 0x06006235 RID: 25141 RVA: 0x00271CF4 File Offset: 0x0026FEF4
		public bool HasValidRoutineTimes(out string invalid)
		{
			float num = ((this.floorRoutine != null) ? this.floorRoutine.length : 0f);
			invalid = ((num <= 0f) ? "floorRoutine" : ((this.riseToElbowsStart >= this.bellyLoop.x || this.bellyLoop.x >= this.bellyLoop.y) ? "bellyLoop" : ((this.bellyLoop.y > this.rollToBackStart + 0.5f || this.rollToBackStart >= this.backLoop.x) ? "rollToBackStart" : ((this.backLoop.x >= this.backLoop.y || this.backLoop.y > this.rollToBellyStart + 0.5f) ? "backLoop" : ((this.rollToBellyStart >= this.rollToBellyEnd || this.rollToBellyEnd >= num) ? "rollToBellyEnd" : ((this.sleepPose < this.backLoop.x || this.sleepPose > this.backLoop.y) ? "sleepPose" : ((this.getUp.x >= this.getUp.y || this.getUp.y > num) ? "getUp" : null)))))));
			return invalid == null;
		}

		// Token: 0x06006236 RID: 25142 RVA: 0x00271E60 File Offset: 0x00270060
		public void EvaluateFloorPoseFix(float clipSeconds, float[] offsets)
		{
			if (offsets == null)
			{
				return;
			}
			for (int i = 0; i < offsets.Length; i++)
			{
				AnimationCurve animationCurve = ((this.floorPoseFixes != null && i < this.floorPoseFixes.Length && this.floorPoseFixes[i] != null) ? this.floorPoseFixes[i].offset : null);
				offsets[i] = ((animationCurve != null && animationCurve.length > 0) ? animationCurve.Evaluate(clipSeconds) : 0f);
			}
		}

		// Token: 0x06006237 RID: 25143 RVA: 0x00271ECA File Offset: 0x002700CA
		public float EvaluateFloorHeight(float clipSeconds)
		{
			if (this.floorHeightOffset == null || this.floorHeightOffset.length <= 0)
			{
				return 0f;
			}
			return this.floorHeightOffset.Evaluate(clipSeconds);
		}

		// Token: 0x06006238 RID: 25144 RVA: 0x00271EF4 File Offset: 0x002700F4
		public float EvaluateFloorBodyPitch(float clipSeconds)
		{
			if (this.floorBodyPitch == null || this.floorBodyPitch.length <= 0)
			{
				return 0f;
			}
			return this.floorBodyPitch.Evaluate(clipSeconds);
		}

		// Token: 0x06006239 RID: 25145 RVA: 0x00271F20 File Offset: 0x00270120
		private static bool HasRuntimeFace(LongWayHomeFaceAnimationSO face)
		{
			if (face == null || face.duration <= 0f || face.curves == null || face.curves.Length == 0)
			{
				return false;
			}
			foreach (LongWayHomeFaceAnimationSO.CurveBinding curveBinding in face.curves)
			{
				if (curveBinding == null || string.IsNullOrEmpty(curveBinding.blendShapeName) || curveBinding.curve == null || curveBinding.curve.length == 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600623A RID: 25146 RVA: 0x00271F98 File Offset: 0x00270198
		public static AnimationCurve DefaultFallSpeed()
		{
			AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
			{
				new Keyframe(0f, 0.3f),
				new Keyframe(0.3f, 0.3f),
				new Keyframe(0.45f, 0.65f),
				new Keyframe(0.66f, 0.6f),
				new Keyframe(0.78f, 0.28f),
				new Keyframe(1f, 0.3f),
				new Keyframe(1.2f, 0.85f),
				new Keyframe(2.57f, 1f)
			});
			for (int i = 0; i < animationCurve.length; i++)
			{
				animationCurve.SmoothTangents(i, 0f);
			}
			return animationCurve;
		}

		// Token: 0x0600623B RID: 25147 RVA: 0x0027207F File Offset: 0x0027027F
		public static AnimationCurve DefaultFloorBodyPitch()
		{
			return LongWayHomeUniversityConfigSO.Spans(new Keyframe[][]
			{
				LongWayHomeUniversityConfigSO.Plateau(LongWayHomeUniversityConfigSO.BellyStart, LongWayHomeUniversityConfigSO.BellyEnd, -3.5f),
				LongWayHomeUniversityConfigSO.Plateau(LongWayHomeUniversityConfigSO.BackStart, LongWayHomeUniversityConfigSO.BackEnd, 5f)
			});
		}

		// Token: 0x0600623C RID: 25148 RVA: 0x002720BC File Offset: 0x002702BC
		public static LongWayHomeUniversityConfigSO.MuscleOffset[] DefaultFloorPoseFixes()
		{
			return new LongWayHomeUniversityConfigSO.MuscleOffset[]
			{
				new LongWayHomeUniversityConfigSO.MuscleOffset("Neck Nod Down-Up", LongWayHomeUniversityConfigSO.Spans(new Keyframe[][] { LongWayHomeUniversityConfigSO.Keyed(new ValueTuple<float, float>[]
				{
					new ValueTuple<float, float>(6.85f, 0f),
					new ValueTuple<float, float>(7.2f, 0f),
					new ValueTuple<float, float>(7.4f, -0.15f),
					new ValueTuple<float, float>(7.6f, -0.25f),
					new ValueTuple<float, float>(7.8f, -0.35f),
					new ValueTuple<float, float>(8f, -0.35f),
					new ValueTuple<float, float>(8.2f, -0.3f),
					new ValueTuple<float, float>(8.4f, -0.2f),
					new ValueTuple<float, float>(8.6f, -0.1f),
					new ValueTuple<float, float>(8.8f, 0f),
					new ValueTuple<float, float>(9.2f, 0f),
					new ValueTuple<float, float>(9.4f, -0.05f),
					new ValueTuple<float, float>(9.6f, -0.15f),
					new ValueTuple<float, float>(9.8f, -0.25f),
					new ValueTuple<float, float>(10f, -0.35f),
					new ValueTuple<float, float>(10.2f, -0.35f),
					new ValueTuple<float, float>(10.4f, -0.3f),
					new ValueTuple<float, float>(10.6f, -0.25f),
					new ValueTuple<float, float>(10.8f, -0.15f),
					new ValueTuple<float, float>(11f, -0.05f),
					new ValueTuple<float, float>(11.2f, 0f)
				}) })),
				new LongWayHomeUniversityConfigSO.MuscleOffset("Head Nod Down-Up", LongWayHomeUniversityConfigSO.Spans(new Keyframe[][] { LongWayHomeUniversityConfigSO.Plateau(LongWayHomeUniversityConfigSO.BackStart, LongWayHomeUniversityConfigSO.BackEnd, -0.05f) })),
				new LongWayHomeUniversityConfigSO.MuscleOffset("Left Arm Front-Back", LongWayHomeUniversityConfigSO.Spans(new Keyframe[][] { LongWayHomeUniversityConfigSO.Plateau(LongWayHomeUniversityConfigSO.BackStart, LongWayHomeUniversityConfigSO.BackEnd, -0.1f) })),
				new LongWayHomeUniversityConfigSO.MuscleOffset("Right Arm Front-Back", LongWayHomeUniversityConfigSO.Spans(new Keyframe[][] { LongWayHomeUniversityConfigSO.Plateau(LongWayHomeUniversityConfigSO.BackStart, LongWayHomeUniversityConfigSO.BackEnd, -0.1f) })),
				new LongWayHomeUniversityConfigSO.MuscleOffset("Left Upper Leg Front-Back", LongWayHomeUniversityConfigSO.Spans(new Keyframe[][] { LongWayHomeUniversityConfigSO.Plateau(LongWayHomeUniversityConfigSO.BackStart, LongWayHomeUniversityConfigSO.BackEnd, -0.12f) })),
				new LongWayHomeUniversityConfigSO.MuscleOffset("Right Upper Leg Front-Back", LongWayHomeUniversityConfigSO.Spans(new Keyframe[][] { LongWayHomeUniversityConfigSO.Plateau(LongWayHomeUniversityConfigSO.BackStart, LongWayHomeUniversityConfigSO.BackEnd, -0.12f) }))
			};
		}

		// Token: 0x0600623D RID: 25149 RVA: 0x002723BC File Offset: 0x002705BC
		public static AnimationCurve DefaultFloorHeight()
		{
			return LongWayHomeUniversityConfigSO.Spans(new Keyframe[][]
			{
				LongWayHomeUniversityConfigSO.Keyed(new ValueTuple<float, float>[]
				{
					new ValueTuple<float, float>(0.9f, 0f),
					new ValueTuple<float, float>(1f, 0.039f),
					new ValueTuple<float, float>(1.1f, 0.043f),
					new ValueTuple<float, float>(1.3f, 0.042f),
					new ValueTuple<float, float>(1.5f, 0.038f),
					new ValueTuple<float, float>(1.7f, 0.035f),
					new ValueTuple<float, float>(1.85f, 0.042f),
					new ValueTuple<float, float>(2f, 0.041f),
					new ValueTuple<float, float>(2.25f, 0f)
				}),
				LongWayHomeUniversityConfigSO.Keyed(new ValueTuple<float, float>[]
				{
					new ValueTuple<float, float>(6.85f, 0f),
					new ValueTuple<float, float>(7f, -0.005f),
					new ValueTuple<float, float>(7.2f, 0.001f),
					new ValueTuple<float, float>(7.4f, 0.002f),
					new ValueTuple<float, float>(7.6f, 0.008f),
					new ValueTuple<float, float>(7.8f, 0.012f),
					new ValueTuple<float, float>(8f, 0.014f),
					new ValueTuple<float, float>(8.2f, 0.014f),
					new ValueTuple<float, float>(8.4f, 0.012f),
					new ValueTuple<float, float>(8.6f, 0.009f),
					new ValueTuple<float, float>(8.8f, 0.006f),
					new ValueTuple<float, float>(9f, 0.004f),
					new ValueTuple<float, float>(9.2f, 0.003f),
					new ValueTuple<float, float>(9.4f, 0.005f),
					new ValueTuple<float, float>(9.6f, 0.007f),
					new ValueTuple<float, float>(9.8f, 0.008f),
					new ValueTuple<float, float>(10.2f, 0.008f),
					new ValueTuple<float, float>(10.4f, 0.006f),
					new ValueTuple<float, float>(10.6f, 0.004f),
					new ValueTuple<float, float>(10.8f, 0.002f),
					new ValueTuple<float, float>(11f, 0.001f),
					new ValueTuple<float, float>(11.2f, -0.002f),
					new ValueTuple<float, float>(11.4f, 0f)
				})
			});
		}

		// Token: 0x0600623E RID: 25150 RVA: 0x002726C0 File Offset: 0x002708C0
		private static Keyframe[] Plateau(Vector2 rise, Vector2 fall, float value)
		{
			return new Keyframe[]
			{
				new Keyframe(rise.x, 0f),
				new Keyframe(rise.y, value),
				new Keyframe(fall.x, value),
				new Keyframe(fall.y, 0f)
			};
		}

		// Token: 0x0600623F RID: 25151 RVA: 0x00272728 File Offset: 0x00270928
		private static Keyframe[] Keyed([TupleElementNames(new string[] { "time", "value" })] params ValueTuple<float, float>[] keys)
		{
			Keyframe[] array = new Keyframe[keys.Length];
			for (int i = 0; i < keys.Length; i++)
			{
				array[i] = new Keyframe(keys[i].Item1, keys[i].Item2);
			}
			return array;
		}

		// Token: 0x06006240 RID: 25152 RVA: 0x00272774 File Offset: 0x00270974
		private static AnimationCurve Spans(params Keyframe[][] spans)
		{
			AnimationCurve animationCurve = new AnimationCurve();
			HashSet<float> hashSet = new HashSet<float>();
			foreach (Keyframe[] array in spans)
			{
				foreach (Keyframe keyframe in array)
				{
					animationCurve.AddKey(keyframe);
				}
				hashSet.Add(array[0].time);
				hashSet.Add(array[array.Length - 1].time);
			}
			for (int k = 0; k < animationCurve.length; k++)
			{
				Keyframe keyframe2 = animationCurve[k];
				bool flag = (k > 0 && Mathf.Approximately(animationCurve[k - 1].value, keyframe2.value)) || (k < animationCurve.length - 1 && Mathf.Approximately(animationCurve[k + 1].value, keyframe2.value));
				if (hashSet.Contains(keyframe2.time) || flag)
				{
					keyframe2.inTangent = (keyframe2.outTangent = 0f);
					animationCurve.MoveKey(k, keyframe2);
				}
				else
				{
					animationCurve.SmoothTangents(k, 0f);
				}
			}
			return animationCurve;
		}

		// Token: 0x06006241 RID: 25153 RVA: 0x002728C0 File Offset: 0x00270AC0
		public static LongWayHomeUniversityConfigSO.BodyShot[] DefaultCorridorShots()
		{
			return new LongWayHomeUniversityConfigSO.BodyShot[]
			{
				new LongWayHomeUniversityConfigSO.BodyShot("Front", HumanBodyBones.Hips, new Vector3(0.55f, 0.55f, 3.1f), new Vector3(0f, 0.3f, 0f), 42f, 0.25f),
				new LongWayHomeUniversityConfigSO.BodyShot("Breasts", HumanBodyBones.Chest, new Vector3(0.12f, 0.06f, 0.66f), new Vector3(0f, -0.04f, 0.1f), 38f, 0.12f),
				new LongWayHomeUniversityConfigSO.BodyShot("Crotch", HumanBodyBones.Hips, new Vector3(0.1f, -0.1f, 0.66f), new Vector3(0f, -0.11f, 0.11f), 38f, 0.12f),
				new LongWayHomeUniversityConfigSO.BodyShot("Butt", HumanBodyBones.Hips, new Vector3(-0.12f, 0.02f, -0.78f), new Vector3(0f, -0.06f, -0.12f), 40f, 0.12f),
				new LongWayHomeUniversityConfigSO.BodyShot("Side", HumanBodyBones.Hips, new Vector3(2.1f, 0.25f, 0.35f), new Vector3(0f, 0.12f, 0f), 44f, 0.3f)
			};
		}

		// Token: 0x06006242 RID: 25154 RVA: 0x00272A14 File Offset: 0x00270C14
		public static LongWayHomeUniversityConfigSO.BystanderMark[] DefaultBystanders()
		{
			return new LongWayHomeUniversityConfigSO.BystanderMark[]
			{
				new LongWayHomeUniversityConfigSO.BystanderMark("Hallway - by the armchairs", new Vector3(100.7f, 8.025f, 12.6f), new Vector3(100.7f, 8.025f, 12.6f), 80f, LongWayHomeUniversityBystander.Reaction.Photo, false, true, LongWayHomeUniversityBystander.Preference.Man),
				new LongWayHomeUniversityConfigSO.BystanderMark("Hallway - by the bench", new Vector3(104.45f, 8.005f, 11.2f), new Vector3(104.45f, 8.005f, 11.2f), 265f, LongWayHomeUniversityBystander.Reaction.Video, false, false, LongWayHomeUniversityBystander.Preference.Woman),
				new LongWayHomeUniversityConfigSO.BystanderMark("Hallway - texting", new Vector3(101.7f, 8.005f, 9f), new Vector3(101.7f, 8.005f, 14.6f), 0f, LongWayHomeUniversityBystander.Reaction.None, true, false, LongWayHomeUniversityBystander.Preference.Any),
				new LongWayHomeUniversityConfigSO.BystanderMark("Bridge - at the windows", new Vector3(47.5f, 8.002f, 2.75f), new Vector3(47.5f, 8.002f, 2.75f), 165f, LongWayHomeUniversityBystander.Reaction.Photo, false, false, LongWayHomeUniversityBystander.Preference.Any),
				new LongWayHomeUniversityConfigSO.BystanderMark("Bridge - walking", new Vector3(45f, 8.003f, 5f), new Vector3(50.5f, 8.003f, 5f), 0f, LongWayHomeUniversityBystander.Reaction.Video, false, false, LongWayHomeUniversityBystander.Preference.Woman),
				new LongWayHomeUniversityConfigSO.BystanderMark("Bridge - far end", new Vector3(55.5f, 8.003f, 3f), new Vector3(61.5f, 8.003f, 3f), 0f, LongWayHomeUniversityBystander.Reaction.None, false, false, LongWayHomeUniversityBystander.Preference.Any),
				new LongWayHomeUniversityConfigSO.BystanderMark("Hostel - junction", new Vector3(44.3f, 8.002f, 2.55f), new Vector3(44.3f, 8.002f, 2.55f), 30f, LongWayHomeUniversityBystander.Reaction.Video, false, false, LongWayHomeUniversityBystander.Preference.Man),
				new LongWayHomeUniversityConfigSO.BystanderMark("Hostel - by the lockers", new Vector3(41.05f, 8.076f, -1.2f), new Vector3(41.05f, 8.076f, -1.2f), 270f, LongWayHomeUniversityBystander.Reaction.Photo, false, true, LongWayHomeUniversityBystander.Preference.Woman)
			};
		}

		// Token: 0x06006243 RID: 25155 RVA: 0x00272C18 File Offset: 0x00270E18
		public static LongWayHomeUniversityConfigSO.BodyShot[] DefaultHallShots()
		{
			return new LongWayHomeUniversityConfigSO.BodyShot[]
			{
				new LongWayHomeUniversityConfigSO.BodyShot("Audience", HumanBodyBones.Hips, new Vector3(2.6f, 1.55f, 0.9f), new Vector3(0f, 0.05f, 0.2f), 44f, 0.35f),
				new LongWayHomeUniversityConfigSO.BodyShot("Legs", HumanBodyBones.Hips, new Vector3(0.4f, 0.85f, -1.85f), new Vector3(0f, 0.05f, 0.25f), 44f, 0.35f),
				new LongWayHomeUniversityConfigSO.BodyShot("Side", HumanBodyBones.Hips, new Vector3(1.35f, 0.45f, 0.25f), new Vector3(0f, 0.02f, 0.1f), 46f, 0.3f),
				new LongWayHomeUniversityConfigSO.BodyShot("Above", HumanBodyBones.Hips, new Vector3(0.05f, 2.55f, 0.1f), new Vector3(0f, 0f, 0.15f), 52f, 0.35f),
				new LongWayHomeUniversityConfigSO.BodyShot("Head", HumanBodyBones.Head, new Vector3(-0.85f, 0.55f, 0.95f), Vector3.zero, 40f, 0.3f)
			};
		}

		// Token: 0x04006474 RID: 25716
		public const string ResourceName = "LongWayHomeUniversityConfig";

		// Token: 0x04006475 RID: 25717
		[Header("Clock (Azure hours)")]
		[Range(0f, 24f)]
		public float nightHour = 23.5f;

		// Token: 0x04006476 RID: 25718
		[Tooltip("The bell rings and the run continues from this hour.")]
		[Range(0f, 24f)]
		public float morningHour = 8.1f;

		// Token: 0x04006477 RID: 25719
		[Header("Ciel animation")]
		public AnimationClip corridorIdle;

		// Token: 0x04006478 RID: 25720
		public AnimationClip corridorWalk;

		// Token: 0x04006479 RID: 25721
		[Tooltip("MudAnim_Var_01: fall, belly loop, roll, back loop, roll back, get up.")]
		public AnimationClip floorRoutine;

		// Token: 0x0400647A RID: 25722
		[Tooltip("Where she stops in the bathroom: the game's EMB idle.")]
		public AnimationClip escapeIdle;

		// Token: 0x0400647B RID: 25723
		[Tooltip("The escape run: the game's EMB run (Run EMB 02). The cover pose (RMB) still goes on top.")]
		public AnimationClip escapeRun;

		// Token: 0x0400647C RID: 25724
		[Tooltip("The game's confident cover pose \"Stimulation\" per breast size (BoobsType 0 Flat .. 5 Balloon): hands on her chest. Key 1 in the corridor.")]
		public AnimationClip[] stimulationArms = new AnimationClip[6];

		// Token: 0x0400647D RID: 25725
		[Tooltip("The same pose with \"Cover lower body\" on: one hand goes down (FullCover), per breast size.")]
		public AnimationClip[] advancedStimulationArms = new AnimationClip[6];

		// Token: 0x0400647E RID: 25726
		[Tooltip("The game's embarrassed pose \"Hold Boobs\" per breast size. Key 2 in the corridor.")]
		public AnimationClip[] holdBoobsArms = new AnimationClip[6];

		// Token: 0x0400647F RID: 25727
		[Tooltip("The game's embarrassed pose \"Shy Pose\" (GrabMid, left arm only) per breast size. Key 3 in the corridor.")]
		public AnimationClip[] shyPoseArms = new AnimationClip[6];

		// Token: 0x04006480 RID: 25728
		[Tooltip("The game's embarrassed cover pose \"Cover\" per breast size, used while she runs away.")]
		public AnimationClip[] coverArms = new AnimationClip[6];

		// Token: 0x04006481 RID: 25729
		[Tooltip("The embarrassed \"Cover\" with \"Cover lower body\" on, per breast size.")]
		public AnimationClip[] coverLowerArms = new AnimationClip[6];

		// Token: 0x04006482 RID: 25730
		[Tooltip("The Animator's Arms layer mask (OnlyArms).")]
		public AvatarMask stimulationMask;

		// Token: 0x04006483 RID: 25731
		[Tooltip("The Animator's LeftArm layer mask, for the poses that move only the left arm (Shy Pose).")]
		public AvatarMask leftArmMask;

		// Token: 0x04006484 RID: 25732
		[Header("Floor routine (seconds of MudAnim_Var_01)")]
		[Tooltip("Playback speed over the fall (x = clip seconds 0..Belly Loop.x). The source drops to her knees in 0.25 s and onto the floor in 0.2 s: both are slowed down here.")]
		public AnimationCurve fallPlaybackSpeed = LongWayHomeUniversityConfigSO.DefaultFallSpeed();

		// Token: 0x04006485 RID: 25733
		[Tooltip("Lying on her belly on her elbows, kicking her legs.")]
		public Vector2 bellyLoop = new Vector2(2.57f, 4.77f);

		// Token: 0x04006486 RID: 25734
		[Tooltip("Lying on her back.")]
		public Vector2 backLoop = new Vector2(7.4f, 10.83f);

		// Token: 0x04006487 RID: 25735
		[Tooltip("Belly -> back: blend into the roll here and play it until Back Loop.x.")]
		public float rollToBackStart = 4.9f;

		// Token: 0x04006488 RID: 25736
		[Tooltip("Back -> belly: blend into the roll here and play it until Roll To Belly End...")]
		public float rollToBellyStart = 11.4f;

		// Token: 0x04006489 RID: 25737
		public float rollToBellyEnd = 13.85f;

		// Token: 0x0400648A RID: 25738
		[Tooltip("... then blend into the rise onto her elbows and play it until Belly Loop.x.")]
		public float riseToElbowsStart = 1.7f;

		// Token: 0x0400648B RID: 25739
		[Tooltip("The calm pose on her back where she falls asleep and wakes up.")]
		public float sleepPose = 8f;

		// Token: 0x0400648C RID: 25740
		[Tooltip("Get-up after the wake-up: roll onto her belly, push up, kneel and stand.")]
		public Vector2 getUp = new Vector2(11.4f, 18.35f);

		// Token: 0x0400648D RID: 25741
		[Min(0.05f)]
		public float loopBlendSeconds = 0.35f;

		// Token: 0x0400648E RID: 25742
		[Min(0.05f)]
		public float switchBlendSeconds = 0.45f;

		// Token: 0x0400648F RID: 25743
		[Min(0.05f)]
		public float reentryBlendSeconds = 0.6f;

		// Token: 0x04006490 RID: 25744
		[Range(0.2f, 1.5f)]
		public float rollPlaybackSpeed = 0.9f;

		// Token: 0x04006491 RID: 25745
		[Range(0.2f, 1.5f)]
		public float getUpPlaybackSpeed = 0.85f;

		// Token: 0x04006492 RID: 25746
		[Tooltip("Seconds over which the routine slows to a stop while she falls asleep.")]
		[Min(0.1f)]
		public float sleepSettleSeconds = 2.5f;

		// Token: 0x04006493 RID: 25747
		[Header("Floor contact (Ciel's proportions on MudAnim_Var_01)")]
		[Tooltip("Pitch of the whole body around its centre over MudAnim_Var_01 seconds, in degrees: positive raises the head end (on her back), negative raises the chest (on her belly).")]
		public AnimationCurve floorBodyPitch = LongWayHomeUniversityConfigSO.DefaultFloorBodyPitch();

		// Token: 0x04006494 RID: 25748
		[Tooltip("Muscle offsets that keep her head, shoulders, arms and chest on the floor instead of inside it: the clip was recorded on smaller proportions (head and chest).")]
		public LongWayHomeUniversityConfigSO.MuscleOffset[] floorPoseFixes = LongWayHomeUniversityConfigSO.DefaultFloorPoseFixes();

		// Token: 0x04006495 RID: 25749
		[Tooltip("Height of the whole body over MudAnim_Var_01 seconds, in metres (negative lowers her).")]
		public AnimationCurve floorHeightOffset = LongWayHomeUniversityConfigSO.DefaultFloorHeight();

		// Token: 0x04006496 RID: 25750
		[Tooltip("While she lies in the hall her hair collides with the floor instead of passing through it.")]
		public bool hairRestsOnFloor = true;

		// Token: 0x04006497 RID: 25751
		[Header("Faces (rebased and baked by Tools > Long Way Home)")]
		public AnimationClip corridorFace;

		// Token: 0x04006498 RID: 25752
		public AnimationClip pleasureFace;

		// Token: 0x04006499 RID: 25753
		public AnimationClip floorFace;

		// Token: 0x0400649A RID: 25754
		public AnimationClip embarrassedFace;

		// Token: 0x0400649B RID: 25755
		[Tooltip("Ciel_GetUp_Cinematic: its first seconds hold the prison's eyes-opening blinks.")]
		public AnimationClip wakeFaceSource;

		// Token: 0x0400649C RID: 25756
		public Vector2 wakeFaceRange = new Vector2(0f, 4.6f);

		// Token: 0x0400649D RID: 25757
		public LongWayHomeFaceAnimationSO corridorFaceAnimation;

		// Token: 0x0400649E RID: 25758
		public LongWayHomeFaceAnimationSO pleasureFaceAnimation;

		// Token: 0x0400649F RID: 25759
		public LongWayHomeFaceAnimationSO floorFaceAnimation;

		// Token: 0x040064A0 RID: 25760
		public LongWayHomeFaceAnimationSO sleepFaceAnimation;

		// Token: 0x040064A1 RID: 25761
		public LongWayHomeFaceAnimationSO wakeFaceAnimation;

		// Token: 0x040064A2 RID: 25762
		public LongWayHomeFaceAnimationSO embarrassedFaceAnimation;

		// Token: 0x040064A3 RID: 25763
		[Header("Voice and sound")]
		[Tooltip("Voice_LewdEnjoyment1: the gameplay stimulation moan with its subtitle. As in the game it plays once when the pose starts and stops when it ends.")]
		public CielVoiceSO stimulationVoice;

		// Token: 0x040064A4 RID: 25764
		[Tooltip("The game's night ambience (crickets), NightAmbientSoundLoop. In POIs the game plays it at 5%: the cinematic plays it itself, at night only.")]
		public AudioClip nightAmbience;

		// Token: 0x040064A5 RID: 25765
		[Tooltip("0.088 is the game's outdoor night level (master 0.5 x night 0.176).")]
		[Range(0f, 1f)]
		public float nightAmbienceVolume = 0.088f;

		// Token: 0x040064A6 RID: 25766
		[Tooltip("Ciel's barefoot steps, the same clips as her FootstepController.")]
		public AudioClip[] footstepClips = new AudioClip[2];

		// Token: 0x040064A7 RID: 25767
		[Range(0f, 1f)]
		public float walkFootstepVolume = 0.32f;

		// Token: 0x040064A8 RID: 25768
		[Range(0f, 1f)]
		public float runFootstepVolume = 0.4f;

		// Token: 0x040064A9 RID: 25769
		[Tooltip("Optional recorded school bell. Empty uses the built-in Westminster chime.")]
		public AudioClip morningBell;

		// Token: 0x040064AA RID: 25770
		[Range(0f, 1f)]
		public float bellVolume = 0.55f;

		// Token: 0x040064AB RID: 25771
		public AudioClip realizationGasp;

		// Token: 0x040064AC RID: 25772
		[Range(0f, 1f)]
		public float gaspVolume = 0.9f;

		// Token: 0x040064AD RID: 25773
		[Header("Crowd (morning)")]
		public AnimationClip npcIdle;

		// Token: 0x040064AE RID: 25774
		public AnimationClip npcWalk;

		// Token: 0x040064AF RID: 25775
		public AnimationClip npcRun;

		// Token: 0x040064B0 RID: 25776
		public AnimationClip npcPhoto;

		// Token: 0x040064B1 RID: 25777
		public AudioClip npcShutter;

		// Token: 0x040064B2 RID: 25778
		[Tooltip("Standing students around Ciel; each faces her and takes photos. The ring stays open towards the tiers (the main hall camera) and to the north, where she runs out.")]
		public Vector3[] crowdMarks = new Vector3[]
		{
			new Vector3(110.35f, 8.388f, 4.55f),
			new Vector3(110.25f, 8.388f, 3.15f),
			new Vector3(110.45f, 8.388f, 1.7f),
			new Vector3(111.45f, 8.388f, 0.85f),
			new Vector3(112.85f, 8.388f, 0.95f),
			new Vector3(113.7f, 8.388f, 1.55f),
			new Vector3(113.35f, 8.388f, 5.55f),
			new Vector3(110.95f, 8.388f, 5.7f)
		};

		// Token: 0x040064B3 RID: 25779
		[Header("Students on her way (morning)")]
		[Tooltip("The morning campus while she runs to the bathroom: students in the hallway outside the hall, on the covered bridge and in the hostel corridor. Visual copies of the run's civilians like the hall crowd (no AI, senses or detection): they stand or walk, and some photograph or film her.")]
		public LongWayHomeUniversityConfigSO.BystanderMark[] escapeBystanders = LongWayHomeUniversityConfigSO.DefaultBystanders();

		// Token: 0x040064B4 RID: 25780
		[Tooltip("They notice her this close, on the same floor, when no wall or door hides her.")]
		[Min(1f)]
		public float bystanderReactDistance = 7f;

		// Token: 0x040064B5 RID: 25781
		[Min(0.1f)]
		public float bystanderWalkMetersPerSecond = 1.2f;

		// Token: 0x040064B6 RID: 25782
		[Tooltip("The NPC controller's idle and walk for men and women (its Stance blend).")]
		public AnimationClip bystanderIdleMale;

		// Token: 0x040064B7 RID: 25783
		public AnimationClip bystanderIdleFemale;

		// Token: 0x040064B8 RID: 25784
		public AnimationClip bystanderWalkMale;

		// Token: 0x040064B9 RID: 25785
		public AnimationClip bystanderWalkFemale;

		// Token: 0x040064BA RID: 25786
		[Tooltip("The NPC controller's texting walk (Writing Movement).")]
		public AnimationClip bystanderTextingWalk;

		// Token: 0x040064BB RID: 25787
		[Tooltip("The NPC controller's Stop & Stare phone clips: Make Photo and Make Video.")]
		public AnimationClip bystanderPhoto;

		// Token: 0x040064BC RID: 25788
		public AnimationClip bystanderVideo;

		// Token: 0x040064BD RID: 25789
		[Tooltip("The NPC controller's Face layer: Surprised.")]
		public AnimationClip bystanderSurprisedFace;

		// Token: 0x040064BE RID: 25790
		[Tooltip("The game's NPC reaction voices: the Stop & Stare line for the student's gender.")]
		public NPCReactionAudioProfileSO bystanderVoices;

		// Token: 0x040064BF RID: 25791
		[Header("Corridor (second floor)")]
		[Tooltip("The two ends of the night walk. She walks from the first to the second, turns around and walks back, again and again, until the player continues.")]
		public Vector3[] corridorPath = new Vector3[]
		{
			new Vector3(75.5f, 8.005f, 4.2f),
			new Vector3(101f, 8.005f, 4.2f)
		};

		// Token: 0x040064C0 RID: 25792
		[Tooltip("Half the distance between the outbound and return lanes: the radius of her U-turns.")]
		[Range(0.3f, 1.5f)]
		public float corridorTurnRadius = 0.6f;

		// Token: 0x040064C1 RID: 25793
		[Min(0.1f)]
		public float corridorMetersPerSecond = 1.2f;

		// Token: 0x040064C2 RID: 25794
		public LongWayHomeUniversityConfigSO.BodyShot[] corridorShots = LongWayHomeUniversityConfigSO.DefaultCorridorShots();

		// Token: 0x040064C3 RID: 25795
		[Header("Lecture hall (upper round classroom)")]
		[Tooltip("Where Ciel stands when the routine starts; she falls forward from here.")]
		public Vector3 hallMark = new Vector3(112f, 8.388f, 2.4f);

		// Token: 0x040064C4 RID: 25796
		public float hallYaw;

		// Token: 0x040064C5 RID: 25797
		[Tooltip("Hall shots change with F; mouse, wheel and right stick then move the camera freely.")]
		public LongWayHomeUniversityConfigSO.BodyShot[] hallShots = LongWayHomeUniversityConfigSO.DefaultHallShots();

		// Token: 0x040064C6 RID: 25798
		[Tooltip("Close-up used while she falls asleep and wakes up, from her face along its forward axis.")]
		public float faceShotDistance = 0.62f;

		// Token: 0x040064C7 RID: 25799
		public float faceShotHeight = 0.12f;

		// Token: 0x040064C8 RID: 25800
		[Range(15f, 90f)]
		public float faceShotFieldOfView = 40f;

		// Token: 0x040064C9 RID: 25801
		[Header("Free camera (lecture hall)")]
		[Tooltip("Sensitivity, zoom step, distance range, smoothing and field of view come from the gameplay camera's Default state. This only keeps the camera from looking up from under the floor.")]
		[Range(-40f, 30f)]
		public float orbitMinPitch = -6f;

		// Token: 0x040064CA RID: 25802
		[Header("Escape")]
		[Tooltip("Run through the crowd, out of the lecture hall's north-west doorway and down the hallway towards the main corridor (the shot cuts before she reaches it).")]
		public Vector3[] hallEscapePath = new Vector3[]
		{
			new Vector3(112f, 8.388f, 2.4f),
			new Vector3(112.35f, 8.388f, 5.6f),
			new Vector3(110.6f, 8.388f, 9.5f),
			new Vector3(109.3f, 8.388f, 13f),
			new Vector3(107.9f, 8.388f, 15.5f),
			new Vector3(106.2f, 8.35f, 16.1f),
			new Vector3(104.3f, 8.005f, 15.7f),
			new Vector3(103.3f, 8.005f, 13.6f),
			new Vector3(103.1f, 8.005f, 10.2f),
			new Vector3(103f, 8.005f, 8.2f)
		};

		// Token: 0x040064CB RID: 25803
		public Vector3 hallEscapeCamera = new Vector3(107.4f, 10.25f, 12.4f);

		// Token: 0x040064CC RID: 25804
		[Tooltip("Outside the lecture hall: she bursts out of the doorway and runs down the hallway towards this camera.")]
		public Vector3 hallDoorCamera = new Vector3(101.8f, 9.6f, 5.6f);

		// Token: 0x040064CD RID: 25805
		public Vector3 hallDoorway = new Vector3(106.2f, 8.35f, 16.1f);

		// Token: 0x040064CE RID: 25806
		[Tooltip("The shot cuts outside when she is this close to the doorway.")]
		[Min(0.5f)]
		public float hallDoorCameraAtDistance = 2.2f;

		// Token: 0x040064CF RID: 25807
		[Tooltip("Second floor: along the covered bridge to the hostel, then down its hallway to the door of the women's bathroom with the shower, where she stops.")]
		public Vector3[] restroomPath = new Vector3[]
		{
			new Vector3(53f, 8.003f, 4.1f),
			new Vector3(44.5f, 8.003f, 4f),
			new Vector3(41.4f, 8.073f, 3.6f),
			new Vector3(40.2f, 8.073f, 1.8f),
			new Vector3(40.05f, 8.073f, -3f),
			new Vector3(40.05f, 8.071f, -6.8f)
		};

		// Token: 0x040064D0 RID: 25808
		[Tooltip("Once the door is open: through the doorway to the handoff mark.")]
		public Vector3[] restroomEntryPath = new Vector3[]
		{
			new Vector3(40.05f, 8.071f, -6.8f),
			new Vector3(40.05f, 8.071f, -7.4f),
			new Vector3(40f, 8.071f, -8.75f)
		};

		// Token: 0x040064D1 RID: 25809
		public Vector3 restroomCorridorCamera = new Vector3(39f, 9.65f, 6f);

		// Token: 0x040064D2 RID: 25810
		[Tooltip("The shot from inside the bathroom as she opens the door and comes in.")]
		public Vector3 restroomInsideCamera = new Vector3(38.65f, 9.7f, -10.55f);

		// Token: 0x040064D3 RID: 25811
		[Tooltip("The restroom door (a DoorInteraction) nearest this point opens for her and closes behind her.")]
		public Vector3 restroomDoor = new Vector3(40.06f, 8.08f, -7.32f);

		// Token: 0x040064D4 RID: 25812
		[Tooltip("She waits this long at the open door before going in: its Open clip swings the leaf into the bathroom in about 1.5 s, and she reaches the doorway once it is almost fully open.")]
		[Min(0f)]
		public float restroomDoorOpenSeconds = 1f;

		// Token: 0x040064D5 RID: 25813
		[Tooltip("Player control starts here: inside the bathroom, her back to the closed door, facing the shower.")]
		public Vector3 handoffMark = new Vector3(40f, 8.071f, -8.75f);

		// Token: 0x040064D6 RID: 25814
		public float handoffYaw = 180f;

		// Token: 0x040064D7 RID: 25815
		[Min(0.1f)]
		public float escapeMetersPerSecond = 3.3f;

		// Token: 0x040064D8 RID: 25816
		[Range(0.5f, 2f)]
		public float escapePlaybackSpeed = 1f;

		// Token: 0x040064D9 RID: 25817
		[Header("First person")]
		[Tooltip("Pitch the eyes start with when first person is turned on (positive looks down). Placement, look limits and head hiding are the game's first person.")]
		[Range(-30f, 60f)]
		public float povStartPitch = 12f;

		// Token: 0x040064DA RID: 25818
		[Header("Timing")]
		[Min(0.1f)]
		public float transitionSeconds = 0.8f;

		// Token: 0x040064DB RID: 25819
		[Min(0.1f)]
		public float quickCutSeconds = 0.3f;

		// Token: 0x040064DC RID: 25820
		[Min(0.1f)]
		public float handoffFadeSeconds = 0.45f;

		// Token: 0x040064DD RID: 25821
		[Tooltip("Arms pose in and out, as the Animator's Arms layer transitions (0.25 s).")]
		[Min(0.05f)]
		public float armsBlendSeconds = 0.25f;

		// Token: 0x040064DE RID: 25822
		[Min(0.05f)]
		public float faceBlendSeconds = 0.35f;

		// Token: 0x040064DF RID: 25823
		[Tooltip("Black screen between falling asleep and the morning bell.")]
		[Min(0.2f)]
		public float nightPassesSeconds = 2.2f;

		// Token: 0x040064E0 RID: 25824
		[Tooltip("Lying awake (not yet aware of the crowd) before getting up.")]
		[Min(0f)]
		public float wakeBeforeGetUpSeconds = 4.2f;

		// Token: 0x040064E1 RID: 25825
		[Tooltip("Standing, she notices the crowd for this long before running.")]
		[Min(0f)]
		public float realizeSeconds = 1.3f;

		// Token: 0x040064E2 RID: 25826
		[Tooltip("Inside the restroom, with the door closed, before the player takes control. Meanwhile the campus fills up for the run behind the closed door.")]
		[Min(0f)]
		public float restroomHoldSeconds = 3f;

		// Token: 0x040064E3 RID: 25827
		private static readonly Vector2 BackStart = new Vector2(6.85f, 7.1f);

		// Token: 0x040064E4 RID: 25828
		private static readonly Vector2 BackEnd = new Vector2(11.5f, 11.9f);

		// Token: 0x040064E5 RID: 25829
		private static readonly Vector2 BellyStart = new Vector2(0.9f, 1f);

		// Token: 0x040064E6 RID: 25830
		private static readonly Vector2 BellyEnd = new Vector2(2f, 2.25f);

		// Token: 0x02000D24 RID: 3364
		[Serializable]
		public sealed class BodyShot
		{
			// Token: 0x06006246 RID: 25158 RVA: 0x002734B0 File Offset: 0x002716B0
			public BodyShot()
			{
			}

			// Token: 0x06006247 RID: 25159 RVA: 0x0027350C File Offset: 0x0027170C
			public BodyShot(string label, HumanBodyBones anchor, Vector3 cameraOffset, Vector3 lookOffset, float fieldOfView, float damping = 0.18f)
			{
				this.label = label;
				this.anchor = anchor;
				this.cameraOffset = cameraOffset;
				this.lookOffset = lookOffset;
				this.fieldOfView = fieldOfView;
				this.damping = damping;
			}

			// Token: 0x040064E7 RID: 25831
			public string label = "Shot";

			// Token: 0x040064E8 RID: 25832
			public HumanBodyBones anchor;

			// Token: 0x040064E9 RID: 25833
			[Tooltip("Camera position from the anchor bone, in Ciel's yaw frame.")]
			public Vector3 cameraOffset = new Vector3(0f, 0.5f, 2f);

			// Token: 0x040064EA RID: 25834
			[Tooltip("Point looked at, from the anchor bone, in Ciel's yaw frame.")]
			public Vector3 lookOffset = Vector3.zero;

			// Token: 0x040064EB RID: 25835
			[Range(15f, 90f)]
			public float fieldOfView = 45f;

			// Token: 0x040064EC RID: 25836
			[Tooltip("Position damping: 0 is rigid, higher values float behind the body.")]
			[Range(0f, 1f)]
			public float damping = 0.18f;
		}

		// Token: 0x02000D25 RID: 3365
		[Serializable]
		public sealed class MuscleOffset
		{
			// Token: 0x06006248 RID: 25160 RVA: 0x00004D1F File Offset: 0x00002F1F
			public MuscleOffset()
			{
			}

			// Token: 0x06006249 RID: 25161 RVA: 0x00273592 File Offset: 0x00271792
			public MuscleOffset(string muscle, AnimationCurve offset)
			{
				this.muscle = muscle;
				this.offset = offset;
			}

			// Token: 0x040064ED RID: 25837
			[Tooltip("Humanoid muscle name, as listed in the avatar's Muscles & Settings (e.g. Neck Nod Down-Up).")]
			public string muscle;

			// Token: 0x040064EE RID: 25838
			[Tooltip("Offset in normalized muscle units over MudAnim_Var_01 seconds (0 keeps the authored pose).")]
			public AnimationCurve offset;
		}

		// Token: 0x02000D26 RID: 3366
		[Serializable]
		public sealed class BystanderMark
		{
			// Token: 0x0600624A RID: 25162 RVA: 0x002735A8 File Offset: 0x002717A8
			public BystanderMark()
			{
			}

			// Token: 0x0600624B RID: 25163 RVA: 0x002735C4 File Offset: 0x002717C4
			public BystanderMark(string label, Vector3 from, Vector3 to, float yaw, LongWayHomeUniversityBystander.Reaction reaction, bool texting = false, bool voice = false, LongWayHomeUniversityBystander.Preference prefer = LongWayHomeUniversityBystander.Preference.Any)
			{
				this.label = label;
				this.from = from;
				this.to = to;
				this.yaw = yaw;
				this.reaction = reaction;
				this.texting = texting;
				this.voice = voice;
				this.prefer = prefer;
			}

			// Token: 0x040064EF RID: 25839
			public string label = "Student";

			// Token: 0x040064F0 RID: 25840
			[Tooltip("Where they stand, or one end of their walk.")]
			public Vector3 from;

			// Token: 0x040064F1 RID: 25841
			[Tooltip("The other end of their walk, back and forth. Same as From: they stand.")]
			public Vector3 to;

			// Token: 0x040064F2 RID: 25842
			[Tooltip("Where they face while standing, in degrees.")]
			public float yaw;

			// Token: 0x040064F3 RID: 25843
			[Tooltip("The game's Stop & Stare when she runs by: photos or a video with the phone. None: they don't notice her.")]
			public LongWayHomeUniversityBystander.Reaction reaction = LongWayHomeUniversityBystander.Reaction.Photo;

			// Token: 0x040064F4 RID: 25844
			[Tooltip("Walks looking at the phone (the NPC controller's texting walk).")]
			public bool texting;

			// Token: 0x040064F5 RID: 25845
			[Tooltip("Says the game's Stop & Stare line when they notice her. Keep it to one or two.")]
			public bool voice;

			// Token: 0x040064F6 RID: 25846
			[Tooltip("A man or a woman when the run's population has one.")]
			public LongWayHomeUniversityBystander.Preference prefer;
		}
	}
}
