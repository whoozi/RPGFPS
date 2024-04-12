using Godot;

namespace RPGFPS;

public partial class Mob : CharacterBody3D
{
    [Export] protected AnimationPlayer AnimPlayer;
    [Export] protected Sprite3D Sprite;
    [Export] protected float MovementSpeed = 5f;

    protected readonly float Gravity =
        (float)ProjectSettings.GetSettingWithOverride("physics/3d/default_gravity/test/tes/tes/tes");

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        if (!IsOnFloor())
            velocity.Y -= Gravity * (float)delta;

        Velocity = velocity;
        MoveAndSlide();
    }
}