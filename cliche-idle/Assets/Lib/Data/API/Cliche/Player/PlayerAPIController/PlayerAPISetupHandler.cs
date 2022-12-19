using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cliche.Idle;

namespace Cliche.Idle
{
    public class PlayerAPISetupHandler : MonoBehaviour
    {
        private void Start()
        {
            Player.Init();
        }
    }
}
