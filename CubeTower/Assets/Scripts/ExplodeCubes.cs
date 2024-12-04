using UnityEngine;

public class ExplodeCubes : MonoBehaviour
{
    private bool _collisionSet;
    public GameObject explosion;
    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Cube" && !_collisionSet) {
            for (int i = collision.gameObject.transform.childCount - 1; i >= 0; i--) {
                Transform child = collision.transform.GetChild(i);
                child.gameObject.AddComponent<Rigidbody>();
                child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(70f, Vector3.up, 5f);
                child.SetParent(null);
            }

            Instantiate (explosion, new Vector3 (collision.contacts[0].point.x, collision.contacts[0].point.y, collision.contacts[0].point.z), Quaternion.identity);

            if(PlayerPrefs.GetString("music") != "No") 
                GetComponent<AudioSource>().Play();

            Camera.main.gameObject.AddComponent<CameraShake>();
            Destroy(collision.gameObject);
            _collisionSet = true;
        }
    }
}
