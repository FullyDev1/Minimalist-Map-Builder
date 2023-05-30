using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minimalist.MapBuilder
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(BoxCollider))]
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
    public class EditModeInstanceBhv : MonoBehaviour
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    {
        // Public fields
        [HideInInspector]
        public Material material;
        [HideInInspector]
        public Color color = Color.white;
        [HideInInspector]
        public Vector3 scale = Vector3.one;

        // Private fields
        private Transform _transform;
        private Renderer _renderer;

        private void Awake()
        {
            _transform = this.GetComponent<Transform>();

            _transform.localScale = this.scale;

            _renderer = this.GetComponent<Renderer>();

            if (material == null)
            {
                material = _renderer.sharedMaterial;
            }

            _renderer.material = new Material(material) { color = this.color };
        }

        public static bool operator ==(EditModeInstanceBhv a, EditModeInstanceBhv b)
        {
            return a.color == b.color && a.scale == b.scale;
        }

        public static bool operator !=(EditModeInstanceBhv a, EditModeInstanceBhv b)
        {
            return a.color != b.color || a.scale != b.scale;
        }
    }
}