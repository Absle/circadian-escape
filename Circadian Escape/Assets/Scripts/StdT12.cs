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

        public enum BeastAIState
        {
            Tutorial,
            Wander,
            Approach,
            Pursue,
            Search
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

    /*
    public static class StdT12Common
    {
        public static PlayerController Player { get; private set; }
        public static Camera PhoneCamera { get; private set; }
        public static BeastController Beast { get; private set; }
        public static GameObject[] RoomGraph { get; private set; }

        static StdT12Common()
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            Beast = GameObject.FindGameObjectWithTag("Beast").GetComponent<BeastController>();
            PhoneCamera = GameObject.FindGameObjectWithTag("Phone").GetComponent<Camera>();

            GameObject[] nodes = GameObject.FindGameObjectsWithTag("Node");
            RoomGraph = new GameObject[nodes.Length];

            //sort RoomGraph by name (Node1 to Node18 as of 12 Nov. 2018)
            foreach(GameObject node in nodes)
            {
                string stringdex = node.name.Replace("Node", "");
                int index;
                try
                {
                    index = int.Parse(stringdex);
                }
                catch(System.Exception)
                {
                    throw;
                }
                RoomGraph[index-1] = node;
            }
        }
    }
    */
}
