using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveDistance = 1;
    public float moveTime = 0.4f;
    public float colliderDistCheck = 1;
    public bool isIdle = true;
    public bool isMoving = false;
    public bool isDead = false;
    public bool isJumping = false;
    public bool jumpStart = false;
    public ParticleSystem particle = null;
    public GameObject chick = null;
    private Renderer renderer = null;
    private bool isVisible = false;

    public AudioClip audioIdle1 = null;
    public AudioClip audioIdle2 = null;
    public AudioClip audioHop = null;
    public AudioClip audioHit = null;
    public AudioClip audioSplash = null;

    public ParticleSystem splash = null;
    public bool parentedToObject = false;

    void Start() {
        renderer = chick.GetComponent<Renderer>();
    }

    void Update() {
        if (!Manager.instance.CanPlay()) {
            return;
        }

        if (isDead) {
            return;
        }

        CanIdle();
        CanMove();
        IsVisible();
    }

    void CanIdle() {
        if (isIdle) {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                checkIfIdle(270, 0, 0);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                checkIfIdle(270, 180, 0);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                checkIfIdle(270, -90, 0);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                checkIfIdle(270, 90, 0);
            }
        }
    }

    void checkIfIdle(float x, float y, float z) {
        chick.transform.rotation = Quaternion.Euler(x, y, z);

        CheckIfCanMove();
        PlayAudioClip(audioIdle1);
    }

    void CheckIfCanMove() {
        RaycastHit hit;
        Physics.Raycast(this.transform.position, -chick.transform.up, out hit, colliderDistCheck);
        Debug.DrawRay(this.transform.position, -chick.transform.up * colliderDistCheck, Color.red, 2);

        if (hit.collider == null) {
            SetMove();
        } else {
            if (hit.collider.tag == "collider") {
                Debug.Log("Hit something with collider tag!");
                isIdle = true;
            } else {
                SetMove();
            }
        }
    }

    void SetMove() {
        isIdle = false;
        isMoving = true;
        jumpStart = true;
    }

    void CanMove() {
        if (isMoving) {
            if (Input.GetKeyUp(KeyCode.UpArrow)) {
                Moving(new Vector3(transform.position.x, transform.position.y, transform.position.z + moveDistance));
                SetMoveForwardState();
            } else if (Input.GetKeyUp(KeyCode.DownArrow)) {
                Moving(new Vector3(transform.position.x, transform.position.y, transform.position.z - moveDistance));
            } else if (Input.GetKeyUp(KeyCode.LeftArrow)) {
                Moving(new Vector3(transform.position.x - moveDistance, transform.position.y, transform.position.z));
            } else if (Input.GetKeyUp(KeyCode.RightArrow)) {
                Moving(new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z));
            }
        }
    }

    void Moving(Vector3 position) {
        isIdle = false;
        isMoving = false;
        isJumping = true;
        jumpStart = false;

        PlayAudioClip(audioHop);

        LeanTween.move(this.gameObject, position, moveTime).setOnComplete(MoveComplete);
    }

    void MoveComplete() {
        isJumping = false;
        isIdle = true;

        int audioRandom = Random.Range(0, 12);

        if (audioRandom < 4) {
            PlayAudioClip(audioIdle2);
        }
    }

    void SetMoveForwardState() {
        Manager.instance.UpdateDistanceCount();
    }

    void IsVisible() {
        if (renderer.isVisible) {
            isVisible = true;
        }

        if (!renderer.isVisible && isVisible) {
            Debug.Log("Player off screen");
            GotHit();
        }
    }

    public void GotHit() {
        isDead = true;
        ParticleSystem.EmissionModule em = particle.emission;

        em.enabled = true;

        PlayAudioClip(audioHit);

        Manager.instance.GameOver();
    }

    public void GotSoaked() {
        isDead = true;
        ParticleSystem.EmissionModule em = splash.emission;

        em.enabled = true;

        PlayAudioClip(audioSplash);
        chick.SetActive(false);
        Manager.instance.GameOver();
    }

    void PlayAudioClip(AudioClip clip) {
        this.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
