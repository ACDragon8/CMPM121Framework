using System.Collections;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
   public enum ItemType
   {
      Invincibility,
      ManaRefill,
      SpeedBoost
   }

   public ItemType itemType;

   private void OnTriggerEnter2D(Collider2D other)
   {
      var player = other.GetComponent<PlayerController>();
      if (player != null)
      {
         Debug.Log($"Player collected {itemType}");

         // Apply effect to player 
         switch (itemType)
         {
            case ItemType.Invincibility:
               CoroutineManager.Instance.Run(ApplyInvincibility(player));
               break;

            case ItemType.ManaRefill:
               player.spellcaster.RefillMana(); // Assuming you have this; else we’ll add it
               break;

            case ItemType.SpeedBoost:
               CoroutineManager.Instance.Run(ApplySpeedBoost(player));
               break;
         }

         Destroy(gameObject);
      }
   }
   IEnumerator ApplyInvincibility(PlayerController player)
   {
      player.hp.team = Hittable.Team.PLAYER; // Keep team the same, just prevent damage
      var originalHp = player.hp;
      var tempHp = new Hittable(9999, Hittable.Team.PLAYER, player.gameObject);
      player.hp = tempHp;

      Debug.Log("Invincibility ON");
      yield return new WaitForSeconds(5f); // 5 seconds of invincibility

      player.hp = originalHp;
      Debug.Log("Invincibility OFF");
   }

   IEnumerator ApplySpeedBoost(PlayerController player)
   {
      player.modifySpeed("boost", 10); // +10 speed
      player.getSpeed();
      Debug.Log("Speed Boost ON");
      yield return new WaitForSeconds(5f); // lasts 5 seconds
      player.modifySpeed("boost", 0);
      player.getSpeed();
      Debug.Log("Speed Boost OFF");
   }
   
}
