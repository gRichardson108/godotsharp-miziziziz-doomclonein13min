# Doom clone in 13 Minutes by Miziziziz, now in csharp
First and foremost, this project and its assets were taken from Miziziziz's excellent youtube video: "How to make a Doom clone in Godot in 13 minutes", which can be found here: https://www.youtube.com/watch?v=LbyyjmOji0M.

All I've done is convert the godotscript files into csharp. Most of this is pretty straightforward. The only thing that took a bit of figuring out was how to mimic his usage of `yield` inside of Player.cs.

## gdscript version of Player _ready():
```gdscript
onready var anim_player = $AnimationPlayer
onready var raycast = $RayCast
func _ready():
    Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
    yield(get_tree(), "idle_frame")
    get_tree().call_group("zombies", "set_player", self)
```

## Csharp version:
Rather than use the `onready` gdscript notation, we use private variables and set them inside of _Ready(). I don't know if there are any downsides to this since I'm new to godot, but it seems to work.

Additionally, we make _Ready() `async`, and mimic the `yield` by calling `await ToSignal(GetTree(), "idle_frame")`. If you don't have this set up, the zombies won't exist when the next line attempts to call "SetPlayer" for them, so they won't move towards the player.

```cs
private AnimationPlayer animPlayer;
private RayCast rayCast;
public override async void _Ready()
{
    animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    rayCast = GetNode<RayCast>("RayCast");
    Input.SetMouseMode(Input.MouseMode.Captured);

    await ToSignal(GetTree(), "idle_frame");
    GetTree().CallGroup("zombies", "SetPlayer", this);
}
```

## Thanks
Thanks again to Miziziziz and his excellent youtube channel for sparking my interest in godot.