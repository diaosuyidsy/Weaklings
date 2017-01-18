---
title: 'Game Design Document – Weakling'
---

Section 1: Game Mechanics
=========================

-    §1.1 Core Game Play
    --------------------

    -   ### Game Overview

> Weakling is a 2-D platformer stealth-action-process game. It’s overall
> design concept of game play is **HEAVY OPERATION** orientated.

-   ### Overview Expanded

> Stealth action will be explained in the section game flow. Heavy
> operation orientated means game experience depended heavily on player
> operation. Thus both the upper limit and lower limit of the game will
> be high, though upper limit will be higher than the lower limit.

-   §1.2 Game Flow
    --------------

    -   ### Overall Game Flow 

> Player kills enemy -&gt; Gains experience -&gt; upgrade to get new
> abilities -&gt; combine different abilities to set up new play
> strategy (similar to leveling with Diablo III) -&gt; use the different
> strategy to kill enemy
>

-   ###  Core Game Play Flow

> hit one enemy until x% health -&gt; process the enemy (or just kill it
> to loop from beginning)-&gt; use this processed enemy to strike
> another enemy until x% health -&gt; loop
>

-   §1.3 Characters
    ---------------

    -   ### Player

> At first, player has the **basic attack** abilities to start off. With
> killing more enemies and finishing levels, player levels up and gains
> more abilities.
>
> But player can **only use certain amount (let’s say, 3) of abilities
> at one time**. Thus with leveling up, you can only choose 3 abilities
> to use finishing this level, even though player might now have 10
> abilities to use.
>
> For example, player might have mid-air assassinate ability, double
> jump, anti-bullet, master stealth ability upon reaching level 3, but
> player can only choose three abilities to finish next level. Upon
> death / lose, player can change his/her abilities. Upon leveling up,
> player may or may not get extra sockets for abilities. This has to be
> decided later.
>
> When player process an enemy, player’s basic attack ability is
> replaced by enemy’s basic attack ability and player may/may not gain
> an extra ability (depending on whether enemy has one).
>
> Controllable buttons are “q”, “w”, “e”, “r” for player abilities,
> mouse left for primary attack, mouse right for secondary attack.

-   ### Enemy

> Enemies can have multiple abilities. Upon reaching x% health (which
> from now on will be called critical health), enemy becomes able to
> process by player. When processed, player replace his/her basic attack
> with enemies’ basic attack abilities.
>
> Enemies have different basic attack style. For example, some enemies
> can use rifle and some can use machine gun.
>
> Each enemy has different critical health, but the behavior when
> reaching below critical health is similar, slower movements, including
> moving, turning around and usage of abilities and attacks.

-   §1.4 Game Play Elements
    -----------------------

    -   ### Player Primary Attack

> In the beginning of the game, player only has one kind of primary
> attack, melee.
>
> When player process enemy, player temporarily replace his/her current
> primary attack ability and secondary attack ability with the enemy’s
> primary attack ability and secondary attack ability.

-   ### Player Secondary Attack

> In the beginning of the game, player has no secondary attack. Upon
> leveling up, player will learn secondary attack such as concentrate
> melee.
>
> When player process enemy, player will temporarily replace his/her
> current secondary attack with the enemy’s secondary attack. (If
> applicable) If enemy does not have a secondary, then player’s
> secondary ability will be temporarily unusable until player comes out
> of the process.

-   ### Procession: Main Ability

> One of the core game play ability that player will have. This is **not
> removable** at any circumstances and this ability will remain active
> the whole time.
>
> This ability goes as follows: After casting for x sec, player can
> process the enemy which reaches critical health. Player gets enemy’s
> primary attack and secondary attack. May/may not get the enemy’s extra
> abilities.
>
> Player’s current health became enemy’s current health and player
> continuously deals damage to the processed enemy.
>
> When enemy that is processed by player’s health reaches zero, player
> pop out of the enemy, reclaiming everything (including player health,
> except for cooldowns of player ability) to the state right before
> player process the enemy.

-   ### Enemy

> Different enemy has different abilities and status.
>
> For status, different kind of enemy have different:

-   Max health

-   Movement Speed

-   Turning Speed

-   Searching Speed (Speed of the increment of search bar)

-   Losing Speed (Speed of the decrement of search bar)

-   Different kind of armor (TBC)

> For abilities, different kind of enemy have different:

-   Primary Attack

-   Secondary Attack

-   Extra abilities. (TBC)

> When processed by player, all of them will be inherited by player,
> except searching, losing speed and part (or all) of the extra
> abilities.

-   ### Extra Ability

