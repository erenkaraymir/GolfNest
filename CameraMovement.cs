using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GolfNest
{
    public class CameraMovement : MonoBehaviour
    {

        public static CameraMovement instance;
        public Vector3 offset;
        public   Camera MainCamera;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            MainCamera = Camera.main;
        }
        public void CameraMove(Vector3 target)
        {
            transform.position = Vector3.Lerp(transform.position, target + offset, Time.deltaTime * 10);
        }
    }

}
