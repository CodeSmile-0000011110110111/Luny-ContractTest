using Luny.Engine.Bridge;
using Luny.Engine.Bridge.Identity;
using System;

namespace Luny.ContractTest.Mocks
{
	public sealed class MockPrefab : LunyPrefab
	{
		public LunyAssetID AssetID { get; }
		public LunyAssetPath AssetPath { get; }
		public Boolean IsPlaceholder { get; }

		public MockPrefab(Object mockPrefab, LunyAssetPath path, Boolean isPlaceholder = false)
			: base(mockPrefab, path)
		{
			AssetID = mockPrefab.GetHashCode();
			AssetPath = path;
			IsPlaceholder = isPlaceholder;
		}

		public override String ToString() => $"MockPrefab({AssetPath}, ID={AssetID}, Placeholder={IsPlaceholder})";
		public override T Instantiate<T>() => throw new NotImplementedException();
	}
}
