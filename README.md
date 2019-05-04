# JsonSerializerWrapperForUnity
Wrapper for Newtonsoft.Json that serialize and deserialize nested, derived types.

Usage:

Import package from package folder into unity.

Example of usage:

To save object:
BaseSerializer.SaveToJson ( object_instance , file_name, path_inside_project_folder, overwrite_file );
For example:
BaseSerializer.SaveToJson ( apple, "redApple", @"Plants\Apples", true );