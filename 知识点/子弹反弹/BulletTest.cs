using UnityEngine;
public class BulletTest : MonoBehaviour {
    // Use this for initialization
    private Rigidbody rigidbody;
    void Start ()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        m_preVelocity= transform.forward * n;
        rigidbody.velocity = m_preVelocity;

    }
    public Vector3 m_preVelocity = Vector3.zero;//上一帧速度
    public float n=1;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "wall")
        {
            ContactPoint contactPoint = collision.contacts[0];
            Vector3 newDir = Vector3.zero;
            Vector3 curDir = transform.TransformDirection(Vector3.forward);
            newDir = Vector3.Reflect(curDir, contactPoint.normal);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, newDir);
            transform.rotation = rotation;

            rigidbody.velocity = newDir.normalized * m_preVelocity.x / m_preVelocity.normalized.x;
        }
    }
}
