using DG.Tweening;
using UnityEngine;
public class DragCameraController : MonoBehaviour {

  [SerializeField]
  private float zoomSpeed;
  [SerializeField]
  private float moveSpeed;

  private Camera _camera;

  private Vector2 _dragOrigScreenPos;
  private Vector3 _camOrigPos;
  private Vector2 m_cachedDir;

  public float moveTweenDuration;
  public float zoomTweenDuration;

  public float minZoomSize, maxZoomSize;

  public float xMin, xMax, yMin, yMax;
  
  private void Start() {
    _camera = GetComponent<Camera>();
  }

  void Update() {

    // ZOOM
    float mouseScrollVal = Input.mouseScrollDelta.y;

    if (mouseScrollVal != 0) {
      mouseScrollVal = mouseScrollVal > 0 ? -1f : 1f;
      var newSize = _camera.orthographicSize + mouseScrollVal * zoomSpeed * Time.deltaTime;
      newSize = Mathf.Clamp(newSize, minZoomSize, maxZoomSize);

      DOTween.To(
        () => _camera.orthographicSize,
        x => _camera.orthographicSize = x,
        newSize,
        zoomTweenDuration
      );
    }

    // DRAG MOVE
    if (Input.GetMouseButtonDown(2)) {
      _dragOrigScreenPos = Input.mousePosition;
      _camOrigPos = _camera.transform.position;
    }
    if (Input.GetMouseButton(2)) {
      Vector2 currentScreenPos = Input.mousePosition;

      Vector3 curPos = _camera.ScreenToWorldPoint(currentScreenPos);
      Vector3 origPos = _camera.ScreenToWorldPoint(_dragOrigScreenPos);

      Vector3 dir = origPos - curPos;
      dir.z = 0f;

      var pos = _camOrigPos + dir;

      var newPos = new Vector3(
        Mathf.Clamp(pos.x, xMin, xMax),
        Mathf.Clamp(pos.y, yMin, yMax),
        pos.z
        );
      
      _camera.transform.DOMove(newPos, moveTweenDuration);

    }

  }
}