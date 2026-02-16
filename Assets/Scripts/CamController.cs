    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        // Define positions and rotations for camera
        public Vector3 externalPos = new Vector3(0, 1.6f, -4.5f);
        public Vector3 externalRot = new Vector3(14, 0, 0);

        public Vector3 cockpitPos = new Vector3(-0.3f, 0.63f, 0.07f);
        public Vector3 cockpitRot = new Vector3(5.59f, 0, 0);

        public Vector3 bonnetPos = new Vector3(0.07f, 0.63f, 0.82f);
        public Vector3 bonnetRot = new Vector3(-5.5f, 0, 0);

        // Variable to track current camera position
        public int checkPosition = 1;

        void Update()
        {
            // Check if "C" key is pressed
            if (Input.GetKeyDown(KeyCode.C))
            {
                SwitchCamPos();
            }
        }

        // Function to switch camera positions
        void SwitchCamPos()
        {
            // Switch between the three positions and rotations
            if (checkPosition == 1)
            {
                // Move to cockpit view
                transform.localPosition = cockpitPos;
                transform.localRotation = Quaternion.Euler(cockpitRot);
                checkPosition = 2;
            }
            else if (checkPosition == 2)
            {
                // Move to bonnet view
                transform.localPosition = bonnetPos;
                transform.localRotation = Quaternion.Euler(bonnetRot);
                checkPosition = 3;
            }
            else if (checkPosition == 3)
            {
                // Move to external view
                transform.localPosition = externalPos;
                transform.localRotation = Quaternion.Euler(externalRot);
                checkPosition = 1;
            }
        }
    }
