using System;
using UnityEngine;

namespace Examples.MVC
{
    /// Ví dụ game cá bơi - MVC Pattern Demo
    public class MvcScene : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private GameObject fishPrefab;
        [SerializeField] private float speed = 5f;
        [SerializeField] private int fishCount = 500;

        private FishGameController _gameController;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            _gameController = new FishGameController();
            for (var i = 0; i < fishCount; i++)
            {
                _gameController.Add(SpawnFish());
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                _gameController.MoveTo(new Vector2(mousePos.x, mousePos.y));
            }

            _gameController.Update(Time.deltaTime);
        }

        private (FishController, FishView) SpawnFish()
        {
            var fishObj = Instantiate(fishPrefab);
            var fishTransform = fishObj.transform;
            var model = new FishModel
            {
                Position = fishTransform.position,
                Speed = speed
            };
            var controller = new FishController(model);
            var view = new FishView(model, fishTransform);
            return (controller, view);
        }
    }

    
}