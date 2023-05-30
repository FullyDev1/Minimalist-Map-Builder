using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minimalist.MapBuilder.SampleScene
{
    public class DirectionalLightBhv : MonoBehaviour
    {
        [Range(.1f, 10f)]
        public float rotationSpeed = 1;

        private Transform _transform;

        private void Awake()
        {
            _transform = this.GetComponent<Transform>();
        }

        private void Update()
        {
            _transform.RotateAround(Vector3.zero, Vector3.up - Vector3.forward, Time.deltaTime * rotationSpeed);

            _transform.LookAt(Vector3.zero);
        }
    }
}
