using UnityEditor;
using UnityEngine;

public class BackgroundMusicCollectionAssetCreator : MonoBehaviour {

    [MenuItem("Assets/Create/BackgroundMusicCollection")]
    public static void CreateAsset()
    {
        CustomAssetUtility.CreateAsset<BackgroundMusicCollection>();
    }
}
