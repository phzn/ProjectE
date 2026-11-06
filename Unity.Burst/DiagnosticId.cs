using System;

namespace Unity.Burst
{
	// Token: 0x0200002A RID: 42
	internal enum DiagnosticId
	{
		// Token: 0x040000FE RID: 254
		ERR_InternalCompilerErrorInBackend = 100,
		// Token: 0x040000FF RID: 255
		ERR_InternalCompilerErrorInFunction,
		// Token: 0x04000100 RID: 256
		ERR_InternalCompilerErrorInInstruction,
		// Token: 0x04000101 RID: 257
		ERR_OnlyStaticMethodsAllowed = 1000,
		// Token: 0x04000102 RID: 258
		ERR_UnableToAccessManagedMethod,
		// Token: 0x04000103 RID: 259
		ERR_UnableToFindInterfaceMethod,
		// Token: 0x04000104 RID: 260
		ERR_UnexpectedEmptyMethodBody,
		// Token: 0x04000105 RID: 261
		ERR_ManagedArgumentsNotSupported,
		// Token: 0x04000106 RID: 262
		ERR_CatchConstructionNotSupported = 1006,
		// Token: 0x04000107 RID: 263
		ERR_CatchAndFilterConstructionNotSupported,
		// Token: 0x04000108 RID: 264
		ERR_LdfldaWithFixedArrayExpected,
		// Token: 0x04000109 RID: 265
		ERR_PointerExpected,
		// Token: 0x0400010A RID: 266
		ERR_LoadingFieldFromManagedObjectNotSupported,
		// Token: 0x0400010B RID: 267
		ERR_LoadingFieldWithManagedTypeNotSupported,
		// Token: 0x0400010C RID: 268
		ERR_LoadingArgumentWithManagedTypeNotSupported,
		// Token: 0x0400010D RID: 269
		ERR_CallingBurstDiscardMethodWithReturnValueNotSupported = 1015,
		// Token: 0x0400010E RID: 270
		ERR_CallingManagedMethodNotSupported,
		// Token: 0x0400010F RID: 271
		ERR_InstructionUnboxNotSupported = 1019,
		// Token: 0x04000110 RID: 272
		ERR_InstructionBoxNotSupported,
		// Token: 0x04000111 RID: 273
		ERR_InstructionNewobjWithManagedTypeNotSupported,
		// Token: 0x04000112 RID: 274
		ERR_AccessingManagedArrayNotSupported,
		// Token: 0x04000113 RID: 275
		ERR_InstructionLdtokenFieldNotSupported,
		// Token: 0x04000114 RID: 276
		ERR_InstructionLdtokenMethodNotSupported,
		// Token: 0x04000115 RID: 277
		ERR_InstructionLdtokenTypeNotSupported,
		// Token: 0x04000116 RID: 278
		ERR_InstructionLdtokenNotSupported,
		// Token: 0x04000117 RID: 279
		ERR_InstructionLdvirtftnNotSupported,
		// Token: 0x04000118 RID: 280
		ERR_InstructionNewarrNotSupported,
		// Token: 0x04000119 RID: 281
		ERR_InstructionRethrowNotSupported,
		// Token: 0x0400011A RID: 282
		ERR_InstructionCastclassNotSupported,
		// Token: 0x0400011B RID: 283
		ERR_InstructionLdftnNotSupported = 1032,
		// Token: 0x0400011C RID: 284
		ERR_InstructionLdstrNotSupported,
		// Token: 0x0400011D RID: 285
		ERR_InstructionStsfldNotSupported,
		// Token: 0x0400011E RID: 286
		ERR_InstructionEndfilterNotSupported,
		// Token: 0x0400011F RID: 287
		ERR_InstructionEndfinallyNotSupported,
		// Token: 0x04000120 RID: 288
		ERR_InstructionLeaveNotSupported,
		// Token: 0x04000121 RID: 289
		ERR_InstructionNotSupported,
		// Token: 0x04000122 RID: 290
		ERR_LoadingFromStaticFieldNotSupported,
		// Token: 0x04000123 RID: 291
		ERR_LoadingFromNonReadonlyStaticFieldNotSupported,
		// Token: 0x04000124 RID: 292
		ERR_LoadingFromManagedStaticFieldNotSupported,
		// Token: 0x04000125 RID: 293
		ERR_LoadingFromManagedNonReadonlyStaticFieldNotSupported,
		// Token: 0x04000126 RID: 294
		ERR_InstructionStfldToManagedObjectNotSupported,
		// Token: 0x04000127 RID: 295
		ERR_InstructionLdlenNonConstantLengthNotSupported,
		// Token: 0x04000128 RID: 296
		ERR_StructWithAutoLayoutNotSupported,
		// Token: 0x04000129 RID: 297
		ERR_StructWithGenericParametersAndExplicitLayoutNotSupported = 1047,
		// Token: 0x0400012A RID: 298
		ERR_StructSizeNotSupported,
		// Token: 0x0400012B RID: 299
		ERR_StructZeroSizeNotSupported,
		// Token: 0x0400012C RID: 300
		ERR_MarshalAsOnFieldNotSupported,
		// Token: 0x0400012D RID: 301
		ERR_TypeNotSupported,
		// Token: 0x0400012E RID: 302
		ERR_RequiredTypeModifierNotSupported,
		// Token: 0x0400012F RID: 303
		ERR_ErrorWhileProcessingVariable,
		// Token: 0x04000130 RID: 304
		ERR_UnableToResolveType,
		// Token: 0x04000131 RID: 305
		ERR_UnableToResolveMethod,
		// Token: 0x04000132 RID: 306
		ERR_ConstructorNotSupported,
		// Token: 0x04000133 RID: 307
		ERR_FunctionPointerMethodMissingBurstCompileAttribute,
		// Token: 0x04000134 RID: 308
		ERR_FunctionPointerTypeMissingBurstCompileAttribute,
		// Token: 0x04000135 RID: 309
		ERR_FunctionPointerMethodAndTypeMissingBurstCompileAttribute,
		// Token: 0x04000136 RID: 310
		INF_FunctionPointerMethodAndTypeMissingMonoPInvokeCallbackAttribute = 10590,
		// Token: 0x04000137 RID: 311
		ERR_MarshalAsOnParameterNotSupported = 1061,
		// Token: 0x04000138 RID: 312
		ERR_MarshalAsOnReturnTypeNotSupported,
		// Token: 0x04000139 RID: 313
		ERR_TypeNotBlittableForFunctionPointer,
		// Token: 0x0400013A RID: 314
		ERR_StructByValueNotSupported,
		// Token: 0x0400013B RID: 315
		ERR_StructsWithNonUnicodeCharsNotSupported = 1066,
		// Token: 0x0400013C RID: 316
		ERR_VectorsByValueNotSupported,
		// Token: 0x0400013D RID: 317
		ERR_MissingExternBindings,
		// Token: 0x0400013E RID: 318
		ERR_MarshalAsNativeTypeOnReturnTypeNotSupported,
		// Token: 0x0400013F RID: 319
		ERR_AssertTypeNotSupported = 1071,
		// Token: 0x04000140 RID: 320
		ERR_StoringToReadOnlyFieldNotAllowed,
		// Token: 0x04000141 RID: 321
		ERR_StoringToFieldInReadOnlyParameterNotAllowed,
		// Token: 0x04000142 RID: 322
		ERR_StoringToReadOnlyParameterNotAllowed,
		// Token: 0x04000143 RID: 323
		ERR_TypeManagerStaticFieldNotCompatible,
		// Token: 0x04000144 RID: 324
		ERR_UnableToFindTypeIndexForTypeManagerType,
		// Token: 0x04000145 RID: 325
		ERR_UnableToFindFieldForTypeManager,
		// Token: 0x04000146 RID: 326
		ERR_CircularStaticConstructorUsage = 1090,
		// Token: 0x04000147 RID: 327
		ERR_ExternalInternalCallsInStaticConstructorsNotSupported,
		// Token: 0x04000148 RID: 328
		ERR_PlatformNotSupported,
		// Token: 0x04000149 RID: 329
		ERR_InitModuleVerificationError,
		// Token: 0x0400014A RID: 330
		ERR_ModuleVerificationError,
		// Token: 0x0400014B RID: 331
		ERR_UnableToFindTypeRequiredForTypeManager,
		// Token: 0x0400014C RID: 332
		ERR_UnexpectedIntegerTypesForBinaryOperation,
		// Token: 0x0400014D RID: 333
		ERR_BinaryOperationNotSupported,
		// Token: 0x0400014E RID: 334
		ERR_CalliWithThisNotSupported,
		// Token: 0x0400014F RID: 335
		ERR_CalliNonCCallingConventionNotSupported,
		// Token: 0x04000150 RID: 336
		ERR_StringLiteralTooBig,
		// Token: 0x04000151 RID: 337
		ERR_LdftnNonCCallingConventionNotSupported,
		// Token: 0x04000152 RID: 338
		ERR_UnableToCallMethodOnInterfaceObject,
		// Token: 0x04000153 RID: 339
		ERR_UnsupportedCpuDependentBranch = 1199,
		// Token: 0x04000154 RID: 340
		ERR_InstructionTargetCpuFeatureNotAllowedInThisBlock,
		// Token: 0x04000155 RID: 341
		ERR_AssumeRangeTypeMustBeInteger,
		// Token: 0x04000156 RID: 342
		ERR_AssumeRangeTypeMustBeSameSign,
		// Token: 0x04000157 RID: 343
		ERR_UnsupportedSpillTransform = 1300,
		// Token: 0x04000158 RID: 344
		ERR_UnsupportedSpillTransformTooManyUsers,
		// Token: 0x04000159 RID: 345
		ERR_MethodNotSupported,
		// Token: 0x0400015A RID: 346
		ERR_VectorsLoadFieldIsAddress,
		// Token: 0x0400015B RID: 347
		ERR_ConstantExpressionRequired,
		// Token: 0x0400015C RID: 348
		WRN_HWInstrinsicsWithFPDeterminism,
		// Token: 0x0400015D RID: 349
		ERR_PointerArgumentsUnexpectedAliasing = 1310,
		// Token: 0x0400015E RID: 350
		ERR_LoopIntrinsicMustBeCalledInsideLoop = 1320,
		// Token: 0x0400015F RID: 351
		ERR_LoopUnexpectedAutoVectorization,
		// Token: 0x04000160 RID: 352
		WRN_LoopIntrinsicCalledButLoopOptimizedAway,
		// Token: 0x04000161 RID: 353
		ERR_AssertArgTypesDiffer = 1330,
		// Token: 0x04000162 RID: 354
		ERR_StringInternalCompilerFixedStringTooManyUsers = 1340,
		// Token: 0x04000163 RID: 355
		ERR_StringInvalidFormatMissingClosingBrace,
		// Token: 0x04000164 RID: 356
		ERR_StringInvalidIntegerForArgumentIndex,
		// Token: 0x04000165 RID: 357
		ERR_StringInvalidFormatForArgument,
		// Token: 0x04000166 RID: 358
		ERR_StringArgumentIndexOutOfRange,
		// Token: 0x04000167 RID: 359
		ERR_StringInvalidArgumentType,
		// Token: 0x04000168 RID: 360
		ERR_DebugLogNotSupported,
		// Token: 0x04000169 RID: 361
		ERR_StringInvalidNonLiteralFormat,
		// Token: 0x0400016A RID: 362
		ERR_StringInvalidStringFormatMethod,
		// Token: 0x0400016B RID: 363
		ERR_StringInvalidArgument,
		// Token: 0x0400016C RID: 364
		ERR_StringArrayInvalidArrayCreation,
		// Token: 0x0400016D RID: 365
		ERR_StringArrayInvalidArraySize,
		// Token: 0x0400016E RID: 366
		ERR_StringArrayInvalidControlFlow,
		// Token: 0x0400016F RID: 367
		ERR_StringArrayInvalidArrayIndex,
		// Token: 0x04000170 RID: 368
		ERR_StringArrayInvalidArrayIndexOutOfRange,
		// Token: 0x04000171 RID: 369
		ERR_UnmanagedStringMethodMissing,
		// Token: 0x04000172 RID: 370
		ERR_UnmanagedStringMethodInvalid,
		// Token: 0x04000173 RID: 371
		ERR_ManagedStaticConstructor = 1360,
		// Token: 0x04000174 RID: 372
		ERR_StaticConstantArrayInStaticConstructor,
		// Token: 0x04000175 RID: 373
		WRN_ExceptionThrownInNonSafetyCheckGuardedFunction = 1370,
		// Token: 0x04000176 RID: 374
		WRN_ACallToMethodHasBeenDiscarded,
		// Token: 0x04000177 RID: 375
		ERR_AccessingNestedManagedArrayNotSupported = 1380,
		// Token: 0x04000178 RID: 376
		ERR_LdobjFromANonPointerNonReference,
		// Token: 0x04000179 RID: 377
		ERR_StringLiteralRequired,
		// Token: 0x0400017A RID: 378
		ERR_MultiDimensionalArrayUnsupported,
		// Token: 0x0400017B RID: 379
		ERR_NonBlittableAndNonManagedSequentialStructNotSupported,
		// Token: 0x0400017C RID: 380
		ERR_VarArgFunctionNotSupported
	}
}
