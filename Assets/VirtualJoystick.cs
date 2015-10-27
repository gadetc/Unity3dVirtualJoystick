using UnityEngine;

/// <summary>
/// http://catsoft-studios.com/virtual-joysticks-for-unity3d-mobile-games/
/// </summary>
public class VirtualJoystick : MonoBehaviour
{
    [HideInInspector]
    public Vector2 Movement = Vector2.zero;

    public Texture2D PadBackgroundTexture;
    public Texture2D PadControllerTexture;

    private Rect _padBackgroundRect = new Rect(0, 0, 100, 100);
    private Rect _padControllerRect = new Rect(0, 0, 100, 100);

    private Vector2 _padBackgroundPosition = Vector2.zero;
    private Vector2 _padControllerPosition = Vector2.zero;

    private const float PadRadius = 50.0f;

    private bool _active;

    public void Update()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.touches[0];
            var touchPosition = new Vector2(touch.position.x, Screen.height - touch.position.y);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _active = true;
                    _padBackgroundPosition = touchPosition;
                    _padControllerPosition = touchPosition;
                    break;

                case TouchPhase.Moved:
                    _padControllerPosition = touchPosition;
                    break;

                case TouchPhase.Stationary:
                    break;

                case TouchPhase.Canceled:
                    _active = false;
                    _padBackgroundPosition = _padControllerPosition;
                    break;

                case TouchPhase.Ended:
                    _active = false;
                    _padBackgroundPosition = _padControllerPosition;
                    break;
            }
        }

        var direction = (_padControllerPosition - _padBackgroundPosition);
        var distance = Vector2.Distance(_padControllerPosition, _padBackgroundPosition);

        if (PadRadius/distance > 3.5f)
        {
            Movement = Vector2.zero;
        }
        else
        {
            Movement = direction.normalized;
            
            // if the joystick is not being fully pushed, divide the movement by two (to make the player walk or run):
            if (PadRadius / distance > 1.5f) Movement /= 2.0f;
        }
    }

    public void OnGUI()
    {
        if (!_active) { return; }
			
        var backgroundRect = new Rect(
            _padBackgroundPosition.x - (_padBackgroundRect.width / 2.0f),
            _padBackgroundPosition.y - (_padBackgroundRect.height / 2.0f),
            _padBackgroundRect.width,
            _padBackgroundRect.height
            );
            
        var controllerX = _padControllerPosition.x - (_padControllerRect.width / 2.0f);
        var controllerY = _padControllerPosition.y - (_padControllerRect.height / 2.0f);

        var controllerRect = new Rect(
            Mathf.Clamp(controllerX, backgroundRect.x - PadRadius, backgroundRect.x + PadRadius),
            Mathf.Clamp(controllerY, backgroundRect.y - PadRadius, backgroundRect.y + PadRadius) ,
            _padControllerRect.width,
            _padControllerRect.height
            );

        GUI.DrawTexture(backgroundRect, PadBackgroundTexture);
        GUI.DrawTexture(controllerRect, PadControllerTexture);
    }
}