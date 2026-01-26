using Luny.Engine;
using Luny.Engine.Bridge;
using Luny.Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Luny.ContractTest.Base
{
	public sealed class MockEngineObserver : ILunyEngineObserver
	{
		private static readonly List<String> EngineCallbackNames = Enum.GetNames(typeof(EngineCallback)).ToList();

		public List<String> CallOrder { get; } = new();
		public Int64[] FrameCounts { get; } = new Int64[EngineCallbackNames.Count];

		public void OnEngineStartup()
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnEnginePreUpdate()
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnEngineFixedStep(Double fixedDeltaTime)
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnEngineUpdate(Double deltaTime)
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnEngineLateUpdate(Double deltaTime)
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnEnginePostUpdate()
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnEngineShutdown()
		{
			CallOrder.Add(ILunyDebugService.GetMethodName());
			FrameCounts[GetEngineCallbackIndex()] = LunyEngine.Instance.Time.FrameCount;
		}

		public void OnSceneLoaded(ILunyScene loadedScene) => CallOrder.Add(ILunyDebugService.GetMethodName());
		public void OnSceneUnloaded(ILunyScene unloadedScene) => CallOrder.Add(ILunyDebugService.GetMethodName());

		private Int32 GetEngineCallbackIndex([CallerMemberName] String methodName = "") => EngineCallbackNames.IndexOf(methodName);
	}

	public enum EngineCallback
	{
		OnEngineStartup,
		OnEnginePreUpdate,
		OnEngineFixedStep,
		OnEngineUpdate,
		OnEngineLateUpdate,
		OnEnginePostUpdate,
		OnEngineShutdown,
	}
}
