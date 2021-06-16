using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 1.0f;
    public float moveDirection = 0;
    public bool parentOnTrigger = true;
    public bool hitboxOnTrigger = false;
    public GameObject moverObject = null;
    
    private Renderer renderer = null;
    private bool isVisible = false;

    void Start() {
        renderer = moverObject.GetComponent<Renderer>();
    }

    void Update() {
        this.transform.Translate(speed * Time.deltaTime, 0, 0);
        IsVisible();
    }

    void IsVisible() {
        if (renderer.isVisible) {
            isVisible = true;
        }

        if (!renderer.isVisible && isVisible) {
            Debug.Log("Remove object!");

            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            if (parentOnTrigger) {
                other.transform.parent = this.transform;
                other.GetComponent<PlayerController>().parentedToObject = true;
            }

            if (hitboxOnTrigger) {
                other.GetComponent<PlayerController>().GotHit();
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            if (parentOnTrigger) {
                other.transform.parent = null;
                other.GetComponent<PlayerController>().parentedToObject = false;
            }
        }
    }
}
