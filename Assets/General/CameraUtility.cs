using UnityEngine;

namespace BenScr.UnityStack
{
    public static class CameraUtility
    {
        // Returns the world mouse position within 2D space.
        public static Vector2 GetMousePosition2D(Camera camera = null)
        {
            camera ??= Camera.main;

            if (camera == null)
            {
                Debug.LogWarning("No Camera found. Returning Vector2.zero.");
                return Vector2.zero;
            }

            return camera.ScreenToWorldPoint(Input.mousePosition);
        }

        // Returns the mouse position in world space, based wether the camera is orthographic or perspective.
        public static Vector3 GetMousePosition(Camera camera = null)
        {
            camera ??= Camera.main;

            if (camera == null)
            {
                Debug.LogWarning("No Camera found. Returning Vector3.zero.");
                return Vector3.zero;
            }

            bool isOrtho = camera.orthographic;

            if (isOrtho) return GetMousePosition2D(camera);
            else
                return GetMousePosition3D(camera);
        }

        // Returns the world mouse position within 3D space.
        public static Vector3 GetMousePosition3D(Camera camera = null)
        {
            camera ??= Camera.main;

            if (camera == null)
            {
                Debug.LogWarning("No Camera found. Returning Vector3.zero.");
                return Vector3.zero;
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                return hit.point;
            }

            return camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(cam.transform.position.z)));
        }

        public static Vector3 GetMousePositionOnGridPlane()
        {
            var cam = Camera.main;
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            var plane = new Plane(Vector3.up, Vector3.zero);

            if (plane.Raycast(ray, out float enter))
                return ray.GetPoint(enter);

            return Vector3.zero;
        }
    }
}
