using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StdT12
{
    namespace Enums
    {
        public enum PickUpType
        {
            Battery
        }
    }

    namespace Interfaces
    {
        interface IInteractable
        {
            string InteractMessage { get; }
            void Interact();
        }

        interface IPickUpable
        {
            string PickUpMessage { get; }
            Enums.PickUpType Type { get; }
        }
    }
}
