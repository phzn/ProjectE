using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using Unity.Burst;
using Unity.Transforms;
using UnityEngine.Scripting;

[assembly: AssemblyVersion("0.0.0.0")]
[assembly: InternalsVisibleTo("Unity.Transforms.Tests")]
[assembly: InternalsVisibleTo("Unity.Transforms.PerformanceTests")]
[assembly: AlwaysLinkAssembly]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(ParentSystem.__codegen__OnCreate_0000000E$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(ParentSystem.__codegen__OnUpdate_0000000F$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(LocalToWorldSystem.__codegen__OnCreate_00000022$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(LocalToWorldSystem.__codegen__OnUpdate_00000023$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(TransformHelpers.ComputeWorldTransformMatrix_0000005E$BurstDirectCall))]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
