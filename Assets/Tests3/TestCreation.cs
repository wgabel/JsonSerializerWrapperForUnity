using NUnit.Framework;
using System.Collections.Generic;
using WGUnityPackages.JsonWrapperForUnity;
using WGUnityPackages.JsonWrapperForUnity.Tests;
using WGUnityPackages.JsonWrapperForUnity.Tests.Gardening;

namespace Tests.Creations
{
    public class TestCreation
    {
        private readonly string savePath = @"Tests3/SerializedFiles/Plants";

        [Test]
        public void TestCreateSimpleObjectFruit ()
        {
            Fruit fruit = new Fruit ()
            {
                plantName = "some fruit",
                tasteValue = 2f
            };
            Assert.IsNotNull ( fruit );
            Assert.IsInstanceOf ( typeof ( Fruit ), fruit );
            Assert.IsInstanceOf<Plant> ( fruit, "fruit is not plant" );
            string pathFruit = SaveToFile ( fruit, fruit.plantName, savePath );
            var loadedFruit = BaseSerializer.LoadFromJson ( pathFruit );
            Assert.IsInstanceOf<Fruit> ( loadedFruit, "loaded is not fruit" );
            Assert.AreEqual (
                ( loadedFruit as Fruit ).plantName,
                "some fruit",
                message: "Deserialized fruit has wrong name: " + ( loadedFruit as Fruit ).plantName );
        }

        [Test]
        public void TestCreateSimpleObjectApple ()
        {
            Apple apple = new Apple ()
            {
                plantName = "A Test apple",
                tasteValue = 1.2f,
                color = Colors.red
            };
            string pathApple = SaveToFile ( apple, apple.plantName, savePath );
            var loadedFruit = BaseSerializer.LoadFromJson ( pathApple );
            Assert.IsInstanceOf<Apple> ( loadedFruit, "loaded is not fruit" );
            Assert.AreEqual (
                ( loadedFruit as Apple ).plantName,
                "A Test apple",
                message: "Deserialized fruit has wrong name: " + ( loadedFruit as Apple ).plantName );
            Assert.AreEqual (
                ( loadedFruit as Apple ).tasteValue,
                1.2f,
                message: "Deserialized fruit has wrong taste value: " + ( loadedFruit as Apple ).tasteValue );
            Assert.AreEqual (
                ( loadedFruit as Apple ).color,
                Colors.red,
                message: "Deserialized fruit has wrong color: " + ( loadedFruit as Apple ).color );
        }

        [Test]
        public void TestCorrectSaveWithNestedTypes ()
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
            Assert.IsNotNull ( pathClump );
            var loadedObject = BaseSerializer.LoadFromJson ( pathClump );

            Assert.IsNotNull ( loadedObject );
            Assert.IsInstanceOf ( typeof ( PlantClump ), loadedObject );

            var clump = loadedObject as PlantClump;
            Assert.IsNotNull ( clump.plantsInClump );
            Assert.AreEqual ( 3, clump.plantsInClump.Count );
            Assert.IsNotNull ( clump.plantsInClump[0] );
            Assert.IsInstanceOf ( typeof ( Fruit ), clump.plantsInClump[0] );
            Assert.AreEqual ( clump.plantsInClump[0].plantName, "innerPlant1-Fruit" );

            Assert.IsNotNull ( clump.plantsInClump[1] );
            Assert.IsInstanceOf ( typeof ( Fruit ), clump.plantsInClump[1] );
            Assert.AreEqual ( clump.plantsInClump[1].plantName, "innerPlant2-Apple" );

            Assert.IsNotNull ( clump.plantsInClump[2] );
            Assert.IsInstanceOf ( typeof ( Vegetable ), clump.plantsInClump[2] );
            Assert.AreEqual ( clump.plantsInClump[2].plantName, "innerVegetable" );
        }

        private string SaveToFile<T> ( T objectToSave, string objectName, string path, bool overwrite = true )
        {
            return BaseSerializer.SaveToJson<T> (
                objectToSave,
                objectName,
                savePath,
                overwrite );
        }


    }
}
