using UnityEngine;

namespace Rotation
{
    public class RotationZ : MonoBehaviour
    {
        void Update()
        {
            transform.Rotate(new Vector3(0f, 0f, 100f) * Time.deltaTime);
        }
    }
}
