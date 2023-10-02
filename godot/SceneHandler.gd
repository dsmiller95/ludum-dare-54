extends Node

var current_level
var max_levels = 2
var levels = [
	"res://Stage/StageOne/StageOne.tscn",
	"res://Stage/StageTwo/StageTwo.tscn"
]

var health;
# Called when the node enters the scene tree for the first time.
func _ready():
	current_level = 1
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	
func next_level():
	current_level += 1
	get_tree().change_scene_to_file(levels[current_level-1])
	
func restart_level():
	get_tree().change_scene_to_file(levels[current_level-1])
