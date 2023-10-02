extends Node

var current_level
var max_levels = 5
var levels = [
	"res://Stage/StageOne/StageOne.tscn",
	"res://Stage/StageTwo/StageTwo.tscn",
	"res://Stage/StageTwo/StageTwoOne.tscn",
	"res://Stage/StageThree/StageThree.tscn",
	"res://Stage/StageFour/StageFour.tscn",
]

var last_level_start_time = 0;

var health;
# Called when the node enters the scene tree for the first time.
func _ready():
	current_level = 1
	last_level_start_time = Time.get_ticks_msec()
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass

func get_time_to_complete():
	var ms = Time.get_ticks_msec() - last_level_start_time
	return ms / 1000

func next_level():
	last_level_start_time = Time.get_ticks_msec()
	current_level += 1
	get_tree().change_scene_to_file(levels[current_level-1])
	
func restart_level():
	last_level_start_time = Time.get_ticks_msec()
	get_tree().change_scene_to_file(levels[current_level-1])
