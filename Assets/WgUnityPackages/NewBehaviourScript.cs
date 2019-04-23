using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WGUnityPackages.JsonWrapperForUnity;
using WGUnityPackages.JsonWrapperForUnity.Tests;

public class NewBehaviourScript : MonoBehaviour
{
    public string path = @"Tests\Plants2";
    [ContextMenu("Test")]
    void XXX()
    {
        Fruit fruit = new Fruit()
        {
            plantName = "some fruit",
            tasteValue = 2f
        };

        string fruitPath = BaseSerializer.SaveToJson<Fruit>(
            fruit, 
            fruit.plantName, 
            path, 
            overwrite: true);

        Debug.Log(fruitPath);
    }
}
