using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class InvadeManager : Singleton<InvadeManager>
{
    public Queue<SpawnOrWait> actQueue = new Queue<SpawnOrWait>(); // 몹 편성 눌러서 여기에 추가.

}
