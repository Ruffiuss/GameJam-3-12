using System.Collections;
using UnityEngine;


namespace Gamekit2D
{
    internal sealed class DoorOpenClose : MonoBehaviour
    {
        public Animator ControlledAnnimator;
        public bool DoorClosed;
        public float AnimSpeed = 3.0f;
        public float TimeDoorOpened;

        private bool isOpenedByTimer = false;

        private void Awake()
        {
            ControlledAnnimator.GetComponent<Animator>();
        }

        public void ChangeDoorState()
        {
            if (DoorClosed)
            {
                ControlledAnnimator.Play("DoorOpening");
                ControlledAnnimator.speed = AnimSpeed;
            }
            else
            { 
                ControlledAnnimator.Play("DoorClosing");
                ControlledAnnimator.speed = AnimSpeed;
            }
            DoorClosed = !DoorClosed;
        }

        public void ChangeDoorStateByTimer()
        {
            if (!isOpenedByTimer)
            {
                ChangeDoorState();
                StartCoroutine(DoorClosingTimer());
                isOpenedByTimer = true;
            }
        }

        IEnumerator DoorClosingTimer()
        {
            yield return new WaitForSeconds(TimeDoorOpened);
            StopCoroutine("DoorClosingTimer");
            ChangeDoorState();
            isOpenedByTimer = false;
        }
    }
}
