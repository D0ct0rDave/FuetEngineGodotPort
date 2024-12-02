# FESpriteImportPlugin.gd
tool
extends EditorImportPlugin

func get_importer_name():
	return "FuetEngine.Sprite"

func get_visible_name():
	return "FESprite"
	
func get_recognized_extensions():
	return ["spr"]
	
func get_save_extension():
	return "tres"

func get_resource_type():
	return "Text"
	
func get_preset_count():
	return 0

func get_import_options(pepe):
	return []

func import(source_file, save_path, options, r_platform_variants, r_gen_files):
	var file = File.new()
	var err = file.open(source_file, File.READ)
	if err != OK:
		return err
	var line = file.get_buffer(file.get_len())
	file.close()
	var saveExtension = get_save_extension()
	var fileName = "%s.%s" % [save_path, saveExtension]
	return ResourceSaver.save(fileName, line)
