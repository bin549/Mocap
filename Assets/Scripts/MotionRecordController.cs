using UnityEngine;

public class RecordController : MonoBehaviour {
    [SerializeField] private AppSettings appSettings;
    [SerializeField] private Animator _animator;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            this.RecordMotion();
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            this.RecordMotion();
        }
    }

    private void RecordMotion() {
        this.UnityAnimRecording();
    }

    private void UnityAnimRecording() {
        if (!appSettings.motionDataRecorder.isRecording) {
            _animator.SetBool("isRecording", true);
            appSettings.motionDataRecorder.RecordStart();
        } else {
            try {
                appSettings.motionDataRecorder.RecordEnd();
                _animator.SetBool("isRecording", false);
            } catch (System.Exception e) {
                Debug.LogError("Fail！" + e.Message + e.StackTrace);
            }
        }
    }
}
