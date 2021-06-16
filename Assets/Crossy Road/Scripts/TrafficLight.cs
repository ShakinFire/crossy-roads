using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public GameObject trafficLight = null;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "train") {
            trafficLight.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "train") {
            trafficLight.SetActive(false);
        }
    }
}
