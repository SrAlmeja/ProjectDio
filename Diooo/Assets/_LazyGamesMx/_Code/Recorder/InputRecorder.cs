/* using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputRecorder : MonoBehaviour
{
    private List<InputAction> _inputActions = new List<InputAction>();
    private List<float> _timeStamps = new List<float>();
    private List<Vector3> _positions = new List<Vector3>();
    private List<Quaternion> _rotations = new List<Quaternion>();

    public ActionMap inputActionMap;

    void Start()
    {
        // Subscribe to all button press and joystick events
        foreach (InputControl control in inputActionMap.controls)
        {
            if (control is ButtonControl)
            {
                ButtonControl buttonControl = (ButtonControl)control;

                buttonControl.started += ctx => RecordInput(buttonControl, ctx.startTime);

            }
            else if (control is AxisControl axisControl)
            {
                axisControl.started += ctx => RecordInput(axisControl, ctx.startTime);
                axisControl.performed += ctx => RecordInput(axisControl, ctx.time);
                axisControl.canceled += ctx => RecordInput(axisControl, ctx.time);
            }
        }
    }
}

    private void RecordInput(InputControl control, float timestamp)
    {
        // Add the input action and timestamp to the lists
        _inputActions.Add(control.action);
        _timeStamps.Add(timestamp);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Stop recording input
            foreach (var device in InputSystem.devices)
            {
                foreach (var control in device.allControls)
                {
                    if (control is ButtonControl buttonControl)
                    {
                        buttonControl.started -= ctx => RecordInput(buttonControl, ctx.startTime);
                    }
                    else if (control is AxisControl axisControl)
                    {
                        axisControl.started -= ctx => RecordInput(axisControl, ctx.startTime);
                        axisControl.performed -= ctx => RecordInput(axisControl, ctx.time);
                        axisControl.canceled -= ctx => RecordInput(axisControl, ctx.time);
                    }
                }
            }

            // Save the recorded input data to a file
            using (var writer = new BinaryWriter(File.Open("input.dat", FileMode.Create)))
            {
                writer.Write(_inputActions.Count);
                for (int i = 0; i < _inputActions.Count; i++)
                {
                    writer.Write(_inputActions[i].name);
                    writer.Write(_timeStamps[i]);
                    writer.Write(_positions[i].x);
                    writer.Write(_positions[i].y);
                    writer.Write(_positions[i].z);
                    writer.Write(_rotations[i].x);
                    writer.Write(_rotations[i].y);
                    writer.Write(_rotations[i].z);
                }
            }
        }
    }


}
*/
