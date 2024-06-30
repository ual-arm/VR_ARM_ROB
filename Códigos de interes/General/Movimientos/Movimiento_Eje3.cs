using UltimateXR.Core;
using UltimateXR.Core.Components;
using UnityEngine;
using UltimateXR.Animation.IK;

namespace UltimateXR.Devices.UxrControlMovimiento
{




    /// <summary>
    ///     Component to rotate an object around a chosen axis when specific buttons are pressed.
    /// </summary>
    public class Joistick_Eje3 : UxrComponent
    {
        #region Inspector Properties/Serialized Fields

        [SerializeField] private UxrControllerInput _controllerInput;
        [SerializeField] private UxrHandSide _hand;
        [SerializeField] private UxrInputButtons Giro_horario;
        [SerializeField] private UxrInputButtons Giro_Antihorario;
        [SerializeField] private GameObject _objectToRotate;
        [SerializeField] private float _rotationSpeed = 90f;
        [SerializeField] private RotationAxis _rotationAxis;

       // public UxrCcdIKSolver solverScript;

        public GameObject maximo;
        public GameObject minimo;
        public GameObject central;

        
        public enum RotationAxis
        {
            X,
            Y,
            Z
        }


        #endregion

        // Unique rotation counter
        private float _totalRotation = 0f;
       // public GameObject objeto;

        #region Unity

        /// <summary>
        ///     Updates the widget information.
        /// </summary>



        private void Update()
        {

            if (_controllerInput != null && _objectToRotate != null)
            {
                // Rotates the object clockwise when the clockwise button is pressed
                if (_controllerInput.GetButtonsPress(_hand, Giro_horario, true))
                {
                   // if (solverScript != null)
                   // {
                   //     solverScript.enabled = false;
                   // }

                    

                    switch (_rotationAxis)
                    {
                        case RotationAxis.X:
                          
                            break;
                        case RotationAxis.Y:


                          
                            //ebug.Log("Estamos en horario, girando en x" + aux);
                            //bug.Log("Estamos en:" + _objectToRotate.transform.eulerAngles.x);
                            //bug.Log("Estamos en:" + _objectToRotate.transform.eulerAngles.y);
                            //bug.Log("Estamos en objeto" +objeto.transform.eulerAngles.y);
                            //bug.Log("Estamos en objeto" + objeto.transform.eulerAngles.x);
                            // ((aux < (maximo + 5) && _objectToRotate.transform.eulerAngles.y > 91) || (_objectToRotate.transform.eulerAngles.y < 91 && aux < (85  )))
                            if (HayColision(central, minimo)==false)// _objectToRotate.transform.eulerAngles.x > 265)
                                                                          //Debug.Log("Estamos en:" + _objectToRotate.transform.eulerAngles.x);
                            {

                                SentidoHorario();
                            }
                            break;
                        case RotationAxis.Z:
                        
                            break;
                    }

                    //if (_totalRotation < maximo)
                    // if (_totalRotation < maximo)
                    //{
                    //   RotateObjectClockwise();
                    //}
                }

                // Rotates the object counter-clockwise when the counter-clockwise button is pressed
                if (_controllerInput.GetButtonsPress(_hand, Giro_Antihorario, true))
                {
                  //  if (solverScript != null)
                  //  {
                  //      solverScript.enabled = false;
                  //  }


                    switch (_rotationAxis)
                    {
                        case RotationAxis.X:
                           
                            break;
                        case RotationAxis.Y:
                            // aux = (_objectToRotate.transform.eulerAngles.x + 90);
                            // if (aux > 360)
                            // {
                            //     aux = aux - 360;
                            // }

                            
                            if (HayColision(central, maximo)==false)
                            {
                            
                                SentidoAntihorario();
                            }
                            break;
                        case RotationAxis.Z:
                         
                            break;
                    }

                    //if (_totalRotation > minimo)
                    //{
                    //    RotateObjectCounterClockwise();
                    //}
                }

                // Enable the solver script when no rotation buttons are pressed
                if (!_controllerInput.GetButtonsPress(_hand, Giro_horario, true) &&
                    !_controllerInput.GetButtonsPress(_hand, Giro_Antihorario, true))
                {
                  //  solverScript.enabled = true;
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Rotates the object clockwise around the chosen axis.
        /// </summary>
        private void SentidoHorario()
        {
            float rotationAmount = _rotationSpeed * Time.deltaTime;
            _totalRotation += Mathf.Abs(rotationAmount);
            _objectToRotate.transform.Rotate(GetRotationAxis(), rotationAmount);
        }

        /// <summary>
        ///     Rotates the object counter-clockwise around the chosen axis.
        /// </summary>
        private void SentidoAntihorario()
        {
            float rotationAmount = -_rotationSpeed * Time.deltaTime;
            //_totalRotation -= Mathf.Abs(rotationAmount);
            _totalRotation -= rotationAmount;
            _objectToRotate.transform.Rotate(GetRotationAxis(), rotationAmount);
        }

        /// <summary>
        ///     Returns the chosen rotation axis vector based on the selected RotationAxis enum value.
        /// </summary>
        private Vector3 GetRotationAxis()
        {
            switch (_rotationAxis)
            {
                case RotationAxis.X:
                    return Vector3.right;
                case RotationAxis.Y:
                    return Vector3.up;
                case RotationAxis.Z:
                    return Vector3.forward;
                default:
                    return Vector3.right; // Default to right axis
            }
        }

        #endregion

        private bool HayColision(GameObject objetoA, GameObject objetoB)
        {
            Collider colliderA = objetoA.GetComponent<Collider>();
            Collider colliderB = objetoB.GetComponent<Collider>();

            if (colliderA != null && colliderB != null)
            {
                Debug.Log("Hay colision");

                return colliderA.bounds.Intersects(colliderB.bounds);

            }

            return false;

        }
    }

  
}