using NUnit.Framework;
using UnityEngine;
using WGUnityPackages.JsonWrapperForUnity;
using WGUnityPackages.JsonWrapperForUnity.Tests;

public class TestSerialzation
{
    public void TestSaveToJson()
    {
        Fruit fruit = new Fruit()
        {
            plantName = "some fruit",
            tasteValue = 2f
        };

        Assert.IsInstanceOf<Plant>(fruit, "fruit is not plant");

        string fruitPath = BaseSerializer.SaveToJson<Fruit>(
            fruit, 
            fruit.plantName, 
            @"Tests/Plants", 
            overwrite: true);

        var loaded = BaseSerializer.LoadFromJson(fruitPath);
        Debug.Log(loaded);
        Assert.IsInstanceOf<Fruit>(loaded, "loaded is not fruit");
        Assert.AreEqual(
            (loaded as Fruit).plantName, 
            "some fruit", 
            message: "Deserialized fruit has wrong name: " + (loaded as Fruit).plantName);
    }

    [Test]
    public void TestLoadJson()
    {
        var path = "";
        var laodedObejct = BaseSerializer.LoadFromJson(path);
        Assert.IsNull(laodedObejct);
    }
}
