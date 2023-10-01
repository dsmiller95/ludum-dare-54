extends Node

@export var body: RigidBody2D 

@export var bumpEffect: EventAsset
@export var spillEffect: EventAsset
var instance: EventInstance

# Called when the node enters the scene tree for the first time.
func _enter_tree() -> void:
	body.connect("SoftCollision", _play_soft_collision)
	body.connect("HealthDepleted", _play_hard_collision)

func _exit_tree()-> void:
	body.disconnect("SoftCollision", _play_soft_collision)
	body.disconnect("HealthDepleted", _play_hard_collision)
		
func _play_soft_collision():
	instance = FMODRuntime.create_instance(bumpEffect)
	instance.start()
	instance.release()
		
func _play_hard_collision():
	instance = FMODRuntime.create_instance(spillEffect)
	instance.start()
	instance.release()
