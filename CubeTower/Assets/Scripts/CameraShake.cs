using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform camTransform;
    private float shakeDur = 1f, decreaseFactor = 1.5f;
    private Vector3 originPos;

    private void Start() {
        camTransform = GetComponent<Transform>();
        originPos= camTransform.localPosition;
    }

    private void Update() {
        if (shakeDur > 0) {
            camTransform.localPosition = originPos + Random.insideUnitSphere * GameObject.Find("Game Controller").GetComponent<GameController>().maxVer / 100;
            shakeDur -= Time.deltaTime * decreaseFactor;
        }
        else {
            shakeDur = 0;
            camTransform.localPosition = originPos;
        }
    }

}
