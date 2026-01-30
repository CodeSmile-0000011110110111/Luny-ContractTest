using Luny.Engine.Bridge;
using System;

namespace Luny.ContractTest.Mocks
{
	public class MockPathConverter : ILunyPathConverter
	{
		public String ToLuny(String nativePath, LunyPathType type)
		{
			if (String.IsNullOrEmpty(nativePath))
				return nativePath;

			var normalized = nativePath.ToForwardSlashes();

			return type switch
			{
				LunyPathType.Asset => normalized.StartsWith("res://") ? normalized.Substring(6) :
				                      normalized.StartsWith("Assets/Resources/") ? normalized.Substring(17) : 
				                      normalized,
				LunyPathType.Save => normalized.StartsWith("user://") ? normalized.Substring(7) : 
				                     normalized.StartsWith("SaveData/") ? normalized.Substring(9) :
				                     normalized,
				_ => normalized
			};
		}

		public String ToNative(String agnosticPath, LunyPathType type)
		{
			if (String.IsNullOrEmpty(agnosticPath))
				return agnosticPath;

			return type switch
			{
				LunyPathType.Asset => $"res://{agnosticPath}", // Defaulting to Godot-style for mock
				LunyPathType.Save => $"user://{agnosticPath}",
				_ => agnosticPath
			};
		}
	}
}
