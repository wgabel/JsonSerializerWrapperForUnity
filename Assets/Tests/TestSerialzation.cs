using NUnit.Framework;
using WGPackages.JsonWrapperForUnity;
using WGUnityPackages.JsonWrapperForUnity.Tests;

public class TestSerialzation
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestSerialzationSimplePasses()
    {
        Fruit fruit = new Fruit () {plantName ="some fruit", tasteValue = 2f };
        
        Assert.IsInstanceOf<Plant> (fruit, "fruit is not plant");

        string fruitPath = BaseSerializer.SaveToJson<Fruit> ( fruit, fruit.plantName, @"Tests/Plants", overwrite: true );

        var loaded = BaseSerializer.LoadFromJson ( fruitPath );

        Assert.IsInstanceOf<Fruit> (loaded, "loaded is not fruit");
        Assert.AreEqual ( (loaded as Fruit).plantName, "some fruit", message:"Deserialized fruit has wrong name: " + ( loaded as Fruit ).plantName );
    }
}
