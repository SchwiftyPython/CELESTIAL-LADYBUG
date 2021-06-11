using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Rewired;

namespace Dossamer.PanZoom
{
    public class PanZoomRewired : PanZoomBehavior
    {
        // mouse/touch controls still rely on default Unity Input class.

        public int playerId = 0; // Rewired playerId
        private Player player;

        [SerializeField]
        private string rewiredHorizontalAxis = "Horizontal";

        [SerializeField]
        private string rewiredVerticalAxis = "Vertical";

        protected override void Awake()
        {
            base.Awake();

            player = ReInput.players.GetPlayer(playerId);
        }

        protected override void CheckIsPanning()
        {

            // reset values
            var resetCopy = new SerializableDictionary<InputMethod, bool>();

            foreach (InputMethod method in isInputMethodActive.Keys)
            {
                resetCopy[method] = false;
            }

            isInputMethodActive = resetCopy;


            if (GetAxesOfType(InputMethod.HorizontalAxis).Count > 0 && player.GetAxis(rewiredHorizontalAxis) != 0f)
            {
                isInputMethodActive[InputMethod.HorizontalAxis] = true;
            }

            if (GetAxesOfType(InputMethod.VerticalAxis).Count > 0 && player.GetAxis(rewiredVerticalAxis) != 0f)
            {
                isInputMethodActive[InputMethod.VerticalAxis] = true;
            }

            if (GetAxesOfType(InputMethod.PointerHorizontal).Count > 0 && Input.GetMouseButton((int)mouseButton) && Input.touchCount <= 1)
            {
                isInputMethodActive[InputMethod.PointerHorizontal] = true;
            }

            if (GetAxesOfType(InputMethod.PointerVertical).Count > 0 && Input.GetMouseButton((int)mouseButton) && Input.touchCount <= 1)
            {
                isInputMethodActive[InputMethod.PointerVertical] = true;
            }

            if (GetAxesOfType(InputMethod.Scrollwheel).Count > 0 && Input.mouseScrollDelta.y != 0)
            {
                isInputMethodActive[InputMethod.Scrollwheel] = true;
            }

            List<AxisData> customAxes = GetAxesOfType(InputMethod.CustomAxis);
            if (customAxes.Count > 0)
            {
                foreach (AxisData axis in customAxes)
                {
                    if (axis.inputMethod == InputMethod.CustomAxis)
                    {
                        if (player.GetAxis(axis.customInputMethod) != 0)
                        {
                            isInputMethodActive[InputMethod.CustomAxis] = true;
                        }
                    }

                    if (axis.secondaryInputMethod == InputMethod.CustomAxis)
                    {
                        if (player.GetAxis(axis.customSecondaryInputMethod) != 0)
                        {
                            isInputMethodActive[InputMethod.CustomAxis] = true;
                        }
                    }
                }
            }

            if (GetAxesOfType(InputMethod.PinchZoom).Count > 0 && GetPinchZoomImmediateMagnitude() != 0)
            {
                isInputMethodActive[InputMethod.PinchZoom] = true;
            }

            bool wasPanning = IsPanning;

            IsPanning = false;

            foreach (bool status in isInputMethodActive.Values)
            {
                if (status)
                {
                    IsPanning = true;
                    break;
                }
            }

            if (IsPanning)
            {
                if (!wasPanning)
                {
                    OnPanStarted(EventArgs.Empty);
                }

                if (Input.GetMouseButtonDown((int)mouseButton))
                {
                    mousePanStartPosition = Input.mousePosition;
                    lastMouseDelta = null;
                }

                if (Input.touchCount >= 2)
                {
                    if (Input.GetTouch(1).phase == TouchPhase.Began)
                    {
                        lastPinchMagnitude = null;
                    }
                }
            }
            else
            {
                if (wasPanning)
                {
                    OnPanEnded(EventArgs.Empty);
                }
            }
        }

