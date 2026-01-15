using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Examples.MVC
{
    public class FishGameController
    {
        private readonly List<FishController> _controllers = new();
        private readonly List<FishView> _views = new();

        public void Add((FishController, FishView) fish)
        {
            _controllers.Add(fish.Item1);
            _views.Add(fish.Item2);
        }

        public void Update(float deltaTime)
        {
            foreach (var controller in _controllers)
            {
                controller.Update(deltaTime);
            }
            
            // Hidden: code này chỉ để demo nhanh - thực tế nên có phương án dài hạn hơn
            /*UniTask.RunOnThreadPool(() =>
            {
                foreach (var controller in _controllers)
                {
                    controller.Update(deltaTime);
                }
            });*/

            foreach (var view in _views)
            {
                view.Render();
            }
        }

        public void MoveTo(Vector2 targetPosition)
        {
            foreach (var controller in _controllers)
            {
                controller.MoveTo(targetPosition);
            }
        }
    }
}