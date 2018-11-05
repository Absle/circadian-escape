using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StdT12
{
    namespace Enums
    {
        public enum PickUpType
        {
            Battery, 
            KeyItem
        }
    }

    namespace Interfaces
    {
        public interface IInteractable
        {
            string InteractMessage { get; }
            void Interact();
        }

        public interface IPickUpable
        {
            string PickUpMessage { get; }
            Enums.PickUpType Type { get; }
        }

		public interface IHide
		{
			string HideMessage { get; }
			void Hide();
		}
    }
}
