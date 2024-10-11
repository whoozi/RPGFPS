using Godot;

namespace RPGFPS;

public partial class Mob : CharacterBody3D
{
    [Export] private AnimationPlayer _animPlayer;
    [Export] private Sprite3D _sprite;
    [Export] protected float MovementSpeed = 5f;

    protected readonly float Gravity = (float)ProjectSettings.GetSettingWithOverride("physics/3d/default_gravity");

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        if (!IsOnFloor())
            velocity.Y -= Gravity * (float)delta;

        Velocity = velocity;
        MoveAndSlide();
    }
}