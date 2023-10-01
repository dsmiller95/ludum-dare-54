extends Node

@export var body: RigidBody2D 

@export var event: EventAsset
var instance: EventInstance

# Called when the node enters the scene tree for the first time.
func _enter_tree() -> void:
	body.body_entered.connect(_play_one_shot)

func _exit_tree()-> void:
	body.body_entered.disconnect(_play_one_shot)
	
func _play_one_shot(body):
	instance = FMODRuntime.create_instance(event)
	instance.start()
	instance.release()
