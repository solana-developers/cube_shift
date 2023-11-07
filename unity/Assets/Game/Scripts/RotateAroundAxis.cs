
using UnityEngine;

namespace SolPlay.Deeplinks
{
    public class RotateAroundAxis : MonoBehaviour
    {
        public float Speed = 1;
        public Vector3 RotationAxis = Vector3.zero;

        void Update()
        {
            transform.Rotate(RotationAxis, Speed * Time.deltaTime);
        }
    }
}