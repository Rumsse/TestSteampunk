using UnityEngine;
using TunnelGen.Camera;
using TunnelGen.Config;
using TunnelGen.Random;

namespace TunnelGen.Generation
{
	public sealed class TunnelMapGenerator : MonoBehaviour
	{
		[SerializeField] TunnelGenerationSettings generationSettings;
		[SerializeField] TunnelPrefabCatalog prefabCatalog;
		[SerializeField] Transform mapRoot;
		[SerializeField] PathController pathController;

		IRandomSource randomSource;

		void Awake() => randomSource = generationSettings.CreateRandomSource();

		void Start()
		{
			if (!generationSettings || !prefabCatalog || !mapRoot || !pathController)
				throw new MissingReferenceException($"{nameof(TunnelMapGenerator)} is missing references");

			var buildContext = new TunnelBuildContext(generationSettings, prefabCatalog, mapRoot, randomSource);
			var buildResult = TunnelLayoutBuilder.Build(buildContext);

			pathController.Initialize(buildResult.PathCentersX, buildResult.DefaultBranchId);
			pathController.SetTopology(buildResult.TopologyState);
		}
	}
}