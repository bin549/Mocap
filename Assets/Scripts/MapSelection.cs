using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MapSelection : MonoBehaviour {
    [SerializeField] private GameObject currentScene;
    [SerializeField] private GameObject currentMapLight;
    [SerializeField] private Transform lightPivot;
    [SerializeField] private Transform mapPivot;
    [SerializeField] private AvatarCameraController avatarCameraController;
    public VideoPlayer videoPlayer;
    [SerializeField] private UiManager _uiManager;
    [SerializeField] private GameObject mapItem;
    [SerializeField] private MapUnit[] _mapUnits;

    private void Start() {
        foreach (var mapUnit in _mapUnits) {
            MapItem map = GameObject.Instantiate(mapItem).GetComponent<MapItem>();
            map.gameObject.transform.SetParent(transform);
            Button mapButton = map.gameObject.GetComponent<Button>();
            mapButton.image.sprite = mapUnit.sprite;
            mapButton.onClick.AddListener(() => {
                GameObject.Destroy(currentScene);
                GameObject.Destroy(currentMapLight);
                currentScene = GameObject.Instantiate(mapUnit.map);
                currentScene.transform.SetParent(mapPivot);
                currentMapLight = GameObject.Instantiate(mapUnit.mapLight);
                currentMapLight.transform.SetParent(lightPivot);
                RenderSettings.skybox = mapUnit.skyMaterial;
                this.avatarCameraController.isInputDisable = false;
                gameObject.SetActive(false);
            });
        }
    }

    private void OnEnable() {
        this.videoPlayer.Pause();
        _uiManager.screen.gameObject.SetActive(false);
    }

    private void OnDisable() {
        if (!this.videoPlayer) {
            return;
        }
        this.videoPlayer.Play();
        _uiManager.screen.gameObject.SetActive(true);
    }
}
