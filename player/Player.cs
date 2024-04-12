using Godot;

namespace RPGFPS.Player;

public partial class Player : Mob
{
    private const float MouseSens = 0.35f, JoystickSens = 3.5f;
    [Export] private Camera3D _camera;

    private Vector3 _cameraOffset, _lastPhysicsPos, _curPhysicsPos;

    public override void _Ready()
    {
        _cameraOffset = _camera.Position;

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        if (!IsOnFloor())
            velocity.Y -= Gravity * (float)delta;

        var inputDir = Input.GetVector("strafe_left", "strafe_right", "move_backward", "move_forward");
        var direction = (_camera.GlobalBasis * new Vector3(inputDir.X, 0, -inputDir.Y)).Normalized();

        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * MovementSpeed;
            velocity.Z = direction.Z * MovementSpeed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, MovementSpeed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, MovementSpeed);
        }

        Velocity = velocity;
        MoveAndSlide();

        _lastPhysicsPos = _curPhysicsPos;
        _curPhysicsPos = GlobalPosition;
    }

    public override void _Process(double delta)
    {
        LookAroundByInput(Input.GetVector("look_left", "look_right", "look_down", "look_up") * JoystickSens *
                          (float)delta);

        // apply physics interpolation fraction to camera to reduce physics stutter
        _camera.GlobalPosition = _lastPhysicsPos +
                                 (_curPhysicsPos - _lastPhysicsPos) * (float)Engine.GetPhysicsInterpolationFraction() +
                                 _cameraOffset;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (Input.MouseMode != Input.MouseModeEnum.Captured) return;

        if (@event is not InputEventMouseMotion motion) return;

        var look = motion.Relative * MouseSens * 0.01f;
        LookAroundByInput(new Vector2(look.X, -look.Y));
    }

    public override void _Notification(int what)
    {
        Input.MouseMode = (long)what switch
        {
            MainLoop.NotificationApplicationFocusOut => Input.MouseModeEnum.Visible,
            MainLoop.NotificationApplicationFocusIn => Input.MouseModeEnum.Captured,
            _ => Input.MouseMode
        };
    }

    private void LookAroundByInput(Vector2 lookInput)
    {
        RotateY(-lookInput.X);
        _camera.Rotation = new Vector3(
            Mathf.Clamp(_camera.Rotation.X + lookInput.Y, Mathf.DegToRad(-30), Mathf.DegToRad(60)), 0f, 0f);
    }
}