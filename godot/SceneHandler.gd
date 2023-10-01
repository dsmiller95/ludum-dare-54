extends Node

var current_level
var max_levels = 3
var levels = [
	"res://Venues/DiveBar.tscn",
	"res://Venues/DiveBar.tscn",
	"res://Stage/StageOne/StageOne.tscn"
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

