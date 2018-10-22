using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StdT12
{
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
            void PickUp();
        }

		interface IHide
		{
			string HideMessage { get; }
			void Hide();
		}
    }
}
