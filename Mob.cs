using Godot;

namespace RPGFPS;

public partial class Mob : CharacterBody3D
{
    [Export] protected AnimationPlayer _animPlayer;
    [Export] protected Sprite3D _sprite;
    [Export] protected float _movementSpeed = 5f;

    protected readonly float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        if (!IsOnFloor())
            velocity.Y -= _gravity * (float)delta;

        Velocity = velocity;
        MoveAndSlide();
    }
}
