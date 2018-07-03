using System;
using Unity.Mathematics;
using UnityEngine;

public class InputController: MonoBehaviour
{
    private InputControllerRepeater _hori = new InputControllerRepeater("Horizontal");
    private InputControllerRepeater _vert = new InputControllerRepeater("Vertical");

    public string[] _buttons = { "Fire1", "Fire2", "Fire3" };

    public static event EventHandler<DataEventArgs<float2>> MoveEvent;
    public static event EventHandler<DataEventArgs<int>> FireEvent;

    private void Update()
    {
        var input = new float2(_hori.Update(), _vert.Update());

        if (!Equals(input.x, 0f) || (!Equals(input.y, 0f)))
            MoveEvent?.Invoke(this, new DataEventArgs<float2>(input));

        for (var i = 0; i < _buttons.Length; i++)
        {
            if (Input.GetButtonUp(_buttons[i]))
                FireEvent?.Invoke(this, new DataEventArgs<int>(i + 1));
        }
    }

    private class InputControllerRepeater
    {
        private const float Threshold = 0.5f;
        private const float Rate = 0.25f;

        private float _next;
        private bool _hold;
        private string _axis;

        public InputControllerRepeater(string axis)
        {
            _axis = axis;
        }

        public int Update()
        {
            var retValue = 0;
            var value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));

            if (value != 0)
            {
                if (Time.time > _next)
                {
                    retValue = value;
                    _next = Time.time + (_hold ? Rate : Threshold);
                    _hold = true;
                }
            }
            else
            {
                _hold = false;
                _next = 0;
            }

            return retValue;
        }
    }
}
