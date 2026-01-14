using UnityEngine;

namespace Examples.MVC
{
    /// Ví dụ game cá bơi - MVC Pattern Demo
    public class MvcScene : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private GameObject fishPrefab;
        [SerializeField] private float speed = 5f;

        private FishGameController _gameController;

        private void Start()
        {
            _gameController = new FishGameController();
            _gameController.Add(SpawnFish());
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