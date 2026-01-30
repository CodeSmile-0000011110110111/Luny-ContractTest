using Luny.Engine.Bridge;
using Luny.Engine.Bridge.Identity;
using System;

namespace Luny.ContractTest.Mocks
{
	public class MockPrefab : ILunyPrefab
	{
		public LunyAssetID AssetID { get; }
		public LunyAssetPath AssetPath { get; }
		public Boolean IsPlaceholder { get; }

		public MockPrefab(LunyAssetID id, LunyAssetPath path, Boolean isPlaceholder = false)
		{
			AssetID = id;
			AssetPath = path;
			IsPlaceholder = isPlaceholder;
		}

		public override String ToString() => $"MockPrefab({AssetPath}, ID={AssetID}, Placeholder={IsPlaceholder})";
	}
}
