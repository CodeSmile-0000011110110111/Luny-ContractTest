using Luny.Engine.Bridge;
using Luny.Engine.Services;
using System;
using System.Collections.Generic;

namespace Luny.ContractTest.Mocks
{
	public class MockAssetService : LunyAssetServiceBase
	{
		private readonly Dictionary<String, ILunyAsset> _availableAssets = new();
		private readonly Dictionary<Type, String[]> _extensionMapping = new()
		{
			{ typeof(ILunyPrefab), new[] { ".prefab", ".tscn" } },
		};

		public void AddAsset(ILunyAsset asset) => _availableAssets[asset.AssetPath.NativePath] = asset;

		protected override T LoadAsset<T>(LunyAssetPath path)
		{
			if (_availableAssets.TryGetValue(path.NativePath, out var asset) && asset is T typedAsset)
				return typedAsset;

			return null;
		}

		protected override void UnloadAsset(ILunyAsset asset) {}

		protected override IReadOnlyDictionary<Type, String[]> GetExtensionMapping() => _extensionMapping;

		protected override T GetPlaceholder<T>(LunyAssetPath path)
		{
			if (typeof(T) == typeof(ILunyPrefab))
				return new MockPrefab(0, path, true) as T;

			throw new NotImplementedException($"Placeholder for {typeof(T).Name} not implemented in MockAssetService");
		}
	}
}
