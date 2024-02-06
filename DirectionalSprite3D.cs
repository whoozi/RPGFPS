using Godot;

namespace RPGFPS;

public partial class DirectionalSprite3D : Sprite3D {
	public override void _Ready() {
		RegionEnabled = true;
		Vframes = 1;
	}

	public override void _Process(double delta) {
		var rowHeight = Texture.GetHeight() / 5;
		RegionRect = new Rect2(0, rowHeight * CalculateRow(), Texture.GetWidth(), rowHeight);
	}

	private int CalculateRow() {
		var dir = GetViewport().GetCamera3D().GlobalBasis.Z;
		var forward = GlobalBasis.Z.Dot(dir);
		var left = GlobalBasis.X.Dot(dir);

		int row;
		FlipH = false;

		switch (forward) {
			case < -0.85f:
				row = 0; // front
				break;
			case > 0.85f:
				row = 4; // back
				break;
			default: {
				FlipH = left > 0;
				if (Mathf.Abs(forward) < 0.3f)
					row = 2; // left
				else if (forward < 0)
					row = 1; // front left
				else
					row = 3; // back left
				break;
			}
		}

		return row;
	}
}