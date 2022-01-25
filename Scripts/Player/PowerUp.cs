using UnityEngine;

// these should be indavidual classes that inheret from a base class but oh well.
// When the player picks up a powerup, the player's associated stats are incrimented by the powerup's stats.
public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float speedUpgrade;
    [SerializeField] float jumpUpgrade;
    [SerializeField] int dayDamageUpgrade;
    [SerializeField] int nightDamageUpgrade;
    [SerializeField] float gravityUpgrade;
    [SerializeField] int dayDoubleJumpUpgrade;
    [SerializeField] int nightDoubleJumpUpgrade;
    [SerializeField] float blockChanceUpgrade;
    [SerializeField] float cookChanceUpgrade;
    [SerializeField] GameObject killEffectUpgrade; // unused powerup we didn't have time to implement
    [SerializeField] GameObject missEffectUpgrade; // ""
    [SerializeField] int healthUpgrade;
    [SerializeField] float nightExtensionUpgrade;
    [SerializeField] float dayExtensionUpgrade;

    [SerializeField] int healAmount;

    public string itemTitle; // only ones other scripts need
    public string itemDesc;

    [SerializeField] GameObject takeEffect; // poofs

    // increment stats
    public void TakePowerUp(PlayerMovement player)
    {
        player.speed += speedUpgrade;
        player.jumpForce += jumpUpgrade;
        player.dayDamageUpgrade += dayDamageUpgrade;
        player.nightDamageUpgrade += nightDamageUpgrade;
        player.rb2d.gravityScale -= gravityUpgrade;
        player.dayDoubleJumps += dayDoubleJumpUpgrade;
        player.nightDoubleJumps += nightDoubleJumpUpgrade;
        player.hp.blockChance += blockChanceUpgrade;
        player.cookChance += cookChanceUpgrade;
        player.hp.maxHp += healthUpgrade;
        player.dayNight.daySpeed -= dayExtensionUpgrade;
        player.dayNight.nightSpeed -= nightExtensionUpgrade;

        if (killEffectUpgrade)
            player.killEffect.Add(killEffectUpgrade);

        if (missEffectUpgrade)
            player.missEffect.Add(missEffectUpgrade);
		
        // foods
		if (healAmount > 0)
			player.hp.Heal(healAmount);

        Instantiate(takeEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
