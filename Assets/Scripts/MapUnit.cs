using UnityEngine;

[CreateAssetMenu(fileName = "Map Unit")]
public class MapUnit : ScriptableObject {
    public GameObject map;
    public Sprite sprite;
    public Material skyMaterial;
    public GameObject mapLight;
}
