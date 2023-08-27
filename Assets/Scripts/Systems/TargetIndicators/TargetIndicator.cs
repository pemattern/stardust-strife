using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(RectTransform))]
public class TargetIndicator : MonoBehaviour
{
    private Transform _target;
    private Camera _camera;
    private RectTransform _rectTransform;
    private RectTransform _canvasRectTransform;
    private Image _image;

    private Vector3 offset => _canvasRectTransform.sizeDelta * 0.5f;
    private int margin => (int)_rectTransform.sizeDelta.x;
    private Vector3 screenPos => _camera.WorldToScreenPoint(_target.position);

    public void Initialize(Transform target, Camera camera, Canvas canvas)
    {
        _target = target;
        _camera = camera;
        _rectTransform = GetComponent<RectTransform>();
        _canvasRectTransform = canvas.GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    void Update()
    {
        Vector3 position = screenPos - offset;

        if (InsideScreenspace(screenPos))
        {
            _image.enabled = false;
            position.z = 0f;
        }
        else if(position.z >= 0f)
        {
            _image.enabled = true;
            position.z = 0f;
            position = OffScreenIndicatorPosition(position);
        }
        else
        {
            _image.enabled = true;
            position *= -1;
            position = OffScreenIndicatorPosition(position);            
        }

        _rectTransform.anchoredPosition = position;
    }

    private Vector3 OffScreenIndicatorPosition(Vector3 position)
    {
        float distanceX = (offset.x - margin) / Mathf.Abs(position.x);
        float distanceY = (offset.y - margin) / Mathf.Abs(position.y);

        if (distanceX < distanceY)
        {
            float angle = Vector3.SignedAngle(Vector3.right, position, Vector3.forward);
            position.x = Mathf.Sign(position.x) * (offset.x - margin);
            position.y = Mathf.Tan(Mathf.Deg2Rad * angle) * position.x;
        }
        else
        {
            float angle = Vector3.SignedAngle(Vector3.up, position, Vector3.forward);
            position.y = Mathf.Sign(position.y) * (offset.y - margin);
            position.x = -Mathf.Tan(Mathf.Deg2Rad * angle) * position.y;            
        }
        return position;
    }

    private bool InsideScreenspace(Vector3 screenPos)
    {
        return screenPos.x >= 0 && screenPos.x <= _canvasRectTransform.sizeDelta.x &&
            screenPos.y >= 0 && screenPos.y <= _canvasRectTransform.sizeDelta.y &&
            screenPos.z >= 0;
    }
}