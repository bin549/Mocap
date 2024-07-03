using UnityEngine;
using UnityEngine.UI;

public class AppSettingsPanel : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [SerializeField] private AppSettings appSettings;
    [SerializeField] private Button hidePanelButton;
    [SerializeField] private InputField ifVideosFolder;
    [SerializeField] private Button btnVideosFolder;
    [SerializeField] private InputField ifSavedFolder;
    [SerializeField] private Button btnSavedFolder;
    [SerializeField] private Toggle showSkeleton;
    [SerializeField] private Toggle isMaleModel;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        appSettings = GameObject.FindObjectOfType<AppSettings>();
        showSkeleton = GameObject.Find("showSkeleton").GetComponent<Toggle>();
        isMaleModel = GameObject.Find("isMaleModel").GetComponent<Toggle>();
        hidePanelButton = GameObject.Find("HidePanelButton").GetComponent<Button>();
        ifVideosFolder = GameObject.Find("ifVideosFolder").GetComponent<InputField>();
        btnVideosFolder = GameObject.Find("btnVideosFolder").GetComponent<Button>();
        ifSavedFolder = GameObject.Find("ifSavedFolder").GetComponent<InputField>();
        btnSavedFolder = GameObject.Find("btnSavedFolder").GetComponent<Button>();
    }

    private void Start()
    {
        ifVideosFolder.text = appSettings.GetVideosFolderPath();
        ifSavedFolder.text = appSettings.GetSavedFolderPath();
        btnVideosFolder.onClick.AddListener(() =>
        {
            appSettings.SetVideosFolderPath();
            ifVideosFolder.text = appSettings.GetVideosFolderPath();
        });


        btnSavedFolder.onClick.AddListener(() =>
        {
            appSettings.SetSavedFolderPath();
            ifSavedFolder.text = appSettings.GetSavedFolderPath();
        });

        hidePanelButton.onClick.AddListener(() => { Fade(); });
    }

    public void Fade()
    {
        animator.SetTrigger("Fade");
    }

    public void HidePanel()
    {
        this.gameObject.SetActive(false);
    }
}
