# JsonSerializerWrapperForUnity
Wrapper for Newtonsoft.Json that serialize and deserialize nested, derived types.

Usage:

Import package from package folder into unity.

#### Example of usage:

To save object:

```BaseSerializer.SaveToJson ( object_instance , file_name, path_inside_project_folder, overwrite_file );```

For example:

```BaseSerializer.SaveToJson ( apple, "redApple", @"Plants\Apples", true );```

```var loadedFruit = BaseSerializer.LoadFromJson ( pathToFruitJsonFile );```

Example 1:

Lets make some POCO's with some nesting:
```
namespace TestPackage.Gardening
{	
    public class Plant
    {
        public string plantName;
    }
	
	public class Fruit : Plant
    {
        public float tasteValue;
    }
	
	public class Apple : Fruit
    {
        public Colors color; 
    }
	
	public enum Colors {red, green, blue}
}
```

And lets save instances of those types to file:

```
public void SaveToFile()
{
	Fruit fruit = new Fruit ()
	{
		plantName = "some Fruit",
		tasteValue = 2f
	};
	
	Apple apple = new Apple ()
	{
		plantName = "some Apple",
		tasteValue = 1.2f,
		color = Colors.red
	};
	
	string fruitPath = BaseSerializer.SaveToJson ( fruit, "genericFruit", @"Plants/Fruits", true );
	string applePath = BaseSerializer.SaveToJson ( apple, "redApple", @"Plants/Fruits/Apples", true );
}
```

Now, those JSON files should look like this:

```
{
  "$type": "TestPackage.Gardening.Fruit, Tests",
  "tasteValue": 2.0,
  "plantName": "some fruit"
}

{
  "$type": "TestPackage.Gardening.Apple, Tests",
  "color": 0,
  "tasteValue": 1.2,
  "plantName": "A Test apple"
}
```

This also includes nested collections:

```
public class PlantClump : Plant
{
	public List<Plant> plantsInClump;
}
	
public void TestNestedCollections()
{
	var fruitClump = new PlantClump ()
	{
		plantsInClump = new List<Plant> ()
		{
			new Fruit(){plantName="innerPlant1-Fruit"},
			new Apple(){plantName="innerPlant2-Apple"},
			new Vegetable(){plantName ="innerVegetable"}
		}
	};
	
	string pathClump = SaveToFile ( fruitClump, fruitClump.plantName, savePath );
}

```

where json file will look like this:

```
{
  "$type": "WGUnityPackages.JsonWrapperForUnity.Tests.Gardening.PlantClump, Tests3",
  "plantsInClump": {
    "$type": "System.Collections.Generic.List`1[[WGUnityPackages.JsonWrapperForUnity.Tests.Gardening.Plant, Tests3]], mscorlib",
    "$values": [
      {
        "$type": "WGUnityPackages.JsonWrapperForUnity.Tests.Gardening.Fruit, Tests3",
        "tasteValue": 0.0,
        "plantName": "innerPlant1-Fruit"
      },
      {
        "$type": "WGUnityPackages.JsonWrapperForUnity.Tests.Gardening.Apple, Tests3",
        "color": 0,
        "tasteValue": 0.0,
        "plantName": "innerPlant2-Apple"
      },
      {
        "$type": "WGUnityPackages.JsonWrapperForUnity.Tests.Gardening.Vegetable, Tests3",
        "marketValue": 0.0,
        "plantName": "innerVegetable"
      }
    ]
  },
  "plantName": null
}
```
