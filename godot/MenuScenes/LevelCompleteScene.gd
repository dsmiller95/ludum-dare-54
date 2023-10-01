extends Node2D

var level;
# Called when the node enters the scene tree for the first time.
func _ready():
	if SceneHandler.current_level >= SceneHandler.max_levels:
		get_tree().change_scene_to_file("res://MenuScenes/EndScreen.tscn")
	else:
		level = SceneHandler.current_level;
		get_node("Label").set_text("CONGRATS WOOHOO YOU BEAT LEVEL " + str(level))


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_button_pressed():
	SceneHandler.next_level()


func _on_button_2_pressed():
	get_tree().quit()
