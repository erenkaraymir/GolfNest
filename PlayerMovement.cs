using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GolfNest
{
    public class PlayerMovement : MonoBehaviour
    {
        Rigidbody rb;
        Vector2 FirstPos;
        Vector2 LastPos;
        Ray ray;
        RaycastHit hit;
        public bool isMove;
        [SerializeField]
        private float power;
       public Transform CamRotateObject;
        float changeValue;
      public  bool onGround;
        private void Awake()
        {
            rb = GetComponentInChildren<Rigidbody>();
        }
        private void Update()
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                ray = CameraMovement.instance.MainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag != "Player")
                    {
                        print("c");
                        isMove = false;
                        Vector3 mousePos = Input.mousePosition;
                        mousePos.z = Camera.main.nearClipPlane;
                        FirstPos = Camera.main.ScreenToViewportPoint(mousePos);
                        FirstPos.x= Mathf.Clamp01(FirstPos.x);
                        if(FirstPos.x>.5f)
                            changeValue += 30;
                        else
                            changeValue -=30;
                    }
                }
                else
                {
                    isMove = false;
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = Camera.main.nearClipPlane;
                    FirstPos = Camera.main.ScreenToViewportPoint(mousePos);
                    FirstPos.x = Mathf.Clamp01(FirstPos.x);
                    if (FirstPos.x > .5f)
                        changeValue += 30;
                    else
                        changeValue -= 30;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (!isMove)
                    CamRotateObject.DOLocalRotate(new Vector3(CamRotateObject.transform.rotation.x,changeValue, CamRotateObject.transform.rotation.z), .7f);
            }
        }
        private void FixedUpdate()
        {
            CameraMovement.instance.CameraMove(transform.position);
        }
        private void OnMouseDown()
        {
            if(onGround)
            {
                print("b");
                isMove = true;
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                FirstPos = Camera.main.ScreenToViewportPoint(mousePos);
            }
        }
        private void OnMouseUp()
        {
            if(onGround)
            {
                print("a");
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane;
                LastPos = Camera.main.ScreenToViewportPoint(mousePos);
                rb.AddForce(-Camera.main.transform.forward * (Mathf.Clamp(LastPos.y * 2 - FirstPos.y * 2,-1,1) * power));
                rb.AddForce(-Camera.main.transform.right * (Mathf.Clamp(LastPos.x - FirstPos.x,-1,1) * power));
                print(-Camera.main.transform.forward * (Mathf.Clamp(LastPos.y * 2 - FirstPos.y * 2, -1, 1) * power));
                print(-Camera.main.transform.right * (Mathf.Clamp(LastPos.x - FirstPos.x, -1, 1) * power));
            }
           
           
        }
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.tag == "Platform")
                onGround = true;
            else
                onGround = false;

           
        }
        private void OnCollisionStay(Collision other)
        {
            if (other.transform.tag == "Platform")
                onGround = true;
        }
        private void OnCollisionExit(Collision other)
        {
            if (other.transform.tag == "Platform")
                onGround = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.tag=="Finish")
            {
                GameManager.Instance.RestartLevel();
            }
            if (other.transform.CompareTag("Dead"))
            {
                GameManager.Instance.RestartLevel();
            }
        }

    }
}

