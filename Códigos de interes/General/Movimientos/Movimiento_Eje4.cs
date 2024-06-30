using UltimateXR.Core;
using UltimateXR.Core.Components;
using UnityEngine;
using UltimateXR.Animation.IK;

namespace UltimateXR.Devices.UxrControlMovimiento
{




    /// <summary>
    ///     Component to rotate an object around a chosen axis when specific buttons are pressed.
    /// </summary>
    public class Joistick_EJe4 : UxrComponent
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

        public float maximo;
        public float minimo;
        public enum RotationAxis
        {
            X,
            Y,
            Z
        }


        #endregion

        // Unique rotation counter
        private float _totalRotation = 0f;

        #region Unity

        /// <summary>
        ///     Updates the widget information.
        /// </summary>



        private void Update()
        {
            Debug.Log("Estamos en horario  " +_totalRotation);
            
            if (_controllerInput != null && _objectToRotate != null)
            {
                // Rotates the object clockwise when the clockwise button is pressed
                if (_controllerInput.GetButtonsPress(_hand, Giro_horario, true))
                {
                  //  if (solverScript != null)
                  //  {
                  //      solverScript.enabled = false;
                  //  }

                    

                    switch (_rotationAxis)
                    {
                        case RotationAxis.X:
                            //if (_objectToRotate.transform.eulerAngles.z < maximo || _objectToRotate.transform.eulerAngles.z > (minimo - 1))
                            if (_totalRotation <maximo)
                            {
                                SentidoHorario();
                            }
                            break;
                        case RotationAxis.Y:
                            if (_objectToRotate.transform.eulerAngles.y < maximo)
                            {
                                Debug.Log("Estamos en antihorario, girando en x" + _objectToRotate.transform.eulerAngles.x);
                                Debug.Log("Estamos en antihorario, girando en y" + _objectToRotate.transform.eulerAngles.y);

                                SentidoHorario();
                            }
                            break;
                        case RotationAxis.Z:
                            if (_objectToRotate.transform.eulerAngles.y < maximo || _objectToRotate.transform.eulerAngles.y > (minimo - 1))
                            {
                                SentidoHorario();
                            }
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
                            //if (_objectToRotate.transform.eulerAngles.z > minimo || _objectToRotate.transform.eulerAngles.z < (maximo + 2))
                            if(_totalRotation > -minimo)
                            {
                                SentidoAntihorario();
                            }
                            break;
                        case RotationAxis.Y:
                            if (_objectToRotate.transform.eulerAngles.x > minimo || _objectToRotate.transform.eulerAngles.x < (maximo + 2))
                            {
                                Debug.Log("Estamos en antihorario, girando en x" + _objectToRotate.transform.eulerAngles.x);
                                Debug.Log("Estamos en antihorario, girando en y" + _objectToRotate.transform.eulerAngles.y);

                                SentidoAntihorario();
                            }
                            break;
                        case RotationAxis.Z:
                            if (_objectToRotate.transform.eulerAngles.y > minimo || _objectToRotate.transform.eulerAngles.y < (maximo + 2))
                            {
                                SentidoAntihorario();
                            }
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
                   // solverScript.enabled = true;
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

            Debug.Log("Estamos en antihorario  " + rotationAmount);
        }

        /// <summary>
        ///     Rotates the object counter-clockwise around the chosen axis.
        /// </summary>
        private void SentidoAntihorario()
        {
            float rotationAmount = -_rotationSpeed * Time.deltaTime;
            _totalRotation -= Mathf.Abs(rotationAmount);
            //_totalRotation -= rotationAmount;
            _objectToRotate.transform.Rotate(GetRotationAxis(), rotationAmount);

            Debug.Log("Estamos en antihorario  " + rotationAmount);

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
    }
}