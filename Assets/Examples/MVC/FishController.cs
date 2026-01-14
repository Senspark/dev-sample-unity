using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

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
            DoHeavyTask();
            
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

        private void DoHeavyTask()
        {
            new CpuIntensiveBenchmarks().SortLargeArray();
        }
        
        private class CpuIntensiveBenchmarks
        {
            private readonly int[] _largeArray = Enumerable.Range(1, 10_000).OrderBy(x => Guid.NewGuid()).ToArray();

            public void SortLargeArray()
            {
                Array.Sort((int[])_largeArray.Clone());
            }
        }

    }
}