        List<AxisData> GetAxesOfType(InputMethod method)
        {
            List<AxisData> axesList = new List<AxisData>();

            foreach (AxisData axis in axes.Values)
            {
                if (axis.IsEnabled)
                {
                    bool secondaryFlag = false;

                    if (axis.IsSecondaryInputEnabled)
                    {
                        // order matters--button check needs to come last
                        if (!axis.DoesSecondaryNeedTrigger || player.GetButton(axis.secondaryInputTrigger))
                        {
                            if (axis.secondaryInputMethod == method)
                            {
                                secondaryFlag = true;
                            }
                        }
                    }

                    if (axis.inputMethod == method || secondaryFlag)
                    {
                        axesList.Add(axis);
                    }

                }
            }

            return axesList;
        }

        protected override void UpdatePosition()
        {
            if (IsPanning)
            {
                Vector3 translation = Vector3.zero;

                if (isInputMethodActive[InputMethod.HorizontalAxis] ||
                    isInputMethodActive[InputMethod.VerticalAxis] ||
                    isInputMethodActive[InputMethod.Scrollwheel] ||
                    isInputMethodActive[InputMethod.CustomAxis])
                {
                    List<AxisData> horizontalAxes = GetAxesOfType(InputMethod.HorizontalAxis);
                    List<AxisData> verticalAxes = GetAxesOfType(InputMethod.VerticalAxis);
                    List<AxisData> scrollwheelAxes = GetAxesOfType(InputMethod.Scrollwheel);
                    List<AxisData> customAxes = GetAxesOfType(InputMethod.CustomAxis);

                    Vector3 contribution = Vector3.zero;

                    foreach (AxisData axis in horizontalAxes)
                    {
                        float magnitude = player.GetAxis(rewiredHorizontalAxis) * inputMultipliers[axis.inputMethod];
                        AddAxisToContribution(axis, ref contribution, magnitude);
                    }

                    foreach (AxisData axis in verticalAxes)
                    {
                        float magnitude = player.GetAxis(rewiredVerticalAxis) * inputMultipliers[axis.inputMethod];
                        AddAxisToContribution(axis, ref contribution, magnitude);
                    }

                    foreach (AxisData axis in scrollwheelAxes)
                    {
                        float magnitude = Input.mouseScrollDelta.y * inputMultipliers[axis.inputMethod];
                        AddAxisToContribution(axis, ref contribution, magnitude);
                    }

                    if (isInputMethodActive[InputMethod.CustomAxis])
                    {
                        foreach (AxisData axis in customAxes)
                        {
                            string axisString = "";

                            if (!axis.IsSecondaryInputEnabled)
                            {
                                axisString = axis.customInputMethod;
                            }
                            else if (isInputMethodActive[axis.inputMethod] && !isInputMethodActive[axis.secondaryInputMethod])
                            {
                                axisString = axis.customInputMethod;
                            }
                            else if (!isInputMethodActive[axis.inputMethod] && isInputMethodActive[axis.secondaryInputMethod])
                            {
                                axisString = axis.customSecondaryInputMethod;
                            }
                            else if (isInputMethodActive[axis.inputMethod] && isInputMethodActive[axis.secondaryInputMethod])
                            {
                                // both are active, go with the primary unless secondary's trigger is firing
                                if (!axis.DoesSecondaryNeedTrigger || !player.GetButton(axis.secondaryInputTrigger))
                                {
                                    axisString = axis.customInputMethod;
                                }
                                else
                                {
                                    axisString = axis.customSecondaryInputMethod;
                                }
                            }

                            float magnitude = player.GetAxis(axisString) * inputMultipliers[axis.inputMethod];
                            AddAxisToContribution(axis, ref contribution, magnitude);
                        }
                    }

                    // further adjust contribution proportionally to how far from focus target, if target isn't null
                    if (focusTarget != null && isContributionProportionalToDistFromTarget)
                    {
                        if (!referenceCamera.orthographic)
                        {
                            contribution *= Mathf.Abs(GetForwardDotToFocusTarget());
                        }
                        else
                        {
                            contribution *= referenceCamera.orthographicSize;
                        }
                    }

                    translation += contribution * Time.deltaTime;
                }

                if (isInputMethodActive[InputMethod.PointerHorizontal] || isInputMethodActive[InputMethod.PointerVertical])
                {
                    Vector3 mouseDelta = Input.mousePosition - mousePanStartPosition;

                    if (lastMouseDelta == null)
                    {
                        lastMouseDelta = mouseDelta;
                    }

                    if (mouseDelta != lastMouseDelta && mouseDelta.magnitude > precision)
                    {
                        float clipDistance = focusTarget == null ? referenceCamera.nearClipPlane : Mathf.Abs(GetForwardDotToFocusTarget());

                        Vector3 point = referenceCamera.ScreenToWorldPoint(new Vector3(mouseDelta.x - ((Vector3)lastMouseDelta).x, mouseDelta.y - ((Vector3)lastMouseDelta).y, clipDistance));
                        Vector3 origin = referenceCamera.ScreenToWorldPoint(new Vector3(0, 0, clipDistance));

                        // so transform component is on a plane parallel to the viewport
                        // but want it to be parallel towards plane that intersects (1, 0, 0) and (0, 1, 0)
                        Vector3 transformDelta = Quaternion.Inverse(referenceCamera.transform.rotation) * (point - origin);

                        // Debug.DrawLine(transformDelta, Vector3.zero, Color.green, 10f);

                        // Debug.Log("mouse x: " + ((mouseDelta.x - ((Vector3)lastMouseDelta).x)));
                        // Debug.Log("mouse y: " + ((mouseDelta.y - ((Vector3)lastMouseDelta).y)));

                        lastMouseDelta = mouseDelta;

                        List<AxisData> horizontalAxes = GetAxesOfType(InputMethod.PointerHorizontal);
                        List<AxisData> verticalAxes = GetAxesOfType(InputMethod.PointerVertical);

                        Vector3 contribution = Vector3.zero;

                        foreach (AxisData axis in horizontalAxes)
                        {
                            float magnitude = transformDelta.x * inputMultipliers[axis.inputMethod];
                            AddAxisToContribution(axis, ref contribution, magnitude);
                        }

                        foreach (AxisData axis in verticalAxes)
                        {
                            float magnitude = transformDelta.y * inputMultipliers[axis.inputMethod];
                            AddAxisToContribution(axis, ref contribution, magnitude);
                        }

                        // Debug.Log("x: " + transformDelta.x);
                        // Debug.Log("y: " + transformDelta.y);


                        translation += -1 * contribution; // flip the mouse contribution
                    }
                }

                if (isInputMethodActive[InputMethod.PinchZoom])
                {
                    float pinchMagnitude = GetPinchZoomImmediateMagnitude();
                    // Debug.Log(pinchMagnitude + ", " + lastPinchMagnitude);

                    if (lastPinchMagnitude == null)
                    {
                        // Debug.Log("did a reset");
                        lastPinchMagnitude = pinchMagnitude;
                    }

                    if (pinchMagnitude != lastPinchMagnitude)
                    {
                        float deltaMagnitude = pinchMagnitude - (float)lastPinchMagnitude;

                        // Debug.Log(deltaMagnitude);

                        lastPinchMagnitude = pinchMagnitude;

                        List<AxisData> pinchZoomAxes = GetAxesOfType(InputMethod.PinchZoom);

                        Vector3 contribution = Vector3.zero;

                        foreach (AxisData axis in pinchZoomAxes)
                        {
                            float magnitude = deltaMagnitude * inputMultipliers[axis.inputMethod];

                            if (isContributionProportionalToDistFromTarget && referenceCamera.orthographic)
                            {
                                magnitude *= referenceCamera.orthographicSize;
                            }

                            AddAxisToContribution(axis, ref contribution, magnitude);
                        }

                        // further adjust contribution proportionally to how far from focus target, if target isn't null
                        if (focusTarget != null && isContributionProportionalToDistFromTarget)
                        {
                            if (!referenceCamera.orthographic)
                            {
                                contribution *= Mathf.Abs(GetForwardDotToFocusTarget());
                            }
                            else
                            {
                                contribution *= referenceCamera.orthographicSize;
                            }
                        }

                        translation += contribution; // * Time.deltaTime;
                    }
                }

                transform.position += translation;
            }
        }
    }
}