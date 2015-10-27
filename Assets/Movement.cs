using UnityEngine;
using System.Collections;

[RequireComponent(typeof(VirtualJoystick))]
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
	public Camera Camera;

    private CharacterController _character;
    private VirtualJoystick _movementJoystick;
    private float _speed = 3f;
    
    public void Start()
    {
		_movementJoystick = GetComponent<VirtualJoystick>();
		_character = GetComponent<CharacterController> ();
    }

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        var movement = Camera.transform.TransformDirection(new Vector3(_movementJoystick.Movement.x, 0, _movementJoystick.Movement.y * -1));
        movement.y = 0;
        movement.Normalize();

        var absJoyPos = new Vector2(Mathf.Abs(_movementJoystick.Movement.x), Mathf.Abs(_movementJoystick.Movement.y));
        movement *= _speed * ((absJoyPos.x > absJoyPos.y) ? absJoyPos.x : absJoyPos.y);

        movement += Physics.gravity;
        movement *= Time.deltaTime;
        _character.Move(movement);

		FaceMovementDirection();
    }

    private void FaceMovementDirection()
    {
        var absJoyPos = new Vector2((_movementJoystick.Movement.x), (_movementJoystick.Movement.y));
        var joystickInertialPosition = new Vector3(absJoyPos.y, 0, absJoyPos.x);
        transform.LookAt(transform.position + joystickInertialPosition);
    }
}
