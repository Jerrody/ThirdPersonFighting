# ThirdPersonFighting

## File Structure

Every script, animation, and prefab is placed logically where it should be. In my project aren't global spaces like Scripts where you can see any script from the game. In my case, this is Player (folder) -> PlayerPrefab, PlayerController, PlayerCameraController, Animations (folder) -> PlayerAnimationController and it animations.

And this logic is in the project is everywhere.

## Code Style

```cs
namespace CoolNameSpace {
    public delegate void MyCoolNotify(); // All delegates are outside of class scope.
    
    public sealed class MyCoolClass : MonoBehaviour {
        public static event MyCoolNotify MyCoolStaticEvent; // First goes static C# events.
        public event MyCoolNotify MyCoolEvent;  // After that goes non-static C# events.
        
        [field: NonSerialized] public UnityEvent CoolUnityEvent; // After the all C# events I write all `UnityEvent`s.
        
        [Header("References")]
        [SerializeField] private GameObject coolGameObject;
        
        [Header("Stats")]
        [SerializeField] private int healAmount = 40;
        
        private const float MyConstant = -20.0f;
        
        private static bool MyStatic;
        
        public Vector3 MyField { get; private set; }

        private float _anotherField;
        private uint _andAnotherField;
        
        public void OnValidate() {    // Firt goes Unity Methods.
            if (_anotherField < 0.0f)
                _anotherField = 0.0f;
        }
        
        public void Awake() {
            print("HELLO!");
        }
        
        public override void MyMemberMethod() { // After that goes methods of the parent class.
            base.MyMemberMethod(); 
        }
        
        public void InternalMethodOfClass() { // After that goes methods of child class.
            _anotherField += 2;
        }
    }
}
```
