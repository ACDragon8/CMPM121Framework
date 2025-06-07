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

         //Apply effect to player here

         Destroy(gameObject);
      }
   }
}
