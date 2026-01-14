using UnityEngine;

namespace Examples.MVC
{
    /// Có thể là MonoBehaviour
    public class FishView
    {
        private readonly IFishModel _model;
        private readonly Transform _transform;

        public FishView(IFishModel model, Transform transform)
        {
            _model = model;
            _transform = transform;
        }

        public void Render()
        {
            _transform.position = _model.Position;
        }
    }
}