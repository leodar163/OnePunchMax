using Inputs;
using UnityEngine;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Pawn _pawn;

        private void FixedUpdate()
        {
            _pawn.MoveToward(InputsUtility.MainControls.Movements.Move.ReadValue<Vector2>());
        }
    }
}