using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Minimalist.MapBuilder
{
    [CustomEditor(typeof(MapBuilder))]
    public class MapBuilderEditor : Editor
    {
        private Tool _previousTool;
        private EditModeInstanceBhv _dragOnsetTileInstance;
        private bool _editState;

        public override void OnInspectorGUI()
        {
            MapBuilder mapBuilder = target as MapBuilder;

            GUI.enabled = mapBuilder.EditState;

            base.OnInspectorGUI();

            GUILayout.Space(15);

            string buttonLabel = (mapBuilder.EditState ? "Exit" : "Enter") + " edit mode";

            GUI.enabled = true;

            _editState = GUILayout.Toggle(mapBuilder.EditState, buttonLabel, GUI.skin.button);

            if (_editState != mapBuilder.EditState)
            {
                if (_editState)
                {
                    mapBuilder.DoneInstantiating = false;

                    _previousTool = Tools.current;

                    Tools.current = Tool.None;
                }
                else
                {
                    Tools.current = _previousTool;
                }

                EditorWindow.GetWindow<SceneView>().drawGizmos = true;

                mapBuilder.EditState = _editState;
            }

            GUILayout.BeginHorizontal();

            GUI.enabled = false;

            GUIStyle labelStyle = GUI.skin.button;

            labelStyle.alignment = TextAnchor.MiddleCenter;

            GUILayout.Label("Left mouse click to place", labelStyle);

            GUILayout.Label("Right mouse click to destroy", labelStyle);

            GUILayout.EndHorizontal();

            GUI.enabled = _editState;

            if (GUILayout.Button("Clear all tiles", GUI.skin.button))
            {
                mapBuilder.DestroyAllTiles();
            }
        }

        private void OnSceneGUI()
        {
            MapBuilder mapBuilder = target as MapBuilder;

            if (mapBuilder == null || !mapBuilder.EditState || !mapBuilder.DoneInstantiating)
            {
                return;
            }

            int controlId = GUIUtility.GetControlID(FocusType.Passive);

            HandleUtility.AddDefaultControl(controlId);

            Event e = Event.current;

            if (!e.alt && !e.shift && !e.control)
            {
                Vector3 mousePosition = e.mousePosition;

                Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

                HandleUtility.FindNearestVertex(mousePosition, mapBuilder.GridTransforms, out Vector3 nearestVertex);

                mapBuilder.HoveredPosition = nearestVertex;

                Bounds bounds = new Bounds(nearestVertex + Vector3.up * mapBuilder.tileScale.y / 2f, mapBuilder.tileScale);

                Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);

                // left mouse button
                if (e.button == 0)
                {
                    switch (e.type)
                    {
                        case EventType.MouseDown:

                            mapBuilder.Dragging = bounds.IntersectRay(ray);

                            goto case EventType.MouseDrag;

                        case EventType.MouseDrag:

                            if (bounds.IntersectRay(ray) && !mapBuilder.SelectedPositions.Contains(nearestVertex))
                            {
                                mapBuilder.SelectedPositions.Add(nearestVertex);

                                mapBuilder.UpdateGridMesh();
                            }

                            break;

                        case EventType.MouseLeaveWindow:

                        case EventType.MouseUp:

                            mapBuilder.InstantiateTiles();

                            mapBuilder.Dragging = false;

                            break;
                    }

                    InternalEditorUtility.RepaintAllViews();
                }

                // right mouse button
                else if (e.button == 1 && hit.collider != null)
                {
                    switch (e.type)
                    {
                        case EventType.MouseDown:

                            GUIUtility.hotControl = controlId;

                            _dragOnsetTileInstance = hit.transform.GetComponent<EditModeInstanceBhv>();

                            break;

                        case EventType.MouseDrag:

                        case EventType.MouseUp:

                            EditModeInstanceBhv tileInstance = hit.transform.GetComponent<EditModeInstanceBhv>();

                            if (tileInstance == _dragOnsetTileInstance)
                            {
                                mapBuilder.DestroyTile(hit.collider);
                            }

                            break;
                    }

                    InternalEditorUtility.RepaintAllViews();
                }
            }
        }
    }
}