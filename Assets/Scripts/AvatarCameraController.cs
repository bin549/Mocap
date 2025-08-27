using UnityEngine;

public class AvatarCameraController : MonoBehaviour {
    public SkeletonAvatar avatar;
    public Transform avatarTransform;
    private Vector3 offsetPosition;
    private bool isRotating = false;
    public float distance = 7;
    public float scrollSpeed = 30;
    public float rotateSpeed = 30;
    public bool isInputDisable = true;

    public void StartControl() {
        this.avatarTransform = avatar.gameObject.transform;
        transform.LookAt(avatarTransform.position);
        this.offsetPosition = transform.position - avatarTransform.position;
        this.isInputDisable = false;
    }

    public void SetAvatar(Transform avatar) {
        this.avatarTransform = avatar;
    }

    private void Update() {
        if (this.isInputDisable) {
            return;
        }
        transform.position = this.offsetPosition + this.avatarTransform.position;
        this.RotateView();
        this.ScrollViewMouse();
        this.ScrollViewArrow();
    }

    private void ScrollViewArrow() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.RotateAround(this.avatarTransform.transform.position, Vector3.up,
                this.rotateSpeed * 5 * Time.deltaTime);
            this.offsetPosition = transform.position - this.avatarTransform.transform.position;
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.RotateAround(this.avatarTransform.transform.position, Vector3.up,
                -rotateSpeed * 5 * Time.deltaTime);
            this.offsetPosition = transform.position - avatarTransform.transform.position;
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            distance = offsetPosition.magnitude;
            distance += (float)0.2 * -scrollSpeed;
            distance = Mathf.Clamp(distance, 1, 10);
            offsetPosition = offsetPosition.normalized * distance;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            distance = offsetPosition.magnitude;
            distance += (float)0.2 * scrollSpeed;
            distance = Mathf.Clamp(distance, 1, 10);
            offsetPosition = offsetPosition.normalized * distance;
        }
    }

    private void ScrollViewMouse() {
        distance = offsetPosition.magnitude;
        distance += Input.GetAxis("Mouse ScrollWheel") * -scrollSpeed;
        distance = Mathf.Clamp(distance, 5, 10);
        offsetPosition = offsetPosition.normalized * distance;
    }

    private void RotateView() {
        if (Input.GetMouseButtonDown(1)) {
            isRotating = true;
        }
        if (Input.GetMouseButtonUp(1)) {
            isRotating = false;
        }
        if (isRotating) {
            transform.RotateAround(avatarTransform.position, avatarTransform.up,
                rotateSpeed * Input.GetAxis("Mouse X"));
            Vector3 originalPos = transform.position;
            Quaternion originalRotation = transform.rotation;
            //transform.RotateAround(avatarTransform.position, avatarTransform.right, -rotateSpeed * Input.GetAxis("Mouse Y"));
            float x = transform.eulerAngles.x;
            if (x < 10 || x > 80) {
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }
        }
        offsetPosition = transform.position - avatarTransform.position;
    }
}
