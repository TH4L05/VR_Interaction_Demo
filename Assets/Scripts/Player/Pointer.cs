
using UnityEngine;

namespace eecon_lab
{
    public class Pointer : MonoBehaviour
    {
        private RaycastHit hit;
        [SerializeField] private bool isEnabled = true;
        private LineRenderer lineRenderer;
        [SerializeField] private float minRayDistance = 5f;
        [SerializeField] private GameObject dot;
        [SerializeField] private GameObject customStartingPoint;
        public Vector3 hitPosition;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null) return;
            lineRenderer.positionCount = 2;
        }

        private void Update()
        {
            if (!enabled) return;
            DrawRayCast();
        }

        /*private void LateUpdate()
        {
            if (!enabled) return;
            //LineRendererUpdate(transform.position, hitPosition);
        }*/

        private void DrawRayCast()
        {
            //if (controller == null) return;

            Vector3 rayOrigin = transform.position;
            Vector3 rayDirection = transform.forward;
            Ray ray = new Ray(rayOrigin, rayDirection);
            hitPosition = transform.position + (transform.forward * 1f);
            if (dot != null) dot.SetActive(false);
            if (Physics.Raycast(ray, out hit, minRayDistance))
            {
                //hitPosition = hit.point;
                //UpdateSpotPostion(hitPosition);
                Debug.Log(hit.collider.name);
            }
            //Debug.DrawRay(rayOrigin, rayDirection * 5f,Color.red);
            //Debug.DrawLine(transform.position, hitPosition, Color.cyan);       
        }

        /*private void InteractableCheck(RaycastHit hit)
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                var trigger = hit.collider.GetComponent<Trigger>();
                trigger.OnTriggerEnterEvent?.Invoke();
            }
        }*/

        private void LineRendererUpdate(Vector3 startpos, Vector3 hitpos)
        {

            if (lineRenderer == null) return;

            if (customStartingPoint != null)
            {
                lineRenderer.SetPosition(0, customStartingPoint.transform.position);
            }
            else
            {
                lineRenderer.SetPosition(0, startpos);
            }


            lineRenderer.SetPosition(1, hitpos);
        }

        private void UpdateSpotPostion(Vector3 position)
        {
            if (dot == null) return;
            dot.SetActive(true);
            dot.transform.position = position;
        }
    }
}

