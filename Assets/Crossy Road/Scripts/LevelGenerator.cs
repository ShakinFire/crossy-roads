using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> platform = new List<GameObject>();
    public List<float> height = new List<float>();
    private int rndRange = 0;
    private float lastPos = 0;
    private float lastScale = 0;

    public void RandomGenerator() {
        rndRange = Random.Range(0, platform.Count);
        CreatelevelObject(platform[rndRange], height[rndRange]);
    }

    public void CreatelevelObject(GameObject obj, float height) {
        GameObject generatedPlatform = Instantiate<GameObject>(obj);

        float zOffset = lastPos + (lastScale * 0.5f);
        zOffset += generatedPlatform.transform.localScale.z * 0.5f;
        Vector3 pos = new Vector3(0, height, zOffset);

        generatedPlatform.transform.position = pos;

        lastPos = generatedPlatform.transform.position.z;
        lastScale = generatedPlatform.transform.localScale.z;

        generatedPlatform.transform.parent = this.transform;
    }
}
