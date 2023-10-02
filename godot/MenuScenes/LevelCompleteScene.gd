extends Node2D

var level
var health
# Called when the node enters the scene tree for the first time.
func _ready():
	if SceneHandler.current_level >= SceneHandler.max_levels:
		get_tree().change_scene_to_file("res://MenuScenes/EndScreen.tscn")
	else:
		level = SceneHandler.current_level;
		health = get_node("Sprite2D").health
		var time = SceneHandler.get_time_to_complete()
		get_node("Label").set_text("You found your date with " + str(health) + "% of your drink left in " + str(time) + " seconds! Level " + str(level) + " complete!")


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func _on_button_pressed():
	SceneHandler.next_level()


func _on_button_2_pressed():
	get_tree().change_scene_to_file("res://MenuScenes/StartScreen.tscn")
