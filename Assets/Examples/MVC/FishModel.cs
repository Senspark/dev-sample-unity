using UnityEngine;

namespace Examples.MVC
{
    /// View giữ interface - để đảm bảo Read-only
    public interface IFishModel
    {
        Vector2 Position { get; }
        Vector2 TargetPosition { get; }
        float Speed { get; }
    }

    /// Controller giữ concrete class - có quyền modify
    public class FishModel : IFishModel
    {
        public Vector2 Position { get; set; }
        public Vector2 TargetPosition { get; set; }
        public float Speed { get; set; }
    }
}