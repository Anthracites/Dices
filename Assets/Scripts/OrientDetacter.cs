using UnityEngine;
using Doozy.Engine;

namespace Dices.UserInterface
{
    public class OrientDetacter : MonoBehaviour // Class for send message about rotation of device
    {

        void OnRectTransformDimensionsChange()
        {
            GameEventMessage.SendEvent(EventsLibrary.OrientationChanged);
        }
    }
}
