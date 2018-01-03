using UnityEditor;
using UnityEngine;

public class AssetImportSettings : AssetPostprocessor {

    void OnPreprocessModel () {

        ModelImporter importer = assetImporter as ModelImporter;

        importer.useFileScale = false;
        importer.meshCompression = ModelImporterMeshCompression.Off;
        importer.isReadable = true;
        importer.optimizeMesh = true;
        importer.importBlendShapes = false;
        importer.addCollider = false;
        importer.keepQuads = false;
        importer.weldVertices = false;
        importer.importVisibility = false;
        importer.importCameras = false;
        importer.importLights = false;
        importer.swapUVChannels = false;
        importer.generateSecondaryUV = false;

        importer.importNormals = ModelImporterNormals.Import;
        importer.importTangents = ModelImporterTangents.CalculateMikk;

        importer.animationType = ModelImporterAnimationType.None;
        importer.importAnimation = false;

        importer.importMaterials = false;
    }
}