
using UnityEngine;

namespace Karianakis.Utilities
{

    public class LeanInvo2DBuilder : InvoBase
    {
        Transform _transform;
        float _startTime;
        float _duration;
        Vector2 _startPosition;
        Vector2 _targetPosition;
        float _startRotation;
        float _targetRotation;
        bool _moving;
        bool _rotating;
        bool _local;


        internal LeanInvo2DBuilder(Transform trans , bool local) : base(0, _infiniteRepeats, null) { _transform = trans; _local = local; }


        internal override void InvokeMeBeforeProcessing(InvoBase _me)
        {
            float t = (MyTime.now - _startTime) / _duration;

            bool end = false;
            if (t >= 1)
            {
                end = true;
                t = 1;
            }

            if (_moving)
            {
                Vector2 position =
                    Vector2.Lerp(_startPosition, _targetPosition, t);

                if (_local)
                {
                    _transform.localPosition = position;
                }
                else
                {
                    _transform.position = position;
                }

            }
            if (_rotating)
            {
                float rotation = Mathf.Lerp(_startRotation, _targetRotation, t);

                if (_local)
                {
                    _transform.localRotation = Quaternion.Euler(0, 0, rotation);
                }
                else
                {
                    _transform.rotation = Quaternion.Euler(0, 0, rotation);
                }
            }


            if (end)
            {
                End();
            }
        }

        public LeanInvo2DBuilder SetPosition(Vector2 position)
        {
            SetMovingTrue();
            _targetPosition = position;
            return this;
        }
        public LeanInvo2DBuilder AddPosition(Vector2 add)
        {
            SetMovingTrue();
            _targetPosition = _startPosition + add;

            return this;
        }


        public LeanInvo2DBuilder SetDuration(float duration)
        {
            _duration = duration;
            return this;
        }

        public LeanInvo2DBuilder SetRotation(float rotation)
        {
            SetRotatingTrue();
            _targetRotation = rotation;
            return this;
        }





        void SetMovingTrue()
        {
            if (_moving)
            {
                return;
            }

            _moving = true;
            _startPosition = _local ? _transform.localPosition : _transform.position;
        }
        void SetRotatingTrue()
        {
            if (_rotating)
            {
                return;
            }

            _rotating = true;
            _startRotation = _local ? _transform.localRotation.eulerAngles.z : _transform.rotation.eulerAngles.z;
        }

    }

}