using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using WGUnityPackages.JsonWrapperForUnity;
using System.Linq;

namespace WGUnityPackages.JsonWrapperForUnity.Tests
{
    public class TestSerializer : MonoBehaviour
    {
        [ContextMenu("Test Fruit creation")]
        object TestFruitCreation()
        {
            Fruit fruit = new Fruit()
            {
                plantName = "some fruit",
                tasteValue = 2f
            };
            Assert.IsNotNull(fruit);
            Assert.IsInstanceOf(typeof(Fruit), fruit);
            return fruit;
        }

        [ContextMenu("[LOAD]Test empty path")]
        void TestLoadEmptyPath()
        {
            string path = @"";
            var loadedObject = BaseSerializer.LoadFromJson(path);
            Assert.IsNull(loadedObject);
        }

        [ContextMenu("[LOAD]Test invalid Path")]
        void TestLoadInvalidPath()
        {
            string path = @"invalid Path";
            var loadedObject = BaseSerializer.LoadFromJson(path);
            Assert.IsNull(loadedObject);
        }

        [ContextMenu("[LOAD]Test file not exist")]
        void TestLoadFileNotExist()
        {
            string path = @"c:\";
            var loadedObject = BaseSerializer.LoadFromJson(path);
            Assert.IsNull(loadedObject);
        }

        [ContextMenu("[LOAD]Test path is not a file")]
        void TestLoadPathNotFile()
        {
            string path = @"C:\projekty\JsonSerializerWrapperForUnity\Assets\Tests";
            var loadedObject = BaseSerializer.LoadFromJson(path);
            Assert.IsNull(loadedObject);
        }

        [ContextMenu("[LOAD]Test invalid file")]
        void TestLoadInvalidFile()
        {
            
            string path = @"C:\projekty\JsonSerializerWrapperForUnity\Assets\Tests\Plants2\WrongFile.bmp";
            var loadedObject = BaseSerializer.LoadFromJson(path);
            Assert.IsNull(loadedObject);
        }

        [ContextMenu("[LOAD]TestCorrectFile")]
        object TestCorrectFile()
        {
            string path = @"C:\projekty\JsonSerializerWrapperForUnity\Assets\Tests\Plants2\WGUnityPackages.JsonWrapperForUnity.Tests.Fruit_some fruit.json";
            var loadedObject = BaseSerializer.LoadFromJson(path);
            Assert.IsNotNull(loadedObject);
            return loadedObject;
        }

        [ContextMenu("[LOAD]TestCorrecParsedFile")]
        void TestCorrectParsedFle()
        {
            var loadedObject = TestCorrectFile();
            Assert.IsInstanceOf(typeof(Fruit), loadedObject);
        }

        [ContextMenu("[SAVE]TestCorrectSave")]
        void TestSimpleSave()
        {
            var fruit = TestFruitCreation();
            string savePath = BaseSerializer.SaveToJson<Fruit>((Fruit)fruit, "fruit_1", @"Tests\Plants2", true);
            Assert.IsNotNull(savePath);
            Assert.IsNotEmpty(savePath);
        }

        [ContextMenu("[SAVE]TestSaveFileExistsError")]
        void TestSaveFileExistsError()
        {
            var fruit = TestFruitCreation();
            string savePath = BaseSerializer.SaveToJson<Fruit>((Fruit)fruit, "fruit_1", @"Tests\Plants2", false);
            Assert.IsNotNull(savePath);
            Assert.IsNotEmpty(savePath);
            savePath = BaseSerializer.SaveToJson<Fruit>((Fruit)fruit, "fruit_1", @"Tests\Plants2", false);
            Assert.IsNotNull(savePath);
            Assert.IsNotEmpty(savePath);
        }

        [ContextMenu("[SAVE]TestSaveCorrectParsedType")]
        void TestSaveCorrectType()
        {
            var fruit = TestFruitCreation();
            string savePath = BaseSerializer.SaveToJson<Fruit>((Fruit)fruit, "fruit_1", @"Tests\Plants2", true);
            var parsedObejct = BaseSerializer.LoadFromJson(savePath);
            Assert.IsInstanceOf(typeof(Fruit), parsedObejct);
        }

        [ContextMenu("[SAVE]Test Save Correct Parsed Type From Object")]
        void TestSaveCorrectTypeFromObject()
        {
            var fruit = TestFruitCreation();
            string savePath = BaseSerializer.SaveToJson(fruit, "fruit_1", @"Tests\Plants2", true);
            var parsedObejct = BaseSerializer.LoadFromJson(savePath);
            Assert.IsInstanceOf(typeof(Fruit), parsedObejct);
        }

        [ContextMenu("[BOTH] Test Correct nested types")]
        void TestCorrectSaveWithNestedTypes()
        {
            var fruitClump = new PlantClump()
            {
                plantsInClump = new List<Plant>()
                {
                    new Fruit(){plantName="innerPlant1"},
                    new Fruit(){plantName="innerPlant2"},
                    new Vegetable(){plantName ="innerVegetable"}
                }
            };

            string savedPath = BaseSerializer.SaveToJson(fruitClump, "fruitsClump_1", @"Tests\Plants3", true );
            Assert.IsNotNull(savedPath);
            var loadedObject = BaseSerializer.LoadFromJson(savedPath);
            Assert.IsNotNull(loadedObject);
            Assert.IsInstanceOf(typeof(PlantClump), loadedObject);
            var clump = loadedObject as PlantClump;
            Assert.IsNotNull(clump.plantsInClump);
            Assert.AreEqual(3, clump.plantsInClump.Count );
            Assert.IsNotNull(clump.plantsInClump[0]);
            Assert.IsInstanceOf(typeof(Fruit), clump.plantsInClump[0]);
            Assert.AreEqual(clump.plantsInClump[0].plantName, "innerPlant1" );

            Assert.IsNotNull(clump.plantsInClump[1]);
            Assert.IsInstanceOf(typeof(Fruit), clump.plantsInClump[1]);
            Assert.AreEqual(clump.plantsInClump[1].plantName, "innerPlant2" );

            Assert.IsNotNull(clump.plantsInClump[2]);
            Assert.IsInstanceOf(typeof(Vegetable), clump.plantsInClump[2]);
            Assert.AreEqual(clump.plantsInClump[2].plantName, "innerVegetable" );

        }
    }
}