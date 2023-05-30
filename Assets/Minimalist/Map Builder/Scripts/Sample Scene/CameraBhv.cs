using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minimalist.MapBuilder.SampleScene
{
    [ExecuteInEditMode]
    public class CameraBhv : MonoBehaviour
    {
        public Transform target;

        private void Update()
        {
            this.transform.LookAt(target);
        }
    }
}