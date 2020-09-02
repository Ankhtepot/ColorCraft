using UnityEngine;

//Fireball Games * * * PetrZavodny.com

namespace UI
{
    public class CompassController : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private GameObject sourceOfAngle;
        [SerializeField] private GameObject rotationPivot;
#pragma warning restore 649

        void FixedUpdate()
        {
            RotatePivot();
        }

        private void RotatePivot()
        {
            Vector3 eulerRotation = new Vector3(0,0, -sourceOfAngle.transform.eulerAngles.y);
            rotationPivot.transform.rotation = Quaternion.Euler(eulerRotation);
        }
    }
}
