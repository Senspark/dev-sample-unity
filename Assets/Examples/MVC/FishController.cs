using UnityEngine;

namespace Examples.MVC
{
    public class FishController
    {
        private readonly FishModel _model;

        public FishController(FishModel model)
        {
            _model = model;
        }

        public void MoveTo(Vector2 targetPosition)
        {
            _model.TargetPosition = targetPosition;
        }

        public void Update(float deltaTime)
        {
            var direction = _model.TargetPosition - _model.Position;
            var distance = direction.magnitude;

            if (distance < 0.001f)
                return;

            var moveDistance = _model.Speed * deltaTime;

            if (moveDistance >= distance)
            {
                _model.Position = _model.TargetPosition;
            }
            else
            {
                _model.Position += moveDistance / distance * direction;
            }
        }
    }
}