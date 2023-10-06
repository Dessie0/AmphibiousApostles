using Player;
using UnityEngine;

namespace Interactables
{
    public abstract class Interactable : MonoBehaviour
    {

        public abstract void OnInteract(Frog frog);


    }
}