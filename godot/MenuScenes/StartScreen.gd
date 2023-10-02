extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	SceneHandler.current_level = 1


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_start_button_pressed():
	get_tree().change_scene_to_file("res://Stage/StageOne/StageOne.tscn")


func _on_how_to_button_pressed():
	get_tree().change_scene_to_file("res://MenuScenes/HowToScene.tscn")


func _on_quit_button_pressed():
	get_tree().quit()