> When player level up, he/she can choose extra ability. Player can
> equip in total x extra abilities. Overall, every extra ability should
> be **GAME-STRATEGY-CHANGING** boost in essence, not some talent
> “increase damage 5%”, but something like “gain double jump ability”.
>
> **In** **the beginning**, player can only equip 1 extra abilities.
> With passing certain scene, player can equip 2, then player can equip
> 3.
>
> Think of extra ability as Diablo’s ability (1, 2, 3, 4 and left right
> click). With different combination, player can have so **much more
> different strategies**. This is what our extra ability shoots for.
>
> AND one important note, extra abilities with different strategies can
> and will break the main game play loop (kill -&gt; process -&gt;
> kill). BUT NOT UNTIL middle of the game play where player got used to
> the main game loop and started to think about different loop.

-   §1.5 Game Physics and Statistics
    --------------------------------

> Skip since it’s already built and strictly follow normal Physics.

-   §1.6 Artificial Intelligence
    ----------------------------

    -   ### Field of View

> Every enemy (AI) has its own FOV, different kind of enemy can have
> different FOV.
>
> FOV of enemy works as follows:
>
> When player enters the view range, enemy will have a bar indicating
> he/she has noticed something strange. If player continues to stay in
> the view range, the bar would begin to fill up in a certain speed,
> specified by the enemy’s Searching Speed (see 1.4). If player does not
> stay inside the view range, bar starts to decrease in a certain speed,
> specified by the enemy’s Losing Speed (see 1.4). If the bar fills up
> for the first time, then the bar turns yellow, indicating the enemy
> confirms noticing something strange in player’s location. Then bar
> begins to fill up again to turn red with twice the speed. If player
> stays out of FOV now, the enemy will start following the path finding
> algorithm to go to the position where player was standing when the bar
> of enemy turns yellow. Upon bar reaching red, enemy starts to attack.
>
> ![](media/image3.png){width="4.319444444444445in"
> height="8.041666666666666in"}

-   ### Attack

> When Enemy begins attack, enemy gains a wider field of view. If player
> gets out of this wider field of view, then enemy would chase player to
> the last seen position. After x sec of not seeing player, enemy
> returns to normal state. Or enemy and player fight to death. Normally
> player wins.

-   ### Facing

> Before enemy bar turns yellow, enemy does not change his/her facing
> position. Upon changing to yellow, enemy changes his/her facing
> position to player last seen position. Upon changing to red, enemy
> facing follows player.

-   ### Player Too Close and Group Fight

> If player gets too close, within x meters, enemy immediately gains red
> bar and start attacking player.
>
> If enemy’s ally changes into attacking mode (aka bar turns red), then
> any other enemy within his/her FOV that can spot this ally immediately
> changes to attacking mode and start to attack player.

-   ### Good Ally Bad Ally

> If player gains control of an enemy, then player would not trigger
> notice when walk into other enemies’ FOV. But player will be
> immediately attacked if he/she start attacking first.

-   ### Path Finding

> FOV of most enemy cannot penetrate through any obstacles (except some
> red laser folks).
>
> And if player last seen location is somewhere unreachable (such as
> midair), then find the nearest reachable position to be the last seen
> location.

-   §1.7 Storyline
    --------------

    -   ### Synopsis

> This is a human and Vic opposing enemy’s era. Vic and humans have
> similar Astronautically technology and military power. The only
> difference between the two is very weak Vic people, not against the
> human front, which makes the people at the time Vic against humanity
> often take occupy the human mind and the human confrontation with
> human warfare.
>
> The story takes place on a spaceship, hero Blanca, Vic scout looks
> like in a cot bed to wake up. He goes out, but found the ship are all
> human beings. He was very scared and angry. Because in this war Vick
> has countless human spacecraft to be captured, Vick above all been
> slaughtered, and direct human-occupied spacecraft captives, never
> unflinchingly.
>
> Blanca picture that emerges out of his way and off the investigation
> spaceship commander arranged for his reconnaissance missions. So he
> was glad that they are missing the human at the same time, they want
> to kill all humans and recapture the spacecraft to complete the task.
>
> Blanca kill humans at the same time, step by step, like a bridge near.
> He gradually closes the bridge at the same time in my mind a picture:
> he was cruising in the universe, an asteroid coming his way, he tried
> to escape the asteroid, but the rocker and nothing happens.
>
> He did not care about these pictures, but continued with his plan to
> retake the ship.
>
> He further desperate in the bridge when the mind has emerged a
> picture: he pop his spacecraft, the spacecraft was smash asteroids.
>
> He felt very strange, but still did not manage.
>
> As time goes on, step by step, to mind a third screen: his head after
> flying to pop up the spacecraft.
>
> When Blanca finally kill all the human soldiers entered the bridge,
> the captain should fast (Final Boss), killed when Blanca mind, recall
> everything. To save humanity in the universe is a floating Blanca, and
> he made the emergency measures, which is why a start in an emergency
> Blanca bed to wake up.
>
> In this case, Blanca heart filled with remorse, but if you do not kill
> the human beings, they still have to face a dead end. (The player
> chooses to kill the captain / his death, the captain put a way out)
>
> 1\. Blanca chose to kill the captain, doing spacecraft back.
>
> 2\. Blanca chose not to kill the captain, was killed the captain.

Section 2: User Interface
=========================
